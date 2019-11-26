using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.DocZh.Utility.Json.Serialization;

namespace Unity.DocZh.Utility.Json
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonObjectDebugView))]
	public sealed class JsonObject : IEnumerable<KeyValuePair<string, JsonValue>>, IEnumerable<JsonValue>
	{
		private IDictionary<string, JsonValue> properties;
		
		public int Count
		{
			get
			{
				return this.properties.Count;
			}
		}
		
		public JsonValue this[string key]
		{
			get
			{
				if (this.properties.TryGetValue(key, out var value))
				{
					return value;
				}
				else
				{
					return JsonValue.Null;
				}
			}
			set
			{
				this.properties[key] = value;
			}
		}
		
		public JsonObject()
		{
			this.properties = new Dictionary<string, JsonValue>();
		}
		
		public JsonObject Add(string key)
		{
			return Add(key, JsonValue.Null);
		}
		
		public JsonObject Add(string key, JsonValue value)
		{
			this.properties.Add(key, value);
			return this;
		}
		
		public JsonObject AddIfNotNull(string key, JsonValue value)
		{
			if (!value.IsNull)
			{
				Add(key, value);
			}

			return this;
		}
		
		public bool Remove(string key)
		{
			return this.properties.Remove(key);
		}
		
		public JsonObject Clear()
		{
			this.properties.Clear();
			return this;
		}
		
		public JsonObject Rename(string oldKey, string newKey)
		{
			if (this.properties.TryGetValue(oldKey, out var value))
			{
				Remove(oldKey);
				this[newKey] = value;
			}

			return this;
		}
		
		public bool ContainsKey(string key)
		{
			return this.properties.ContainsKey(key);
		}
		
		public bool ContainsKey(string key, out JsonValue value)
		{
			return this.properties.TryGetValue(key, out value);
		}
		
		public bool Contains(JsonValue value)
		{
			return this.properties.Values.Contains(value);
		}
		
		public IEnumerator<KeyValuePair<string, JsonValue>> GetEnumerator()
		{
			return this.properties.GetEnumerator();
		}
		
		IEnumerator<JsonValue> IEnumerable<JsonValue>.GetEnumerator()
		{
			return this.properties.Values.GetEnumerator();
		}
		
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
		
		public override string ToString()
		{
			return ToString(false);
		}
		
		public string ToString(bool pretty)
		{
			return JsonWriter.Serialize(this, pretty);
		}

		private class JsonObjectDebugView
		{
			private JsonObject jsonObject;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public KeyValuePair[] Keys
			{
				get
				{
					var keys = new KeyValuePair[jsonObject.Count];

					var i = 0;
					foreach (var property in jsonObject)
					{
						keys[i] = new KeyValuePair(property.Key, property.Value);
						i += 1;
					}

					return keys;
				}
			}

			public JsonObjectDebugView(JsonObject jsonObject)
			{
				this.jsonObject = jsonObject;
			}

			[DebuggerDisplay("{value.ToString(),nq}", Name = "{key}", Type = "JsonValue({Type})")]
			public class KeyValuePair
			{
				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private string key;

				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private JsonValue value;

				[DebuggerBrowsable(DebuggerBrowsableState.Never)]
				private JsonValueType Type
				{
					get
					{
						return value.Type;
					}
				}

				[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
				public object View
				{
					get
					{
						if (this.value.IsJsonObject)
						{
							return (JsonObject)this.value;
						}
						else if (this.value.IsJsonArray)
						{
							return (JsonArray)this.value;
						}
						else
						{
							return this.value;
						}
					}
				}

				public KeyValuePair(string key, JsonValue value)
				{
					this.key = key;
					this.value = value;
				}
			}
		}
	}
}
