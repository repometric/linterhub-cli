'use strict';
const fs = require('fs');
const path = require('path');

const fileName = process.argv.slice(2)[0];
const content = fs.readFileSync(fileName);
const name = path.basename(fileName);
const schema = JSON.parse(content);

Object.resolve = (path, obj) => path.replace('#/', '').split('/').reduce((prev, curr) => prev ? prev[curr] : undefined, obj || null);

const format = {
    undef: (name) => name.replace('#/definitions/', ''),
    class: (name) => `public class ${format.highCase(format.undef(name))}`,
    highCase: (value) => value.charAt(0).toUpperCase() + value.slice(1),
    enum: (name, value) => {
        name = name.charAt(0).toUpperCase() + name.slice(1);
        const jsonProp = '[JsonConverter(typeof(StringEnumConverter))]\n';
        return jsonProp + `public enum ${format.highCase(name)}Type\n` + format.open() + '\n' +
            (value.enum ? value.enum.join(',\n') : '') +
            (value.items && value.items.enum ? value.items.enum.join(',\n') : '') + '\n' + format.close() + '\n';
    },
    removeExtension: (typeName) => {
        if(typeName.indexOf('.json') == -1) {
            return typeName;
        }
        typeName = typeName.replace('.json', '').split('.');
        for (let i = 0; i < typeName.length; ++i) {
            typeName[i] = format.highCase(typeName[i]);
        }
        return typeName.join('');
    },
    property: (propname, type, isArray) => {
        if (propname === 'defaults') {
            type = 'LinterOptions';
        }
        if (propname === 'options' && name === 'linterhub.config.json') {
            type = 'LinterOptions';
        }
        if (type === 'int' || type === 'bool') {
            type += '?';
        }
        type = type.replace('EngineOutputType', 'EngineOutputSchema');
        return `public ${type} ${format.highCase(propname)}` + (isArray ? ` = new ${type}();` : ` { get; set; }`);
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
        if (name == undefined)
            return 'string';
        if (name === 'string')
            return name;
        if (name == 'integer')
            return 'int';
        if (name == 'boolean')
            return 'bool';
        if (name == 'object')
            return name;
        if (name == 'array')
            return name;
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
        return format.property(name, type, isArray);
    },
};

const tree = {
    types: [],
    doc: [],
    described: [],
    visit: (name, node) => {
        if (!node) {
            return;
        }
        if (node.properties) {
            tree.types.push({name: name, node: node});
            Object.keys(node.properties).forEach((name) => {
                tree.visit(name, node.properties[name]);
            });
        }
        if (node.items && !format.isDefaultType(node.items.type)) {
            tree.types.push({name: name, node: node});
            tree.visit(name, node.items);
        }
        if (node.$ref) {
            const ref = Object.resolve(node.$ref, schema);
            tree.visit(node.$ref, ref);
        }
    },
    node: (nodeName, node) => {
        let typeName = format.type(nodeName);
        if (!nodeName) {
            typeName = format.removeExtension(name + 'Schema');
        }
        tree.doc.push(format.documentation(node.description, true));
        tree.doc.push(format.class(typeName));
        tree.doc.push(format.open());
        if (node.properties) {
            Object.keys(node.properties).forEach((name) => {
                const value = node.properties[name];
                tree.doc.push(format.documentation(value.description, false));
                if (value.enum || (value.items && value.items.enum)) {
                    if (name === 'languages' || name === 'manager' || name === 'severity') {
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
        if(nodeName != undefined) {
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
tree.doc.push('namespace Linterhub.Engine.Schema');
tree.doc.push(format.open());
tree.doc.push('using System.Collections.Generic;');
tree.doc.push('using Newtonsoft.Json;');
tree.doc.push('using Newtonsoft.Json.Converters;');
tree.visit(undefined, schema);
tree.types.forEach((type) => tree.document(type.name, type.node));
tree.doc.push(format.close());
tree.doc.push(format.close());
console.log(format.tabs(tree.doc.join('\n')));
