const path = require('path');
const fs = require('fs');
const cp = require('child_process');

function find(folder, filter, callback) {
    if (!fs.existsSync(folder)) {
        return;
    }

    var files = fs.readdirSync(folder);
    for (var i = 0; i < files.length; i++) {
        var filename = path.join(folder, files[i]);
        var stat = fs.lstatSync(filename);
        if (stat.isDirectory()) {
            find(filename, filter, callback);
        }
        else if (filename.indexOf(filter) >= 0) {
            callback(filename);
        };
    };
};

const folder = path.join('src', 'hub');
const formatter = path.join('script', 'js', 'format.js');
const mask = '.json';
find(folder, mask, function(fileName) {
    console.log('Format: ' + fileName);
    cp.fork(formatter, [fileName]);
});
