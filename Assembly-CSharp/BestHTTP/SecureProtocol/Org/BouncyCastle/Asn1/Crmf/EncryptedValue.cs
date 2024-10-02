using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x0200076F RID: 1903
	public class EncryptedValue : Asn1Encodable
	{
		// Token: 0x06004456 RID: 17494 RVA: 0x0018D6B0 File Offset: 0x0018B8B0
		private EncryptedValue(Asn1Sequence seq)
		{
			int num = 0;
			while (seq[num] is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[num];
				switch (asn1TaggedObject.TagNo)
				{
				case 0:
					this.intendedAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, false);
					break;
				case 1:
					this.symmAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, false);
					break;
				case 2:
					this.encSymmKey = DerBitString.GetInstance(asn1TaggedObject, false);
					break;
				case 3:
					this.keyAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, false);
					break;
				case 4:
					this.valueHint = Asn1OctetString.GetInstance(asn1TaggedObject, false);
					break;
				}
				num++;
			}
			this.encValue = DerBitString.GetInstance(seq[num]);
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0018D767 File Offset: 0x0018B967
		public static EncryptedValue GetInstance(object obj)
		{
			if (obj is EncryptedValue)
			{
				return (EncryptedValue)obj;
			}
			if (obj != null)
			{
				return new EncryptedValue(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0018D788 File Offset: 0x0018B988
		public EncryptedValue(AlgorithmIdentifier intendedAlg, AlgorithmIdentifier symmAlg, DerBitString encSymmKey, AlgorithmIdentifier keyAlg, Asn1OctetString valueHint, DerBitString encValue)
		{
			if (encValue == null)
			{
				throw new ArgumentNullException("encValue");
			}
			this.intendedAlg = intendedAlg;
			this.symmAlg = symmAlg;
			this.encSymmKey = encSymmKey;
			this.keyAlg = keyAlg;
			this.valueHint = valueHint;
			this.encValue = encValue;
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06004459 RID: 17497 RVA: 0x0018D7D7 File Offset: 0x0018B9D7
		public virtual AlgorithmIdentifier IntendedAlg
		{
			get
			{
				return this.intendedAlg;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x0600445A RID: 17498 RVA: 0x0018D7DF File Offset: 0x0018B9DF
		public virtual AlgorithmIdentifier SymmAlg
		{
			get
			{
				return this.symmAlg;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x0600445B RID: 17499 RVA: 0x0018D7E7 File Offset: 0x0018B9E7
		public virtual DerBitString EncSymmKey
		{
			get
			{
				return this.encSymmKey;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600445C RID: 17500 RVA: 0x0018D7EF File Offset: 0x0018B9EF
		public virtual AlgorithmIdentifier KeyAlg
		{
			get
			{
				return this.keyAlg;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600445D RID: 17501 RVA: 0x0018D7F7 File Offset: 0x0018B9F7
		public virtual Asn1OctetString ValueHint
		{
			get
			{
				return this.valueHint;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600445E RID: 17502 RVA: 0x0018D7FF File Offset: 0x0018B9FF
		public virtual DerBitString EncValue
		{
			get
			{
				return this.encValue;
			}
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0018D808 File Offset: 0x0018BA08
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			this.AddOptional(asn1EncodableVector, 0, this.intendedAlg);
			this.AddOptional(asn1EncodableVector, 1, this.symmAlg);
			this.AddOptional(asn1EncodableVector, 2, this.encSymmKey);
			this.AddOptional(asn1EncodableVector, 3, this.keyAlg);
			this.AddOptional(asn1EncodableVector, 4, this.valueHint);
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.encValue
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06004460 RID: 17504 RVA: 0x0018D881 File Offset: 0x0018BA81
		private void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, tagNo, obj)
				});
			}
		}

		// Token: 0x04002CD4 RID: 11476
		private readonly AlgorithmIdentifier intendedAlg;

		// Token: 0x04002CD5 RID: 11477
		private readonly AlgorithmIdentifier symmAlg;

		// Token: 0x04002CD6 RID: 11478
		private readonly DerBitString encSymmKey;

		// Token: 0x04002CD7 RID: 11479
		private readonly AlgorithmIdentifier keyAlg;

		// Token: 0x04002CD8 RID: 11480
		private readonly Asn1OctetString valueHint;

		// Token: 0x04002CD9 RID: 11481
		private readonly DerBitString encValue;
	}
}
