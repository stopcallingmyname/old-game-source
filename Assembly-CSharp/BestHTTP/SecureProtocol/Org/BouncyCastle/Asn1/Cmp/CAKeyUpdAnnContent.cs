using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AA RID: 1962
	public class CAKeyUpdAnnContent : Asn1Encodable
	{
		// Token: 0x0600461B RID: 17947 RVA: 0x001925F8 File Offset: 0x001907F8
		private CAKeyUpdAnnContent(Asn1Sequence seq)
		{
			this.oldWithNew = CmpCertificate.GetInstance(seq[0]);
			this.newWithOld = CmpCertificate.GetInstance(seq[1]);
			this.newWithNew = CmpCertificate.GetInstance(seq[2]);
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00192636 File Offset: 0x00190836
		public static CAKeyUpdAnnContent GetInstance(object obj)
		{
			if (obj is CAKeyUpdAnnContent)
			{
				return (CAKeyUpdAnnContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CAKeyUpdAnnContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x0600461D RID: 17949 RVA: 0x00192675 File Offset: 0x00190875
		public virtual CmpCertificate OldWithNew
		{
			get
			{
				return this.oldWithNew;
			}
		}

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x0600461E RID: 17950 RVA: 0x0019267D File Offset: 0x0019087D
		public virtual CmpCertificate NewWithOld
		{
			get
			{
				return this.newWithOld;
			}
		}

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x0600461F RID: 17951 RVA: 0x00192685 File Offset: 0x00190885
		public virtual CmpCertificate NewWithNew
		{
			get
			{
				return this.newWithNew;
			}
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0019268D File Offset: 0x0019088D
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.oldWithNew,
				this.newWithOld,
				this.newWithNew
			});
		}

		// Token: 0x04002DA0 RID: 11680
		private readonly CmpCertificate oldWithNew;

		// Token: 0x04002DA1 RID: 11681
		private readonly CmpCertificate newWithOld;

		// Token: 0x04002DA2 RID: 11682
		private readonly CmpCertificate newWithNew;
	}
}
