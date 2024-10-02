using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000524 RID: 1316
	public class OfbBlockCipher : IBlockCipher
	{
		// Token: 0x060031E5 RID: 12773 RVA: 0x0012D660 File Offset: 0x0012B860
		public OfbBlockCipher(IBlockCipher cipher, int blockSize)
		{
			this.cipher = cipher;
			this.blockSize = blockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.ofbV = new byte[cipher.GetBlockSize()];
			this.ofbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x0012D6B6 File Offset: 0x0012B8B6
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0012D6C0 File Offset: 0x0012B8C0
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
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

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060031E8 RID: 12776 RVA: 0x0012D762 File Offset: 0x0012B962
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/OFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060031E9 RID: 12777 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031EA RID: 12778 RVA: 0x0012D786 File Offset: 0x0012B986
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x060031EB RID: 12779 RVA: 0x0012D790 File Offset: 0x0012B990
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
			this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				output[outOff + i] = (this.ofbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.ofbV, this.blockSize, this.ofbV, 0, this.ofbV.Length - this.blockSize);
			Array.Copy(this.ofbOutV, 0, this.ofbV, this.ofbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x060031EC RID: 12780 RVA: 0x0012D862 File Offset: 0x0012BA62
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.ofbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x040020D7 RID: 8407
		private byte[] IV;

		// Token: 0x040020D8 RID: 8408
		private byte[] ofbV;

		// Token: 0x040020D9 RID: 8409
		private byte[] ofbOutV;

		// Token: 0x040020DA RID: 8410
		private readonly int blockSize;

		// Token: 0x040020DB RID: 8411
		private readonly IBlockCipher cipher;
	}
}
