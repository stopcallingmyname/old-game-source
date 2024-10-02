using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020007ED RID: 2029
	public static class ExceptionHelper
	{
		// Token: 0x06004844 RID: 18500 RVA: 0x00198915 File Offset: 0x00196B15
		public static Exception ServerClosedTCPStream()
		{
			return new Exception("TCP Stream closed unexpectedly by the remote server");
		}
	}
}
