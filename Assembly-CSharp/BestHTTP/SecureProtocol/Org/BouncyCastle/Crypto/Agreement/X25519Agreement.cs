using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005CC RID: 1484
	public sealed class X25519Agreement : IRawAgreement
	{
		// Token: 0x060038F4 RID: 14580 RVA: 0x0016523F File Offset: 0x0016343F
		public void Init(ICipherParameters parameters)
		{
			this.privateKey = (X25519PrivateKeyParameters)parameters;
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x060038F5 RID: 14581 RVA: 0x0016524D File Offset: 0x0016344D
		public int AgreementSize
		{
			get
			{
				return X25519PrivateKeyParameters.SecretSize;
			}
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x00165254 File Offset: 0x00163454
		public void CalculateAgreement(ICipherParameters publicKey, byte[] buf, int off)
		{
			this.privateKey.GenerateSecret((X25519PublicKeyParameters)publicKey, buf, off);
		}

		// Token: 0x04002543 RID: 9539
		private X25519PrivateKeyParameters privateKey;
	}
}
