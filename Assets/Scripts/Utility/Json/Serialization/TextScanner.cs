using System;
using System.IO;
using System.Text;

namespace Unity.DocZh.Utility.Json.Serialization
{
	using ErrorType = JsonParseException.ErrorType;

	public sealed class TextScanner
	{
		private TextReader reader;
		private TextPosition position;

		public TextPosition Position
		{
			get
			{
				return this.position;
			}
		}

		public bool CanRead
		{
			get
			{
				return (this.reader.Peek() != -1);
			}
		}

		public TextScanner(TextReader reader)
		{
			this.reader = reader ?? throw new ArgumentNullException(nameof(reader));
		}

		public char Peek()
		{
			var next = reader.Peek();

			if (next == -1)
			{
				throw new JsonParseException(
					ErrorType.IncompleteMessage,
					this.position
				);
			}

			return (char)next;
		}

		public char Read()
		{
			var next = reader.Read();

			if (next == -1)
			{
				throw new JsonParseException(
					ErrorType.IncompleteMessage,
					this.position
				);
			}

			switch (next)
			{
				case '\r':
					if (reader.Peek() == '\n')
					{
						reader.Read();
					}
					goto case '\n';

				case '\n':
					this.position.line += 1;
					this.position.column = 0;
					return '\n';

				default:
					this.position.column += 1;
					return (char)next;
			}
		}

		public void SkipWhitespace()
		{
			while (char.IsWhiteSpace(Peek()))
			{
				Read();
			}
		}

		public void Assert(char next)
		{
			if (Peek() == next)
			{
				Read();
			}
			else
			{
				throw new JsonParseException(
					string.Format("Parser expected '{0}'", next),
					ErrorType.InvalidOrUnexpectedCharacter,
					this.position
				);
			}
		}

		public void Assert(string next)
		{
			try
			{
				for (var i = 0; i < next.Length; i += 1)
				{
					Assert(next[i]);
				}
			}
			catch (JsonParseException e) when (e.Type == ErrorType.InvalidOrUnexpectedCharacter)
			{
				throw new JsonParseException(
					string.Format("Parser expected '{0}'", next),
					ErrorType.InvalidOrUnexpectedCharacter,
					this.position
				);
			}
		}
	}
}
