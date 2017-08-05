'use strict';
const fs = require('fs');
const path = require('path');

let fileName = null;
let content = null;
let name = null;
let schema = null;

Object.resolve = (path, obj) => path.replace('#/', '').split('/').reduce((prev, curr) => prev ? prev[curr] : undefined, obj || null);

const format = {
    undef: (name) => name.replace('#/definitions/', ''),
    class: (name, type) => {
        let gname = format.highCase(format.undef(name));
        return `public class ${gname}` + (type ? ` : List<${gname}.${type}>` : '');
    },
    highCase: (value) => value.charAt(0).toUpperCase() + value.slice(1),
    enum: (name, value) => {
        name = name.charAt(0).toUpperCase() + name.slice(1);
        const jsonProp = '[JsonConverter(typeof(StringEnumConverter))]\n';
        return jsonProp + `public enum ${format.highCase(name)}Type\n` + format.open() + '\n' +
            (value.enum ? value.enum.join(',\n') : '') +
            (value.items && value.items.enum ? value.items.enum.join(',\n') : '') + '\n' + format.close() + '\n';
    },
    removeExtension: (typeName) => {
        if (typeName.indexOf('.json') == -1) {
            return typeName;
        }
        typeName = typeName.replace('.json', '').split('.');
        for (let i = 0; i < typeName.length; ++i) {
            typeName[i] = format.highCase(typeName[i]);
        }
        return typeName.join('');
    },
    property: (propname, type, isArray, def) => {
        if (propname === 'defaults') {
            type = 'EngineOptions';
        }
        if (propname === 'stdin') {
            type = 'EngineOptions';
        }
        if (propname === 'options' && name === 'linterhub.config.json') {
            type = 'EngineOptions';
        }
        if (propname === 'result') {
            type = 'EngineOutputSchema';
        }
        if (type === 'int' || type === 'bool') {
            if (propname !== 'installed' && propname !== 'locally') {
                type += '?';
            }
        }
        let postfix = (def != undefined ? (` = ` + (type.includes('string') ? `"${def.toString()}";` : `${def.toString()};`)) : '');
        type = type.replace('EngineOutputType', 'EngineOutputSchema');
        return `public ${type} ${format.highCase(propname)}` + (isArray ? ` = new ${type}();` : ` { get; set; }`) + postfix;
    },
    open: () => '{',
    close: () => '}',
    documentation: (text, isClass) => {
        let result = '\n/// <summary>\n/// ';
        if (isClass) {
            result += `${text}`;
        } else {
            result += `Gets or sets ${text.toLowerCase()}`;
        }
        result += '\n/// </summary>';
        return result;
    },
    type: (name) => {
        switch (name) {
            case undefined:
                return 'string';
            case 'integer':
                return 'int';
            case 'boolean':
                return 'bool';
            case 'object':
            case 'array':
            case 'string':
                return name;
        }
        name = format.undef(name);
        return format.highCase(name) + 'Type';
    },
    isDefaultType: (name) => name === 'string',
    tabs: (code) => {
        const arr = code.split('\n');
        let counter = 0;
        for (let i = 0; i < arr.length; ++i) {
            let line = '';
            if (arr[i] === '}') {
                counter--;
            }
            for (let j = 0; j < counter; ++j) {
                line += '\t';
            }
            if (arr[i] === '{') {
                counter++;
            }
            line += arr[i];
            arr[i] = line;
        }
        return arr.join('\n');
    },
};

const describe = {
    property: (name, value) => {
        let type = format.type(value.type);
        let isArray = false;
        if (value.enum || (value.items && value.items.enum)) {
            if (type === 'array') {
                isArray = true;
                type = 'List<' + format.type(name) + '>';
            } else {
                type = format.type(name);
            }
        }
        if (type === 'array') {
            isArray = true;
            const typeName = value.items.type ? format.type(value.items.type) : format.type(value.items.$ref);
            type = 'List<' + typeName + '>';
        }
        if (type === 'object' && value.properties) {
            type = format.type(name);
        }

        type = format.removeExtension(type);
        return format.property(name, type, isArray, value.default);
    },
};

const tree = {
    types: [],
    doc: [],
    visited_refs: [],
    described: [],
    init: () => {
        tree.types = [];
        tree.described = [];
        tree.doc = [];
        tree.visited_refs = [];
    },
    visit: (name, node) => {
        if (!node) {
            return;
        }
        if (node.properties) {
            tree.types.push({ name: name, node: node });
            Object.keys(node.properties).forEach((name) => {
                tree.visit(name, node.properties[name]);
            });
        }
        if (node.items && !format.isDefaultType(node.items.type)) {
            tree.types.push({ name: name, node: node });
            tree.visit(name, node.items);
        }
        if (node.$ref) {
            const ref = Object.resolve(node.$ref, schema);
            if (!tree.visited_refs[node.$ref]) {
                tree.visited_refs[node.$ref] = true;
                tree.visit(node.$ref, ref);
            }
        }
    },
    node: (nodeName, node) => {
        let typeName = format.type(nodeName);
        if (!nodeName) {
            typeName = format.removeExtension(name + 'Schema');
        }
        tree.doc.push(format.documentation(node.description, true));
        let elemType = node.type == 'array' ? format.type(node.items.$ref) : false;
        tree.doc.push(format.class(typeName, elemType));
        tree.doc.push(format.open());
        if (node.properties) {
            Object.keys(node.properties).forEach((name) => {
                const value = node.properties[name];
                tree.doc.push(format.documentation(value.description, false));
                if (value.enum || (value.items && value.items.enum)) {
                    if (name === 'languages' || name === 'manager' || name === 'severity' || name == 'found') {
                        tree.doc.push(format.enum(name, value));
                    } else {
                        value.enum = null;
                        value.items = {
                            type: 'string',
                        };
                    }
                }
                const prop = describe.property(name, value);
                tree.doc.push(prop);
            });
        }
        if (nodeName !== undefined) {
            tree.doc.push(format.close());
        }
    },
    document: (name, node) => {
        if (name && node.items && node.items.$ref) {
            return;
        }
        if (tree.described.indexOf(name) > -1) {
            return;
        }
        tree.node(name, node);
        tree.described.push(name);
    },
};

module.exports.generate = function (file) {
    tree.init();
    fileName = file;
    content = fs.readFileSync(fileName);
    name = path.basename(fileName);
    schema = JSON.parse(content);
    tree.doc.push('namespace Linterhub.Core.Schema');
    tree.doc.push(format.open());
    tree.doc.push('using System.Collections.Generic;');
    tree.doc.push('using Newtonsoft.Json;');
    tree.doc.push('using Newtonsoft.Json.Converters;');
    tree.visit(undefined, schema);
    tree.types.forEach((type) => tree.document(type.name, type.node));
    tree.doc.push(format.close());
    tree.doc.push(format.close());
    return format.tabs(tree.doc.join('\n'));
};

