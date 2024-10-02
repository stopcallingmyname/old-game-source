using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000493 RID: 1171
	public class DsaSigner : IDsaExt, IDsa
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x0011EE4A File Offset: 0x0011D04A
		public DsaSigner()
		{
			this.kCalculator = new RandomDsaKCalculator();
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x0011EE5D File Offset: 0x0011D05D
		public DsaSigner(IDsaKCalculator kCalculator)
		{
			this.kCalculator = kCalculator;
		}

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06002E0A RID: 11786 RVA: 0x0011EE6C File Offset: 0x0011D06C
		public virtual string AlgorithmName
		{
			get
			{
				return "DSA";
			}
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0011EE74 File Offset: 0x0011D074
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			SecureRandom provided = null;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					provided = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				if (!(parameters is DsaPrivateKeyParameters))
				{
					throw new InvalidKeyException("DSA private key required for signing");
				}
				this.key = (DsaPrivateKeyParameters)parameters;
			}
			else
			{
				if (!(parameters is DsaPublicKeyParameters))
				{
					throw new InvalidKeyException("DSA public key required for verification");
				}
				this.key = (DsaPublicKeyParameters)parameters;
			}
			this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x0011EF03 File Offset: 0x0011D103
		public virtual BigInteger Order
		{
			get
			{
				return this.key.Parameters.Q;
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0011EF18 File Offset: 0x0011D118
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			DsaParameters parameters = this.key.Parameters;
			BigInteger q = parameters.Q;
			BigInteger bigInteger = this.CalculateE(q, message);
			BigInteger x = ((DsaPrivateKeyParameters)this.key).X;
			if (this.kCalculator.IsDeterministic)
			{
				this.kCalculator.Init(q, x, message);
			}
			else
			{
				this.kCalculator.Init(q, this.random);
			}
			BigInteger bigInteger2 = this.kCalculator.NextK();
			BigInteger bigInteger3 = parameters.G.ModPow(bigInteger2, parameters.P).Mod(q);
			bigInteger2 = bigInteger2.ModInverse(q).Multiply(bigInteger.Add(x.Multiply(bigInteger3)));
			BigInteger bigInteger4 = bigInteger2.Mod(q);
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0011EFE0 File Offset: 0x0011D1E0
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			DsaParameters parameters = this.key.Parameters;
			BigInteger q = parameters.Q;
			BigInteger bigInteger = this.CalculateE(q, message);
			if (r.SignValue <= 0 || q.CompareTo(r) <= 0)
			{
				return false;
			}
			if (s.SignValue <= 0 || q.CompareTo(s) <= 0)
			{
				return false;
			}
			BigInteger val = s.ModInverse(q);
			BigInteger bigInteger2 = bigInteger.Multiply(val).Mod(q);
			BigInteger bigInteger3 = r.Multiply(val).Mod(q);
			BigInteger p = parameters.P;
			bigInteger2 = parameters.G.ModPow(bigInteger2, p);
			bigInteger3 = ((DsaPublicKeyParameters)this.key).Y.ModPow(bigInteger3, p);
			return bigInteger2.Multiply(bigInteger3).Mod(p).Mod(q).Equals(r);
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0011F0AC File Offset: 0x0011D2AC
		protected virtual BigInteger CalculateE(BigInteger n, byte[] message)
		{
			int length = Math.Min(message.Length, n.BitLength / 8);
			return new BigInteger(1, message, 0, length);
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0011F0D3 File Offset: 0x0011D2D3
		protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided)
		{
			if (!needed)
			{
				return null;
			}
			if (provided == null)
			{
				return new SecureRandom();
			}
			return provided;
		}

		// Token: 0x04001EC4 RID: 7876
		protected readonly IDsaKCalculator kCalculator;

		// Token: 0x04001EC5 RID: 7877
		protected DsaKeyParameters key;

		// Token: 0x04001EC6 RID: 7878
		protected SecureRandom random;
	}
}
