using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Encodings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200047C RID: 1148
	public class TlsRsaSigner : AbstractTlsSigner
	{
		// Token: 0x06002CED RID: 11501 RVA: 0x0011A3FF File Offset: 0x001185FF
		public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
			signer.BlockUpdate(hash, 0, hash.Length);
			return signer.GenerateSignature();
		}

		// Token: 0x06002CEE RID: 11502 RVA: 0x0011A42B File Offset: 0x0011862B
		public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, false, publicKey);
			signer.BlockUpdate(hash, 0, hash.Length);
			return signer.VerifySignature(sigBytes);
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x0011A44A File Offset: 0x0011864A
		public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
		{
			return this.MakeSigner(algorithm, false, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x0011A466 File Offset: 0x00118666
		public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
		{
			return this.MakeSigner(algorithm, false, false, publicKey);
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x0011A472 File Offset: 0x00118672
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is RsaKeyParameters && !publicKey.IsPrivate;
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0011A488 File Offset: 0x00118688
		protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
		{
			if (algorithm != null != TlsUtilities.IsTlsV12(this.mContext))
			{
				throw new InvalidOperationException();
			}
			if (algorithm != null && algorithm.Signature != 1)
			{
				throw new InvalidOperationException();
			}
			IDigest digest;
			if (raw)
			{
				digest = new NullDigest();
			}
			else if (algorithm == null)
			{
				digest = new CombinedHash();
			}
			else
			{
				digest = TlsUtilities.CreateHash(algorithm.Hash);
			}
			ISigner signer;
			if (algorithm != null)
			{
				signer = new RsaDigestSigner(digest, TlsUtilities.GetOidForHashAlgorithm(algorithm.Hash));
			}
			else
			{
				signer = new GenericSigner(this.CreateRsaImpl(), digest);
			}
			signer.Init(forSigning, cp);
			return signer;
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x0011A50E File Offset: 0x0011870E
		protected virtual IAsymmetricBlockCipher CreateRsaImpl()
		{
			return new Pkcs1Encoding(new RsaBlindedEngine());
		}
	}
}
