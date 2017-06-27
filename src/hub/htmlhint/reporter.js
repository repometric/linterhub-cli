let template = require('../reporter.template');

template.run(function(lines) {
    let results = [];

    lines.forEach(function(line) {
        if (line === '') {
            return;
        }

        let data = JSON.parse(line);

        results = data.map(function(current) {
            return {
                path: current.file,
                messages: current.messages.map(function(problem) {
                    return {
                        message: problem.message,
                        severity: problem.type,
                        source: problem.evidence.trim(),
                        line: problem.line - 1,
                        lineEnd: problem.line - 1,
                        column: problem.col - 1,
                        columnEnd: 1000,
                        ruleId: 'htmlhint:' + problem.rule.id,
                    };
                }),
            };
        });
    });

    console.log(JSON.stringify(results));
});
