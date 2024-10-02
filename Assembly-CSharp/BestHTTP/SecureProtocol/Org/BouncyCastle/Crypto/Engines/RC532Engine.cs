using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000584 RID: 1412
	public class RC532Engine : IBlockCipher
	{
		// Token: 0x06003542 RID: 13634 RVA: 0x00145F83 File Offset: 0x00144183
		public RC532Engine()
		{
			this._noRounds = 12;
		}

		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x06003543 RID: 13635 RVA: 0x00145F93 File Offset: 0x00144193
		public virtual string AlgorithmName
		{
			get
			{
				return "RC5-32";
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06003544 RID: 13636 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x000FCEE9 File Offset: 0x000FB0E9
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00145F9C File Offset: 0x0014419C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (typeof(RC5Parameters).IsInstanceOfType(parameters))
			{
				RC5Parameters rc5Parameters = (RC5Parameters)parameters;
				this._noRounds = rc5Parameters.Rounds;
				this.SetKey(rc5Parameters.GetKey());
			}
			else
			{
				if (!typeof(KeyParameter).IsInstanceOfType(parameters))
				{
					throw new ArgumentException("invalid parameter passed to RC532 init - " + Platform.GetTypeName(parameters));
				}
				KeyParameter keyParameter = (KeyParameter)parameters;
				this.SetKey(keyParameter.GetKey());
			}
			this.forEncryption = forEncryption;
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x00146020 File Offset: 0x00144220
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x00146044 File Offset: 0x00144244
		private void SetKey(byte[] key)
		{
			int[] array = new int[(key.Length + 3) / 4];
			for (int num = 0; num != key.Length; num++)
			{
				array[num / 4] += (int)(key[num] & byte.MaxValue) << 8 * (num % 4);
			}
			this._S = new int[2 * (this._noRounds + 1)];
			this._S[0] = RC532Engine.P32;
			for (int i = 1; i < this._S.Length; i++)
			{
				this._S[i] = this._S[i - 1] + RC532Engine.Q32;
			}
			int num2;
			if (array.Length > this._S.Length)
			{
				num2 = 3 * array.Length;
			}
			else
			{
				num2 = 3 * this._S.Length;
			}
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < num2; j++)
			{
				num3 = (this._S[num5] = this.RotateLeft(this._S[num5] + num3 + num4, 3));
				num4 = (array[num6] = this.RotateLeft(array[num6] + num3 + num4, num3 + num4));
				num5 = (num5 + 1) % this._S.Length;
				num6 = (num6 + 1) % array.Length;
			}
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x00146178 File Offset: 0x00144378
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = this.BytesToWord(input, inOff) + this._S[0];
			int num2 = this.BytesToWord(input, inOff + 4) + this._S[1];
			for (int i = 1; i <= this._noRounds; i++)
			{
				num = this.RotateLeft(num ^ num2, num2) + this._S[2 * i];
				num2 = this.RotateLeft(num2 ^ num, num) + this._S[2 * i + 1];
			}
			this.WordToBytes(num, outBytes, outOff);
			this.WordToBytes(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x00146204 File Offset: 0x00144404
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = this.BytesToWord(input, inOff);
			int num2 = this.BytesToWord(input, inOff + 4);
			for (int i = this._noRounds; i >= 1; i--)
			{
				num2 = (this.RotateRight(num2 - this._S[2 * i + 1], num) ^ num);
				num = (this.RotateRight(num - this._S[2 * i], num2) ^ num2);
			}
			this.WordToBytes(num - this._S[0], outBytes, outOff);
			this.WordToBytes(num2 - this._S[1], outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x0014628D File Offset: 0x0014448D
		private int RotateLeft(int x, int y)
		{
			return x << y | (int)((uint)x >> 32 - (y & 31));
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x001462A5 File Offset: 0x001444A5
		private int RotateRight(int x, int y)
		{
			return (int)((uint)x >> y | (uint)((uint)x << 32 - (y & 31)));
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x001462BD File Offset: 0x001444BD
		private int BytesToWord(byte[] src, int srcOff)
		{
			return (int)(src[srcOff] & byte.MaxValue) | (int)(src[srcOff + 1] & byte.MaxValue) << 8 | (int)(src[srcOff + 2] & byte.MaxValue) << 16 | (int)(src[srcOff + 3] & byte.MaxValue) << 24;
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x001462F4 File Offset: 0x001444F4
		private void WordToBytes(int word, byte[] dst, int dstOff)
		{
			dst[dstOff] = (byte)word;
			dst[dstOff + 1] = (byte)(word >> 8);
			dst[dstOff + 2] = (byte)(word >> 16);
			dst[dstOff + 3] = (byte)(word >> 24);
		}

		// Token: 0x040022D8 RID: 8920
		private int _noRounds;

		// Token: 0x040022D9 RID: 8921
		private int[] _S;

		// Token: 0x040022DA RID: 8922
		private static readonly int P32 = -1209970333;

		// Token: 0x040022DB RID: 8923
		private static readonly int Q32 = -1640531527;

		// Token: 0x040022DC RID: 8924
		private bool forEncryption;
	}
}
