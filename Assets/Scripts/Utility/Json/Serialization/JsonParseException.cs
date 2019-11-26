using System;

namespace Unity.DocZh.Utility.Json.Serialization
{
	public sealed class JsonParseException : Exception
	{
		public TextPosition Position { get; private set; }
		public ErrorType Type { get; private set; }
		public JsonParseException()
			: base(GetDefaultMessage(ErrorType.Unknown)) { }

		public JsonParseException(ErrorType type, TextPosition position)
			: this(GetDefaultMessage(type), type, position) { }
		public JsonParseException(string message, ErrorType type, TextPosition position)
			: base(message)
		{
			this.Type = type;
			this.Position = position;
		}

		private static string GetDefaultMessage(ErrorType type)
		{
			switch (type)
			{
				case ErrorType.IncompleteMessage:
					return "The string ended before a value could be parsed.";

				case ErrorType.InvalidOrUnexpectedCharacter:
					return "The parser encountered an invalid or unexpected character.";

				case ErrorType.DuplicateObjectKeys:
					return "The parser encountered a JsonObject with duplicate keys.";

				default:
					return "An error occurred while parsing the JSON message.";
			}
		}

		public enum ErrorType : int
		{
			Unknown = 0,
			IncompleteMessage,
			DuplicateObjectKeys,
			InvalidOrUnexpectedCharacter,
		}
	}
}
