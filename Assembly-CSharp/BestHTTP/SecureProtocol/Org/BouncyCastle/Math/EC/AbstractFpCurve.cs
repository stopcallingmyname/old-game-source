using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.Field;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x0200031E RID: 798
	public abstract class AbstractFpCurve : ECCurve
	{
		// Token: 0x06001E41 RID: 7745 RVA: 0x000E2366 File Offset: 0x000E0566
		protected AbstractFpCurve(BigInteger q) : base(FiniteFields.GetPrimeField(q))
		{
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x000E2374 File Offset: 0x000E0574
		public override bool IsValidFieldElement(BigInteger x)
		{
			return x != null && x.SignValue >= 0 && x.CompareTo(this.Field.Characteristic) < 0;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000E2398 File Offset: 0x000E0598
		protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(X1);
			ECFieldElement ecfieldElement2 = ecfieldElement.Square().Add(this.A).Multiply(ecfieldElement).Add(this.B).Sqrt();
			if (ecfieldElement2 == null)
			{
				throw new ArgumentException("Invalid point compression");
			}
			if (ecfieldElement2.TestBitZero() != (yTilde == 1))
			{
				ecfieldElement2 = ecfieldElement2.Negate();
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, true);
		}
	}
}
