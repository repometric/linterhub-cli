namespace Metrics.Integrations.Linters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Extensions;

    // sh linterhub.sh --mode analyze --name jslint:"jslint *.js":output.txt --path /project/path --session true
    public class DockerEngine
    {
        public string LinterHubPath;

        public string OutputFile { get; set; }

        public string Build<T>(T configuration, String name, String Filename)
        {
            LinterHubPath = ((string)System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase)).ToString().Substring(6) + "\\..\\LinterHub\\";
            //LinterHubPath = @"C:\Users\xsofa\Documents\LinterHub\";
            var propertyInfo = configuration.GetType().GetProperty("TestPath");
            var ProjectPath = (string)propertyInfo.GetValue(configuration, null);
            OutputFile = Filename;
            return "linterhub.sh --mode analyze --name " + name + ":\"" + (new ArgBuilder()).Build(configuration) + "\":" + OutputFile + " --path " + ProjectPath + " --session true --clean true --env true";
        }
    }
}
