using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000501 RID: 1281
	public sealed class Srp6GroupParameters
	{
		// Token: 0x060030CA RID: 12490 RVA: 0x0012823F File Offset: 0x0012643F
		public Srp6GroupParameters(BigInteger N, BigInteger g)
		{
			this.n = N;
			this.g = g;
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x00128255 File Offset: 0x00126455
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x0012825D File Offset: 0x0012645D
		public BigInteger N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x04002040 RID: 8256
		private readonly BigInteger n;

		// Token: 0x04002041 RID: 8257
		private readonly BigInteger g;
	}
}
