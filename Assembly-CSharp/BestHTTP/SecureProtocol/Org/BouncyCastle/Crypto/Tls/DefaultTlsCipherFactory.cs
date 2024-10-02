using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000415 RID: 1045
	public class DefaultTlsCipherFactory : AbstractTlsCipherFactory
	{
		// Token: 0x060029C7 RID: 10695 RVA: 0x0010F210 File Offset: 0x0010D410
		public override TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
		{
			switch (encryptionAlgorithm)
			{
			case 0:
				return this.CreateNullCipher(context, macAlgorithm);
			case 1:
			case 3:
			case 4:
			case 5:
			case 6:
				break;
			case 2:
				return this.CreateRC4Cipher(context, 16, macAlgorithm);
			case 7:
				return this.CreateDesEdeCipher(context, macAlgorithm);
			case 8:
				return this.CreateAESCipher(context, 16, macAlgorithm);
			case 9:
				return this.CreateAESCipher(context, 32, macAlgorithm);
			case 10:
				return this.CreateCipher_Aes_Gcm(context, 16, 16);
			case 11:
				return this.CreateCipher_Aes_Gcm(context, 32, 16);
			case 12:
				return this.CreateCamelliaCipher(context, 16, macAlgorithm);
			case 13:
				return this.CreateCamelliaCipher(context, 32, macAlgorithm);
			case 14:
				return this.CreateSeedCipher(context, macAlgorithm);
			case 15:
				return this.CreateCipher_Aes_Ccm(context, 16, 16);
			case 16:
				return this.CreateCipher_Aes_Ccm(context, 16, 8);
			case 17:
				return this.CreateCipher_Aes_Ccm(context, 32, 16);
			case 18:
				return this.CreateCipher_Aes_Ccm(context, 32, 8);
			case 19:
				return this.CreateCipher_Camellia_Gcm(context, 16, 16);
			case 20:
				return this.CreateCipher_Camellia_Gcm(context, 32, 16);
			case 21:
				return this.CreateChaCha20Poly1305(context);
			default:
				if (encryptionAlgorithm == 103)
				{
					return this.CreateCipher_Aes_Ocb(context, 16, 12);
				}
				if (encryptionAlgorithm == 104)
				{
					return this.CreateCipher_Aes_Ocb(context, 32, 12);
				}
				break;
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060029C8 RID: 10696 RVA: 0x0010F361 File Offset: 0x0010D561
		protected virtual TlsBlockCipher CreateAESCipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateAesBlockCipher(), this.CreateAesBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);
		}

		// Token: 0x060029C9 RID: 10697 RVA: 0x0010F384 File Offset: 0x0010D584
		protected virtual TlsBlockCipher CreateCamelliaCipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateCamelliaBlockCipher(), this.CreateCamelliaBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize);
		}

		// Token: 0x060029CA RID: 10698 RVA: 0x0010F3A7 File Offset: 0x0010D5A7
		protected virtual TlsCipher CreateChaCha20Poly1305(TlsContext context)
		{
			return new Chacha20Poly1305(context);
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x0010F3AF File Offset: 0x0010D5AF
		protected virtual TlsAeadCipher CreateCipher_Aes_Ccm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ccm(), this.CreateAeadBlockCipher_Aes_Ccm(), cipherKeySize, macSize);
		}

		// Token: 0x060029CC RID: 10700 RVA: 0x0010F3C5 File Offset: 0x0010D5C5
		protected virtual TlsAeadCipher CreateCipher_Aes_Gcm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Gcm(), this.CreateAeadBlockCipher_Aes_Gcm(), cipherKeySize, macSize);
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x0010F3DB File Offset: 0x0010D5DB
		protected virtual TlsAeadCipher CreateCipher_Aes_Ocb(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Aes_Ocb(), this.CreateAeadBlockCipher_Aes_Ocb(), cipherKeySize, macSize, 2);
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x0010F3F2 File Offset: 0x0010D5F2
		protected virtual TlsAeadCipher CreateCipher_Camellia_Gcm(TlsContext context, int cipherKeySize, int macSize)
		{
			return new TlsAeadCipher(context, this.CreateAeadBlockCipher_Camellia_Gcm(), this.CreateAeadBlockCipher_Camellia_Gcm(), cipherKeySize, macSize);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0010F408 File Offset: 0x0010D608
		protected virtual TlsBlockCipher CreateDesEdeCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateDesEdeBlockCipher(), this.CreateDesEdeBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 24);
		}

		// Token: 0x060029D0 RID: 10704 RVA: 0x0010F42C File Offset: 0x0010D62C
		protected virtual TlsNullCipher CreateNullCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsNullCipher(context, this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm));
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x0010F442 File Offset: 0x0010D642
		protected virtual TlsStreamCipher CreateRC4Cipher(TlsContext context, int cipherKeySize, int macAlgorithm)
		{
			return new TlsStreamCipher(context, this.CreateRC4StreamCipher(), this.CreateRC4StreamCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), cipherKeySize, false);
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x0010F466 File Offset: 0x0010D666
		protected virtual TlsBlockCipher CreateSeedCipher(TlsContext context, int macAlgorithm)
		{
			return new TlsBlockCipher(context, this.CreateSeedBlockCipher(), this.CreateSeedBlockCipher(), this.CreateHMacDigest(macAlgorithm), this.CreateHMacDigest(macAlgorithm), 16);
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x0010F48A File Offset: 0x0010D68A
		protected virtual IBlockCipher CreateAesEngine()
		{
			return new AesEngine();
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x0010F491 File Offset: 0x0010D691
		protected virtual IBlockCipher CreateCamelliaEngine()
		{
			return new CamelliaEngine();
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0010F498 File Offset: 0x0010D698
		protected virtual IBlockCipher CreateAesBlockCipher()
		{
			return new CbcBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0010F4A5 File Offset: 0x0010D6A5
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ccm()
		{
			return new CcmBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x0010F4B2 File Offset: 0x0010D6B2
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Gcm()
		{
			return new GcmBlockCipher(this.CreateAesEngine());
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0010F4BF File Offset: 0x0010D6BF
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Aes_Ocb()
		{
			return new OcbBlockCipher(this.CreateAesEngine(), this.CreateAesEngine());
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x0010F4D2 File Offset: 0x0010D6D2
		protected virtual IAeadBlockCipher CreateAeadBlockCipher_Camellia_Gcm()
		{
			return new GcmBlockCipher(this.CreateCamelliaEngine());
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x0010F4DF File Offset: 0x0010D6DF
		protected virtual IBlockCipher CreateCamelliaBlockCipher()
		{
			return new CbcBlockCipher(this.CreateCamelliaEngine());
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x0010F4EC File Offset: 0x0010D6EC
		protected virtual IBlockCipher CreateDesEdeBlockCipher()
		{
			return new CbcBlockCipher(new DesEdeEngine());
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x0010F4F8 File Offset: 0x0010D6F8
		protected virtual IStreamCipher CreateRC4StreamCipher()
		{
			return new RC4Engine();
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x0010F4FF File Offset: 0x0010D6FF
		protected virtual IBlockCipher CreateSeedBlockCipher()
		{
			return new CbcBlockCipher(new SeedEngine());
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x0010F50C File Offset: 0x0010D70C
		protected virtual IDigest CreateHMacDigest(int macAlgorithm)
		{
			switch (macAlgorithm)
			{
			case 0:
				return null;
			case 1:
				return TlsUtilities.CreateHash(1);
			case 2:
				return TlsUtilities.CreateHash(2);
			case 3:
				return TlsUtilities.CreateHash(4);
			case 4:
				return TlsUtilities.CreateHash(5);
			case 5:
				return TlsUtilities.CreateHash(6);
			default:
				throw new TlsFatalAlert(80);
			}
		}
	}
}
