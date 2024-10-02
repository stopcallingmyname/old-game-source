using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace LitJson
{
	// Token: 0x02000151 RID: 337
	public sealed class JsonData : IJsonWrapper, IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary, IEquatable<JsonData>
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0008B05D File Offset: 0x0008925D
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0008B06A File Offset: 0x0008926A
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0008B075 File Offset: 0x00089275
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0008B080 File Offset: 0x00089280
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0008B08B File Offset: 0x0008928B
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0008B096 File Offset: 0x00089296
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0008B0A1 File Offset: 0x000892A1
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0008B0AC File Offset: 0x000892AC
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0008B0B7 File Offset: 0x000892B7
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0008B0CB File Offset: 0x000892CB
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0008B0D3 File Offset: 0x000892D3
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0008B0E0 File Offset: 0x000892E0
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0008B0ED File Offset: 0x000892ED
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0008B0FA File Offset: 0x000892FA
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x0008B108 File Offset: 0x00089308
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0008B170 File Offset: 0x00089370
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x0008B1D8 File Offset: 0x000893D8
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x0008B1E0 File Offset: 0x000893E0
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x0008B1E8 File Offset: 0x000893E8
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0008B1F0 File Offset: 0x000893F0
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000B70 RID: 2928 RVA: 0x0008B1F8 File Offset: 0x000893F8
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0008B200 File Offset: 0x00089400
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0008B208 File Offset: 0x00089408
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0008B210 File Offset: 0x00089410
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0008B21D File Offset: 0x0008941D
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x17000094 RID: 148
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x17000095 RID: 149
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x17000096 RID: 150
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x17000097 RID: 151
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x17000098 RID: 152
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00022F1F File Offset: 0x0002111F
		public JsonData()
		{
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0008B47B File Offset: 0x0008967B
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0008B491 File Offset: 0x00089691
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0008B4A7 File Offset: 0x000896A7
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0008B4BD File Offset: 0x000896BD
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0008B4D4 File Offset: 0x000896D4
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x0008B57D File Offset: 0x0008977D
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0008B593 File Offset: 0x00089793
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0008B59B File Offset: 0x0008979B
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x0008B5A3 File Offset: 0x000897A3
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0008B5AB File Offset: 0x000897AB
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x0008B5B3 File Offset: 0x000897B3
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x0008B5BB File Offset: 0x000897BB
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x0008B5D7 File Offset: 0x000897D7
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x0008B5F3 File Offset: 0x000897F3
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0008B60F File Offset: 0x0008980F
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0008B62B File Offset: 0x0008982B
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0008B647 File Offset: 0x00089847
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0008B658 File Offset: 0x00089858
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0008B69B File Offset: 0x0008989B
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0008B6BA File Offset: 0x000898BA
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0008B6C8 File Offset: 0x000898C8
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0008B6D0 File Offset: 0x000898D0
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0008B735 File Offset: 0x00089935
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0008B742 File Offset: 0x00089942
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0008B75E File Offset: 0x0008995E
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0008B77A File Offset: 0x0008997A
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0008B796 File Offset: 0x00089996
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0008B7B2 File Offset: 0x000899B2
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0008B7CE File Offset: 0x000899CE
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0008B7E5 File Offset: 0x000899E5
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0008B7FC File Offset: 0x000899FC
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x0008B813 File Offset: 0x00089A13
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0008B82A File Offset: 0x00089A2A
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0008B841 File Offset: 0x00089A41
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0008B849 File Offset: 0x00089A49
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0008B852 File Offset: 0x00089A52
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0008B85B File Offset: 0x00089A5B
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0008B86F File Offset: 0x00089A6F
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0008B87D File Offset: 0x00089A7D
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0008B88B File Offset: 0x00089A8B
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0008B8A1 File Offset: 0x00089AA1
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0008B8B6 File Offset: 0x00089AB6
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0008B8CB File Offset: 0x00089ACB
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0008B8E4 File Offset: 0x00089AE4
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0008B920 File Offset: 0x00089B20
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0008B960 File Offset: 0x00089B60
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0008B998 File Offset: 0x00089B98
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0008B9F8 File Offset: 0x00089BF8
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0008BA4A File Offset: 0x00089C4A
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0008BA68 File Offset: 0x00089C68
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				foreach (object obj2 in obj)
				{
					JsonData.WriteJson((JsonData)obj2, writer);
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				foreach (object obj3 in obj)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
					writer.WritePropertyName((string)dictionaryEntry.Key);
					JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0008BBB8 File Offset: 0x00089DB8
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0008BBE0 File Offset: 0x00089DE0
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0008BC00 File Offset: 0x00089E00
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0008BCD5 File Offset: 0x00089ED5
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0008BCE0 File Offset: 0x00089EE0
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0008BD80 File Offset: 0x00089F80
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0008BDCC File Offset: 0x00089FCC
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0008BDF8 File Offset: 0x00089FF8
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x040011FC RID: 4604
		private IList<JsonData> inst_array;

		// Token: 0x040011FD RID: 4605
		private bool inst_boolean;

		// Token: 0x040011FE RID: 4606
		private double inst_double;

		// Token: 0x040011FF RID: 4607
		private int inst_int;

		// Token: 0x04001200 RID: 4608
		private long inst_long;

		// Token: 0x04001201 RID: 4609
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x04001202 RID: 4610
		private string inst_string;

		// Token: 0x04001203 RID: 4611
		private string json;

		// Token: 0x04001204 RID: 4612
		private JsonType type;

		// Token: 0x04001205 RID: 4613
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
