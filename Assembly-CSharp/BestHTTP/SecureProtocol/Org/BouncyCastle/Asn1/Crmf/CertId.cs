using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000765 RID: 1893
	public class CertId : Asn1Encodable
	{
		// Token: 0x0600440A RID: 17418 RVA: 0x0018CC64 File Offset: 0x0018AE64
		private CertId(Asn1Sequence seq)
		{
			this.issuer = GeneralName.GetInstance(seq[0]);
			this.serialNumber = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x0018CC90 File Offset: 0x0018AE90
		public static CertId GetInstance(object obj)
		{
			if (obj is CertId)
			{
				return (CertId)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertId((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x0018CCCF File Offset: 0x0018AECF
		public static CertId GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return CertId.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x0600440D RID: 17421 RVA: 0x0018CCDD File Offset: 0x0018AEDD
		public virtual GeneralName Issuer
		{
			get
			{
				return this.issuer;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x0600440E RID: 17422 RVA: 0x0018CCE5 File Offset: 0x0018AEE5
		public virtual DerInteger SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x0018CCED File Offset: 0x0018AEED
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.issuer,
				this.serialNumber
			});
		}

		// Token: 0x04002CA9 RID: 11433
		private readonly GeneralName issuer;

		// Token: 0x04002CAA RID: 11434
		private readonly DerInteger serialNumber;
	}
}
