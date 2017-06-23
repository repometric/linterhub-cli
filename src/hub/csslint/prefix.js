let template = require("../prefix.template");
let child_process = require('child_process');

template.run = function (extname) {
    var command = "";

    for (var i = process.argv.length - 1; i != 0; --i) {
        if (process.argv[i] === "./") {
            var list = "";
            this.getFiles(process.cwd(), null, extname).forEach(function (file) {
                list += file + " ";
            });
            command = list + command;
        }
        else {
            command = process.argv[i] + " " + command;
            if (process.argv[i] === "csslint")
                break;
        }
    }

    child_process.exec(command, { cwd: process.cwd() }, function (error, stdout, stderr) {
        console.log(stdout);
    });
}

template.run(".css");