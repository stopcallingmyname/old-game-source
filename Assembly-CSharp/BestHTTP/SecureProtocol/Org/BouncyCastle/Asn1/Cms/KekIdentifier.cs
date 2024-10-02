using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078E RID: 1934
	public class KekIdentifier : Asn1Encodable
	{
		// Token: 0x06004533 RID: 17715 RVA: 0x0018FF41 File Offset: 0x0018E141
		public KekIdentifier(byte[] keyIdentifier, DerGeneralizedTime date, OtherKeyAttribute other)
		{
			this.keyIdentifier = new DerOctetString(keyIdentifier);
			this.date = date;
			this.other = other;
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x0018FF64 File Offset: 0x0018E164
		public KekIdentifier(Asn1Sequence seq)
		{
			this.keyIdentifier = (Asn1OctetString)seq[0];
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
				throw new ArgumentException("Invalid KekIdentifier");
			}
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x0019000A File Offset: 0x0018E20A
		public static KekIdentifier GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return KekIdentifier.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00190018 File Offset: 0x0018E218
		public static KekIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is KekIdentifier)
			{
				return (KekIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KekIdentifier((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid KekIdentifier: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x00190055 File Offset: 0x0018E255
		public Asn1OctetString KeyIdentifier
		{
			get
			{
				return this.keyIdentifier;
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06004538 RID: 17720 RVA: 0x0019005D File Offset: 0x0018E25D
		public DerGeneralizedTime Date
		{
			get
			{
				return this.date;
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06004539 RID: 17721 RVA: 0x00190065 File Offset: 0x0018E265
		public OtherKeyAttribute Other
		{
			get
			{
				return this.other;
			}
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x00190070 File Offset: 0x0018E270
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.keyIdentifier
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.date,
				this.other
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D48 RID: 11592
		private Asn1OctetString keyIdentifier;

		// Token: 0x04002D49 RID: 11593
		private DerGeneralizedTime date;

		// Token: 0x04002D4A RID: 11594
		private OtherKeyAttribute other;
	}
}
