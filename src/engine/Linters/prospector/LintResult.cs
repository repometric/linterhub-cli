namespace Linterhub.Engine.Linters.prospector
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class LintResult : ILinterResult
    {
        [JsonProperty("messages")]
        public List<Message> MessagesList { get; set; }
    }
}