namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// Linterhub output is an array of engines results
	/// </summary>
	public class LinterhubOutputSchema : List<LinterhubOutputSchema.ResultType>
	{
		
		/// <summary>
		/// The engine result
		/// </summary>
		public class ResultType
		{
			
			/// <summary>
			/// Gets or sets the engine name that performed analysis
			/// </summary>
			public string Engine { get; set; }
			
			/// <summary>
			/// Gets or sets the analysis result
			/// </summary>
			public EngineOutputSchema Result = new EngineOutputSchema();
			
			/// <summary>
			/// Gets or sets the problem definition if analysis is not possible
			/// </summary>
			public LinterhubErrorSchema Error { get; set; }
		}
	}
}