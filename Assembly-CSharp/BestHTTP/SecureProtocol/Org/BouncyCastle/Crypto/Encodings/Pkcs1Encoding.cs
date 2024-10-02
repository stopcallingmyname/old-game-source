using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings
{
	// Token: 0x020005A0 RID: 1440
	public class Pkcs1Encoding : IAsymmetricBlockCipher
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x0600368E RID: 13966 RVA: 0x00151CD1 File Offset: 0x0014FED1
		// (set) Token: 0x0600368F RID: 13967 RVA: 0x00151CDA File Offset: 0x0014FEDA
		public static bool StrictLengthEnabled
		{
			get
			{
				return Pkcs1Encoding.strictLengthEnabled[0];
			}
			set
			{
				Pkcs1Encoding.strictLengthEnabled[0] = value;
			}
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x00151CE4 File Offset: 0x0014FEE4
		static Pkcs1Encoding()
		{
			string environmentVariable = Platform.GetEnvironmentVariable("BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs1.Strict");
			Pkcs1Encoding.strictLengthEnabled = new bool[]
			{
				environmentVariable == null || environmentVariable.Equals("true")
			};
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x00151D1B File Offset: 0x0014FF1B
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x00151D3C File Offset: 0x0014FF3C
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher, int pLen)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
			this.pLen = pLen;
		}

		// Token: 0x06003693 RID: 13971 RVA: 0x00151D64 File Offset: 0x0014FF64
		public Pkcs1Encoding(IAsymmetricBlockCipher cipher, byte[] fallback)
		{
			this.engine = cipher;
			this.useStrictLength = Pkcs1Encoding.StrictLengthEnabled;
			this.fallback = fallback;
			this.pLen = fallback.Length;
		}

		// Token: 0x06003694 RID: 13972 RVA: 0x00151D95 File Offset: 0x0014FF95
		public IAsymmetricBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x06003695 RID: 13973 RVA: 0x00151D9D File Offset: 0x0014FF9D
		public string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/PKCS1Padding";
			}
		}

		// Token: 0x06003696 RID: 13974 RVA: 0x00151DB4 File Offset: 0x0014FFB4
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				this.random = parametersWithRandom.Random;
				asymmetricKeyParameter = (AsymmetricKeyParameter)parametersWithRandom.Parameters;
			}
			else
			{
				this.random = new SecureRandom();
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			this.engine.Init(forEncryption, parameters);
			this.forPrivateKey = asymmetricKeyParameter.IsPrivate;
			this.forEncryption = forEncryption;
			this.blockBuffer = new byte[this.engine.GetOutputBlockSize()];
			if (this.pLen > 0 && this.fallback == null && this.random == null)
			{
				throw new ArgumentException("encoder requires random");
			}
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x00151E58 File Offset: 0x00150058
		public int GetInputBlockSize()
		{
			int inputBlockSize = this.engine.GetInputBlockSize();
			if (!this.forEncryption)
			{
				return inputBlockSize;
			}
			return inputBlockSize - 10;
		}

		// Token: 0x06003698 RID: 13976 RVA: 0x00151E80 File Offset: 0x00150080
		public int GetOutputBlockSize()
		{
			int outputBlockSize = this.engine.GetOutputBlockSize();
			if (!this.forEncryption)
			{
				return outputBlockSize - 10;
			}
			return outputBlockSize;
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x00151EA7 File Offset: 0x001500A7
		public byte[] ProcessBlock(byte[] input, int inOff, int length)
		{
			if (!this.forEncryption)
			{
				return this.DecodeBlock(input, inOff, length);
			}
			return this.EncodeBlock(input, inOff, length);
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x00151EC4 File Offset: 0x001500C4
		private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
		{
			if (inLen > this.GetInputBlockSize())
			{
				throw new ArgumentException("input data too large", "inLen");
			}
			byte[] array = new byte[this.engine.GetInputBlockSize()];
			if (this.forPrivateKey)
			{
				array[0] = 1;
				for (int num = 1; num != array.Length - inLen - 1; num++)
				{
					array[num] = byte.MaxValue;
				}
			}
			else
			{
				this.random.NextBytes(array);
				array[0] = 2;
				for (int num2 = 1; num2 != array.Length - inLen - 1; num2++)
				{
					while (array[num2] == 0)
					{
						array[num2] = (byte)this.random.NextInt();
					}
				}
			}
			array[array.Length - inLen - 1] = 0;
			Array.Copy(input, inOff, array, array.Length - inLen, inLen);
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x00151F84 File Offset: 0x00150184
		private static int CheckPkcs1Encoding(byte[] encoded, int pLen)
		{
			int num = 0;
			num |= (int)(encoded[0] ^ 2);
			int num2 = encoded.Length - (pLen + 1);
			for (int i = 1; i < num2; i++)
			{
				int num3 = (int)encoded[i];
				num3 |= num3 >> 1;
				num3 |= num3 >> 2;
				num3 |= num3 >> 4;
				num |= (num3 & 1) - 1;
			}
			num |= (int)encoded[encoded.Length - (pLen + 1)];
			num |= num >> 1;
			num |= num >> 2;
			num |= num >> 4;
			return ~((num & 1) - 1);
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x00151FF4 File Offset: 0x001501F4
		private byte[] DecodeBlockOrRandom(byte[] input, int inOff, int inLen)
		{
			if (!this.forPrivateKey)
			{
				throw new InvalidCipherTextException("sorry, this method is only for decryption, not for signing");
			}
			byte[] array = this.engine.ProcessBlock(input, inOff, inLen);
			byte[] array2;
			if (this.fallback == null)
			{
				array2 = new byte[this.pLen];
				this.random.NextBytes(array2);
			}
			else
			{
				array2 = this.fallback;
			}
			byte[] array3 = (this.useStrictLength & array.Length != this.engine.GetOutputBlockSize()) ? this.blockBuffer : array;
			int num = Pkcs1Encoding.CheckPkcs1Encoding(array3, this.pLen);
			byte[] array4 = new byte[this.pLen];
			for (int i = 0; i < this.pLen; i++)
			{
				array4[i] = (byte)(((int)array3[i + (array3.Length - this.pLen)] & ~num) | ((int)array2[i] & num));
			}
			Arrays.Fill(array3, 0);
			return array4;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x001520CC File Offset: 0x001502CC
		private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
		{
			if (this.pLen != -1)
			{
				return this.DecodeBlockOrRandom(input, inOff, inLen);
			}
			byte[] array = this.engine.ProcessBlock(input, inOff, inLen);
			bool flag = this.useStrictLength & array.Length != this.engine.GetOutputBlockSize();
			byte[] array2;
			if (array.Length < this.GetOutputBlockSize())
			{
				array2 = this.blockBuffer;
			}
			else
			{
				array2 = array;
			}
			byte b = this.forPrivateKey ? 2 : 1;
			byte b2 = array2[0];
			bool flag2 = b2 != b;
			int num = this.FindStart(b2, array2);
			num++;
			if (flag2 | num < 10)
			{
				Arrays.Fill(array2, 0);
				throw new InvalidCipherTextException("block incorrect");
			}
			if (flag)
			{
				Arrays.Fill(array2, 0);
				throw new InvalidCipherTextException("block incorrect size");
			}
			byte[] array3 = new byte[array2.Length - num];
			Array.Copy(array2, num, array3, 0, array3.Length);
			return array3;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x001521A4 File Offset: 0x001503A4
		private int FindStart(byte type, byte[] block)
		{
			int num = -1;
			bool flag = false;
			for (int num2 = 1; num2 != block.Length; num2++)
			{
				byte b = block[num2];
				if (b == 0 & num < 0)
				{
					num = num2;
				}
				flag |= (type == 1 & num < 0 & b != byte.MaxValue);
			}
			if (!flag)
			{
				return num;
			}
			return -1;
		}

		// Token: 0x040023AC RID: 9132
		public const string StrictLengthEnabledProperty = "BestHTTP.SecureProtocol.Org.BouncyCastle.Pkcs1.Strict";

		// Token: 0x040023AD RID: 9133
		private const int HeaderLength = 10;

		// Token: 0x040023AE RID: 9134
		private static readonly bool[] strictLengthEnabled;

		// Token: 0x040023AF RID: 9135
		private SecureRandom random;

		// Token: 0x040023B0 RID: 9136
		private IAsymmetricBlockCipher engine;

		// Token: 0x040023B1 RID: 9137
		private bool forEncryption;

		// Token: 0x040023B2 RID: 9138
		private bool forPrivateKey;

		// Token: 0x040023B3 RID: 9139
		private bool useStrictLength;

		// Token: 0x040023B4 RID: 9140
		private int pLen = -1;

		// Token: 0x040023B5 RID: 9141
		private byte[] fallback;

		// Token: 0x040023B6 RID: 9142
		private byte[] blockBuffer;
	}
}
