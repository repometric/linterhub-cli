namespace Linterhub.Engine
{
    using System;
    using System.Collections.Generic;
    using Exceptions;
    using Linters;

    public class LinterFactory
    {
        public Registry.Record GetRecord(string name)
        {
            var record = Registry.Get(name);
            if (record == null)
            {
                throw new LinterNotFoundException(name);
            }

            return record;
        }

        public IEnumerable<Registry.Record> GetRecords()
        {
            return Registry.Get();
        }

        internal T Create<T>(Type type)
        {
            try 
            {
                return (T)Activator.CreateInstance(type);
            }
            catch (Exception exception)
            {
                throw new LinterReflectionException(exception);
            }
        }

        public ILinter Create(string name)
        {
            var record = GetRecord(name);
            var linter = Create<ILinter>(record.Linter);
            var proxy = new LinterProxy(linter);
            return proxy;
        }

        public ILinterArgs CreateArguments(string name)
        {
            var record = GetRecord(name);
            var args = Create<ILinterArgs>(record.Args);
            return args;
        }

        public string CreateCommand(ILinterArgs args, string path = "")
        {
            var builder = new ArgBuilder();
            return builder.Build(args, path);
        }

        public string GetArguments(string name, string path = "")
        {
            var record = GetRecord(name);
            var args = CreateArguments(name);
            return CreateCommand(args, path); 
        }
    }
}