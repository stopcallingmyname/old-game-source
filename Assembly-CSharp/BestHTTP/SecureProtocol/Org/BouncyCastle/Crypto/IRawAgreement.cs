using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003DE RID: 990
	public interface IRawAgreement
	{
		// Token: 0x06002858 RID: 10328
		void Init(ICipherParameters parameters);

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06002859 RID: 10329
		int AgreementSize { get; }

		// Token: 0x0600285A RID: 10330
		void CalculateAgreement(ICipherParameters publicKey, byte[] buf, int off);
	}
}
