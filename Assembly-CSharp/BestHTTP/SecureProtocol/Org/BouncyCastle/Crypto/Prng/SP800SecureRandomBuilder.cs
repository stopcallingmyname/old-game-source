using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng
{
	// Token: 0x020004B5 RID: 1205
	public class SP800SecureRandomBuilder
	{
		// Token: 0x06002F12 RID: 12050 RVA: 0x00123B70 File Offset: 0x00121D70
		public SP800SecureRandomBuilder() : this(new SecureRandom(), false)
		{
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x00123B7E File Offset: 0x00121D7E
		public SP800SecureRandomBuilder(SecureRandom entropySource, bool predictionResistant)
		{
			this.mSecurityStrength = 256;
			this.mEntropyBitsRequired = 256;
			base..ctor();
			this.mRandom = entropySource;
			this.mEntropySourceProvider = new BasicEntropySourceProvider(entropySource, predictionResistant);
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x00123BB0 File Offset: 0x00121DB0
		public SP800SecureRandomBuilder(IEntropySourceProvider entropySourceProvider)
		{
			this.mSecurityStrength = 256;
			this.mEntropyBitsRequired = 256;
			base..ctor();
			this.mRandom = null;
			this.mEntropySourceProvider = entropySourceProvider;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x00123BDC File Offset: 0x00121DDC
		public SP800SecureRandomBuilder SetPersonalizationString(byte[] personalizationString)
		{
			this.mPersonalizationString = personalizationString;
			return this;
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x00123BE6 File Offset: 0x00121DE6
		public SP800SecureRandomBuilder SetSecurityStrength(int securityStrength)
		{
			this.mSecurityStrength = securityStrength;
			return this;
		}

		// Token: 0x06002F17 RID: 12055 RVA: 0x00123BF0 File Offset: 0x00121DF0
		public SP800SecureRandomBuilder SetEntropyBitsRequired(int entropyBitsRequired)
		{
			this.mEntropyBitsRequired = entropyBitsRequired;
			return this;
		}

		// Token: 0x06002F18 RID: 12056 RVA: 0x00123BFA File Offset: 0x00121DFA
		public SP800SecureRandom BuildHash(IDigest digest, byte[] nonce, bool predictionResistant)
		{
			return new SP800SecureRandom(this.mRandom, this.mEntropySourceProvider.Get(this.mEntropyBitsRequired), new SP800SecureRandomBuilder.HashDrbgProvider(digest, nonce, this.mPersonalizationString, this.mSecurityStrength), predictionResistant);
		}

		// Token: 0x06002F19 RID: 12057 RVA: 0x00123C2C File Offset: 0x00121E2C
		public SP800SecureRandom BuildCtr(IBlockCipher cipher, int keySizeInBits, byte[] nonce, bool predictionResistant)
		{
			return new SP800SecureRandom(this.mRandom, this.mEntropySourceProvider.Get(this.mEntropyBitsRequired), new SP800SecureRandomBuilder.CtrDrbgProvider(cipher, keySizeInBits, nonce, this.mPersonalizationString, this.mSecurityStrength), predictionResistant);
		}

		// Token: 0x06002F1A RID: 12058 RVA: 0x00123C60 File Offset: 0x00121E60
		public SP800SecureRandom BuildHMac(IMac hMac, byte[] nonce, bool predictionResistant)
		{
			return new SP800SecureRandom(this.mRandom, this.mEntropySourceProvider.Get(this.mEntropyBitsRequired), new SP800SecureRandomBuilder.HMacDrbgProvider(hMac, nonce, this.mPersonalizationString, this.mSecurityStrength), predictionResistant);
		}

		// Token: 0x04001F6D RID: 8045
		private readonly SecureRandom mRandom;

		// Token: 0x04001F6E RID: 8046
		private readonly IEntropySourceProvider mEntropySourceProvider;

		// Token: 0x04001F6F RID: 8047
		private byte[] mPersonalizationString;

		// Token: 0x04001F70 RID: 8048
		private int mSecurityStrength;

		// Token: 0x04001F71 RID: 8049
		private int mEntropyBitsRequired;

		// Token: 0x02000950 RID: 2384
		private class HashDrbgProvider : IDrbgProvider
		{
			// Token: 0x06004F07 RID: 20231 RVA: 0x001B3A66 File Offset: 0x001B1C66
			public HashDrbgProvider(IDigest digest, byte[] nonce, byte[] personalizationString, int securityStrength)
			{
				this.mDigest = digest;
				this.mNonce = nonce;
				this.mPersonalizationString = personalizationString;
				this.mSecurityStrength = securityStrength;
			}

			// Token: 0x06004F08 RID: 20232 RVA: 0x001B3A8B File Offset: 0x001B1C8B
			public ISP80090Drbg Get(IEntropySource entropySource)
			{
				return new HashSP800Drbg(this.mDigest, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce);
			}

			// Token: 0x04003627 RID: 13863
			private readonly IDigest mDigest;

			// Token: 0x04003628 RID: 13864
			private readonly byte[] mNonce;

			// Token: 0x04003629 RID: 13865
			private readonly byte[] mPersonalizationString;

			// Token: 0x0400362A RID: 13866
			private readonly int mSecurityStrength;
		}

		// Token: 0x02000951 RID: 2385
		private class HMacDrbgProvider : IDrbgProvider
		{
			// Token: 0x06004F09 RID: 20233 RVA: 0x001B3AAB File Offset: 0x001B1CAB
			public HMacDrbgProvider(IMac hMac, byte[] nonce, byte[] personalizationString, int securityStrength)
			{
				this.mHMac = hMac;
				this.mNonce = nonce;
				this.mPersonalizationString = personalizationString;
				this.mSecurityStrength = securityStrength;
			}

			// Token: 0x06004F0A RID: 20234 RVA: 0x001B3AD0 File Offset: 0x001B1CD0
			public ISP80090Drbg Get(IEntropySource entropySource)
			{
				return new HMacSP800Drbg(this.mHMac, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce);
			}

			// Token: 0x0400362B RID: 13867
			private readonly IMac mHMac;

			// Token: 0x0400362C RID: 13868
			private readonly byte[] mNonce;

			// Token: 0x0400362D RID: 13869
			private readonly byte[] mPersonalizationString;

			// Token: 0x0400362E RID: 13870
			private readonly int mSecurityStrength;
		}

		// Token: 0x02000952 RID: 2386
		private class CtrDrbgProvider : IDrbgProvider
		{
			// Token: 0x06004F0B RID: 20235 RVA: 0x001B3AF0 File Offset: 0x001B1CF0
			public CtrDrbgProvider(IBlockCipher blockCipher, int keySizeInBits, byte[] nonce, byte[] personalizationString, int securityStrength)
			{
				this.mBlockCipher = blockCipher;
				this.mKeySizeInBits = keySizeInBits;
				this.mNonce = nonce;
				this.mPersonalizationString = personalizationString;
				this.mSecurityStrength = securityStrength;
			}

			// Token: 0x06004F0C RID: 20236 RVA: 0x001B3B1D File Offset: 0x001B1D1D
			public ISP80090Drbg Get(IEntropySource entropySource)
			{
				return new CtrSP800Drbg(this.mBlockCipher, this.mKeySizeInBits, this.mSecurityStrength, entropySource, this.mPersonalizationString, this.mNonce);
			}

			// Token: 0x0400362F RID: 13871
			private readonly IBlockCipher mBlockCipher;

			// Token: 0x04003630 RID: 13872
			private readonly int mKeySizeInBits;

			// Token: 0x04003631 RID: 13873
			private readonly byte[] mNonce;

			// Token: 0x04003632 RID: 13874
			private readonly byte[] mPersonalizationString;

			// Token: 0x04003633 RID: 13875
			private readonly int mSecurityStrength;
		}
	}
}
