const child_process = require('child_process');
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
            if (path.extname(name) === ".css")
            files_.push(name);
        }
    }
    return files_;
};

var result = [];

function executeForFile(file)
{
    var command = "";
    for (var i = process.argv.length - 1; i != 0; --i)
    {
        var current = process.argv[i];
        if(current === "*")
            current = file;
        if(current === "node")
            current = "| node";
        command = current + " " + command;
        if (current === "colorguard")
            break;
    }
    var parsed = JSON.parse(child_process.execSync(command, { cwd: process.cwd() }).toString());
    if(parsed.filePath != undefined)
        result.push(parsed);
}

if(process.argv.findIndex((x) => x === "*") != -1)
    getFiles(process.cwd()).forEach(executeForFile);
else
    executeForFile("");

console.log(JSON.stringify(result));