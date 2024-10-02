using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Nist;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A8 RID: 680
	public class TimeStampToken
	{
		// Token: 0x060018CB RID: 6347 RVA: 0x000BA964 File Offset: 0x000B8B64
		public TimeStampToken(BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.ContentInfo contentInfo) : this(new CmsSignedData(contentInfo))
		{
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x000BA974 File Offset: 0x000B8B74
		public TimeStampToken(CmsSignedData signedData)
		{
			this.tsToken = signedData;
			if (!this.tsToken.SignedContentType.Equals(PkcsObjectIdentifiers.IdCTTstInfo))
			{
				throw new TspValidationException("ContentInfo object not for a time stamp.");
			}
			ICollection signers = this.tsToken.GetSignerInfos().GetSigners();
			if (signers.Count != 1)
			{
				throw new ArgumentException("Time-stamp token signed by " + signers.Count + " signers, but it must contain just the TSA signature.");
			}
			IEnumerator enumerator = signers.GetEnumerator();
			enumerator.MoveNext();
			this.tsaSignerInfo = (SignerInformation)enumerator.Current;
			try
			{
				CmsProcessable signedContent = this.tsToken.SignedContent;
				MemoryStream memoryStream = new MemoryStream();
				signedContent.Write(memoryStream);
				this.tstInfo = new TimeStampTokenInfo(TstInfo.GetInstance(Asn1Object.FromByteArray(memoryStream.ToArray())));
				Attribute attribute = this.tsaSignerInfo.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificate];
				if (attribute != null)
				{
					SigningCertificate instance = SigningCertificate.GetInstance(attribute.AttrValues[0]);
					this.certID = new TimeStampToken.CertID(EssCertID.GetInstance(instance.GetCerts()[0]));
				}
				else
				{
					attribute = this.tsaSignerInfo.SignedAttributes[PkcsObjectIdentifiers.IdAASigningCertificateV2];
					if (attribute == null)
					{
						throw new TspValidationException("no signing certificate attribute found, time stamp invalid.");
					}
					SigningCertificateV2 instance2 = SigningCertificateV2.GetInstance(attribute.AttrValues[0]);
					this.certID = new TimeStampToken.CertID(EssCertIDv2.GetInstance(instance2.GetCerts()[0]));
				}
			}
			catch (CmsException ex)
			{
				throw new TspException(ex.Message, ex.InnerException);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x000BAAFC File Offset: 0x000B8CFC
		public TimeStampTokenInfo TimeStampInfo
		{
			get
			{
				return this.tstInfo;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x000BAB04 File Offset: 0x000B8D04
		public SignerID SignerID
		{
			get
			{
				return this.tsaSignerInfo.SignerID;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x000BAB11 File Offset: 0x000B8D11
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable SignedAttributes
		{
			get
			{
				return this.tsaSignerInfo.SignedAttributes;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x000BAB1E File Offset: 0x000B8D1E
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable UnsignedAttributes
		{
			get
			{
				return this.tsaSignerInfo.UnsignedAttributes;
			}
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x000BAB2B File Offset: 0x000B8D2B
		public IX509Store GetCertificates(string type)
		{
			return this.tsToken.GetCertificates(type);
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x000BAB39 File Offset: 0x000B8D39
		public IX509Store GetCrls(string type)
		{
			return this.tsToken.GetCrls(type);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000BAB47 File Offset: 0x000B8D47
		public IX509Store GetAttributeCertificates(string type)
		{
			return this.tsToken.GetAttributeCertificates(type);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x000BAB58 File Offset: 0x000B8D58
		public void Validate(X509Certificate cert)
		{
			try
			{
				byte[] b = DigestUtilities.CalculateDigest(this.certID.GetHashAlgorithmName(), cert.GetEncoded());
				if (!Arrays.ConstantTimeAreEqual(this.certID.GetCertHash(), b))
				{
					throw new TspValidationException("certificate hash does not match certID hash.");
				}
				if (this.certID.IssuerSerial != null)
				{
					if (!this.certID.IssuerSerial.Serial.Value.Equals(cert.SerialNumber))
					{
						throw new TspValidationException("certificate serial number does not match certID for signature.");
					}
					GeneralName[] names = this.certID.IssuerSerial.Issuer.GetNames();
					X509Name issuerX509Principal = PrincipalUtilities.GetIssuerX509Principal(cert);
					bool flag = false;
					for (int num = 0; num != names.Length; num++)
					{
						if (names[num].TagNo == 4 && X509Name.GetInstance(names[num].Name).Equivalent(issuerX509Principal))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						throw new TspValidationException("certificate name does not match certID for signature. ");
					}
				}
				TspUtil.ValidateCertificate(cert);
				cert.CheckValidity(this.tstInfo.GenTime);
				if (!this.tsaSignerInfo.Verify(cert))
				{
					throw new TspValidationException("signature not created by certificate.");
				}
			}
			catch (CmsException ex)
			{
				if (ex.InnerException != null)
				{
					throw new TspException(ex.Message, ex.InnerException);
				}
				throw new TspException("CMS exception: " + ex, ex);
			}
			catch (CertificateEncodingException ex2)
			{
				throw new TspException("problem processing certificate: " + ex2, ex2);
			}
			catch (SecurityUtilityException ex3)
			{
				throw new TspException("cannot find algorithm: " + ex3.Message, ex3);
			}
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x000BAD20 File Offset: 0x000B8F20
		public CmsSignedData ToCmsSignedData()
		{
			return this.tsToken;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x000BAD28 File Offset: 0x000B8F28
		public byte[] GetEncoded()
		{
			return this.tsToken.GetEncoded();
		}

		// Token: 0x04001845 RID: 6213
		private readonly CmsSignedData tsToken;

		// Token: 0x04001846 RID: 6214
		private readonly SignerInformation tsaSignerInfo;

		// Token: 0x04001847 RID: 6215
		private readonly TimeStampTokenInfo tstInfo;

		// Token: 0x04001848 RID: 6216
		private readonly TimeStampToken.CertID certID;

		// Token: 0x020008FE RID: 2302
		private class CertID
		{
			// Token: 0x06004E1E RID: 19998 RVA: 0x001B0C8A File Offset: 0x001AEE8A
			internal CertID(EssCertID certID)
			{
				this.certID = certID;
				this.certIDv2 = null;
			}

			// Token: 0x06004E1F RID: 19999 RVA: 0x001B0CA0 File Offset: 0x001AEEA0
			internal CertID(EssCertIDv2 certID)
			{
				this.certIDv2 = certID;
				this.certID = null;
			}

			// Token: 0x06004E20 RID: 20000 RVA: 0x001B0CB8 File Offset: 0x001AEEB8
			public string GetHashAlgorithmName()
			{
				if (this.certID != null)
				{
					return "SHA-1";
				}
				if (NistObjectIdentifiers.IdSha256.Equals(this.certIDv2.HashAlgorithm.Algorithm))
				{
					return "SHA-256";
				}
				return this.certIDv2.HashAlgorithm.Algorithm.Id;
			}

			// Token: 0x06004E21 RID: 20001 RVA: 0x001B0D0A File Offset: 0x001AEF0A
			public AlgorithmIdentifier GetHashAlgorithm()
			{
				if (this.certID == null)
				{
					return this.certIDv2.HashAlgorithm;
				}
				return new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1);
			}

			// Token: 0x06004E22 RID: 20002 RVA: 0x001B0D2A File Offset: 0x001AEF2A
			public byte[] GetCertHash()
			{
				if (this.certID == null)
				{
					return this.certIDv2.GetCertHash();
				}
				return this.certID.GetCertHash();
			}

			// Token: 0x17000C25 RID: 3109
			// (get) Token: 0x06004E23 RID: 20003 RVA: 0x001B0D4B File Offset: 0x001AEF4B
			public IssuerSerial IssuerSerial
			{
				get
				{
					if (this.certID == null)
					{
						return this.certIDv2.IssuerSerial;
					}
					return this.certID.IssuerSerial;
				}
			}

			// Token: 0x040034AF RID: 13487
			private EssCertID certID;

			// Token: 0x040034B0 RID: 13488
			private EssCertIDv2 certIDv2;
		}
	}
}
