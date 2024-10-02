using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F2 RID: 2034
	public class KeyValuePairList
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06004859 RID: 18521 RVA: 0x00198E02 File Offset: 0x00197002
		// (set) Token: 0x0600485A RID: 18522 RVA: 0x00198E0A File Offset: 0x0019700A
		public List<HeaderValue> Values { get; protected set; }

		// Token: 0x0600485B RID: 18523 RVA: 0x00198E14 File Offset: 0x00197014
		public bool TryGet(string valueKeyName, out HeaderValue param)
		{
			param = null;
			for (int i = 0; i < this.Values.Count; i++)
			{
				if (string.CompareOrdinal(this.Values[i].Key, valueKeyName) == 0)
				{
					param = this.Values[i];
					return true;
				}
			}
			return false;
		}
	}
}
