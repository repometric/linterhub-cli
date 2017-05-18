const exec = require('child_process').exec;
const fs = require('fs');
const path = require('path');

var getFiles = function (dir, files_) {
    files_ = files_ || [];
    var files = fs.readdirSync(dir);
    for (var i in files) {
        var name = dir + '/' + files[i];
        if (fs.statSync(name).isDirectory()) {
            getFiles(name, files_);
        } else {
            if (path.extname(name) === ".json")
            files_.push(name);
        }
    }
    return files_;
};

function executeForFile(file)
{
    var command = "";
    for (var i = process.argv.length - 1; i != 0; --i)
    {
        var current = process.argv[i];
        if(current === "*")
            current = file;
        command = current + " " + command;
        if (current === "jsonlint")
            break;
    }
    exec(command, { cwd: process.cwd() }, function (error, stdout, stderr) {
        console.log(stderr);
    });
}

if(process.argv.findIndex((x) => x === "*") != -1)
    getFiles(process.cwd()).forEach(executeForFile);
else
    executeForFile("");