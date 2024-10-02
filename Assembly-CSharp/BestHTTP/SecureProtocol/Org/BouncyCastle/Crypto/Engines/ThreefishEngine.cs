using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000597 RID: 1431
	public class ThreefishEngine : IBlockCipher
	{
		// Token: 0x0600362D RID: 13869 RVA: 0x0014D0B0 File Offset: 0x0014B2B0
		static ThreefishEngine()
		{
			for (int i = 0; i < ThreefishEngine.MOD9.Length; i++)
			{
				ThreefishEngine.MOD17[i] = i % 17;
				ThreefishEngine.MOD9[i] = i % 9;
				ThreefishEngine.MOD5[i] = i % 5;
				ThreefishEngine.MOD3[i] = i % 3;
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x0014D138 File Offset: 0x0014B338
		public ThreefishEngine(int blocksizeBits)
		{
			this.blocksizeBytes = blocksizeBits / 8;
			this.blocksizeWords = this.blocksizeBytes / 8;
			this.currentBlock = new ulong[this.blocksizeWords];
			this.kw = new ulong[2 * this.blocksizeWords + 1];
			if (blocksizeBits == 256)
			{
				this.cipher = new ThreefishEngine.Threefish256Cipher(this.kw, this.t);
				return;
			}
			if (blocksizeBits == 512)
			{
				this.cipher = new ThreefishEngine.Threefish512Cipher(this.kw, this.t);
				return;
			}
			if (blocksizeBits != 1024)
			{
				throw new ArgumentException("Invalid blocksize - Threefish is defined with block size of 256, 512, or 1024 bits");
			}
			this.cipher = new ThreefishEngine.Threefish1024Cipher(this.kw, this.t);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x0014D200 File Offset: 0x0014B400
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			byte[] key;
			byte[] array;
			if (parameters is TweakableBlockCipherParameters)
			{
				TweakableBlockCipherParameters tweakableBlockCipherParameters = (TweakableBlockCipherParameters)parameters;
				key = tweakableBlockCipherParameters.Key.GetKey();
				array = tweakableBlockCipherParameters.Tweak;
			}
			else
			{
				if (!(parameters is KeyParameter))
				{
					throw new ArgumentException("Invalid parameter passed to Threefish init - " + Platform.GetTypeName(parameters));
				}
				key = ((KeyParameter)parameters).GetKey();
				array = null;
			}
			ulong[] array2 = null;
			ulong[] tweak = null;
			if (key != null)
			{
				if (key.Length != this.blocksizeBytes)
				{
					throw new ArgumentException("Threefish key must be same size as block (" + this.blocksizeBytes + " bytes)");
				}
				array2 = new ulong[this.blocksizeWords];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = ThreefishEngine.BytesToWord(key, i * 8);
				}
			}
			if (array != null)
			{
				if (array.Length != 16)
				{
					throw new ArgumentException("Threefish tweak must be " + 16 + " bytes");
				}
				tweak = new ulong[]
				{
					ThreefishEngine.BytesToWord(array, 0),
					ThreefishEngine.BytesToWord(array, 8)
				};
			}
			this.Init(forEncryption, array2, tweak);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x0014D305 File Offset: 0x0014B505
		internal void Init(bool forEncryption, ulong[] key, ulong[] tweak)
		{
			this.forEncryption = forEncryption;
			if (key != null)
			{
				this.SetKey(key);
			}
			if (tweak != null)
			{
				this.SetTweak(tweak);
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x0014D324 File Offset: 0x0014B524
		private void SetKey(ulong[] key)
		{
			if (key.Length != this.blocksizeWords)
			{
				throw new ArgumentException("Threefish key must be same size as block (" + this.blocksizeWords + " words)");
			}
			ulong num = 2004413935125273122UL;
			for (int i = 0; i < this.blocksizeWords; i++)
			{
				this.kw[i] = key[i];
				num ^= this.kw[i];
			}
			this.kw[this.blocksizeWords] = num;
			Array.Copy(this.kw, 0, this.kw, this.blocksizeWords + 1, this.blocksizeWords);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0014D3BC File Offset: 0x0014B5BC
		private void SetTweak(ulong[] tweak)
		{
			if (tweak.Length != 2)
			{
				throw new ArgumentException("Tweak must be " + 2 + " words.");
			}
			this.t[0] = tweak[0];
			this.t[1] = tweak[1];
			this.t[2] = (this.t[0] ^ this.t[1]);
			this.t[3] = this.t[0];
			this.t[4] = this.t[1];
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06003633 RID: 13875 RVA: 0x0014D439 File Offset: 0x0014B639
		public virtual string AlgorithmName
		{
			get
			{
				return "Threefish-" + this.blocksizeBytes * 8;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06003634 RID: 13876 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x0014D452 File Offset: 0x0014B652
		public virtual int GetBlockSize()
		{
			return this.blocksizeBytes;
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x06003637 RID: 13879 RVA: 0x0014D45C File Offset: 0x0014B65C
		public virtual int ProcessBlock(byte[] inBytes, int inOff, byte[] outBytes, int outOff)
		{
			if (outOff + this.blocksizeBytes > outBytes.Length)
			{
				throw new DataLengthException("Output buffer too short");
			}
			if (inOff + this.blocksizeBytes > inBytes.Length)
			{
				throw new DataLengthException("Input buffer too short");
			}
			for (int i = 0; i < this.blocksizeBytes; i += 8)
			{
				this.currentBlock[i >> 3] = ThreefishEngine.BytesToWord(inBytes, inOff + i);
			}
			this.ProcessBlock(this.currentBlock, this.currentBlock);
			for (int j = 0; j < this.blocksizeBytes; j += 8)
			{
				ThreefishEngine.WordToBytes(this.currentBlock[j >> 3], outBytes, outOff + j);
			}
			return this.blocksizeBytes;
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x0014D4FC File Offset: 0x0014B6FC
		internal int ProcessBlock(ulong[] inWords, ulong[] outWords)
		{
			if (this.kw[this.blocksizeWords] == 0UL)
			{
				throw new InvalidOperationException("Threefish engine not initialised");
			}
			if (inWords.Length != this.blocksizeWords)
			{
				throw new DataLengthException("Input buffer too short");
			}
			if (outWords.Length != this.blocksizeWords)
			{
				throw new DataLengthException("Output buffer too short");
			}
			if (this.forEncryption)
			{
				this.cipher.EncryptBlock(inWords, outWords);
			}
			else
			{
				this.cipher.DecryptBlock(inWords, outWords);
			}
			return this.blocksizeWords;
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x0014D57C File Offset: 0x0014B77C
		internal static ulong BytesToWord(byte[] bytes, int off)
		{
			if (off + 8 > bytes.Length)
			{
				throw new ArgumentException();
			}
			int num = off + 1;
			return ((ulong)bytes[off] & 255UL) | ((ulong)bytes[num++] & 255UL) << 8 | ((ulong)bytes[num++] & 255UL) << 16 | ((ulong)bytes[num++] & 255UL) << 24 | ((ulong)bytes[num++] & 255UL) << 32 | ((ulong)bytes[num++] & 255UL) << 40 | ((ulong)bytes[num++] & 255UL) << 48 | ((ulong)bytes[num++] & 255UL) << 56;
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x0014D62C File Offset: 0x0014B82C
		internal static void WordToBytes(ulong word, byte[] bytes, int off)
		{
			if (off + 8 > bytes.Length)
			{
				throw new ArgumentException();
			}
			int num = off + 1;
			bytes[off] = (byte)word;
			bytes[num++] = (byte)(word >> 8);
			bytes[num++] = (byte)(word >> 16);
			bytes[num++] = (byte)(word >> 24);
			bytes[num++] = (byte)(word >> 32);
			bytes[num++] = (byte)(word >> 40);
			bytes[num++] = (byte)(word >> 48);
			bytes[num++] = (byte)(word >> 56);
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x0014D6A5 File Offset: 0x0014B8A5
		private static ulong RotlXor(ulong x, int n, ulong xor)
		{
			return (x << n | x >> 64 - n) ^ xor;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x0014D6BC File Offset: 0x0014B8BC
		private static ulong XorRotr(ulong x, int n, ulong xor)
		{
			ulong num = x ^ xor;
			return num >> n | num << 64 - n;
		}

		// Token: 0x0400234F RID: 9039
		public const int BLOCKSIZE_256 = 256;

		// Token: 0x04002350 RID: 9040
		public const int BLOCKSIZE_512 = 512;

		// Token: 0x04002351 RID: 9041
		public const int BLOCKSIZE_1024 = 1024;

		// Token: 0x04002352 RID: 9042
		private const int TWEAK_SIZE_BYTES = 16;

		// Token: 0x04002353 RID: 9043
		private const int TWEAK_SIZE_WORDS = 2;

		// Token: 0x04002354 RID: 9044
		private const int ROUNDS_256 = 72;

		// Token: 0x04002355 RID: 9045
		private const int ROUNDS_512 = 72;

		// Token: 0x04002356 RID: 9046
		private const int ROUNDS_1024 = 80;

		// Token: 0x04002357 RID: 9047
		private const int MAX_ROUNDS = 80;

		// Token: 0x04002358 RID: 9048
		private const ulong C_240 = 2004413935125273122UL;

		// Token: 0x04002359 RID: 9049
		private static readonly int[] MOD9 = new int[80];

		// Token: 0x0400235A RID: 9050
		private static readonly int[] MOD17 = new int[ThreefishEngine.MOD9.Length];

		// Token: 0x0400235B RID: 9051
		private static readonly int[] MOD5 = new int[ThreefishEngine.MOD9.Length];

		// Token: 0x0400235C RID: 9052
		private static readonly int[] MOD3 = new int[ThreefishEngine.MOD9.Length];

		// Token: 0x0400235D RID: 9053
		private readonly int blocksizeBytes;

		// Token: 0x0400235E RID: 9054
		private readonly int blocksizeWords;

		// Token: 0x0400235F RID: 9055
		private readonly ulong[] currentBlock;

		// Token: 0x04002360 RID: 9056
		private readonly ulong[] t = new ulong[5];

		// Token: 0x04002361 RID: 9057
		private readonly ulong[] kw;

		// Token: 0x04002362 RID: 9058
		private readonly ThreefishEngine.ThreefishCipher cipher;

		// Token: 0x04002363 RID: 9059
		private bool forEncryption;

		// Token: 0x02000956 RID: 2390
		private abstract class ThreefishCipher
		{
			// Token: 0x06004F1C RID: 20252 RVA: 0x001B3E6E File Offset: 0x001B206E
			protected ThreefishCipher(ulong[] kw, ulong[] t)
			{
				this.kw = kw;
				this.t = t;
			}

			// Token: 0x06004F1D RID: 20253
			internal abstract void EncryptBlock(ulong[] block, ulong[] outWords);

			// Token: 0x06004F1E RID: 20254
			internal abstract void DecryptBlock(ulong[] block, ulong[] outWords);

			// Token: 0x0400363B RID: 13883
			protected readonly ulong[] t;

			// Token: 0x0400363C RID: 13884
			protected readonly ulong[] kw;
		}

		// Token: 0x02000957 RID: 2391
		private sealed class Threefish256Cipher : ThreefishEngine.ThreefishCipher
		{
			// Token: 0x06004F1F RID: 20255 RVA: 0x001B3E84 File Offset: 0x001B2084
			public Threefish256Cipher(ulong[] kw, ulong[] t) : base(kw, t)
			{
			}

			// Token: 0x06004F20 RID: 20256 RVA: 0x001B3E90 File Offset: 0x001B2090
			internal override void EncryptBlock(ulong[] block, ulong[] outWords)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD5;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 9)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				num += kw[0];
				num2 += kw[1] + t[0];
				num3 += kw[2] + t[1];
				num4 += kw[3];
				for (int i = 1; i < 18; i += 2)
				{
					int num5 = mod[i];
					int num6 = mod2[i];
					num2 = ThreefishEngine.RotlXor(num2, 14, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 16, num3 += num4);
					num4 = ThreefishEngine.RotlXor(num4, 52, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 57, num3 += num2);
					num2 = ThreefishEngine.RotlXor(num2, 23, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 40, num3 += num4);
					num4 = ThreefishEngine.RotlXor(num4, 5, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 37, num3 += num2);
					num += kw[num5];
					num2 += kw[num5 + 1] + t[num6];
					num3 += kw[num5 + 2] + t[num6 + 1];
					num4 += kw[num5 + 3] + (ulong)i;
					num2 = ThreefishEngine.RotlXor(num2, 25, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 33, num3 += num4);
					num4 = ThreefishEngine.RotlXor(num4, 46, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 12, num3 += num2);
					num2 = ThreefishEngine.RotlXor(num2, 58, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 22, num3 += num4);
					num4 = ThreefishEngine.RotlXor(num4, 32, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 32, num3 += num2);
					num += kw[num5 + 1];
					num2 += kw[num5 + 2] + t[num6 + 1];
					num3 += kw[num5 + 3] + t[num6 + 2];
					num4 += kw[num5 + 4] + (ulong)i + 1UL;
				}
				outWords[0] = num;
				outWords[1] = num2;
				outWords[2] = num3;
				outWords[3] = num4;
			}

			// Token: 0x06004F21 RID: 20257 RVA: 0x001B40F0 File Offset: 0x001B22F0
			internal override void DecryptBlock(ulong[] block, ulong[] state)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD5;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 9)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				for (int i = 17; i >= 1; i -= 2)
				{
					int num5 = mod[i];
					int num6 = mod2[i];
					num -= kw[num5 + 1];
					num2 -= kw[num5 + 2] + t[num6 + 1];
					num3 -= kw[num5 + 3] + t[num6 + 2];
					num4 -= kw[num5 + 4] + (ulong)i + 1UL;
					num4 = ThreefishEngine.XorRotr(num4, 32, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 32, num3);
					num3 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 58, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 22, num3);
					num3 -= num4;
					num4 = ThreefishEngine.XorRotr(num4, 46, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 12, num3);
					num3 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 25, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 33, num3);
					num3 -= num4;
					num -= kw[num5];
					num2 -= kw[num5 + 1] + t[num6];
					num3 -= kw[num5 + 2] + t[num6 + 1];
					num4 -= kw[num5 + 3] + (ulong)i;
					num4 = ThreefishEngine.XorRotr(num4, 5, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 37, num3);
					num3 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 23, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 40, num3);
					num3 -= num4;
					num4 = ThreefishEngine.XorRotr(num4, 52, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 57, num3);
					num3 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 14, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 16, num3);
					num3 -= num4;
				}
				num -= kw[0];
				num2 -= kw[1] + t[0];
				num3 -= kw[2] + t[1];
				num4 -= kw[3];
				state[0] = num;
				state[1] = num2;
				state[2] = num3;
				state[3] = num4;
			}

			// Token: 0x0400363D RID: 13885
			private const int ROTATION_0_0 = 14;

			// Token: 0x0400363E RID: 13886
			private const int ROTATION_0_1 = 16;

			// Token: 0x0400363F RID: 13887
			private const int ROTATION_1_0 = 52;

			// Token: 0x04003640 RID: 13888
			private const int ROTATION_1_1 = 57;

			// Token: 0x04003641 RID: 13889
			private const int ROTATION_2_0 = 23;

			// Token: 0x04003642 RID: 13890
			private const int ROTATION_2_1 = 40;

			// Token: 0x04003643 RID: 13891
			private const int ROTATION_3_0 = 5;

			// Token: 0x04003644 RID: 13892
			private const int ROTATION_3_1 = 37;

			// Token: 0x04003645 RID: 13893
			private const int ROTATION_4_0 = 25;

			// Token: 0x04003646 RID: 13894
			private const int ROTATION_4_1 = 33;

			// Token: 0x04003647 RID: 13895
			private const int ROTATION_5_0 = 46;

			// Token: 0x04003648 RID: 13896
			private const int ROTATION_5_1 = 12;

			// Token: 0x04003649 RID: 13897
			private const int ROTATION_6_0 = 58;

			// Token: 0x0400364A RID: 13898
			private const int ROTATION_6_1 = 22;

			// Token: 0x0400364B RID: 13899
			private const int ROTATION_7_0 = 32;

			// Token: 0x0400364C RID: 13900
			private const int ROTATION_7_1 = 32;
		}

		// Token: 0x02000958 RID: 2392
		private sealed class Threefish512Cipher : ThreefishEngine.ThreefishCipher
		{
			// Token: 0x06004F22 RID: 20258 RVA: 0x001B3E84 File Offset: 0x001B2084
			internal Threefish512Cipher(ulong[] kw, ulong[] t) : base(kw, t)
			{
			}

			// Token: 0x06004F23 RID: 20259 RVA: 0x001B4360 File Offset: 0x001B2560
			internal override void EncryptBlock(ulong[] block, ulong[] outWords)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD9;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 17)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				ulong num5 = block[4];
				ulong num6 = block[5];
				ulong num7 = block[6];
				ulong num8 = block[7];
				num += kw[0];
				num2 += kw[1];
				num3 += kw[2];
				num4 += kw[3];
				num5 += kw[4];
				num6 += kw[5] + t[0];
				num7 += kw[6] + t[1];
				num8 += kw[7];
				for (int i = 1; i < 18; i += 2)
				{
					int num9 = mod[i];
					int num10 = mod2[i];
					num2 = ThreefishEngine.RotlXor(num2, 46, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 36, num3 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 19, num5 += num6);
					num8 = ThreefishEngine.RotlXor(num8, 37, num7 += num8);
					num2 = ThreefishEngine.RotlXor(num2, 33, num3 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 27, num5 += num8);
					num6 = ThreefishEngine.RotlXor(num6, 14, num7 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 42, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 17, num5 += num2);
					num4 = ThreefishEngine.RotlXor(num4, 49, num7 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 36, num += num6);
					num8 = ThreefishEngine.RotlXor(num8, 39, num3 += num8);
					num2 = ThreefishEngine.RotlXor(num2, 44, num7 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 9, num += num8);
					num6 = ThreefishEngine.RotlXor(num6, 54, num3 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 56, num5 += num4);
					num += kw[num9];
					num2 += kw[num9 + 1];
					num3 += kw[num9 + 2];
					num4 += kw[num9 + 3];
					num5 += kw[num9 + 4];
					num6 += kw[num9 + 5] + t[num10];
					num7 += kw[num9 + 6] + t[num10 + 1];
					num8 += kw[num9 + 7] + (ulong)i;
					num2 = ThreefishEngine.RotlXor(num2, 39, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 30, num3 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 34, num5 += num6);
					num8 = ThreefishEngine.RotlXor(num8, 24, num7 += num8);
					num2 = ThreefishEngine.RotlXor(num2, 13, num3 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 50, num5 += num8);
					num6 = ThreefishEngine.RotlXor(num6, 10, num7 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 17, num += num4);
					num2 = ThreefishEngine.RotlXor(num2, 25, num5 += num2);
					num4 = ThreefishEngine.RotlXor(num4, 29, num7 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 39, num += num6);
					num8 = ThreefishEngine.RotlXor(num8, 43, num3 += num8);
					num2 = ThreefishEngine.RotlXor(num2, 8, num7 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 35, num += num8);
					num6 = ThreefishEngine.RotlXor(num6, 56, num3 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 22, num5 += num4);
					num += kw[num9 + 1];
					num2 += kw[num9 + 2];
					num3 += kw[num9 + 3];
					num4 += kw[num9 + 4];
					num5 += kw[num9 + 5];
					num6 += kw[num9 + 6] + t[num10 + 1];
					num7 += kw[num9 + 7] + t[num10 + 2];
					num8 += kw[num9 + 8] + (ulong)i + 1UL;
				}
				outWords[0] = num;
				outWords[1] = num2;
				outWords[2] = num3;
				outWords[3] = num4;
				outWords[4] = num5;
				outWords[5] = num6;
				outWords[6] = num7;
				outWords[7] = num8;
			}

			// Token: 0x06004F24 RID: 20260 RVA: 0x001B4790 File Offset: 0x001B2990
			internal override void DecryptBlock(ulong[] block, ulong[] state)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD9;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 17)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				ulong num5 = block[4];
				ulong num6 = block[5];
				ulong num7 = block[6];
				ulong num8 = block[7];
				for (int i = 17; i >= 1; i -= 2)
				{
					int num9 = mod[i];
					int num10 = mod2[i];
					num -= kw[num9 + 1];
					num2 -= kw[num9 + 2];
					num3 -= kw[num9 + 3];
					num4 -= kw[num9 + 4];
					num5 -= kw[num9 + 5];
					num6 -= kw[num9 + 6] + t[num10 + 1];
					num7 -= kw[num9 + 7] + t[num10 + 2];
					num8 -= kw[num9 + 8] + (ulong)i + 1UL;
					num2 = ThreefishEngine.XorRotr(num2, 8, num7);
					num7 -= num2;
					num8 = ThreefishEngine.XorRotr(num8, 35, num);
					num -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 56, num3);
					num3 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 22, num5);
					num5 -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 25, num5);
					num5 -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 29, num7);
					num7 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 39, num);
					num -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 43, num3);
					num3 -= num8;
					num2 = ThreefishEngine.XorRotr(num2, 13, num3);
					num3 -= num2;
					num8 = ThreefishEngine.XorRotr(num8, 50, num5);
					num5 -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 10, num7);
					num7 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 17, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 39, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 30, num3);
					num3 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 34, num5);
					num5 -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 24, num7);
					num7 -= num8;
					num -= kw[num9];
					num2 -= kw[num9 + 1];
					num3 -= kw[num9 + 2];
					num4 -= kw[num9 + 3];
					num5 -= kw[num9 + 4];
					num6 -= kw[num9 + 5] + t[num10];
					num7 -= kw[num9 + 6] + t[num10 + 1];
					num8 -= kw[num9 + 7] + (ulong)i;
					num2 = ThreefishEngine.XorRotr(num2, 44, num7);
					num7 -= num2;
					num8 = ThreefishEngine.XorRotr(num8, 9, num);
					num -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 54, num3);
					num3 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 56, num5);
					num5 -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 17, num5);
					num5 -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 49, num7);
					num7 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 36, num);
					num -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 39, num3);
					num3 -= num8;
					num2 = ThreefishEngine.XorRotr(num2, 33, num3);
					num3 -= num2;
					num8 = ThreefishEngine.XorRotr(num8, 27, num5);
					num5 -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 14, num7);
					num7 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 42, num);
					num -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 46, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 36, num3);
					num3 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 19, num5);
					num5 -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 37, num7);
					num7 -= num8;
				}
				num -= kw[0];
				num2 -= kw[1];
				num3 -= kw[2];
				num4 -= kw[3];
				num5 -= kw[4];
				num6 -= kw[5] + t[0];
				num7 -= kw[6] + t[1];
				num8 -= kw[7];
				state[0] = num;
				state[1] = num2;
				state[2] = num3;
				state[3] = num4;
				state[4] = num5;
				state[5] = num6;
				state[6] = num7;
				state[7] = num8;
			}

			// Token: 0x0400364D RID: 13901
			private const int ROTATION_0_0 = 46;

			// Token: 0x0400364E RID: 13902
			private const int ROTATION_0_1 = 36;

			// Token: 0x0400364F RID: 13903
			private const int ROTATION_0_2 = 19;

			// Token: 0x04003650 RID: 13904
			private const int ROTATION_0_3 = 37;

			// Token: 0x04003651 RID: 13905
			private const int ROTATION_1_0 = 33;

			// Token: 0x04003652 RID: 13906
			private const int ROTATION_1_1 = 27;

			// Token: 0x04003653 RID: 13907
			private const int ROTATION_1_2 = 14;

			// Token: 0x04003654 RID: 13908
			private const int ROTATION_1_3 = 42;

			// Token: 0x04003655 RID: 13909
			private const int ROTATION_2_0 = 17;

			// Token: 0x04003656 RID: 13910
			private const int ROTATION_2_1 = 49;

			// Token: 0x04003657 RID: 13911
			private const int ROTATION_2_2 = 36;

			// Token: 0x04003658 RID: 13912
			private const int ROTATION_2_3 = 39;

			// Token: 0x04003659 RID: 13913
			private const int ROTATION_3_0 = 44;

			// Token: 0x0400365A RID: 13914
			private const int ROTATION_3_1 = 9;

			// Token: 0x0400365B RID: 13915
			private const int ROTATION_3_2 = 54;

			// Token: 0x0400365C RID: 13916
			private const int ROTATION_3_3 = 56;

			// Token: 0x0400365D RID: 13917
			private const int ROTATION_4_0 = 39;

			// Token: 0x0400365E RID: 13918
			private const int ROTATION_4_1 = 30;

			// Token: 0x0400365F RID: 13919
			private const int ROTATION_4_2 = 34;

			// Token: 0x04003660 RID: 13920
			private const int ROTATION_4_3 = 24;

			// Token: 0x04003661 RID: 13921
			private const int ROTATION_5_0 = 13;

			// Token: 0x04003662 RID: 13922
			private const int ROTATION_5_1 = 50;

			// Token: 0x04003663 RID: 13923
			private const int ROTATION_5_2 = 10;

			// Token: 0x04003664 RID: 13924
			private const int ROTATION_5_3 = 17;

			// Token: 0x04003665 RID: 13925
			private const int ROTATION_6_0 = 25;

			// Token: 0x04003666 RID: 13926
			private const int ROTATION_6_1 = 29;

			// Token: 0x04003667 RID: 13927
			private const int ROTATION_6_2 = 39;

			// Token: 0x04003668 RID: 13928
			private const int ROTATION_6_3 = 43;

			// Token: 0x04003669 RID: 13929
			private const int ROTATION_7_0 = 8;

			// Token: 0x0400366A RID: 13930
			private const int ROTATION_7_1 = 35;

			// Token: 0x0400366B RID: 13931
			private const int ROTATION_7_2 = 56;

			// Token: 0x0400366C RID: 13932
			private const int ROTATION_7_3 = 22;
		}

		// Token: 0x02000959 RID: 2393
		private sealed class Threefish1024Cipher : ThreefishEngine.ThreefishCipher
		{
			// Token: 0x06004F25 RID: 20261 RVA: 0x001B3E84 File Offset: 0x001B2084
			public Threefish1024Cipher(ulong[] kw, ulong[] t) : base(kw, t)
			{
			}

			// Token: 0x06004F26 RID: 20262 RVA: 0x001B4BE0 File Offset: 0x001B2DE0
			internal override void EncryptBlock(ulong[] block, ulong[] outWords)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD17;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 33)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				ulong num5 = block[4];
				ulong num6 = block[5];
				ulong num7 = block[6];
				ulong num8 = block[7];
				ulong num9 = block[8];
				ulong num10 = block[9];
				ulong num11 = block[10];
				ulong num12 = block[11];
				ulong num13 = block[12];
				ulong num14 = block[13];
				ulong num15 = block[14];
				ulong num16 = block[15];
				num += kw[0];
				num2 += kw[1];
				num3 += kw[2];
				num4 += kw[3];
				num5 += kw[4];
				num6 += kw[5];
				num7 += kw[6];
				num8 += kw[7];
				num9 += kw[8];
				num10 += kw[9];
				num11 += kw[10];
				num12 += kw[11];
				num13 += kw[12];
				num14 += kw[13] + t[0];
				num15 += kw[14] + t[1];
				num16 += kw[15];
				for (int i = 1; i < 20; i += 2)
				{
					int num17 = mod[i];
					int num18 = mod2[i];
					num2 = ThreefishEngine.RotlXor(num2, 24, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 13, num3 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 8, num5 += num6);
					num8 = ThreefishEngine.RotlXor(num8, 47, num7 += num8);
					num10 = ThreefishEngine.RotlXor(num10, 8, num9 += num10);
					num12 = ThreefishEngine.RotlXor(num12, 17, num11 += num12);
					num14 = ThreefishEngine.RotlXor(num14, 22, num13 += num14);
					num16 = ThreefishEngine.RotlXor(num16, 37, num15 += num16);
					num10 = ThreefishEngine.RotlXor(num10, 38, num += num10);
					num14 = ThreefishEngine.RotlXor(num14, 19, num3 += num14);
					num12 = ThreefishEngine.RotlXor(num12, 10, num7 += num12);
					num16 = ThreefishEngine.RotlXor(num16, 55, num5 += num16);
					num8 = ThreefishEngine.RotlXor(num8, 49, num11 += num8);
					num4 = ThreefishEngine.RotlXor(num4, 18, num13 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 23, num15 += num6);
					num2 = ThreefishEngine.RotlXor(num2, 52, num9 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 33, num += num8);
					num6 = ThreefishEngine.RotlXor(num6, 4, num3 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 51, num5 += num4);
					num2 = ThreefishEngine.RotlXor(num2, 13, num7 += num2);
					num16 = ThreefishEngine.RotlXor(num16, 34, num13 += num16);
					num14 = ThreefishEngine.RotlXor(num14, 41, num15 += num14);
					num12 = ThreefishEngine.RotlXor(num12, 59, num9 += num12);
					num10 = ThreefishEngine.RotlXor(num10, 17, num11 += num10);
					num16 = ThreefishEngine.RotlXor(num16, 5, num += num16);
					num12 = ThreefishEngine.RotlXor(num12, 20, num3 += num12);
					num14 = ThreefishEngine.RotlXor(num14, 48, num7 += num14);
					num10 = ThreefishEngine.RotlXor(num10, 41, num5 += num10);
					num2 = ThreefishEngine.RotlXor(num2, 47, num15 += num2);
					num6 = ThreefishEngine.RotlXor(num6, 28, num9 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 16, num11 += num4);
					num8 = ThreefishEngine.RotlXor(num8, 25, num13 += num8);
					num += kw[num17];
					num2 += kw[num17 + 1];
					num3 += kw[num17 + 2];
					num4 += kw[num17 + 3];
					num5 += kw[num17 + 4];
					num6 += kw[num17 + 5];
					num7 += kw[num17 + 6];
					num8 += kw[num17 + 7];
					num9 += kw[num17 + 8];
					num10 += kw[num17 + 9];
					num11 += kw[num17 + 10];
					num12 += kw[num17 + 11];
					num13 += kw[num17 + 12];
					num14 += kw[num17 + 13] + t[num18];
					num15 += kw[num17 + 14] + t[num18 + 1];
					num16 += kw[num17 + 15] + (ulong)i;
					num2 = ThreefishEngine.RotlXor(num2, 41, num += num2);
					num4 = ThreefishEngine.RotlXor(num4, 9, num3 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 37, num5 += num6);
					num8 = ThreefishEngine.RotlXor(num8, 31, num7 += num8);
					num10 = ThreefishEngine.RotlXor(num10, 12, num9 += num10);
					num12 = ThreefishEngine.RotlXor(num12, 47, num11 += num12);
					num14 = ThreefishEngine.RotlXor(num14, 44, num13 += num14);
					num16 = ThreefishEngine.RotlXor(num16, 30, num15 += num16);
					num10 = ThreefishEngine.RotlXor(num10, 16, num += num10);
					num14 = ThreefishEngine.RotlXor(num14, 34, num3 += num14);
					num12 = ThreefishEngine.RotlXor(num12, 56, num7 += num12);
					num16 = ThreefishEngine.RotlXor(num16, 51, num5 += num16);
					num8 = ThreefishEngine.RotlXor(num8, 4, num11 += num8);
					num4 = ThreefishEngine.RotlXor(num4, 53, num13 += num4);
					num6 = ThreefishEngine.RotlXor(num6, 42, num15 += num6);
					num2 = ThreefishEngine.RotlXor(num2, 41, num9 += num2);
					num8 = ThreefishEngine.RotlXor(num8, 31, num += num8);
					num6 = ThreefishEngine.RotlXor(num6, 44, num3 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 47, num5 += num4);
					num2 = ThreefishEngine.RotlXor(num2, 46, num7 += num2);
					num16 = ThreefishEngine.RotlXor(num16, 19, num13 += num16);
					num14 = ThreefishEngine.RotlXor(num14, 42, num15 += num14);
					num12 = ThreefishEngine.RotlXor(num12, 44, num9 += num12);
					num10 = ThreefishEngine.RotlXor(num10, 25, num11 += num10);
					num16 = ThreefishEngine.RotlXor(num16, 9, num += num16);
					num12 = ThreefishEngine.RotlXor(num12, 48, num3 += num12);
					num14 = ThreefishEngine.RotlXor(num14, 35, num7 += num14);
					num10 = ThreefishEngine.RotlXor(num10, 52, num5 += num10);
					num2 = ThreefishEngine.RotlXor(num2, 23, num15 += num2);
					num6 = ThreefishEngine.RotlXor(num6, 31, num9 += num6);
					num4 = ThreefishEngine.RotlXor(num4, 37, num11 += num4);
					num8 = ThreefishEngine.RotlXor(num8, 20, num13 += num8);
					num += kw[num17 + 1];
					num2 += kw[num17 + 2];
					num3 += kw[num17 + 3];
					num4 += kw[num17 + 4];
					num5 += kw[num17 + 5];
					num6 += kw[num17 + 6];
					num7 += kw[num17 + 7];
					num8 += kw[num17 + 8];
					num9 += kw[num17 + 9];
					num10 += kw[num17 + 10];
					num11 += kw[num17 + 11];
					num12 += kw[num17 + 12];
					num13 += kw[num17 + 13];
					num14 += kw[num17 + 14] + t[num18 + 1];
					num15 += kw[num17 + 15] + t[num18 + 2];
					num16 += kw[num17 + 16] + (ulong)i + 1UL;
				}
				outWords[0] = num;
				outWords[1] = num2;
				outWords[2] = num3;
				outWords[3] = num4;
				outWords[4] = num5;
				outWords[5] = num6;
				outWords[6] = num7;
				outWords[7] = num8;
				outWords[8] = num9;
				outWords[9] = num10;
				outWords[10] = num11;
				outWords[11] = num12;
				outWords[12] = num13;
				outWords[13] = num14;
				outWords[14] = num15;
				outWords[15] = num16;
			}

			// Token: 0x06004F27 RID: 20263 RVA: 0x001B53D0 File Offset: 0x001B35D0
			internal override void DecryptBlock(ulong[] block, ulong[] state)
			{
				ulong[] kw = this.kw;
				ulong[] t = this.t;
				int[] mod = ThreefishEngine.MOD17;
				int[] mod2 = ThreefishEngine.MOD3;
				if (kw.Length != 33)
				{
					throw new ArgumentException();
				}
				if (t.Length != 5)
				{
					throw new ArgumentException();
				}
				ulong num = block[0];
				ulong num2 = block[1];
				ulong num3 = block[2];
				ulong num4 = block[3];
				ulong num5 = block[4];
				ulong num6 = block[5];
				ulong num7 = block[6];
				ulong num8 = block[7];
				ulong num9 = block[8];
				ulong num10 = block[9];
				ulong num11 = block[10];
				ulong num12 = block[11];
				ulong num13 = block[12];
				ulong num14 = block[13];
				ulong num15 = block[14];
				ulong num16 = block[15];
				for (int i = 19; i >= 1; i -= 2)
				{
					int num17 = mod[i];
					int num18 = mod2[i];
					num -= kw[num17 + 1];
					num2 -= kw[num17 + 2];
					num3 -= kw[num17 + 3];
					num4 -= kw[num17 + 4];
					num5 -= kw[num17 + 5];
					num6 -= kw[num17 + 6];
					num7 -= kw[num17 + 7];
					num8 -= kw[num17 + 8];
					num9 -= kw[num17 + 9];
					num10 -= kw[num17 + 10];
					num11 -= kw[num17 + 11];
					num12 -= kw[num17 + 12];
					num13 -= kw[num17 + 13];
					num14 -= kw[num17 + 14] + t[num18 + 1];
					num15 -= kw[num17 + 15] + t[num18 + 2];
					num16 -= kw[num17 + 16] + (ulong)i + 1UL;
					num16 = ThreefishEngine.XorRotr(num16, 9, num);
					num -= num16;
					num12 = ThreefishEngine.XorRotr(num12, 48, num3);
					num3 -= num12;
					num14 = ThreefishEngine.XorRotr(num14, 35, num7);
					num7 -= num14;
					num10 = ThreefishEngine.XorRotr(num10, 52, num5);
					num5 -= num10;
					num2 = ThreefishEngine.XorRotr(num2, 23, num15);
					num15 -= num2;
					num6 = ThreefishEngine.XorRotr(num6, 31, num9);
					num9 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 37, num11);
					num11 -= num4;
					num8 = ThreefishEngine.XorRotr(num8, 20, num13);
					num13 -= num8;
					num8 = ThreefishEngine.XorRotr(num8, 31, num);
					num -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 44, num3);
					num3 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 47, num5);
					num5 -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 46, num7);
					num7 -= num2;
					num16 = ThreefishEngine.XorRotr(num16, 19, num13);
					num13 -= num16;
					num14 = ThreefishEngine.XorRotr(num14, 42, num15);
					num15 -= num14;
					num12 = ThreefishEngine.XorRotr(num12, 44, num9);
					num9 -= num12;
					num10 = ThreefishEngine.XorRotr(num10, 25, num11);
					num11 -= num10;
					num10 = ThreefishEngine.XorRotr(num10, 16, num);
					num -= num10;
					num14 = ThreefishEngine.XorRotr(num14, 34, num3);
					num3 -= num14;
					num12 = ThreefishEngine.XorRotr(num12, 56, num7);
					num7 -= num12;
					num16 = ThreefishEngine.XorRotr(num16, 51, num5);
					num5 -= num16;
					num8 = ThreefishEngine.XorRotr(num8, 4, num11);
					num11 -= num8;
					num4 = ThreefishEngine.XorRotr(num4, 53, num13);
					num13 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 42, num15);
					num15 -= num6;
					num2 = ThreefishEngine.XorRotr(num2, 41, num9);
					num9 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 41, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 9, num3);
					num3 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 37, num5);
					num5 -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 31, num7);
					num7 -= num8;
					num10 = ThreefishEngine.XorRotr(num10, 12, num9);
					num9 -= num10;
					num12 = ThreefishEngine.XorRotr(num12, 47, num11);
					num11 -= num12;
					num14 = ThreefishEngine.XorRotr(num14, 44, num13);
					num13 -= num14;
					num16 = ThreefishEngine.XorRotr(num16, 30, num15);
					num15 -= num16;
					num -= kw[num17];
					num2 -= kw[num17 + 1];
					num3 -= kw[num17 + 2];
					num4 -= kw[num17 + 3];
					num5 -= kw[num17 + 4];
					num6 -= kw[num17 + 5];
					num7 -= kw[num17 + 6];
					num8 -= kw[num17 + 7];
					num9 -= kw[num17 + 8];
					num10 -= kw[num17 + 9];
					num11 -= kw[num17 + 10];
					num12 -= kw[num17 + 11];
					num13 -= kw[num17 + 12];
					num14 -= kw[num17 + 13] + t[num18];
					num15 -= kw[num17 + 14] + t[num18 + 1];
					num16 -= kw[num17 + 15] + (ulong)i;
					num16 = ThreefishEngine.XorRotr(num16, 5, num);
					num -= num16;
					num12 = ThreefishEngine.XorRotr(num12, 20, num3);
					num3 -= num12;
					num14 = ThreefishEngine.XorRotr(num14, 48, num7);
					num7 -= num14;
					num10 = ThreefishEngine.XorRotr(num10, 41, num5);
					num5 -= num10;
					num2 = ThreefishEngine.XorRotr(num2, 47, num15);
					num15 -= num2;
					num6 = ThreefishEngine.XorRotr(num6, 28, num9);
					num9 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 16, num11);
					num11 -= num4;
					num8 = ThreefishEngine.XorRotr(num8, 25, num13);
					num13 -= num8;
					num8 = ThreefishEngine.XorRotr(num8, 33, num);
					num -= num8;
					num6 = ThreefishEngine.XorRotr(num6, 4, num3);
					num3 -= num6;
					num4 = ThreefishEngine.XorRotr(num4, 51, num5);
					num5 -= num4;
					num2 = ThreefishEngine.XorRotr(num2, 13, num7);
					num7 -= num2;
					num16 = ThreefishEngine.XorRotr(num16, 34, num13);
					num13 -= num16;
					num14 = ThreefishEngine.XorRotr(num14, 41, num15);
					num15 -= num14;
					num12 = ThreefishEngine.XorRotr(num12, 59, num9);
					num9 -= num12;
					num10 = ThreefishEngine.XorRotr(num10, 17, num11);
					num11 -= num10;
					num10 = ThreefishEngine.XorRotr(num10, 38, num);
					num -= num10;
					num14 = ThreefishEngine.XorRotr(num14, 19, num3);
					num3 -= num14;
					num12 = ThreefishEngine.XorRotr(num12, 10, num7);
					num7 -= num12;
					num16 = ThreefishEngine.XorRotr(num16, 55, num5);
					num5 -= num16;
					num8 = ThreefishEngine.XorRotr(num8, 49, num11);
					num11 -= num8;
					num4 = ThreefishEngine.XorRotr(num4, 18, num13);
					num13 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 23, num15);
					num15 -= num6;
					num2 = ThreefishEngine.XorRotr(num2, 52, num9);
					num9 -= num2;
					num2 = ThreefishEngine.XorRotr(num2, 24, num);
					num -= num2;
					num4 = ThreefishEngine.XorRotr(num4, 13, num3);
					num3 -= num4;
					num6 = ThreefishEngine.XorRotr(num6, 8, num5);
					num5 -= num6;
					num8 = ThreefishEngine.XorRotr(num8, 47, num7);
					num7 -= num8;
					num10 = ThreefishEngine.XorRotr(num10, 8, num9);
					num9 -= num10;
					num12 = ThreefishEngine.XorRotr(num12, 17, num11);
					num11 -= num12;
					num14 = ThreefishEngine.XorRotr(num14, 22, num13);
					num13 -= num14;
					num16 = ThreefishEngine.XorRotr(num16, 37, num15);
					num15 -= num16;
				}
				num -= kw[0];
				num2 -= kw[1];
				num3 -= kw[2];
				num4 -= kw[3];
				num5 -= kw[4];
				num6 -= kw[5];
				num7 -= kw[6];
				num8 -= kw[7];
				num9 -= kw[8];
				num10 -= kw[9];
				num11 -= kw[10];
				num12 -= kw[11];
				num13 -= kw[12];
				num14 -= kw[13] + t[0];
				num15 -= kw[14] + t[1];
				num16 -= kw[15];
				state[0] = num;
				state[1] = num2;
				state[2] = num3;
				state[3] = num4;
				state[4] = num5;
				state[5] = num6;
				state[6] = num7;
				state[7] = num8;
				state[8] = num9;
				state[9] = num10;
				state[10] = num11;
				state[11] = num12;
				state[12] = num13;
				state[13] = num14;
				state[14] = num15;
				state[15] = num16;
			}

			// Token: 0x0400366D RID: 13933
			private const int ROTATION_0_0 = 24;

			// Token: 0x0400366E RID: 13934
			private const int ROTATION_0_1 = 13;

			// Token: 0x0400366F RID: 13935
			private const int ROTATION_0_2 = 8;

			// Token: 0x04003670 RID: 13936
			private const int ROTATION_0_3 = 47;

			// Token: 0x04003671 RID: 13937
			private const int ROTATION_0_4 = 8;

			// Token: 0x04003672 RID: 13938
			private const int ROTATION_0_5 = 17;

			// Token: 0x04003673 RID: 13939
			private const int ROTATION_0_6 = 22;

			// Token: 0x04003674 RID: 13940
			private const int ROTATION_0_7 = 37;

			// Token: 0x04003675 RID: 13941
			private const int ROTATION_1_0 = 38;

			// Token: 0x04003676 RID: 13942
			private const int ROTATION_1_1 = 19;

			// Token: 0x04003677 RID: 13943
			private const int ROTATION_1_2 = 10;

			// Token: 0x04003678 RID: 13944
			private const int ROTATION_1_3 = 55;

			// Token: 0x04003679 RID: 13945
			private const int ROTATION_1_4 = 49;

			// Token: 0x0400367A RID: 13946
			private const int ROTATION_1_5 = 18;

			// Token: 0x0400367B RID: 13947
			private const int ROTATION_1_6 = 23;

			// Token: 0x0400367C RID: 13948
			private const int ROTATION_1_7 = 52;

			// Token: 0x0400367D RID: 13949
			private const int ROTATION_2_0 = 33;

			// Token: 0x0400367E RID: 13950
			private const int ROTATION_2_1 = 4;

			// Token: 0x0400367F RID: 13951
			private const int ROTATION_2_2 = 51;

			// Token: 0x04003680 RID: 13952
			private const int ROTATION_2_3 = 13;

			// Token: 0x04003681 RID: 13953
			private const int ROTATION_2_4 = 34;

			// Token: 0x04003682 RID: 13954
			private const int ROTATION_2_5 = 41;

			// Token: 0x04003683 RID: 13955
			private const int ROTATION_2_6 = 59;

			// Token: 0x04003684 RID: 13956
			private const int ROTATION_2_7 = 17;

			// Token: 0x04003685 RID: 13957
			private const int ROTATION_3_0 = 5;

			// Token: 0x04003686 RID: 13958
			private const int ROTATION_3_1 = 20;

			// Token: 0x04003687 RID: 13959
			private const int ROTATION_3_2 = 48;

			// Token: 0x04003688 RID: 13960
			private const int ROTATION_3_3 = 41;

			// Token: 0x04003689 RID: 13961
			private const int ROTATION_3_4 = 47;

			// Token: 0x0400368A RID: 13962
			private const int ROTATION_3_5 = 28;

			// Token: 0x0400368B RID: 13963
			private const int ROTATION_3_6 = 16;

			// Token: 0x0400368C RID: 13964
			private const int ROTATION_3_7 = 25;

			// Token: 0x0400368D RID: 13965
			private const int ROTATION_4_0 = 41;

			// Token: 0x0400368E RID: 13966
			private const int ROTATION_4_1 = 9;

			// Token: 0x0400368F RID: 13967
			private const int ROTATION_4_2 = 37;

			// Token: 0x04003690 RID: 13968
			private const int ROTATION_4_3 = 31;

			// Token: 0x04003691 RID: 13969
			private const int ROTATION_4_4 = 12;

			// Token: 0x04003692 RID: 13970
			private const int ROTATION_4_5 = 47;

			// Token: 0x04003693 RID: 13971
			private const int ROTATION_4_6 = 44;

			// Token: 0x04003694 RID: 13972
			private const int ROTATION_4_7 = 30;

			// Token: 0x04003695 RID: 13973
			private const int ROTATION_5_0 = 16;

			// Token: 0x04003696 RID: 13974
			private const int ROTATION_5_1 = 34;

			// Token: 0x04003697 RID: 13975
			private const int ROTATION_5_2 = 56;

			// Token: 0x04003698 RID: 13976
			private const int ROTATION_5_3 = 51;

			// Token: 0x04003699 RID: 13977
			private const int ROTATION_5_4 = 4;

			// Token: 0x0400369A RID: 13978
			private const int ROTATION_5_5 = 53;

			// Token: 0x0400369B RID: 13979
			private const int ROTATION_5_6 = 42;

			// Token: 0x0400369C RID: 13980
			private const int ROTATION_5_7 = 41;

			// Token: 0x0400369D RID: 13981
			private const int ROTATION_6_0 = 31;

			// Token: 0x0400369E RID: 13982
			private const int ROTATION_6_1 = 44;

			// Token: 0x0400369F RID: 13983
			private const int ROTATION_6_2 = 47;

			// Token: 0x040036A0 RID: 13984
			private const int ROTATION_6_3 = 46;

			// Token: 0x040036A1 RID: 13985
			private const int ROTATION_6_4 = 19;

			// Token: 0x040036A2 RID: 13986
			private const int ROTATION_6_5 = 42;

			// Token: 0x040036A3 RID: 13987
			private const int ROTATION_6_6 = 44;

			// Token: 0x040036A4 RID: 13988
			private const int ROTATION_6_7 = 25;

			// Token: 0x040036A5 RID: 13989
			private const int ROTATION_7_0 = 9;

			// Token: 0x040036A6 RID: 13990
			private const int ROTATION_7_1 = 48;

			// Token: 0x040036A7 RID: 13991
			private const int ROTATION_7_2 = 35;

			// Token: 0x040036A8 RID: 13992
			private const int ROTATION_7_3 = 52;

			// Token: 0x040036A9 RID: 13993
			private const int ROTATION_7_4 = 23;

			// Token: 0x040036AA RID: 13994
			private const int ROTATION_7_5 = 31;

			// Token: 0x040036AB RID: 13995
			private const int ROTATION_7_6 = 37;

			// Token: 0x040036AC RID: 13996
			private const int ROTATION_7_7 = 20;
		}
	}
}
