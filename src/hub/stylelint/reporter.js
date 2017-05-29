module.exports = function (stylelintResults) {
    var result = stylelintResults.map(function (file) {
        return {
            path: file.source,
            messages: file.warnings.map(function (problem) {
                return {
                    message: problem.text,
                    severity: problem.severity,
                    line: problem.line - 1,
                    lineEnd: problem.line - 1,
                    column: problem.column - 1,
                    columnEnd: 1000,
                    ruleId: problem.rule
                };
            })
        };
    });
    return JSON.stringify(result);
};