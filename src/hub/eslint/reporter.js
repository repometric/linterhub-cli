module.exports = function (results) {
    results.forEach(function(element) {
        element.path = element.filePath;
        element.filePath = undefined;
    }, this);
    console.log(JSON.stringify(results, null, 2));
}