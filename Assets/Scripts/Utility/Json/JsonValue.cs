using System;
using System.Diagnostics;
using System.Collections.Generic;
using Unity.DocZh.Utility.Json.Serialization;
using Unity.UIWidgets.ui;

namespace Unity.DocZh.Utility.Json
{
	[DebuggerDisplay("{ToString(),nq}", Type = "JsonValue({Type})")]
	[DebuggerTypeProxy(typeof(JsonValueDebugView))]
	public struct JsonValue
	{
		private readonly JsonValueType type;
		private readonly object reference;
		private readonly double value;

		public static readonly JsonValue Null = new JsonValue(JsonValueType.Null, default(double), null);
		
		public JsonValueType Type => this.type;
		public bool IsNull => this.Type == JsonValueType.Null;
		public bool IsBoolean => this.Type == JsonValueType.Boolean;

		public bool IsInteger
		{
			get
			{
				if (!this.IsNumber)
				{
					return false;
				}

				var value = this.value;

				return (value >= Int32.MinValue) && (value <= Int32.MaxValue) && unchecked((Int32)value) == value;
			}
		}
		
		public bool IsNumber => this.Type == JsonValueType.Number;
		public bool IsString => this.Type == JsonValueType.String;
		public bool IsJsonObject => this.Type == JsonValueType.Object;
		public bool IsJsonArray => this.Type == JsonValueType.Array;
		public bool IsDateTime => this.AsDateTime != null;

		public bool AsBoolean
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (this.value == 1);

					case JsonValueType.Number:
						return (this.value != 0);

					case JsonValueType.String:
						return ((string)this.reference != "");

					case JsonValueType.Object:
					case JsonValueType.Array:
						return true;

