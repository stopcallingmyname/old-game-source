using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000519 RID: 1305
	public sealed class CbcBlockCipher : IBlockCipher
	{
		// Token: 0x06003136 RID: 12598 RVA: 0x00129530 File Offset: 0x00127730
		public CbcBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.IV = new byte[this.blockSize];
			this.cbcV = new byte[this.blockSize];
			this.cbcNextV = new byte[this.blockSize];
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x00129589 File Offset: 0x00127789
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x00129594 File Offset: 0x00127794
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			bool flag = this.encrypting;
			this.encrypting = forEncryption;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length != this.blockSize)
				{
					throw new ArgumentException("initialisation vector must be the same length as block size");
				}
				Array.Copy(iv, 0, this.IV, 0, iv.Length);
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(this.encrypting, parameters);
				return;
			}
			if (flag != this.encrypting)
			{
				throw new ArgumentException("cannot change encrypting state without providing key.");
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06003139 RID: 12601 RVA: 0x00129622 File Offset: 0x00127822
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CBC";
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x0600313A RID: 12602 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x00129639 File Offset: 0x00127839
		public int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x00129646 File Offset: 0x00127846
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.encrypting)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x00129667 File Offset: 0x00127867
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.cbcV, 0, this.IV.Length);
			Array.Clear(this.cbcNextV, 0, this.cbcNextV.Length);
			this.cipher.Reset();
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x001296A4 File Offset: 0x001278A4
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			for (int i = 0; i < this.blockSize; i++)
			{
				byte[] array = this.cbcV;
				int num = i;
				array[num] ^= input[inOff + i];
			}
			int result = this.cipher.ProcessBlock(this.cbcV, 0, outBytes, outOff);
			Array.Copy(outBytes, outOff, this.cbcV, 0, this.cbcV.Length);
			return result;
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x0012971C File Offset: 0x0012791C
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			Array.Copy(input, inOff, this.cbcNextV, 0, this.blockSize);
			int result = this.cipher.ProcessBlock(input, inOff, outBytes, outOff);
			for (int i = 0; i < this.blockSize; i++)
			{
				int num = outOff + i;
				outBytes[num] ^= this.cbcV[i];
			}
			byte[] array = this.cbcV;
			this.cbcV = this.cbcNextV;
			this.cbcNextV = array;
			return result;
		}

		// Token: 0x04002060 RID: 8288
		private byte[] IV;

		// Token: 0x04002061 RID: 8289
		private byte[] cbcV;

		// Token: 0x04002062 RID: 8290
		private byte[] cbcNextV;

		// Token: 0x04002063 RID: 8291
		private int blockSize;

		// Token: 0x04002064 RID: 8292
		private IBlockCipher cipher;

		// Token: 0x04002065 RID: 8293
		private bool encrypting;
	}
}
