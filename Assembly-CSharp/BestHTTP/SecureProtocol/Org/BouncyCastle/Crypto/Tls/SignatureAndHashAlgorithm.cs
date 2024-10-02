using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000449 RID: 1097
	public class SignatureAndHashAlgorithm
	{
		// Token: 0x06002B2F RID: 11055 RVA: 0x00114490 File Offset: 0x00112690
		public SignatureAndHashAlgorithm(byte hash, byte signature)
		{
			if (!TlsUtilities.IsValidUint8((int)hash))
			{
				throw new ArgumentException("should be a uint8", "hash");
			}
			if (!TlsUtilities.IsValidUint8((int)signature))
			{
				throw new ArgumentException("should be a uint8", "signature");
			}
			if (signature == 0)
			{
				throw new ArgumentException("MUST NOT be \"anonymous\"", "signature");
			}
			this.mHash = hash;
			this.mSignature = signature;
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06002B30 RID: 11056 RVA: 0x001144F4 File Offset: 0x001126F4
		public virtual byte Hash
		{
			get
			{
				return this.mHash;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06002B31 RID: 11057 RVA: 0x001144FC File Offset: 0x001126FC
		public virtual byte Signature
		{
			get
			{
				return this.mSignature;
			}
		}

		// Token: 0x06002B32 RID: 11058 RVA: 0x00114504 File Offset: 0x00112704
		public override bool Equals(object obj)
		{
			if (!(obj is SignatureAndHashAlgorithm))
			{
				return false;
			}
			SignatureAndHashAlgorithm signatureAndHashAlgorithm = (SignatureAndHashAlgorithm)obj;
			return signatureAndHashAlgorithm.Hash == this.Hash && signatureAndHashAlgorithm.Signature == this.Signature;
		}

		// Token: 0x06002B33 RID: 11059 RVA: 0x00114540 File Offset: 0x00112740
		public override int GetHashCode()
		{
			return (int)this.Hash << 16 | (int)this.Signature;
		}

		// Token: 0x06002B34 RID: 11060 RVA: 0x00114552 File Offset: 0x00112752
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.Hash, output);
			TlsUtilities.WriteUint8(this.Signature, output);
		}

		// Token: 0x06002B35 RID: 11061 RVA: 0x0011456C File Offset: 0x0011276C
		public static SignatureAndHashAlgorithm Parse(Stream input)
		{
			byte hash = TlsUtilities.ReadUint8(input);
			byte signature = TlsUtilities.ReadUint8(input);
			return new SignatureAndHashAlgorithm(hash, signature);
		}

		// Token: 0x04001DE9 RID: 7657
		protected readonly byte mHash;

		// Token: 0x04001DEA RID: 7658
		protected readonly byte mSignature;
	}
}
