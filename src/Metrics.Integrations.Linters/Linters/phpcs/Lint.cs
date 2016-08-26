using System;
using System.IO;
using Metrics.Integrations.Linters;
using Newtonsoft.Json;

namespace Metrics.Integrations.Linters.Phpcs
{
    public class Lint : Linter
    {
        public override ILinterResult Parse(Stream stream){
            string json = new StreamReader(stream).ReadToEnd();
            return JsonConvert.DeserializeObject<LintResult>(json);
        }

        public override ILinterModel Map(ILinterResult result) {
            return (LintResult)result;
        }
    }
}