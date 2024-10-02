using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000535 RID: 1333
	public class GMac : IMac
	{
		// Token: 0x06003274 RID: 12916 RVA: 0x0012FFA1 File Offset: 0x0012E1A1
		public GMac(GcmBlockCipher cipher) : this(cipher, 128)
		{
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x0012FFAF File Offset: 0x0012E1AF
		public GMac(GcmBlockCipher cipher, int macSizeBits)
		{
			this.cipher = cipher;
			this.macSizeBits = macSizeBits;
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x0012FFC8 File Offset: 0x0012E1C8
		public void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				KeyParameter key = (KeyParameter)parametersWithIV.Parameters;
				this.cipher.Init(true, new AeadParameters(key, this.macSizeBits, iv));
				return;
			}
			throw new ArgumentException("GMAC requires ParametersWithIV");
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x06003277 RID: 12919 RVA: 0x00130019 File Offset: 0x0012E219
		public string AlgorithmName
		{
			get
			{
				return this.cipher.GetUnderlyingCipher().AlgorithmName + "-GMAC";
			}
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x00130035 File Offset: 0x0012E235
		public int GetMacSize()
		{
			return this.macSizeBits / 8;
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x0013003F File Offset: 0x0012E23F
		public void Update(byte input)
		{
			this.cipher.ProcessAadByte(input);
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x0013004D File Offset: 0x0012E24D
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.cipher.ProcessAadBytes(input, inOff, len);
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00130060 File Offset: 0x0012E260
		public int DoFinal(byte[] output, int outOff)
		{
			int result;
			try
			{
				result = this.cipher.DoFinal(output, outOff);
			}
			catch (InvalidCipherTextException ex)
			{
				throw new InvalidOperationException(ex.ToString());
			}
			return result;
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x0013009C File Offset: 0x0012E29C
		public void Reset()
		{
			this.cipher.Reset();
		}

		// Token: 0x0400211B RID: 8475
		private readonly GcmBlockCipher cipher;

		// Token: 0x0400211C RID: 8476
		private readonly int macSizeBits;
	}
}
