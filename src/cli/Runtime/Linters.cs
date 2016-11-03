namespace Linterhub.Cli.Runtime
{
        using Newtonsoft.Json.Linq;

        public class Linters
        {
            public Linters()
            {
                linters = new Linter[0];
            }

            public Linter[] linters;
            public JObject platforms;
            public JObject licenses;
            public JObject dockers;
            public class Linter
            {
                public string name;
                public string version;
                public string description;
                public string url;
                public string languages;
                public string license;
                public string platform;
                public JObject config;
            }
        }
}