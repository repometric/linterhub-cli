namespace Linterhub.Engine
{
    /// <summary>
    /// Represents custom generator for linter arguments.
    /// </summary>
    public interface IArgProvider
    {
        /// <summary>
        /// Build linter arguments.
        /// </summary>
        /// <returns></returns>
        string Build();
    }
}
