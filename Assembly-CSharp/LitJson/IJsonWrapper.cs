using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x02000150 RID: 336
	public interface IJsonWrapper : IList, ICollection, IEnumerable, IOrderedDictionary, IDictionary
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000B47 RID: 2887
		bool IsArray { get; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000B48 RID: 2888
		bool IsBoolean { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000B49 RID: 2889
		bool IsDouble { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000B4A RID: 2890
		bool IsInt { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000B4B RID: 2891
		bool IsLong { get; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000B4C RID: 2892
		bool IsObject { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000B4D RID: 2893
		bool IsString { get; }

		// Token: 0x06000B4E RID: 2894
		bool GetBoolean();

		// Token: 0x06000B4F RID: 2895
		double GetDouble();

		// Token: 0x06000B50 RID: 2896
		int GetInt();

		// Token: 0x06000B51 RID: 2897
		JsonType GetJsonType();

		// Token: 0x06000B52 RID: 2898
		long GetLong();

		// Token: 0x06000B53 RID: 2899
		string GetString();

		// Token: 0x06000B54 RID: 2900
		void SetBoolean(bool val);

		// Token: 0x06000B55 RID: 2901
		void SetDouble(double val);

		// Token: 0x06000B56 RID: 2902
		void SetInt(int val);

		// Token: 0x06000B57 RID: 2903
		void SetJsonType(JsonType type);

		// Token: 0x06000B58 RID: 2904
		void SetLong(long val);

		// Token: 0x06000B59 RID: 2905
		void SetString(string val);

		// Token: 0x06000B5A RID: 2906
		string ToJson();

		// Token: 0x06000B5B RID: 2907
		void ToJson(JsonWriter writer);
	}
}
