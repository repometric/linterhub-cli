namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// Version of linterhub
	/// </summary>
	public class LinterhubVersionSchema
	{
		
		/// <summary>
		/// Gets or sets version of linterhub
		/// </summary>
		public string Version { get; set; }
	}
}