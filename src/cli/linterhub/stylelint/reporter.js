module.exports = function (stylelintResults) {
    var result = [];
    stylelintResults.forEach(function (file) {
        var obj = {
            filePath: file.source,
            messages: []
        };
        file.warnings.forEach(function (problem) {
            obj.messages.push({
                message: problem.text,
                severity: problem.severity,
                line: problem.line - 1,
                lineEnd: problem.line - 1,
                column: problem.column - 1,
                columnEnd: 1000,
                ruleId: problem.rule
            });
        });
        result.push(obj);
    });
    return JSON.stringify(result);
};