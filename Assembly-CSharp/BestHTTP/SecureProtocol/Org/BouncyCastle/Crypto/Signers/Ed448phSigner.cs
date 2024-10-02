using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Rfc8032;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200049A RID: 1178
	public class Ed448phSigner : ISigner
	{
		// Token: 0x06002E42 RID: 11842 RVA: 0x0011FCED File Offset: 0x0011DEED
		public Ed448phSigner(byte[] context)
		{
			this.context = Arrays.Clone(context);
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x0011FD0C File Offset: 0x0011DF0C
		public virtual string AlgorithmName
		{
			get
			{
				return "Ed448ph";
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0011FD14 File Offset: 0x0011DF14
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				this.privateKey = (Ed448PrivateKeyParameters)parameters;
				this.publicKey = this.privateKey.GeneratePublicKey();
			}
			else
			{
				this.privateKey = null;
				this.publicKey = (Ed448PublicKeyParameters)parameters;
			}
			this.Reset();
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x0011FD63 File Offset: 0x0011DF63
		public virtual void Update(byte b)
		{
			this.prehash.Update(b);
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x0011FD71 File Offset: 0x0011DF71
		public virtual void BlockUpdate(byte[] buf, int off, int len)
		{
			this.prehash.BlockUpdate(buf, off, len);
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x0011FD84 File Offset: 0x0011DF84
		public virtual byte[] GenerateSignature()
		{
			if (!this.forSigning || this.privateKey == null)
			{
				throw new InvalidOperationException("Ed448phSigner not initialised for signature generation.");
			}
			byte[] array = new byte[Ed448.PrehashSize];
			if (Ed448.PrehashSize != this.prehash.DoFinal(array, 0, Ed448.PrehashSize))
			{
				throw new InvalidOperationException("Prehash digest failed");
			}
			byte[] array2 = new byte[Ed448PrivateKeyParameters.SignatureSize];
			this.privateKey.Sign(Ed448.Algorithm.Ed448ph, this.publicKey, this.context, array, 0, Ed448.PrehashSize, array2, 0);
			return array2;
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x0011FE08 File Offset: 0x0011E008
		public virtual bool VerifySignature(byte[] signature)
		{
			if (this.forSigning || this.publicKey == null)
			{
				throw new InvalidOperationException("Ed448phSigner not initialised for verification");
			}
			byte[] encoded = this.publicKey.GetEncoded();
			return Ed448.VerifyPrehash(signature, 0, encoded, 0, this.context, this.prehash);
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x0011FE51 File Offset: 0x0011E051
		public void Reset()
		{
			this.prehash.Reset();
		}

		// Token: 0x04001EDE RID: 7902
		private readonly IXof prehash = Ed448.CreatePrehash();

		// Token: 0x04001EDF RID: 7903
		private readonly byte[] context;

		// Token: 0x04001EE0 RID: 7904
		private bool forSigning;

		// Token: 0x04001EE1 RID: 7905
		private Ed448PrivateKeyParameters privateKey;

		// Token: 0x04001EE2 RID: 7906
		private Ed448PublicKeyParameters publicKey;
	}
}
