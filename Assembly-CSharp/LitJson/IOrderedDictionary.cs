using System;
using System.Collections;

namespace LitJson
{
	// Token: 0x0200014F RID: 335
	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x06000B42 RID: 2882
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06000B43 RID: 2883
		void Insert(int index, object key, object value);

		// Token: 0x06000B44 RID: 2884
		void RemoveAt(int index);

		// Token: 0x17000073 RID: 115
		object this[int index]
		{
			get;
			set;
		}
	}
}
