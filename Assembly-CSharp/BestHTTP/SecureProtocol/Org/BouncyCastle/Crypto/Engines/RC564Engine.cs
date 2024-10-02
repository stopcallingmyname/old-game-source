using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000585 RID: 1413
	public class RC564Engine : IBlockCipher
	{
		// Token: 0x06003551 RID: 13649 RVA: 0x0014632E File Offset: 0x0014452E
		public RC564Engine()
		{
			this._noRounds = 12;
		}

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003552 RID: 13650 RVA: 0x0014633E File Offset: 0x0014453E
		public virtual string AlgorithmName
		{
			get
			{
				return "RC5-64";
			}
		}

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06003553 RID: 13651 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x00146345 File Offset: 0x00144545
		public virtual int GetBlockSize()
		{
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00146350 File Offset: 0x00144550
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!typeof(RC5Parameters).IsInstanceOfType(parameters))
			{
				throw new ArgumentException("invalid parameter passed to RC564 init - " + Platform.GetTypeName(parameters));
			}
			RC5Parameters rc5Parameters = (RC5Parameters)parameters;
			this.forEncryption = forEncryption;
			this._noRounds = rc5Parameters.Rounds;
			this.SetKey(rc5Parameters.GetKey());
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x001463AB File Offset: 0x001445AB
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x001463CC File Offset: 0x001445CC
		private void SetKey(byte[] key)
		{
			long[] array = new long[(key.Length + (RC564Engine.bytesPerWord - 1)) / RC564Engine.bytesPerWord];
			for (int num = 0; num != key.Length; num++)
			{
				array[num / RC564Engine.bytesPerWord] += (long)(key[num] & byte.MaxValue) << 8 * (num % RC564Engine.bytesPerWord);
			}
			this._S = new long[2 * (this._noRounds + 1)];
			this._S[0] = RC564Engine.P64;
			for (int i = 1; i < this._S.Length; i++)
			{
				this._S[i] = this._S[i - 1] + RC564Engine.Q64;
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
			long num3 = 0L;
			long num4 = 0L;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < num2; j++)
			{
				num3 = (this._S[num5] = this.RotateLeft(this._S[num5] + num3 + num4, 3L));
				num4 = (array[num6] = this.RotateLeft(array[num6] + num3 + num4, num3 + num4));
				num5 = (num5 + 1) % this._S.Length;
				num6 = (num6 + 1) % array.Length;
			}
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x00146514 File Offset: 0x00144714
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			long num = this.BytesToWord(input, inOff) + this._S[0];
			long num2 = this.BytesToWord(input, inOff + RC564Engine.bytesPerWord) + this._S[1];
			for (int i = 1; i <= this._noRounds; i++)
			{
				num = this.RotateLeft(num ^ num2, num2) + this._S[2 * i];
				num2 = this.RotateLeft(num2 ^ num, num) + this._S[2 * i + 1];
			}
			this.WordToBytes(num, outBytes, outOff);
			this.WordToBytes(num2, outBytes, outOff + RC564Engine.bytesPerWord);
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x001465AC File Offset: 0x001447AC
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			long num = this.BytesToWord(input, inOff);
			long num2 = this.BytesToWord(input, inOff + RC564Engine.bytesPerWord);
			for (int i = this._noRounds; i >= 1; i--)
			{
				num2 = (this.RotateRight(num2 - this._S[2 * i + 1], num) ^ num);
				num = (this.RotateRight(num - this._S[2 * i], num2) ^ num2);
			}
			this.WordToBytes(num - this._S[0], outBytes, outOff);
			this.WordToBytes(num2 - this._S[1], outBytes, outOff + RC564Engine.bytesPerWord);
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x00146643 File Offset: 0x00144843
		private long RotateLeft(long x, long y)
		{
			return x << (int)(y & (long)(RC564Engine.wordSize - 1)) | (long)((ulong)x >> (int)((long)RC564Engine.wordSize - (y & (long)(RC564Engine.wordSize - 1))));
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x0014666D File Offset: 0x0014486D
		private long RotateRight(long x, long y)
		{
			return (long)((ulong)x >> (int)(y & (long)(RC564Engine.wordSize - 1)) | (ulong)((ulong)x << (int)((long)RC564Engine.wordSize - (y & (long)(RC564Engine.wordSize - 1)))));
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x00146698 File Offset: 0x00144898
		private long BytesToWord(byte[] src, int srcOff)
		{
			long num = 0L;
			for (int i = RC564Engine.bytesPerWord - 1; i >= 0; i--)
			{
				num = (num << 8) + (long)(src[i + srcOff] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x001466CC File Offset: 0x001448CC
		private void WordToBytes(long word, byte[] dst, int dstOff)
		{
			for (int i = 0; i < RC564Engine.bytesPerWord; i++)
			{
				dst[i + dstOff] = (byte)word;
				word = (long)((ulong)word >> 8);
			}
		}

		// Token: 0x040022DD RID: 8925
		private static readonly int wordSize = 64;

		// Token: 0x040022DE RID: 8926
		private static readonly int bytesPerWord = RC564Engine.wordSize / 8;

		// Token: 0x040022DF RID: 8927
		private int _noRounds;

		// Token: 0x040022E0 RID: 8928
		private long[] _S;

		// Token: 0x040022E1 RID: 8929
		private static readonly long P64 = -5196783011329398165L;

		// Token: 0x040022E2 RID: 8930
		private static readonly long Q64 = -7046029254386353131L;

		// Token: 0x040022E3 RID: 8931
		private bool forEncryption;
	}
}
