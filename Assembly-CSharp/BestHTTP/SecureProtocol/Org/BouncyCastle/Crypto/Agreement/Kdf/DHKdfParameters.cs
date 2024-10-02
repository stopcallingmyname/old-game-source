using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement.Kdf
{
	// Token: 0x020005D4 RID: 1492
	public class DHKdfParameters : IDerivationParameters
	{
		// Token: 0x06003928 RID: 14632 RVA: 0x00165CBD File Offset: 0x00163EBD
		public DHKdfParameters(DerObjectIdentifier algorithm, int keySize, byte[] z) : this(algorithm, keySize, z, null)
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x00165CC9 File Offset: 0x00163EC9
		public DHKdfParameters(DerObjectIdentifier algorithm, int keySize, byte[] z, byte[] extraInfo)
		{
			this.algorithm = algorithm;
			this.keySize = keySize;
			this.z = z;
			this.extraInfo = extraInfo;
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600392A RID: 14634 RVA: 0x00165CEE File Offset: 0x00163EEE
		public DerObjectIdentifier Algorithm
		{
			get
			{
				return this.algorithm;
			}
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600392B RID: 14635 RVA: 0x00165CF6 File Offset: 0x00163EF6
		public int KeySize
		{
			get
			{
				return this.keySize;
			}
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x00165CFE File Offset: 0x00163EFE
		public byte[] GetZ()
		{
			return this.z;
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x00165D06 File Offset: 0x00163F06
		public byte[] GetExtraInfo()
		{
			return this.extraInfo;
		}

		// Token: 0x0400257B RID: 9595
		private readonly DerObjectIdentifier algorithm;

		// Token: 0x0400257C RID: 9596
		private readonly int keySize;

		// Token: 0x0400257D RID: 9597
		private readonly byte[] z;

		// Token: 0x0400257E RID: 9598
		private readonly byte[] extraInfo;
	}
}
