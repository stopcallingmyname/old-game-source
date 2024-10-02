using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200049E RID: 1182
	public class Gost3410Signer : IDsaExt, IDsa
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002E62 RID: 11874 RVA: 0x00120361 File Offset: 0x0011E561
		public virtual string AlgorithmName
		{
			get
			{
				return "GOST3410";
			}
		}

		// Token: 0x06002E63 RID: 11875 RVA: 0x00120368 File Offset: 0x0011E568
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
				if (!(parameters is Gost3410PrivateKeyParameters))
				{
					throw new InvalidKeyException("GOST3410 private key required for signing");
				}
				this.key = (Gost3410PrivateKeyParameters)parameters;
				return;
			}
			else
			{
				if (!(parameters is Gost3410PublicKeyParameters))
				{
					throw new InvalidKeyException("GOST3410 public key required for signing");
				}
				this.key = (Gost3410PublicKeyParameters)parameters;
				return;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002E64 RID: 11876 RVA: 0x001203E7 File Offset: 0x0011E5E7
		public virtual BigInteger Order
		{
			get
			{
				return this.key.Parameters.Q;
			}
		}

		// Token: 0x06002E65 RID: 11877 RVA: 0x001203FC File Offset: 0x0011E5FC
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger val = new BigInteger(1, array);
			Gost3410Parameters parameters = this.key.Parameters;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(parameters.Q.BitLength, this.random);
			}
			while (bigInteger.CompareTo(parameters.Q) >= 0);
			BigInteger bigInteger2 = parameters.A.ModPow(bigInteger, parameters.P).Mod(parameters.Q);
			BigInteger bigInteger3 = bigInteger.Multiply(val).Add(((Gost3410PrivateKeyParameters)this.key).X.Multiply(bigInteger2)).Mod(parameters.Q);
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x06002E66 RID: 11878 RVA: 0x001204CC File Offset: 0x0011E6CC
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger bigInteger = new BigInteger(1, array);
			Gost3410Parameters parameters = this.key.Parameters;
			if (r.SignValue < 0 || parameters.Q.CompareTo(r) <= 0)
			{
				return false;
			}
			if (s.SignValue < 0 || parameters.Q.CompareTo(s) <= 0)
			{
				return false;
			}
			BigInteger val = bigInteger.ModPow(parameters.Q.Subtract(BigInteger.Two), parameters.Q);
			BigInteger bigInteger2 = s.Multiply(val).Mod(parameters.Q);
			BigInteger bigInteger3 = parameters.Q.Subtract(r).Multiply(val).Mod(parameters.Q);
			bigInteger2 = parameters.A.ModPow(bigInteger2, parameters.P);
			bigInteger3 = ((Gost3410PublicKeyParameters)this.key).Y.ModPow(bigInteger3, parameters.P);
			return bigInteger2.Multiply(bigInteger3).Mod(parameters.P).Mod(parameters.Q).Equals(r);
		}

		// Token: 0x04001EEE RID: 7918
		private Gost3410KeyParameters key;

		// Token: 0x04001EEF RID: 7919
		private SecureRandom random;
	}
}
