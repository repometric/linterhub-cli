let template = require('../reporter.template');

template.run(function (lines) {
    try {
    let results = JSON.parse(lines.join('\n'));
    results.forEach(function (element) {
        element.path = element.filePath;
        element.filePath = undefined;
        element.warningCount = undefined;
        element.errorCount = undefined;
        element.ruleId = 'sass-lint:' + element.ruleId;
    });
    console.log(JSON.stringify(results));
    } catch (e) {
        console.log(JSON.stringify([{
            messages: [],
            path: '',
        }]));
    }
});
