using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BA RID: 1466
	public class Sha512Digest : LongDigest
	{
		// Token: 0x0600383F RID: 14399 RVA: 0x00161078 File Offset: 0x0015F278
		public Sha512Digest()
		{
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00161080 File Offset: 0x0015F280
		public Sha512Digest(Sha512Digest t) : base(t)
		{
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06003841 RID: 14401 RVA: 0x001612CE File Offset: 0x0015F4CE
		public override string AlgorithmName
		{
			get
			{
				return "SHA-512";
			}
		}

		// Token: 0x06003842 RID: 14402 RVA: 0x00153D0C File Offset: 0x00151F0C
		public override int GetDigestSize()
		{
			return 64;
		}

		// Token: 0x06003843 RID: 14403 RVA: 0x001612D8 File Offset: 0x0015F4D8
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt64_To_BE(this.H1, output, outOff);
			Pack.UInt64_To_BE(this.H2, output, outOff + 8);
			Pack.UInt64_To_BE(this.H3, output, outOff + 16);
			Pack.UInt64_To_BE(this.H4, output, outOff + 24);
			Pack.UInt64_To_BE(this.H5, output, outOff + 32);
			Pack.UInt64_To_BE(this.H6, output, outOff + 40);
			Pack.UInt64_To_BE(this.H7, output, outOff + 48);
			Pack.UInt64_To_BE(this.H8, output, outOff + 56);
			this.Reset();
			return 64;
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x00161370 File Offset: 0x0015F570
		public override void Reset()
		{
			base.Reset();
			this.H1 = 7640891576956012808UL;
			this.H2 = 13503953896175478587UL;
			this.H3 = 4354685564936845355UL;
			this.H4 = 11912009170470909681UL;
			this.H5 = 5840696475078001361UL;
			this.H6 = 11170449401992604703UL;
			this.H7 = 2270897969802886507UL;
			this.H8 = 6620516959819538809UL;
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x001613FB File Offset: 0x0015F5FB
		public override IMemoable Copy()
		{
			return new Sha512Digest(this);
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00161404 File Offset: 0x0015F604
		public override void Reset(IMemoable other)
		{
			Sha512Digest t = (Sha512Digest)other;
			base.CopyIn(t);
		}

		// Token: 0x040024AC RID: 9388
		private const int DigestLength = 64;
	}
}
