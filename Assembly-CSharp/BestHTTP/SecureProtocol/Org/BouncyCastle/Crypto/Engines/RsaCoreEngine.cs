using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200058C RID: 1420
	public class RsaCoreEngine : IRsa
	{
		// Token: 0x060035A4 RID: 13732 RVA: 0x001484EF File Offset: 0x001466EF
		private void CheckInitialised()
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("RSA engine not initialised");
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00148504 File Offset: 0x00146704
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (!(parameters is RsaKeyParameters))
			{
				throw new InvalidKeyException("Not an RSA key");
			}
			this.key = (RsaKeyParameters)parameters;
			this.forEncryption = forEncryption;
			this.bitSize = this.key.Modulus.BitLength;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x00148562 File Offset: 0x00146762
		public virtual int GetInputBlockSize()
		{
			this.CheckInitialised();
			if (this.forEncryption)
			{
				return (this.bitSize - 1) / 8;
			}
			return (this.bitSize + 7) / 8;
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x00148587 File Offset: 0x00146787
		public virtual int GetOutputBlockSize()
		{
			this.CheckInitialised();
			if (this.forEncryption)
			{
				return (this.bitSize + 7) / 8;
			}
			return (this.bitSize - 1) / 8;
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x001485AC File Offset: 0x001467AC
		public virtual BigInteger ConvertInput(byte[] inBuf, int inOff, int inLen)
		{
			this.CheckInitialised();
			int num = (this.bitSize + 7) / 8;
			if (inLen > num)
			{
				throw new DataLengthException("input too large for RSA cipher.");
			}
			BigInteger bigInteger = new BigInteger(1, inBuf, inOff, inLen);
			if (bigInteger.CompareTo(this.key.Modulus) >= 0)
			{
				throw new DataLengthException("input too large for RSA cipher.");
			}
			return bigInteger;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x00148604 File Offset: 0x00146804
		public virtual byte[] ConvertOutput(BigInteger result)
		{
			this.CheckInitialised();
			byte[] array = result.ToByteArrayUnsigned();
			if (this.forEncryption)
			{
				int outputBlockSize = this.GetOutputBlockSize();
				if (array.Length < outputBlockSize)
				{
					byte[] array2 = new byte[outputBlockSize];
					array.CopyTo(array2, array2.Length - array.Length);
					array = array2;
				}
			}
			return array;
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0014864C File Offset: 0x0014684C
		public virtual BigInteger ProcessBlock(BigInteger input)
		{
			this.CheckInitialised();
			if (this.key is RsaPrivateCrtKeyParameters)
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)this.key;
				BigInteger p = rsaPrivateCrtKeyParameters.P;
				BigInteger q = rsaPrivateCrtKeyParameters.Q;
				BigInteger dp = rsaPrivateCrtKeyParameters.DP;
				BigInteger dq = rsaPrivateCrtKeyParameters.DQ;
				BigInteger qinv = rsaPrivateCrtKeyParameters.QInv;
				BigInteger bigInteger = input.Remainder(p).ModPow(dp, p);
				BigInteger bigInteger2 = input.Remainder(q).ModPow(dq, q);
				return bigInteger.Subtract(bigInteger2).Multiply(qinv).Mod(p).Multiply(q).Add(bigInteger2);
			}
			return input.ModPow(this.key.Exponent, this.key.Modulus);
		}

		// Token: 0x04002310 RID: 8976
		private RsaKeyParameters key;

		// Token: 0x04002311 RID: 8977
		private bool forEncryption;

		// Token: 0x04002312 RID: 8978
		private int bitSize;
	}
}
