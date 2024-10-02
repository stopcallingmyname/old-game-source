using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005C4 RID: 1476
	public class DHBasicAgreement : IBasicAgreement
	{
		// Token: 0x060038C9 RID: 14537 RVA: 0x00164210 File Offset: 0x00162410
		public virtual void Init(ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (!(parameters is DHPrivateKeyParameters))
			{
				throw new ArgumentException("DHEngine expects DHPrivateKeyParameters");
			}
			this.key = (DHPrivateKeyParameters)parameters;
			this.dhParams = this.key.Parameters;
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x00164262 File Offset: 0x00162462
		public virtual int GetFieldSize()
		{
			return (this.key.Parameters.P.BitLength + 7) / 8;
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x00164280 File Offset: 0x00162480
		public virtual BigInteger CalculateAgreement(ICipherParameters pubKey)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("Agreement algorithm not initialised");
			}
			DHPublicKeyParameters dhpublicKeyParameters = (DHPublicKeyParameters)pubKey;
			if (!dhpublicKeyParameters.Parameters.Equals(this.dhParams))
			{
				throw new ArgumentException("Diffie-Hellman public key has wrong parameters.");
			}
			BigInteger p = this.dhParams.P;
			BigInteger y = dhpublicKeyParameters.Y;
			if (y == null || y.CompareTo(BigInteger.One) <= 0 || y.CompareTo(p.Subtract(BigInteger.One)) >= 0)
			{
				throw new ArgumentException("Diffie-Hellman public key is weak");
			}
			BigInteger bigInteger = y.ModPow(this.key.X, p);
			if (bigInteger.Equals(BigInteger.One))
			{
				throw new InvalidOperationException("Shared key can't be 1");
			}
			return bigInteger;
		}

		// Token: 0x040024FF RID: 9471
		private DHPrivateKeyParameters key;

		// Token: 0x04002500 RID: 9472
		private DHParameters dhParams;
	}
}
