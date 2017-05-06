const fs = require('fs');
const fileName = process.argv.slice(2)[0];
const content = fs.readFileSync(fileName).toString();
const json = JSON.parse(content);
const formatted = JSON.stringify(json, null, 4);
fs.writeFileSync(fileName, formatted + '\n');
