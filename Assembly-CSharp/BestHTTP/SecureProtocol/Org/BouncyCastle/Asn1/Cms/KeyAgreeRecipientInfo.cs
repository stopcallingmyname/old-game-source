using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000791 RID: 1937
	public class KeyAgreeRecipientInfo : Asn1Encodable
	{
		// Token: 0x0600454B RID: 17739 RVA: 0x001902AD File Offset: 0x0018E4AD
		public KeyAgreeRecipientInfo(OriginatorIdentifierOrKey originator, Asn1OctetString ukm, AlgorithmIdentifier keyEncryptionAlgorithm, Asn1Sequence recipientEncryptedKeys)
		{
			this.version = new DerInteger(3);
			this.originator = originator;
			this.ukm = ukm;
			this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
			this.recipientEncryptedKeys = recipientEncryptedKeys;
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x001902E0 File Offset: 0x0018E4E0
		public KeyAgreeRecipientInfo(Asn1Sequence seq)
		{
			int index = 0;
			this.version = (DerInteger)seq[index++];
			this.originator = OriginatorIdentifierOrKey.GetInstance((Asn1TaggedObject)seq[index++], true);
			if (seq[index] is Asn1TaggedObject)
			{
				this.ukm = Asn1OctetString.GetInstance((Asn1TaggedObject)seq[index++], true);
			}
			this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[index++]);
			this.recipientEncryptedKeys = (Asn1Sequence)seq[index++];
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x0019037D File Offset: 0x0018E57D
		public static KeyAgreeRecipientInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return KeyAgreeRecipientInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x0019038B File Offset: 0x0018E58B
		public static KeyAgreeRecipientInfo GetInstance(object obj)
		{
			if (obj == null || obj is KeyAgreeRecipientInfo)
			{
				return (KeyAgreeRecipientInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KeyAgreeRecipientInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Illegal object in KeyAgreeRecipientInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x0600454F RID: 17743 RVA: 0x001903C8 File Offset: 0x0018E5C8
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x001903D0 File Offset: 0x0018E5D0
		public OriginatorIdentifierOrKey Originator
		{
			get
			{
				return this.originator;
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06004551 RID: 17745 RVA: 0x001903D8 File Offset: 0x0018E5D8
		public Asn1OctetString UserKeyingMaterial
		{
			get
			{
				return this.ukm;
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x001903E0 File Offset: 0x0018E5E0
		public AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				return this.keyEncryptionAlgorithm;
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06004553 RID: 17747 RVA: 0x001903E8 File Offset: 0x0018E5E8
		public Asn1Sequence RecipientEncryptedKeys
		{
			get
			{
				return this.recipientEncryptedKeys;
			}
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x001903F0 File Offset: 0x0018E5F0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				new DerTaggedObject(true, 0, this.originator)
			});
			if (this.ukm != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.ukm)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.keyEncryptionAlgorithm,
				this.recipientEncryptedKeys
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D51 RID: 11601
		private DerInteger version;

		// Token: 0x04002D52 RID: 11602
		private OriginatorIdentifierOrKey originator;

		// Token: 0x04002D53 RID: 11603
		private Asn1OctetString ukm;

		// Token: 0x04002D54 RID: 11604
		private AlgorithmIdentifier keyEncryptionAlgorithm;

		// Token: 0x04002D55 RID: 11605
		private Asn1Sequence recipientEncryptedKeys;
	}
}
