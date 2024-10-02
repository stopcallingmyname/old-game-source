using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DC RID: 1244
	public class ElGamalKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x0600301A RID: 12314 RVA: 0x00126ED3 File Offset: 0x001250D3
		public ElGamalKeyGenerationParameters(SecureRandom random, ElGamalParameters parameters) : base(random, ElGamalKeyGenerationParameters.GetStrength(parameters))
		{
			this.parameters = parameters;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x00126EE9 File Offset: 0x001250E9
		public ElGamalParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x00126EF1 File Offset: 0x001250F1
		internal static int GetStrength(ElGamalParameters parameters)
		{
			if (parameters.L == 0)
			{
				return parameters.P.BitLength;
			}
			return parameters.L;
		}

		// Token: 0x04001FE9 RID: 8169
		private readonly ElGamalParameters parameters;
	}
}
