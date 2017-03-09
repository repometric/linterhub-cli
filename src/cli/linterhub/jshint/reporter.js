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
                line: error.evidence,
                row: {
                    start: error.line,
                    end: error.line
                },
                column: {
                    start: error.character,
                    end: 1000
                },
                rule: {
                    id: error.code,
                    name: error.id,
                    namespace: error.scope
                }
            });
        });
        process.stdout.write(JSON.stringify(Object.keys(files).map(function (key) {
            return {
                path: key,
                messages: files[key]
            }
        })));
	}
};