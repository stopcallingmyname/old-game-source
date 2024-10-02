using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000498 RID: 1176
	public class Ed25519phSigner : ISigner
	{
		// Token: 0x06002E32 RID: 11826 RVA: 0x0011FA89 File Offset: 0x0011DC89
		public Ed25519phSigner(byte[] context)
		{
			this.context = Arrays.Clone(context);
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002E33 RID: 11827 RVA: 0x0011FAA8 File Offset: 0x0011DCA8
		public virtual string AlgorithmName
		{
			get
			{
				return "Ed25519ph";
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x0011FAB0 File Offset: 0x0011DCB0
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				this.privateKey = (Ed25519PrivateKeyParameters)parameters;
				this.publicKey = this.privateKey.GeneratePublicKey();
			}
			else
			{
				this.privateKey = null;
				this.publicKey = (Ed25519PublicKeyParameters)parameters;
			}
			this.Reset();
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0011FAFF File Offset: 0x0011DCFF
		public virtual void Update(byte b)
		{
			this.prehash.Update(b);
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x0011FB0D File Offset: 0x0011DD0D
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.prehash.BlockUpdate(buf, off, len);
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x0011FB20 File Offset: 0x0011DD20
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning || this.privateKey == null)
			{
				throw new InvalidOperationException("Ed25519phSigner not initialised for signature generation.");
			}
			byte[] array = new byte[Ed25519.PrehashSize];
			if (Ed25519.PrehashSize != this.prehash.DoFinal(array, 0))
			{
				throw new InvalidOperationException("Prehash digest failed");
			}
			byte[] array2 = new byte[Ed25519PrivateKeyParameters.SignatureSize];
			this.privateKey.Sign(Ed25519.Algorithm.Ed25519ph, this.publicKey, this.context, array, 0, Ed25519.PrehashSize, array2, 0);
			return array2;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x0011FBA0 File Offset: 0x0011DDA0
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning || this.publicKey == null)
			{
				throw new InvalidOperationException("Ed25519phSigner not initialised for verification");
			}
			byte[] encoded = this.publicKey.GetEncoded();
			return Ed25519.VerifyPrehash(signature, 0, encoded, 0, this.context, this.prehash);
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x0011FBE9 File Offset: 0x0011DDE9
		public void Reset()
		{
			this.prehash.Reset();
		}

		// Token: 0x04001ED5 RID: 7893
		private readonly IDigest prehash = Ed25519.CreatePrehash();

		// Token: 0x04001ED6 RID: 7894
		private readonly byte[] context;

		// Token: 0x04001ED7 RID: 7895
		private bool forSigning;

		// Token: 0x04001ED8 RID: 7896
		private Ed25519PrivateKeyParameters privateKey;

		// Token: 0x04001ED9 RID: 7897
		private Ed25519PublicKeyParameters publicKey;
	}
}
