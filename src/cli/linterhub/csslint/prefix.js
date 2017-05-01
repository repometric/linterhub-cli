const exec = require('child_process').exec;
const fs = require('fs');
const path = require('path');

var command = "";

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

for (var i = process.argv.length - 1; i != 0; --i)
{
    if(process.argv[i] === "./")
    {
        var list = "";
        getFiles(process.cwd()).forEach(function (file) {
            list += file + " ";
        });
        command = list + command;
    }
    else
    {
        command = process.argv[i] + " " + command;
        if (process.argv[i] === "csslint")
            break;
    }
}

exec(command, { cwd: process.cwd() }, function (error, stdout, stderr) {
    console.log(stdout);
});