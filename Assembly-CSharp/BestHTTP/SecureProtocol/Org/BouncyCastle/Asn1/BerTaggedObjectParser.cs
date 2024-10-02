using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000645 RID: 1605
	public class BerTaggedObjectParser : Asn1TaggedObjectParser, IAsn1Convertible
	{
		// Token: 0x06003C02 RID: 15362 RVA: 0x00170180 File Offset: 0x0016E380
		[Obsolete]
		internal BerTaggedObjectParser(int baseTag, int tagNumber, Stream contentStream) : this((baseTag & 32) != 0, tagNumber, new Asn1StreamParser(contentStream))
		{
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x00170196 File Offset: 0x0016E396
		internal BerTaggedObjectParser(bool constructed, int tagNumber, Asn1StreamParser parser)
		{
			this._constructed = constructed;
			this._tagNumber = tagNumber;
			this._parser = parser;
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06003C04 RID: 15364 RVA: 0x001701B3 File Offset: 0x0016E3B3
		public bool IsConstructed
		{
			get
			{
				return this._constructed;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06003C05 RID: 15365 RVA: 0x001701BB File Offset: 0x0016E3BB
		public int TagNo
		{
			get
			{
				return this._tagNumber;
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001701C3 File Offset: 0x0016E3C3
		public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
		{
			if (!isExplicit)
			{
				return this._parser.ReadImplicit(this._constructed, tag);
			}
			if (!this._constructed)
			{
				throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)");
			}
			return this._parser.ReadObject();
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x001701FC File Offset: 0x0016E3FC
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = this._parser.ReadTaggedObject(this._constructed, this._tagNumber);
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException(ex.Message);
			}
			return result;
		}

		// Token: 0x040026CD RID: 9933
		private bool _constructed;

		// Token: 0x040026CE RID: 9934
		private int _tagNumber;

		// Token: 0x040026CF RID: 9935
		private Asn1StreamParser _parser;
	}
}
