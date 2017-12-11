'use strict';
module.exports = {
	reporter: function (results, data, opts) {
        var files = {};
		opts = opts || {};
        results.forEach(function(record) {
            var error = record.error;
            var severity = '';
            if (!files[record.file]) {
                files[record.file] = [];
            }

            switch (error.code[0]) {
                case 'I':
                    severity = 'info';
                    break;
                case 'W':
                    severity = 'warning';
                    break;
                default:
                case 'E':
                    severity = 'error';
                    break;
            }

            files[record.file].push({
                message: error.reason,
                severity: severity,
                source: error.evidence.trim(),
                line: error.line,
                lineEnd: error.line,
                column: error.character,
                columnEnd: 1000,
                ruleId: error.code,
                ruleName: error.id,
                ruleNamespace: error.scope
            });
        });
        process.stdout.write(JSON.stringify(Object.keys(files).map(function (key) {
            return {
                filePath: key,
                messages: files[key]
            }
        })));
	}
};