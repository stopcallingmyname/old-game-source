using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509.Store;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005FE RID: 1534
	public class CmsSignedDataParser : CmsContentInfoParser
	{
		// Token: 0x06003A38 RID: 14904 RVA: 0x00169605 File Offset: 0x00167805
		public CmsSignedDataParser(byte[] sigBlock) : this(new MemoryStream(sigBlock, false))
		{
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00169614 File Offset: 0x00167814
		public CmsSignedDataParser(CmsTypedStream signedContent, byte[] sigBlock) : this(signedContent, new MemoryStream(sigBlock, false))
		{
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x00169624 File Offset: 0x00167824
		public CmsSignedDataParser(Stream sigData) : this(null, sigData)
		{
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x00169630 File Offset: 0x00167830
		public CmsSignedDataParser(CmsTypedStream signedContent, Stream sigData) : base(sigData)
		{
			try
			{
				this._signedContent = signedContent;
				this._signedData = SignedDataParser.GetInstance(this.contentInfo.GetContent(16));
				this._digests = Platform.CreateHashtable();
				this._digestOids = new HashSet();
				Asn1SetParser digestAlgorithms = this._signedData.GetDigestAlgorithms();
				IAsn1Convertible asn1Convertible;
				while ((asn1Convertible = digestAlgorithms.ReadObject()) != null)
				{
					AlgorithmIdentifier instance = AlgorithmIdentifier.GetInstance(asn1Convertible.ToAsn1Object());
					try
					{
						string id = instance.Algorithm.Id;
						string digestAlgName = CmsSignedDataParser.Helper.GetDigestAlgName(id);
						if (!this._digests.Contains(digestAlgName))
						{
							this._digests[digestAlgName] = CmsSignedDataParser.Helper.GetDigestInstance(digestAlgName);
							this._digestOids.Add(id);
						}
					}
					catch (SecurityUtilityException)
					{
					}
				}
				ContentInfoParser encapContentInfo = this._signedData.GetEncapContentInfo();
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)encapContentInfo.GetContent(4);
				if (asn1OctetStringParser != null)
				{
					CmsTypedStream cmsTypedStream = new CmsTypedStream(encapContentInfo.ContentType.Id, asn1OctetStringParser.GetOctetStream());
					if (this._signedContent == null)
					{
						this._signedContent = cmsTypedStream;
					}
					else
					{
						cmsTypedStream.Drain();
					}
				}
				this._signedContentType = ((this._signedContent == null) ? encapContentInfo.ContentType : new DerObjectIdentifier(this._signedContent.ContentType));
			}
			catch (IOException ex)
			{
				throw new CmsException("io exception: " + ex.Message, ex);
			}
		}

		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06003A3C RID: 14908 RVA: 0x001697B8 File Offset: 0x001679B8
		public int Version
		{
			get
			{
				return this._signedData.Version.Value.IntValue;
			}
		}

		// Token: 0x17000787 RID: 1927
		// (get) Token: 0x06003A3D RID: 14909 RVA: 0x001697CF File Offset: 0x001679CF
		public ISet DigestOids
		{
			get
			{
				return new HashSet(this._digestOids);
			}
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x001697DC File Offset: 0x001679DC
		public SignerInformationStore GetSignerInfos()
		{
			if (this._signerInfoStore == null)
			{
				this.PopulateCertCrlSets();
				IList list = Platform.CreateArrayList();
				IDictionary dictionary = Platform.CreateHashtable();
				foreach (object key in this._digests.Keys)
				{
					dictionary[key] = DigestUtilities.DoFinal((IDigest)this._digests[key]);
				}
				try
				{
					Asn1SetParser signerInfos = this._signedData.GetSignerInfos();
					IAsn1Convertible asn1Convertible;
					while ((asn1Convertible = signerInfos.ReadObject()) != null)
					{
						SignerInfo instance = SignerInfo.GetInstance(asn1Convertible.ToAsn1Object());
						string digestAlgName = CmsSignedDataParser.Helper.GetDigestAlgName(instance.DigestAlgorithm.Algorithm.Id);
						byte[] digest = (byte[])dictionary[digestAlgName];
						list.Add(new SignerInformation(instance, this._signedContentType, null, new BaseDigestCalculator(digest)));
					}
				}
				catch (IOException ex)
				{
					throw new CmsException("io exception: " + ex.Message, ex);
				}
				this._signerInfoStore = new SignerInformationStore(list);
			}
			return this._signerInfoStore;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00169918 File Offset: 0x00167B18
		public IX509Store GetAttributeCertificates(string type)
		{
			if (this._attributeStore == null)
			{
				this.PopulateCertCrlSets();
				this._attributeStore = CmsSignedDataParser.Helper.CreateAttributeStore(type, this._certSet);
			}
			return this._attributeStore;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00169945 File Offset: 0x00167B45
		public IX509Store GetCertificates(string type)
		{
			if (this._certificateStore == null)
			{
				this.PopulateCertCrlSets();
				this._certificateStore = CmsSignedDataParser.Helper.CreateCertificateStore(type, this._certSet);
			}
			return this._certificateStore;
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00169972 File Offset: 0x00167B72
		public IX509Store GetCrls(string type)
		{
			if (this._crlStore == null)
			{
				this.PopulateCertCrlSets();
				this._crlStore = CmsSignedDataParser.Helper.CreateCrlStore(type, this._crlSet);
			}
			return this._crlStore;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x001699A0 File Offset: 0x00167BA0
		private void PopulateCertCrlSets()
		{
			if (this._isCertCrlParsed)
			{
				return;
			}
			this._isCertCrlParsed = true;
			try
			{
				this._certSet = CmsSignedDataParser.GetAsn1Set(this._signedData.GetCertificates());
				this._crlSet = CmsSignedDataParser.GetAsn1Set(this._signedData.GetCrls());
			}
			catch (IOException e)
			{
				throw new CmsException("problem parsing cert/crl sets", e);
			}
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06003A43 RID: 14915 RVA: 0x00169A08 File Offset: 0x00167C08
		public DerObjectIdentifier SignedContentType
		{
			get
			{
				return this._signedContentType;
			}
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x00169A10 File Offset: 0x00167C10
		public CmsTypedStream GetSignedContent()
		{
			if (this._signedContent == null)
			{
				return null;
			}
			Stream stream = this._signedContent.ContentStream;
			foreach (object obj in this._digests.Values)
			{
				IDigest readDigest = (IDigest)obj;
				stream = new DigestStream(stream, readDigest, null);
			}
			return new CmsTypedStream(this._signedContent.ContentType, stream);
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00169A98 File Offset: 0x00167C98
		public static Stream ReplaceSigners(Stream original, SignerInformationStore signerInformationStore, Stream outStr)
		{
			CmsSignedDataStreamGenerator cmsSignedDataStreamGenerator = new CmsSignedDataStreamGenerator();
			CmsSignedDataParser cmsSignedDataParser = new CmsSignedDataParser(original);
			cmsSignedDataStreamGenerator.AddSigners(signerInformationStore);
			CmsTypedStream signedContent = cmsSignedDataParser.GetSignedContent();
			bool flag = signedContent != null;
			Stream stream = cmsSignedDataStreamGenerator.Open(outStr, cmsSignedDataParser.SignedContentType.Id, flag);
			if (flag)
			{
				Streams.PipeAll(signedContent.ContentStream, stream);
			}
			cmsSignedDataStreamGenerator.AddAttributeCertificates(cmsSignedDataParser.GetAttributeCertificates("Collection"));
			cmsSignedDataStreamGenerator.AddCertificates(cmsSignedDataParser.GetCertificates("Collection"));
			cmsSignedDataStreamGenerator.AddCrls(cmsSignedDataParser.GetCrls("Collection"));
			Platform.Dispose(stream);
			return outStr;
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00169B20 File Offset: 0x00167D20
		public static Stream ReplaceCertificatesAndCrls(Stream original, IX509Store x509Certs, IX509Store x509Crls, IX509Store x509AttrCerts, Stream outStr)
		{
			CmsSignedDataStreamGenerator cmsSignedDataStreamGenerator = new CmsSignedDataStreamGenerator();
			CmsSignedDataParser cmsSignedDataParser = new CmsSignedDataParser(original);
			cmsSignedDataStreamGenerator.AddDigests(cmsSignedDataParser.DigestOids);
			CmsTypedStream signedContent = cmsSignedDataParser.GetSignedContent();
			bool flag = signedContent != null;
			Stream stream = cmsSignedDataStreamGenerator.Open(outStr, cmsSignedDataParser.SignedContentType.Id, flag);
			if (flag)
			{
				Streams.PipeAll(signedContent.ContentStream, stream);
			}
			if (x509AttrCerts != null)
			{
				cmsSignedDataStreamGenerator.AddAttributeCertificates(x509AttrCerts);
			}
			if (x509Certs != null)
			{
				cmsSignedDataStreamGenerator.AddCertificates(x509Certs);
			}
			if (x509Crls != null)
			{
				cmsSignedDataStreamGenerator.AddCrls(x509Crls);
			}
			cmsSignedDataStreamGenerator.AddSigners(cmsSignedDataParser.GetSignerInfos());
			Platform.Dispose(stream);
			return outStr;
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00169BAB File Offset: 0x00167DAB
		private static Asn1Set GetAsn1Set(Asn1SetParser asn1SetParser)
		{
			if (asn1SetParser != null)
			{
				return Asn1Set.GetInstance(asn1SetParser.ToAsn1Object());
			}
			return null;
		}

		// Token: 0x04002618 RID: 9752
		private static readonly CmsSignedHelper Helper = CmsSignedHelper.Instance;

		// Token: 0x04002619 RID: 9753
		private SignedDataParser _signedData;

		// Token: 0x0400261A RID: 9754
		private DerObjectIdentifier _signedContentType;

		// Token: 0x0400261B RID: 9755
		private CmsTypedStream _signedContent;

		// Token: 0x0400261C RID: 9756
		private IDictionary _digests;

		// Token: 0x0400261D RID: 9757
		private ISet _digestOids;

		// Token: 0x0400261E RID: 9758
		private SignerInformationStore _signerInfoStore;

		// Token: 0x0400261F RID: 9759
		private Asn1Set _certSet;

		// Token: 0x04002620 RID: 9760
		private Asn1Set _crlSet;

		// Token: 0x04002621 RID: 9761
		private bool _isCertCrlParsed;

		// Token: 0x04002622 RID: 9762
		private IX509Store _attributeStore;

		// Token: 0x04002623 RID: 9763
		private IX509Store _certificateStore;

		// Token: 0x04002624 RID: 9764
		private IX509Store _crlStore;
	}
}
