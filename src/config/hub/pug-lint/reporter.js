'use strict';

let reporter = require('../reporter.regex');
let md5 = require('../md5.min');
let regex = /(.*):([0-9]+):([0-9]+) (.*)/g;

reporter.run(regex, function (match) {
    let ruleId = md5(match[4]).substr(0, 6);
    return {
        path: match[1].trim().replace(/^(\.\/)/,""),
        message: {
            message: match[4],
            severity: 'warning',
            line: match[2] - 1,
            lineEnd: match[2] - 1,
            column: match[3] - 1,
            ruleId: 'pug-lint:' + ruleId
        },
    };
});
