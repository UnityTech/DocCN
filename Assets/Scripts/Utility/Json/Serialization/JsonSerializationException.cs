using System;

namespace Unity.DocZh.Utility.Json.Serialization
{
	public sealed class JsonSerializationException : Exception
	{
		public ErrorType Type { get; private set; } 
		public JsonSerializationException()
			: base(GetDefaultMessage(ErrorType.Unknown)) { }

		public JsonSerializationException(ErrorType type)
			: this(GetDefaultMessage(type), type) { }

		public JsonSerializationException(string message, ErrorType type)
			: base(message)
		{
			this.Type = type;
		}

		private static string GetDefaultMessage(ErrorType type)
		{
			switch (type)
			{
				case ErrorType.InvalidNumber:
					return "The value been serialized contains an invalid number value (NAN, infinity).";

				case ErrorType.InvalidValueType:
					return "The value been serialized contains (or is) an invalid JSON type.";

				case ErrorType.CircularReference:
					return "The value been serialized contains circular references.";

				default:
					return "An error occurred during serialization.";
			}
		}

		public enum ErrorType : int
		{
			Unknown = 0, 
			InvalidNumber,
			InvalidValueType, 
			CircularReference,
		}
	}
}
