const fs = require('fs');
const path = require('path');

const fileName = process.argv.slice(2)[0];
const content = fs.readFileSync(fileName);
const name = path.basename(fileName);
const schema = JSON.parse(content);

Object.resolve = (path, obj) => path.replace('#/', '').split('/').reduce((prev, curr) => prev ? prev[curr] : undefined, obj || self);

const format = {
    undef: (name) => name.replace('#/definitions/', ''),
    class: (name) => `public class ${format.type(format.undef(name))}`,
    enum: (name, value) => {
        return `public enum ${format.type(name)}\n` + format.open() + "\n" +
            (value.enum ? value.enum.join(',\n') : '') +
            (value.items && value.items.enum ? value.items.enum.join(',\n') : '') + "\n" + format.close();
    },
    property: (name, type) => `public ${type} ${name} { get; set; }`,
    open: () => `{`,
    close: () => '}',
    documentation: (text) => `\n/// <summary>\n/// ${text}\n/// </summary>`,
    type: (name) => {
        if (name == undefined)
            return "string";
        name = format.undef(name);
        return name.charAt(0).toUpperCase() + name.slice(1);
    },
    isDefaultType: (name) => name === 'string',
    tabs: (code) => {
        var arr = code.split('\n');
        var counter = 0;
        for (var i = 0; i < arr.length; ++i) {
            var line = "";
            if (arr[i] === "}")
                counter--;
            for(var j = 0; j < counter; ++j)
                line += "\t";
            if (arr[i] === "{")
                counter++;
            line += arr[i];
            arr[i] = line;
        }
        return arr.join('\n');
    }
};

const describe = {
    property: (name, value) => {
        var type = format.type(value.type);
        if(value.enum || (value.items && value.items.enum))
        {
             if (type === 'Array')
                type = format.type(name) + "[]";
             else
                type = format.type(name);
        }
        if (type === 'Array') {
          const typeName = value.items.type ? format.type(value.items.type) : format.type(value.items.$ref);
          type = `${typeName}[]`;
        }
        if (type === 'Object') {
          if(value.properties)
            type = format.type(name);
        }

        return format.property(name, type);
    }
};

const tree = {
    types: [],
    doc: [],
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
            tree.visit(node.$ref, ref);
        }
    },
    node: (nodeName, node) => {
        var typeName = format.type(nodeName);
        if (!nodeName)
        {
            typeName = name.replace('.json', '').split(".");
            for(var i = 0; i < typeName.length; ++i)
                typeName[i] = format.type(typeName[i]);
            typeName = typeName.join("");
        }
        tree.doc.push(format.documentation(node.description));
        tree.doc.push(format.class(typeName));
        tree.doc.push(format.open());
        if (node.properties) {
            Object.keys(node.properties).forEach((name) => {
                const value = node.properties[name];
                tree.doc.push(format.documentation(value.description));
                const prop = describe.property(name, value);
                tree.doc.push(prop);
                tree.doc.push("");
                if(value.enum || (value.items && value.items.enum))
                    tree.doc.push(format.enum(name, value));
            });
        }
        if(nodeName != undefined)
            tree.doc.push(format.close());
    },
    document: (name, node) => {
        if (name && node.items && node.items.$ref) {
            return;
        }
        tree.node(name, node);
    },
};

tree.visit(undefined, schema);
tree.types.forEach(type => tree.document(type.name, type.node));
tree.doc.push(format.close());
console.log(format.tabs(tree.doc.join('\n')));
