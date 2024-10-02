using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000771 RID: 1905
	public class PkiArchiveOptions : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06004466 RID: 17510 RVA: 0x0018D9AE File Offset: 0x0018BBAE
		public static PkiArchiveOptions GetInstance(object obj)
		{
			if (obj is PkiArchiveOptions)
			{
				return (PkiArchiveOptions)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return new PkiArchiveOptions((Asn1TaggedObject)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0018D9F0 File Offset: 0x0018BBF0
		private PkiArchiveOptions(Asn1TaggedObject tagged)
		{
			switch (tagged.TagNo)
			{
			case 0:
				this.value = EncryptedKey.GetInstance(tagged.GetObject());
				return;
			case 1:
				this.value = Asn1OctetString.GetInstance(tagged, false);
				return;
			case 2:
				this.value = DerBoolean.GetInstance(tagged, false);
				return;
			default:
				throw new ArgumentException("unknown tag number: " + tagged.TagNo, "tagged");
			}
		}

		// Token: 0x06004468 RID: 17512 RVA: 0x0018DA6B File Offset: 0x0018BC6B
		public PkiArchiveOptions(EncryptedKey encKey)
		{
			this.value = encKey;
		}

		// Token: 0x06004469 RID: 17513 RVA: 0x0018DA6B File Offset: 0x0018BC6B
		public PkiArchiveOptions(Asn1OctetString keyGenParameters)
		{
			this.value = keyGenParameters;
		}

		// Token: 0x0600446A RID: 17514 RVA: 0x0018DA7A File Offset: 0x0018BC7A
		public PkiArchiveOptions(bool archiveRemGenPrivKey)
		{
			this.value = DerBoolean.GetInstance(archiveRemGenPrivKey);
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x0600446B RID: 17515 RVA: 0x0018DA8E File Offset: 0x0018BC8E
		public virtual int Type
		{
			get
			{
				if (this.value is EncryptedKey)
				{
					return 0;
				}
				if (this.value is Asn1OctetString)
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x0018DAAF File Offset: 0x0018BCAF
		public virtual Asn1Encodable Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0018DAB8 File Offset: 0x0018BCB8
		public override Asn1Object ToAsn1Object()
		{
			if (this.value is EncryptedKey)
			{
				return new DerTaggedObject(true, 0, this.value);
			}
			if (this.value is Asn1OctetString)
			{
				return new DerTaggedObject(false, 1, this.value);
			}
			return new DerTaggedObject(false, 2, this.value);
		}

		// Token: 0x04002CDC RID: 11484
		public const int encryptedPrivKey = 0;

		// Token: 0x04002CDD RID: 11485
		public const int keyGenParameters = 1;

		// Token: 0x04002CDE RID: 11486
		public const int archiveRemGenPrivKey = 2;

		// Token: 0x04002CDF RID: 11487
		private readonly Asn1Encodable value;
	}
}
