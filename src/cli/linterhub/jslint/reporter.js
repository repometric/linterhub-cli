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
    else {
        var data = JSON.parse(line);
        if (data[1][data[1].length - 1] === null)
            data[1].splice(data[1].length - 1, 1);
        result.push({
            path: data[0],
            messages: data[1].map(function (problem) {
                if (problem != null)
                    return {
                        message: problem.reason,
                        severity: problem.id === "(error)" ? "error" : "warning",
                        source: (problem.evidence == undefined ? "" : problem.evidence).trim(),
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