using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ess;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A9 RID: 681
	public class TimeStampTokenGenerator
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x000BAD35 File Offset: 0x000B8F35
		public TimeStampTokenGenerator(AsymmetricKeyParameter key, X509Certificate cert, string digestOID, string tsaPolicyOID) : this(key, cert, digestOID, tsaPolicyOID, null, null)
		{
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x000BAD44 File Offset: 0x000B8F44
		public TimeStampTokenGenerator(AsymmetricKeyParameter key, X509Certificate cert, string digestOID, string tsaPolicyOID, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr, BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr)
		{
			this.key = key;
			this.cert = cert;
			this.digestOID = digestOID;
			this.tsaPolicyOID = tsaPolicyOID;
			this.unsignedAttr = unsignedAttr;
			TspUtil.ValidateCertificate(cert);
			IDictionary dictionary;
			if (signedAttr != null)
			{
				dictionary = signedAttr.ToDictionary();
			}
			else
			{
				dictionary = Platform.CreateHashtable();
			}
			try
			{
				EssCertID essCertID = new EssCertID(DigestUtilities.CalculateDigest("SHA-1", cert.GetEncoded()));
				Attribute attribute = new Attribute(PkcsObjectIdentifiers.IdAASigningCertificate, new DerSet(new SigningCertificate(essCertID)));
				dictionary[attribute.AttrType] = attribute;
			}
			catch (CertificateEncodingException e)
			{
				throw new TspException("Exception processing certificate.", e);
			}
			catch (SecurityUtilityException e2)
			{
				throw new TspException("Can't find a SHA-1 implementation.", e2);
			}
			this.signedAttr = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(dictionary);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x000BAE2C File Offset: 0x000B902C
		public void SetCertificates(IX509Store certificates)
		{
			this.x509Certs = certificates;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x000BAE35 File Offset: 0x000B9035
		public void SetCrls(IX509Store crls)
		{
			this.x509Crls = crls;
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x000BAE3E File Offset: 0x000B903E
		public void SetAccuracySeconds(int accuracySeconds)
		{
			this.accuracySeconds = accuracySeconds;
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x000BAE47 File Offset: 0x000B9047
		public void SetAccuracyMillis(int accuracyMillis)
		{
			this.accuracyMillis = accuracyMillis;
		}

		// Token: 0x060018DD RID: 6365 RVA: 0x000BAE50 File Offset: 0x000B9050
		public void SetAccuracyMicros(int accuracyMicros)
		{
			this.accuracyMicros = accuracyMicros;
		}

		// Token: 0x060018DE RID: 6366 RVA: 0x000BAE59 File Offset: 0x000B9059
		public void SetOrdering(bool ordering)
		{
			this.ordering = ordering;
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x000BAE62 File Offset: 0x000B9062
		public void SetTsa(GeneralName tsa)
		{
			this.tsa = tsa;
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x000BAE6C File Offset: 0x000B906C
		public TimeStampToken Generate(TimeStampRequest request, BigInteger serialNumber, DateTime genTime)
		{
			MessageImprint messageImprint = new MessageImprint(new AlgorithmIdentifier(new DerObjectIdentifier(request.MessageImprintAlgOid), DerNull.Instance), request.GetMessageImprintDigest());
			Accuracy accuracy = null;
			if (this.accuracySeconds > 0 || this.accuracyMillis > 0 || this.accuracyMicros > 0)
			{
				DerInteger seconds = null;
				if (this.accuracySeconds > 0)
				{
					seconds = new DerInteger(this.accuracySeconds);
				}
				DerInteger millis = null;
				if (this.accuracyMillis > 0)
				{
					millis = new DerInteger(this.accuracyMillis);
				}
				DerInteger micros = null;
				if (this.accuracyMicros > 0)
				{
					micros = new DerInteger(this.accuracyMicros);
				}
				accuracy = new Accuracy(seconds, millis, micros);
			}
			DerBoolean derBoolean = null;
			if (this.ordering)
			{
				derBoolean = DerBoolean.GetInstance(this.ordering);
			}
			DerInteger nonce = null;
			if (request.Nonce != null)
			{
				nonce = new DerInteger(request.Nonce);
			}
			DerObjectIdentifier tsaPolicyId = new DerObjectIdentifier(this.tsaPolicyOID);
			if (request.ReqPolicy != null)
			{
				tsaPolicyId = new DerObjectIdentifier(request.ReqPolicy);
			}
			TstInfo tstInfo = new TstInfo(tsaPolicyId, messageImprint, new DerInteger(serialNumber), new DerGeneralizedTime(genTime), accuracy, derBoolean, nonce, this.tsa, request.Extensions);
			TimeStampToken result;
			try
			{
				CmsSignedDataGenerator cmsSignedDataGenerator = new CmsSignedDataGenerator();
				byte[] derEncoded = tstInfo.GetDerEncoded();
				if (request.CertReq)
				{
					cmsSignedDataGenerator.AddCertificates(this.x509Certs);
				}
				cmsSignedDataGenerator.AddCrls(this.x509Crls);
				cmsSignedDataGenerator.AddSigner(this.key, this.cert, this.digestOID, this.signedAttr, this.unsignedAttr);
				result = new TimeStampToken(cmsSignedDataGenerator.Generate(PkcsObjectIdentifiers.IdCTTstInfo.Id, new CmsProcessableByteArray(derEncoded), true));
			}
			catch (CmsException e)
			{
				throw new TspException("Error generating time-stamp token", e);
			}
			catch (IOException e2)
			{
				throw new TspException("Exception encoding info", e2);
			}
			catch (X509StoreException e3)
			{
				throw new TspException("Exception handling CertStore", e3);
			}
			return result;
		}

		// Token: 0x04001849 RID: 6217
		private int accuracySeconds = -1;

		// Token: 0x0400184A RID: 6218
		private int accuracyMillis = -1;

		// Token: 0x0400184B RID: 6219
		private int accuracyMicros = -1;

		// Token: 0x0400184C RID: 6220
		private bool ordering;

		// Token: 0x0400184D RID: 6221
		private GeneralName tsa;

		// Token: 0x0400184E RID: 6222
		private string tsaPolicyOID;

		// Token: 0x0400184F RID: 6223
		private AsymmetricKeyParameter key;

		// Token: 0x04001850 RID: 6224
		private X509Certificate cert;

		// Token: 0x04001851 RID: 6225
		private string digestOID;

		// Token: 0x04001852 RID: 6226
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable signedAttr;

		// Token: 0x04001853 RID: 6227
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unsignedAttr;

		// Token: 0x04001854 RID: 6228
		private IX509Store x509Certs;

		// Token: 0x04001855 RID: 6229
		private IX509Store x509Crls;
	}
}
