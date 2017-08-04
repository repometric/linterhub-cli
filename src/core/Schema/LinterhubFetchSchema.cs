namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// Fetch engines output is list of possible used engines
	/// </summary>
	public class LinterhubFetchSchema : List<LinterhubFetchSchema.ResultType>
	{
		
		/// <summary>
		/// Founded engine
		/// </summary>
		public class ResultType
		{
			
			/// <summary>
			/// Gets or sets the engine name
			/// </summary>
			public string Name { get; set; }
			
			/// <summary>
			/// Gets or sets the way how engine founded
			/// </summary>
			[JsonConverter(typeof(StringEnumConverter))]
			public enum FoundType
			{
				sourceExtension,
				engineConfig,
				projectConfig
			}
			
			public FoundType Found { get; set; }
		}
	}
}