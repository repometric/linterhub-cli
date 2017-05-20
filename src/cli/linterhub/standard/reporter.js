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
    print_result();
});

var result = [];

function processLine(line) {
    var regex = /(.*):([0-9]+):([0-9]+): (.*) \((.*)\)/g;
    var match = regex.exec(line);

    if (match == null) {
        return;
    }

    var problem = {
        message: match[4],
        severity: "warning",
        line: match[2] - 1,
        lineEnd: match[2] - 1,
        column: match[3] - 1,
        columnEnd: 1000,
        ruleId: "standard:" + match[5],
        ruleName: match[5]
    }

    var filePath = match[1].trim();

    var obj = result.find(function (element, index, array) {
        return filePath === element.path;
    });

    if (obj === undefined) {
        obj = {
            path: path,
            messages: []
        };
        result.push(obj);
    }

    obj.messages.push(problem);
}

function print_result() {
    console.log(JSON.stringify(result));
}