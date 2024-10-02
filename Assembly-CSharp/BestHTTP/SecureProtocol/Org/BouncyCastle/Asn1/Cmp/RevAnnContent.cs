using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CC RID: 1996
	public class RevAnnContent : Asn1Encodable
	{
		// Token: 0x06004705 RID: 18181 RVA: 0x00194C4C File Offset: 0x00192E4C
		private RevAnnContent(Asn1Sequence seq)
		{
			this.status = PkiStatusEncodable.GetInstance(seq[0]);
			this.certId = CertId.GetInstance(seq[1]);
			this.willBeRevokedAt = DerGeneralizedTime.GetInstance(seq[2]);
			this.badSinceDate = DerGeneralizedTime.GetInstance(seq[3]);
			if (seq.Count > 4)
			{
				this.crlDetails = X509Extensions.GetInstance(seq[4]);
			}
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x00194CC2 File Offset: 0x00192EC2
		public static RevAnnContent GetInstance(object obj)
		{
			if (obj is RevAnnContent)
			{
				return (RevAnnContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevAnnContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x00194D01 File Offset: 0x00192F01
		public virtual PkiStatusEncodable Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004708 RID: 18184 RVA: 0x00194D09 File Offset: 0x00192F09
		public virtual CertId CertID
		{
			get
			{
				return this.certId;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06004709 RID: 18185 RVA: 0x00194D11 File Offset: 0x00192F11
		public virtual DerGeneralizedTime WillBeRevokedAt
		{
			get
			{
				return this.willBeRevokedAt;
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600470A RID: 18186 RVA: 0x00194D19 File Offset: 0x00192F19
		public virtual DerGeneralizedTime BadSinceDate
		{
			get
			{
				return this.badSinceDate;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600470B RID: 18187 RVA: 0x00194D21 File Offset: 0x00192F21
		public virtual X509Extensions CrlDetails
		{
			get
			{
				return this.crlDetails;
			}
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00194D2C File Offset: 0x00192F2C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.status,
				this.certId,
				this.willBeRevokedAt,
				this.badSinceDate
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.crlDetails
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002E58 RID: 11864
		private readonly PkiStatusEncodable status;

		// Token: 0x04002E59 RID: 11865
		private readonly CertId certId;

		// Token: 0x04002E5A RID: 11866
		private readonly DerGeneralizedTime willBeRevokedAt;

		// Token: 0x04002E5B RID: 11867
		private readonly DerGeneralizedTime badSinceDate;

		// Token: 0x04002E5C RID: 11868
		private readonly X509Extensions crlDetails;
	}
}
