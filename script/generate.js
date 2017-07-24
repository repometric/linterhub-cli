'use strict';
const fs = require('fs');
const cs = require('./js/cs.js');
const md = require('./js/md.js');


const content = fs.readFileSync('script/js/generate.json');
const data = JSON.parse(content);

data.forEach(function(elem) {
    if (elem.cs !== undefined) {
        console.log('MD:', elem.schema);
        fs.writeFile(elem.cs, cs.generate(elem.schema));
    }
    if (elem.md !== undefined) {
        console.log('CS:', elem.schema);
        fs.writeFile(elem.md, md.generate(elem.schema));
    }
});
