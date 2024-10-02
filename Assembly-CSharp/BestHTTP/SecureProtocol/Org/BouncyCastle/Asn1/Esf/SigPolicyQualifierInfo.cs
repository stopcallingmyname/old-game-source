using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200075A RID: 1882
	public class SigPolicyQualifierInfo : Asn1Encodable
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x0018BA2C File Offset: 0x00189C2C
		public static SigPolicyQualifierInfo GetInstance(object obj)
		{
			if (obj == null || obj is SigPolicyQualifierInfo)
			{
				return (SigPolicyQualifierInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SigPolicyQualifierInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'SigPolicyQualifierInfo' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0018BA7C File Offset: 0x00189C7C
		private SigPolicyQualifierInfo(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.sigPolicyQualifierId = (DerObjectIdentifier)seq[0].ToAsn1Object();
			this.sigQualifier = seq[1].ToAsn1Object();
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0018BAEF File Offset: 0x00189CEF
		public SigPolicyQualifierInfo(DerObjectIdentifier sigPolicyQualifierId, Asn1Encodable sigQualifier)
		{
			this.sigPolicyQualifierId = sigPolicyQualifierId;
			this.sigQualifier = sigQualifier.ToAsn1Object();
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x0018BB0A File Offset: 0x00189D0A
		public DerObjectIdentifier SigPolicyQualifierId
		{
			get
			{
				return this.sigPolicyQualifierId;
			}
		}

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x0018BB12 File Offset: 0x00189D12
		public Asn1Object SigQualifier
		{
			get
			{
				return this.sigQualifier;
			}
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0018BB1A File Offset: 0x00189D1A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.sigPolicyQualifierId,
				this.sigQualifier
			});
		}

		// Token: 0x04002C5C RID: 11356
		private readonly DerObjectIdentifier sigPolicyQualifierId;

		// Token: 0x04002C5D RID: 11357
		private readonly Asn1Object sigQualifier;
	}
}
