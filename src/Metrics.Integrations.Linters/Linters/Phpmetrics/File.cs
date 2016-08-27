namespace Metrics.Integrations.Linters.Phpmetrics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;

    public class File
    {
        [JsonProperty("filename")]
        public string Filename;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("loc")]
        // Number of lines of code
        public int LinesNumber;

        [JsonProperty("logicalLoc")]
        // Number of logical lines of code
        public int LogicalLinesNumber;

        [JsonProperty("volume")]
        // Halstead volume
        public double Volume;

        [JsonProperty("length")]
        // Halstead length
        public int Length;

        [JsonProperty("vocabulary")]
        // Halstead vocabulary
        public int Vocabulary;

        [JsonProperty("effort")]
        // Halstead effort
        public double Effort;

        [JsonProperty("difficulty")]
        // Halstead difficulty (average)
        public string sDifficulty;
        public double Difficulty{
            set{
                this.sDifficulty = Convert.ToString(value);
            }
            get{
                return Double.Parse(this.sDifficulty, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("time")]
        public int Time;

        [JsonProperty("bugs")]
        // Estimated number of bugs (average by file)
        public double Bugs;

        [JsonProperty("intelligentContent")]
        public double IntelligentContent;

        [JsonProperty("maintainabilityIndexWithoutComment")]
        public string sMaintainabilityIndexWithoutComment;
        public double MaintainabilityIndexWithoutComment{
            set{
                this.sMaintainabilityIndexWithoutComment = Convert.ToString(value);
            }
            get{
                return Double.Parse(this.sMaintainabilityIndexWithoutComment, CultureInfo.InvariantCulture);
            }
        }

        [JsonProperty("maintainabilityIndex")]
        public string sMaintainabilityIndex;
        // Maintainability Index (mi < 65: low ; 65 < mi < 85: correct; 85 < mi: good)
        public double MaintainabilityIndex{
            set{
                this.sMaintainabilityIndex = Convert.ToString(value);
            }
            get{
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
        // Number of abstract classes and interfaces
        public int NumberOfAbstractClasses;

        [JsonProperty("nocc")]
        // Number of concrete classes and interfaces
        public int NumberOfConcreteClasses;

        [JsonProperty("noc-anon")]
        // хрен его знает что это. похоже на анонимные классы. или пространства имен
        public int NumberOfAnonymousClasses;

        [JsonProperty("noi")]
        public int NumberOfInterfaces;

        [JsonProperty("nom")]
        public int NumberOfMethods;

        [JsonProperty("cyclomaticComplexity")]
        public double CyclomaticComplexity;

        [JsonProperty("myerInterval")]
        // Myer's interval indicates the distance between cyclomatic complexity number and the number of operators
        public string MyerInterval;

        [JsonProperty("myerDistance")]
        // Myer's distance is derivated from Cyclomatic complexity
        public double MyerDistance;

        [JsonProperty("operators")]
        public int NumberOfOperators;

        [JsonProperty("lcom")]
        // Lack of cohesion of methods (LCOM4)
        public double LackOfCohesion;

        [JsonProperty("sysc")]
        // Total System complexity (Card and Agresti metric)
        public double TotalSystemComplexity;

        [JsonProperty("rsysc")]
        // Relative System complexity (Card and Agresti metric)
        public double RelativeSystemComplexity;

        [JsonProperty("dc")]
        // Data Complexity (Card and Agresti metric)
        public double DataComplexity;

        [JsonProperty("rdc")]
        // Relative data complexity (Card and Agresti metric)
        public double RelativeDataComplexity;

        [JsonProperty("sc")]
        // System complexity (Card and Agresti metric)
        public double SystemComplexity;

        [JsonProperty("rsc")]
        // Relative structural complexity (Card and Agresti metric)
        public double RelativeStructuralComplexity;
    }
}