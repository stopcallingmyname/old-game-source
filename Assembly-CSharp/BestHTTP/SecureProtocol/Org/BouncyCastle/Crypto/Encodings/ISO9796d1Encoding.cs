using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings
{
	// Token: 0x0200059E RID: 1438
	public class ISO9796d1Encoding : IAsymmetricBlockCipher
	{
		// Token: 0x06003674 RID: 13940 RVA: 0x001512F7 File Offset: 0x0014F4F7
		public ISO9796d1Encoding(IAsymmetricBlockCipher cipher)
		{
			this.engine = cipher;
		}

		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06003675 RID: 13941 RVA: 0x00151306 File Offset: 0x0014F506
		public string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/ISO9796-1Padding";
			}
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x0015131D File Offset: 0x0014F51D
		public IAsymmetricBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00151328 File Offset: 0x0014F528
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			RsaKeyParameters rsaKeyParameters;
			if (parameters is ParametersWithRandom)
			{
				rsaKeyParameters = (RsaKeyParameters)((ParametersWithRandom)parameters).Parameters;
			}
			else
			{
				rsaKeyParameters = (RsaKeyParameters)parameters;
			}
			this.engine.Init(forEncryption, parameters);
			this.modulus = rsaKeyParameters.Modulus;
			this.bitSize = this.modulus.BitLength;
			this.forEncryption = forEncryption;
		}

		// Token: 0x06003678 RID: 13944 RVA: 0x00151388 File Offset: 0x0014F588
		public int GetInputBlockSize()
		{
			int inputBlockSize = this.engine.GetInputBlockSize();
			if (this.forEncryption)
			{
				return (inputBlockSize + 1) / 2;
			}
			return inputBlockSize;
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x001513B0 File Offset: 0x0014F5B0
		public int GetOutputBlockSize()
		{
			int outputBlockSize = this.engine.GetOutputBlockSize();
			if (this.forEncryption)
			{
				return outputBlockSize;
			}
			return (outputBlockSize + 1) / 2;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x001513D8 File Offset: 0x0014F5D8
		public void SetPadBits(int padBits)
		{
			if (padBits > 7)
			{
				throw new ArgumentException("padBits > 7");
			}
			this.padBits = padBits;
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x001513F0 File Offset: 0x0014F5F0
		public int GetPadBits()
		{
			return this.padBits;
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x001513F8 File Offset: 0x0014F5F8
		public byte[] ProcessBlock(byte[] input, int inOff, int length)
		{
			if (this.forEncryption)
			{
				return this.EncodeBlock(input, inOff, length);
			}
			return this.DecodeBlock(input, inOff, length);
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x00151418 File Offset: 0x0014F618
		private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
		{
			byte[] array = new byte[(this.bitSize + 7) / 8];
			int num = this.padBits + 1;
			int num2 = (this.bitSize + 13) / 16;
			for (int i = 0; i < num2; i += inLen)
			{
				if (i > num2 - inLen)
				{
					Array.Copy(input, inOff + inLen - (num2 - i), array, array.Length - num2, num2 - i);
				}
				else
				{
					Array.Copy(input, inOff, array, array.Length - (i + inLen), inLen);
				}
			}
			for (int num3 = array.Length - 2 * num2; num3 != array.Length; num3 += 2)
			{
				byte b = array[array.Length - num2 + num3 / 2];
				array[num3] = (byte)((int)ISO9796d1Encoding.shadows[(int)((uint)(b & byte.MaxValue) >> 4)] << 4 | (int)ISO9796d1Encoding.shadows[(int)(b & 15)]);
				array[num3 + 1] = b;
			}
			byte[] array2 = array;
			int num4 = array.Length - 2 * inLen;
			array2[num4] ^= (byte)num;
			array[array.Length - 1] = (byte)((int)array[array.Length - 1] << 4 | 6);
			int num5 = 8 - (this.bitSize - 1) % 8;
			int num6 = 0;
			if (num5 != 8)
			{
				byte[] array3 = array;
				int num7 = 0;
				array3[num7] &= (byte)(255 >> num5);
				byte[] array4 = array;
				int num8 = 0;
				array4[num8] |= (byte)(128 >> num5);
			}
			else
			{
				array[0] = 0;
				byte[] array5 = array;
				int num9 = 1;
				array5[num9] |= 128;
				num6 = 1;
			}
			return this.engine.ProcessBlock(array, num6, array.Length - num6);
		}

		// Token: 0x0600367E RID: 13950 RVA: 0x0015157C File Offset: 0x0014F77C
		private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
		{
			byte[] array = this.engine.ProcessBlock(input, inOff, inLen);
			int num = 1;
			int num2 = (this.bitSize + 13) / 16;
			BigInteger bigInteger = new BigInteger(1, array);
			BigInteger bigInteger2;
			if (bigInteger.Mod(ISO9796d1Encoding.Sixteen).Equals(ISO9796d1Encoding.Six))
			{
				bigInteger2 = bigInteger;
			}
			else
			{
				bigInteger2 = this.modulus.Subtract(bigInteger);
				if (!bigInteger2.Mod(ISO9796d1Encoding.Sixteen).Equals(ISO9796d1Encoding.Six))
				{
					throw new InvalidCipherTextException("resulting integer iS or (modulus - iS) is not congruent to 6 mod 16");
				}
			}
			array = bigInteger2.ToByteArrayUnsigned();
			if ((array[array.Length - 1] & 15) != 6)
			{
				throw new InvalidCipherTextException("invalid forcing byte in block");
			}
			array[array.Length - 1] = (byte)((ushort)(array[array.Length - 1] & byte.MaxValue) >> 4 | (int)ISO9796d1Encoding.inverse[(array[array.Length - 2] & byte.MaxValue) >> 4] << 4);
			array[0] = (byte)((int)ISO9796d1Encoding.shadows[(int)((uint)(array[1] & byte.MaxValue) >> 4)] << 4 | (int)ISO9796d1Encoding.shadows[(int)(array[1] & 15)]);
			bool flag = false;
			int num3 = 0;
			for (int i = array.Length - 1; i >= array.Length - 2 * num2; i -= 2)
			{
				int num4 = (int)ISO9796d1Encoding.shadows[(int)((uint)(array[i] & byte.MaxValue) >> 4)] << 4 | (int)ISO9796d1Encoding.shadows[(int)(array[i] & 15)];
				if ((((int)array[i - 1] ^ num4) & 255) != 0)
				{
					if (flag)
					{
						throw new InvalidCipherTextException("invalid tsums in block");
					}
					flag = true;
					num = (((int)array[i - 1] ^ num4) & 255);
					num3 = i - 1;
				}
			}
			array[num3] = 0;
			byte[] array2 = new byte[(array.Length - num3) / 2];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = array[2 * j + num3 + 1];
			}
			this.padBits = num - 1;
			return array2;
		}

		// Token: 0x0400239E RID: 9118
		private static readonly BigInteger Sixteen = BigInteger.ValueOf(16L);

		// Token: 0x0400239F RID: 9119
		private static readonly BigInteger Six = BigInteger.ValueOf(6L);

		// Token: 0x040023A0 RID: 9120
		private static readonly byte[] shadows = new byte[]
		{
			14,
			3,
			5,
			8,
			9,
			4,
			2,
			15,
			0,
			13,
			11,
			6,
			7,
			10,
			12,
			1
		};

		// Token: 0x040023A1 RID: 9121
		private static readonly byte[] inverse = new byte[]
		{
			8,
			15,
			6,
			1,
			5,
			2,
			11,
			12,
			3,
			4,
			13,
			10,
			14,
			9,
			0,
			7
		};

		// Token: 0x040023A2 RID: 9122
		private readonly IAsymmetricBlockCipher engine;

		// Token: 0x040023A3 RID: 9123
		private bool forEncryption;

		// Token: 0x040023A4 RID: 9124
		private int bitSize;

		// Token: 0x040023A5 RID: 9125
		private int padBits;

		// Token: 0x040023A6 RID: 9126
		private BigInteger modulus;
	}
}
