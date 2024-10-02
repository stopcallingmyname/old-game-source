using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x020004A1 RID: 1185
	public interface IDsaKCalculator
	{
		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002E70 RID: 11888
		bool IsDeterministic { get; }

		// Token: 0x06002E71 RID: 11889
		void Init(BigInteger n, SecureRandom random);

		// Token: 0x06002E72 RID: 11890
		void Init(BigInteger n, BigInteger d, byte[] message);

		// Token: 0x06002E73 RID: 11891
		BigInteger NextK();
	}
}
