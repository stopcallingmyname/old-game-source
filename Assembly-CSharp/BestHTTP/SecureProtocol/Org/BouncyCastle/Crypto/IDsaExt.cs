using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003D9 RID: 985
	public interface IDsaExt : IDsa
	{
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002849 RID: 10313
		BigInteger Order { get; }
	}
}
