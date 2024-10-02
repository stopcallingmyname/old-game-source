using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x02000281 RID: 641
	[Serializable]
	public class PemGenerationException : Exception
	{
		// Token: 0x0600179D RID: 6045 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public PemGenerationException()
		{
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x0008BF89 File Offset: 0x0008A189
		public PemGenerationException(string message) : base(message)
		{
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x0008BF92 File Offset: 0x0008A192
		public PemGenerationException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
