using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200051F RID: 1311
	public class GOfbBlockCipher : IBlockCipher
	{
		// Token: 0x06003190 RID: 12688 RVA: 0x0012B960 File Offset: 0x00129B60
		public GOfbBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			if (this.blockSize != 8)
			{
				throw new ArgumentException("GCTR only for 64 bit block ciphers");
			}
			this.IV = new byte[cipher.GetBlockSize()];
			this.ofbV = new byte[cipher.GetBlockSize()];
			this.ofbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x06003191 RID: 12689 RVA: 0x0012B9D4 File Offset: 0x00129BD4
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06003192 RID: 12690 RVA: 0x0012B9DC File Offset: 0x00129BDC
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.firstStep = true;
			this.N3 = 0;
			this.N4 = 0;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length < this.IV.Length)
				{
					Array.Copy(iv, 0, this.IV, this.IV.Length - iv.Length, iv.Length);
					for (int i = 0; i < this.IV.Length - iv.Length; i++)
					{
						this.IV[i] = 0;
					}
				}
				else
				{
					Array.Copy(iv, 0, this.IV, 0, this.IV.Length);
				}
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(true, parameters);
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x06003193 RID: 12691 RVA: 0x0012BA93 File Offset: 0x00129C93
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/GCTR";
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x06003194 RID: 12692 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003195 RID: 12693 RVA: 0x0012BAAA File Offset: 0x00129CAA
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x0012BAB4 File Offset: 0x00129CB4
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			if (this.firstStep)
			{
				this.firstStep = false;
				this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
				this.N3 = this.bytesToint(this.ofbOutV, 0);
				this.N4 = this.bytesToint(this.ofbOutV, 4);
			}
			this.N3 += 16843009;
			this.N4 += 16843012;
			if (this.N4 < 16843012 && this.N4 > 0)
			{
				this.N4++;
			}
			this.intTobytes(this.N3, this.ofbV, 0);
			this.intTobytes(this.N4, this.ofbV, 4);
			this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				output[outOff + i] = (this.ofbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.ofbV, this.blockSize, this.ofbV, 0, this.ofbV.Length - this.blockSize);
			Array.Copy(this.ofbOutV, 0, this.ofbV, this.ofbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x0012BC43 File Offset: 0x00129E43
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.ofbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x0012BC6B File Offset: 0x00129E6B
		private int bytesToint(byte[] inBytes, int inOff)
		{
			return (int)((long)((long)inBytes[inOff + 3] << 24) & (long)((ulong)-16777216)) + ((int)inBytes[inOff + 2] << 16 & 16711680) + ((int)inBytes[inOff + 1] << 8 & 65280) + (int)(inBytes[inOff] & byte.MaxValue);
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0012BCA5 File Offset: 0x00129EA5
		private void intTobytes(int num, byte[] outBytes, int outOff)
		{
			outBytes[outOff + 3] = (byte)(num >> 24);
			outBytes[outOff + 2] = (byte)(num >> 16);
			outBytes[outOff + 1] = (byte)(num >> 8);
			outBytes[outOff] = (byte)num;
		}

		// Token: 0x0400209D RID: 8349
		private byte[] IV;

		// Token: 0x0400209E RID: 8350
		private byte[] ofbV;

		// Token: 0x0400209F RID: 8351
		private byte[] ofbOutV;

		// Token: 0x040020A0 RID: 8352
		private readonly int blockSize;

		// Token: 0x040020A1 RID: 8353
		private readonly IBlockCipher cipher;

		// Token: 0x040020A2 RID: 8354
		private bool firstStep = true;

		// Token: 0x040020A3 RID: 8355
		private int N3;

		// Token: 0x040020A4 RID: 8356
		private int N4;

		// Token: 0x040020A5 RID: 8357
		private const int C1 = 16843012;

		// Token: 0x040020A6 RID: 8358
		private const int C2 = 16843009;
	}
}
