using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005E9 RID: 1513
	public class CmsCompressedData
	{
		// Token: 0x060039A2 RID: 14754 RVA: 0x001677EF File Offset: 0x001659EF
		public CmsCompressedData(byte[] compressedData) : this(CmsUtilities.ReadContentInfo(compressedData))
		{
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001677FD File Offset: 0x001659FD
		public CmsCompressedData(Stream compressedDataStream) : this(CmsUtilities.ReadContentInfo(compressedDataStream))
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x0016780B File Offset: 0x00165A0B
		public CmsCompressedData(ContentInfo contentInfo)
		{
			this.contentInfo = contentInfo;
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x0016781C File Offset: 0x00165A1C
		public byte[] GetContent()
		{
			ZInputStream zinputStream = new ZInputStream(((Asn1OctetString)CompressedData.GetInstance(this.contentInfo.Content).EncapContentInfo.Content).GetOctetStream());
			byte[] result;
			try
			{
				result = CmsUtilities.StreamToByteArray(zinputStream);
			}
			catch (IOException e)
			{
				throw new CmsException("exception reading compressed stream.", e);
			}
			finally
			{
				Platform.Dispose(zinputStream);
			}
			return result;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x00167890 File Offset: 0x00165A90
		public byte[] GetContent(int limit)
		{
			ZInputStream inStream = new ZInputStream(new MemoryStream(((Asn1OctetString)CompressedData.GetInstance(this.contentInfo.Content).EncapContentInfo.Content).GetOctets(), false));
			byte[] result;
			try
			{
				result = CmsUtilities.StreamToByteArray(inStream, limit);
			}
			catch (IOException e)
			{
				throw new CmsException("exception reading compressed stream.", e);
			}
			return result;
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x060039A7 RID: 14759 RVA: 0x001678F8 File Offset: 0x00165AF8
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x00167900 File Offset: 0x00165B00
		public byte[] GetEncoded()
		{
			return this.contentInfo.GetEncoded();
		}

		// Token: 0x040025D5 RID: 9685
		internal ContentInfo contentInfo;
	}
}
