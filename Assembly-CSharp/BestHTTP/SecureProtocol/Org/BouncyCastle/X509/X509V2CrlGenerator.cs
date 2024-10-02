using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200024F RID: 591
	public class X509V2CrlGenerator
	{
		// Token: 0x06001573 RID: 5491 RVA: 0x000AE5B8 File Offset: 0x000AC7B8
		public X509V2CrlGenerator()
		{
			this.tbsGen = new V2TbsCertListGenerator();
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x000AE5D6 File Offset: 0x000AC7D6
		public void Reset()
		{
			this.tbsGen = new V2TbsCertListGenerator();
			this.extGenerator.Reset();
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x000AE5EE File Offset: 0x000AC7EE
		public void SetIssuerDN(X509Name issuer)
		{
			this.tbsGen.SetIssuer(issuer);
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x000AE5FC File Offset: 0x000AC7FC
		public void SetThisUpdate(DateTime date)
		{
			this.tbsGen.SetThisUpdate(new Time(date));
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x000AE60F File Offset: 0x000AC80F
		public void SetNextUpdate(DateTime date)
		{
			this.tbsGen.SetNextUpdate(new Time(date));
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x000AE622 File Offset: 0x000AC822
		public void AddCrlEntry(BigInteger userCertificate, DateTime revocationDate, int reason)
		{
			this.tbsGen.AddCrlEntry(new DerInteger(userCertificate), new Time(revocationDate), reason);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000AE63C File Offset: 0x000AC83C
		public void AddCrlEntry(BigInteger userCertificate, DateTime revocationDate, int reason, DateTime invalidityDate)
		{
			this.tbsGen.AddCrlEntry(new DerInteger(userCertificate), new Time(revocationDate), reason, new DerGeneralizedTime(invalidityDate));
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x000AE65D File Offset: 0x000AC85D
		public void AddCrlEntry(BigInteger userCertificate, DateTime revocationDate, X509Extensions extensions)
		{
			this.tbsGen.AddCrlEntry(new DerInteger(userCertificate), new Time(revocationDate), extensions);
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x000AE678 File Offset: 0x000AC878
		public void AddCrl(X509Crl other)
		{
			if (other == null)
			{
				throw new ArgumentNullException("other");
			}
			ISet revokedCertificates = other.GetRevokedCertificates();
			if (revokedCertificates != null)
			{
				foreach (object obj in revokedCertificates)
				{
					X509CrlEntry x509CrlEntry = (X509CrlEntry)obj;
					try
					{
						this.tbsGen.AddCrlEntry(Asn1Sequence.GetInstance(Asn1Object.FromByteArray(x509CrlEntry.GetEncoded())));
					}
					catch (IOException e)
					{
						throw new CrlException("exception processing encoding of CRL", e);
					}
				}
			}
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x000AE718 File Offset: 0x000AC918
		[Obsolete("Not needed if Generate used with an ISignatureFactory")]
		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				this.sigOID = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("Unknown signature type requested", innerException);
			}
			this.sigAlgId = X509Utilities.GetSigAlgID(this.sigOID, signatureAlgorithm);
			this.tbsGen.SetSignature(this.sigAlgId);
		}

		// Token: 0x0600157D RID: 5501 RVA: 0x000AE77C File Offset: 0x000AC97C
		public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		// Token: 0x0600157E RID: 5502 RVA: 0x000AE791 File Offset: 0x000AC991
		public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extensionValue)
		{
			this.extGenerator.AddExtension(oid, critical, extensionValue);
		}

		// Token: 0x0600157F RID: 5503 RVA: 0x000AE7A1 File Offset: 0x000AC9A1
		public void AddExtension(string oid, bool critical, byte[] extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, new DerOctetString(extensionValue));
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x000AE7BB File Offset: 0x000AC9BB
		public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extensionValue)
		{
			this.extGenerator.AddExtension(oid, critical, new DerOctetString(extensionValue));
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x000AE7D0 File Offset: 0x000AC9D0
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Crl Generate(AsymmetricKeyParameter privateKey)
		{
			return this.Generate(privateKey, null);
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000AE7DA File Offset: 0x000AC9DA
		[Obsolete("Use Generate with an ISignatureFactory")]
		public X509Crl Generate(AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			return this.Generate(new Asn1SignatureFactory(this.signatureAlgorithm, privateKey, random));
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x000AE7F0 File Offset: 0x000AC9F0
		public X509Crl Generate(ISignatureFactory signatureCalculatorFactory)
		{
			this.tbsGen.SetSignature((AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails);
			TbsCertificateList tbsCertificateList = this.GenerateCertList();
			IStreamCalculator streamCalculator = signatureCalculatorFactory.CreateCalculator();
			byte[] derEncoded = tbsCertificateList.GetDerEncoded();
			streamCalculator.Stream.Write(derEncoded, 0, derEncoded.Length);
			Platform.Dispose(streamCalculator.Stream);
			return this.GenerateJcaObject(tbsCertificateList, (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails, ((IBlockResult)streamCalculator.GetResult()).Collect());
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x000AE865 File Offset: 0x000ACA65
		private TbsCertificateList GenerateCertList()
		{
			if (!this.extGenerator.IsEmpty)
			{
				this.tbsGen.SetExtensions(this.extGenerator.Generate());
			}
			return this.tbsGen.GenerateTbsCertList();
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x000AE895 File Offset: 0x000ACA95
		private X509Crl GenerateJcaObject(TbsCertificateList tbsCrl, AlgorithmIdentifier algId, byte[] signature)
		{
			return new X509Crl(CertificateList.GetInstance(new DerSequence(new Asn1Encodable[]
			{
				tbsCrl,
				algId,
				new DerBitString(signature)
			})));
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x000ADED6 File Offset: 0x000AC0D6
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04001653 RID: 5715
		private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

		// Token: 0x04001654 RID: 5716
		private V2TbsCertListGenerator tbsGen;

		// Token: 0x04001655 RID: 5717
		private DerObjectIdentifier sigOID;

		// Token: 0x04001656 RID: 5718
		private AlgorithmIdentifier sigAlgId;

		// Token: 0x04001657 RID: 5719
		private string signatureAlgorithm;
	}
}
