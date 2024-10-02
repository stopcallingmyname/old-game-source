using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020007EE RID: 2030
	public sealed class HeaderParser : KeyValuePairList
	{
		// Token: 0x06004845 RID: 18501 RVA: 0x00198921 File Offset: 0x00196B21
		public HeaderParser(string headerStr)
		{
			base.Values = this.Parse(headerStr);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x00198938 File Offset: 0x00196B38
		private List<HeaderValue> Parse(string headerStr)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			int i = 0;
			try
			{
				while (i < headerStr.Length)
				{
					HeaderValue headerValue = new HeaderValue();
					headerValue.Parse(headerStr, ref i);
					list.Add(headerValue);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HeaderParser - Parse", headerStr, ex);
			}
			return list;
		}
	}
}
