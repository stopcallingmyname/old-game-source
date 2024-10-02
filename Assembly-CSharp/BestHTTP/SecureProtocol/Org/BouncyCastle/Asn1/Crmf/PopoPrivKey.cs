using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000774 RID: 1908
	public class PopoPrivKey : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600447B RID: 17531 RVA: 0x0018DCB8 File Offset: 0x0018BEB8
		private PopoPrivKey(Asn1TaggedObject obj)
		{
			this.tagNo = obj.TagNo;
			switch (this.tagNo)
			{
			case 0:
				this.obj = DerBitString.GetInstance(obj, false);
				return;
			case 1:
				this.obj = SubsequentMessage.ValueOf(DerInteger.GetInstance(obj, false).Value.IntValue);
				return;
			case 2:
				this.obj = DerBitString.GetInstance(obj, false);
				return;
			case 3:
				this.obj = PKMacValue.GetInstance(obj, false);
				return;
			case 4:
				this.obj = EnvelopedData.GetInstance(obj, false);
				return;
			default:
				throw new ArgumentException("unknown tag in PopoPrivKey", "obj");
			}
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x0018DD5E File Offset: 0x0018BF5E
		public static PopoPrivKey GetInstance(Asn1TaggedObject tagged, bool isExplicit)
		{
			return new PopoPrivKey(Asn1TaggedObject.GetInstance(tagged.GetObject()));
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x0018DD70 File Offset: 0x0018BF70
		public PopoPrivKey(SubsequentMessage msg)
		{
			this.tagNo = 1;
			this.obj = msg;
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x0018DD86 File Offset: 0x0018BF86
		public virtual int Type
		{
			get
			{
				return this.tagNo;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600447F RID: 17535 RVA: 0x0018DD8E File Offset: 0x0018BF8E
		public virtual Asn1Encodable Value
		{
			get
			{
				return this.obj;
			}
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x0018DD96 File Offset: 0x0018BF96
		public override Asn1Object ToAsn1Object()
		{
			return new DerTaggedObject(false, this.tagNo, this.obj);
		}

		// Token: 0x04002CE4 RID: 11492
		public const int thisMessage = 0;

		// Token: 0x04002CE5 RID: 11493
		public const int subsequentMessage = 1;

		// Token: 0x04002CE6 RID: 11494
		public const int dhMAC = 2;

		// Token: 0x04002CE7 RID: 11495
		public const int agreeMAC = 3;

		// Token: 0x04002CE8 RID: 11496
		public const int encryptedKey = 4;

		// Token: 0x04002CE9 RID: 11497
		private readonly int tagNo;

		// Token: 0x04002CEA RID: 11498
		private readonly Asn1Encodable obj;
	}
}
