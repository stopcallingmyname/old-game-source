using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005F0 RID: 1520
	public class CmsEnvelopedDataParser : CmsContentInfoParser
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x00167EB8 File Offset: 0x001660B8
		public CmsEnvelopedDataParser(byte[] envelopedData) : this(new MemoryStream(envelopedData, false))
		{
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x00167EC8 File Offset: 0x001660C8
		public CmsEnvelopedDataParser(Stream envelopedData) : base(envelopedData)
		{
			this._attrNotRead = true;
			this.envelopedData = new EnvelopedDataParser((Asn1SequenceParser)this.contentInfo.GetContent(16));
			Asn1Set instance = Asn1Set.GetInstance(this.envelopedData.GetRecipientInfos().ToAsn1Object());
			EncryptedContentInfoParser encryptedContentInfo = this.envelopedData.GetEncryptedContentInfo();
			this._encAlg = encryptedContentInfo.ContentEncryptionAlgorithm;
			CmsReadable readable = new CmsProcessableInputStream(((Asn1OctetStringParser)encryptedContentInfo.GetEncryptedContent(4)).GetOctetStream());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsEnvelopedSecureReadable(this._encAlg, readable);
			this.recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(instance, secureReadable);
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060039C4 RID: 14788 RVA: 0x00167F5F File Offset: 0x0016615F
		public AlgorithmIdentifier EncryptionAlgorithmID
		{
			get
			{
				return this._encAlg;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060039C5 RID: 14789 RVA: 0x00167F67 File Offset: 0x00166167
		public string EncryptionAlgOid
		{
			get
			{
				return this._encAlg.Algorithm.Id;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060039C6 RID: 14790 RVA: 0x00167F7C File Offset: 0x0016617C
		public Asn1Object EncryptionAlgParams
		{
			get
			{
				Asn1Encodable parameters = this._encAlg.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x00167FA0 File Offset: 0x001661A0
		public RecipientInformationStore GetRecipientInfos()
		{
			return this.recipientInfoStore;
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00167FA8 File Offset: 0x001661A8
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnprotectedAttributes()
		{
			if (this._unprotectedAttributes == null && this._attrNotRead)
			{
				Asn1SetParser unprotectedAttrs = this.envelopedData.GetUnprotectedAttrs();
				this._attrNotRead = false;
				if (unprotectedAttrs != null)
				{
					Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					IAsn1Convertible asn1Convertible;
					while ((asn1Convertible = unprotectedAttrs.ReadObject()) != null)
					{
						Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							asn1SequenceParser.ToAsn1Object()
						});
					}
					this._unprotectedAttributes = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(new DerSet(asn1EncodableVector));
				}
			}
			return this._unprotectedAttributes;
		}

		// Token: 0x040025DF RID: 9695
		internal RecipientInformationStore recipientInfoStore;

		// Token: 0x040025E0 RID: 9696
		internal EnvelopedDataParser envelopedData;

		// Token: 0x040025E1 RID: 9697
		private AlgorithmIdentifier _encAlg;

		// Token: 0x040025E2 RID: 9698
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable _unprotectedAttributes;

		// Token: 0x040025E3 RID: 9699
		private bool _attrNotRead;
	}
}
