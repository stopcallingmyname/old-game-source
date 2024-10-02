using System;
using System.Runtime.InteropServices;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000809 RID: 2057
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	internal class ZlibException : Exception
	{
		// Token: 0x0600491B RID: 18715 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public ZlibException()
		{
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x0008BF89 File Offset: 0x0008A189
		public ZlibException(string s) : base(s)
		{
		}
	}
}
