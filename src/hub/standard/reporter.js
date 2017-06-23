'use strict';

let reporter = require("../reporter.regex");
let regex = /(.*):([0-9]+):([0-9]+): (.*) \((.*)\)/g;

reporter.run(regex, function (match) {
    return {
        path: match[1].trim(),
        message: {
            message: match[4],
            severity: "warning",
            line: match[2] - 1,
            lineEnd: match[2] - 1,
            column: match[3] - 1,
            columnEnd: 1000,
            ruleId: "standard:" + match[5],
            ruleName: match[5]
        }
    };
});
