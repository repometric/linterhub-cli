'use strict';

var md5 = require("./md5.min")

process.stdin.resume();
process.stdin.setEncoding('utf8');

var lingeringLine = "";

process.stdin.on('data', function (chunk) {
    var lines = chunk.split("\n");

    lines[0] = lingeringLine + lines[0];
    lingeringLine = lines.pop();

    lines.forEach(processLine);
});

process.stdin.on('end', function () {
    print_result();
});

var filePath = undefined;
var result = [];

function processLine(line) {
    if(filePath == undefined)
    {
        filePath = line.trim();
        return;
    }
    var regex = /\s+line ([0-9]+)\s+col ([0-9]+)\s+(.*)/g;
    var match = regex.exec(line);

    if (match == null) {
        return;
    }

    var problem = {
        message: match[3].trim(),
        severity: "warning",
        line: match[1] - 1,
        lineEnd: match[1] - 1,
        column: match[2] - 1,
        ruleId: "colorguard:" + md5(match[3]).substr(0,6),
    }

    result.push(problem);
}

function print_result() {
    var ret = {
        path: filePath,
        messages: result
    };
    console.log(JSON.stringify(ret));
}