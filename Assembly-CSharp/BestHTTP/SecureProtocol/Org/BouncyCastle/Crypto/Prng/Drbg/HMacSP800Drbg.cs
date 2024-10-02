using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Prng.Drbg
{
	// Token: 0x020004BE RID: 1214
	public class HMacSP800Drbg : ISP80090Drbg
	{
		// Token: 0x06002F56 RID: 12118 RVA: 0x001253A0 File Offset: 0x001235A0
		public HMacSP800Drbg(IMac hMac, int securityStrength, IEntropySource entropySource, byte[] personalizationString, byte[] nonce)
		{
			if (securityStrength > DrbgUtilities.GetMaxSecurityStrength(hMac))
			{
				throw new ArgumentException("Requested security strength is not supported by the derivation function");
			}
			if (entropySource.EntropySize < securityStrength)
			{
				throw new ArgumentException("Not enough entropy for security strength required");
			}
			this.mHMac = hMac;
			this.mSecurityStrength = securityStrength;
			this.mEntropySource = entropySource;
			byte[] entropy = this.GetEntropy();
			byte[] seedMaterial = Arrays.ConcatenateAll(new byte[][]
			{
				entropy,
				nonce,
				personalizationString
			});
			this.mK = new byte[hMac.GetMacSize()];
			this.mV = new byte[this.mK.Length];
			Arrays.Fill(this.mV, 1);
			this.hmac_DRBG_Update(seedMaterial);
			this.mReseedCounter = 1L;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x00125450 File Offset: 0x00123650
		private void hmac_DRBG_Update(byte[] seedMaterial)
		{
			this.hmac_DRBG_Update_Func(seedMaterial, 0);
			if (seedMaterial != null)
			{
				this.hmac_DRBG_Update_Func(seedMaterial, 1);
			}
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x00125468 File Offset: 0x00123668
		private void hmac_DRBG_Update_Func(byte[] seedMaterial, byte vValue)
		{
			this.mHMac.Init(new KeyParameter(this.mK));
			this.mHMac.BlockUpdate(this.mV, 0, this.mV.Length);
			this.mHMac.Update(vValue);
			if (seedMaterial != null)
			{
				this.mHMac.BlockUpdate(seedMaterial, 0, seedMaterial.Length);
			}
			this.mHMac.DoFinal(this.mK, 0);
			this.mHMac.Init(new KeyParameter(this.mK));
			this.mHMac.BlockUpdate(this.mV, 0, this.mV.Length);
			this.mHMac.DoFinal(this.mV, 0);
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x06002F59 RID: 12121 RVA: 0x0012551A File Offset: 0x0012371A
		public int BlockSize
		{
			get
			{
				return this.mV.Length * 8;
			}
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x00125528 File Offset: 0x00123728
		public int Generate(byte[] output, byte[] additionalInput, bool predictionResistant)
		{
			int num = output.Length * 8;
			if (num > HMacSP800Drbg.MAX_BITS_REQUEST)
			{
				throw new ArgumentException("Number of bits per request limited to " + HMacSP800Drbg.MAX_BITS_REQUEST, "output");
			}
			if (this.mReseedCounter > HMacSP800Drbg.RESEED_MAX)
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
				this.hmac_DRBG_Update(additionalInput);
			}
			byte[] array = new byte[output.Length];
			int num2 = output.Length / this.mV.Length;
			this.mHMac.Init(new KeyParameter(this.mK));
			for (int i = 0; i < num2; i++)
			{
				this.mHMac.BlockUpdate(this.mV, 0, this.mV.Length);
				this.mHMac.DoFinal(this.mV, 0);
				Array.Copy(this.mV, 0, array, i * this.mV.Length, this.mV.Length);
			}
			if (num2 * this.mV.Length < array.Length)
			{
				this.mHMac.BlockUpdate(this.mV, 0, this.mV.Length);
				this.mHMac.DoFinal(this.mV, 0);
				Array.Copy(this.mV, 0, array, num2 * this.mV.Length, array.Length - num2 * this.mV.Length);
			}
			this.hmac_DRBG_Update(additionalInput);
			this.mReseedCounter += 1L;
			Array.Copy(array, 0, output, 0, output.Length);
			return num;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x00125690 File Offset: 0x00123890
		public void Reseed(byte[] additionalInput)
		{
			byte[] seedMaterial = Arrays.Concatenate(this.GetEntropy(), additionalInput);
			this.hmac_DRBG_Update(seedMaterial);
			this.mReseedCounter = 1L;
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x001256B9 File Offset: 0x001238B9
		private byte[] GetEntropy()
		{
			byte[] entropy = this.mEntropySource.GetEntropy();
			if (entropy.Length < (this.mSecurityStrength + 7) / 8)
			{
				throw new InvalidOperationException("Insufficient entropy provided by entropy source");
			}
			return entropy;
		}

		// Token: 0x04001FA0 RID: 8096
		private static readonly long RESEED_MAX = 140737488355328L;

		// Token: 0x04001FA1 RID: 8097
		private static readonly int MAX_BITS_REQUEST = 262144;

		// Token: 0x04001FA2 RID: 8098
		private readonly byte[] mK;

		// Token: 0x04001FA3 RID: 8099
		private readonly byte[] mV;

		// Token: 0x04001FA4 RID: 8100
		private readonly IEntropySource mEntropySource;

		// Token: 0x04001FA5 RID: 8101
		private readonly IMac mHMac;

		// Token: 0x04001FA6 RID: 8102
		private readonly int mSecurityStrength;

		// Token: 0x04001FA7 RID: 8103
		private long mReseedCounter;
	}
}
