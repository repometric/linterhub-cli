let template = require('../prefix.template');
let childProcess = require('child_process');

template.run('.json', function(file) {
    let command = '';
    for (let i = process.argv.length - 1; i != 0; --i) {
        let current = process.argv[i];
        if (current === '*') {
            current = file;
        }
        command = current + ' ' + command;
        if (current === 'jsonlint') {
            break;
        }
    }
    childProcess.exec(command, { cwd: process.cwd() }, function(error, stdout, stderr) {
        console.log(stderr);
    });
});
