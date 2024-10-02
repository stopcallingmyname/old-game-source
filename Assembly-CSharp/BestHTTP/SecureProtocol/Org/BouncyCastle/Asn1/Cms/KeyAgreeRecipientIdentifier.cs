using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000790 RID: 1936
	public class KeyAgreeRecipientIdentifier : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004544 RID: 17732 RVA: 0x001901D7 File Offset: 0x0018E3D7
		public static KeyAgreeRecipientIdentifier GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return KeyAgreeRecipientIdentifier.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x001901E8 File Offset: 0x0018E3E8
		public static KeyAgreeRecipientIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is KeyAgreeRecipientIdentifier)
			{
				return (KeyAgreeRecipientIdentifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KeyAgreeRecipientIdentifier(IssuerAndSerialNumber.GetInstance(obj));
			}
			if (obj is Asn1TaggedObject && ((Asn1TaggedObject)obj).TagNo == 0)
			{
				return new KeyAgreeRecipientIdentifier(RecipientKeyIdentifier.GetInstance((Asn1TaggedObject)obj, false));
			}
			throw new ArgumentException("Invalid KeyAgreeRecipientIdentifier: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x0019025C File Offset: 0x0018E45C
		public KeyAgreeRecipientIdentifier(IssuerAndSerialNumber issuerSerial)
		{
			this.issuerSerial = issuerSerial;
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0019026B File Offset: 0x0018E46B
		public KeyAgreeRecipientIdentifier(RecipientKeyIdentifier rKeyID)
		{
			this.rKeyID = rKeyID;
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x0019027A File Offset: 0x0018E47A
		public IssuerAndSerialNumber IssuerAndSerialNumber
		{
			get
			{
				return this.issuerSerial;
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06004549 RID: 17737 RVA: 0x00190282 File Offset: 0x0018E482
		public RecipientKeyIdentifier RKeyID
		{
			get
			{
				return this.rKeyID;
			}
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x0019028A File Offset: 0x0018E48A
		public override Asn1Object ToAsn1Object()
		{
			if (this.issuerSerial != null)
			{
				return this.issuerSerial.ToAsn1Object();
			}
			return new DerTaggedObject(false, 0, this.rKeyID);
		}

		// Token: 0x04002D4F RID: 11599
		private readonly IssuerAndSerialNumber issuerSerial;

		// Token: 0x04002D50 RID: 11600
		private readonly RecipientKeyIdentifier rKeyID;
	}
}
