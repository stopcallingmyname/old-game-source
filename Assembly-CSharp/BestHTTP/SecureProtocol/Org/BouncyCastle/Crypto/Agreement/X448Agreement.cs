using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005CD RID: 1485
	public sealed class X448Agreement : IRawAgreement
	{
		// Token: 0x060038F8 RID: 14584 RVA: 0x00165269 File Offset: 0x00163469
		public void Init(ICipherParameters parameters)
		{
			this.privateKey = (X448PrivateKeyParameters)parameters;
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x060038F9 RID: 14585 RVA: 0x00165277 File Offset: 0x00163477
		public int AgreementSize
		{
			get
			{
				return X448PrivateKeyParameters.SecretSize;
			}
		}

		// Token: 0x060038FA RID: 14586 RVA: 0x0016527E File Offset: 0x0016347E
		public void CalculateAgreement(ICipherParameters publicKey, byte[] buf, int off)
		{
			this.privateKey.GenerateSecret((X448PublicKeyParameters)publicKey, buf, off);
		}

		// Token: 0x04002544 RID: 9540
		private X448PrivateKeyParameters privateKey;
	}
}
