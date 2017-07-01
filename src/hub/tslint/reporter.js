'use strict';

let reporter = require('../reporter.regex');
let regex = /(.*): \((.*)\) (.*)\[([0-9]+), ([0-9]+)\]: (.*)/g;

reporter.run(regex, function (match) {
    return {
        path: match[3].trim(),
        message: {
            message: match[6],
            severity: match[1] === 'ERROR' ? 'error' : 'warning',
            line: match[4] - 1,
            lineEnd: match[4] - 1,
            column: match[5] - 1,
            ruleId: 'tslint:' + match[2],
            ruleName: match[2],
        },
    };
});
