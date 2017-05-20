const fs = require('fs');
const path = require('path');
const _process = require('child_process');
//const compareObj = require('compare-obj');
const JsDiff = require('diff');

var integration = path.join(__dirname, 'integration');

function printError(f, s)
{
    console.log("\x1b[37m%s: \x1b[31m%s\x1b[37m", f, s);
}

function printSuccess(f, s)
{
    console.log("\x1b[37m%s: \x1b[32m%s\x1b[37m", f, s);
}

function printWarning(f, s)
{
    console.log("\x1b[37m%s: \x1b[33m%s\x1b[37m", f, s);
}

fs.readdir(integration, (err, files) => {
    files.forEach(file => {
        var filePath = path.join(integration, file);
        if(!fs.lstatSync(filePath).isDirectory() && path.extname(filePath) === ".sh")
        {
            var actualDir = path.join(integration, 'actual');
            if (!fs.existsSync(actualDir)){
                fs.mkdirSync(actualDir);
            }
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
                    fs.writeFileSync(path.join(actualDir, file + ".log"), stdout);
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
                                var diff = JsDiff.diffLines(JSON.stringify(result, null, 2), JSON.stringify(expected, null, 2));
                            
                                diff.forEach(function(part){
                                    if(part.removed)
                                        console.log("\x1b[31m%s\x1b[37m", part.value);
                                    else if(part.added)
                                        console.log("\x1b[32m%s\x1b[37m", part.value);
                                    else 
                                        console.log("%s", part.value);
                                });
                                //console.log(compareObj(result, expected));
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
                            
                            var diff = JsDiff.diffLines(result, expected);
                            
                            diff.forEach(function(part){
                                if(part.removed)
                                    console.log("\x1b[31m%s\x1b[37m", part.value);
                                if(part.added)
                                    console.log("\x1b[32m%s\x1b[37m", part.value);
                            });
                            process.exit(1);
                        }
                    }
                }
            );
        }
    });
})