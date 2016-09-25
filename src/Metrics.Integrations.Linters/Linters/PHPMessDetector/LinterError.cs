namespace Metrics.Integrations.Linters.Phpmd
{

    public class LinterError : LinterFileModel.Error
    { 
        public Location ErrorLocation;

        /// <summary>
        /// It contains hierarchical information for the convenience of finding errors
        /// </summary>
        public class Location
        {
            public string Class;
            public string Method;
            public string Package;
        }
    }
}