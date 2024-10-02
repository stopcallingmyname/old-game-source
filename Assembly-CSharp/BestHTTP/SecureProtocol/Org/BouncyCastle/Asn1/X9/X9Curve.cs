using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200067C RID: 1660
	public class X9Curve : Asn1Encodable
	{
		// Token: 0x06003D9B RID: 15771 RVA: 0x001746A7 File Offset: 0x001728A7
		public X9Curve(ECCurve curve) : this(curve, null)
		{
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x001746B4 File Offset: 0x001728B4
		public X9Curve(ECCurve curve, byte[] seed)
		{
			if (curve == null)
			{
				throw new ArgumentNullException("curve");
			}
			this.curve = curve;
			this.seed = Arrays.Clone(seed);
			if (ECAlgorithms.IsFpCurve(curve))
			{
				this.fieldIdentifier = X9ObjectIdentifiers.PrimeField;
				return;
			}
			if (ECAlgorithms.IsF2mCurve(curve))
			{
				this.fieldIdentifier = X9ObjectIdentifiers.CharacteristicTwoField;
				return;
			}
			throw new ArgumentException("This type of ECCurve is not implemented");
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x0017471A File Offset: 0x0017291A
		[Obsolete("Use constructor including order/cofactor")]
		public X9Curve(X9FieldID fieldID, Asn1Sequence seq) : this(fieldID, null, null, seq)
		{
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x00174728 File Offset: 0x00172928
		public X9Curve(X9FieldID fieldID, BigInteger order, BigInteger cofactor, Asn1Sequence seq)
		{
			if (fieldID == null)
			{
				throw new ArgumentNullException("fieldID");
			}
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			this.fieldIdentifier = fieldID.Identifier;
			if (this.fieldIdentifier.Equals(X9ObjectIdentifiers.PrimeField))
			{
				BigInteger value = ((DerInteger)fieldID.Parameters).Value;
				BigInteger a = new BigInteger(1, Asn1OctetString.GetInstance(seq[0]).GetOctets());
				BigInteger b = new BigInteger(1, Asn1OctetString.GetInstance(seq[1]).GetOctets());
				this.curve = new FpCurve(value, a, b, order, cofactor);
			}
			else
			{
				if (!this.fieldIdentifier.Equals(X9ObjectIdentifiers.CharacteristicTwoField))
				{
					throw new ArgumentException("This type of ECCurve is not implemented");
				}
				DerSequence derSequence = (DerSequence)fieldID.Parameters;
				int intValue = ((DerInteger)derSequence[0]).Value.IntValue;
				object obj = (DerObjectIdentifier)derSequence[1];
				int k = 0;
				int k2 = 0;
				int intValue2;
				if (obj.Equals(X9ObjectIdentifiers.TPBasis))
				{
					intValue2 = ((DerInteger)derSequence[2]).Value.IntValue;
				}
				else
				{
					DerSequence derSequence2 = (DerSequence)derSequence[2];
					intValue2 = ((DerInteger)derSequence2[0]).Value.IntValue;
					k = ((DerInteger)derSequence2[1]).Value.IntValue;
					k2 = ((DerInteger)derSequence2[2]).Value.IntValue;
				}
				BigInteger a2 = new BigInteger(1, Asn1OctetString.GetInstance(seq[0]).GetOctets());
				BigInteger b2 = new BigInteger(1, Asn1OctetString.GetInstance(seq[1]).GetOctets());
				this.curve = new F2mCurve(intValue, intValue2, k, k2, a2, b2, order, cofactor);
			}
			if (seq.Count == 3)
			{
				this.seed = ((DerBitString)seq[2]).GetBytes();
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x00174911 File Offset: 0x00172B11
		public ECCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x06003DA0 RID: 15776 RVA: 0x00174919 File Offset: 0x00172B19
		public byte[] GetSeed()
		{
			return Arrays.Clone(this.seed);
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00174928 File Offset: 0x00172B28
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.fieldIdentifier.Equals(X9ObjectIdentifiers.PrimeField) || this.fieldIdentifier.Equals(X9ObjectIdentifiers.CharacteristicTwoField))
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new X9FieldElement(this.curve.A).ToAsn1Object()
				});
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new X9FieldElement(this.curve.B).ToAsn1Object()
				});
			}
			if (this.seed != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerBitString(this.seed)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002720 RID: 10016
		private readonly ECCurve curve;

		// Token: 0x04002721 RID: 10017
		private readonly byte[] seed;

		// Token: 0x04002722 RID: 10018
		private readonly DerObjectIdentifier fieldIdentifier;
	}
}
