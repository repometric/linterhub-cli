namespace Metrics.Integrations.Linters.Phpmetrics
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    public class File : LinterFileModel.File
    {
        [JsonProperty("filename")]
        public string Filename {
            get
            {
                return Path;
            }
            set
            {
                Path = value;
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("loc")]
        /// <summary>
        /// Number of lines of code
        /// </summary>
        public int LinesNumber { get; set; }

        [JsonProperty("logicalLoc")]
        /// <summary>
        /// Number of logical lines of code
        /// </summary>
        public int LogicalLinesNumber { get; set; }

        [JsonProperty("volume")]
        /// <summary>
        /// Halstead volume
        /// </summary>
        public double Volume { get; set; }

        [JsonProperty("length")]
        /// <summary>
        /// Halstead length
        /// </summary>
        public int Length { get; set; }

        [JsonProperty("vocabulary")]
        /// <summary>
        /// Halstead vocabulary
        /// </summary>
        public int Vocabulary { get; set; }

        [JsonProperty("effort")]
        /// <summary>
        /// Halstead effort
        /// </summary>
        public double Effort { get; set; }

        [JsonProperty("difficulty")]
        /// <summary>
        /// Halstead difficulty (average)
        /// </summary>
        public string sDifficulty { get; set; }
        public double Difficulty
        {
            set
            {
                this.sDifficulty = Convert.ToString(value);
            }
            get
            {
                return Double.Parse(this.sDifficulty, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("bugs")]
        /// <summary>
        /// Estimated number of bugs (average by file)
        /// </summary>
        public double Bugs { get; set; }

        [JsonProperty("intelligentContent")]
        public double IntelligentContent;

        [JsonProperty("maintainabilityIndexWithoutComment")]
        public string sMaintainabilityIndexWithoutComment { get; set; }
        public double MaintainabilityIndexWithoutComment
        {
            set
            {
                this.sMaintainabilityIndexWithoutComment = Convert.ToString(value);
            }
            get
            {
                return Double.Parse(this.sMaintainabilityIndexWithoutComment, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("maintainabilityIndex")]
        public string sMaintainabilityIndex { get; set; }
        /// <summary>
        /// Maintainability Index (mi < 65: low ; 65 < mi < 85: correct; 85 < mi: good)
        /// </summary>
        public double MaintainabilityIndex
        {
            set
            {
                this.sMaintainabilityIndex = Convert.ToString(value);
            }
            get
            {
                return Double.Parse(this.sMaintainabilityIndex, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("commentWeight")]
        public double CommentWeight { get; set; }

        [JsonProperty("instability")]
        public double Instability { get; set; }

        [JsonProperty("afferentCoupling")]
        public double AfferentCoupling { get; set; }

        [JsonProperty("efferentCoupling")]
        public double EfferentCoupling { get; set; }

        [JsonProperty("noc")]
        public int NumberOfClasses { get; set; }

        [JsonProperty("noca")]
        /// <summary>
        /// Number of abstract classes and interfaces
        /// </summary>
        public int NumberOfAbstractClasses { get; set; }

        [JsonProperty("nocc")]
        /// <summary>
        /// Number of concrete classes and interfaces
        /// </summary>
        public int NumberOfConcreteClasses { get; set; }

        [JsonProperty("noc-anon")]
        /// <summary>
        /// ?? I'm not sure
        /// </summary>
        public int NumberOfAnonymousClasses { get; set; }

        [JsonProperty("noi")]
        public int NumberOfInterfaces { get; set; }

        [JsonProperty("nom")]
        public int NumberOfMethods { get; set; }

        [JsonProperty("cyclomaticComplexity")]
        public double CyclomaticComplexity { get; set; }

        [JsonProperty("myerInterval")]
        /// <summary>
        /// Myer's interval indicates the distance between cyclomatic complexity number and the number of operators
        /// </summary>
        public string MyerInterval { get; set; }

        [JsonProperty("myerDistance")]
        /// <summary>
        /// Myer's distance is derivated from Cyclomatic complexity
        /// </summary>
        public double MyerDistance { get; set; }

        [JsonProperty("operators")]
        public int NumberOfOperators { get; set; }

        [JsonProperty("lcom")]
        /// <summary>
        /// Lack of cohesion of methods (LCOM4)
        /// </summary>
        public double LackOfCohesion { get; set; }

        [JsonProperty("sysc")]
        /// <summary>
        /// Total System complexity (Card and Agresti metric)
        /// </summary>
        public double TotalSystemComplexity { get; set; }

        [JsonProperty("rsysc")]
        /// <summary>
        /// Relative System complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeSystemComplexity { get; set; }

        [JsonProperty("dc")]
        /// <summary>
        /// Data Complexity (Card and Agresti metric)
        /// </summary>
        public double DataComplexity { get; set; }

        [JsonProperty("rdc")]
        /// <summary>
        /// Relative data complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeDataComplexity { get; set; }

        [JsonProperty("sc")]
        /// <summary>
        /// System complexity (Card and Agresti metric)
        /// </summary>
        public double SystemComplexity { get; set; }

        [JsonProperty("rsc")]
        /// <summary>
        /// Relative structural complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeStructuralComplexity { get; set; }
    }
}