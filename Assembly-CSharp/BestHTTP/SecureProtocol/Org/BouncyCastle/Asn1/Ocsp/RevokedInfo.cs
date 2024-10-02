using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000713 RID: 1811
	public class RevokedInfo : Asn1Encodable
	{
		// Token: 0x06004212 RID: 16914 RVA: 0x00184D34 File Offset: 0x00182F34
		public static RevokedInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return RevokedInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x00184D44 File Offset: 0x00182F44
		public static RevokedInfo GetInstance(object obj)
		{
			if (obj == null || obj is RevokedInfo)
			{
				return (RevokedInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevokedInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004214 RID: 16916 RVA: 0x00184D91 File Offset: 0x00182F91
		public RevokedInfo(DerGeneralizedTime revocationTime) : this(revocationTime, null)
		{
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x00184D9B File Offset: 0x00182F9B
		public RevokedInfo(DerGeneralizedTime revocationTime, CrlReason revocationReason)
		{
			if (revocationTime == null)
			{
				throw new ArgumentNullException("revocationTime");
			}
			this.revocationTime = revocationTime;
			this.revocationReason = revocationReason;
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00184DBF File Offset: 0x00182FBF
		private RevokedInfo(Asn1Sequence seq)
		{
			this.revocationTime = (DerGeneralizedTime)seq[0];
			if (seq.Count > 1)
			{
				this.revocationReason = new CrlReason(DerEnumerated.GetInstance((Asn1TaggedObject)seq[1], true));
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06004217 RID: 16919 RVA: 0x00184DFF File Offset: 0x00182FFF
		public DerGeneralizedTime RevocationTime
		{
			get
			{
				return this.revocationTime;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06004218 RID: 16920 RVA: 0x00184E07 File Offset: 0x00183007
		public CrlReason RevocationReason
		{
			get
			{
				return this.revocationReason;
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00184E10 File Offset: 0x00183010
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.revocationTime
			});
			if (this.revocationReason != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.revocationReason)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002ACC RID: 10956
		private readonly DerGeneralizedTime revocationTime;

		// Token: 0x04002ACD RID: 10957
		private readonly CrlReason revocationReason;
	}
}
