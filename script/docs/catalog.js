const path = require('path');
const _process = require('child_process');

var doc = [];

const format = {
  h1: (text) => `# ${text}`,
  h2: (text) => `## ${text}`,
  h3: (text) => `### ${text}`,
  required: (is) => is ? '+' : '-',
  capitalize: (text) => text.replace(/\w\S*/g, (txt) => txt.charAt(0).toUpperCase() + txt.substr(1)),
  table: {
    row: (name, type, required, description) => `|${name}|${type}|${required}|${description}|`,
    header: () => `|Name|Description|Languages|Details|`,
    columns: () => `|-|-|:-:|-|`,
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

doc.push(format.h1("Available Engines"));
doc.push("The document describes all supported engines");
doc.push("");
doc.push(format.table.header());
doc.push(format.table.columns());

_process.exec('dotnet cli.dll --mode=catalog', {
        cwd: path.join(__dirname, '../../bin/dotnet')
    }, function(error, stdout, stderr) {
        var data = JSON.parse(stdout);
        data.forEach(function(element) {
            doc.push(`|[${element.name}](${element.url})|${element.description}|${element.languages.join(', ')}|[Options](${element.name})|`)
        }, this);
        console.log(doc.join('\n'));
    }
);
