'use strict';

let reporter = require('../reporter.regex');
let regex = /(.*): line ([0-9]+), col ([0-9]+), (.*)/g;
let md5 = require('../md5.min');

reporter.run(regex, function(match) {
    return {
        path: match[1].trim(),
        message: {
            message: match[4],
            severity: 'warning',
            line: match[2] - 1,
            lineEnd: match[2] - 1,
            column: match[3] - 1,
            ruleId: 'jsonlint:' + md5(match[4]).substr(0, 6),
        },
    };
});
