using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000763 RID: 1891
	public class Gost3410PublicKeyAlgParameters : Asn1Encodable
	{
		// Token: 0x060043FA RID: 17402 RVA: 0x0018CA7F File Offset: 0x0018AC7F
		public static Gost3410PublicKeyAlgParameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Gost3410PublicKeyAlgParameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0018CA8D File Offset: 0x0018AC8D
		public static Gost3410PublicKeyAlgParameters GetInstance(object obj)
		{
			if (obj == null || obj is Gost3410PublicKeyAlgParameters)
			{
				return (Gost3410PublicKeyAlgParameters)obj;
			}
			return new Gost3410PublicKeyAlgParameters(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0018CAAC File Offset: 0x0018ACAC
		public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet) : this(publicKeyParamSet, digestParamSet, null)
		{
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0018CAB7 File Offset: 0x0018ACB7
		public Gost3410PublicKeyAlgParameters(DerObjectIdentifier publicKeyParamSet, DerObjectIdentifier digestParamSet, DerObjectIdentifier encryptionParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			if (digestParamSet == null)
			{
				throw new ArgumentNullException("digestParamSet");
			}
			this.publicKeyParamSet = publicKeyParamSet;
			this.digestParamSet = digestParamSet;
			this.encryptionParamSet = encryptionParamSet;
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0018CAF0 File Offset: 0x0018ACF0
		public Gost3410PublicKeyAlgParameters(Asn1Sequence seq)
		{
			this.publicKeyParamSet = (DerObjectIdentifier)seq[0];
			this.digestParamSet = (DerObjectIdentifier)seq[1];
			if (seq.Count > 2)
			{
				this.encryptionParamSet = (DerObjectIdentifier)seq[2];
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060043FF RID: 17407 RVA: 0x0018CB42 File Offset: 0x0018AD42
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06004400 RID: 17408 RVA: 0x0018CB4A File Offset: 0x0018AD4A
		public DerObjectIdentifier DigestParamSet
		{
			get
			{
				return this.digestParamSet;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x06004401 RID: 17409 RVA: 0x0018CB52 File Offset: 0x0018AD52
		public DerObjectIdentifier EncryptionParamSet
		{
			get
			{
				return this.encryptionParamSet;
			}
		}

		// Token: 0x06004402 RID: 17410 RVA: 0x0018CB5C File Offset: 0x0018AD5C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.publicKeyParamSet,
				this.digestParamSet
			});
			if (this.encryptionParamSet != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.encryptionParamSet
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CA4 RID: 11428
		private DerObjectIdentifier publicKeyParamSet;

		// Token: 0x04002CA5 RID: 11429
		private DerObjectIdentifier digestParamSet;

		// Token: 0x04002CA6 RID: 11430
		private DerObjectIdentifier encryptionParamSet;
	}
}
