using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057C RID: 1404
	public class IesEngine
	{
		// Token: 0x060034F3 RID: 13555 RVA: 0x00143A38 File Offset: 0x00141C38
		public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac)
		{
			this.agree = agree;
			this.kdf = kdf;
			this.mac = mac;
			this.macBuf = new byte[mac.GetMacSize()];
		}

		// Token: 0x060034F4 RID: 13556 RVA: 0x00143A66 File Offset: 0x00141C66
		public IesEngine(IBasicAgreement agree, IDerivationFunction kdf, IMac mac, BufferedBlockCipher cipher)
		{
			this.agree = agree;
			this.kdf = kdf;
			this.mac = mac;
			this.macBuf = new byte[mac.GetMacSize()];
			this.cipher = cipher;
		}

		// Token: 0x060034F5 RID: 13557 RVA: 0x00143A9C File Offset: 0x00141C9C
		public virtual void Init(bool forEncryption, ICipherParameters privParameters, ICipherParameters pubParameters, ICipherParameters iesParameters)
		{
			this.forEncryption = forEncryption;
			this.privParam = privParameters;
			this.pubParam = pubParameters;
			this.param = (IesParameters)iesParameters;
		}

		// Token: 0x060034F6 RID: 13558 RVA: 0x00143AC0 File Offset: 0x00141CC0
		private byte[] DecryptBlock(byte[] in_enc, int inOff, int inLen, byte[] z)
		{
			KdfParameters kdfParameters = new KdfParameters(z, this.param.GetDerivationV());
			int macKeySize = this.param.MacKeySize;
			this.kdf.Init(kdfParameters);
			if (inLen < this.mac.GetMacSize())
			{
				throw new InvalidCipherTextException("Length of input must be greater than the MAC");
			}
			inLen -= this.mac.GetMacSize();
			byte[] array2;
			KeyParameter parameters;
			if (this.cipher == null)
			{
				byte[] array = this.GenerateKdfBytes(kdfParameters, inLen + macKeySize / 8);
				array2 = new byte[inLen];
				for (int num = 0; num != inLen; num++)
				{
					array2[num] = (in_enc[inOff + num] ^ array[num]);
				}
				parameters = new KeyParameter(array, inLen, macKeySize / 8);
			}
			else
			{
				int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
				byte[] key = this.GenerateKdfBytes(kdfParameters, cipherKeySize / 8 + macKeySize / 8);
				this.cipher.Init(false, new KeyParameter(key, 0, cipherKeySize / 8));
				array2 = this.cipher.DoFinal(in_enc, inOff, inLen);
				parameters = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
			}
			byte[] encodingV = this.param.GetEncodingV();
			this.mac.Init(parameters);
			this.mac.BlockUpdate(in_enc, inOff, inLen);
			this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
			this.mac.DoFinal(this.macBuf, 0);
			inOff += inLen;
			if (!Arrays.ConstantTimeAreEqual(Arrays.CopyOfRange(in_enc, inOff, inOff + this.macBuf.Length), this.macBuf))
			{
				throw new InvalidCipherTextException("Invalid MAC.");
			}
			return array2;
		}

		// Token: 0x060034F7 RID: 13559 RVA: 0x00143C44 File Offset: 0x00141E44
		private byte[] EncryptBlock(byte[] input, int inOff, int inLen, byte[] z)
		{
			KdfParameters kParam = new KdfParameters(z, this.param.GetDerivationV());
			int macKeySize = this.param.MacKeySize;
			byte[] array2;
			int num;
			KeyParameter parameters;
			if (this.cipher == null)
			{
				byte[] array = this.GenerateKdfBytes(kParam, inLen + macKeySize / 8);
				array2 = new byte[inLen + this.mac.GetMacSize()];
				num = inLen;
				for (int num2 = 0; num2 != inLen; num2++)
				{
					array2[num2] = (input[inOff + num2] ^ array[num2]);
				}
				parameters = new KeyParameter(array, inLen, macKeySize / 8);
			}
			else
			{
				int cipherKeySize = ((IesWithCipherParameters)this.param).CipherKeySize;
				byte[] key = this.GenerateKdfBytes(kParam, cipherKeySize / 8 + macKeySize / 8);
				this.cipher.Init(true, new KeyParameter(key, 0, cipherKeySize / 8));
				num = this.cipher.GetOutputSize(inLen);
				byte[] array3 = new byte[num];
				int num3 = this.cipher.ProcessBytes(input, inOff, inLen, array3, 0);
				num3 += this.cipher.DoFinal(array3, num3);
				array2 = new byte[num3 + this.mac.GetMacSize()];
				num = num3;
				Array.Copy(array3, 0, array2, 0, num3);
				parameters = new KeyParameter(key, cipherKeySize / 8, macKeySize / 8);
			}
			byte[] encodingV = this.param.GetEncodingV();
			this.mac.Init(parameters);
			this.mac.BlockUpdate(array2, 0, num);
			this.mac.BlockUpdate(encodingV, 0, encodingV.Length);
			this.mac.DoFinal(array2, num);
			return array2;
		}

		// Token: 0x060034F8 RID: 13560 RVA: 0x00143DCC File Offset: 0x00141FCC
		private byte[] GenerateKdfBytes(KdfParameters kParam, int length)
		{
			byte[] array = new byte[length];
			this.kdf.Init(kParam);
			this.kdf.GenerateBytes(array, 0, array.Length);
			return array;
		}

		// Token: 0x060034F9 RID: 13561 RVA: 0x00143E00 File Offset: 0x00142000
		public virtual byte[] ProcessBlock(byte[] input, int inOff, int inLen)
		{
			this.agree.Init(this.privParam);
			BigInteger n = this.agree.CalculateAgreement(this.pubParam);
			byte[] array = BigIntegers.AsUnsignedByteArray(this.agree.GetFieldSize(), n);
			byte[] result;
			try
			{
				result = (this.forEncryption ? this.EncryptBlock(input, inOff, inLen, array) : this.DecryptBlock(input, inOff, inLen, array));
			}
			finally
			{
				Array.Clear(array, 0, array.Length);
			}
			return result;
		}

		// Token: 0x040022A5 RID: 8869
		private readonly IBasicAgreement agree;

		// Token: 0x040022A6 RID: 8870
		private readonly IDerivationFunction kdf;

		// Token: 0x040022A7 RID: 8871
		private readonly IMac mac;

		// Token: 0x040022A8 RID: 8872
		private readonly BufferedBlockCipher cipher;

		// Token: 0x040022A9 RID: 8873
		private readonly byte[] macBuf;

		// Token: 0x040022AA RID: 8874
		private bool forEncryption;

		// Token: 0x040022AB RID: 8875
		private ICipherParameters privParam;

		// Token: 0x040022AC RID: 8876
		private ICipherParameters pubParam;

		// Token: 0x040022AD RID: 8877
		private IesParameters param;
	}
}
