using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CA RID: 1226
	public class DsaKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002FA7 RID: 12199 RVA: 0x00125FC7 File Offset: 0x001241C7
		public DsaKeyGenerationParameters(SecureRandom random, DsaParameters parameters) : base(random, parameters.P.BitLength - 1)
		{
			this.parameters = parameters;
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x06002FA8 RID: 12200 RVA: 0x00125FE4 File Offset: 0x001241E4
		public DsaParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04001FBF RID: 8127
		private readonly DsaParameters parameters;
	}
}
