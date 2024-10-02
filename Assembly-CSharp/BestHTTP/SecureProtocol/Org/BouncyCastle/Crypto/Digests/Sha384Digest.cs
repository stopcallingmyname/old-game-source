using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005B8 RID: 1464
	public class Sha384Digest : LongDigest
	{
		// Token: 0x0600382F RID: 14383 RVA: 0x00161078 File Offset: 0x0015F278
		public Sha384Digest()
		{
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x00161080 File Offset: 0x0015F280
		public Sha384Digest(Sha384Digest t) : base(t)
		{
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06003831 RID: 14385 RVA: 0x00161089 File Offset: 0x0015F289
		public override string AlgorithmName
		{
			get
			{
				return "SHA-384";
			}
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x00161090 File Offset: 0x0015F290
		public override int GetDigestSize()
		{
			return 48;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x00161094 File Offset: 0x0015F294
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt64_To_BE(this.H1, output, outOff);
			Pack.UInt64_To_BE(this.H2, output, outOff + 8);
			Pack.UInt64_To_BE(this.H3, output, outOff + 16);
			Pack.UInt64_To_BE(this.H4, output, outOff + 24);
			Pack.UInt64_To_BE(this.H5, output, outOff + 32);
			Pack.UInt64_To_BE(this.H6, output, outOff + 40);
			this.Reset();
			return 48;
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x0016110C File Offset: 0x0015F30C
		public override void Reset()
		{
			base.Reset();
			this.H1 = 14680500436340154072UL;
			this.H2 = 7105036623409894663UL;
			this.H3 = 10473403895298186519UL;
			this.H4 = 1526699215303891257UL;
			this.H5 = 7436329637833083697UL;
			this.H6 = 10282925794625328401UL;
			this.H7 = 15784041429090275239UL;
			this.H8 = 5167115440072839076UL;
		}

		// Token: 0x06003835 RID: 14389 RVA: 0x00161197 File Offset: 0x0015F397
		public override IMemoable Copy()
		{
			return new Sha384Digest(this);
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x001611A0 File Offset: 0x0015F3A0
		public override void Reset(IMemoable other)
		{
			Sha384Digest t = (Sha384Digest)other;
			base.CopyIn(t);
		}

		// Token: 0x040024AB RID: 9387
		private const int DigestLength = 48;
	}
}
