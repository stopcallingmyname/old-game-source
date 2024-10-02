using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200079A RID: 1946
	public class PasswordRecipientInfo : Asn1Encodable
	{
		// Token: 0x06004595 RID: 17813 RVA: 0x00190CDF File Offset: 0x0018EEDF
		public PasswordRecipientInfo(AlgorithmIdentifier keyEncryptionAlgorithm, Asn1OctetString encryptedKey)
		{
			this.version = new DerInteger(0);
			this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00190D01 File Offset: 0x0018EF01
		public PasswordRecipientInfo(AlgorithmIdentifier keyDerivationAlgorithm, AlgorithmIdentifier keyEncryptionAlgorithm, Asn1OctetString encryptedKey)
		{
			this.version = new DerInteger(0);
			this.keyDerivationAlgorithm = keyDerivationAlgorithm;
			this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x00190D2C File Offset: 0x0018EF2C
		public PasswordRecipientInfo(Asn1Sequence seq)
		{
			this.version = (DerInteger)seq[0];
			if (seq[1] is Asn1TaggedObject)
			{
				this.keyDerivationAlgorithm = AlgorithmIdentifier.GetInstance((Asn1TaggedObject)seq[1], false);
				this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[2]);
				this.encryptedKey = (Asn1OctetString)seq[3];
				return;
			}
			this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			this.encryptedKey = (Asn1OctetString)seq[2];
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x00190DC0 File Offset: 0x0018EFC0
		public static PasswordRecipientInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return PasswordRecipientInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00190DCE File Offset: 0x0018EFCE
		public static PasswordRecipientInfo GetInstance(object obj)
		{
			if (obj == null || obj is PasswordRecipientInfo)
			{
				return (PasswordRecipientInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PasswordRecipientInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid PasswordRecipientInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x00190E0B File Offset: 0x0018F00B
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x00190E13 File Offset: 0x0018F013
		public AlgorithmIdentifier KeyDerivationAlgorithm
		{
			get
			{
				return this.keyDerivationAlgorithm;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x0600459C RID: 17820 RVA: 0x00190E1B File Offset: 0x0018F01B
		public AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				return this.keyEncryptionAlgorithm;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x00190E23 File Offset: 0x0018F023
		public Asn1OctetString EncryptedKey
		{
			get
			{
				return this.encryptedKey;
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00190E2C File Offset: 0x0018F02C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version
			});
			if (this.keyDerivationAlgorithm != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.keyDerivationAlgorithm)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.keyEncryptionAlgorithm,
				this.encryptedKey
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D69 RID: 11625
		private readonly DerInteger version;

		// Token: 0x04002D6A RID: 11626
		private readonly AlgorithmIdentifier keyDerivationAlgorithm;

		// Token: 0x04002D6B RID: 11627
		private readonly AlgorithmIdentifier keyEncryptionAlgorithm;

		// Token: 0x04002D6C RID: 11628
		private readonly Asn1OctetString encryptedKey;
	}
}
