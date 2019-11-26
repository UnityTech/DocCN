using System;

namespace Unity.DocZh.Utility.Json
{
	public enum JsonValueType : byte
	{
		Null = 0,
		Boolean,
		Number,
		String,
		Object,
		Array,
	}
}
