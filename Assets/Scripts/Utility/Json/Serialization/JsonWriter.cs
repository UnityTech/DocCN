using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Unity.DocZh.Utility.Json.Serialization
{
	using ErrorType = JsonSerializationException.ErrorType;

	public sealed class JsonWriter
	{
		private int indent;
		private bool isNewLine;
		private HashSet<IEnumerable<JsonValue>> renderingCollections;
		public string IndentString { get; set; }
		public string SpacingString { get; set; }
		public string NewLineString { get; set; }
		public bool SortObjects { get; set; }
		public TextWriter InnerWriter { get; set; }
		public JsonWriter(TextWriter innerWriter) : this(innerWriter, false) { }
		public JsonWriter(TextWriter innerWriter, bool pretty)
		{
			if (pretty)
			{
				this.IndentString = "\t";
				this.SpacingString = " ";
				this.NewLineString = "\n";
			}

			renderingCollections = new HashSet<IEnumerable<JsonValue>>();
			InnerWriter = innerWriter;
		}

		private void Write(string text)
		{
			if (this.isNewLine)
			{
				this.isNewLine = false;
				WriteIndentation();
			}

			InnerWriter.Write(text);
		}

		private void WriteEncodedJsonValue(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
					Write("null");
					break;

				case JsonValueType.Boolean:
					Write(value.AsString);
					break;

				case JsonValueType.Number:
					if (!IsValidNumber(value))
					{
						throw new JsonSerializationException(ErrorType.InvalidNumber);
					}

					Write(((double)value).ToString(CultureInfo.InvariantCulture));
					break;

				case JsonValueType.String:
					WriteEncodedString((string)value);
					break;

				case JsonValueType.Object:
					Write(string.Format("JsonObject[{0}]", value.AsJsonObject.Count));
					break;

				case JsonValueType.Array:
					Write(string.Format("JsonArray[{0}]", value.AsJsonArray.Count));
					break;

				default:
					throw new InvalidOperationException("Invalid value type.");
			}
		}

		private void WriteEncodedString(string text)
		{
			Write("\"");

			for (int i = 0; i < text.Length; i += 1)
			{
				var currentChar = text[i];

				// Encoding special characters.
				switch (currentChar)
				{
					case '\\':
						InnerWriter.Write("\\\\");
						break;

					case '\"':
						InnerWriter.Write("\\\"");
						break;

					case '/':
						InnerWriter.Write("\\/");
						break;

					case '\b':
						InnerWriter.Write("\\b");
						break;

					case '\f':
						InnerWriter.Write("\\f");
						break;

					case '\n':
						InnerWriter.Write("\\n");
						break;

					case '\r':
						InnerWriter.Write("\\r");
						break;

					case '\t':
						InnerWriter.Write("\\t");
						break;

					default:
						InnerWriter.Write(currentChar);
						break;
				}
			}

			InnerWriter.Write("\"");
		}

		private void WriteIndentation()
		{
			for (var i = 0; i < this.indent; i += 1)
			{
				Write(this.IndentString);
			}
		}

		private void WriteSpacing()
		{
			Write(this.SpacingString);
		}

		private void WriteLine()
		{
			Write(this.NewLineString);
			this.isNewLine = true;
		}

		private void WriteLine(string line)
		{
			Write(line);
			WriteLine();
		}

		private void AddRenderingCollection(IEnumerable<JsonValue> value)
		{
			if (!renderingCollections.Add(value))
			{
				throw new JsonSerializationException(ErrorType.CircularReference);
			}
		}

		private void RemoveRenderingCollection(IEnumerable<JsonValue> value)
		{
			renderingCollections.Remove(value);
		}

		private void Render(JsonValue value)
		{
			switch (value.Type)
			{
				case JsonValueType.Null:
				case JsonValueType.Boolean:
				case JsonValueType.Number:
				case JsonValueType.String:
					WriteEncodedJsonValue(value);
					break;

				case JsonValueType.Object:
					Render((JsonObject)value);
					break;

				case JsonValueType.Array:
					Render((JsonArray)value);
					break;

				default:
					throw new JsonSerializationException(ErrorType.InvalidValueType);
			}
		}

		private void Render(JsonArray value)
		{
			AddRenderingCollection(value);

			WriteLine("[");

			indent += 1;

			using (var enumerator = value.GetEnumerator())
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					Render(enumerator.Current);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("]");

			RemoveRenderingCollection(value);
		}

		private void Render(JsonObject value)
		{
			AddRenderingCollection(value);

			WriteLine("{");

			indent += 1;

			using(var enumerator = GetJsonObjectEnumerator(value))
			{
				var hasNext = enumerator.MoveNext();

				while (hasNext)
				{
					WriteEncodedString(enumerator.Current.Key);
					Write(":");
					WriteSpacing();
					Render(enumerator.Current.Value);

					hasNext = enumerator.MoveNext();

					if (hasNext)
					{
						WriteLine(",");
					}
					else
					{
						WriteLine();
					}
				}
			}

			indent -= 1;

			Write("}");

			RemoveRenderingCollection(value);
		}

		private IEnumerator<KeyValuePair<string, JsonValue>> GetJsonObjectEnumerator(JsonObject jsonObject)
		{
			if (this.SortObjects)
			{
				var sortedDictionary = new SortedDictionary<string, JsonValue>(StringComparer.Ordinal);

				foreach (var item in jsonObject)
				{
					sortedDictionary.Add(item.Key, item.Value);
				}

				return sortedDictionary.GetEnumerator();
			}
			else
			{
				return jsonObject.GetEnumerator();
			}
		}

		public void Write(JsonValue jsonValue)
		{
			this.indent = 0;
			this.isNewLine = true;

			Render(jsonValue);

			this.renderingCollections.Clear();
		}

		private static bool IsValidNumber(double number)
		{
			return !(double.IsNaN(number) || double.IsInfinity(number));
		}

		public static string Serialize(JsonValue value)
		{
			return Serialize(value, false);
		}

		public static string Serialize(JsonValue value, bool pretty)
		{
			using (var stringWriter = new StringWriter())
			{
				var jsonWriter = new JsonWriter(stringWriter, pretty);

				jsonWriter.Write(value);

				return stringWriter.ToString();
			}
		}
	}
}
