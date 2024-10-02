using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Operators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200024E RID: 590
	public class X509V2AttributeCertificateGenerator
	{
		// Token: 0x06001563 RID: 5475 RVA: 0x000AE381 File Offset: 0x000AC581
		public X509V2AttributeCertificateGenerator()
		{
			this.acInfoGen = new V2AttributeCertificateInfoGenerator();
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x000AE39F File Offset: 0x000AC59F
		public void Reset()
		{
			this.acInfoGen = new V2AttributeCertificateInfoGenerator();
			this.extGenerator.Reset();
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x000AE3B7 File Offset: 0x000AC5B7
		public void SetHolder(AttributeCertificateHolder holder)
		{
			this.acInfoGen.SetHolder(holder.holder);
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x000AE3CA File Offset: 0x000AC5CA
		public void SetIssuer(AttributeCertificateIssuer issuer)
		{
			this.acInfoGen.SetIssuer(AttCertIssuer.GetInstance(issuer.form));
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x000AE3E2 File Offset: 0x000AC5E2
		public void SetSerialNumber(BigInteger serialNumber)
		{
			this.acInfoGen.SetSerialNumber(new DerInteger(serialNumber));
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x000AE3F5 File Offset: 0x000AC5F5
		public void SetNotBefore(DateTime date)
		{
			this.acInfoGen.SetStartDate(new DerGeneralizedTime(date));
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000AE408 File Offset: 0x000AC608
		public void SetNotAfter(DateTime date)
		{
			this.acInfoGen.SetEndDate(new DerGeneralizedTime(date));
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000AE41C File Offset: 0x000AC61C
		[Obsolete("Not needed if Generate used with an ISignatureFactory")]
		public void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
			try
			{
				this.sigOID = X509Utilities.GetAlgorithmOid(signatureAlgorithm);
			}
			catch (Exception)
			{
				throw new ArgumentException("Unknown signature type requested");
			}
			this.sigAlgId = X509Utilities.GetSigAlgID(this.sigOID, signatureAlgorithm);
			this.acInfoGen.SetSignature(this.sigAlgId);
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000AE480 File Offset: 0x000AC680
		public void AddAttribute(X509Attribute attribute)
		{
			this.acInfoGen.AddAttribute(AttributeX509.GetInstance(attribute.ToAsn1Object()));
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000AE498 File Offset: 0x000AC698
		public void SetIssuerUniqueId(bool[] iui)
		{
			throw Platform.CreateNotImplementedException("SetIssuerUniqueId()");
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000AE4A4 File Offset: 0x000AC6A4
		public void AddExtension(string oid, bool critical, Asn1Encodable extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x000AE4B9 File Offset: 0x000AC6B9
		public void AddExtension(string oid, bool critical, byte[] extensionValue)
		{
			this.extGenerator.AddExtension(new DerObjectIdentifier(oid), critical, extensionValue);
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000AE4CE File Offset: 0x000AC6CE
		[Obsolete("Use Generate with an ISignatureFactory")]
		public IX509AttributeCertificate Generate(AsymmetricKeyParameter privateKey)
		{
			return this.Generate(privateKey, null);
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x000AE4D8 File Offset: 0x000AC6D8
		[Obsolete("Use Generate with an ISignatureFactory")]
		public IX509AttributeCertificate Generate(AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			return this.Generate(new Asn1SignatureFactory(this.signatureAlgorithm, privateKey, random));
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x000AE4F0 File Offset: 0x000AC6F0
		public IX509AttributeCertificate Generate(ISignatureFactory signatureCalculatorFactory)
		{
			if (!this.extGenerator.IsEmpty)
			{
				this.acInfoGen.SetExtensions(this.extGenerator.Generate());
			}
			AlgorithmIdentifier signature = (AlgorithmIdentifier)signatureCalculatorFactory.AlgorithmDetails;
			this.acInfoGen.SetSignature(signature);
			AttributeCertificateInfo attributeCertificateInfo = this.acInfoGen.GenerateAttributeCertificateInfo();
			byte[] derEncoded = attributeCertificateInfo.GetDerEncoded();
			IStreamCalculator streamCalculator = signatureCalculatorFactory.CreateCalculator();
			streamCalculator.Stream.Write(derEncoded, 0, derEncoded.Length);
			Platform.Dispose(streamCalculator.Stream);
			IX509AttributeCertificate result;
			try
			{
				DerBitString signatureValue = new DerBitString(((IBlockResult)streamCalculator.GetResult()).Collect());
				result = new X509V2AttributeCertificate(new AttributeCertificate(attributeCertificateInfo, signature, signatureValue));
			}
			catch (Exception e)
			{
				throw new CertificateEncodingException("constructed invalid certificate", e);
			}
			return result;
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x000ADED6 File Offset: 0x000AC0D6
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x0400164E RID: 5710
		private readonly X509ExtensionsGenerator extGenerator = new X509ExtensionsGenerator();

		// Token: 0x0400164F RID: 5711
		private V2AttributeCertificateInfoGenerator acInfoGen;

		// Token: 0x04001650 RID: 5712
		private DerObjectIdentifier sigOID;

		// Token: 0x04001651 RID: 5713
		private AlgorithmIdentifier sigAlgId;

		// Token: 0x04001652 RID: 5714
		private string signatureAlgorithm;
	}
}
