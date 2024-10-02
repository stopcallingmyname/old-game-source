using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x020002F5 RID: 757
	public class CertificateID
	{
		// Token: 0x06001B97 RID: 7063 RVA: 0x000D0868 File Offset: 0x000CEA68
		public CertificateID(CertID id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.id = id;
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x000D0888 File Offset: 0x000CEA88
		public CertificateID(string hashAlgorithm, X509Certificate issuerCert, BigInteger serialNumber)
		{
			AlgorithmIdentifier hashAlg = new AlgorithmIdentifier(new DerObjectIdentifier(hashAlgorithm), DerNull.Instance);
			this.id = CertificateID.CreateCertID(hashAlg, issuerCert, new DerInteger(serialNumber));
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001B99 RID: 7065 RVA: 0x000D08BF File Offset: 0x000CEABF
		public string HashAlgOid
		{
			get
			{
				return this.id.HashAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x000D08D6 File Offset: 0x000CEAD6
		public byte[] GetIssuerNameHash()
		{
			return this.id.IssuerNameHash.GetOctets();
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x000D08E8 File Offset: 0x000CEAE8
		public byte[] GetIssuerKeyHash()
		{
			return this.id.IssuerKeyHash.GetOctets();
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000D08FA File Offset: 0x000CEAFA
		public BigInteger SerialNumber
		{
			get
			{
				return this.id.SerialNumber.Value;
			}
		}

		// Token: 0x06001B9D RID: 7069 RVA: 0x000D090C File Offset: 0x000CEB0C
		public bool MatchesIssuer(X509Certificate issuerCert)
		{
			return CertificateID.CreateCertID(this.id.HashAlgorithm, issuerCert, this.id.SerialNumber).Equals(this.id);
		}

		// Token: 0x06001B9E RID: 7070 RVA: 0x000D0935 File Offset: 0x000CEB35
		public CertID ToAsn1Object()
		{
			return this.id;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x000D0940 File Offset: 0x000CEB40
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			CertificateID certificateID = obj as CertificateID;
			return certificateID != null && this.id.ToAsn1Object().Equals(certificateID.id.ToAsn1Object());
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000D097A File Offset: 0x000CEB7A
		public override int GetHashCode()
		{
			return this.id.ToAsn1Object().GetHashCode();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000D098C File Offset: 0x000CEB8C
		public static CertificateID DeriveCertificateID(CertificateID original, BigInteger newSerialNumber)
		{
			return new CertificateID(new CertID(original.id.HashAlgorithm, original.id.IssuerNameHash, original.id.IssuerKeyHash, new DerInteger(newSerialNumber)));
		}

		// Token: 0x06001BA2 RID: 7074 RVA: 0x000D09C0 File Offset: 0x000CEBC0
		private static CertID CreateCertID(AlgorithmIdentifier hashAlg, X509Certificate issuerCert, DerInteger serialNumber)
		{
			CertID result;
			try
			{
				string algorithm = hashAlg.Algorithm.Id;
				X509Name subjectX509Principal = PrincipalUtilities.GetSubjectX509Principal(issuerCert);
				byte[] str = DigestUtilities.CalculateDigest(algorithm, subjectX509Principal.GetEncoded());
				SubjectPublicKeyInfo subjectPublicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(issuerCert.GetPublicKey());
				byte[] str2 = DigestUtilities.CalculateDigest(algorithm, subjectPublicKeyInfo.PublicKeyData.GetBytes());
				result = new CertID(hashAlg, new DerOctetString(str), new DerOctetString(str2), serialNumber);
			}
			catch (Exception ex)
			{
				throw new OcspException("problem creating ID: " + ex, ex);
			}
			return result;
		}

		// Token: 0x040018FA RID: 6394
		public const string HashSha1 = "1.3.14.3.2.26";

		// Token: 0x040018FB RID: 6395
		private readonly CertID id;
	}
}
