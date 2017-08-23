namespace Linterhub.Core.Factory
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Utils;
    using Exceptions;
    using Schema;
    using Newtonsoft.Json.Schema;

    public interface IEngineFactory
    {
        EngineSchema GetSchema(string name);
        EngineOptionsSchema GetOptionsSchema(string name);
        EngineSpecification GetSpecification(string name);
        IEnumerable<string> GetNames();
        IEnumerable<EngineSpecification> GetSpecifications();
    }

    public class EngineFileSystemFactory : IEngineFactory
    {
        protected const string EngineDefinitionFile = "engine.json";
        protected const string EngineArgsSchema = "args.json";

        protected string Folder { get; }

        protected IEnumerable<EngineSpecification> Specifications;

        public EngineFileSystemFactory(string folder)
        {
            Folder = folder;
        }

        private string GetSchemaPath(string name)
        {
            return Path.Combine(Folder, name, EngineDefinitionFile);
        }

        private string GetOptionsSchemaPath(string name)
        {
            return Path.Combine(Folder, name, EngineArgsSchema);
        }

        public EngineSchema GetSchema(string name)
        {
            var path = GetSchemaPath(name);
            var content = "";
            try
            {
                content = File.ReadAllText(path);
            }
            catch (System.IO.IOException)
            {
                throw new EngineIsMissingException(name);
            }
            return content.DeserializeAsJson<EngineSchema>();
        }

        public EngineOptionsSchema GetOptionsSchema(string name)
        {
            var path = GetOptionsSchemaPath(name);
            var content = "";
            try
            {
                content = File.ReadAllText(path);
            }
            catch (System.IO.IOException)
            {
                throw new EngineIsMissingException(name);
            }
            var schema = JSchema.Parse(content);
            var schemaOptions = schema.ExtensionData["definitions"]["options"]["properties"];
            return schemaOptions.ToObject<EngineOptionsSchema>();
        }

        public EngineSpecification GetSpecification(string name)
        {
            var engineName = name.ToLower();
            if (Specifications != null)
            {
                return Specifications.First(x => x.Schema.Name.ToLower() == engineName);
            } 

            var schema = GetSchema(engineName);
            var optionsSchema = GetOptionsSchema(engineName);
            return new EngineSpecification(schema, optionsSchema);
        }

        public IEnumerable<string> GetNames()
        {
            return Directory
                .EnumerateDirectories(Folder)
                .Select(x => x.Replace(Folder, string.Empty)
                .TrimStart(Path.DirectorySeparatorChar))
                .Where(x => x != ".schema");
        }

        public IEnumerable<EngineSpecification> GetSpecifications()
        {
            Specifications = Specifications ?? GetNames().Select(GetSpecification).ToList();
            return Specifications;
        }
    }
}