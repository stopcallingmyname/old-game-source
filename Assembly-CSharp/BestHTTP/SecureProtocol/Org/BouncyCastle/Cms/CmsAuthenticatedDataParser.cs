using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E4 RID: 1508
	public class CmsAuthenticatedDataParser : CmsContentInfoParser
	{
		// Token: 0x0600398A RID: 14730 RVA: 0x001671E0 File Offset: 0x001653E0
		public CmsAuthenticatedDataParser(byte[] envelopedData) : this(new MemoryStream(envelopedData, false))
		{
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x001671F0 File Offset: 0x001653F0
		public CmsAuthenticatedDataParser(Stream envelopedData) : base(envelopedData)
		{
			this.authAttrNotRead = true;
			this.authData = new AuthenticatedDataParser((Asn1SequenceParser)this.contentInfo.GetContent(16));
			Asn1Set instance = Asn1Set.GetInstance(this.authData.GetRecipientInfos().ToAsn1Object());
			this.macAlg = this.authData.GetMacAlgorithm();
			CmsReadable readable = new CmsProcessableInputStream(((Asn1OctetStringParser)this.authData.GetEnapsulatedContentInfo().GetContent(4)).GetOctetStream());
			CmsSecureReadable secureReadable = new CmsEnvelopedHelper.CmsAuthenticatedSecureReadable(this.macAlg, readable);
			this._recipientInfoStore = CmsEnvelopedHelper.BuildRecipientInformationStore(instance, secureReadable);
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x0600398C RID: 14732 RVA: 0x0016728A File Offset: 0x0016548A
		public AlgorithmIdentifier MacAlgorithmID
		{
			get
			{
				return this.macAlg;
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600398D RID: 14733 RVA: 0x00167292 File Offset: 0x00165492
		public string MacAlgOid
		{
			get
			{
				return this.macAlg.Algorithm.Id;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600398E RID: 14734 RVA: 0x001672A4 File Offset: 0x001654A4
		public Asn1Object MacAlgParams
		{
			get
			{
				Asn1Encodable parameters = this.macAlg.Parameters;
				if (parameters != null)
				{
					return parameters.ToAsn1Object();
				}
				return null;
			}
		}

		// Token: 0x0600398F RID: 14735 RVA: 0x001672C8 File Offset: 0x001654C8
		public RecipientInformationStore GetRecipientInfos()
		{
			return this._recipientInfoStore;
		}

		// Token: 0x06003990 RID: 14736 RVA: 0x001672D0 File Offset: 0x001654D0
		public byte[] GetMac()
		{
			if (this.mac == null)
			{
				this.GetAuthAttrs();
				this.mac = this.authData.GetMac().GetOctets();
			}
			return Arrays.Clone(this.mac);
		}

		// Token: 0x06003991 RID: 14737 RVA: 0x00167304 File Offset: 0x00165504
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetAuthAttrs()
		{
			if (this.authAttrs == null && this.authAttrNotRead)
			{
				Asn1SetParser asn1SetParser = this.authData.GetAuthAttrs();
				this.authAttrNotRead = false;
				if (asn1SetParser != null)
				{
					Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					IAsn1Convertible asn1Convertible;
					while ((asn1Convertible = asn1SetParser.ReadObject()) != null)
					{
						Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							asn1SequenceParser.ToAsn1Object()
						});
					}
					this.authAttrs = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(new DerSet(asn1EncodableVector));
				}
			}
			return this.authAttrs;
		}

		// Token: 0x06003992 RID: 14738 RVA: 0x00167384 File Offset: 0x00165584
		public BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable GetUnauthAttrs()
		{
			if (this.unauthAttrs == null && this.unauthAttrNotRead)
			{
				Asn1SetParser asn1SetParser = this.authData.GetUnauthAttrs();
				this.unauthAttrNotRead = false;
				if (asn1SetParser != null)
				{
					Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
					IAsn1Convertible asn1Convertible;
					while ((asn1Convertible = asn1SetParser.ReadObject()) != null)
					{
						Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)asn1Convertible;
						asn1EncodableVector.Add(new Asn1Encodable[]
						{
							asn1SequenceParser.ToAsn1Object()
						});
					}
					this.unauthAttrs = new BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable(new DerSet(asn1EncodableVector));
				}
			}
			return this.unauthAttrs;
		}

		// Token: 0x040025BE RID: 9662
		internal RecipientInformationStore _recipientInfoStore;

		// Token: 0x040025BF RID: 9663
		internal AuthenticatedDataParser authData;

		// Token: 0x040025C0 RID: 9664
		private AlgorithmIdentifier macAlg;

		// Token: 0x040025C1 RID: 9665
		private byte[] mac;

		// Token: 0x040025C2 RID: 9666
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable authAttrs;

		// Token: 0x040025C3 RID: 9667
		private BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms.AttributeTable unauthAttrs;

		// Token: 0x040025C4 RID: 9668
		private bool authAttrNotRead;

		// Token: 0x040025C5 RID: 9669
		private bool unauthAttrNotRead;
	}
}
