using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Agreement
{
	// Token: 0x020005C3 RID: 1475
	public class DHAgreement
	{
		// Token: 0x060038C5 RID: 14533 RVA: 0x00164070 File Offset: 0x00162270
		public void Init(ICipherParameters parameters)
		{
			AsymmetricKeyParameter asymmetricKeyParameter;
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				this.random = parametersWithRandom.Random;
				asymmetricKeyParameter = (AsymmetricKeyParameter)parametersWithRandom.Parameters;
			}
			else
			{
				this.random = new SecureRandom();
				asymmetricKeyParameter = (AsymmetricKeyParameter)parameters;
			}
			if (!(asymmetricKeyParameter is DHPrivateKeyParameters))
			{
				throw new ArgumentException("DHEngine expects DHPrivateKeyParameters");
			}
			this.key = (DHPrivateKeyParameters)asymmetricKeyParameter;
			this.dhParams = this.key.Parameters;
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x001640E8 File Offset: 0x001622E8
		public BigInteger CalculateMessage()
		{
			DHKeyPairGenerator dhkeyPairGenerator = new DHKeyPairGenerator();
			dhkeyPairGenerator.Init(new DHKeyGenerationParameters(this.random, this.dhParams));
			AsymmetricCipherKeyPair asymmetricCipherKeyPair = dhkeyPairGenerator.GenerateKeyPair();
			this.privateValue = ((DHPrivateKeyParameters)asymmetricCipherKeyPair.Private).X;
			return ((DHPublicKeyParameters)asymmetricCipherKeyPair.Public).Y;
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x00164140 File Offset: 0x00162340
		public BigInteger CalculateAgreement(DHPublicKeyParameters pub, BigInteger message)
		{
			if (pub == null)
			{
				throw new ArgumentNullException("pub");
			}
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			if (!pub.Parameters.Equals(this.dhParams))
			{
				throw new ArgumentException("Diffie-Hellman public key has wrong parameters.");
			}
			BigInteger p = this.dhParams.P;
			BigInteger y = pub.Y;
			if (y == null || y.CompareTo(BigInteger.One) <= 0 || y.CompareTo(p.Subtract(BigInteger.One)) >= 0)
			{
				throw new ArgumentException("Diffie-Hellman public key is weak");
			}
			BigInteger bigInteger = y.ModPow(this.privateValue, p);
			if (bigInteger.Equals(BigInteger.One))
			{
				throw new InvalidOperationException("Shared key can't be 1");
			}
			return message.ModPow(this.key.X, p).Multiply(bigInteger).Mod(p);
		}

		// Token: 0x040024FB RID: 9467
		private DHPrivateKeyParameters key;

		// Token: 0x040024FC RID: 9468
		private DHParameters dhParams;

		// Token: 0x040024FD RID: 9469
		private BigInteger privateValue;

		// Token: 0x040024FE RID: 9470
		private SecureRandom random;
	}
}
