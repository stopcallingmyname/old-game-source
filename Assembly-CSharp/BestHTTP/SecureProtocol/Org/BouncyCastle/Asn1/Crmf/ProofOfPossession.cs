using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000777 RID: 1911
	public class ProofOfPossession : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004491 RID: 17553 RVA: 0x0018E0A8 File Offset: 0x0018C2A8
		private ProofOfPossession(Asn1TaggedObject tagged)
		{
			this.tagNo = tagged.TagNo;
			switch (this.tagNo)
			{
			case 0:
				this.obj = DerNull.Instance;
				return;
			case 1:
				this.obj = PopoSigningKey.GetInstance(tagged, false);
				return;
			case 2:
			case 3:
				this.obj = PopoPrivKey.GetInstance(tagged, false);
				return;
			default:
				throw new ArgumentException("unknown tag: " + this.tagNo, "tagged");
			}
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x0018E12D File Offset: 0x0018C32D
		public static ProofOfPossession GetInstance(object obj)
		{
			if (obj is ProofOfPossession)
			{
				return (ProofOfPossession)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new ProofOfPossession((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0018E16C File Offset: 0x0018C36C
		public ProofOfPossession()
		{
			this.tagNo = 0;
			this.obj = DerNull.Instance;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x0018E186 File Offset: 0x0018C386
		public ProofOfPossession(PopoSigningKey Poposk)
		{
			this.tagNo = 1;
			this.obj = Poposk;
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x0018E19C File Offset: 0x0018C39C
		public ProofOfPossession(int type, PopoPrivKey privkey)
		{
			this.tagNo = type;
			this.obj = privkey;
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x0018E1B2 File Offset: 0x0018C3B2
		public virtual int Type
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x0018E1BA File Offset: 0x0018C3BA
		public virtual Asn1Encodable Object
		{
			get
			{
				return this.obj;
			}
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x0018E1C2 File Offset: 0x0018C3C2
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(false, this.tagNo, this.obj);
		}

		// Token: 0x04002CF1 RID: 11505
		public const int TYPE_RA_VERIFIED = 0;

		// Token: 0x04002CF2 RID: 11506
		public const int TYPE_SIGNING_KEY = 1;

		// Token: 0x04002CF3 RID: 11507
		public const int TYPE_KEY_ENCIPHERMENT = 2;

		// Token: 0x04002CF4 RID: 11508
		public const int TYPE_KEY_AGREEMENT = 3;

		// Token: 0x04002CF5 RID: 11509
		private readonly int tagNo;

		// Token: 0x04002CF6 RID: 11510
		private readonly Asn1Encodable obj;
	}
}
