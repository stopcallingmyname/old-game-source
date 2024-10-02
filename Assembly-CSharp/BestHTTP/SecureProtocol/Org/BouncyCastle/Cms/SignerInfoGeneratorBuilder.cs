using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200061E RID: 1566
	public class SignerInfoGeneratorBuilder
	{
		// Token: 0x06003B12 RID: 15122 RVA: 0x0016D2B6 File Offset: 0x0016B4B6
		public SignerInfoGeneratorBuilder SetDirectSignature(bool hasNoSignedAttributes)
		{
			this.directSignature = hasNoSignedAttributes;
			return this;
		}

		// Token: 0x06003B13 RID: 15123 RVA: 0x0016D2C0 File Offset: 0x0016B4C0
		public SignerInfoGeneratorBuilder WithSignedAttributeGenerator(CmsAttributeTableGenerator signedGen)
		{
			this.signedGen = signedGen;
			return this;
		}

		// Token: 0x06003B14 RID: 15124 RVA: 0x0016D2CA File Offset: 0x0016B4CA
		public SignerInfoGeneratorBuilder WithUnsignedAttributeGenerator(CmsAttributeTableGenerator unsignedGen)
		{
			this.unsignedGen = unsignedGen;
			return this;
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x0016D2D4 File Offset: 0x0016B4D4
		public SignerInfoGenerator Build(ISignatureFactory contentSigner, X509Certificate certificate)
		{
			SignerIdentifier sigId = new SignerIdentifier(new IssuerAndSerialNumber(certificate.IssuerDN, new DerInteger(certificate.SerialNumber)));
			SignerInfoGenerator signerInfoGenerator = this.CreateGenerator(contentSigner, sigId);
			signerInfoGenerator.setAssociatedCertificate(certificate);
			return signerInfoGenerator;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x0016D30C File Offset: 0x0016B50C
		public SignerInfoGenerator Build(ISignatureFactory signerFactory, byte[] subjectKeyIdentifier)
		{
			SignerIdentifier sigId = new SignerIdentifier(new DerOctetString(subjectKeyIdentifier));
			return this.CreateGenerator(signerFactory, sigId);
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x0016D330 File Offset: 0x0016B530
		private SignerInfoGenerator CreateGenerator(ISignatureFactory contentSigner, SignerIdentifier sigId)
		{
			if (this.directSignature)
			{
				return new SignerInfoGenerator(sigId, contentSigner, true);
			}
			if (this.signedGen != null || this.unsignedGen != null)
			{
				if (this.signedGen == null)
				{
					this.signedGen = new DefaultSignedAttributeTableGenerator();
				}
				return new SignerInfoGenerator(sigId, contentSigner, this.signedGen, this.unsignedGen);
			}
			return new SignerInfoGenerator(sigId, contentSigner);
		}

		// Token: 0x04002680 RID: 9856
		private bool directSignature;

		// Token: 0x04002681 RID: 9857
		private CmsAttributeTableGenerator signedGen;

		// Token: 0x04002682 RID: 9858
		private CmsAttributeTableGenerator unsignedGen;
	}
}
