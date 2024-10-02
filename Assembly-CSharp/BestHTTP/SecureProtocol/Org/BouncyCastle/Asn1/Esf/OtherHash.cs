using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000750 RID: 1872
	public class OtherHash : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x0018A827 File Offset: 0x00188A27
		public static OtherHash GetInstance(object obj)
		{
			if (obj == null || obj is OtherHash)
			{
				return (OtherHash)obj;
			}
			if (obj is Asn1OctetString)
			{
				return new OtherHash((Asn1OctetString)obj);
			}
			return new OtherHash(OtherHashAlgAndValue.GetInstance(obj));
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0018A85A File Offset: 0x00188A5A
		public OtherHash(byte[] sha1Hash)
		{
			if (sha1Hash == null)
			{
				throw new ArgumentNullException("sha1Hash");
			}
			this.sha1Hash = new DerOctetString(sha1Hash);
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x0018A87C File Offset: 0x00188A7C
		public OtherHash(Asn1OctetString sha1Hash)
		{
			if (sha1Hash == null)
			{
				throw new ArgumentNullException("sha1Hash");
			}
			this.sha1Hash = sha1Hash;
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0018A899 File Offset: 0x00188A99
		public OtherHash(OtherHashAlgAndValue otherHash)
		{
			if (otherHash == null)
			{
				throw new ArgumentNullException("otherHash");
			}
			this.otherHash = otherHash;
		}

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06004384 RID: 17284 RVA: 0x0018A8B6 File Offset: 0x00188AB6
		public AlgorithmIdentifier HashAlgorithm
		{
			get
			{
				if (this.otherHash != null)
				{
					return this.otherHash.HashAlgorithm;
				}
				return new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1);
			}
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0018A8D6 File Offset: 0x00188AD6
		public byte[] GetHashValue()
		{
			if (this.otherHash != null)
			{
				return this.otherHash.GetHashValue();
			}
			return this.sha1Hash.GetOctets();
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0018A8F7 File Offset: 0x00188AF7
		public override Asn1Object ToAsn1Object()
		{
			if (this.otherHash != null)
			{
				return this.otherHash.ToAsn1Object();
			}
			return this.sha1Hash;
		}

		// Token: 0x04002C46 RID: 11334
		private readonly Asn1OctetString sha1Hash;

		// Token: 0x04002C47 RID: 11335
		private readonly OtherHashAlgAndValue otherHash;
	}
}
