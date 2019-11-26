using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.DocZh.Utility.Json.Serialization;

namespace Unity.DocZh.Utility.Json
{
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(JsonArrayDebugView))]
	public sealed class JsonArray : IEnumerable<JsonValue>
	{
		private IList<JsonValue> items;


		public int Count => this.items.Count;
		
		public JsonValue this[int index]
		{
			get
			{
				if (index >= 0 && index < this.items.Count)
				{
					return this.items[index];
				}
				else
				{
					return JsonValue.Null;
				}
			}
			set
			{
				this.items[index] = value;
			}
		}
		
		public JsonArray()
		{
			this.items = new List<JsonValue>();
		}
		
		public JsonArray(params JsonValue[] values) : this()
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values));
			}

			foreach (var value in values)
			{
				this.items.Add(value);
			}
		}
		
		public JsonArray Add(JsonValue value)
		{
			this.items.Add(value);
			return this;
		}
		
		public JsonArray AddIfNotNull(JsonValue value)
		{
			if (!value.IsNull)
			{
				Add(value);
			}

			return this;
		}
		
		public JsonArray Insert(int index, JsonValue value)
		{
			this.items.Insert(index, value);
			return this;
		}
		
		public JsonArray InsertIfNotNull(int index, JsonValue value)
		{
			if (!value.IsNull)
			{
				Insert(index, value);
			}

			return this;
		}
		
		public JsonArray Remove(int index)
		{
			this.items.RemoveAt(index);
			return this;
		}
		
		public JsonArray Clear()
		{
			this.items.Clear();
			return this;
		}
		
		public bool Contains(JsonValue item)
		{
			return this.items.Contains(item);
		}
		
		public int IndexOf(JsonValue item)
		{
			return this.items.IndexOf(item);
		}
		
		public IEnumerator<JsonValue> GetEnumerator()
		{
			return this.items.GetEnumerator();
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

		private class JsonArrayDebugView
		{
			private JsonArray jsonArray;

			[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
			public JsonValue[] Items
			{
				get
				{
					var items = new JsonValue[this.jsonArray.Count];

					for (int i = 0; i < this.jsonArray.Count; i += 1)
					{
						items[i] = this.jsonArray[i];
					}

					return items;
				}
			}

			public JsonArrayDebugView(JsonArray jsonArray)
			{
				this.jsonArray = jsonArray;
			}
		}
	}
}
