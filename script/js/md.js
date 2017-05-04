const fs = require('fs');
const path = require('path');

const fileName = process.argv.slice(2)[0];
const content = fs.readFileSync(fileName);
const name = path.basename(fileName);
const schema = JSON.parse(content);

Object.resolve = (path, obj) => path.replace('#/', '').split('/').reduce((prev, curr) => prev ? prev[curr] : undefined, obj || self);

const format = {
  h1: (text) => `# ${text}`,
  h2: (text) => `## ${text}`,
  h3: (text) => `### ${text}`,
  required: (is) => is ? '+' : '-',
  capitalize: (text) => text.replace(/\w\S*/g, (txt) => txt.charAt(0).toUpperCase() + txt.substr(1)),
  table: {
    row: (name, type, required, description) => `|${name}|${type}|${required}|${description}|`,
    header: () => `|Key|Type|Required|Description|`,
    columns: () => `|-|:-:|:-:|-|`,
    array: (type) => {
      if (format.isDefaultType(type)) {
        return `${type}[]`;
      } else if (type.indexOf('.json') < 0) {
        return `[${type}](#${type})[]`;
      } else { 
        const name = format.capitalize(format.title(type));
        const file = type.replace('.json', '.md');
        return `[${name}](${file})[]`;
      }
    },
    type: (type) => `[${type}](#${type})`,
  },
  title: (text) => text.replace('.json', '').replace('.', ' '),
  type: (name) => name.replace('#/definitions/', ''),
  isDefaultType: (name) => name === 'string'
};

const describe = {
  description: (value) => {
    return value.description +
      (value.enum ? '. Possible values: ' + value.enum.map(v => `\`${v}\``).join(', ') : '') +
      (value.items && value.items.enum ? '. Possible values: ' + value.items.enum.map(v => `\`${v}\``).join(', ') : '') +
      (value.default ? '. Default is `' + value.default + '`' : '');
  },
  type: (name, value) => {
    var type = value.type;
    if (type === 'array') {
      const typeName = value.items.type ? value.items.type : format.type(value.items.$ref);
      type = format.table.array(typeName);
    }
    if (type === 'object') {
      type = format.table.type(name);
    }
    return type;
  },
  property: (name, value, required) => {
    const type = describe.type(name, value);
    const requiredText = format.required(required);
    const description = describe.description(value);
    return format.table.row(name, type, requiredText, description);
  },
};

const tree = {
  types: [],
  described: [],
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
    if (nodeName) {
      const typeName = format.type(nodeName);
      tree.doc.push(format.h3(typeName));
    }
    tree.doc.push(node.description);
    if (node.properties) {
      tree.doc.push('');
      tree.doc.push(format.table.header());
      tree.doc.push(format.table.columns());
      Object.keys(node.properties).forEach((name) => {
        const value = node.properties[name];
        const required = node.required ? node.required.indexOf(name) > -1 : false;
        const row = describe.property(name, value, required);
        tree.doc.push(row);
      });
    }
  },
  document: (name, node) => {
    if (tree.described.indexOf(name) > -1) {
      return;
    }
    if (name && node.items && node.items.$ref) {
      return;
    }
    tree.node(name, node);
    tree.described.push(name);
  },
  example: (nodeName, node) => {
    var result = '';
    if (!node) {
      return result;
    }
    result += (nodeName ? `"${nodeName}":`: '');
    if (nodeName && !node.properties && !node.items) {
      result += node.type === 'integer' ? 0 :
                node.enum ? `"${node.enum[0]}"` :
                node.type === 'boolean' ? false : `"${node.type}"`;
    }
    if (node.properties) {
      const properties = Object.keys(node.properties).map((name) => tree.example(name, node.properties[name])).join(',');
      result += `{${properties}}`;
    }
    result += 
      (node.items ? `[${tree.example(undefined, node.items)}]` : '') +
      (node.$ref ? tree.example(undefined, Object.resolve(node.$ref, schema)) : '');

    return result;
  },
};

const title = format.capitalize(format.title(name));
tree.visit(undefined, schema);
tree.doc.push(format.h1(title));
tree.doc.push(schema.title);
tree.doc.push(format.h2('Structure'));
tree.types.forEach(type => tree.document(type.name, type.node));
tree.doc.push(format.h2('Example'));
const example = tree.example(undefined, schema);
const jsonExample = JSON.stringify(JSON.parse(example), null, 4);
tree.doc.push('```');
tree.doc.push(jsonExample);
tree.doc.push('```');
console.log(tree.doc.join('\n'));
