using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000556 RID: 1366
	public class HkdfBytesGenerator : IDerivationFunction
	{
		// Token: 0x06003378 RID: 13176 RVA: 0x001352BD File Offset: 0x001334BD
		public HkdfBytesGenerator(IDigest hash)
		{
			this.hMacHash = new HMac(hash);
			this.hashLen = hash.GetDigestSize();
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x001352E0 File Offset: 0x001334E0
		public virtual void Init(IDerivationParameters parameters)
		{
			if (!(parameters is HkdfParameters))
			{
				throw new ArgumentException("HKDF parameters required for HkdfBytesGenerator", "parameters");
			}
			HkdfParameters hkdfParameters = (HkdfParameters)parameters;
			if (hkdfParameters.SkipExtract)
			{
				this.hMacHash.Init(new KeyParameter(hkdfParameters.GetIkm()));
			}
			else
			{
				this.hMacHash.Init(this.Extract(hkdfParameters.GetSalt(), hkdfParameters.GetIkm()));
			}
			this.info = hkdfParameters.GetInfo();
			this.generatedBytes = 0;
			this.currentT = new byte[this.hashLen];
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x00135370 File Offset: 0x00133570
		private KeyParameter Extract(byte[] salt, byte[] ikm)
		{
			if (salt == null)
			{
				this.hMacHash.Init(new KeyParameter(new byte[this.hashLen]));
			}
			else
			{
				this.hMacHash.Init(new KeyParameter(salt));
			}
			this.hMacHash.BlockUpdate(ikm, 0, ikm.Length);
			byte[] array = new byte[this.hashLen];
			this.hMacHash.DoFinal(array, 0);
			return new KeyParameter(array);
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x001353E0 File Offset: 0x001335E0
		private void ExpandNext()
		{
			int num = this.generatedBytes / this.hashLen + 1;
			if (num >= 256)
			{
				throw new DataLengthException("HKDF cannot generate more than 255 blocks of HashLen size");
			}
			if (this.generatedBytes != 0)
			{
				this.hMacHash.BlockUpdate(this.currentT, 0, this.hashLen);
			}
			this.hMacHash.BlockUpdate(this.info, 0, this.info.Length);
			this.hMacHash.Update((byte)num);
			this.hMacHash.DoFinal(this.currentT, 0);
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x0600337C RID: 13180 RVA: 0x0013546A File Offset: 0x0013366A
		public virtual IDigest Digest
		{
			get
			{
				return this.hMacHash.GetUnderlyingDigest();
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x00135478 File Offset: 0x00133678
		public virtual int GenerateBytes(byte[] output, int outOff, int len)
		{
			if (this.generatedBytes + len > 255 * this.hashLen)
			{
				throw new DataLengthException("HKDF may only be used for 255 * HashLen bytes of output");
			}
			if (this.generatedBytes % this.hashLen == 0)
			{
				this.ExpandNext();
			}
			int sourceIndex = this.generatedBytes % this.hashLen;
			int num = Math.Min(this.hashLen - this.generatedBytes % this.hashLen, len);
			Array.Copy(this.currentT, sourceIndex, output, outOff, num);
			this.generatedBytes += num;
			int i = len - num;
			outOff += num;
			while (i > 0)
			{
				this.ExpandNext();
				num = Math.Min(this.hashLen, i);
				Array.Copy(this.currentT, 0, output, outOff, num);
				this.generatedBytes += num;
				i -= num;
				outOff += num;
			}
			return len;
		}

		// Token: 0x040021B1 RID: 8625
		private HMac hMacHash;

		// Token: 0x040021B2 RID: 8626
		private int hashLen;

		// Token: 0x040021B3 RID: 8627
		private byte[] info;

		// Token: 0x040021B4 RID: 8628
		private byte[] currentT;

		// Token: 0x040021B5 RID: 8629
		private int generatedBytes;
	}
}
