using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005EE RID: 1518
	public class CmsEnvelopedData
	{
		// Token: 0x060039B4 RID: 14772 RVA: 0x00167B55 File Offset: 0x00165D55
		public CmsEnvelopedData(byte[] envelopedData) : this(CmsUtilities.ReadContentInfo(envelopedData))
		{
		}

		// Token: 0x060039B5 RID: 14773 RVA: 0x00167B63 File Offset: 0x00165D63
		public CmsEnvelopedData(Stream envelopedData) : this(CmsUtilities.ReadContentInfo(envelopedData))
		{
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x00167B74 File Offset: 0x00165D74
		public CmsEnvelopedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
			EnvelopedData instance = EnvelopedData.GetInstance(contentInfo.Content);
			Asn1Set recipientInfos = instance.RecipientInfos;
			EncryptedContentInfo encryptedContentInfo = instance.EncryptedContentInfo;
			this.encAlg = encryptedContentInfo.ContentEncryptionAlgorithm;
			CmsReadable readable = new CmsProcessableByteArray(encryptedContentInfo.EncryptedContent.GetOctets());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsEnvelopedSecureReadable(this.encAlg, readable);
			this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
			this.unprotectedAttributes = instance.UnprotectedAttrs;
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x060039B7 RID: 14775 RVA: 0x00167BED File Offset: 0x00165DED
		public AlgorithmIdentifier EncryptionAlgorithmID
		{
			get
			{
				return this.encAlg;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x00167BF5 File Offset: 0x00165DF5
		public string EncryptionAlgOid
		{
			get
			{
				return this.encAlg.Algorithm.Id;
			}
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x00167C07 File Offset: 0x00165E07
		public RecipientInformationStore GetRecipientInfos()
		{
			return this.recipientInfoStore;
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x00167C0F File Offset: 0x00165E0F
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x00167C17 File Offset: 0x00165E17
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnprotectedAttributes()
		{
			if (this.unprotectedAttributes == null)
			{
				return null;
			}
			return new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(this.unprotectedAttributes);
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x00167C2E File Offset: 0x00165E2E
		public byte[] GetEncoded()
		{
			return this.contentInfo.GetEncoded();
		}

		// Token: 0x040025DB RID: 9691
		internal RecipientInformationStore recipientInfoStore;

		// Token: 0x040025DC RID: 9692
		internal ContentInfo contentInfo;

		// Token: 0x040025DD RID: 9693
		private AlgorithmIdentifier encAlg;

		// Token: 0x040025DE RID: 9694
		private Asn1Set unprotectedAttributes;
	}
}
