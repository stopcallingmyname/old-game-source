using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200053A RID: 1338
	public class SipHash : IMac
	{
		// Token: 0x060032AC RID: 12972 RVA: 0x0013172A File Offset: 0x0012F92A
		public SipHash() : this(2, 4)
		{
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00131734 File Offset: 0x0012F934
		public SipHash(int c, int d)
		{
			this.c = c;
			this.d = d;
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x060032AE RID: 12974 RVA: 0x0013174A File Offset: 0x0012F94A
		public virtual string AlgorithmName
		{
			get
			{
				return string.Concat(new object[]
				{
					"SipHash-",
					this.c,
					"-",
					this.d
				});
			}
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetMacSize()
		{
			return 8;
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x00131784 File Offset: 0x0012F984
		public virtual void Init(ICipherParameters parameters)
		{
			KeyParameter keyParameter = parameters as KeyParameter;
			if (keyParameter == null)
			{
				throw new ArgumentException("must be an instance of KeyParameter", "parameters");
			}
			byte[] key = keyParameter.GetKey();
			if (key.Length != 16)
			{
				throw new ArgumentException("must be a 128-bit key", "parameters");
			}
			this.k0 = (long)Pack.LE_To_UInt64(key, 0);
			this.k1 = (long)Pack.LE_To_UInt64(key, 8);
			this.Reset();
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x001317E8 File Offset: 0x0012F9E8
		public virtual void Update(byte input)
		{
			this.m = (long)((ulong)this.m >> 8 | (ulong)input << 56);
			int num = this.wordPos + 1;
			this.wordPos = num;
			if (num == 8)
			{
				this.ProcessMessageWord();
				this.wordPos = 0;
			}
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x0013182C File Offset: 0x0012FA2C
		public virtual void BlockUpdate(byte[] input, int offset, int length)
		{
			int i = 0;
			int num = length & -8;
			if (this.wordPos == 0)
			{
				while (i < num)
				{
					this.m = (long)Pack.LE_To_UInt64(input, offset + i);
					this.ProcessMessageWord();
					i += 8;
				}
				while (i < length)
				{
					this.m = (long)((ulong)this.m >> 8 | (ulong)input[offset + i] << 56);
					i++;
				}
				this.wordPos = length - num;
				return;
			}
			int num2 = this.wordPos << 3;
			while (i < num)
			{
				ulong num3 = Pack.LE_To_UInt64(input, offset + i);
				this.m = (long)(num3 << num2 | (ulong)this.m >> -num2);
				this.ProcessMessageWord();
				this.m = (long)num3;
				i += 8;
			}
			while (i < length)
			{
				this.m = (long)((ulong)this.m >> 8 | (ulong)input[offset + i] << 56);
				int num4 = this.wordPos + 1;
				this.wordPos = num4;
				if (num4 == 8)
				{
					this.ProcessMessageWord();
					this.wordPos = 0;
				}
				i++;
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x0013191C File Offset: 0x0012FB1C
		public virtual long DoFinal()
		{
			this.m = (long)((ulong)this.m >> (7 - this.wordPos << 3));
			this.m = (long)((ulong)this.m >> 8);
			this.m |= (long)((this.wordCount << 3) + this.wordPos) << 56;
			this.ProcessMessageWord();
			this.v2 ^= 255L;
			this.ApplySipRounds(this.d);
			long result = this.v0 ^ this.v1 ^ this.v2 ^ this.v3;
			this.Reset();
			return result;
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x001319B7 File Offset: 0x0012FBB7
		public virtual int DoFinal(byte[] output, int outOff)
		{
			Pack.UInt64_To_LE((ulong)this.DoFinal(), output, outOff);
			return 8;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x001319C8 File Offset: 0x0012FBC8
		public virtual void Reset()
		{
			this.v0 = (this.k0 ^ 8317987319222330741L);
			this.v1 = (this.k1 ^ 7237128888997146477L);
			this.v2 = (this.k0 ^ 7816392313619706465L);
			this.v3 = (this.k1 ^ 8387220255154660723L);
			this.m = 0L;
			this.wordPos = 0;
			this.wordCount = 0;
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x00131A44 File Offset: 0x0012FC44
		protected virtual void ProcessMessageWord()
		{
			this.wordCount++;
			this.v3 ^= this.m;
			this.ApplySipRounds(this.c);
			this.v0 ^= this.m;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00131A94 File Offset: 0x0012FC94
		protected virtual void ApplySipRounds(int n)
		{
			long num = this.v0;
			long num2 = this.v1;
			long num3 = this.v2;
			long num4 = this.v3;
			for (int i = 0; i < n; i++)
			{
				num += num2;
				num3 += num4;
				num2 = SipHash.RotateLeft(num2, 13);
				num4 = SipHash.RotateLeft(num4, 16);
				num2 ^= num;
				num4 ^= num3;
				num = SipHash.RotateLeft(num, 32);
				num3 += num2;
				num += num4;
				num2 = SipHash.RotateLeft(num2, 17);
				num4 = SipHash.RotateLeft(num4, 21);
				num2 ^= num3;
				num4 ^= num;
				num3 = SipHash.RotateLeft(num3, 32);
			}
			this.v0 = num;
			this.v1 = num2;
			this.v2 = num3;
			this.v3 = num4;
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x00131B40 File Offset: 0x0012FD40
		protected static long RotateLeft(long x, int n)
		{
			return x << n | (long)((ulong)x >> -n);
		}

		// Token: 0x0400214E RID: 8526
		protected readonly int c;

		// Token: 0x0400214F RID: 8527
		protected readonly int d;

		// Token: 0x04002150 RID: 8528
		protected long k0;

		// Token: 0x04002151 RID: 8529
		protected long k1;

		// Token: 0x04002152 RID: 8530
		protected long v0;

		// Token: 0x04002153 RID: 8531
		protected long v1;

		// Token: 0x04002154 RID: 8532
		protected long v2;

		// Token: 0x04002155 RID: 8533
		protected long v3;

		// Token: 0x04002156 RID: 8534
		protected long m;

		// Token: 0x04002157 RID: 8535
		protected int wordPos;

		// Token: 0x04002158 RID: 8536
		protected int wordCount;
	}
}
