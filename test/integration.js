const fs = require('fs');
const path = require('path');
const _process = require('child_process');

var integration = path.join(__dirname, 'integration');

function printError(f, s)
{
    console.error("\x1b[37m%s: \x1b[31m%s\x1b[37m", f, s);
}

function printSuccess(f, s)
{
    console.error("\x1b[37m%s: \x1b[32m%s\x1b[37m", f, s);
}

function printWarning(f, s)
{
    console.error("\x1b[37m%s: \x1b[33m%s\x1b[37m", f, s);
}

fs.readdir(integration, (err, files) => {
    files.forEach(file => {
        var filePath = path.join(integration, file);
        if(!fs.lstatSync(filePath).isDirectory() && path.extname(filePath) === ".sh")
        {
            _process.exec('sh ' + filePath, {
                    cwd: path.join(__dirname, '../bin/dotnet')
                }, function(error, stdout, stderr) {
                    var expected = fs.readFileSync(path.join(integration, 'expected', file + ".log"), 'utf8');
                    var is_json = true;
                    try {
                        expected = JSON.parse(expected);
                    }
                    catch(e){
                        printWarning(file, "can't parse expected output as json");
                        is_json = false;
                        //process.exit(1);
                    }
                    fs.writeFileSync(path.join(integration, 'actual', file + ".log"), stdout);
                    var result = stdout;
                    if(is_json)
                    {
                        try {
                            result = JSON.parse(stdout);
                            if(JSON.stringify(result) == JSON.stringify(expected))
                            {
                                printSuccess(file, "PASSED!");
                            }
                            else
                            {
                                printError(file, "FAILED!");
                                _process.execSync('json-diff ' + path.join(integration, 'actual', file + ".log") + ' ' + path.join(integration, 'expected', file + ".log"), {stdio:[0,1,2]});
                                process.exit(1);
                            }
                        }
                        catch(e){
                            printError(file, "can't parse command output:");
                            console.log(stdout);
                            process.exit(1);
                        }
                    }
                    else
                    {
                        if(result == expected)
                        {
                            printSuccess(file, "PASSED!");
                        }
                        else
                        {
                            printError(file, "FAILED!");
                            _process.execSync('sh diff -a' + path.join(integration, 'actual', file + ".log") + ' ' + path.join(integration, 'expected', file + ".log"), {stdio:[0,1,2]});
                            process.exit(1);
                        }
                    }
                }
            );
        }
    });
})