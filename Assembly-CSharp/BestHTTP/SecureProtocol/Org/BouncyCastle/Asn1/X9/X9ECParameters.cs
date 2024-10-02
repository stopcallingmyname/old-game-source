using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200067D RID: 1661
	public class X9ECParameters : Asn1Encodable
	{
		// Token: 0x06003DA2 RID: 15778 RVA: 0x001749D4 File Offset: 0x00172BD4
		public static X9ECParameters GetInstance(object obj)
		{
			if (obj is X9ECParameters)
			{
				return (X9ECParameters)obj;
			}
			if (obj != null)
			{
				return new X9ECParameters(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x001749F8 File Offset: 0x00172BF8
		public X9ECParameters(Asn1Sequence seq)
		{
			if (!(seq[0] is DerInteger) || !((DerInteger)seq[0]).Value.Equals(BigInteger.One))
			{
				throw new ArgumentException("bad version in X9ECParameters");
			}
			this.n = ((DerInteger)seq[4]).Value;
			if (seq.Count == 6)
			{
				this.h = ((DerInteger)seq[5]).Value;
			}
			X9Curve x9Curve = new X9Curve(X9FieldID.GetInstance(seq[1]), this.n, this.h, Asn1Sequence.GetInstance(seq[2]));
			this.curve = x9Curve.Curve;
			object obj = seq[3];
			if (obj is X9ECPoint)
			{
				this.g = (X9ECPoint)obj;
			}
			else
			{
				this.g = new X9ECPoint(this.curve, (Asn1OctetString)obj);
			}
			this.seed = x9Curve.GetSeed();
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00174AEF File Offset: 0x00172CEF
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, null, null)
		{
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00174AFC File Offset: 0x00172CFC
		public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00174B0A File Offset: 0x00172D0A
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00174B18 File Offset: 0x00172D18
		public X9ECParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed) : this(curve, new X9ECPoint(g), n, h, seed)
		{
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00174B2C File Offset: 0x00172D2C
		public X9ECParameters(ECCurve curve, X9ECPoint g, BigInteger n, BigInteger h, byte[] seed)
		{
			this.curve = curve;
			this.g = g;
			this.n = n;
			this.h = h;
			this.seed = seed;
			if (ECAlgorithms.IsFpCurve(curve))
			{
				this.fieldID = new X9FieldID(curve.Field.Characteristic);
				return;
			}
			if (!ECAlgorithms.IsF2mCurve(curve))
			{
				throw new ArgumentException("'curve' is of an unsupported type");
			}
			int[] exponentsPresent = ((IPolynomialExtensionField)curve.Field).MinimalPolynomial.GetExponentsPresent();
			if (exponentsPresent.Length == 3)
			{
				this.fieldID = new X9FieldID(exponentsPresent[2], exponentsPresent[1]);
				return;
			}
			if (exponentsPresent.Length == 5)
			{
				this.fieldID = new X9FieldID(exponentsPresent[4], exponentsPresent[1], exponentsPresent[2], exponentsPresent[3]);
				return;
			}
			throw new ArgumentException("Only trinomial and pentomial curves are supported");
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x00174BEC File Offset: 0x00172DEC
		public ECCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x00174BF4 File Offset: 0x00172DF4
		public ECPoint G
		{
			get
			{
				return this.g.Point;
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x00174C01 File Offset: 0x00172E01
		public BigInteger N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x00174C09 File Offset: 0x00172E09
		public BigInteger H
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x00174C11 File Offset: 0x00172E11
		public byte[] GetSeed()
		{
			return this.seed;
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x00174C19 File Offset: 0x00172E19
		public X9Curve CurveEntry
		{
			get
			{
				return new X9Curve(this.curve, this.seed);
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06003DAF RID: 15791 RVA: 0x00174C2C File Offset: 0x00172E2C
		public X9FieldID FieldIDEntry
		{
			get
			{
				return this.fieldID;
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06003DB0 RID: 15792 RVA: 0x00174C34 File Offset: 0x00172E34
		public X9ECPoint BaseEntry
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x00174C3C File Offset: 0x00172E3C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				new DerInteger(BigInteger.One),
				this.fieldID,
				new X9Curve(this.curve, this.seed),
				this.g,
				new DerInteger(this.n)
			});
			if (this.h != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerInteger(this.h)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002723 RID: 10019
		private X9FieldID fieldID;

		// Token: 0x04002724 RID: 10020
		private ECCurve curve;

		// Token: 0x04002725 RID: 10021
		private X9ECPoint g;

		// Token: 0x04002726 RID: 10022
		private BigInteger n;

		// Token: 0x04002727 RID: 10023
		private BigInteger h;

		// Token: 0x04002728 RID: 10024
		private byte[] seed;
	}
}
