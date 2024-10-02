using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200058B RID: 1419
	public class RsaBlindingEngine : IAsymmetricBlockCipher
	{
		// Token: 0x0600359B RID: 13723 RVA: 0x00148392 File Offset: 0x00146592
		public RsaBlindingEngine() : this(new RsaCoreEngine())
		{
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0014839F File Offset: 0x0014659F
		public RsaBlindingEngine(IRsa rsa)
		{
			this.core = rsa;
		}

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x0600359D RID: 13725 RVA: 0x00148210 File Offset: 0x00146410
		public virtual string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x001483B0 File Offset: 0x001465B0
		public virtual void Init(bool forEncryption, ICipherParameters param)
		{
			RsaBlindingParameters rsaBlindingParameters;
			if (param is ParametersWithRandom)
			{
				rsaBlindingParameters = (RsaBlindingParameters)((ParametersWithRandom)param).Parameters;
			}
			else
			{
				rsaBlindingParameters = (RsaBlindingParameters)param;
			}
			this.core.Init(forEncryption, rsaBlindingParameters.PublicKey);
			this.forEncryption = forEncryption;
			this.key = rsaBlindingParameters.PublicKey;
			this.blindingFactor = rsaBlindingParameters.BlindingFactor;
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x00148410 File Offset: 0x00146610
		public virtual int GetInputBlockSize()
		{
			return this.core.GetInputBlockSize();
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0014841D File Offset: 0x0014661D
		public virtual int GetOutputBlockSize()
		{
			return this.core.GetOutputBlockSize();
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x0014842C File Offset: 0x0014662C
		public virtual byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
		{
			BigInteger bigInteger = this.core.ConvertInput(inBuf, inOff, inLen);
			if (this.forEncryption)
			{
				bigInteger = this.BlindMessage(bigInteger);
			}
			else
			{
				bigInteger = this.UnblindMessage(bigInteger);
			}
			return this.core.ConvertOutput(bigInteger);
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00148470 File Offset: 0x00146670
		private BigInteger BlindMessage(BigInteger msg)
		{
			BigInteger bigInteger = this.blindingFactor;
			bigInteger = msg.Multiply(bigInteger.ModPow(this.key.Exponent, this.key.Modulus));
			return bigInteger.Mod(this.key.Modulus);
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x001484BC File Offset: 0x001466BC
		private BigInteger UnblindMessage(BigInteger blindedMsg)
		{
			BigInteger modulus = this.key.Modulus;
			BigInteger val = this.blindingFactor.ModInverse(modulus);
			return blindedMsg.Multiply(val).Mod(modulus);
		}

		// Token: 0x0400230C RID: 8972
		private readonly IRsa core;

		// Token: 0x0400230D RID: 8973
		private RsaKeyParameters key;

		// Token: 0x0400230E RID: 8974
		private BigInteger blindingFactor;

		// Token: 0x0400230F RID: 8975
		private bool forEncryption;
	}
}
