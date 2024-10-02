using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200058A RID: 1418
	public class RsaBlindedEngine : IAsymmetricBlockCipher
	{
		// Token: 0x06003594 RID: 13716 RVA: 0x001481F4 File Offset: 0x001463F4
		public RsaBlindedEngine() : this(new RsaCoreEngine())
		{
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x00148201 File Offset: 0x00146401
		public RsaBlindedEngine(IRsa rsa)
		{
			this.core = rsa;
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06003596 RID: 13718 RVA: 0x00148210 File Offset: 0x00146410
		public virtual string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x00148218 File Offset: 0x00146418
		public virtual void Init(bool forEncryption, ICipherParameters param)
		{
			this.core.Init(forEncryption, param);
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.key = (RsaKeyParameters)parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
				return;
			}
			this.key = (RsaKeyParameters)param;
			this.random = new SecureRandom();
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00148276 File Offset: 0x00146476
		public virtual int GetInputBlockSize()
		{
			return this.core.GetInputBlockSize();
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x00148283 File Offset: 0x00146483
		public virtual int GetOutputBlockSize()
		{
			return this.core.GetOutputBlockSize();
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x00148290 File Offset: 0x00146490
		public virtual byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("RSA engine not initialised");
			}
			BigInteger bigInteger = this.core.ConvertInput(inBuf, inOff, inLen);
			BigInteger bigInteger4;
			if (this.key is RsaPrivateCrtKeyParameters)
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)this.key;
				BigInteger publicExponent = rsaPrivateCrtKeyParameters.PublicExponent;
				if (publicExponent != null)
				{
					BigInteger modulus = rsaPrivateCrtKeyParameters.Modulus;
					BigInteger bigInteger2 = BigIntegers.CreateRandomInRange(BigInteger.One, modulus.Subtract(BigInteger.One), this.random);
					BigInteger input = bigInteger2.ModPow(publicExponent, modulus).Multiply(bigInteger).Mod(modulus);
					BigInteger bigInteger3 = this.core.ProcessBlock(input);
					BigInteger val = bigInteger2.ModInverse(modulus);
					bigInteger4 = bigInteger3.Multiply(val).Mod(modulus);
					if (!bigInteger.Equals(bigInteger4.ModPow(publicExponent, modulus)))
					{
						throw new InvalidOperationException("RSA engine faulty decryption/signing detected");
					}
				}
				else
				{
					bigInteger4 = this.core.ProcessBlock(bigInteger);
				}
			}
			else
			{
				bigInteger4 = this.core.ProcessBlock(bigInteger);
			}
			return this.core.ConvertOutput(bigInteger4);
		}

		// Token: 0x04002309 RID: 8969
		private readonly IRsa core;

		// Token: 0x0400230A RID: 8970
		private RsaKeyParameters key;

		// Token: 0x0400230B RID: 8971
		private SecureRandom random;
	}
}
