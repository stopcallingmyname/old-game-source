using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C2 RID: 962
	public class AsymmetricCipherKeyPair
	{
		// Token: 0x060027A9 RID: 10153 RVA: 0x0010BDA4 File Offset: 0x00109FA4
		public AsymmetricCipherKeyPair(AsymmetricKeyParameter publicParameter, AsymmetricKeyParameter privateParameter)
		{
			if (publicParameter.IsPrivate)
			{
				throw new ArgumentException("Expected a public key", "publicParameter");
			}
			if (!privateParameter.IsPrivate)
			{
				throw new ArgumentException("Expected a private key", "privateParameter");
			}
			this.publicParameter = publicParameter;
			this.privateParameter = privateParameter;
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x060027AA RID: 10154 RVA: 0x0010BDF5 File Offset: 0x00109FF5
		public AsymmetricKeyParameter Public
		{
			get
			{
				return this.publicParameter;
			}
		}

		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x060027AB RID: 10155 RVA: 0x0010BDFD File Offset: 0x00109FFD
		public AsymmetricKeyParameter Private
		{
			get
			{
				return this.privateParameter;
			}
		}

		// Token: 0x04001AF2 RID: 6898
		private readonly AsymmetricKeyParameter publicParameter;

		// Token: 0x04001AF3 RID: 6899
		private readonly AsymmetricKeyParameter privateParameter;
	}
}
