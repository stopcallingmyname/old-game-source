using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005EB RID: 1515
	public class CmsCompressedDataParser : CmsContentInfoParser
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x00167994 File Offset: 0x00165B94
		public CmsCompressedDataParser(byte[] compressedData) : this(new MemoryStream(compressedData, false))
		{
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001679A3 File Offset: 0x00165BA3
		public CmsCompressedDataParser(Stream compressedData) : base(compressedData)
		{
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x001679AC File Offset: 0x00165BAC
		public CmsTypedStream GetContent()
		{
			CmsTypedStream result;
			try
			{
				ContentInfoParser encapContentInfo = new CompressedDataParser((Asn1SequenceParser)this.contentInfo.GetContent(16)).GetEncapContentInfo();
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)encapContentInfo.GetContent(4);
				result = new CmsTypedStream(encapContentInfo.ContentType.ToString(), new ZInputStream(asn1OctetStringParser.GetOctetStream()));
			}
			catch (IOException e)
			{
				throw new CmsException("IOException reading compressed content.", e);
			}
			return result;
		}
	}
}