					default:
						return false;
				}
			}
		}
		
		public int AsInteger => ((int)this.AsNumber).clamp(int.MinValue, int.MaxValue);
		public double AsNumber
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (this.value == 1)
							? 1
							: 0;

					case JsonValueType.Number:
						return this.value;

					case JsonValueType.String:
						double number;
						if (double.TryParse((string)this.reference, out number))
						{
							return number;
						}
						else
						{
							goto default;
						}

					default:
						return 0;
				}
			}
		}
		
		public string AsString
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
						return (this.value == 1)
							? "true"
							: "false";

					case JsonValueType.Number:
						return this.value.ToString();

					case JsonValueType.String:
						return (string)this.reference;

					default:
						return null;
				}
			}
		}
		
		public JsonObject AsJsonObject
		{
			get
			{
				return (this.IsJsonObject)
					? (JsonObject)this.reference
					: null;
			}
		}
		
		public JsonArray AsJsonArray
		{
			get
			{
				return (this.IsJsonArray)
					? (JsonArray)this.reference
					: null;
			}
		}
		
		public DateTime? AsDateTime
		{
			get
			{
				if (this.IsString && DateTime.TryParse((string)this.reference, out DateTime value))
				{
					return value;
				}
				else
				{
					return null;
				}
			}
		}
		
		public object AsObject
		{
			get
			{
				switch (this.Type)
				{
					case JsonValueType.Boolean:
					case JsonValueType.Number:
						return this.value;

					case JsonValueType.String:
					case JsonValueType.Object:
					case JsonValueType.Array:
						return this.reference;

					default:
						return null;
				}
			}
		}
		
		public JsonValue this[string key]
		{
			get
			{
				if (this.IsJsonObject)
				{
					return ((JsonObject)this.reference)[key];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonObject.");
				}
			}
			set
			{
				if (this.IsJsonObject)
				{
					((JsonObject)this.reference)[key] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonObject.");
				}
			}
		}
		
		public JsonValue this[int index]
		{
			get
			{
				if (this.IsJsonArray)
				{
					return ((JsonArray)this.reference)[index];
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonArray.");
				}
			}
			set
			{
				if (this.IsJsonArray)
				{
					((JsonArray)this.reference)[index] = value;
				}
				else
				{
					throw new InvalidOperationException("This value does not represent a JsonArray.");
				}
			}
		}
		
		private JsonValue(JsonValueType type, double value, object reference)
		{
			this.type      = type;
			this.value     = value;
			this.reference = reference;
		}
		
		public JsonValue(bool? value)
		{
			if (value.HasValue)
			{
				this.reference = null;

				this.type = JsonValueType.Boolean;

				this.value = value.Value ? 1 : 0;
			}
			else
			{
				this = JsonValue.Null;
			}
		}
		
		public JsonValue(double? value)
		{
			if (value.HasValue)
			{
				this.reference = null;

				this.type = JsonValueType.Number;

				this.value = value.Value;
			}
			else
			{
				this = JsonValue.Null;
			}
		}
		
		public JsonValue(string value)
		{
			if (value != null)
			{
				this.value = default(double);

				this.type = JsonValueType.String;

				this.reference = value;
			}
			else
			{
				this = JsonValue.Null;
			}
		}
		
		public JsonValue(JsonObject value)
		{
			if (value != null)
			{
				this.value = default(double);

				this.type = JsonValueType.Object;

				this.reference = value;
			}
			else
			{
				this = JsonValue.Null;
			}
		}
		
		public JsonValue(JsonArray value)
		{
			if (value != null)
			{
				this.value = default(double);

				this.type = JsonValueType.Array;

				this.reference = value;
			}
			else
			{
				this = JsonValue.Null;
			}
		}
		
		public static implicit operator JsonValue(bool? value)
		{
			return new JsonValue(value);
		}
		
		public static implicit operator JsonValue(double? value)
		{
			return new JsonValue(value);
		}
		
		public static implicit operator JsonValue(string value)
		{
			return new JsonValue(value);
		}
		
		public static implicit operator JsonValue(JsonObject value)
		{
			return new JsonValue(value);
		}
		
		public static implicit operator JsonValue(JsonArray value)
		{
			return new JsonValue(value);
		}
		
		public static implicit operator JsonValue(DateTime? value)
		{
			if (value == null)
			{
				return JsonValue.Null;
			}

			return new JsonValue(value.Value.ToString("o"));
		}
		
		public static implicit operator int(JsonValue jsonValue)
		{
			if (jsonValue.IsInteger)
			{
				return jsonValue.AsInteger;
			}
			else
			{
				return 0;
			}
		}
		
		public static implicit operator int?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			else
			{
				return (int)jsonValue;
			}
		}
		
		public static implicit operator bool(JsonValue jsonValue)
		{
			if (jsonValue.IsBoolean)
			{
				return (jsonValue.value == 1);
			}
			else
			{
				return false;
			}
		}
		
		public static implicit operator bool?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			else
			{
				return (bool)jsonValue;
			}
		}
		
		public static implicit operator double(JsonValue jsonValue)
		{
			if (jsonValue.IsNumber)
			{
				return jsonValue.value;
			}
			else
			{
				return double.NaN;
			}
		}
		
		public static implicit operator double?(JsonValue jsonValue)
		{
			if (jsonValue.IsNull)
			{
				return null;
			}
			else
			{
				return (double)jsonValue;
			}
		}
		
		public static implicit operator string(JsonValue jsonValue)
		{
			if (jsonValue.IsString || jsonValue.IsNull)
			{
				return jsonValue.reference as string;
			}
			else
			{
				return null;
			}
		}
		
		public static implicit operator JsonObject(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonObject || jsonValue.IsNull)
			{
				return jsonValue.reference as JsonObject;
			}
			else
			{
				return null;
			}
		}
		
		public static implicit operator JsonArray(JsonValue jsonValue)
		{
			if (jsonValue.IsJsonArray || jsonValue.IsNull)
			{
				return jsonValue.reference as JsonArray;
			}
			else
			{
				return null;
			}
		}
		
		public static implicit operator DateTime(JsonValue jsonValue)
		{
			var dateTime = jsonValue.AsDateTime;

			if (dateTime.HasValue)
			{
				return dateTime.Value;
			}
			else
			{
				return DateTime.MinValue;
			}
		}
		
		public static implicit operator DateTime?(JsonValue jsonValue)
		{
			if (jsonValue.IsDateTime || jsonValue.IsNull)
			{
				return jsonValue.AsDateTime;
			}
			else
			{
				return null;
			}
		}
		
		public static bool operator ==(JsonValue a, JsonValue b)
		{
			return (a.Type == b.Type)
				&& (a.value == b.value)
				&& Equals(a.reference, b.reference);
		}
		
		public static bool operator !=(JsonValue a, JsonValue b)
		{
			return !(a == b);
		}
		
		public static JsonValue Parse(string text)
		{
			return JsonReader.Parse(text);
		}
		
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return this.IsNull;
			}

			var jsonValue = obj as JsonValue?;

			if (jsonValue.HasValue)
			{
				return (this == jsonValue.Value);
			}
			else
			{
				return false;
			}
		}
		
		public override int GetHashCode()
		{
			if (this.IsNull)
			{
				return this.Type.GetHashCode();
			}
			else
			{
				return this.Type.GetHashCode()
					^ this.value.GetHashCode()
					^ EqualityComparer<object>.Default.GetHashCode(this.reference);
			}
		}
		
		public override string ToString()
		{
			return ToString(false);
		}
		
		public string ToString(bool pretty)
		{
			return JsonWriter.Serialize(this, pretty);
		}

		private class JsonValueDebugView
		{
			private JsonValue jsonValue;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonObject ObjectView
			{
				get
				{
					if (jsonValue.IsJsonObject)
					{
						return (JsonObject)jsonValue.reference;
					}
					else
					{
						return null;
					}
				}
			}

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonArray ArrayView
			{
				get
				{
					if (jsonValue.IsJsonArray)
					{
						return (JsonArray)jsonValue.reference;
					}
					else
					{
						return null;
					}
				}
			}

			public JsonValueType Type
			{
				get
				{
					return jsonValue.Type;
				}
			}

			public object Value
			{
				get
				{
					if (jsonValue.IsJsonObject)
					{
						return (JsonObject)jsonValue.reference;
					}
					else if (jsonValue.IsJsonArray)
					{
						return (JsonArray)jsonValue.reference;
					}
					else
					{
						return jsonValue;
					}
				}
			}

			public JsonValueDebugView(JsonValue jsonValue)
			{
				this.jsonValue = jsonValue;
			}
		}
	}
}
