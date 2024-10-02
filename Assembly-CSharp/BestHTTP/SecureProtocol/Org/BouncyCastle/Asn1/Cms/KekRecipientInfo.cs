using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x0200078F RID: 1935
	public class KekRecipientInfo : Asn1Encodable
	{
		// Token: 0x0600453B RID: 17723 RVA: 0x001900B6 File Offset: 0x0018E2B6
		public KekRecipientInfo(KekIdentifier kekID, AlgorithmIdentifier keyEncryptionAlgorithm, Asn1OctetString encryptedKey)
		{
			this.version = new DerInteger(4);
			this.kekID = kekID;
			this.keyEncryptionAlgorithm = keyEncryptionAlgorithm;
			this.encryptedKey = encryptedKey;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x001900E0 File Offset: 0x0018E2E0
		public KekRecipientInfo(Asn1Sequence seq)
		{
			this.version = (DerInteger)seq[0];
			this.kekID = KekIdentifier.GetInstance(seq[1]);
			this.keyEncryptionAlgorithm = AlgorithmIdentifier.GetInstance(seq[2]);
			this.encryptedKey = (Asn1OctetString)seq[3];
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x0019013B File Offset: 0x0018E33B
		public static KekRecipientInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return KekRecipientInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x00190149 File Offset: 0x0018E349
		public static KekRecipientInfo GetInstance(object obj)
		{
			if (obj == null || obj is KekRecipientInfo)
			{
				return (KekRecipientInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KekRecipientInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid KekRecipientInfo: " + Platform.GetTypeName(obj));
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x00190186 File Offset: 0x0018E386
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x0019018E File Offset: 0x0018E38E
		public KekIdentifier KekID
		{
			get
			{
				return this.kekID;
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06004541 RID: 17729 RVA: 0x00190196 File Offset: 0x0018E396
		public AlgorithmIdentifier KeyEncryptionAlgorithm
		{
			get
			{
				return this.keyEncryptionAlgorithm;
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06004542 RID: 17730 RVA: 0x0019019E File Offset: 0x0018E39E
		public Asn1OctetString EncryptedKey
		{
			get
			{
				return this.encryptedKey;
			}
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x001901A6 File Offset: 0x0018E3A6
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.version,
				this.kekID,
				this.keyEncryptionAlgorithm,
				this.encryptedKey
			});
		}

		// Token: 0x04002D4B RID: 11595
		private DerInteger version;

		// Token: 0x04002D4C RID: 11596
		private KekIdentifier kekID;

		// Token: 0x04002D4D RID: 11597
		private AlgorithmIdentifier keyEncryptionAlgorithm;

		// Token: 0x04002D4E RID: 11598
		private Asn1OctetString encryptedKey;
	}
}
