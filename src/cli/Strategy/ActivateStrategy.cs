namespace Linterhub.Cli.Strategy
{
    using System.IO;
    using System.Linq;
    using Runtime;
    using Engine;
    using Engine.Exceptions;
    using Linterhub.Engine.Extensions;

    public class ActivateStrategy : IStrategy
    {
        public object Run(RunContext context, LinterEngine engine, LogManager log)
        {
            if (string.IsNullOrEmpty(context.Linter))
            {
                throw new LinterEngineException("Linter is not specified: " + context.Linter);
            }

            var projectConfigFile = Path.Combine(context.Project, ".linterhub.json");
            ExtConfig extConfig;
            
            if (!File.Exists(projectConfigFile))
            {
                extConfig = new ExtConfig();
            } 
            else 
            {
                using (var fs = File.Open(projectConfigFile, FileMode.Open))
                {
                    extConfig = fs.DeserializeAsJson<ExtConfig>();
                }
            }
 
            var linter = extConfig.Linters.FirstOrDefault(x => x.Name == context.Linter);
            if (linter != null)
            {
                linter.Active = context.Activate ? null : false;
            }
            else
            {
                extConfig.Linters.Add(new ExtConfig.ExtLint 
                {
                    Name = context.Linter,
                    Active = context.Activate,
                    Command = engine.Factory.GetArguments(context.Linter)
                });
            }

            var content = extConfig.SerializeAsJson();
            File.WriteAllText(projectConfigFile, content);
            return content;
        }
    }
}