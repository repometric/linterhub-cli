namespace Linterhub.Cli.Strategy
{
    /// <summary>
    /// The strategy interface.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Run strategy.
        /// </summary>
        /// <param name="locator">The service locator.</param>
        /// <returns>Run results.</returns>
        object Run(ServiceLocator locator);
    }
}