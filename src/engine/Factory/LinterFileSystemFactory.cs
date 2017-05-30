namespace Linterhub.Engine.Factory
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Linterhub.Engine.Extensions;
    using Linterhub.Engine.Schema;
    using Newtonsoft.Json.Schema;

    public interface ILinterFactory
    {
        EngineSchema GetSchema(string name);
        LinterOptionsSchema GetOptionsSchema(string name);
        LinterSpecification GetSpecification(string name);
        IEnumerable<string> GetNames();
        IEnumerable<LinterSpecification> GetSpecifications();
    }

    public class LinterFileSystemFactory : ILinterFactory
    {
        protected const string LinterDefinitionFile = "engine.json";
        protected const string LinterArgsSchema = "args.json";

        protected string Folder { get; }

        protected IEnumerable<LinterSpecification> Specifications;

        public LinterFileSystemFactory(string folder)
        {
            Folder = folder;
        }

        private string GetSchemaPath(string name)
        {
            return Path.Combine(Folder, name, LinterDefinitionFile);
        }

        private string GetOptionsSchemaPath(string name)
        {
            return Path.Combine(Folder, name, LinterArgsSchema);
        }

        public EngineSchema GetSchema(string name)
        {
            var path = GetSchemaPath(name);
            var content = File.ReadAllText(path);
            return content.DeserializeAsJson<EngineSchema>();
        }

        public LinterOptionsSchema GetOptionsSchema(string name)
        {
            var path = GetOptionsSchemaPath(name);
            var content = File.ReadAllText(path);
            var schema = JSchema.Parse(content);
            var schemaOptions = schema.ExtensionData["definitions"]["options"]["properties"];
            return schemaOptions.ToObject<LinterOptionsSchema>();
        }

        public LinterSpecification GetSpecification(string name)
        {
            var linterName = name.ToLower();
            if (Specifications != null)
            {
                return Specifications.First(x => x.Schema.Name.ToLower() == linterName);
            } 

            var schema = GetSchema(linterName);
            var optionsSchema = GetOptionsSchema(linterName);
            return new LinterSpecification(schema, optionsSchema);
        }

        public IEnumerable<string> GetNames()
        {
            return Directory
                .EnumerateDirectories(Folder)
                .Select(x => x.Replace(Folder, string.Empty)
                .TrimStart(Path.DirectorySeparatorChar))
                .Where(x => x != ".schema");
        }

        public IEnumerable<LinterSpecification> GetSpecifications()
        {
            Specifications = Specifications ?? GetNames().Select(GetSpecification).ToList();
            return Specifications;
        }
    }
}