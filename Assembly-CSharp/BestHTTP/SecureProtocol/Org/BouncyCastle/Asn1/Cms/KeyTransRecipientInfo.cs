using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000792 RID: 1938
	public class KeyTransRecipientInfo : Asn1Encodable
	{
		// Token: 0x06004555 RID: 17749 RVA: 0x0019046C File Offset: 0x0018E66C
		public KeyTransRecipientInfo(RecipientIdentifier rid, AlgorithmIdentifier keyEncryptionAlgorithm, Asn1OctetString encryptedKey)
		{
			if (rid.ToAsn1Object() is Asn1TaggedObject)
			{
				this.version = new DerInteger(2);
			}
			else
			{
				this.version = new DerInteger(0);
			}
			this.rid = rid;
			this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x001904BC File Offset: 0x0018E6BC
		public KeyTransRecipientInfo(Asn1Sequence seq)
		{
			this.version = (DerInteger)seq[0];
			this.rid = RecipientIdentifier.GetInstance(seq[1]);
			this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[2]);
			this.encryptedKey = (Asn1OctetString)seq[3];
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x00190517 File Offset: 0x0018E717
		public static KeyTransRecipientInfo GetInstance(object obj)
		{
			if (obj == null || obj is KeyTransRecipientInfo)
			{
				return (KeyTransRecipientInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KeyTransRecipientInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Illegal object in KeyTransRecipientInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x00190554 File Offset: 0x0018E754
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06004559 RID: 17753 RVA: 0x0019055C File Offset: 0x0018E75C
		public RecipientIdentifier RecipientIdentifier
		{
			get
			{
				return this.rid;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x00190564 File Offset: 0x0018E764
		public AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				return this.keyEncryptionAlgorithm;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x0019056C File Offset: 0x0018E76C
		public Asn1OctetString EncryptedKey
		{
			get
			{
				return this.encryptedKey;
			}
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00190574 File Offset: 0x0018E774
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.version,
				this.rid,
				this.keyEncryptionAlgorithm,
				this.encryptedKey
			});
		}

		// Token: 0x04002D56 RID: 11606
		private DerInteger version;

		// Token: 0x04002D57 RID: 11607
		private RecipientIdentifier rid;

		// Token: 0x04002D58 RID: 11608
		private AlgorithmIdentifier keyEncryptionAlgorithm;

		// Token: 0x04002D59 RID: 11609
		private Asn1OctetString encryptedKey;
	}
}
