namespace Metrics.Integrations.Linters.pmd
{

    public class LinterError : LinterFileModel.Error
    { 
        public Location ErrorLocation;

        public string Variable { get; set; }

        public string ExternalInfoUrl { get; set; }

        public int Priority { get; set; }
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