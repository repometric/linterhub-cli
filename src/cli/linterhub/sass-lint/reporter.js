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

var lines = "";

function processLine(line) {
    lines += line + "\n";
}

function print_result() {
    var results = JSON.parse(lines);
    results.forEach(function(element) {
        element.path = element.filePath;
        element.filePath = null;
    }, this);
    console.log(JSON.stringify(results));
}