using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000526 RID: 1318
	public class SicBlockCipher : IBlockCipher
	{
		// Token: 0x060031F8 RID: 12792 RVA: 0x0012DEAC File Offset: 0x0012C0AC
		public SicBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.counter = new byte[this.blockSize];
			this.counterOut = new byte[this.blockSize];
			this.IV = new byte[this.blockSize];
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0012DF05 File Offset: 0x0012C105
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0012DF10 File Offset: 0x0012C110
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			ParametersWithIV parametersWithIV = parameters as ParametersWithIV;
			if (parametersWithIV == null)
			{
				throw new ArgumentException("CTR/SIC mode requires ParametersWithIV", "parameters");
			}
			this.IV = Arrays.Clone(parametersWithIV.GetIV());
			if (this.blockSize < this.IV.Length)
			{
				throw new ArgumentException("CTR/SIC mode requires IV no greater than: " + this.blockSize + " bytes.");
			}
			int num = Math.Min(8, this.blockSize / 2);
			if (this.blockSize - this.IV.Length > num)
			{
				throw new ArgumentException("CTR/SIC mode requires IV of at least: " + (this.blockSize - num) + " bytes.");
			}
			if (parametersWithIV.Parameters != null)
			{
				this.cipher.Init(true, parametersWithIV.Parameters);
			}
			this.Reset();
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x0012DFDB File Offset: 0x0012C1DB
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/SIC";
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x0006AE98 File Offset: 0x00069098
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031FD RID: 12797 RVA: 0x0012DFF2 File Offset: 0x0012C1F2
		public virtual int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0012E000 File Offset: 0x0012C200
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.cipher.ProcessBlock(this.counter, 0, this.counterOut, 0);
			for (int i = 0; i < this.counterOut.Length; i++)
			{
				output[outOff + i] = (this.counterOut[i] ^ input[inOff + i]);
			}
			int num = this.counter.Length;
			while (--num >= 0)
			{
				byte[] array = this.counter;
				int num2 = num;
				byte b = array[num2] + 1;
				array[num2] = b;
				if (b != 0)
				{
					break;
				}
			}
			return this.counter.Length;
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x0012E07F File Offset: 0x0012C27F
		public virtual void Reset()
		{
			Arrays.Fill(this.counter, 0);
			Array.Copy(this.IV, 0, this.counter, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x040020E3 RID: 8419
		private readonly IBlockCipher cipher;

		// Token: 0x040020E4 RID: 8420
		private readonly int blockSize;

		// Token: 0x040020E5 RID: 8421
		private readonly byte[] counter;

		// Token: 0x040020E6 RID: 8422
		private readonly byte[] counterOut;

		// Token: 0x040020E7 RID: 8423
		private byte[] IV;
	}
}
