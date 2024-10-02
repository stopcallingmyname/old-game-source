using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC
{
	// Token: 0x02000329 RID: 809
	public abstract class ECPointBase : ECPoint
	{
		// Token: 0x06001EFE RID: 7934 RVA: 0x000E439A File Offset: 0x000E259A
		protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x000E43A7 File Offset: 0x000E25A7
		protected internal ECPointBase(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x000E43B8 File Offset: 0x000E25B8
		public override byte[] GetEncoded(bool compressed)
		{
			if (base.IsInfinity)
			{
				return new byte[1];
			}
			ECPoint ecpoint = this.Normalize();
			byte[] encoded = ecpoint.XCoord.GetEncoded();
			if (compressed)
			{
				byte[] array = new byte[encoded.Length + 1];
				array[0] = (ecpoint.CompressionYTilde ? 3 : 2);
				Array.Copy(encoded, 0, array, 1, encoded.Length);
				return array;
			}
			byte[] encoded2 = ecpoint.YCoord.GetEncoded();
			byte[] array2 = new byte[encoded.Length + encoded2.Length + 1];
			array2[0] = 4;
			Array.Copy(encoded, 0, array2, 1, encoded.Length);
			Array.Copy(encoded2, 0, array2, encoded.Length + 1, encoded2.Length);
			return array2;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x000E4454 File Offset: 0x000E2654
		public override ECPoint Multiply(BigInteger k)
		{
			return this.Curve.GetMultiplier().Multiply(this, k);
		}
	}
}
