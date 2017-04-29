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
    processLine(lingeringLine);
});

var result = [];

function processLine(line) {
    var data = JSON.parse(line);
    data.forEach(function (problem) {
        var obj = result.find(function (element, index, array) {
            return problem.name === element.filePath;
        });
        if (obj === undefined) {
            obj = {
                filePath: problem.name,
                messages: []
            };
            result.push(obj);
        }
        obj.messages.push({
            message: problem.failure,
            severity: problem.ruleSeverity === "ERROR" ? "error" : "warning",
            line: problem.startPosition.line,
            lineEnd: problem.endPosition.line,
            column: problem.startPosition.character,
            columnEnd: problem.endPosition.character,
            ruleId: "tslint:" + problem.ruleName,
            ruleName: problem.ruleName
        });
    });
    print_result();
}

function print_result() {
    console.log(JSON.stringify(result));
}