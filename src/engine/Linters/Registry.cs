namespace Linterhub.Engine.Linters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Registry
    {
        public class Record
        {
            public string Name { get; set; }
            public Type Linter { get; set; }
            public Type Args { get; set; }
            public Type Result { get; set; }
            public Type Model { get; set; }
            public string ArgsDefault { get; set; }
        }

        public static Record Get(string name)
        {
            return Get().SingleOrDefault(x => x.Name == name);
        }

        public static IEnumerable<Record> Get()
        {
            return linters;
        }

        public static void Register(Record record)
        {
            linters.Add(record);
        }

        private static List<Record> linters = new List<Record>
        {
            new Record
            {
                Name = "phpcheckstyle",
                Linter = typeof(Phpcs.Lint),
                Args = typeof(Phpcs.LintArgs),
                Result = typeof(Phpcs.LintResult),
                Model = typeof(LinterFileModel)
            },
            new Record
            {
                Name = "phpmd",
                Linter = typeof(Phpmd.Lint),
                Args = typeof(Phpmd.LintArgs),
                Result = typeof(Phpmd.LintResult),
                Model = typeof(LinterFileModel)
            },
            new Record
            {
                Name = "phpmetrics",
                Linter = typeof(Phpmetrics.Lint),
                Args = typeof(Phpmetrics.LintArgs),
                Result = typeof(Phpmetrics.LintResult),
                Model = typeof(Phpmetrics.LintResult)
            },
            new Record
            {
                Name = "phpsa",
                Linter = typeof(Phpsa.Lint),
                Args = typeof(Phpsa.LintArgs),
                Result = typeof(Phpsa.LintResult),
                Model = typeof(Phpsa.LintResult)
            },
            new Record
            {
                Name = "phpcpd",
                Linter = typeof(Phpcpd.Lint),
                Args = typeof(Phpcpd.LintArgs),
                Result = typeof(Phpcpd.LintResult),
                Model = typeof(Phpcpd.LintResult)
            },
            new Record
            {
                Name = "php-assumptions",
                Linter = typeof(PhpAssumptions.Lint),
                Args = typeof(PhpAssumptions.LintArgs),
                Result = typeof(PhpAssumptions.LintResult),
                Model = typeof(PhpAssumptions.LintResult)
            },
            new Record
            {
                Name = "phpcodefixer",
                Linter = typeof(Phpcf.Lint),
                Args = typeof(Phpcf.LintArgs),
                Result = typeof(Phpcf.LintResult),
                Model = typeof(Phpcf.LintResult)
            },
            new Record
            {
                Name = "htmlhint",
                Linter = typeof(htmlhint.Lint),
                Args = typeof(htmlhint.LintArgs),
                Result = typeof(htmlhint.LintResult),
                Model = typeof(htmlhint.LintResult)
            },
            /*new Record
            {
                Name = "phpcheckstyle",
                Linter = typeof(phpcheckstyle.Lint),
                Args = typeof(phpcheckstyle.LintArgs),
                Result = typeof(phpcheckstyle.LintResult),
                Model = typeof(phpcheckstyle.LintResult)
            },*/
            new Record
            {
                Name = "coffeelint",
                Linter = typeof(coffeelint.Lint),
                Args = typeof(coffeelint.LintArgs),
                Result = typeof(coffeelint.LintResult),
                Model = typeof(coffeelint.LintResult)
            },
            new Record
            {
                Name = "csslint",
                Linter = typeof(csslint.Lint),
                Args = typeof(csslint.LintArgs),
                Result = typeof(csslint.LintResult),
                Model = typeof(csslint.LintResult),
                ArgsDefault = "csslint . --format=json"
            },
            new Record
            {
                Name = "jshint",
                Linter = typeof(jshint.Lint),
                Args = typeof(jshint.LintArgs),
                Result = typeof(jshint.LintResult),
                Model = typeof(jshint.LintResult),
                ArgsDefault = "jshint . --reporter=jslint"
            },
            new Record
            {
                Name = "jslint",
                Linter = typeof(jslint.Lint),
                Args = typeof(LinterPlainArgs),
                Result = typeof(jslint.LintResult),
                Model = typeof(LinterFileModel),
                ArgsDefault = "jslint --json **/*.js"
            },
            new Record
            {
                Name = "eslint",
                Linter = typeof(eslint.Lint),
                Args = typeof(LinterPlainArgs),
                Result = typeof(eslint.LintResult),
                Model = typeof(LinterFileModel)
            },
            new Record
            {
                Name = "pep8",
                Linter = typeof(pep8.Lint),
                Args = typeof(pep8.LintArgs),
                Result = typeof(pep8.LintResult),
                Model = typeof(LinterFileModel)
            }
        };
    }
}
