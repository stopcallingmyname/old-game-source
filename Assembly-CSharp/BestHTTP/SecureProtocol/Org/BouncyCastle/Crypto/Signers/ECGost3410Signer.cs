using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC.Multiplier;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000495 RID: 1173
	public class ECGost3410Signer : IDsaExt, IDsa
	{
		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x0011F46F File Offset: 0x0011D66F
		public virtual string AlgorithmName
		{
			get
			{
				return "ECGOST3410";
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0011F478 File Offset: 0x0011D678
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
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

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x0011F4F7 File Offset: 0x0011D6F7
		public virtual BigInteger Order
		{
			get
			{
				return this.key.Parameters.N;
			}
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0011F50C File Offset: 0x0011D70C
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger val = new BigInteger(1, array);
			ECDomainParameters parameters = this.key.Parameters;
			BigInteger n = parameters.N;
			BigInteger d = ((ECPrivateKeyParameters)this.key).D;
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			BigInteger bigInteger2;
			BigInteger bigInteger3;
			for (;;)
			{
				BigInteger bigInteger = new BigInteger(n.BitLength, this.random);
				if (bigInteger.SignValue != 0)
				{
					bigInteger2 = ecmultiplier.Multiply(parameters.G, bigInteger).Normalize().AffineXCoord.ToBigInteger().Mod(n);
					if (bigInteger2.SignValue != 0)
					{
						bigInteger3 = bigInteger.Multiply(val).Add(d.Multiply(bigInteger2)).Mod(n);
						if (bigInteger3.SignValue != 0)
						{
							break;
						}
					}
				}
			}
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x0011F5FC File Offset: 0x0011D7FC
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger bigInteger = new BigInteger(1, array);
			BigInteger n = this.key.Parameters.N;
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.One) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			BigInteger val = bigInteger.ModInverse(n);
			BigInteger a = s.Multiply(val).Mod(n);
			BigInteger b = n.Subtract(r).Multiply(val).Mod(n);
			ECPoint g = this.key.Parameters.G;
			ECPoint q = ((ECPublicKeyParameters)this.key).Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b).Normalize();
			return !ecpoint.IsInfinity && ecpoint.AffineXCoord.ToBigInteger().Mod(n).Equals(r);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x0011F41C File Offset: 0x0011D61C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x04001ECB RID: 7883
		private ECKeyParameters key;

		// Token: 0x04001ECC RID: 7884
		private SecureRandom random;
	}
}
