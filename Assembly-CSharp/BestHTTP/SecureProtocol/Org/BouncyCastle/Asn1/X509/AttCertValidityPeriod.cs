using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000687 RID: 1671
	public class AttCertValidityPeriod : Asn1Encodable
	{
		// Token: 0x06003DE8 RID: 15848 RVA: 0x0017583C File Offset: 0x00173A3C
		public static AttCertValidityPeriod GetInstance(object obj)
		{
			if (obj is AttCertValidityPeriod || obj == null)
			{
				return (AttCertValidityPeriod)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AttCertValidityPeriod((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00175889 File Offset: 0x00173A89
		public static AttCertValidityPeriod GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return AttCertValidityPeriod.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x00175898 File Offset: 0x00173A98
		private AttCertValidityPeriod(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.notBeforeTime = DerGeneralizedTime.GetInstance(seq[0]);
			this.notAfterTime = DerGeneralizedTime.GetInstance(seq[1]);
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x001758F3 File Offset: 0x00173AF3
		public AttCertValidityPeriod(DerGeneralizedTime notBeforeTime, DerGeneralizedTime notAfterTime)
		{
			this.notBeforeTime = notBeforeTime;
			this.notAfterTime = notAfterTime;
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x00175909 File Offset: 0x00173B09
		public DerGeneralizedTime NotBeforeTime
		{
			get
			{
				return this.notBeforeTime;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06003DED RID: 15853 RVA: 0x00175911 File Offset: 0x00173B11
		public DerGeneralizedTime NotAfterTime
		{
			get
			{
				return this.notAfterTime;
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x00175919 File Offset: 0x00173B19
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.notBeforeTime,
				this.notAfterTime
			});
		}

		// Token: 0x0400277A RID: 10106
		private readonly DerGeneralizedTime notBeforeTime;

		// Token: 0x0400277B RID: 10107
		private readonly DerGeneralizedTime notAfterTime;
	}
}
