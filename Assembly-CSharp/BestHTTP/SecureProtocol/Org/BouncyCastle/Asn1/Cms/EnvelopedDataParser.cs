using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078B RID: 1931
	public class EnvelopedDataParser
	{
		// Token: 0x06004521 RID: 17697 RVA: 0x0018FC98 File Offset: 0x0018DE98
		public EnvelopedDataParser(Asn1SequenceParser seq)
		{
			this._seq = seq;
			this._version = (DerInteger)seq.ReadObject();
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06004522 RID: 17698 RVA: 0x0018FCB8 File Offset: 0x0018DEB8
		public DerInteger Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0018FCC0 File Offset: 0x0018DEC0
		public OriginatorInfo GetOriginatorInfo()
		{
			this._originatorInfoCalled = true;
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			if (this._nextObject is Asn1TaggedObjectParser && ((Asn1TaggedObjectParser)this._nextObject).TagNo == 0)
			{
				IAsn1Convertible asn1Convertible = (Asn1SequenceParser)((Asn1TaggedObjectParser)this._nextObject).GetObjectParser(16, false);
				this._nextObject = null;
				return OriginatorInfo.GetInstance(asn1Convertible.ToAsn1Object());
			}
			return null;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x0018FD37 File Offset: 0x0018DF37
		public Asn1SetParser GetRecipientInfos()
		{
			if (!this._originatorInfoCalled)
			{
				this.GetOriginatorInfo();
			}
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			Asn1SetParser result = (Asn1SetParser)this._nextObject;
			this._nextObject = null;
			return result;
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x0018FD73 File Offset: 0x0018DF73
		public EncryptedContentInfoParser GetEncryptedContentInfo()
		{
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			if (this._nextObject != null)
			{
				Asn1SequenceParser seq = (Asn1SequenceParser)this._nextObject;
				this._nextObject = null;
				return new EncryptedContentInfoParser(seq);
			}
			return null;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x0018FDB0 File Offset: 0x0018DFB0
		public Asn1SetParser GetUnprotectedAttrs()
		{
			if (this._nextObject == null)
			{
				this._nextObject = this._seq.ReadObject();
			}
			if (this._nextObject != null)
			{
				IAsn1Convertible nextObject = this._nextObject;
				this._nextObject = null;
				return (Asn1SetParser)((Asn1TaggedObjectParser)nextObject).GetObjectParser(17, false);
			}
			return null;
		}

		// Token: 0x04002D41 RID: 11585
		private Asn1SequenceParser _seq;

		// Token: 0x04002D42 RID: 11586
		private DerInteger _version;

		// Token: 0x04002D43 RID: 11587
		private IAsn1Convertible _nextObject;

		// Token: 0x04002D44 RID: 11588
		private bool _originatorInfoCalled;
	}
}
