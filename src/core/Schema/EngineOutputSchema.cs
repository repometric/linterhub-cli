namespace Linterhub.Core.Schema
{
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	
	/// <summary>
	/// Engine output is an array of analysis results
	/// </summary>
	public class EngineOutputSchema
	{
		
		/// <summary>
		/// Represents analysis result
		/// </summary>
		public class ResultType
		{
			
			/// <summary>
			/// Gets or sets the path relative to the analysis root
			/// </summary>
			public string Path { get; set; }
			
			/// <summary>
			/// Gets or sets list of messages in the path
			/// </summary>
			public List<MessageType> Messages = new List<MessageType>();
		}
		
		/// <summary>
		/// Represents analysis message
		/// </summary>
		public class MessageType
		{
			
			/// <summary>
			/// Gets or sets the short description of the message
			/// </summary>
			public string Message { get; set; }
			
			/// <summary>
			/// Gets or sets the explanatory text of the message
			/// </summary>
			public string Description { get; set; }
			
			/// <summary>
			/// Gets or sets the severity of the message
			/// </summary>
			[JsonConverter(typeof(StringEnumConverter))]
			public enum SeverityType
			{
				verbose,
				hint,
				information,
				warning,
				error
			}
			
			public SeverityType Severity { get; set; }
			
			/// <summary>
			/// Gets or sets the line where the message is located
			/// </summary>
			public int? Line { get; set; }
			
			/// <summary>
			/// Gets or sets the end line where the message is located (the same as line by default)
			/// </summary>
			public int? LineEnd { get; set; }
			
			/// <summary>
			/// Gets or sets the column where the message is located
			/// </summary>
			public int? Column { get; set; }
			
			/// <summary>
			/// Gets or sets the end column where the message is located
			/// </summary>
			public int? ColumnEnd { get; set; }
			
			/// <summary>
			/// Gets or sets the id of the rule that produced the message
			/// </summary>
			public string RuleId { get; set; }
			
			/// <summary>
			/// Gets or sets the name of the rule that produced the message
			/// </summary>
			public string RuleName { get; set; }
			
			/// <summary>
			/// Gets or sets the namespace of the rule that produced the message
			/// </summary>
			public string RuleNs { get; set; }
		}
	}
}