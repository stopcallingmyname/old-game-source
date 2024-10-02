using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000681 RID: 1665
	public class X9FieldID : Asn1Encodable
	{
		// Token: 0x06003DC2 RID: 15810 RVA: 0x00174E6F File Offset: 0x0017306F
		public X9FieldID(BigInteger primeP)
		{
			this.id = X9ObjectIdentifiers.PrimeField;
			this.parameters = new DerInteger(primeP);
		}

		// Token: 0x06003DC3 RID: 15811 RVA: 0x00174E8E File Offset: 0x0017308E
		public X9FieldID(int m, int k1) : this(m, k1, 0, 0)
		{
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x00174E9C File Offset: 0x0017309C
		public X9FieldID(int m, int k1, int k2, int k3)
		{
			this.id = X9ObjectIdentifiers.CharacteristicTwoField;
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(m)
			});
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("inconsistent k values");
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					X9ObjectIdentifiers.TPBasis,
					new DerInteger(k1)
				});
			}
			else
			{
				if (k2 <= k1 || k3 <= k2)
				{
					throw new ArgumentException("inconsistent k values");
				}
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					X9ObjectIdentifiers.PPBasis,
					new DerSequence(new Asn1Encodable[]
					{
						new DerInteger(k1),
						new DerInteger(k2),
						new DerInteger(k3)
					})
				});
			}
			this.parameters = new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06003DC5 RID: 15813 RVA: 0x00174F5E File Offset: 0x0017315E
		private X9FieldID(Asn1Sequence seq)
		{
			this.id = DerObjectIdentifier.GetInstance(seq[0]);
			this.parameters = seq[1].ToAsn1Object();
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x00174F8A File Offset: 0x0017318A
		public static X9FieldID GetInstance(object obj)
		{
			if (obj is X9FieldID)
			{
				return (X9FieldID)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new X9FieldID(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x00174FAB File Offset: 0x001731AB
		public DerObjectIdentifier Identifier
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06003DC8 RID: 15816 RVA: 0x00174FB3 File Offset: 0x001731B3
		public Asn1Object Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x00174FBB File Offset: 0x001731BB
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.id,
				this.parameters
			});
		}

		// Token: 0x0400272E RID: 10030
		private readonly DerObjectIdentifier id;

		// Token: 0x0400272F RID: 10031
		private readonly Asn1Object parameters;
	}
}
