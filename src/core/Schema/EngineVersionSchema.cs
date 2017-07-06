namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// Engine version output is an array of objects describing the state of the engines
	/// </summary>
	public class EngineVersionSchema
	{
		
		/// <summary>
		/// Represents install/version request result
		/// </summary>
		public class ResultType
		{
			
			/// <summary>
			/// Gets or sets installation status
			/// </summary>
			public bool? Installed { get; set; }
			
			/// <summary>
			/// Gets or sets error message if cant install engine
			/// </summary>
			public string Message { get; set; }
			
			/// <summary>
			/// Gets or sets engine name
			/// </summary>
			public string Name { get; set; }
			
			/// <summary>
			/// Gets or sets engine version
			/// </summary>
			public string Version { get; set; }
			
			/// <summary>
			/// Gets or sets list of dependencies
			/// </summary>
			public List<ResultType> Packages = new List<ResultType>();
		}
	}
}