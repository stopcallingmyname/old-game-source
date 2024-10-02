using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x0200061D RID: 1565
	public class SignerInfoGenerator
	{
		// Token: 0x06003B0D RID: 15117 RVA: 0x0016D224 File Offset: 0x0016B424
		internal SignerInfoGenerator(SignerIdentifier sigId, ISignatureFactory signerFactory) : this(sigId, signerFactory, false)
		{
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0016D230 File Offset: 0x0016B430
		internal SignerInfoGenerator(SignerIdentifier sigId, ISignatureFactory signerFactory, bool isDirectSignature)
		{
			this.sigId = sigId;
			this.contentSigner = signerFactory;
			this.isDirectSignature = isDirectSignature;
			if (this.isDirectSignature)
			{
				this.signedGen = null;
				this.unsignedGen = null;
				return;
			}
			this.signedGen = new DefaultSignedAttributeTableGenerator();
			this.unsignedGen = null;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0016D281 File Offset: 0x0016B481
		internal SignerInfoGenerator(SignerIdentifier sigId, ISignatureFactory contentSigner, CmsAttributeTableGenerator signedGen, CmsAttributeTableGenerator unsignedGen)
		{
			this.sigId = sigId;
			this.contentSigner = contentSigner;
			this.signedGen = signedGen;
			this.unsignedGen = unsignedGen;
			this.isDirectSignature = false;
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0016D2AD File Offset: 0x0016B4AD
		internal void setAssociatedCertificate(X509Certificate certificate)
		{
			this.certificate = certificate;
		}

		// Token: 0x0400267A RID: 9850
		internal X509Certificate certificate;

		// Token: 0x0400267B RID: 9851
		internal ISignatureFactory contentSigner;

		// Token: 0x0400267C RID: 9852
		internal SignerIdentifier sigId;

		// Token: 0x0400267D RID: 9853
		internal CmsAttributeTableGenerator signedGen;

		// Token: 0x0400267E RID: 9854
		internal CmsAttributeTableGenerator unsignedGen;

		// Token: 0x0400267F RID: 9855
		private bool isDirectSignature;
	}
}
