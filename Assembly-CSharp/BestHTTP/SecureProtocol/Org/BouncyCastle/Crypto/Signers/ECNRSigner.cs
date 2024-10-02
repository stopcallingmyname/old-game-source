using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000496 RID: 1174
	public class ECNRSigner : IDsaExt, IDsa
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x0011F702 File Offset: 0x0011D902
		public virtual string AlgorithmName
		{
			get
			{
				return "ECNR";
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0011F70C File Offset: 0x0011D90C
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					this.random = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				else
				{
					this.random = new SecureRandom();
				}
				if (!(parameters is ECPrivateKeyParameters))
				{
					throw new InvalidKeyException("EC private key required for signing");
				}
				this.key = (ECPrivateKeyParameters)parameters;
				return;
			}
			else
			{
				if (!(parameters is ECPublicKeyParameters))
				{
					throw new InvalidKeyException("EC public key required for verification");
				}
				this.key = (ECPublicKeyParameters)parameters;
				return;
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x0011F792 File Offset: 0x0011D992
		public virtual BigInteger Order
		{
			get
			{
				return this.key.Parameters.N;
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x0011F7A4 File Offset: 0x0011D9A4
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("not initialised for signing");
			}
			BigInteger order = this.Order;
			int bitLength = order.BitLength;
			BigInteger bigInteger = new BigInteger(1, message);
			int bitLength2 = bigInteger.BitLength;
			ECPrivateKeyParameters ecprivateKeyParameters = (ECPrivateKeyParameters)this.key;
			if (bitLength2 > bitLength)
			{
				throw new DataLengthException("input too large for ECNR key.");
			}
			AsymmetricCipherKeyPair asymmetricCipherKeyPair;
			BigInteger bigInteger2;
			do
			{
				ECKeyPairGenerator eckeyPairGenerator = new ECKeyPairGenerator();
				eckeyPairGenerator.Init(new ECKeyGenerationParameters(ecprivateKeyParameters.Parameters, this.random));
				asymmetricCipherKeyPair = eckeyPairGenerator.GenerateKeyPair();
				bigInteger2 = ((ECPublicKeyParameters)asymmetricCipherKeyPair.Public).Q.AffineXCoord.ToBigInteger().Add(bigInteger).Mod(order);
			}
			while (bigInteger2.SignValue == 0);
			BigInteger d = ecprivateKeyParameters.D;
			BigInteger bigInteger3 = ((ECPrivateKeyParameters)asymmetricCipherKeyPair.Private).D.Subtract(bigInteger2.Multiply(d)).Mod(order);
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x0011F894 File Offset: 0x0011DA94
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("not initialised for verifying");
			}
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)this.key;
			BigInteger n = ecpublicKeyParameters.Parameters.N;
			int bitLength = n.BitLength;
			BigInteger bigInteger = new BigInteger(1, message);
			if (bigInteger.BitLength > bitLength)
			{
				throw new DataLengthException("input too large for ECNR key.");
			}
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.Zero) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			ECPoint g = ecpublicKeyParameters.Parameters.G;
			ECPoint q = ecpublicKeyParameters.Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, s, q, r).Normalize();
			if (ecpoint.IsInfinity)
			{
				return false;
			}
			BigInteger n2 = ecpoint.AffineXCoord.ToBigInteger();
			return r.Subtract(n2).Mod(n).Equals(bigInteger);
		}

		// Token: 0x04001ECD RID: 7885
		private bool forSigning;

		// Token: 0x04001ECE RID: 7886
		private ECKeyParameters key;

		// Token: 0x04001ECF RID: 7887
		private SecureRandom random;
	}
}
