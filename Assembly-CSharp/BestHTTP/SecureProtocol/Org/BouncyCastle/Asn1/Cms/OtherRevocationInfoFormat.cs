using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000799 RID: 1945
	public class OtherRevocationInfoFormat : Asn1Encodable
	{
		// Token: 0x0600458E RID: 17806 RVA: 0x00190C44 File Offset: 0x0018EE44
		public OtherRevocationInfoFormat(DerObjectIdentifier otherRevInfoFormat, Asn1Encodable otherRevInfo)
		{
			this.otherRevInfoFormat = otherRevInfoFormat;
			this.otherRevInfo = otherRevInfo;
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00190C5A File Offset: 0x0018EE5A
		private OtherRevocationInfoFormat(Asn1Sequence seq)
		{
			this.otherRevInfoFormat = DerObjectIdentifier.GetInstance(seq[0]);
			this.otherRevInfo = seq[1];
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x00190C81 File Offset: 0x0018EE81
		public static OtherRevocationInfoFormat GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return OtherRevocationInfoFormat.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00190C8F File Offset: 0x0018EE8F
		public static OtherRevocationInfoFormat GetInstance(object obj)
		{
			if (obj is OtherRevocationInfoFormat)
			{
				return (OtherRevocationInfoFormat)obj;
			}
			if (obj != null)
			{
				return new OtherRevocationInfoFormat(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06004592 RID: 17810 RVA: 0x00190CB0 File Offset: 0x0018EEB0
		public virtual DerObjectIdentifier InfoFormat
		{
			get
			{
				return this.otherRevInfoFormat;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06004593 RID: 17811 RVA: 0x00190CB8 File Offset: 0x0018EEB8
		public virtual Asn1Encodable Info
		{
			get
			{
				return this.otherRevInfo;
			}
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x00190CC0 File Offset: 0x0018EEC0
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.otherRevInfoFormat,
				this.otherRevInfo
			});
		}

		// Token: 0x04002D67 RID: 11623
		private readonly DerObjectIdentifier otherRevInfoFormat;

		// Token: 0x04002D68 RID: 11624
		private readonly Asn1Encodable otherRevInfo;
	}
}
