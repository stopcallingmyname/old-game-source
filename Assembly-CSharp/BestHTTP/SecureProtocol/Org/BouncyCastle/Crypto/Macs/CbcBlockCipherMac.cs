using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200052F RID: 1327
	public class CbcBlockCipherMac : IMac
	{
		// Token: 0x06003237 RID: 12855 RVA: 0x0012EE24 File Offset: 0x0012D024
		public CbcBlockCipherMac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8 / 2, null)
		{
		}

		// Token: 0x06003238 RID: 12856 RVA: 0x0012EE38 File Offset: 0x0012D038
		public CbcBlockCipherMac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, cipher.GetBlockSize() * 8 / 2, padding)
		{
		}

		// Token: 0x06003239 RID: 12857 RVA: 0x0012EE4C File Offset: 0x0012D04C
		public CbcBlockCipherMac(IBlockCipher cipher, int macSizeInBits) : this(cipher, macSizeInBits, null)
		{
		}

		// Token: 0x0600323A RID: 12858 RVA: 0x0012EE58 File Offset: 0x0012D058
		public CbcBlockCipherMac(IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.buf = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x0012EEAF File Offset: 0x0012D0AF
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x0012EEBC File Offset: 0x0012D0BC
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x0012EED1 File Offset: 0x0012D0D1
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x0012EEDC File Offset: 0x0012D0DC
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x0012EF34 File Offset: 0x0012D134
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = blockSize - this.bufOff;
			if (len > num)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num);
				this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
				this.bufOff = 0;
				len -= num;
				inOff += num;
				while (len > blockSize)
				{
					this.cipher.ProcessBlock(input, inOff, this.buf, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x06003240 RID: 12864 RVA: 0x0012EFF0 File Offset: 0x0012D1F0
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] array = this.buf;
					int num = this.bufOff;
					this.bufOff = num + 1;
					array[num] = 0;
				}
			}
			else
			{
				if (this.bufOff == blockSize)
				{
					this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
					this.bufOff = 0;
				}
				this.padding.AddPadding(this.buf, this.bufOff);
			}
			this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
			Array.Copy(this.buf, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x0012F0B3 File Offset: 0x0012D2B3
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x040020F3 RID: 8435
		private byte[] buf;

		// Token: 0x040020F4 RID: 8436
		private int bufOff;

		// Token: 0x040020F5 RID: 8437
		private IBlockCipher cipher;

		// Token: 0x040020F6 RID: 8438
		private IBlockCipherPadding padding;

		// Token: 0x040020F7 RID: 8439
		private int macSize;
	}
}
