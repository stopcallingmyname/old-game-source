using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006D1 RID: 1745
	public class QCStatement : Asn1Encodable
	{
		// Token: 0x06004049 RID: 16457 RVA: 0x0017E04C File Offset: 0x0017C24C
		public static QCStatement GetInstance(object obj)
		{
			if (obj == null || obj is QCStatement)
			{
				return (QCStatement)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new QCStatement(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x0017E099 File Offset: 0x0017C299
		private QCStatement(Asn1Sequence seq)
		{
			this.qcStatementId = DerObjectIdentifier.GetInstance(seq[0]);
			if (seq.Count > 1)
			{
				this.qcStatementInfo = seq[1];
			}
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0017E0C9 File Offset: 0x0017C2C9
		public QCStatement(DerObjectIdentifier qcStatementId)
		{
			this.qcStatementId = qcStatementId;
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x0017E0D8 File Offset: 0x0017C2D8
		public QCStatement(DerObjectIdentifier qcStatementId, Asn1Encodable qcStatementInfo)
		{
			this.qcStatementId = qcStatementId;
			this.qcStatementInfo = qcStatementInfo;
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x0600404D RID: 16461 RVA: 0x0017E0EE File Offset: 0x0017C2EE
		public DerObjectIdentifier StatementId
		{
			get
			{
				return this.qcStatementId;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0017E0F6 File Offset: 0x0017C2F6
		public Asn1Encodable StatementInfo
		{
			get
			{
				return this.qcStatementInfo;
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0017E100 File Offset: 0x0017C300
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.qcStatementId
			});
			if (this.qcStatementInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.qcStatementInfo
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040028EE RID: 10478
		private readonly DerObjectIdentifier qcStatementId;

		// Token: 0x040028EF RID: 10479
		private readonly Asn1Encodable qcStatementInfo;
	}
}
