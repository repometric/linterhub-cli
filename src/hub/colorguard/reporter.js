'use strict';

let md5 = require("../md5.min");
let template = require("../reporter.template");

template.run(function (lines) {
    let filePath = undefined;
    let results = [];

    lines.forEach(function (line) {
        if (filePath === undefined) {
            filePath = line
            return;
        }

        let regex = /\s+line ([0-9]+)\s+col ([0-9]+)\s+(.*)/g;
        let match = regex.exec(line);

        if (match === null) {
            return;
        }

        var problem = {
            message: match[3].trim().replace(/\s+/g,' '),
            severity: "warning",
            line: match[1] - 1,
            lineEnd: match[1] - 1,
            column: match[2] - 1,
            ruleId: "colorguard:" + md5(match[3]).substr(0, 6),
        }

        results.push(problem);
    });

    console.log(JSON.stringify({
        path: filePath,
        messages: results
    }));
});