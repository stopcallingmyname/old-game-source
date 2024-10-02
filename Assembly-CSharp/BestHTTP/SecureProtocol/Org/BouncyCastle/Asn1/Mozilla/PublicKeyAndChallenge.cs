using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Mozilla
{
	// Token: 0x0200071B RID: 1819
	public class PublicKeyAndChallenge : Asn1Encodable
	{
		// Token: 0x0600424C RID: 16972 RVA: 0x00185B13 File Offset: 0x00183D13
		public static PublicKeyAndChallenge GetInstance(object obj)
		{
			if (obj is PublicKeyAndChallenge)
			{
				return (PublicKeyAndChallenge)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PublicKeyAndChallenge((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in 'PublicKeyAndChallenge' factory : " + Platform.GetTypeName(obj) + ".");
		}

		// Token: 0x0600424D RID: 16973 RVA: 0x00185B52 File Offset: 0x00183D52
		public PublicKeyAndChallenge(Asn1Sequence seq)
		{
			this.pkacSeq = seq;
			this.spki = SubjectPublicKeyInfo.GetInstance(seq[0]);
			this.challenge = DerIA5String.GetInstance(seq[1]);
		}

		// Token: 0x0600424E RID: 16974 RVA: 0x00185B85 File Offset: 0x00183D85
		public override Asn1Object ToAsn1Object()
		{
			return this.pkacSeq;
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x0600424F RID: 16975 RVA: 0x00185B8D File Offset: 0x00183D8D
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.spki;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06004250 RID: 16976 RVA: 0x00185B95 File Offset: 0x00183D95
		public DerIA5String Challenge
		{
			get
			{
				return this.challenge;
			}
		}

		// Token: 0x04002B13 RID: 11027
		private Asn1Sequence pkacSeq;

		// Token: 0x04002B14 RID: 11028
		private SubjectPublicKeyInfo spki;

		// Token: 0x04002B15 RID: 11029
		private DerIA5String challenge;
	}
}
