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
    if (line === "")
        return;

    var data = JSON.parse(line);
    result = data.map(function (file) {
        return {
            path: file.file,
            messages: file.messages.map(function (problem) {
                return {
                    message: problem.message,
                    severity: problem.type,
                    source: problem.evidence.trim(),
                    line: problem.line - 1,
                    lineEnd: problem.line - 1,
                    column: problem.col - 1,
                    columnEnd: 1000,
                    ruleId: "htmlhint:" + problem.rule.id
                }
            })
        }
    });
}
