let template = require('../prefix.template');
let childProcess = require('child_process');

template.run('.css', function (file, result) {
    let command = '';
    for (let i = process.argv.length - 1; i != 0; --i) {
        let current = process.argv[i];
        if (current === '*') {
            current = file;
        }
        if (current === 'node') {
            current = '| node';
        }
        command = current + ' ' + command;
        if (current === 'colorguard') {
            break;
        }
    }
    let parsed = JSON.parse(childProcess.execSync(command, { cwd: process.cwd() }).toString());
    if (parsed.path != undefined) {
        result.push(parsed);
    }
}, function (result) {
    console.log(JSON.stringify(result));
});
