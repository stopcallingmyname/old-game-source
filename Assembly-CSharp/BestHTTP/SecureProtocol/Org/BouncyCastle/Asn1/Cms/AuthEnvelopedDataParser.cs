using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000780 RID: 1920
	public class AuthEnvelopedDataParser
	{
		// Token: 0x060044E0 RID: 17632 RVA: 0x0018F153 File Offset: 0x0018D353
		public AuthEnvelopedDataParser(Asn1SequenceParser seq)
		{
			this.seq = seq;
			this.version = (DerInteger)seq.ReadObject();
		}

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x060044E1 RID: 17633 RVA: 0x0018F173 File Offset: 0x0018D373
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x0018F17C File Offset: 0x0018D37C
		public OriginatorInfo GetOriginatorInfo()
		{
			this.originatorInfoCalled = true;
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject is Asn1TaggedObjectParser && ((Asn1TaggedObjectParser)this.nextObject).TagNo == 0)
			{
				IAsn1Convertible asn1Convertible = (Asn1SequenceParser)((Asn1TaggedObjectParser)this.nextObject).GetObjectParser(16, false);
				this.nextObject = null;
				return OriginatorInfo.GetInstance(asn1Convertible.ToAsn1Object());
			}
			return null;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x0018F1F3 File Offset: 0x0018D3F3
		public Asn1SetParser GetRecipientInfos()
		{
			if (!this.originatorInfoCalled)
			{
				this.GetOriginatorInfo();
			}
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			Asn1SetParser result = (Asn1SetParser)this.nextObject;
			this.nextObject = null;
			return result;
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x0018F22F File Offset: 0x0018D42F
		public EncryptedContentInfoParser GetAuthEncryptedContentInfo()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject != null)
			{
				Asn1SequenceParser asn1SequenceParser = (Asn1SequenceParser)this.nextObject;
				this.nextObject = null;
				return new EncryptedContentInfoParser(asn1SequenceParser);
			}
			return null;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0018F26C File Offset: 0x0018D46C
		public Asn1SetParser GetAuthAttrs()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject is Asn1TaggedObjectParser)
			{
				IAsn1Convertible asn1Convertible = this.nextObject;
				this.nextObject = null;
				return (Asn1SetParser)((Asn1TaggedObjectParser)asn1Convertible).GetObjectParser(17, false);
			}
			return null;
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x0018F2C0 File Offset: 0x0018D4C0
		public Asn1OctetString GetMac()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			IAsn1Convertible asn1Convertible = this.nextObject;
			this.nextObject = null;
			return Asn1OctetString.GetInstance(asn1Convertible.ToAsn1Object());
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0018F2F4 File Offset: 0x0018D4F4
		public Asn1SetParser GetUnauthAttrs()
		{
			if (this.nextObject == null)
			{
				this.nextObject = this.seq.ReadObject();
			}
			if (this.nextObject != null)
			{
				IAsn1Convertible asn1Convertible = this.nextObject;
				this.nextObject = null;
				return (Asn1SetParser)((Asn1TaggedObjectParser)asn1Convertible).GetObjectParser(17, false);
			}
			return null;
		}

		// Token: 0x04002D13 RID: 11539
		private Asn1SequenceParser seq;

		// Token: 0x04002D14 RID: 11540
		private DerInteger version;

		// Token: 0x04002D15 RID: 11541
		private IAsn1Convertible nextObject;

		// Token: 0x04002D16 RID: 11542
		private bool originatorInfoCalled;
	}
}
