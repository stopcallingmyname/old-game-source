using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings
{
	// Token: 0x0200059F RID: 1439
	public class OaepEncoding : IAsymmetricBlockCipher
	{
		// Token: 0x06003680 RID: 13952 RVA: 0x00151788 File Offset: 0x0014F988
		public OaepEncoding(IAsymmetricBlockCipher cipher) : this(cipher, new Sha1Digest(), null)
		{
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x00151797 File Offset: 0x0014F997
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash) : this(cipher, hash, null)
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x001517A2 File Offset: 0x0014F9A2
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, byte[] encodingParams) : this(cipher, hash, hash, encodingParams)
		{
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x001517B0 File Offset: 0x0014F9B0
		public OaepEncoding(IAsymmetricBlockCipher cipher, IDigest hash, IDigest mgf1Hash, byte[] encodingParams)
		{
			this.engine = cipher;
			this.mgf1Hash = mgf1Hash;
			this.defHash = new byte[hash.GetDigestSize()];
			hash.Reset();
			if (encodingParams != null)
			{
				hash.BlockUpdate(encodingParams, 0, encodingParams.Length);
			}
			hash.DoFinal(this.defHash, 0);
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00151807 File Offset: 0x0014FA07
		public IAsymmetricBlockCipher GetUnderlyingCipher()
		{
			return this.engine;
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06003685 RID: 13957 RVA: 0x0015180F File Offset: 0x0014FA0F
		public string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName + "/OAEPPadding";
			}
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x00151828 File Offset: 0x0014FA28
		public void Init(bool forEncryption, ICipherParameters param)
		{
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.random = parametersWithRandom.Random;
			}
			else
			{
				this.random = new SecureRandom();
			}
			this.engine.Init(forEncryption, param);
			this.forEncryption = forEncryption;
		}

		// Token: 0x06003687 RID: 13959 RVA: 0x00151874 File Offset: 0x0014FA74
		public int GetInputBlockSize()
		{
			int inputBlockSize = this.engine.GetInputBlockSize();
			if (this.forEncryption)
			{
				return inputBlockSize - 1 - 2 * this.defHash.Length;
			}
			return inputBlockSize;
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x001518A8 File Offset: 0x0014FAA8
		public int GetOutputBlockSize()
		{
			int outputBlockSize = this.engine.GetOutputBlockSize();
			if (this.forEncryption)
			{
				return outputBlockSize;
			}
			return outputBlockSize - 1 - 2 * this.defHash.Length;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x001518D9 File Offset: 0x0014FAD9
		public byte[] ProcessBlock(byte[] inBytes, int inOff, int inLen)
		{
			if (this.forEncryption)
			{
				return this.EncodeBlock(inBytes, inOff, inLen);
			}
			return this.DecodeBlock(inBytes, inOff, inLen);
		}

		// Token: 0x0600368A RID: 13962 RVA: 0x001518F8 File Offset: 0x0014FAF8
		private byte[] EncodeBlock(byte[] inBytes, int inOff, int inLen)
		{
			Check.DataLength(inLen > this.GetInputBlockSize(), "input data too long");
			byte[] array = new byte[this.GetInputBlockSize() + 1 + 2 * this.defHash.Length];
			Array.Copy(inBytes, inOff, array, array.Length - inLen, inLen);
			array[array.Length - inLen - 1] = 1;
			Array.Copy(this.defHash, 0, array, this.defHash.Length, this.defHash.Length);
			byte[] nextBytes = SecureRandom.GetNextBytes(this.random, this.defHash.Length);
			byte[] array2 = this.maskGeneratorFunction1(nextBytes, 0, nextBytes.Length, array.Length - this.defHash.Length);
			for (int num = this.defHash.Length; num != array.Length; num++)
			{
				byte[] array3 = array;
				int num2 = num;
				array3[num2] ^= array2[num - this.defHash.Length];
			}
			Array.Copy(nextBytes, 0, array, 0, this.defHash.Length);
			array2 = this.maskGeneratorFunction1(array, this.defHash.Length, array.Length - this.defHash.Length, this.defHash.Length);
			for (int num3 = 0; num3 != this.defHash.Length; num3++)
			{
				byte[] array4 = array;
				int num4 = num3;
				array4[num4] ^= array2[num3];
			}
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x00151A2C File Offset: 0x0014FC2C
		private byte[] DecodeBlock(byte[] inBytes, int inOff, int inLen)
		{
			byte[] array = this.engine.ProcessBlock(inBytes, inOff, inLen);
			byte[] array2 = new byte[this.engine.GetOutputBlockSize()];
			bool flag = array2.Length < 2 * this.defHash.Length + 1;
			if (array.Length <= array2.Length)
			{
				Array.Copy(array, 0, array2, array2.Length - array.Length, array.Length);
			}
			else
			{
				Array.Copy(array, 0, array2, 0, array2.Length);
				flag = true;
			}
			byte[] array3 = this.maskGeneratorFunction1(array2, this.defHash.Length, array2.Length - this.defHash.Length, this.defHash.Length);
			for (int num = 0; num != this.defHash.Length; num++)
			{
				byte[] array4 = array2;
				int num2 = num;
				array4[num2] ^= array3[num];
			}
			array3 = this.maskGeneratorFunction1(array2, 0, this.defHash.Length, array2.Length - this.defHash.Length);
			for (int num3 = this.defHash.Length; num3 != array2.Length; num3++)
			{
				byte[] array5 = array2;
				int num4 = num3;
				array5[num4] ^= array3[num3 - this.defHash.Length];
			}
			bool flag2 = false;
			for (int num5 = 0; num5 != this.defHash.Length; num5++)
			{
				if (this.defHash[num5] != array2[this.defHash.Length + num5])
				{
					flag2 = true;
				}
			}
			int num6 = array2.Length;
			for (int num7 = 2 * this.defHash.Length; num7 != array2.Length; num7++)
			{
				if (array2[num7] > 0 & num6 == array2.Length)
				{
					num6 = num7;
				}
			}
			bool flag3 = num6 > array2.Length - 1 | array2[num6] != 1;
			num6++;
			if (flag2 || flag || flag3)
			{
				Arrays.Fill(array2, 0);
				throw new InvalidCipherTextException("data wrong");
			}
			byte[] array6 = new byte[array2.Length - num6];
			Array.Copy(array2, num6, array6, 0, array6.Length);
			return array6;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x001212E0 File Offset: 0x0011F4E0
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x0600368D RID: 13965 RVA: 0x00151BF0 File Offset: 0x0014FDF0
		private byte[] maskGeneratorFunction1(byte[] Z, int zOff, int zLen, int length)
		{
			byte[] array = new byte[length];
			byte[] array2 = new byte[this.mgf1Hash.GetDigestSize()];
			byte[] array3 = new byte[4];
			int i = 0;
			this.mgf1Hash.Reset();
			while (i < length / array2.Length)
			{
				this.ItoOSP(i, array3);
				this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
				this.mgf1Hash.BlockUpdate(array3, 0, array3.Length);
				this.mgf1Hash.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, i * array2.Length, array2.Length);
				i++;
			}
			if (i * array2.Length < length)
			{
				this.ItoOSP(i, array3);
				this.mgf1Hash.BlockUpdate(Z, zOff, zLen);
				this.mgf1Hash.BlockUpdate(array3, 0, array3.Length);
				this.mgf1Hash.DoFinal(array2, 0);
				Array.Copy(array2, 0, array, i * array2.Length, array.Length - i * array2.Length);
			}
			return array;
		}

		// Token: 0x040023A7 RID: 9127
		private byte[] defHash;

		// Token: 0x040023A8 RID: 9128
		private IDigest mgf1Hash;

		// Token: 0x040023A9 RID: 9129
		private IAsymmetricBlockCipher engine;

		// Token: 0x040023AA RID: 9130
		private SecureRandom random;

		// Token: 0x040023AB RID: 9131
		private bool forEncryption;
	}
}
