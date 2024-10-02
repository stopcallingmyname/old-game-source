using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079B RID: 1947
	public class RecipientEncryptedKey : Asn1Encodable
	{
		// Token: 0x0600459F RID: 17823 RVA: 0x00190E96 File Offset: 0x0018F096
		private RecipientEncryptedKey(Asn1Sequence seq)
		{
			this.identifier = KeyAgreeRecipientIdentifier.GetInstance(seq[0]);
			this.encryptedKey = (Asn1OctetString)seq[1];
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00190EC2 File Offset: 0x0018F0C2
		public static RecipientEncryptedKey GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return RecipientEncryptedKey.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00190ED0 File Offset: 0x0018F0D0
		public static RecipientEncryptedKey GetInstance(object obj)
		{
			if (obj == null || obj is RecipientEncryptedKey)
			{
				return (RecipientEncryptedKey)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RecipientEncryptedKey((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid RecipientEncryptedKey: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00190F1D File Offset: 0x0018F11D
		public RecipientEncryptedKey(KeyAgreeRecipientIdentifier id, Asn1OctetString encryptedKey)
		{
			this.identifier = id;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060045A3 RID: 17827 RVA: 0x00190F33 File Offset: 0x0018F133
		public KeyAgreeRecipientIdentifier Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060045A4 RID: 17828 RVA: 0x00190F3B File Offset: 0x0018F13B
		public Asn1OctetString EncryptedKey
		{
			get
			{
				return this.encryptedKey;
			}
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x00190F43 File Offset: 0x0018F143
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.identifier,
				this.encryptedKey
			});
		}

		// Token: 0x04002D6D RID: 11629
		private readonly KeyAgreeRecipientIdentifier identifier;

		// Token: 0x04002D6E RID: 11630
		private readonly Asn1OctetString encryptedKey;
	}
}
