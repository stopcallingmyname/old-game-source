using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005AE RID: 1454
	public class MD5Digest : GeneralDigest
	{
		// Token: 0x0600377F RID: 14207 RVA: 0x00158813 File Offset: 0x00156A13
		public MD5Digest()
		{
			this.Reset();
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x0015882E File Offset: 0x00156A2E
		public MD5Digest(MD5Digest t) : base(t)
		{
			this.CopyIn(t);
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0015884C File Offset: 0x00156A4C
		private void CopyIn(MD5Digest t)
		{
			base.CopyIn(t);
			this.H1 = t.H1;
			this.H2 = t.H2;
			this.H3 = t.H3;
			this.H4 = t.H4;
			Array.Copy(t.X, 0, this.X, 0, t.X.Length);
			this.xOff = t.xOff;
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x001588B7 File Offset: 0x00156AB7
		public override string AlgorithmName
		{
			get
			{
				return "MD5";
			}
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0012AD29 File Offset: 0x00128F29
		public override int GetDigestSize()
		{
			return 16;
		}

		// Token: 0x06003784 RID: 14212 RVA: 0x001588C0 File Offset: 0x00156AC0
		internal override void ProcessWord(byte[] input, int inOff)
		{
			this.X[this.xOff] = Pack.LE_To_UInt32(input, inOff);
			int num = this.xOff + 1;
			this.xOff = num;
			if (num == 16)
			{
				this.ProcessBlock();
			}
		}

		// Token: 0x06003785 RID: 14213 RVA: 0x001588FC File Offset: 0x00156AFC
		internal override void ProcessLength(long bitLength)
		{
			if (this.xOff > 14)
			{
				if (this.xOff == 15)
				{
					this.X[15] = 0U;
				}
				this.ProcessBlock();
			}
			for (int i = this.xOff; i < 14; i++)
			{
				this.X[i] = 0U;
			}
			this.X[14] = (uint)bitLength;
			this.X[15] = (uint)((ulong)bitLength >> 32);
		}

		// Token: 0x06003786 RID: 14214 RVA: 0x00158964 File Offset: 0x00156B64
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Pack.UInt32_To_LE(this.H1, output, outOff);
			Pack.UInt32_To_LE(this.H2, output, outOff + 4);
			Pack.UInt32_To_LE(this.H3, output, outOff + 8);
			Pack.UInt32_To_LE(this.H4, output, outOff + 12);
			this.Reset();
			return 16;
		}

		// Token: 0x06003787 RID: 14215 RVA: 0x001589BC File Offset: 0x00156BBC
		public override void Reset()
		{
			base.Reset();
			this.H1 = 1732584193U;
			this.H2 = 4023233417U;
			this.H3 = 2562383102U;
			this.H4 = 271733878U;
			this.xOff = 0;
			for (int num = 0; num != this.X.Length; num++)
			{
				this.X[num] = 0U;
			}
		}

		// Token: 0x06003788 RID: 14216 RVA: 0x00148EDA File Offset: 0x001470DA
		private static uint RotateLeft(uint x, int n)
		{
			return x << n | x >> 32 - n;
		}

		// Token: 0x06003789 RID: 14217 RVA: 0x00158A1E File Offset: 0x00156C1E
		private static uint F(uint u, uint v, uint w)
		{
			return (u & v) | (~u & w);
		}

		// Token: 0x0600378A RID: 14218 RVA: 0x00158A28 File Offset: 0x00156C28
		private static uint G(uint u, uint v, uint w)
		{
			return (u & w) | (v & ~w);
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x00158A32 File Offset: 0x00156C32
		private static uint H(uint u, uint v, uint w)
		{
			return u ^ v ^ w;
		}

		// Token: 0x0600378C RID: 14220 RVA: 0x00158A39 File Offset: 0x00156C39
		private static uint K(uint u, uint v, uint w)
		{
			return v ^ (u | ~w);
		}

		// Token: 0x0600378D RID: 14221 RVA: 0x00158A44 File Offset: 0x00156C44
		internal override void ProcessBlock()
		{
			uint num = this.H1;
			uint num2 = this.H2;
			uint num3 = this.H3;
			uint num4 = this.H4;
			num = MD5Digest.RotateLeft(num + MD5Digest.F(num2, num3, num4) + this.X[0] + 3614090360U, MD5Digest.S11) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.F(num, num2, num3) + this.X[1] + 3905402710U, MD5Digest.S12) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.F(num4, num, num2) + this.X[2] + 606105819U, MD5Digest.S13) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.F(num3, num4, num) + this.X[3] + 3250441966U, MD5Digest.S14) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.F(num2, num3, num4) + this.X[4] + 4118548399U, MD5Digest.S11) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.F(num, num2, num3) + this.X[5] + 1200080426U, MD5Digest.S12) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.F(num4, num, num2) + this.X[6] + 2821735955U, MD5Digest.S13) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.F(num3, num4, num) + this.X[7] + 4249261313U, MD5Digest.S14) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.F(num2, num3, num4) + this.X[8] + 1770035416U, MD5Digest.S11) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.F(num, num2, num3) + this.X[9] + 2336552879U, MD5Digest.S12) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.F(num4, num, num2) + this.X[10] + 4294925233U, MD5Digest.S13) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.F(num3, num4, num) + this.X[11] + 2304563134U, MD5Digest.S14) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.F(num2, num3, num4) + this.X[12] + 1804603682U, MD5Digest.S11) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.F(num, num2, num3) + this.X[13] + 4254626195U, MD5Digest.S12) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.F(num4, num, num2) + this.X[14] + 2792965006U, MD5Digest.S13) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.F(num3, num4, num) + this.X[15] + 1236535329U, MD5Digest.S14) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.G(num2, num3, num4) + this.X[1] + 4129170786U, MD5Digest.S21) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.G(num, num2, num3) + this.X[6] + 3225465664U, MD5Digest.S22) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.G(num4, num, num2) + this.X[11] + 643717713U, MD5Digest.S23) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.G(num3, num4, num) + this.X[0] + 3921069994U, MD5Digest.S24) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.G(num2, num3, num4) + this.X[5] + 3593408605U, MD5Digest.S21) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.G(num, num2, num3) + this.X[10] + 38016083U, MD5Digest.S22) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.G(num4, num, num2) + this.X[15] + 3634488961U, MD5Digest.S23) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.G(num3, num4, num) + this.X[4] + 3889429448U, MD5Digest.S24) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.G(num2, num3, num4) + this.X[9] + 568446438U, MD5Digest.S21) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.G(num, num2, num3) + this.X[14] + 3275163606U, MD5Digest.S22) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.G(num4, num, num2) + this.X[3] + 4107603335U, MD5Digest.S23) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.G(num3, num4, num) + this.X[8] + 1163531501U, MD5Digest.S24) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.G(num2, num3, num4) + this.X[13] + 2850285829U, MD5Digest.S21) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.G(num, num2, num3) + this.X[2] + 4243563512U, MD5Digest.S22) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.G(num4, num, num2) + this.X[7] + 1735328473U, MD5Digest.S23) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.G(num3, num4, num) + this.X[12] + 2368359562U, MD5Digest.S24) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.H(num2, num3, num4) + this.X[5] + 4294588738U, MD5Digest.S31) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.H(num, num2, num3) + this.X[8] + 2272392833U, MD5Digest.S32) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.H(num4, num, num2) + this.X[11] + 1839030562U, MD5Digest.S33) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.H(num3, num4, num) + this.X[14] + 4259657740U, MD5Digest.S34) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.H(num2, num3, num4) + this.X[1] + 2763975236U, MD5Digest.S31) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.H(num, num2, num3) + this.X[4] + 1272893353U, MD5Digest.S32) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.H(num4, num, num2) + this.X[7] + 4139469664U, MD5Digest.S33) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.H(num3, num4, num) + this.X[10] + 3200236656U, MD5Digest.S34) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.H(num2, num3, num4) + this.X[13] + 681279174U, MD5Digest.S31) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.H(num, num2, num3) + this.X[0] + 3936430074U, MD5Digest.S32) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.H(num4, num, num2) + this.X[3] + 3572445317U, MD5Digest.S33) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.H(num3, num4, num) + this.X[6] + 76029189U, MD5Digest.S34) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.H(num2, num3, num4) + this.X[9] + 3654602809U, MD5Digest.S31) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.H(num, num2, num3) + this.X[12] + 3873151461U, MD5Digest.S32) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.H(num4, num, num2) + this.X[15] + 530742520U, MD5Digest.S33) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.H(num3, num4, num) + this.X[2] + 3299628645U, MD5Digest.S34) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.K(num2, num3, num4) + this.X[0] + 4096336452U, MD5Digest.S41) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.K(num, num2, num3) + this.X[7] + 1126891415U, MD5Digest.S42) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.K(num4, num, num2) + this.X[14] + 2878612391U, MD5Digest.S43) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.K(num3, num4, num) + this.X[5] + 4237533241U, MD5Digest.S44) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.K(num2, num3, num4) + this.X[12] + 1700485571U, MD5Digest.S41) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.K(num, num2, num3) + this.X[3] + 2399980690U, MD5Digest.S42) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.K(num4, num, num2) + this.X[10] + 4293915773U, MD5Digest.S43) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.K(num3, num4, num) + this.X[1] + 2240044497U, MD5Digest.S44) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.K(num2, num3, num4) + this.X[8] + 1873313359U, MD5Digest.S41) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.K(num, num2, num3) + this.X[15] + 4264355552U, MD5Digest.S42) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.K(num4, num, num2) + this.X[6] + 2734768916U, MD5Digest.S43) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.K(num3, num4, num) + this.X[13] + 1309151649U, MD5Digest.S44) + num3;
			num = MD5Digest.RotateLeft(num + MD5Digest.K(num2, num3, num4) + this.X[4] + 4149444226U, MD5Digest.S41) + num2;
			num4 = MD5Digest.RotateLeft(num4 + MD5Digest.K(num, num2, num3) + this.X[11] + 3174756917U, MD5Digest.S42) + num;
			num3 = MD5Digest.RotateLeft(num3 + MD5Digest.K(num4, num, num2) + this.X[2] + 718787259U, MD5Digest.S43) + num4;
			num2 = MD5Digest.RotateLeft(num2 + MD5Digest.K(num3, num4, num) + this.X[9] + 3951481745U, MD5Digest.S44) + num3;
			this.H1 += num;
			this.H2 += num2;
			this.H3 += num3;
			this.H4 += num4;
			this.xOff = 0;
		}

		// Token: 0x0600378E RID: 14222 RVA: 0x00159448 File Offset: 0x00157648
		public override IMemoable Copy()
		{
			return new MD5Digest(this);
		}

		// Token: 0x0600378F RID: 14223 RVA: 0x00159450 File Offset: 0x00157650
		public override void Reset(IMemoable other)
		{
			MD5Digest t = (MD5Digest)other;
			this.CopyIn(t);
		}

		// Token: 0x04002447 RID: 9287
		private const int DigestLength = 16;

		// Token: 0x04002448 RID: 9288
		private uint H1;

		// Token: 0x04002449 RID: 9289
		private uint H2;

		// Token: 0x0400244A RID: 9290
		private uint H3;

		// Token: 0x0400244B RID: 9291
		private uint H4;

		// Token: 0x0400244C RID: 9292
		private uint[] X = new uint[16];

		// Token: 0x0400244D RID: 9293
		private int xOff;

		// Token: 0x0400244E RID: 9294
		private static readonly int S11 = 7;

		// Token: 0x0400244F RID: 9295
		private static readonly int S12 = 12;

		// Token: 0x04002450 RID: 9296
		private static readonly int S13 = 17;

		// Token: 0x04002451 RID: 9297
		private static readonly int S14 = 22;

		// Token: 0x04002452 RID: 9298
		private static readonly int S21 = 5;

		// Token: 0x04002453 RID: 9299
		private static readonly int S22 = 9;

		// Token: 0x04002454 RID: 9300
		private static readonly int S23 = 14;

		// Token: 0x04002455 RID: 9301
		private static readonly int S24 = 20;

		// Token: 0x04002456 RID: 9302
		private static readonly int S31 = 4;

		// Token: 0x04002457 RID: 9303
		private static readonly int S32 = 11;

		// Token: 0x04002458 RID: 9304
		private static readonly int S33 = 16;

		// Token: 0x04002459 RID: 9305
		private static readonly int S34 = 23;

		// Token: 0x0400245A RID: 9306
		private static readonly int S41 = 6;

		// Token: 0x0400245B RID: 9307
		private static readonly int S42 = 10;

		// Token: 0x0400245C RID: 9308
		private static readonly int S43 = 15;

		// Token: 0x0400245D RID: 9309
		private static readonly int S44 = 21;
	}
}
