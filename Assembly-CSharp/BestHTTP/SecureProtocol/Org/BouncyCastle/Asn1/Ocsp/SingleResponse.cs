using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000716 RID: 1814
	public class SingleResponse : Asn1Encodable
	{
		// Token: 0x0600422C RID: 16940 RVA: 0x001850E9 File Offset: 0x001832E9
		public SingleResponse(CertID certID, CertStatus certStatus, DerGeneralizedTime thisUpdate, DerGeneralizedTime nextUpdate, X509Extensions singleExtensions)
		{
			this.certID = certID;
			this.certStatus = certStatus;
			this.thisUpdate = thisUpdate;
			this.nextUpdate = nextUpdate;
			this.singleExtensions = singleExtensions;
		}

		// Token: 0x0600422D RID: 16941 RVA: 0x00185118 File Offset: 0x00183318
		public SingleResponse(Asn1Sequence seq)
		{
			this.certID = CertID.GetInstance(seq[0]);
			this.certStatus = CertStatus.GetInstance(seq[1]);
			this.thisUpdate = (DerGeneralizedTime)seq[2];
			if (seq.Count > 4)
			{
				this.nextUpdate = DerGeneralizedTime.GetInstance((Asn1TaggedObject)seq[3], true);
				this.singleExtensions = X509Extensions.GetInstance((Asn1TaggedObject)seq[4], true);
				return;
			}
			if (seq.Count > 3)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[3];
				if (asn1TaggedObject.TagNo == 0)
				{
					this.nextUpdate = DerGeneralizedTime.GetInstance(asn1TaggedObject, true);
					return;
				}
				this.singleExtensions = X509Extensions.GetInstance(asn1TaggedObject, true);
			}
		}

		// Token: 0x0600422E RID: 16942 RVA: 0x001851D4 File Offset: 0x001833D4
		public static SingleResponse GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return SingleResponse.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600422F RID: 16943 RVA: 0x001851E4 File Offset: 0x001833E4
		public static SingleResponse GetInstance(object obj)
		{
			if (obj == null || obj is SingleResponse)
			{
				return (SingleResponse)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SingleResponse((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06004230 RID: 16944 RVA: 0x00185231 File Offset: 0x00183431
		public CertID CertId
		{
			get
			{
				return this.certID;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06004231 RID: 16945 RVA: 0x00185239 File Offset: 0x00183439
		public CertStatus CertStatus
		{
			get
			{
				return this.certStatus;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06004232 RID: 16946 RVA: 0x00185241 File Offset: 0x00183441
		public DerGeneralizedTime ThisUpdate
		{
			get
			{
				return this.thisUpdate;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06004233 RID: 16947 RVA: 0x00185249 File Offset: 0x00183449
		public DerGeneralizedTime NextUpdate
		{
			get
			{
				return this.nextUpdate;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06004234 RID: 16948 RVA: 0x00185251 File Offset: 0x00183451
		public X509Extensions SingleExtensions
		{
			get
			{
				return this.singleExtensions;
			}
		}

		// Token: 0x06004235 RID: 16949 RVA: 0x0018525C File Offset: 0x0018345C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certID,
				this.certStatus,
				this.thisUpdate
			});
			if (this.nextUpdate != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.nextUpdate)
				});
			}
			if (this.singleExtensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.singleExtensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AD3 RID: 10963
		private readonly CertID certID;

		// Token: 0x04002AD4 RID: 10964
		private readonly CertStatus certStatus;

		// Token: 0x04002AD5 RID: 10965
		private readonly DerGeneralizedTime thisUpdate;

		// Token: 0x04002AD6 RID: 10966
		private readonly DerGeneralizedTime nextUpdate;

		// Token: 0x04002AD7 RID: 10967
		private readonly X509Extensions singleExtensions;
	}
}
