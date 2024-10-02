using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000525 RID: 1317
	public class OpenPgpCfbBlockCipher : IBlockCipher
	{
		// Token: 0x060031ED RID: 12781 RVA: 0x0012D88C File Offset: 0x0012BA8C
		public OpenPgpCfbBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.IV = new byte[this.blockSize];
			this.FR = new byte[this.blockSize];
			this.FRE = new byte[this.blockSize];
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0012D8E5 File Offset: 0x0012BAE5
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060031EF RID: 12783 RVA: 0x0012D8ED File Offset: 0x0012BAED
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/OpenPGPCFB";
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060031F0 RID: 12784 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0012D904 File Offset: 0x0012BB04
		public int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x0012D911 File Offset: 0x0012BB11
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x060031F3 RID: 12787 RVA: 0x0012D932 File Offset: 0x0012BB32
		public void Reset()
		{
			this.count = 0;
			Array.Copy(this.IV, 0, this.FR, 0, this.FR.Length);
			this.cipher.Reset();
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x0012D964 File Offset: 0x0012BB64
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
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
			this.cipher.Init(true, parameters);
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x0012DA0A File Offset: 0x0012BC0A
		private byte EncryptByte(byte data, int blockOff)
		{
			return this.FRE[blockOff] ^ data;
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0012DA18 File Offset: 0x0012BC18
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			if (this.count > this.blockSize)
			{
				this.FR[this.blockSize - 2] = (outBytes[outOff] = this.EncryptByte(input[inOff], this.blockSize - 2));
				this.FR[this.blockSize - 1] = (outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], this.blockSize - 1));
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int i = 2; i < this.blockSize; i++)
				{
					this.FR[i - 2] = (outBytes[outOff + i] = this.EncryptByte(input[inOff + i], i - 2));
				}
			}
			else if (this.count == 0)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int j = 0; j < this.blockSize; j++)
				{
					this.FR[j] = (outBytes[outOff + j] = this.EncryptByte(input[inOff + j], j));
				}
				this.count += this.blockSize;
			}
			else if (this.count == this.blockSize)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				outBytes[outOff] = this.EncryptByte(input[inOff], 0);
				outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], 1);
				Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
				Array.Copy(outBytes, outOff, this.FR, this.blockSize - 2, 2);
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int k = 2; k < this.blockSize; k++)
				{
					this.FR[k - 2] = (outBytes[outOff + k] = this.EncryptByte(input[inOff + k], k - 2));
				}
				this.count += this.blockSize;
			}
			return this.blockSize;
		}

		// Token: 0x060031F7 RID: 12791 RVA: 0x0012DC54 File Offset: 0x0012BE54
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			if (this.count > this.blockSize)
			{
				byte b = input[inOff];
				this.FR[this.blockSize - 2] = b;
				outBytes[outOff] = this.EncryptByte(b, this.blockSize - 2);
				b = input[inOff + 1];
				this.FR[this.blockSize - 1] = b;
				outBytes[outOff + 1] = this.EncryptByte(b, this.blockSize - 1);
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int i = 2; i < this.blockSize; i++)
				{
					b = input[inOff + i];
					this.FR[i - 2] = b;
					outBytes[outOff + i] = this.EncryptByte(b, i - 2);
				}
			}
			else if (this.count == 0)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int j = 0; j < this.blockSize; j++)
				{
					this.FR[j] = input[inOff + j];
					outBytes[j] = this.EncryptByte(input[inOff + j], j);
				}
				this.count += this.blockSize;
			}
			else if (this.count == this.blockSize)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				byte b2 = input[inOff];
				byte b3 = input[inOff + 1];
				outBytes[outOff] = this.EncryptByte(b2, 0);
				outBytes[outOff + 1] = this.EncryptByte(b3, 1);
				Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
				this.FR[this.blockSize - 2] = b2;
				this.FR[this.blockSize - 1] = b3;
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int k = 2; k < this.blockSize; k++)
				{
					byte b4 = input[inOff + k];
					this.FR[k - 2] = b4;
					outBytes[outOff + k] = this.EncryptByte(b4, k - 2);
				}
				this.count += this.blockSize;
			}
			return this.blockSize;
		}

		// Token: 0x040020DC RID: 8412
		private byte[] IV;

		// Token: 0x040020DD RID: 8413
		private byte[] FR;

		// Token: 0x040020DE RID: 8414
		private byte[] FRE;

		// Token: 0x040020DF RID: 8415
		private readonly IBlockCipher cipher;

		// Token: 0x040020E0 RID: 8416
		private readonly int blockSize;

		// Token: 0x040020E1 RID: 8417
		private int count;

		// Token: 0x040020E2 RID: 8418
		private bool forEncryption;
	}
}
