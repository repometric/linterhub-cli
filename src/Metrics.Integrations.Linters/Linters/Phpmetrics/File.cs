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

        /// <summary>
        /// Number of lines of code
        /// </summary>
        [JsonProperty("loc")]
        public int LinesNumber { get; set; }

        /// <summary>
        /// Number of logical lines of code
        /// </summary>
        [JsonProperty("logicalLoc")]
        public int LogicalLinesNumber { get; set; }

        /// <summary>
        /// Halstead volume
        /// </summary>
        [JsonProperty("volume")]
        public double Volume { get; set; }

        /// <summary>
        /// Halstead length
        /// </summary>
        [JsonProperty("length")]
        public int Length { get; set; }

        /// <summary>
        /// Halstead vocabulary
        /// </summary>
        [JsonProperty("vocabulary")]
        public int Vocabulary { get; set; }

        /// <summary>
        /// Halstead effort
        /// </summary>
        [JsonProperty("effort")]
        public double Effort { get; set; }

        /// <summary>
        /// Halstead difficulty (average)
        /// </summary>
        [JsonProperty("difficulty")]
        public string sDifficulty { get; set; }

        public double Difficulty
        {
            set
            {
                sDifficulty = Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            get
            {
                return double.Parse(sDifficulty, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("time")]
        public int Time { get; set; }

        /// <summary>
        /// Estimated number of bugs (average by file)
        /// </summary>
        [JsonProperty("bugs")]
        public double Bugs { get; set; }

        [JsonProperty("intelligentContent")]
        public double IntelligentContent;

        [JsonProperty("maintainabilityIndexWithoutComment")]
        public string sMaintainabilityIndexWithoutComment { get; set; }

        public double MaintainabilityIndexWithoutComment
        {
            set
            {
                sMaintainabilityIndexWithoutComment = Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            get
            {
                return double.Parse(sMaintainabilityIndexWithoutComment, CultureInfo.InvariantCulture);
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
                sMaintainabilityIndex = Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            get
            {
                return double.Parse(sMaintainabilityIndex, CultureInfo.InvariantCulture);
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

        /// <summary>
        /// Number of abstract classes and interfaces
        /// </summary>
        [JsonProperty("noca")]
        public int NumberOfAbstractClasses { get; set; }

        /// <summary>
        /// Number of concrete classes and interfaces
        /// </summary>
        [JsonProperty("nocc")]
        public int NumberOfConcreteClasses { get; set; }

        /// <summary>
        /// ?? I'm not sure
        /// </summary>
        [JsonProperty("noc-anon")]
        public int NumberOfAnonymousClasses { get; set; }

        [JsonProperty("noi")]
        public int NumberOfInterfaces { get; set; }

        [JsonProperty("nom")]
        public int NumberOfMethods { get; set; }

        [JsonProperty("cyclomaticComplexity")]
        public double CyclomaticComplexity { get; set; }

        /// <summary>
        /// Myer's interval indicates the distance between cyclomatic complexity number and the number of operators
        /// </summary>
        [JsonProperty("myerInterval")]
        public string MyerInterval { get; set; }

        /// <summary>
        /// Myer's distance is derivated from Cyclomatic complexity
        /// </summary>
        [JsonProperty("myerDistance")]
        public double MyerDistance { get; set; }

        [JsonProperty("operators")]
        public int NumberOfOperators { get; set; }

        /// <summary>
        /// Lack of cohesion of methods (LCOM4)
        /// </summary>
        [JsonProperty("lcom")]
        public double LackOfCohesion { get; set; }

        /// <summary>
        /// Total System complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("sysc")]
        public double TotalSystemComplexity { get; set; }

        /// <summary>
        /// Relative System complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("rsysc")]
        public double RelativeSystemComplexity { get; set; }

        /// <summary>
        /// Data Complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("dc")]
        public double DataComplexity { get; set; }

        /// <summary>
        /// Relative data complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("rdc")]
        public double RelativeDataComplexity { get; set; }

        /// <summary>
        /// System complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("sc")]
        public double SystemComplexity { get; set; }

        /// <summary>
        /// Relative structural complexity (Card and Agresti metric)
        /// </summary>
        [JsonProperty("rsc")]
        public double RelativeStructuralComplexity { get; set; }
    }
}