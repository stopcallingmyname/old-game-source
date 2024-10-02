using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E2 RID: 1506
	public class CmsAuthenticatedData
	{
		// Token: 0x0600397B RID: 14715 RVA: 0x00166E99 File Offset: 0x00165099
		public CmsAuthenticatedData(byte[] authData) : this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x00166EA7 File Offset: 0x001650A7
		public CmsAuthenticatedData(Stream authData) : this(CmsUtilities.ReadContentInfo(authData))
		{
		}

		// Token: 0x0600397D RID: 14717 RVA: 0x00166EB8 File Offset: 0x001650B8
		public CmsAuthenticatedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
			AuthenticatedData instance = AuthenticatedData.GetInstance(contentInfo.Content);
			Asn1Set recipientInfos = instance.RecipientInfos;
			this.macAlg = instance.MacAlgorithm;
			CmsReadable readable = new CmsProcessableByteArray(Asn1OctetString.GetInstance(instance.EncapsulatedContentInfo.Content).GetOctets());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(this.macAlg, readable);
			this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(recipientInfos, secureReadable);
			this.authAttrs = instance.AuthAttrs;
			this.mac = instance.Mac.GetOctets();
			this.unauthAttrs = instance.UnauthAttrs;
		}

		// Token: 0x0600397E RID: 14718 RVA: 0x00166F4F File Offset: 0x0016514F
		public byte[] GetMac()
		{
			return Arrays.Clone(this.mac);
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x0600397F RID: 14719 RVA: 0x00166F5C File Offset: 0x0016515C
		public AlgorithmIdentifier MacAlgorithmID
		{
			get
			{
				return this.macAlg;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06003980 RID: 14720 RVA: 0x00166F64 File Offset: 0x00165164
		public string MacAlgOid
		{
			get
			{
				return this.macAlg.Algorithm.Id;
			}
		}

		// Token: 0x06003981 RID: 14721 RVA: 0x00166F76 File Offset: 0x00165176
		public RecipientInformationStore GetRecipientInfos()
		{
			return this.recipientInfoStore;
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06003982 RID: 14722 RVA: 0x00166F7E File Offset: 0x0016517E
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x00166F86 File Offset: 0x00165186
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
		{
			if (this.authAttrs == null)
			{
				return null;
			}
			return new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(this.authAttrs);
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00166F9D File Offset: 0x0016519D
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
		{
			if (this.unauthAttrs == null)
			{
				return null;
			}
			return new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(this.unauthAttrs);
		}

		// Token: 0x06003985 RID: 14725 RVA: 0x00166FB4 File Offset: 0x001651B4
		public byte[] GetEncoded()
		{
			return this.contentInfo.GetEncoded();
		}

		// Token: 0x040025B8 RID: 9656
		internal RecipientInformationStore recipientInfoStore;

		// Token: 0x040025B9 RID: 9657
		internal ContentInfo contentInfo;

		// Token: 0x040025BA RID: 9658
		private AlgorithmIdentifier macAlg;

		// Token: 0x040025BB RID: 9659
		private Asn1Set authAttrs;

		// Token: 0x040025BC RID: 9660
		private Asn1Set unauthAttrs;

		// Token: 0x040025BD RID: 9661
		private byte[] mac;
	}
}
