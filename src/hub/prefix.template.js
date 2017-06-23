let fs = require('fs');
let path = require('path');

module.exports = {
    run: function (extname, executeForFile, afterAll) {

        let result = [];

        if (process.argv.findIndex((x) => x === "*") != -1)
            this.getFiles(process.cwd(), null, extname).forEach((x) => executeForFile(x, result));
        else
            executeForFile("", result);
        if(afterAll)
            afterAll(result);
    },
    getFiles: function (dir, files_, extname) {
        files_ = files_ || [];
        let files = fs.readdirSync(dir);
        for (let i in files) {
            let name = dir + '/' + files[i];
            if (fs.statSync(name).isDirectory()) {
                this.getFiles(name, files_, extname);
            } else {
                if (path.extname(name) === extname)
                    files_.push(name);
            }
        }
        return files_;
    }
}