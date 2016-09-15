namespace Metrics.Integrations.Linters.Phpmetrics
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filename")]
        public string Filename;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("loc")]
        /// <summary>
        /// Number of lines of code
        /// </summary>
        public int LinesNumber;

        [JsonProperty("logicalLoc")]
        /// <summary>
        /// Number of logical lines of code
        /// </summary>
        public int LogicalLinesNumber;

        [JsonProperty("volume")]
        /// <summary>
        /// Halstead volume
        /// </summary>
        public double Volume;

        [JsonProperty("length")]
        /// <summary>
        /// Halstead length
        /// </summary>
        public int Length;

        [JsonProperty("vocabulary")]
        /// <summary>
        /// Halstead vocabulary
        /// </summary>
        public int Vocabulary;

        [JsonProperty("effort")]
        /// <summary>
        /// Halstead effort
        /// </summary>
        public double Effort;

        [JsonProperty("difficulty")]
        /// <summary>
        /// Halstead difficulty (average)
        /// </summary>
        public string sDifficulty;
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
        public int Time;

        [JsonProperty("bugs")]
        /// <summary>
        /// Estimated number of bugs (average by file)
        /// </summary>
        public double Bugs;

        [JsonProperty("intelligentContent")]
        public double IntelligentContent;

        [JsonProperty("maintainabilityIndexWithoutComment")]
        public string sMaintainabilityIndexWithoutComment;
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
        public string sMaintainabilityIndex;
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
        public double CommentWeight;

        [JsonProperty("instability")]
        public double Instability;

        [JsonProperty("afferentCoupling")]
        public double AfferentCoupling;

        [JsonProperty("efferentCoupling")]
        public double EfferentCoupling;

        [JsonProperty("noc")]
        public int NumberOfClasses;

        [JsonProperty("noca")]
        /// <summary>
        /// Number of abstract classes and interfaces
        /// </summary>
        public int NumberOfAbstractClasses;

        [JsonProperty("nocc")]
        /// <summary>
        /// Number of concrete classes and interfaces
        /// </summary>
        public int NumberOfConcreteClasses;

        [JsonProperty("noc-anon")]
        /// <summary>
        /// ?? I'm not sure
        /// </summary>
        public int NumberOfAnonymousClasses;

        [JsonProperty("noi")]
        public int NumberOfInterfaces;

        [JsonProperty("nom")]
        public int NumberOfMethods;

        [JsonProperty("cyclomaticComplexity")]
        public double CyclomaticComplexity;

        [JsonProperty("myerInterval")]
        /// <summary>
        /// Myer's interval indicates the distance between cyclomatic complexity number and the number of operators
        /// </summary>
        public string MyerInterval;

        [JsonProperty("myerDistance")]
        /// <summary>
        /// Myer's distance is derivated from Cyclomatic complexity
        /// </summary>
        public double MyerDistance;

        [JsonProperty("operators")]
        public int NumberOfOperators;

        [JsonProperty("lcom")]
        /// <summary>
        /// Lack of cohesion of methods (LCOM4)
        /// </summary>
        public double LackOfCohesion;

        [JsonProperty("sysc")]
        /// <summary>
        /// Total System complexity (Card and Agresti metric)
        /// </summary>
        public double TotalSystemComplexity;

        [JsonProperty("rsysc")]
        /// <summary>
        /// Relative System complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeSystemComplexity;

        [JsonProperty("dc")]
        /// <summary>
        /// Data Complexity (Card and Agresti metric)
        /// </summary>
        public double DataComplexity;

        [JsonProperty("rdc")]
        /// <summary>
        /// Relative data complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeDataComplexity;

        [JsonProperty("sc")]
        /// <summary>
        /// System complexity (Card and Agresti metric)
        /// </summary>
        public double SystemComplexity;

        [JsonProperty("rsc")]
        /// <summary>
        /// Relative structural complexity (Card and Agresti metric)
        /// </summary>
        public double RelativeStructuralComplexity;
    }
}