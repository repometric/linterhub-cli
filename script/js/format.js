const fs = require('fs');
const path = require('path');
const cp = require('child_process');
const com = require('./common.js');
const folder = path.join('src', 'hub');
const mask = '.json';
com.find(folder, mask, function(fileName) {
    console.log(`File: ${fileName}`);
    console.log('Format');
    var content = fs.readFileSync(fileName).toString();
    var json = JSON.parse(content);
    var formatted = JSON.stringify(json, null, 4);
    fs.writeFileSync(fileName, formatted + '\n');
    console.log('Validate');
    var result = cp.spawnSync(`z-schema`, [`${fileName}`]);
    if (result.status) {
        console.log(`Fail`);
        process.exit(1);
    }
});
