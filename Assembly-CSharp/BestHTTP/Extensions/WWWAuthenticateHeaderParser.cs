using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020007FA RID: 2042
	public sealed class WWWAuthenticateHeaderParser : KeyValuePairList
	{
		// Token: 0x0600489A RID: 18586 RVA: 0x00199B4E File Offset: 0x00197D4E
		public WWWAuthenticateHeaderParser(string headerValue)
		{
			base.Values = this.ParseQuotedHeader(headerValue);
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x00199B64 File Offset: 0x00197D64
		private List<HeaderValue> ParseQuotedHeader(string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str != null)
			{
				int i = 0;
				string key = str.Read(ref i, (char ch) => !char.IsWhiteSpace(ch) && !char.IsControl(ch), true).TrimAndLower();
				list.Add(new HeaderValue(key));
				while (i < str.Length)
				{
					HeaderValue headerValue = new HeaderValue(str.Read(ref i, '=', true).TrimAndLower());
					str.SkipWhiteSpace(ref i);
					headerValue.Value = str.ReadPossibleQuotedText(ref i);
					list.Add(headerValue);
				}
			}
			return list;
		}
	}
}
