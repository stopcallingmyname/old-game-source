using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005EA RID: 1514
	public class CmsCompressedDataGenerator
	{
		// Token: 0x060039AA RID: 14762 RVA: 0x00167910 File Offset: 0x00165B10
		public CmsCompressedData Generate(CmsProcessable content, string compressionOid)
		{
			AlgorithmIdentifier compressionAlgorithm;
			Asn1OctetString content2;
			try
			{
				MemoryStream memoryStream = new MemoryStream();
				ZOutputStream zoutputStream = new ZOutputStream(memoryStream, -1);
				content.Write(zoutputStream);
				Platform.Dispose(zoutputStream);
				compressionAlgorithm = new AlgorithmIdentifier(new DerObjectIdentifier(compressionOid));
				content2 = new BerOctetString(memoryStream.ToArray());
			}
			catch (IOException e)
			{
				throw new CmsException("exception encoding data.", e);
			}
			ContentInfo encapContentInfo = new ContentInfo(CmsObjectIdentifiers.Data, content2);
			return new CmsCompressedData(new ContentInfo(CmsObjectIdentifiers.CompressedData, new CompressedData(compressionAlgorithm, encapContentInfo)));
		}

		// Token: 0x040025D6 RID: 9686
		public const string ZLib = "1.2.840.113549.1.9.16.3.8";
	}
}
