using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000798 RID: 1944
	public class OtherRecipientInfo : Asn1Encodable
	{
		// Token: 0x06004587 RID: 17799 RVA: 0x00190BA1 File Offset: 0x0018EDA1
		public OtherRecipientInfo(DerObjectIdentifier oriType, Asn1Encodable oriValue)
		{
			this.oriType = oriType;
			this.oriValue = oriValue;
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x00190BB7 File Offset: 0x0018EDB7
		[Obsolete("Use GetInstance() instead")]
		public OtherRecipientInfo(Asn1Sequence seq)
		{
			this.oriType = DerObjectIdentifier.GetInstance(seq[0]);
			this.oriValue = seq[1];
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00190BDE File Offset: 0x0018EDDE
		public static OtherRecipientInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return OtherRecipientInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00190BEC File Offset: 0x0018EDEC
		public static OtherRecipientInfo GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			OtherRecipientInfo otherRecipientInfo = obj as OtherRecipientInfo;
			if (otherRecipientInfo != null)
			{
				return otherRecipientInfo;
			}
			return new OtherRecipientInfo(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600458B RID: 17803 RVA: 0x00190C15 File Offset: 0x0018EE15
		public virtual DerObjectIdentifier OriType
		{
			get
			{
				return this.oriType;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x00190C1D File Offset: 0x0018EE1D
		public virtual Asn1Encodable OriValue
		{
			get
			{
				return this.oriValue;
			}
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x00190C25 File Offset: 0x0018EE25
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.oriType,
				this.oriValue
			});
		}

		// Token: 0x04002D65 RID: 11621
		private readonly DerObjectIdentifier oriType;

		// Token: 0x04002D66 RID: 11622
		private readonly Asn1Encodable oriValue;
	}
}
