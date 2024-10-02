using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005DE RID: 1502
	internal class BaseDigestCalculator : IDigestCalculator
	{
		// Token: 0x06003975 RID: 14709 RVA: 0x00166E62 File Offset: 0x00165062
		internal BaseDigestCalculator(byte[] digest)
		{
			this.digest = digest;
		}

		// Token: 0x06003976 RID: 14710 RVA: 0x00166E71 File Offset: 0x00165071
		public byte[] GetDigest()
		{
			return Arrays.Clone(this.digest);
		}

		// Token: 0x040025B2 RID: 9650
		private readonly byte[] digest;
	}
}
