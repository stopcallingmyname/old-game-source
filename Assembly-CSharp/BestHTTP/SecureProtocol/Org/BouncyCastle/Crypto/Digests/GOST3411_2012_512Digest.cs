using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A7 RID: 1447
	public class GOST3411_2012_512Digest : GOST3411_2012Digest
	{
		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06003700 RID: 14080 RVA: 0x00154E85 File Offset: 0x00153085
		public override string AlgorithmName
		{
			get
			{
				return "GOST3411-2012-512";
			}
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00154E8C File Offset: 0x0015308C
		public GOST3411_2012_512Digest() : base(GOST3411_2012_512Digest.IV)
		{
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x00154E99 File Offset: 0x00153099
		public GOST3411_2012_512Digest(GOST3411_2012_512Digest other) : base(GOST3411_2012_512Digest.IV)
		{
			base.Reset(other);
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x00153D0C File Offset: 0x00151F0C
		public override int GetDigestSize()
		{
			return 64;
		}

		// Token: 0x06003704 RID: 14084 RVA: 0x00154EAD File Offset: 0x001530AD
		public override IMemoable Copy()
		{
			return new GOST3411_2012_512Digest(this);
		}

		// Token: 0x040023F3 RID: 9203
		private static readonly byte[] IV = new byte[64];
	}
}
