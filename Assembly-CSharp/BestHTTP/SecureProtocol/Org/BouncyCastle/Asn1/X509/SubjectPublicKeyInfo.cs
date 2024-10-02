using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B2 RID: 1714
	public class SubjectPublicKeyInfo : Asn1Encodable
	{
		// Token: 0x06003F33 RID: 16179 RVA: 0x00179B46 File Offset: 0x00177D46
		public static SubjectPublicKeyInfo GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return SubjectPublicKeyInfo.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x00179B54 File Offset: 0x00177D54
		public static SubjectPublicKeyInfo GetInstance(object obj)
		{
			if (obj is SubjectPublicKeyInfo)
			{
				return (SubjectPublicKeyInfo)obj;
			}
			if (obj != null)
			{
				return new SubjectPublicKeyInfo(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00179B75 File Offset: 0x00177D75
		public SubjectPublicKeyInfo(AlgorithmIdentifier algID, Asn1Encodable publicKey)
		{
			this.keyData = new DerBitString(publicKey);
			this.algID = algID;
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x00179B90 File Offset: 0x00177D90
		public SubjectPublicKeyInfo(AlgorithmIdentifier algID, byte[] publicKey)
		{
			this.keyData = new DerBitString(publicKey);
			this.algID = algID;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00179BAC File Offset: 0x00177DAC
		private SubjectPublicKeyInfo(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.algID = AlgorithmIdentifier.GetInstance(seq[0]);
			this.keyData = DerBitString.GetInstance(seq[1]);
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06003F38 RID: 16184 RVA: 0x00179C0C File Offset: 0x00177E0C
		public AlgorithmIdentifier AlgorithmID
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x00179C14 File Offset: 0x00177E14
		public Asn1Object GetPublicKey()
		{
			return Asn1Object.FromByteArray(this.keyData.GetOctets());
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x00179C26 File Offset: 0x00177E26
		public DerBitString PublicKeyData
		{
			get
			{
				return this.keyData;
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x00179C2E File Offset: 0x00177E2E
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.algID,
				this.keyData
			});
		}

		// Token: 0x04002815 RID: 10261
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04002816 RID: 10262
		private readonly DerBitString keyData;
	}
}
