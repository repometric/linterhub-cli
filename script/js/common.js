const path = require('path');
const fs = require('fs');

/**
 * Find files by filter in the target folder
 * @param {string} folder - Target folder
 * @param {string} filter - Filter filesystem names
 * @param {Object} callback - Callback for search results
 */
function find(folder, filter, callback) {
    if (!fs.existsSync(folder)) {
        return;
    }

    const files = fs.readdirSync(folder);
    for (let i = 0; i < files.length; i++) {
        const filename = path.join(folder, files[i]);
        const stat = fs.lstatSync(filename);
        if (stat.isDirectory()) {
            find(filename, filter, callback);
        } else if (filename.indexOf(filter) >= 0) {
            callback(filename);
        }
    }
}

module.exports = {
    find: find,
};
