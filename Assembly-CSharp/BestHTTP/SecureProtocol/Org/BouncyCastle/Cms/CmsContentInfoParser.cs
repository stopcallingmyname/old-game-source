using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005ED RID: 1517
	public class CmsContentInfoParser
	{
		// Token: 0x060039B2 RID: 14770 RVA: 0x00167AC8 File Offset: 0x00165CC8
		protected CmsContentInfoParser(Stream data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.data = data;
			try
			{
				Asn1StreamParser asn1StreamParser = new Asn1StreamParser(data);
				this.contentInfo = new ContentInfoParser((Asn1SequenceParser)asn1StreamParser.ReadObject());
			}
			catch (IOException e)
			{
				throw new CmsException("IOException reading content.", e);
			}
			catch (InvalidCastException e2)
			{
				throw new CmsException("Unexpected object reading content.", e2);
			}
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x00167B48 File Offset: 0x00165D48
		public void Close()
		{
			Platform.Dispose(this.data);
		}

		// Token: 0x040025D9 RID: 9689
		protected ContentInfoParser contentInfo;

		// Token: 0x040025DA RID: 9690
		protected Stream data;
	}
}
