using System;
using System.Collections.Generic;

namespace BestHTTP.Caching
{
	// Token: 0x02000818 RID: 2072
	public sealed class UriComparer : IEqualityComparer<Uri>
	{
		// Token: 0x060049DA RID: 18906 RVA: 0x001A356D File Offset: 0x001A176D
		public bool Equals(Uri x, Uri y)
		{
			return Uri.Compare(x, y, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x001A357D File Offset: 0x001A177D
		public int GetHashCode(Uri uri)
		{
			return uri.ToString().GetHashCode();
		}
	}
}
