﻿using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BB RID: 1467
	public class Sha512tDigest : LongDigest
	{
		// Token: 0x06003847 RID: 14407 RVA: 0x00161420 File Offset: 0x0015F620
		public Sha512tDigest(int bitLength)
		{
			if (bitLength >= 512)
			{
				throw new ArgumentException("cannot be >= 512", "bitLength");
			}
			if (bitLength % 8 != 0)
			{
				throw new ArgumentException("needs to be a multiple of 8", "bitLength");
			}
			if (bitLength == 384)
			{
				throw new ArgumentException("cannot be 384 use SHA384 instead", "bitLength");
			}
			this.digestLength = bitLength / 8;
			this.tIvGenerate(this.digestLength * 8);
			this.Reset();
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00161495 File Offset: 0x0015F695
		public Sha512tDigest(Sha512tDigest t) : base(t)
		{
			this.digestLength = t.digestLength;
			this.Reset(t);
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x001614B1 File Offset: 0x0015F6B1
		public override string AlgorithmName
		{
			get
			{
				return "SHA-512/" + this.digestLength * 8;
			}
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x001614CA File Offset: 0x0015F6CA
		public override int GetDigestSize()
		{
			return this.digestLength;
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x001614D4 File Offset: 0x0015F6D4
		public override int DoFinal(byte[] output, int outOff)
		{
			base.Finish();
			Sha512tDigest.UInt64_To_BE(this.H1, output, outOff, this.digestLength);
			Sha512tDigest.UInt64_To_BE(this.H2, output, outOff + 8, this.digestLength - 8);
			Sha512tDigest.UInt64_To_BE(this.H3, output, outOff + 16, this.digestLength - 16);
			Sha512tDigest.UInt64_To_BE(this.H4, output, outOff + 24, this.digestLength - 24);
			Sha512tDigest.UInt64_To_BE(this.H5, output, outOff + 32, this.digestLength - 32);
			Sha512tDigest.UInt64_To_BE(this.H6, output, outOff + 40, this.digestLength - 40);
			Sha512tDigest.UInt64_To_BE(this.H7, output, outOff + 48, this.digestLength - 48);
			Sha512tDigest.UInt64_To_BE(this.H8, output, outOff + 56, this.digestLength - 56);
			this.Reset();
			return this.digestLength;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x001615B4 File Offset: 0x0015F7B4
		public override void Reset()
		{
			base.Reset();
			this.H1 = this.H1t;
			this.H2 = this.H2t;
			this.H3 = this.H3t;
			this.H4 = this.H4t;
			this.H5 = this.H5t;
			this.H6 = this.H6t;
			this.H7 = this.H7t;
			this.H8 = this.H8t;
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x00161628 File Offset: 0x0015F828
		private void tIvGenerate(int bitLength)
		{
			this.H1 = 14964410163792538797UL;
			this.H2 = 2216346199247487646UL;
			this.H3 = 11082046791023156622UL;
			this.H4 = 65953792586715988UL;
			this.H5 = 17630457682085488500UL;
			this.H6 = 4512832404995164602UL;
			this.H7 = 13413544941332994254UL;
			this.H8 = 18322165818757711068UL;
			base.Update(83);
			base.Update(72);
			base.Update(65);
			base.Update(45);
			base.Update(53);
			base.Update(49);
			base.Update(50);
			base.Update(47);
			if (bitLength > 100)
			{
				base.Update((byte)(bitLength / 100 + 48));
				bitLength %= 100;
				base.Update((byte)(bitLength / 10 + 48));
				bitLength %= 10;
				base.Update((byte)(bitLength + 48));
			}
			else if (bitLength > 10)
			{
				base.Update((byte)(bitLength / 10 + 48));
				bitLength %= 10;
				base.Update((byte)(bitLength + 48));
			}
			else
			{
				base.Update((byte)(bitLength + 48));
			}
			base.Finish();
			this.H1t = this.H1;
			this.H2t = this.H2;
			this.H3t = this.H3;
			this.H4t = this.H4;
			this.H5t = this.H5;
			this.H6t = this.H6;
			this.H7t = this.H7;
			this.H8t = this.H8;
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x001617BE File Offset: 0x0015F9BE
		private static void UInt64_To_BE(ulong n, byte[] bs, int off, int max)
		{
			if (max > 0)
			{
				Sha512tDigest.UInt32_To_BE((uint)(n >> 32), bs, off, max);
				if (max > 4)
				{
					Sha512tDigest.UInt32_To_BE((uint)n, bs, off + 4, max - 4);
				}
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x001617E4 File Offset: 0x0015F9E4
		private static void UInt32_To_BE(uint n, byte[] bs, int off, int max)
		{
			int num = Math.Min(4, max);
			while (--num >= 0)
			{
				int num2 = 8 * (3 - num);
				bs[off + num] = (byte)(n >> num2);
			}
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x00161815 File Offset: 0x0015FA15
		public override IMemoable Copy()
		{
			return new Sha512tDigest(this);
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00161820 File Offset: 0x0015FA20
		public override void Reset(IMemoable other)
		{
			Sha512tDigest sha512tDigest = (Sha512tDigest)other;
			if (this.digestLength != sha512tDigest.digestLength)
			{
				throw new MemoableResetException("digestLength inappropriate in other");
			}
			base.CopyIn(sha512tDigest);
			this.H1t = sha512tDigest.H1t;
			this.H2t = sha512tDigest.H2t;
			this.H3t = sha512tDigest.H3t;
			this.H4t = sha512tDigest.H4t;
			this.H5t = sha512tDigest.H5t;
			this.H6t = sha512tDigest.H6t;
			this.H7t = sha512tDigest.H7t;
			this.H8t = sha512tDigest.H8t;
		}

		// Token: 0x040024AD RID: 9389
		private const ulong A5 = 11936128518282651045UL;

		// Token: 0x040024AE RID: 9390
		private readonly int digestLength;

		// Token: 0x040024AF RID: 9391
		private ulong H1t;

		// Token: 0x040024B0 RID: 9392
		private ulong H2t;

		// Token: 0x040024B1 RID: 9393
		private ulong H3t;

		// Token: 0x040024B2 RID: 9394
		private ulong H4t;

		// Token: 0x040024B3 RID: 9395
		private ulong H5t;

		// Token: 0x040024B4 RID: 9396
		private ulong H6t;

		// Token: 0x040024B5 RID: 9397
		private ulong H7t;

		// Token: 0x040024B6 RID: 9398
		private ulong H8t;
	}
}
