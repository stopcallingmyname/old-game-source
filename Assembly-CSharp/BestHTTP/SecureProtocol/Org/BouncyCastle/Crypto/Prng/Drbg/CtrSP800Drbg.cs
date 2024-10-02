using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg
{
	// Token: 0x020004BB RID: 1211
	public class CtrSP800Drbg : ISP80090Drbg
	{
		// Token: 0x06002F34 RID: 12084 RVA: 0x001243E0 File Offset: 0x001225E0
		public CtrSP800Drbg(IBlockCipher engine, int keySizeInBits, int securityStrength, IEntropySource entropySource, byte[] personalizationString, byte[] nonce)
		{
			if (securityStrength > 256)
			{
				throw new ArgumentException("Requested security strength is not supported by the derivation function");
			}
			if (this.GetMaxSecurityStrength(engine, keySizeInBits) < securityStrength)
			{
				throw new ArgumentException("Requested security strength is not supported by block cipher and key size");
			}
			if (entropySource.EntropySize < securityStrength)
			{
				throw new ArgumentException("Not enough entropy for security strength required");
			}
			this.mEntropySource = entropySource;
			this.mEngine = engine;
			this.mKeySizeInBits = keySizeInBits;
			this.mSecurityStrength = securityStrength;
			this.mSeedLength = keySizeInBits + engine.GetBlockSize() * 8;
			this.mIsTdea = this.IsTdea(engine);
			byte[] entropy = this.GetEntropy();
			this.CTR_DRBG_Instantiate_algorithm(entropy, nonce, personalizationString);
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x00124480 File Offset: 0x00122680
		private void CTR_DRBG_Instantiate_algorithm(byte[] entropy, byte[] nonce, byte[] personalisationString)
		{
			byte[] inputString = Arrays.ConcatenateAll(new byte[][]
			{
				entropy,
				nonce,
				personalisationString
			});
			byte[] seed = this.Block_Cipher_df(inputString, this.mSeedLength);
			int blockSize = this.mEngine.GetBlockSize();
			this.mKey = new byte[(this.mKeySizeInBits + 7) / 8];
			this.mV = new byte[blockSize];
			this.CTR_DRBG_Update(seed, this.mKey, this.mV);
			this.mReseedCounter = 1L;
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x001244FC File Offset: 0x001226FC
		private void CTR_DRBG_Update(byte[] seed, byte[] key, byte[] v)
		{
			byte[] array = new byte[seed.Length];
			byte[] array2 = new byte[this.mEngine.GetBlockSize()];
			int num = 0;
			int blockSize = this.mEngine.GetBlockSize();
			this.mEngine.Init(true, new KeyParameter(this.ExpandKey(key)));
			while (num * blockSize < seed.Length)
			{
				this.AddOneTo(v);
				this.mEngine.ProcessBlock(v, 0, array2, 0);
				int length = (array.Length - num * blockSize > blockSize) ? blockSize : (array.Length - num * blockSize);
				Array.Copy(array2, 0, array, num * blockSize, length);
				num++;
			}
			this.XOR(array, seed, array, 0);
			Array.Copy(array, 0, key, 0, key.Length);
			Array.Copy(array, key.Length, v, 0, v.Length);
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x001245B8 File Offset: 0x001227B8
		private void CTR_DRBG_Reseed_algorithm(byte[] additionalInput)
		{
			byte[] array = Arrays.Concatenate(this.GetEntropy(), additionalInput);
			array = this.Block_Cipher_df(array, this.mSeedLength);
			this.CTR_DRBG_Update(array, this.mKey, this.mV);
			this.mReseedCounter = 1L;
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x001245FC File Offset: 0x001227FC
		private void XOR(byte[] output, byte[] a, byte[] b, int bOff)
		{
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = (a[i] ^ b[bOff + i]);
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x00124628 File Offset: 0x00122828
		private void AddOneTo(byte[] longer)
		{
			uint num = 1U;
			int num2 = longer.Length;
			while (--num2 >= 0)
			{
				num += (uint)longer[num2];
				longer[num2] = (byte)num;
				num >>= 8;
			}
		}

		// Token: 0x06002F3A RID: 12090 RVA: 0x00124654 File Offset: 0x00122854
		private byte[] GetEntropy()
		{
			byte[] entropy = this.mEntropySource.GetEntropy();
			if (entropy.Length < (this.mSecurityStrength + 7) / 8)
			{
				throw new InvalidOperationException("Insufficient entropy provided by entropy source");
			}
			return entropy;
		}

		// Token: 0x06002F3B RID: 12091 RVA: 0x0012467C File Offset: 0x0012287C
		private byte[] Block_Cipher_df(byte[] inputString, int bitLength)
		{
			int blockSize = this.mEngine.GetBlockSize();
			int num = inputString.Length;
			int value = bitLength / 8;
			byte[] array = new byte[(8 + num + 1 + blockSize - 1) / blockSize * blockSize];
			this.copyIntToByteArray(array, num, 0);
			this.copyIntToByteArray(array, value, 4);
			Array.Copy(inputString, 0, array, 8, num);
			array[8 + num] = 128;
			byte[] array2 = new byte[this.mKeySizeInBits / 8 + blockSize];
			byte[] array3 = new byte[blockSize];
			byte[] array4 = new byte[blockSize];
			int num2 = 0;
			byte[] array5 = new byte[this.mKeySizeInBits / 8];
			Array.Copy(CtrSP800Drbg.K_BITS, 0, array5, 0, array5.Length);
			while (num2 * blockSize * 8 < this.mKeySizeInBits + blockSize * 8)
			{
				this.copyIntToByteArray(array4, num2, 0);
				this.BCC(array3, array5, array4, array);
				int length = (array2.Length - num2 * blockSize > blockSize) ? blockSize : (array2.Length - num2 * blockSize);
				Array.Copy(array3, 0, array2, num2 * blockSize, length);
				num2++;
			}
			byte[] array6 = new byte[blockSize];
			Array.Copy(array2, 0, array5, 0, array5.Length);
			Array.Copy(array2, array5.Length, array6, 0, array6.Length);
			array2 = new byte[bitLength / 2];
			num2 = 0;
			this.mEngine.Init(true, new KeyParameter(this.ExpandKey(array5)));
			while (num2 * blockSize < array2.Length)
			{
				this.mEngine.ProcessBlock(array6, 0, array6, 0);
				int length2 = (array2.Length - num2 * blockSize > blockSize) ? blockSize : (array2.Length - num2 * blockSize);
				Array.Copy(array6, 0, array2, num2 * blockSize, length2);
				num2++;
			}
			return array2;
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x00124818 File Offset: 0x00122A18
		private void BCC(byte[] bccOut, byte[] k, byte[] iV, byte[] data)
		{
			int blockSize = this.mEngine.GetBlockSize();
			byte[] array = new byte[blockSize];
			int num = data.Length / blockSize;
			byte[] array2 = new byte[blockSize];
			this.mEngine.Init(true, new KeyParameter(this.ExpandKey(k)));
			this.mEngine.ProcessBlock(iV, 0, array, 0);
			for (int i = 0; i < num; i++)
			{
				this.XOR(array2, array, data, i * blockSize);
				this.mEngine.ProcessBlock(array2, 0, array, 0);
			}
			Array.Copy(array, 0, bccOut, 0, bccOut.Length);
		}

		// Token: 0x06002F3D RID: 12093 RVA: 0x001248A8 File Offset: 0x00122AA8
		private void copyIntToByteArray(byte[] buf, int value, int offSet)
		{
			buf[offSet] = (byte)(value >> 24);
			buf[offSet + 1] = (byte)(value >> 16);
			buf[offSet + 2] = (byte)(value >> 8);
			buf[offSet + 3] = (byte)value;
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x06002F3E RID: 12094 RVA: 0x001248CC File Offset: 0x00122ACC
		public int BlockSize
		{
			get
			{
				return this.mV.Length * 8;
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x001248D8 File Offset: 0x00122AD8
		public int Generate(byte[] output, byte[] additionalInput, bool predictionResistant)
		{
			if (this.mIsTdea)
			{
				if (this.mReseedCounter > CtrSP800Drbg.TDEA_RESEED_MAX)
				{
					return -1;
				}
				if (DrbgUtilities.IsTooLarge(output, CtrSP800Drbg.TDEA_MAX_BITS_REQUEST / 8))
				{
					throw new ArgumentException("Number of bits per request limited to " + CtrSP800Drbg.TDEA_MAX_BITS_REQUEST, "output");
				}
			}
			else
			{
				if (this.mReseedCounter > CtrSP800Drbg.AES_RESEED_MAX)
				{
					return -1;
				}
				if (DrbgUtilities.IsTooLarge(output, CtrSP800Drbg.AES_MAX_BITS_REQUEST / 8))
				{
					throw new ArgumentException("Number of bits per request limited to " + CtrSP800Drbg.AES_MAX_BITS_REQUEST, "output");
				}
			}
			if (predictionResistant)
			{
				this.CTR_DRBG_Reseed_algorithm(additionalInput);
				additionalInput = null;
			}
			if (additionalInput != null)
			{
				additionalInput = this.Block_Cipher_df(additionalInput, this.mSeedLength);
				this.CTR_DRBG_Update(additionalInput, this.mKey, this.mV);
			}
			else
			{
				additionalInput = new byte[this.mSeedLength];
			}
			byte[] array = new byte[this.mV.Length];
			this.mEngine.Init(true, new KeyParameter(this.ExpandKey(this.mKey)));
			for (int i = 0; i <= output.Length / array.Length; i++)
			{
				int num = (output.Length - i * array.Length > array.Length) ? array.Length : (output.Length - i * this.mV.Length);
				if (num != 0)
				{
					this.AddOneTo(this.mV);
					this.mEngine.ProcessBlock(this.mV, 0, array, 0);
					Array.Copy(array, 0, output, i * array.Length, num);
				}
			}
			this.CTR_DRBG_Update(additionalInput, this.mKey, this.mV);
			this.mReseedCounter += 1L;
			return output.Length * 8;
		}

		// Token: 0x06002F40 RID: 12096 RVA: 0x00124A60 File Offset: 0x00122C60
		public void Reseed(byte[] additionalInput)
		{
			this.CTR_DRBG_Reseed_algorithm(additionalInput);
		}

		// Token: 0x06002F41 RID: 12097 RVA: 0x00124A69 File Offset: 0x00122C69
		private bool IsTdea(IBlockCipher cipher)
		{
			return cipher.AlgorithmName.Equals("DESede") || cipher.AlgorithmName.Equals("TDEA");
		}

		// Token: 0x06002F42 RID: 12098 RVA: 0x00124A8F File Offset: 0x00122C8F
		private int GetMaxSecurityStrength(IBlockCipher cipher, int keySizeInBits)
		{
			if (this.IsTdea(cipher) && keySizeInBits == 168)
			{
				return 112;
			}
			if (cipher.AlgorithmName.Equals("AES"))
			{
				return keySizeInBits;
			}
			return -1;
		}

		// Token: 0x06002F43 RID: 12099 RVA: 0x00124ABC File Offset: 0x00122CBC
		private byte[] ExpandKey(byte[] key)
		{
			if (this.mIsTdea)
			{
				byte[] array = new byte[24];
				this.PadKey(key, 0, array, 0);
				this.PadKey(key, 7, array, 8);
				this.PadKey(key, 14, array, 16);
				return array;
			}
			return key;
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x00124AFC File Offset: 0x00122CFC
		private void PadKey(byte[] keyMaster, int keyOff, byte[] tmp, int tmpOff)
		{
			tmp[tmpOff] = (keyMaster[keyOff] & 254);
			tmp[tmpOff + 1] = (byte)((int)keyMaster[keyOff] << 7 | (keyMaster[keyOff + 1] & 252) >> 1);
			tmp[tmpOff + 2] = (byte)((int)keyMaster[keyOff + 1] << 6 | (keyMaster[keyOff + 2] & 248) >> 2);
			tmp[tmpOff + 3] = (byte)((int)keyMaster[keyOff + 2] << 5 | (keyMaster[keyOff + 3] & 240) >> 3);
			tmp[tmpOff + 4] = (byte)((int)keyMaster[keyOff + 3] << 4 | (keyMaster[keyOff + 4] & 224) >> 4);
			tmp[tmpOff + 5] = (byte)((int)keyMaster[keyOff + 4] << 3 | (keyMaster[keyOff + 5] & 192) >> 5);
			tmp[tmpOff + 6] = (byte)((int)keyMaster[keyOff + 5] << 2 | (keyMaster[keyOff + 6] & 128) >> 6);
			tmp[tmpOff + 7] = (byte)(keyMaster[keyOff + 6] << 1);
			DesParameters.SetOddParity(tmp, tmpOff, 8);
		}

		// Token: 0x04001F86 RID: 8070
		private static readonly long TDEA_RESEED_MAX = (long)((ulong)int.MinValue);

		// Token: 0x04001F87 RID: 8071
		private static readonly long AES_RESEED_MAX = 140737488355328L;

		// Token: 0x04001F88 RID: 8072
		private static readonly int TDEA_MAX_BITS_REQUEST = 4096;

		// Token: 0x04001F89 RID: 8073
		private static readonly int AES_MAX_BITS_REQUEST = 262144;

		// Token: 0x04001F8A RID: 8074
		private readonly IEntropySource mEntropySource;

		// Token: 0x04001F8B RID: 8075
		private readonly IBlockCipher mEngine;

		// Token: 0x04001F8C RID: 8076
		private readonly int mKeySizeInBits;

		// Token: 0x04001F8D RID: 8077
		private readonly int mSeedLength;

		// Token: 0x04001F8E RID: 8078
		private readonly int mSecurityStrength;

		// Token: 0x04001F8F RID: 8079
		private byte[] mKey;

		// Token: 0x04001F90 RID: 8080
		private byte[] mV;

		// Token: 0x04001F91 RID: 8081
		private long mReseedCounter;

		// Token: 0x04001F92 RID: 8082
		private bool mIsTdea;

		// Token: 0x04001F93 RID: 8083
		private static readonly byte[] K_BITS = Hex.Decode("000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F");
	}
}
