using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000464 RID: 1124
	public abstract class TlsDsaSigner : AbstractTlsSigner
	{
		// Token: 0x06002BD9 RID: 11225 RVA: 0x00116E78 File Offset: 0x00115078
		public override byte[] GenerateRawSignature(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, true, new ParametersWithRandom(privateKey, this.mContext.SecureRandom));
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.GenerateSignature();
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x00116EC4 File Offset: 0x001150C4
		public override bool VerifyRawSignature(SignatureAndHashAlgorithm algorithm, byte[] sigBytes, AsymmetricKeyParameter publicKey, byte[] hash)
		{
			ISigner signer = this.MakeSigner(algorithm, true, false, publicKey);
			if (algorithm == null)
			{
				signer.BlockUpdate(hash, 16, 20);
			}
			else
			{
				signer.BlockUpdate(hash, 0, hash.Length);
			}
			return signer.VerifySignature(sigBytes);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x00116F01 File Offset: 0x00115101
		public override ISigner CreateSigner(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter privateKey)
		{
			return this.MakeSigner(algorithm, false, true, privateKey);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x00116F0D File Offset: 0x0011510D
		public override ISigner CreateVerifyer(SignatureAndHashAlgorithm algorithm, AsymmetricKeyParameter publicKey)
		{
			return this.MakeSigner(algorithm, false, false, publicKey);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x00116F19 File Offset: 0x00115119
		protected virtual ICipherParameters MakeInitParameters(bool forSigning, ICipherParameters cp)
		{
			return cp;
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00116F1C File Offset: 0x0011511C
		protected virtual ISigner MakeSigner(SignatureAndHashAlgorithm algorithm, bool raw, bool forSigning, ICipherParameters cp)
		{
			if (algorithm != null != TlsUtilities.IsTlsV12(this.mContext))
			{
				throw new InvalidOperationException();
			}
			if (algorithm != null && algorithm.Signature != this.SignatureAlgorithm)
			{
				throw new InvalidOperationException();
			}
			byte hashAlgorithm = (algorithm == null) ? 2 : algorithm.Hash;
			IDigest digest;
			if (!raw)
			{
				digest = TlsUtilities.CreateHash(hashAlgorithm);
			}
			else
			{
				IDigest digest2 = new NullDigest();
				digest = digest2;
			}
			IDigest digest3 = digest;
			DsaDigestSigner dsaDigestSigner = new DsaDigestSigner(this.CreateDsaImpl(hashAlgorithm), digest3);
			((ISigner)dsaDigestSigner).Init(forSigning, this.MakeInitParameters(forSigning, cp));
			return dsaDigestSigner;
		}

		// Token: 0x170005E0 RID: 1504
		// (get) Token: 0x06002BDF RID: 11231
		protected abstract byte SignatureAlgorithm { get; }

		// Token: 0x06002BE0 RID: 11232
		protected abstract IDsa CreateDsaImpl(byte hashAlgorithm);
	}
}
