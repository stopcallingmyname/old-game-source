using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000534 RID: 1332
	public class Dstu7624Mac : IMac
	{
		// Token: 0x0600326A RID: 12906 RVA: 0x0012FC5C File Offset: 0x0012DE5C
		public Dstu7624Mac(int blockSizeBits, int q)
		{
			this.engine = new Dstu7624Engine(blockSizeBits);
			this.blockSize = blockSizeBits / 8;
			this.macSize = q / 8;
			this.c = new byte[this.blockSize];
			this.cTemp = new byte[this.blockSize];
			this.kDelta = new byte[this.blockSize];
			this.buf = new byte[this.blockSize];
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x0012FCD4 File Offset: 0x0012DED4
		public void Init(ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				this.engine.Init(true, (KeyParameter)parameters);
				this.engine.ProcessBlock(this.kDelta, 0, this.kDelta, 0);
				return;
			}
			throw new ArgumentException("invalid parameter passed to Dstu7624Mac init - " + Platform.GetTypeName(parameters));
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x0600326C RID: 12908 RVA: 0x0012FD2B File Offset: 0x0012DF2B
		public string AlgorithmName
		{
			get
			{
				return "Dstu7624Mac";
			}
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x0012FD32 File Offset: 0x0012DF32
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x0012FD3C File Offset: 0x0012DF3C
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.processBlock(this.buf, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x0012FD88 File Offset: 0x0012DF88
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int num = this.engine.GetBlockSize();
			int num2 = num - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num2);
				this.processBlock(this.buf, 0);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				while (len > num)
				{
					this.processBlock(input, inOff);
					len -= num;
					inOff += num;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x0012FE29 File Offset: 0x0012E029
		private void processBlock(byte[] input, int inOff)
		{
			this.Xor(this.c, 0, input, inOff, this.cTemp);
			this.engine.ProcessBlock(this.cTemp, 0, this.c, 0);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x0012FE5C File Offset: 0x0012E05C
		private void Xor(byte[] c, int cOff, byte[] input, int inOff, byte[] xorResult)
		{
			for (int i = 0; i < this.blockSize; i++)
			{
				xorResult[i] = (c[i + cOff] ^ input[i + inOff]);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x0012FE8C File Offset: 0x0012E08C
		public int DoFinal(byte[] output, int outOff)
		{
			if (this.bufOff % this.buf.Length != 0)
			{
				throw new DataLengthException("Input must be a multiple of blocksize");
			}
			this.Xor(this.c, 0, this.buf, 0, this.cTemp);
			this.Xor(this.cTemp, 0, this.kDelta, 0, this.c);
			this.engine.ProcessBlock(this.c, 0, this.c, 0);
			if (this.macSize + outOff > output.Length)
			{
				throw new DataLengthException("Output buffer too short");
			}
			Array.Copy(this.c, 0, output, outOff, this.macSize);
			return this.macSize;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x0012FF38 File Offset: 0x0012E138
		public void Reset()
		{
			Arrays.Fill(this.c, 0);
			Arrays.Fill(this.cTemp, 0);
			Arrays.Fill(this.kDelta, 0);
			Arrays.Fill(this.buf, 0);
			this.engine.Reset();
			this.engine.ProcessBlock(this.kDelta, 0, this.kDelta, 0);
			this.bufOff = 0;
		}

		// Token: 0x04002113 RID: 8467
		private int macSize;

		// Token: 0x04002114 RID: 8468
		private Dstu7624Engine engine;

		// Token: 0x04002115 RID: 8469
		private int blockSize;

		// Token: 0x04002116 RID: 8470
		private byte[] c;

		// Token: 0x04002117 RID: 8471
		private byte[] cTemp;

		// Token: 0x04002118 RID: 8472
		private byte[] kDelta;

		// Token: 0x04002119 RID: 8473
		private byte[] buf;

		// Token: 0x0400211A RID: 8474
		private int bufOff;
	}
}
