using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Security
{
	// Token: 0x020002E3 RID: 739
	[Obsolete("Never thrown")]
	[Serializable]
	public class NoSuchAlgorithmException : GeneralSecurityException
	{
		// Token: 0x06001AFC RID: 6908 RVA: 0x000BC908 File Offset: 0x000BAB08
		public NoSuchAlgorithmException()
		{
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x000BC910 File Offset: 0x000BAB10
		public NoSuchAlgorithmException(string message) : base(message)
		{
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x000BC919 File Offset: 0x000BAB19
		public NoSuchAlgorithmException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
