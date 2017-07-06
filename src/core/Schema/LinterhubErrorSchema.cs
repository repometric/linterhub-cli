namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// The problem definition if catch some errors
	/// </summary>
	public class LinterhubErrorSchema
	{
		
		/// <summary>
		/// Gets or sets the error code
		/// </summary>
		public int? Code { get; set; }
		
		/// <summary>
		/// Gets or sets the error title
		/// </summary>
		public string Title { get; set; }
		
		/// <summary>
		/// Gets or sets the error decription
		/// </summary>
		public string Description { get; set; }
	}
}