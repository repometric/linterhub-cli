namespace Linterhub.Engine
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Exceptions;
    using Extensions;
    using Linters;

    /// <summary>
    /// Represents common methods for constructing objects for linters.
    /// </summary>
    public class LinterFactory
    {
        /// <summary>
        /// Get the linter definition from the registry.
        /// </summary>
        /// <param name="name">The linter name.</param>
        /// <returns>The linter <see cref="Registry.Record"/>.</returns>
        public Registry.Record GetRecord(string name)
        {
            var record = Registry.Get(name);
            if (record == null)
            {
                throw new LinterNotFoundException(name);
            }

            return record;
        }

        /// <summary>
        /// Get all records from the registry.
        /// </summary>
        /// <returns>The collection of linter <see cref="Registry.Record"/>.</returns>
        public IEnumerable<Registry.Record> GetRecords()
        {
            return Registry.Get();
        }

        /// <summary>
        /// Create an instance of the specified type using that type's default constructor. 
        /// </summary>
        /// <typeparam name="T">The expected type.</typeparam>
        /// <param name="type">The type of object to create.</param>
        /// <returns>A reference to the newly created object.</returns>
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

        /// <summary>
        /// Create a linter.
        /// </summary>
        /// <param name="name">The linter name.</param>
        /// <returns>The instance of linter.</returns>
        public ILinter Create(string name)
        {
            var record = GetRecord(name);
            var linter = Create<ILinter>(record.Linter);
            var proxy = new LinterProxy(linter);
            return proxy;
        }

        /// <summary>
        /// Create a linter arguments.
        /// </summary>
        /// <param name="name">The linter name.</param>
        /// <returns>The instance of linter arguments.</returns>
        public ILinterArgs CreateArguments(string name)
        {
            var record = GetRecord(name);
            var args = Create<ILinterArgs>(record.Args);
            return args;
        }

        public ILinterModel CreateModel(string name, Stream stream, ILinterArgs args)
        {
            var linter = Create(name);
            var raw = linter.Parse(stream, args);
            var map = linter.Map(raw);
            return map;
        }

        public ILinterArgs CreateArguments(string name, Stream stream)
        {
            var args = CreateArguments(name);
            return stream.DeserializeAsJson<ILinterArgs>(args.GetType());
        }

        public string BuildCommand(ILinterArgs args, string workDir, string path, ArgMode mode)
        {
            var builder = new ArgBuilder();
            return builder.Build(args, workDir, path, mode);
        }

        public string BuildCommand(string name, string workDir, string path, ArgMode mode)
        {
            var args = CreateArguments(name);
            return BuildCommand(args, workDir, path, mode); 
        }
    }
}