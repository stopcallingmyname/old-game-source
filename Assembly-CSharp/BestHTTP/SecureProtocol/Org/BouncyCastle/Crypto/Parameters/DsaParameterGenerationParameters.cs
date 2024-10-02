using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CC RID: 1228
	public class DsaParameterGenerationParameters
	{
		// Token: 0x06002FAE RID: 12206 RVA: 0x00126073 File Offset: 0x00124273
		public DsaParameterGenerationParameters(int L, int N, int certainty, SecureRandom random) : this(L, N, certainty, random, -1)
		{
		}

		// Token: 0x06002FAF RID: 12207 RVA: 0x00126081 File Offset: 0x00124281
		public DsaParameterGenerationParameters(int L, int N, int certainty, SecureRandom random, int usageIndex)
		{
			this.l = L;
			this.n = N;
			this.certainty = certainty;
			this.random = random;
			this.usageIndex = usageIndex;
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002FB0 RID: 12208 RVA: 0x001260AE File Offset: 0x001242AE
		public virtual int L
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002FB1 RID: 12209 RVA: 0x001260B6 File Offset: 0x001242B6
		public virtual int N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002FB2 RID: 12210 RVA: 0x001260BE File Offset: 0x001242BE
		public virtual int UsageIndex
		{
			get
			{
				return this.usageIndex;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06002FB3 RID: 12211 RVA: 0x001260C6 File Offset: 0x001242C6
		public virtual int Certainty
		{
			get
			{
				return this.certainty;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002FB4 RID: 12212 RVA: 0x001260CE File Offset: 0x001242CE
		public virtual SecureRandom Random
		{
			get
			{
				return this.random;
			}
		}

		// Token: 0x04001FC1 RID: 8129
		public const int DigitalSignatureUsage = 1;

		// Token: 0x04001FC2 RID: 8130
		public const int KeyEstablishmentUsage = 2;

		// Token: 0x04001FC3 RID: 8131
		private readonly int l;

		// Token: 0x04001FC4 RID: 8132
		private readonly int n;

		// Token: 0x04001FC5 RID: 8133
		private readonly int certainty;

		// Token: 0x04001FC6 RID: 8134
		private readonly SecureRandom random;

		// Token: 0x04001FC7 RID: 8135
		private readonly int usageIndex;
	}
}
