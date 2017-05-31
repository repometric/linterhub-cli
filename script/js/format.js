const fs = require('fs');
const path = require('path');
const cp = require('child_process');
const com = require('./common.js');
const folder = path.join('src', 'hub');
const mask = '.json';
com.find(folder, mask, function(fileName) {
    console.log(`File: ${fileName}`);
    console.log('Format');
    const content = fs.readFileSync(fileName).toString();
    const json = JSON.parse(content);
    const formatted = JSON.stringify(json, null, 4);
    fs.writeFileSync(fileName, formatted + '\n');
    console.log('Validate');
    const result = cp.spawnSync(`z-schema`, [`${fileName}`]);
    if (result.status) {
        console.log(`Fail`);
        console.log(result.stderr.toString());
        process.exit(1);
    }
});
