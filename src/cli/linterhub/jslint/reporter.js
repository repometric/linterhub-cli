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
    if (line === "")
        print_result();
    else {
        var data = JSON.parse(line);
        result.push({
            filePath: data[0],
            messages: data[1].map(function (problem) {
                if (problem != null)
                    return {
                        message: problem.reason,
                        severity: problem.id === "(error)" ? "error" : "warning",
                        source: problem.evidence,
                        line: problem.line - 1,
                        lineEnd: problem.line - 1,
                        column: problem.character - 1,
                        columnEnd: 1000,
                        ruleId: problem.code
                    };
            })
        });
    }
}

function print_result() {
    console.log(JSON.stringify(result));
}