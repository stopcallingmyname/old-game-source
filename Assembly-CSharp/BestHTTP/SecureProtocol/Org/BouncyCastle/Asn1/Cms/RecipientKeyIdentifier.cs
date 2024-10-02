using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079E RID: 1950
	public class RecipientKeyIdentifier : Asn1Encodable
	{
		// Token: 0x060045B9 RID: 17849 RVA: 0x0019123C File Offset: 0x0018F43C
		public RecipientKeyIdentifier(Asn1OctetString subjectKeyIdentifier, DerGeneralizedTime date, OtherKeyAttribute other)
		{
			this.subjectKeyIdentifier = subjectKeyIdentifier;
			this.date = date;
			this.other = other;
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00191259 File Offset: 0x0018F459
		public RecipientKeyIdentifier(byte[] subjectKeyIdentifier) : this(subjectKeyIdentifier, null, null)
		{
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00191264 File Offset: 0x0018F464
		public RecipientKeyIdentifier(byte[] subjectKeyIdentifier, DerGeneralizedTime date, OtherKeyAttribute other)
		{
			this.subjectKeyIdentifier = new DerOctetString(subjectKeyIdentifier);
			this.date = date;
			this.other = other;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00191288 File Offset: 0x0018F488
		public RecipientKeyIdentifier(Asn1Sequence seq)
		{
			this.subjectKeyIdentifier = Asn1OctetString.GetInstance(seq[0]);
			switch (seq.Count)
			{
			case 1:
				return;
			case 2:
				if (seq[1] is DerGeneralizedTime)
				{
					this.date = (DerGeneralizedTime)seq[1];
					return;
				}
				this.other = OtherKeyAttribute.GetInstance(seq[2]);
				return;
			case 3:
				this.date = (DerGeneralizedTime)seq[1];
				this.other = OtherKeyAttribute.GetInstance(seq[2]);
				return;
			default:
				throw new ArgumentException("Invalid RecipientKeyIdentifier");
			}
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x0019132E File Offset: 0x0018F52E
		public static RecipientKeyIdentifier GetInstance(Asn1TaggedObject ato, bool explicitly)
		{
			return RecipientKeyIdentifier.GetInstance(Asn1Sequence.GetInstance(ato, explicitly));
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x0019133C File Offset: 0x0018F53C
		public static RecipientKeyIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is RecipientKeyIdentifier)
			{
				return (RecipientKeyIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RecipientKeyIdentifier((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid RecipientKeyIdentifier: " + Platform.GetTypeName(obj));
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x00191379 File Offset: 0x0018F579
		public Asn1OctetString SubjectKeyIdentifier
		{
			get
			{
				return this.subjectKeyIdentifier;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060045C0 RID: 17856 RVA: 0x00191381 File Offset: 0x0018F581
		public DerGeneralizedTime Date
		{
			get
			{
				return this.date;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x00191389 File Offset: 0x0018F589
		public OtherKeyAttribute OtherKeyAttribute
		{
			get
			{
				return this.other;
			}
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x00191394 File Offset: 0x0018F594
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.subjectKeyIdentifier
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.date,
				this.other
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D71 RID: 11633
		private Asn1OctetString subjectKeyIdentifier;

		// Token: 0x04002D72 RID: 11634
		private DerGeneralizedTime date;

		// Token: 0x04002D73 RID: 11635
		private OtherKeyAttribute other;
	}
}
