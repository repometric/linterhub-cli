let template = require("../reporter.template");

template.run(function (lines) {
    let results = [];
    
    lines.forEach(function (line) {
        if (line === "")
            return;
        else {
            let data = JSON.parse(line);
            if (data[1][data[1].length - 1] === null)
                data[1].splice(data[1].length - 1, 1);
            results.push({
                path: data[0],
                messages: data[1].map(function (problem) {
                    if (problem != null)
                        return {
                            message: problem.reason,
                            severity: problem.id === "(error)" ? "error" : "warning",
                            source: (problem.evidence == undefined ? "" : problem.evidence).trim(),
                            line: problem.line - 1,
                            lineEnd: problem.line - 1,
                            column: problem.character - 1,
                            columnEnd: 1000,
                            ruleId: problem.code
                        };
                })
            });
        }
    });

    console.log(JSON.stringify(results));
});