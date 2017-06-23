let template = require("../reporter.template");

template.run(function (lines) {
    let results = JSON.parse(lines.join("\n"));
    results.forEach(function (element) {
        element.path = element.filePath;
        element.filePath = null;
    }, this);
    console.log(JSON.stringify(results));
});