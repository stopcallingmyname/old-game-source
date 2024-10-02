using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000527 RID: 1319
	public class BasicGcmExponentiator : IGcmExponentiator
	{
		// Token: 0x06003200 RID: 12800 RVA: 0x0012E0B3 File Offset: 0x0012C2B3
		public void Init(byte[] x)
		{
			this.x = GcmUtilities.AsUints(x);
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x0012E0C4 File Offset: 0x0012C2C4
		public void ExponentiateX(long pow, byte[] output)
		{
			uint[] array = GcmUtilities.OneAsUints();
			if (pow > 0L)
			{
				uint[] y = Arrays.Clone(this.x);
				do
				{
					if ((pow & 1L) != 0L)
					{
						GcmUtilities.Multiply(array, y);
					}
					GcmUtilities.Multiply(y, y);
					pow >>= 1;
				}
				while (pow > 0L);
			}
			GcmUtilities.AsBytes(array, output);
		}

		// Token: 0x040020E8 RID: 8424
		private uint[] x;
	}
}
