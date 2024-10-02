using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005FC RID: 1532
	public class CmsSignedData
	{
		// Token: 0x06003A0D RID: 14861 RVA: 0x00168C14 File Offset: 0x00166E14
		private CmsSignedData(CmsSignedData c)
		{
			this.signedData = c.signedData;
			this.contentInfo = c.contentInfo;
			this.signedContent = c.signedContent;
			this.signerInfoStore = c.signerInfoStore;
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x00168C4C File Offset: 0x00166E4C
		public CmsSignedData(byte[] sigBlock) : this(CmsUtilities.ReadContentInfo(new MemoryStream(sigBlock, false)))
		{
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00168C60 File Offset: 0x00166E60
		public CmsSignedData(CmsProcessable signedContent, byte[] sigBlock) : this(signedContent, CmsUtilities.ReadContentInfo(new MemoryStream(sigBlock, false)))
		{
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x00168C75 File Offset: 0x00166E75
		public CmsSignedData(IDictionary hashes, byte[] sigBlock) : this(hashes, CmsUtilities.ReadContentInfo(sigBlock))
		{
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00168C84 File Offset: 0x00166E84
		public CmsSignedData(CmsProcessable signedContent, Stream sigData) : this(signedContent, CmsUtilities.ReadContentInfo(sigData))
		{
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00168C93 File Offset: 0x00166E93
		public CmsSignedData(Stream sigData) : this(CmsUtilities.ReadContentInfo(sigData))
		{
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x00168CA1 File Offset: 0x00166EA1
		public CmsSignedData(CmsProcessable signedContent, ContentInfo sigData)
		{
			this.signedContent = signedContent;
			this.contentInfo = sigData;
			this.signedData = SignedData.GetInstance(this.contentInfo.Content);
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x00168CCD File Offset: 0x00166ECD
		public CmsSignedData(IDictionary hashes, ContentInfo sigData)
		{
			this.hashes = hashes;
			this.contentInfo = sigData;
			this.signedData = SignedData.GetInstance(this.contentInfo.Content);
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00168CFC File Offset: 0x00166EFC
		public CmsSignedData(ContentInfo sigData)
		{
			this.contentInfo = sigData;
			this.signedData = SignedData.GetInstance(this.contentInfo.Content);
			if (this.signedData.EncapContentInfo.Content != null)
			{
				this.signedContent = new CmsProcessableByteArray(((Asn1OctetString)this.signedData.EncapContentInfo.Content).GetOctets());
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06003A16 RID: 14870 RVA: 0x00168D63 File Offset: 0x00166F63
		public int Version
		{
			get
			{
				return this.signedData.Version.Value.IntValue;
			}
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x00168D7C File Offset: 0x00166F7C
		public SignerInformationStore GetSignerInfos()
		{
			if (this.signerInfoStore == null)
			{
				IList list = Platform.CreateArrayList();
				foreach (object obj in this.signedData.SignerInfos)
				{
					SignerInfo instance = SignerInfo.GetInstance(obj);
					DerObjectIdentifier contentType = this.signedData.EncapContentInfo.ContentType;
					if (this.hashes == null)
					{
						list.Add(new SignerInformation(instance, contentType, this.signedContent, null));
					}
					else
					{
						byte[] digest = (byte[])this.hashes[instance.DigestAlgorithm.Algorithm.Id];
						list.Add(new SignerInformation(instance, contentType, null, new BaseDigestCalculator(digest)));
					}
				}
				this.signerInfoStore = new SignerInformationStore(list);
			}
			return this.signerInfoStore;
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00168E64 File Offset: 0x00167064
		public IX509Store GetAttributeCertificates(string type)
		{
			if (this.attrCertStore == null)
			{
				this.attrCertStore = CmsSignedData.Helper.CreateAttributeStore(type, this.signedData.Certificates);
			}
			return this.attrCertStore;
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00168E90 File Offset: 0x00167090
		public IX509Store GetCertificates(string type)
		{
			if (this.certificateStore == null)
			{
				this.certificateStore = CmsSignedData.Helper.CreateCertificateStore(type, this.signedData.Certificates);
			}
			return this.certificateStore;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00168EBC File Offset: 0x001670BC
		public IX509Store GetCrls(string type)
		{
			if (this.crlStore == null)
			{
				this.crlStore = CmsSignedData.Helper.CreateCrlStore(type, this.signedData.CRLs);
			}
			return this.crlStore;
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x06003A1B RID: 14875 RVA: 0x00168EE8 File Offset: 0x001670E8
		[Obsolete("Use 'SignedContentType' property instead.")]
		public string SignedContentTypeOid
		{
			get
			{
				return this.signedData.EncapContentInfo.ContentType.Id;
			}
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x06003A1C RID: 14876 RVA: 0x00168EFF File Offset: 0x001670FF
		public DerObjectIdentifier SignedContentType
		{
			get
			{
				return this.signedData.EncapContentInfo.ContentType;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x06003A1D RID: 14877 RVA: 0x00168F11 File Offset: 0x00167111
		public CmsProcessable SignedContent
		{
			get
			{
				return this.signedContent;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x06003A1E RID: 14878 RVA: 0x00168F19 File Offset: 0x00167119
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00168F21 File Offset: 0x00167121
		public byte[] GetEncoded()
		{
			return this.contentInfo.GetEncoded();
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00168F30 File Offset: 0x00167130
		public static CmsSignedData ReplaceSigners(CmsSignedData signedData, SignerInformationStore signerInformationStore)
		{
			CmsSignedData cmsSignedData = new CmsSignedData(signedData);
			cmsSignedData.signerInfoStore = signerInformationStore;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			Asn1EncodableVector asn1EncodableVector2 = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			foreach (object obj in signerInformationStore.GetSigners())
			{
				SignerInformation signerInformation = (SignerInformation)obj;
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					CmsSignedData.Helper.FixAlgID(signerInformation.DigestAlgorithmID)
				});
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					signerInformation.ToSignerInfo()
				});
			}
			Asn1Set asn1Set = new DerSet(asn1EncodableVector);
			Asn1Set asn1Set2 = new DerSet(asn1EncodableVector2);
			Asn1Sequence asn1Sequence = (Asn1Sequence)signedData.signedData.ToAsn1Object();
			asn1EncodableVector2 = new Asn1EncodableVector(new Asn1Encodable[]
			{
				asn1Sequence[0],
				asn1Set
			});
			for (int num = 2; num != asn1Sequence.Count - 1; num++)
			{
				asn1EncodableVector2.Add(new Asn1Encodable[]
				{
					asn1Sequence[num]
				});
			}
			asn1EncodableVector2.Add(new Asn1Encodable[]
			{
				asn1Set2
			});
			cmsSignedData.signedData = SignedData.GetInstance(new BerSequence(asn1EncodableVector2));
			cmsSignedData.contentInfo = new ContentInfo(cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData);
			return cmsSignedData;
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00169090 File Offset: 0x00167290
		public static CmsSignedData ReplaceCertificatesAndCrls(CmsSignedData signedData, IX509Store x509Certs, IX509Store x509Crls, IX509Store x509AttrCerts)
		{
			if (x509AttrCerts != null)
			{
				throw Platform.CreateNotImplementedException("Currently can't replace attribute certificates");
			}
			CmsSignedData cmsSignedData = new CmsSignedData(signedData);
			Asn1Set certificates = null;
			try
			{
				Asn1Set asn1Set = CmsUtilities.CreateBerSetFromList(CmsUtilities.GetCertificatesFromStore(x509Certs));
				if (asn1Set.Count != 0)
				{
					certificates = asn1Set;
				}
			}
			catch (X509StoreException e)
			{
				throw new CmsException("error getting certificates from store", e);
			}
			Asn1Set crls = null;
			try
			{
				Asn1Set asn1Set2 = CmsUtilities.CreateBerSetFromList(CmsUtilities.GetCrlsFromStore(x509Crls));
				if (asn1Set2.Count != 0)
				{
					crls = asn1Set2;
				}
			}
			catch (X509StoreException e2)
			{
				throw new CmsException("error getting CRLs from store", e2);
			}
			SignedData signedData2 = signedData.signedData;
			cmsSignedData.signedData = new SignedData(signedData2.DigestAlgorithms, signedData2.EncapContentInfo, certificates, crls, signedData2.SignerInfos);
			cmsSignedData.contentInfo = new ContentInfo(cmsSignedData.contentInfo.ContentType, cmsSignedData.signedData);
			return cmsSignedData;
		}

		// Token: 0x0400260D RID: 9741
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		// Token: 0x0400260E RID: 9742
		private readonly CmsProcessable signedContent;

		// Token: 0x0400260F RID: 9743
		private SignedData signedData;

		// Token: 0x04002610 RID: 9744
		private ContentInfo contentInfo;

		// Token: 0x04002611 RID: 9745
		private SignerInformationStore signerInfoStore;

		// Token: 0x04002612 RID: 9746
		private IX509Store attrCertStore;

		// Token: 0x04002613 RID: 9747
		private IX509Store certificateStore;

		// Token: 0x04002614 RID: 9748
		private IX509Store crlStore;

		// Token: 0x04002615 RID: 9749
		private IDictionary hashes;
	}
}
