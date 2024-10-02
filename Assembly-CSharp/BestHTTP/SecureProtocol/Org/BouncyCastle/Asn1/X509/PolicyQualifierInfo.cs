using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006AB RID: 1707
	public class PolicyQualifierInfo : Asn1Encodable
	{
		// Token: 0x06003F03 RID: 16131 RVA: 0x00179244 File Offset: 0x00177444
		public PolicyQualifierInfo(DerObjectIdentifier policyQualifierId, Asn1Encodable qualifier)
		{
			this.policyQualifierId = policyQualifierId;
			this.qualifier = qualifier;
		}

		// Token: 0x06003F04 RID: 16132 RVA: 0x0017925A File Offset: 0x0017745A
		public PolicyQualifierInfo(string cps)
		{
			this.policyQualifierId = PolicyQualifierID.IdQtCps;
			this.qualifier = new DerIA5String(cps);
		}

		// Token: 0x06003F05 RID: 16133 RVA: 0x0017927C File Offset: 0x0017747C
		private PolicyQualifierInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.policyQualifierId = DerObjectIdentifier.GetInstance(seq[0]);
			this.qualifier = seq[1];
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x001792D7 File Offset: 0x001774D7
		public static PolicyQualifierInfo GetInstance(object obj)
		{
			if (obj is PolicyQualifierInfo)
			{
				return (PolicyQualifierInfo)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new PolicyQualifierInfo(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06003F07 RID: 16135 RVA: 0x001792F8 File Offset: 0x001774F8
		public virtual DerObjectIdentifier PolicyQualifierId
		{
			get
			{
				return this.policyQualifierId;
			}
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06003F08 RID: 16136 RVA: 0x00179300 File Offset: 0x00177500
		public virtual Asn1Encodable Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		// Token: 0x06003F09 RID: 16137 RVA: 0x00179308 File Offset: 0x00177508
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.policyQualifierId,
				this.qualifier
			});
		}

		// Token: 0x04002802 RID: 10242
		private readonly DerObjectIdentifier policyQualifierId;

		// Token: 0x04002803 RID: 10243
		private readonly Asn1Encodable qualifier;
	}
}
