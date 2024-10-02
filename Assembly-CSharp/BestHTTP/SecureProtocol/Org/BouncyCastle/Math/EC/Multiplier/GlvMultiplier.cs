using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Endo;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x0200033F RID: 831
	public class GlvMultiplier : AbstractECMultiplier
	{
		// Token: 0x0600204E RID: 8270 RVA: 0x000EF6CC File Offset: 0x000ED8CC
		public GlvMultiplier(ECCurve curve, GlvEndomorphism glvEndomorphism)
		{
			if (curve == null || curve.Order == null)
			{
				throw new ArgumentException("Need curve with known group order", "curve");
			}
			this.curve = curve;
			this.glvEndomorphism = glvEndomorphism;
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000EF700 File Offset: 0x000ED900
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			if (!this.curve.Equals(p.Curve))
			{
				throw new InvalidOperationException();
			}
			BigInteger order = p.Curve.Order;
			BigInteger[] array = this.glvEndomorphism.DecomposeScalar(k.Mod(order));
			BigInteger k2 = array[0];
			BigInteger l = array[1];
			ECPointMap pointMap = this.glvEndomorphism.PointMap;
			if (this.glvEndomorphism.HasEfficientPointMap)
			{
				return ECAlgorithms.ImplShamirsTrickWNaf(p, k2, pointMap, l);
			}
			return ECAlgorithms.ImplShamirsTrickWNaf(p, k2, pointMap.Map(p), l);
		}

		// Token: 0x040019DF RID: 6623
		protected readonly ECCurve curve;

		// Token: 0x040019E0 RID: 6624
		protected readonly GlvEndomorphism glvEndomorphism;
	}
}
