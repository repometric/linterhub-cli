'use strict';

let reporter = require('../reporter.regex');
let regex = /(.*): line ([0-9]+), col ([0-9]+), (.*) - (.*) \((.*)\)/g;

reporter.run(regex, function (match) {
    return {
        path: match[1],
        message: {
            message: match[5],
            severity: match[4].toLowerCase(),
            line: match[2],
            lineEnd: match[2],
            column: match[3],
            columnEnd: 1000,
            ruleId: 'csslint:' + match[6],
            ruleName: match[6],
        },
    };
});
