using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005C7 RID: 1479
	public class ECDHCBasicAgreement : IBasicAgreement
	{
		// Token: 0x060038D7 RID: 14551 RVA: 0x0016473E File Offset: 0x0016293E
		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.privKey = (ECPrivateKeyParameters)parameters;
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x00164761 File Offset: 0x00162961
		public virtual int GetFieldSize()
		{
			return (this.privKey.Parameters.Curve.FieldSize + 7) / 8;
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0016477C File Offset: 0x0016297C
		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)pubKey;
			ECDomainParameters parameters = this.privKey.Parameters;
			if (!parameters.Equals(ecpublicKeyParameters.Parameters))
			{
				throw new InvalidOperationException("ECDHC public key has wrong domain parameters");
			}
			BigInteger b = parameters.H.Multiply(this.privKey.D).Mod(parameters.N);
			ECPoint ecpoint = ECAlgorithms.CleanPoint(parameters.Curve, ecpublicKeyParameters.Q);
			if (ecpoint.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid public key for ECDHC");
			}
			ECPoint ecpoint2 = ecpoint.Multiply(b).Normalize();
			if (ecpoint2.IsInfinity)
			{
				throw new InvalidOperationException("Infinity is not a valid agreement value for ECDHC");
			}
			return ecpoint2.AffineXCoord.ToBigInteger();
		}

		// Token: 0x04002534 RID: 9524
		private ECPrivateKeyParameters privKey;
	}
}
