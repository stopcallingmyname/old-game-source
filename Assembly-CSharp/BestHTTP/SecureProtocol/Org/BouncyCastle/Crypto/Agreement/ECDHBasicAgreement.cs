using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005C6 RID: 1478
	public class ECDHBasicAgreement : IBasicAgreement
	{
		// Token: 0x060038D3 RID: 14547 RVA: 0x00164634 File Offset: 0x00162834
		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.privKey = (ECPrivateKeyParameters)parameters;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x00164657 File Offset: 0x00162857
		public virtual int GetFieldSize()
		{
			return (this.privKey.Parameters.Curve.FieldSize + 7) / 8;
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x00164674 File Offset: 0x00162874
		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)pubKey;
			ECDomainParameters parameters = this.privKey.Parameters;
			if (!parameters.Equals(ecpublicKeyParameters.Parameters))
			{
				throw new InvalidOperationException("ECDH public key has wrong domain parameters");
			}
			BigInteger bigInteger = this.privKey.D;
			ECPoint ecpoint = ECAlgorithms.CleanPoint(parameters.Curve, ecpublicKeyParameters.Q);
			if (ecpoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid public key for ECDH");
			}
			BigInteger h = parameters.H;
			if (!h.Equals(BigInteger.One))
			{
				bigInteger = parameters.HInv.Multiply(bigInteger).Mod(parameters.N);
				ecpoint = ECAlgorithms.ReferenceMultiply(ecpoint, h);
			}
			ECPoint ecpoint2 = ecpoint.Multiply(bigInteger).Normalize();
			if (ecpoint2.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for ECDH");
			}
			return ecpoint2.AffineXCoord.ToBigInteger();
		}

		// Token: 0x04002533 RID: 9523
		protected internal ECPrivateKeyParameters privKey;
	}
}
