'use strict';

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
    console.log(JSON.stringify(result));
});

var result = [];

function processLine(line) {
    var regex = /(.*): \((.*)\) (.*)\[([0-9]+), ([0-9]+)\]\: (.*)/g;
    var match = regex.exec(line);

    if (match == null) {
        return;
    }

    var problem = {
        message: match[6],
        severity: match[1] === "ERROR" ? "error" : "warning",
        line: match[4] - 1,
        lineEnd: match[4] - 1,
        column: match[5] - 1,
        columnEnd: 1000,
        ruleId: "tslint:" + match[2],
        ruleName: match[2]
    }

    var filePath = match[3].trim();

    var obj = result.find(function (element, index, array) {
        return filePath === element.path;
    });

    if (obj === undefined) {
        obj = {
            path: filePath,
            messages: []
        };
        result.push(obj);
    }

    obj.messages.push(problem);
}

function print_result() {
    console.log(JSON.stringify(result));
}