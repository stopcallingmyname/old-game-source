using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005A6 RID: 1446
	public class GOST3411_2012_256Digest : GOST3411_2012Digest
	{
		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060036F9 RID: 14073 RVA: 0x00154E0C File Offset: 0x0015300C
		public override string AlgorithmName
		{
			get
			{
				return "GOST3411-2012-256";
			}
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x00154E13 File Offset: 0x00153013
		public GOST3411_2012_256Digest() : base(GOST3411_2012_256Digest.IV)
		{
		}

		// Token: 0x060036FB RID: 14075 RVA: 0x00154E20 File Offset: 0x00153020
		public GOST3411_2012_256Digest(GOST3411_2012_256Digest other) : base(GOST3411_2012_256Digest.IV)
		{
			base.Reset(other);
		}

		// Token: 0x060036FC RID: 14076 RVA: 0x00154E34 File Offset: 0x00153034
		public override int GetDigestSize()
		{
			return 32;
		}

		// Token: 0x060036FD RID: 14077 RVA: 0x00154E38 File Offset: 0x00153038
		public override int DoFinal(byte[] output, int outOff)
		{
			byte[] array = new byte[64];
			base.DoFinal(array, 0);
			Array.Copy(array, 32, output, outOff, 32);
			return 32;
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x00154E64 File Offset: 0x00153064
		public override IMemoable Copy()
		{
			return new GOST3411_2012_256Digest(this);
		}

		// Token: 0x040023F2 RID: 9202
		private static readonly byte[] IV = new byte[]
		{
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1,
			1
		};
	}
}
