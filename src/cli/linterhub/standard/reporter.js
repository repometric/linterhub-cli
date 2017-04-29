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
        var arr = line.split(":");
        var filePath = "";
        var sdv = 0;

        if (arr.length != 4)
            sdv = arr.length - 4;

        for (var i = 0; i <= sdv; ++i)
        {
            filePath += arr[i];
            if (i != sdv)
                filePath += ":";
        }

        filePath = filePath.trim();

        var problem = {
            line: arr[1 + sdv] - 1,
            lineEnd: arr[1 + sdv] - 1,
            column: arr[2 + sdv],
            columnEnd: 1000,
            message: arr[3 + sdv].split("(")[0].trim(),
            ruleId: (arr[3 + sdv].split("(")[1]).split(")")[0].trim()
        };

        var obj = result.find(function (element, index, array) {
            return filePath === element.filePath;
        });
        if (obj === undefined) {
            obj = {
                filePath: filePath,
                messages: []
            };
            result.push(obj);
        }
        obj.messages.push(problem);
    }
}

function print_result() {
    console.log(JSON.stringify(result));
}