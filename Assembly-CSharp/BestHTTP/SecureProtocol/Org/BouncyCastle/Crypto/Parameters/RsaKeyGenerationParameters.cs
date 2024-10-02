using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FB RID: 1275
	public class RsaKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x060030A2 RID: 12450 RVA: 0x00127BF5 File Offset: 0x00125DF5
		public RsaKeyGenerationParameters(BigInteger publicExponent, SecureRandom random, int strength, int certainty) : base(random, strength)
		{
			this.publicExponent = publicExponent;
			this.certainty = certainty;
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060030A3 RID: 12451 RVA: 0x00127C0E File Offset: 0x00125E0E
		public BigInteger PublicExponent
		{
			get
			{
				return this.publicExponent;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060030A4 RID: 12452 RVA: 0x00127C16 File Offset: 0x00125E16
		public int Certainty
		{
			get
			{
				return this.certainty;
			}
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x00127C20 File Offset: 0x00125E20
		public override bool Equals(object obj)
		{
			RsaKeyGenerationParameters rsaKeyGenerationParameters = obj as RsaKeyGenerationParameters;
			return rsaKeyGenerationParameters != null && this.certainty == rsaKeyGenerationParameters.certainty && this.publicExponent.Equals(rsaKeyGenerationParameters.publicExponent);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x00127C5C File Offset: 0x00125E5C
		public override int GetHashCode()
		{
			return this.certainty.GetHashCode() ^ this.publicExponent.GetHashCode();
		}

		// Token: 0x04002025 RID: 8229
		private readonly BigInteger publicExponent;

		// Token: 0x04002026 RID: 8230
		private readonly int certainty;
	}
}
