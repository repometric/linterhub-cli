namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// The configuration for linterhub engines
	/// </summary>
	public class LinterhubConfigSchema
	{
		
		/// <summary>
		/// Gets or sets list of engines configurations
		/// </summary>
		public List<ConfigurationType> Engines = new List<ConfigurationType>();
		
		/// <summary>
		/// Gets or sets the list of global ignore rules
		/// </summary>
		public List<IgnoreType> Ignore = new List<IgnoreType>();
		
		/// <summary>
		/// The engine configuration
		/// </summary>
		public class ConfigurationType
		{
			
			/// <summary>
			/// Gets or sets the engine name
			/// </summary>
			public string Name { get; set; }
			
			/// <summary>
			/// Gets or sets whether engine is enabled
			/// </summary>
			public bool? Active { get; set; } = true;
			
			/// <summary>
			/// Gets or sets whether engine is installed locally or globally
			/// </summary>
			public bool Locally { get; set; } = true;
			
			/// <summary>
			/// Gets or sets the engine specific configuration
			/// </summary>
			public object Config { get; set; }
			
			/// <summary>
			/// Gets or sets the list of rules for ignoring engine results
			/// </summary>
			public List<IgnoreType> Ignore = new List<IgnoreType>();
		}
		
		/// <summary>
		/// The configuration for ignoring engine results
		/// </summary>
		public class IgnoreType
		{
			
			/// <summary>
			/// Gets or sets the path mask
			/// </summary>
			public string Mask { get; set; }
			
			/// <summary>
			/// Gets or sets the line nubmer
			/// </summary>
			public int? Line { get; set; }
			
			/// <summary>
			/// Gets or sets the rule id
			/// </summary>
			public string RuleId { get; set; }
		}
	}
}