using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D2 RID: 978
	public interface IBlockResult
	{
		// Token: 0x06002827 RID: 10279
		byte[] Collect();

		// Token: 0x06002828 RID: 10280
		int Collect(byte[] destination, int offset);
	}
}
