using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200058D RID: 1421
	public class RsaEngine : IAsymmetricBlockCipher
	{
		// Token: 0x060035AC RID: 13740 RVA: 0x001486F6 File Offset: 0x001468F6
		public RsaEngine() : this(new RsaCoreEngine())
		{
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x00148703 File Offset: 0x00146903
		public RsaEngine(IRsa rsa)
		{
			this.core = rsa;
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x060035AE RID: 13742 RVA: 0x00148210 File Offset: 0x00146410
		public virtual string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00148712 File Offset: 0x00146912
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.core.Init(forEncryption, parameters);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x00148721 File Offset: 0x00146921
		public virtual int GetInputBlockSize()
		{
			return this.core.GetInputBlockSize();
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0014872E File Offset: 0x0014692E
		public virtual int GetOutputBlockSize()
		{
			return this.core.GetOutputBlockSize();
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x0014873C File Offset: 0x0014693C
		public virtual byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
		{
			BigInteger input = this.core.ConvertInput(inBuf, inOff, inLen);
			BigInteger result = this.core.ProcessBlock(input);
			return this.core.ConvertOutput(result);
		}

		// Token: 0x04002313 RID: 8979
		private readonly IRsa core;
	}
}
