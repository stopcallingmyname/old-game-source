using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg
{
	// Token: 0x020004BD RID: 1213
	public class HashSP800Drbg : ISP80090Drbg
	{
		// Token: 0x06002F4C RID: 12108 RVA: 0x00124E2C File Offset: 0x0012302C
		static HashSP800Drbg()
		{
			HashSP800Drbg.seedlens.Add("SHA-1", 440);
			HashSP800Drbg.seedlens.Add("SHA-224", 440);
			HashSP800Drbg.seedlens.Add("SHA-256", 440);
			HashSP800Drbg.seedlens.Add("SHA-512/256", 440);
			HashSP800Drbg.seedlens.Add("SHA-512/224", 440);
			HashSP800Drbg.seedlens.Add("SHA-384", 888);
			HashSP800Drbg.seedlens.Add("SHA-512", 888);
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x00124F1C File Offset: 0x0012311C
		public HashSP800Drbg(IDigest digest, int securityStrength, IEntropySource entropySource, byte[] personalizationString, byte[] nonce)
		{
			if (securityStrength > DrbgUtilities.GetMaxSecurityStrength(digest))
			{
				throw new ArgumentException("Requested security strength is not supported by the derivation function");
			}
			if (entropySource.EntropySize < securityStrength)
			{
				throw new ArgumentException("Not enough entropy for security strength required");
			}
			this.mDigest = digest;
			this.mEntropySource = entropySource;
			this.mSecurityStrength = securityStrength;
			this.mSeedLength = (int)HashSP800Drbg.seedlens[digest.AlgorithmName];
			byte[] entropy = this.GetEntropy();
			byte[] seedMaterial = Arrays.ConcatenateAll(new byte[][]
			{
				entropy,
				nonce,
				personalizationString
			});
			byte[] array = DrbgUtilities.HashDF(this.mDigest, seedMaterial, this.mSeedLength);
			this.mV = array;
			byte[] array2 = new byte[this.mV.Length + 1];
			Array.Copy(this.mV, 0, array2, 1, this.mV.Length);
			this.mC = DrbgUtilities.HashDF(this.mDigest, array2, this.mSeedLength);
			this.mReseedCounter = 1L;
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x06002F4E RID: 12110 RVA: 0x00125008 File Offset: 0x00123208
		public int BlockSize
		{
			get
			{
				return this.mDigest.GetDigestSize() * 8;
			}
		}

		// Token: 0x06002F4F RID: 12111 RVA: 0x00125018 File Offset: 0x00123218
		public int Generate(byte[] output, byte[] additionalInput, bool predictionResistant)
		{
			int num = output.Length * 8;
			if (num > HashSP800Drbg.MAX_BITS_REQUEST)
			{
				throw new ArgumentException("Number of bits per request limited to " + HashSP800Drbg.MAX_BITS_REQUEST, "output");
			}
			if (this.mReseedCounter > HashSP800Drbg.RESEED_MAX)
			{
				return -1;
			}
			if (predictionResistant)
			{
				this.Reseed(additionalInput);
				additionalInput = null;
			}
			if (additionalInput != null)
			{
				byte[] array = new byte[1 + this.mV.Length + additionalInput.Length];
				array[0] = 2;
				Array.Copy(this.mV, 0, array, 1, this.mV.Length);
				Array.Copy(additionalInput, 0, array, 1 + this.mV.Length, additionalInput.Length);
				byte[] shorter = this.Hash(array);
				this.AddTo(this.mV, shorter);
			}
			Array sourceArray = this.hashgen(this.mV, num);
			byte[] array2 = new byte[this.mV.Length + 1];
			Array.Copy(this.mV, 0, array2, 1, this.mV.Length);
			array2[0] = 3;
			byte[] shorter2 = this.Hash(array2);
			this.AddTo(this.mV, shorter2);
			this.AddTo(this.mV, this.mC);
			byte[] shorter3 = new byte[]
			{
				(byte)(this.mReseedCounter >> 24),
				(byte)(this.mReseedCounter >> 16),
				(byte)(this.mReseedCounter >> 8),
				(byte)this.mReseedCounter
			};
			this.AddTo(this.mV, shorter3);
			this.mReseedCounter += 1L;
			Array.Copy(sourceArray, 0, output, 0, output.Length);
			return num;
		}

		// Token: 0x06002F50 RID: 12112 RVA: 0x0012518D File Offset: 0x0012338D
		private byte[] GetEntropy()
		{
			byte[] entropy = this.mEntropySource.GetEntropy();
			if (entropy.Length < (this.mSecurityStrength + 7) / 8)
			{
				throw new InvalidOperationException("Insufficient entropy provided by entropy source");
			}
			return entropy;
		}

		// Token: 0x06002F51 RID: 12113 RVA: 0x001251B4 File Offset: 0x001233B4
		private void AddTo(byte[] longer, byte[] shorter)
		{
			int num = longer.Length - shorter.Length;
			uint num2 = 0U;
			int num3 = shorter.Length;
			while (--num3 >= 0)
			{
				num2 += (uint)(longer[num + num3] + shorter[num3]);
				longer[num + num3] = (byte)num2;
				num2 >>= 8;
			}
			num3 = num;
			while (--num3 >= 0)
			{
				num2 += (uint)longer[num3];
				longer[num3] = (byte)num2;
				num2 >>= 8;
			}
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x0012520C File Offset: 0x0012340C
		public void Reseed(byte[] additionalInput)
		{
			byte[] entropy = this.GetEntropy();
			byte[] seedMaterial = Arrays.ConcatenateAll(new byte[][]
			{
				HashSP800Drbg.ONE,
				this.mV,
				entropy,
				additionalInput
			});
			byte[] array = DrbgUtilities.HashDF(this.mDigest, seedMaterial, this.mSeedLength);
			this.mV = array;
			byte[] array2 = new byte[this.mV.Length + 1];
			array2[0] = 0;
			Array.Copy(this.mV, 0, array2, 1, this.mV.Length);
			this.mC = DrbgUtilities.HashDF(this.mDigest, array2, this.mSeedLength);
			this.mReseedCounter = 1L;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x001252AC File Offset: 0x001234AC
		private byte[] Hash(byte[] input)
		{
			byte[] array = new byte[this.mDigest.GetDigestSize()];
			this.DoHash(input, array);
			return array;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x001252D3 File Offset: 0x001234D3
		private void DoHash(byte[] input, byte[] output)
		{
			this.mDigest.BlockUpdate(input, 0, input.Length);
			this.mDigest.DoFinal(output, 0);
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x001252F4 File Offset: 0x001234F4
		private byte[] hashgen(byte[] input, int lengthInBits)
		{
			int digestSize = this.mDigest.GetDigestSize();
			int num = lengthInBits / 8 / digestSize;
			byte[] array = new byte[input.Length];
			Array.Copy(input, 0, array, 0, input.Length);
			byte[] array2 = new byte[lengthInBits / 8];
			byte[] array3 = new byte[this.mDigest.GetDigestSize()];
			for (int i = 0; i <= num; i++)
			{
				this.DoHash(array, array3);
				int length = (array2.Length - i * array3.Length > array3.Length) ? array3.Length : (array2.Length - i * array3.Length);
				Array.Copy(array3, 0, array2, i * array3.Length, length);
				this.AddTo(array, HashSP800Drbg.ONE);
			}
			return array2;
		}

		// Token: 0x04001F95 RID: 8085
		private static readonly byte[] ONE = new byte[]
		{
			1
		};

		// Token: 0x04001F96 RID: 8086
		private static readonly long RESEED_MAX = 140737488355328L;

		// Token: 0x04001F97 RID: 8087
		private static readonly int MAX_BITS_REQUEST = 262144;

		// Token: 0x04001F98 RID: 8088
		private static readonly IDictionary seedlens = Platform.CreateHashtable();

		// Token: 0x04001F99 RID: 8089
		private readonly IDigest mDigest;

		// Token: 0x04001F9A RID: 8090
		private readonly IEntropySource mEntropySource;

		// Token: 0x04001F9B RID: 8091
		private readonly int mSecurityStrength;

		// Token: 0x04001F9C RID: 8092
		private readonly int mSeedLength;

		// Token: 0x04001F9D RID: 8093
		private byte[] mV;

		// Token: 0x04001F9E RID: 8094
		private byte[] mC;

		// Token: 0x04001F9F RID: 8095
		private long mReseedCounter;
	}
}
