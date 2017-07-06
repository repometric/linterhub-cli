namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// The meta information of engine
	/// </summary>
	public class EngineSchema
	{
		
		/// <summary>
		/// Gets or sets the engine name
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets custom engine name. use this property when need to change default engine name on execute
		/// </summary>
		public string CustomName { get; set; }
		
		/// <summary>
		/// Gets or sets the engine description
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Gets or sets the engine url or homepage
		/// </summary>
		public string Url { get; set; }
		
		/// <summary>
		/// Gets or sets the engine version (expected)
		/// </summary>
		public VersionType Version { get; set; }
		
		/// <summary>
		/// Gets or sets the list of supported languages
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public enum LanguagesType
		{
			coffeescript,
			css,
			html,
			javascript,
			json,
			jsx,
			sass,
			typescript
		}
		
		public List<LanguagesType> Languages = new List<LanguagesType>();
		
		/// <summary>
		/// Gets or sets common file extensions parsed by engine
		/// </summary>
		public List<string> Extensions = new List<string>();
		
		/// <summary>
		/// Gets or sets the engine license
		/// </summary>
		public string License { get; set; }
		
		/// <summary>
		/// Gets or sets the engine requirements
		/// </summary>
		public List<RequirementType> Requirements = new List<RequirementType>();
		
		/// <summary>
		/// Gets or sets the engine areas
		/// </summary>
		public List<string> Areas = new List<string>();
		
		/// <summary>
		/// Gets or sets can use masks for multiple files analyze
		/// </summary>
		public bool? AcceptMask { get; set; } = true;
		
		/// <summary>
		/// Gets or sets posstfix in terminal (normaly post processor)
		/// </summary>
		public string Postfix { get; set; }
		
		/// <summary>
		/// Gets or sets delimiter for options (space by default)
		/// </summary>
		public string OptionsDelimiter { get; set; } = " ";
		
		/// <summary>
		/// Gets or sets success exit code
		/// </summary>
		public int? SuccessCode { get; set; }
		
		/// <summary>
		/// Gets or sets a value indicating whether engine is active
		/// </summary>
		public bool? Active { get; set; } = true;
		
		/// <summary>
		/// Gets or sets the default configuration of engine. this property is specific for each engine
		/// </summary>
		public EngineOptions Defaults { get; set; }
		
		/// <summary>
		/// Gets or sets support of stdin analyze
		/// </summary>
		public StdinType Stdin { get; set; }
		
		/// <summary>
		/// The engine version (expected)
		/// </summary>
		public class VersionType
		{
			
			/// <summary>
			/// Gets or sets package version
			/// </summary>
			public string Package { get; set; }
			
			/// <summary>
			/// Gets or sets local version
			/// </summary>
			public string Local { get; set; }
		}
		
		/// <summary>
		/// The engine dependency
		/// </summary>
		public class RequirementType
		{
			
			/// <summary>
			/// Gets or sets the manager for dependency
			/// </summary>
			[JsonConverter(typeof(StringEnumConverter))]
			public enum ManagerType
			{
				system,
				url,
				composer,
				gem,
				npm,
				pip
			}
			
			public ManagerType Manager { get; set; }
			
			/// <summary>
			/// Gets or sets the package name
			/// </summary>
			public string Package { get; set; }
		}
		
		/// <summary>
		/// Support of stdin analyze
		/// </summary>
		public class StdinType
		{
			
			/// <summary>
			/// Gets or sets supports stdin or not
			/// </summary>
			public bool? Available { get; set; }
			
			/// <summary>
			/// Gets or sets the stdin configuration of engine. this property is specific for each engine
			/// </summary>
			public EngineOptions Arguments { get; set; }
		}
	}
}