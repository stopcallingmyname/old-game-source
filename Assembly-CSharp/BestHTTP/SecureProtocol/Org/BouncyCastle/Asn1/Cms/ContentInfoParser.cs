using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000786 RID: 1926
	public class ContentInfoParser
	{
		// Token: 0x060044FF RID: 17663 RVA: 0x0018F6CB File Offset: 0x0018D8CB
		public ContentInfoParser(Asn1SequenceParser seq)
		{
			this.contentType = (DerObjectIdentifier)seq.ReadObject();
			this.content = (Asn1TaggedObjectParser)seq.ReadObject();
		}

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06004500 RID: 17664 RVA: 0x0018F6F5 File Offset: 0x0018D8F5
		public DerObjectIdentifier ContentType
		{
			get
			{
				return this.contentType;
			}
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x0018F6FD File Offset: 0x0018D8FD
		public IAsn1Convertible GetContent(int tag)
		{
			if (this.content == null)
			{
				return null;
			}
			return this.content.GetObjectParser(tag, true);
		}

		// Token: 0x04002D31 RID: 11569
		private DerObjectIdentifier contentType;

		// Token: 0x04002D32 RID: 11570
		private Asn1TaggedObjectParser content;
	}
}
