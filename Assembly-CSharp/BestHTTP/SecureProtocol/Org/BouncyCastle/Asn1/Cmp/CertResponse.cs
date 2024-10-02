using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AF RID: 1967
	public class CertResponse : Asn1Encodable
	{
		// Token: 0x0600463A RID: 17978 RVA: 0x00192BB8 File Offset: 0x00190DB8
		private CertResponse(Asn1Sequence seq)
		{
			this.certReqId = DerInteger.GetInstance(seq[0]);
			this.status = PkiStatusInfo.GetInstance(seq[1]);
			if (seq.Count >= 3)
			{
				if (seq.Count == 3)
				{
					Asn1Encodable asn1Encodable = seq[2];
					if (asn1Encodable is Asn1OctetString)
					{
						this.rspInfo = Asn1OctetString.GetInstance(asn1Encodable);
						return;
					}
					this.certifiedKeyPair = CertifiedKeyPair.GetInstance(asn1Encodable);
					return;
				}
				else
				{
					this.certifiedKeyPair = CertifiedKeyPair.GetInstance(seq[2]);
					this.rspInfo = Asn1OctetString.GetInstance(seq[3]);
				}
			}
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x00192C4F File Offset: 0x00190E4F
		public static CertResponse GetInstance(object obj)
		{
			if (obj is CertResponse)
			{
				return (CertResponse)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertResponse((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x00192C8E File Offset: 0x00190E8E
		public CertResponse(DerInteger certReqId, PkiStatusInfo status) : this(certReqId, status, null, null)
		{
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x00192C9C File Offset: 0x00190E9C
		public CertResponse(DerInteger certReqId, PkiStatusInfo status, CertifiedKeyPair certifiedKeyPair, Asn1OctetString rspInfo)
		{
			if (certReqId == null)
			{
				throw new ArgumentNullException("certReqId");
			}
			if (status == null)
			{
				throw new ArgumentNullException("status");
			}
			this.certReqId = certReqId;
			this.status = status;
			this.certifiedKeyPair = certifiedKeyPair;
			this.rspInfo = rspInfo;
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x00192CE8 File Offset: 0x00190EE8
		public virtual DerInteger CertReqID
		{
			get
			{
				return this.certReqId;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x00192CF0 File Offset: 0x00190EF0
		public virtual PkiStatusInfo Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x00192CF8 File Offset: 0x00190EF8
		public virtual CertifiedKeyPair CertifiedKeyPair
		{
			get
			{
				return this.certifiedKeyPair;
			}
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x00192D00 File Offset: 0x00190F00
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certReqId,
				this.status
			});
			asn1EncodableVector.AddOptional(new Asn1Encodable[]
			{
				this.certifiedKeyPair,
				this.rspInfo
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DAB RID: 11691
		private readonly DerInteger certReqId;

		// Token: 0x04002DAC RID: 11692
		private readonly PkiStatusInfo status;

		// Token: 0x04002DAD RID: 11693
		private readonly CertifiedKeyPair certifiedKeyPair;

		// Token: 0x04002DAE RID: 11694
		private readonly Asn1OctetString rspInfo;
	}
}
