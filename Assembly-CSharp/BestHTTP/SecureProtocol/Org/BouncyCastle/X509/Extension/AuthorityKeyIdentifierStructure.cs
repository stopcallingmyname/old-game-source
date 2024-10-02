using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Extension
{
	// Token: 0x0200025D RID: 605
	public class AuthorityKeyIdentifierStructure : AuthorityKeyIdentifier
	{
		// Token: 0x06001615 RID: 5653 RVA: 0x000AFC94 File Offset: 0x000ADE94
		public AuthorityKeyIdentifierStructure(Asn1OctetString encodedValue) : base((Asn1Sequence)X509ExtensionUtilities.FromExtensionValue(encodedValue))
		{
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x000AFCA8 File Offset: 0x000ADEA8
		private static Asn1Sequence FromCertificate(X509Certificate certificate)
		{
			Asn1Sequence result;
			try
			{
				GeneralName name = new GeneralName(PrincipalUtilities.GetIssuerX509Principal(certificate));
				if (certificate.Version == 3)
				{
					Asn1OctetString extensionValue = certificate.GetExtensionValue(X509Extensions.SubjectKeyIdentifier);
					if (extensionValue != null)
					{
						return (Asn1Sequence)new AuthorityKeyIdentifier(((Asn1OctetString)X509ExtensionUtilities.FromExtensionValue(extensionValue)).GetOctets(), new GeneralNames(name), certificate.SerialNumber).ToAsn1Object();
					}
				}
				result = (Asn1Sequence)new AuthorityKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(certificate.GetPublicKey()), new GeneralNames(name), certificate.SerialNumber).ToAsn1Object();
			}
			catch (Exception exception)
			{
				throw new CertificateParsingException("Exception extracting certificate details", exception);
			}
			return result;
		}

		// Token: 0x06001617 RID: 5655 RVA: 0x000AFD50 File Offset: 0x000ADF50
		private static Asn1Sequence FromKey(AsymmetricKeyParameter pubKey)
		{
			Asn1Sequence result;
			try
			{
				result = (Asn1Sequence)new AuthorityKeyIdentifier(SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(pubKey)).ToAsn1Object();
			}
			catch (Exception arg)
			{
				throw new InvalidKeyException("can't process key: " + arg);
			}
			return result;
		}

		// Token: 0x06001618 RID: 5656 RVA: 0x000AFD98 File Offset: 0x000ADF98
		public AuthorityKeyIdentifierStructure(X509Certificate certificate) : base(AuthorityKeyIdentifierStructure.FromCertificate(certificate))
		{
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x000AFDA6 File Offset: 0x000ADFA6
		public AuthorityKeyIdentifierStructure(AsymmetricKeyParameter pubKey) : base(AuthorityKeyIdentifierStructure.FromKey(pubKey))
		{
		}
	}
}
