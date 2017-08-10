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
		/// Gets or sets the engine executable name; by default it's equal to engine name
		/// </summary>
		public string Executable { get; set; }
		
		/// <summary>
		/// Gets or sets way how to run engine. if engine installed locally for current project, than cant execute it with just engine name
		/// </summary>
		public bool? RunLocally { get; set; } = false;
		
		/// <summary>
		/// Gets or sets the engine description
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Gets or sets the engine url or homepage
		/// </summary>
		public string Url { get; set; }
		
		/// <summary>
		/// Gets or sets the engine version
		/// </summary>
		public string Version { get; set; }
		
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
		/// Gets or sets list of file names which could be treated as engine config
		/// </summary>
		public List<string> Configs = new List<string>();
		
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
		/// Gets or sets the engine output format
		/// </summary>
		public string Output { get; set; } = "json";
		
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
		/// Gets or sets the stdin configuration of engine. this property is specific for each engine. must be an empty object, if engine needs no params, but supports stdin
		/// </summary>
		public EngineOptions Stdin { get; set; }
		
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
			
			/// <summary>
			/// Gets or sets the package version
			/// </summary>
			public string Version { get; set; }
		}
	}
}