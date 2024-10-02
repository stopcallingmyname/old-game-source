using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000580 RID: 1408
	public class NullEngine : IBlockCipher
	{
		// Token: 0x06003520 RID: 13600 RVA: 0x00144EE0 File Offset: 0x001430E0
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.initialised = true;
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x00144EE9 File Offset: 0x001430E9
		public virtual string AlgorithmName
		{
			get
			{
				return "Null";
			}
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003523 RID: 13603 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual int GetBlockSize()
		{
			return 1;
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x00144EF0 File Offset: 0x001430F0
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.initialised)
			{
				throw new InvalidOperationException("Null engine not initialised");
			}
			Check.DataLength(input, inOff, 1, "input buffer too short");
			Check.OutputLength(output, outOff, 1, "output buffer too short");
			for (int i = 0; i < 1; i++)
			{
				output[outOff + i] = input[inOff + i];
			}
			return 1;
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x0000248C File Offset: 0x0000068C
		public virtual void Reset()
		{
		}

		// Token: 0x040022C4 RID: 8900
		private bool initialised;

		// Token: 0x040022C5 RID: 8901
		private const int BlockSize = 1;
	}
}
