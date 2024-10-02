using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C4 RID: 1220
	public class DHKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002F79 RID: 12153 RVA: 0x00125A08 File Offset: 0x00123C08
		public DHKeyGenerationParameters(SecureRandom random, DHParameters parameters) : base(random, DHKeyGenerationParameters.GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002F7A RID: 12154 RVA: 0x00125A1E File Offset: 0x00123C1E
		public DHParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x00125A26 File Offset: 0x00123C26
		internal static int GetStrength(DHParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}

		// Token: 0x04001FB0 RID: 8112
		private readonly DHParameters parameters;
	}
}
