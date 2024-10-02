using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000264 RID: 612
	public abstract class Integers
	{
		// Token: 0x06001667 RID: 5735 RVA: 0x000B0B79 File Offset: 0x000AED79
		public static int RotateLeft(int i, int distance)
		{
			return i << distance ^ (int)((uint)i >> -distance);
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000B0B79 File Offset: 0x000AED79
		[CLSCompliant(false)]
		public static uint RotateLeft(uint i, int distance)
		{
			return i << distance ^ i >> -distance;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000B0B89 File Offset: 0x000AED89
		public static int RotateRight(int i, int distance)
		{
			return (int)((uint)i >> distance ^ (uint)((uint)i << -distance));
		}

		// Token: 0x0600166A RID: 5738 RVA: 0x000B0B89 File Offset: 0x000AED89
		[CLSCompliant(false)]
		public static uint RotateRight(uint i, int distance)
		{
			return i >> distance ^ i << -distance;
		}
	}
}
