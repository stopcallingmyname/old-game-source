using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x0200015D RID: 349
	public sealed class JsonMockWrapper : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000C0B RID: 3083 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0008D534 File Offset: 0x0008B734
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0008D53F File Offset: 0x0008B73F
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0008D543 File Offset: 0x0008B743
		public string GetString()
		{
			return "";
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetDouble(double val)
		{
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetInt(int val)
		{
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetLong(long val)
		{
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0000248C File Offset: 0x0000068C
		public void SetString(string val)
		{
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0008D543 File Offset: 0x0008B743
		public string ToJson()
		{
			return "";
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0000248C File Offset: 0x0000068C
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x0006AE98 File Offset: 0x00069098
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000C1C RID: 3100 RVA: 0x0006AE98 File Offset: 0x00069098
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000AC RID: 172
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x0007D96F File Offset: 0x0007BB6F
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0000248C File Offset: 0x0000068C
		void IList.Clear()
		{
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0007D96F File Offset: 0x0007BB6F
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0008D54D File Offset: 0x0008B74D
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0000248C File Offset: 0x0000068C
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0000248C File Offset: 0x0000068C
		void IList.Remove(object value)
		{
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0000248C File Offset: 0x0000068C
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0007D96F File Offset: 0x0007BB6F
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0007D96F File Offset: 0x0007BB6F
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0008D54A File Offset: 0x0008B74A
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0000248C File Offset: 0x0000068C
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0008D54A File Offset: 0x0008B74A
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0006AE98 File Offset: 0x00069098
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0006AE98 File Offset: 0x00069098
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0008D54A File Offset: 0x0008B74A
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x0008D54A File Offset: 0x0008B74A
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000B4 RID: 180
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0000248C File Offset: 0x0000068C
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0000248C File Offset: 0x0000068C
		void IDictionary.Clear()
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0007D96F File Offset: 0x0007BB6F
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0000248C File Offset: 0x0000068C
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x0008D54A File Offset: 0x0008B74A
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170000B5 RID: 181
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0008D54A File Offset: 0x0008B74A
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06000C39 RID: 3129 RVA: 0x0000248C File Offset: 0x0000068C
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x06000C3A RID: 3130 RVA: 0x0000248C File Offset: 0x0000068C
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
