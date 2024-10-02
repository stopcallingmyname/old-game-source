using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000577 RID: 1399
	public class ElGamalEngine : IAsymmetricBlockCipher
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x060034AE RID: 13486 RVA: 0x00142282 File Offset: 0x00140482
		public virtual string AlgorithmName
		{
			get
			{
				return "ElGamal";
			}
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x0014228C File Offset: 0x0014048C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				this.key = (ElGamalKeyParameters)parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
			}
			else
			{
				this.key = (ElGamalKeyParameters)parameters;
				this.random = new SecureRandom();
			}
			this.forEncryption = forEncryption;
			this.bitSize = this.key.Parameters.P.BitLength;
			if (forEncryption)
			{
				if (!(this.key is ElGamalPublicKeyParameters))
				{
					throw new ArgumentException("ElGamalPublicKeyParameters are required for encryption.");
				}
			}
			else if (!(this.key is ElGamalPrivateKeyParameters))
			{
				throw new ArgumentException("ElGamalPrivateKeyParameters are required for decryption.");
			}
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00142333 File Offset: 0x00140533
		public virtual int GetInputBlockSize()
		{
			if (this.forEncryption)
			{
				return (this.bitSize - 1) / 8;
			}
			return 2 * ((this.bitSize + 7) / 8);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00142354 File Offset: 0x00140554
		public virtual int GetOutputBlockSize()
		{
			if (this.forEncryption)
			{
				return 2 * ((this.bitSize + 7) / 8);
			}
			return (this.bitSize - 1) / 8;
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00142378 File Offset: 0x00140578
		public virtual byte[] ProcessBlock(byte[] input, int inOff, int length)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("ElGamal engine not initialised");
			}
			int num = this.forEncryption ? ((this.bitSize - 1 + 7) / 8) : this.GetInputBlockSize();
			if (length > num)
			{
				throw new DataLengthException("input too large for ElGamal cipher.\n");
			}
			BigInteger p = this.key.Parameters.P;
			byte[] array;
			if (this.key is ElGamalPrivateKeyParameters)
			{
				int num2 = length / 2;
				BigInteger bigInteger = new BigInteger(1, input, inOff, num2);
				BigInteger val = new BigInteger(1, input, inOff + num2, num2);
				ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = (ElGamalPrivateKeyParameters)this.key;
				array = bigInteger.ModPow(p.Subtract(BigInteger.One).Subtract(elGamalPrivateKeyParameters.X), p).Multiply(val).Mod(p).ToByteArrayUnsigned();
			}
			else
			{
				BigInteger bigInteger2 = new BigInteger(1, input, inOff, length);
				if (bigInteger2.BitLength >= p.BitLength)
				{
					throw new DataLengthException("input too large for ElGamal cipher.\n");
				}
				ElGamalPublicKeyParameters elGamalPublicKeyParameters = (ElGamalPublicKeyParameters)this.key;
				BigInteger value = p.Subtract(BigInteger.Two);
				BigInteger bigInteger3;
				do
				{
					bigInteger3 = new BigInteger(p.BitLength, this.random);
				}
				while (bigInteger3.SignValue == 0 || bigInteger3.CompareTo(value) > 0);
				BigInteger bigInteger4 = this.key.Parameters.G.ModPow(bigInteger3, p);
				BigInteger bigInteger5 = bigInteger2.Multiply(elGamalPublicKeyParameters.Y.ModPow(bigInteger3, p)).Mod(p);
				array = new byte[this.GetOutputBlockSize()];
				byte[] array2 = bigInteger4.ToByteArrayUnsigned();
				byte[] array3 = bigInteger5.ToByteArrayUnsigned();
				array2.CopyTo(array, array.Length / 2 - array2.Length);
				array3.CopyTo(array, array.Length - array3.Length);
			}
			return array;
		}

		// Token: 0x04002280 RID: 8832
		private ElGamalKeyParameters key;

		// Token: 0x04002281 RID: 8833
		private SecureRandom random;

		// Token: 0x04002282 RID: 8834
		private bool forEncryption;

		// Token: 0x04002283 RID: 8835
		private int bitSize;
	}
}
