using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A1 RID: 1953
	public class SignedDataParser
	{
		// Token: 0x060045D7 RID: 17879 RVA: 0x0019196C File Offset: 0x0018FB6C
		public static SignedDataParser GetInstance(object o)
		{
			if (o is Asn1Sequence)
			{
				return new SignedDataParser(((Asn1Sequence)o).Parser);
			}
			if (o is Asn1SequenceParser)
			{
				return new SignedDataParser((Asn1SequenceParser)o);
			}
			throw new IOException("unknown object encountered: " + Platform.GetTypeName(o));
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x001919BB File Offset: 0x0018FBBB
		public SignedDataParser(Asn1SequenceParser seq)
		{
			this._seq = seq;
			this._version = (DerInteger)seq.ReadObject();
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x060045D9 RID: 17881 RVA: 0x001919DB File Offset: 0x0018FBDB
		public DerInteger Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x001919E3 File Offset: 0x0018FBE3
		public Asn1SetParser GetDigestAlgorithms()
		{
			return (Asn1SetParser)this._seq.ReadObject();
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x001919F5 File Offset: 0x0018FBF5
		public ContentInfoParser GetEncapContentInfo()
		{
			return new ContentInfoParser((Asn1SequenceParser)this._seq.ReadObject());
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x00191A0C File Offset: 0x0018FC0C
		public Asn1SetParser GetCertificates()
		{
			this._certsCalled = true;
			this._nextObject = this._seq.ReadObject();
			if (this._nextObject is Asn1TaggedObjectParser && ((Asn1TaggedObjectParser)this._nextObject).TagNo == 0)
			{
				Asn1SetParser result = (Asn1SetParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser(17, false);
				this._nextObject = null;
				return result;
			}
			return null;
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00191A74 File Offset: 0x0018FC74
		public Asn1SetParser GetCrls()
		{
			if (!this._certsCalled)
			{
				throw new IOException("GetCerts() has not been called.");
			}
			this._crlsCalled = true;
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			if (this._nextObject is Asn1TaggedObjectParser && ((Asn1TaggedObjectParser)this._nextObject).TagNo == 1)
			{
				Asn1SetParser result = (Asn1SetParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser(17, false);
				this._nextObject = null;
				return result;
			}
			return null;
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x00191AF8 File Offset: 0x0018FCF8
		public Asn1SetParser GetSignerInfos()
		{
			if (!this._certsCalled || !this._crlsCalled)
			{
				throw new IOException("GetCerts() and/or GetCrls() has not been called.");
			}
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			return (Asn1SetParser)this._nextObject;
		}

		// Token: 0x04002D82 RID: 11650
		private Asn1SequenceParser _seq;

		// Token: 0x04002D83 RID: 11651
		private DerInteger _version;

		// Token: 0x04002D84 RID: 11652
		private object _nextObject;

		// Token: 0x04002D85 RID: 11653
		private bool _certsCalled;

		// Token: 0x04002D86 RID: 11654
		private bool _crlsCalled;
	}
}
