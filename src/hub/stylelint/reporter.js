module.exports = function (stylelintResults) {
    let result = stylelintResults.map(function (file) {
        return {
            path: file.source,
            messages: file.warnings.map(function (problem) {
                return {
                    message: problem.text,
                    severity: problem.severity,
                    line: problem.line - 1,
                    lineEnd: problem.line - 1,
                    column: problem.column - 1,
                    ruleId: 'stylelint:' + problem.rule,
                };
            }),
        };
    });
    return JSON.stringify(result);
};
