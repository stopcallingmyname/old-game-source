using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000152 RID: 338
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0008BE7A File Offset: 0x0008A07A
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0008BE88 File Offset: 0x0008A088
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0008BEB4 File Offset: 0x0008A0B4
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0008BED4 File Offset: 0x0008A0D4
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0008BEF4 File Offset: 0x0008A0F4
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0008BF03 File Offset: 0x0008A103
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0008BF10 File Offset: 0x0008A110
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04001206 RID: 4614
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
