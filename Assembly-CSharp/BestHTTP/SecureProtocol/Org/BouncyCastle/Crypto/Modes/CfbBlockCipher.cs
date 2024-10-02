using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200051B RID: 1307
	public class CfbBlockCipher : IBlockCipher
	{
		// Token: 0x06003154 RID: 12628 RVA: 0x00129F04 File Offset: 0x00128104
		public CfbBlockCipher(IBlockCipher cipher, int bitBlockSize)
		{
			this.cipher = cipher;
			this.blockSize = bitBlockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.cfbV = new byte[cipher.GetBlockSize()];
			this.cfbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x00129F5A File Offset: 0x0012815A
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06003156 RID: 12630 RVA: 0x00129F64 File Offset: 0x00128164
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.encrypting = forEncryption;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				int num = this.IV.Length - iv.Length;
				Array.Copy(iv, 0, this.IV, num, iv.Length);
				Array.Clear(this.IV, 0, num);
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(true, parameters);
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06003157 RID: 12631 RVA: 0x00129FD5 File Offset: 0x001281D5
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x06003158 RID: 12632 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003159 RID: 12633 RVA: 0x00129FF9 File Offset: 0x001281F9
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x0600315A RID: 12634 RVA: 0x0012A001 File Offset: 0x00128201
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.encrypting)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x0012A024 File Offset: 0x00128224
		public int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				outBytes[outOff + i] = (this.cfbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
			Array.Copy(outBytes, outOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x0600315C RID: 12636 RVA: 0x0012A0F4 File Offset: 0x001282F4
		public int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
			Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
			Array.Copy(input, inOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
			for (int i = 0; i < this.blockSize; i++)
			{
				outBytes[outOff + i] = (this.cfbOutV[i] ^ input[inOff + i]);
			}
			return this.blockSize;
		}

		// Token: 0x0600315D RID: 12637 RVA: 0x0012A1C1 File Offset: 0x001283C1
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.cfbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x04002070 RID: 8304
		private byte[] IV;

		// Token: 0x04002071 RID: 8305
		private byte[] cfbV;

		// Token: 0x04002072 RID: 8306
		private byte[] cfbOutV;

		// Token: 0x04002073 RID: 8307
		private bool encrypting;

		// Token: 0x04002074 RID: 8308
		private readonly int blockSize;

		// Token: 0x04002075 RID: 8309
		private readonly IBlockCipher cipher;
	}
}
