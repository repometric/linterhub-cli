module.exports = function (results) {
    results.forEach(function(element) {
        element.path = element.filePath;
        element.filePath = undefined;
        element.ruleId = 'eslint:' + element.ruleId;
    }, this);
    return JSON.stringify(results, null, 2);
}
