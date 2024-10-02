using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002AC RID: 684
	[Serializable]
	public class TspException : Exception
	{
		// Token: 0x060018F1 RID: 6385 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public TspException()
		{
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x0008BF89 File Offset: 0x0008A189
		public TspException(string message) : base(message)
		{
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x0008BF92 File Offset: 0x0008A192
		public TspException(string message, Exception e) : base(message, e)
		{
		}
	}
}
