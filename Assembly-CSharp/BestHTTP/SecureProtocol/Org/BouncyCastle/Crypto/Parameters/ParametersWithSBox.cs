using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F7 RID: 1271
	public class ParametersWithSBox : ICipherParameters
	{
		// Token: 0x06003095 RID: 12437 RVA: 0x00127B02 File Offset: 0x00125D02
		public ParametersWithSBox(ICipherParameters parameters, byte[] sBox)
		{
			this.parameters = parameters;
			this.sBox = sBox;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x00127B18 File Offset: 0x00125D18
		public byte[] GetSBox()
		{
			return this.sBox;
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x00127B20 File Offset: 0x00125D20
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0400201F RID: 8223
		private ICipherParameters parameters;

		// Token: 0x04002020 RID: 8224
		private byte[] sBox;
	}
}
