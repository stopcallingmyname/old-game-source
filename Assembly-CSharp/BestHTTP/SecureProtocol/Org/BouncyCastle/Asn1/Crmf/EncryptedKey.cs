using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076E RID: 1902
	public class EncryptedKey : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004450 RID: 17488 RVA: 0x0018D5F8 File Offset: 0x0018B7F8
		public static EncryptedKey GetInstance(object o)
		{
			if (o is EncryptedKey)
			{
				return (EncryptedKey)o;
			}
			if (o is Asn1TaggedObject)
			{
				return new EncryptedKey(EnvelopedData.GetInstance((Asn1TaggedObject)o, false));
			}
			if (o is EncryptedValue)
			{
				return new EncryptedKey((EncryptedValue)o);
			}
			return new EncryptedKey(EncryptedValue.GetInstance(o));
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0018D64D File Offset: 0x0018B84D
		public EncryptedKey(EnvelopedData envelopedData)
		{
			this.envelopedData = envelopedData;
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0018D65C File Offset: 0x0018B85C
		public EncryptedKey(EncryptedValue encryptedValue)
		{
			this.encryptedValue = encryptedValue;
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06004453 RID: 17491 RVA: 0x0018D66B File Offset: 0x0018B86B
		public virtual bool IsEncryptedValue
		{
			get
			{
				return this.encryptedValue != null;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06004454 RID: 17492 RVA: 0x0018D676 File Offset: 0x0018B876
		public virtual Asn1Encodable Value
		{
			get
			{
				if (this.encryptedValue != null)
				{
					return this.encryptedValue;
				}
				return this.envelopedData;
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0018D68D File Offset: 0x0018B88D
		public override Asn1Object ToAsn1Object()
		{
			if (this.encryptedValue != null)
			{
				return this.encryptedValue.ToAsn1Object();
			}
			return new DerTaggedObject(false, 0, this.envelopedData);
		}

		// Token: 0x04002CD2 RID: 11474
		private readonly EnvelopedData envelopedData;

		// Token: 0x04002CD3 RID: 11475
		private readonly EncryptedValue encryptedValue;
	}
}
