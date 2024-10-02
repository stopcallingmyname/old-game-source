using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000531 RID: 1329
	public class CfbBlockCipherMac : IMac
	{
		// Token: 0x0600324A RID: 12874 RVA: 0x0012F2E4 File Offset: 0x0012D4E4
		public CfbBlockCipherMac(IBlockCipher cipher) : this(cipher, 8, cipher.GetBlockSize() * 8 / 2, null)
		{
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x0012F2F9 File Offset: 0x0012D4F9
		public CfbBlockCipherMac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, 8, cipher.GetBlockSize() * 8 / 2, padding)
		{
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x0012F30E File Offset: 0x0012D50E
		public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits) : this(cipher, cfbBitSize, macSizeInBits, null)
		{
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x0012F31C File Offset: 0x0012D51C
		public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			this.mac = new byte[cipher.GetBlockSize()];
			this.cipher = new MacCFBBlockCipher(cipher, cfbBitSize);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.Buffer = new byte[this.cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x0600324E RID: 12878 RVA: 0x0012F38B File Offset: 0x0012D58B
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x0600324F RID: 12879 RVA: 0x0012F398 File Offset: 0x0012D598
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x0012F3AD File Offset: 0x0012D5AD
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x0012F3B8 File Offset: 0x0012D5B8
		public void Update(byte input)
		{
			if (this.bufOff == this.Buffer.Length)
			{
				this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] buffer = this.Buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			buffer[num] = input;
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x0012F410 File Offset: 0x0012D610
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = 0;
			int num2 = blockSize - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.Buffer, this.bufOff, num2);
				num += this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				while (len > blockSize)
				{
					num += this.cipher.ProcessBlock(input, inOff, this.mac, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(input, inOff, this.Buffer, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x06003253 RID: 12883 RVA: 0x0012F4D4 File Offset: 0x0012D6D4
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] buffer = this.Buffer;
					int num = this.bufOff;
					this.bufOff = num + 1;
					buffer[num] = 0;
				}
			}
			else
			{
				this.padding.AddPadding(this.Buffer, this.bufOff);
			}
			this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
			this.cipher.GetMacBlock(this.mac);
			Array.Copy(this.mac, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x06003254 RID: 12884 RVA: 0x0012F57E File Offset: 0x0012D77E
		public void Reset()
		{
			Array.Clear(this.Buffer, 0, this.Buffer.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x040020FD RID: 8445
		private byte[] mac;

		// Token: 0x040020FE RID: 8446
		private byte[] Buffer;

		// Token: 0x040020FF RID: 8447
		private int bufOff;

		// Token: 0x04002100 RID: 8448
		private MacCFBBlockCipher cipher;

		// Token: 0x04002101 RID: 8449
		private IBlockCipherPadding padding;

		// Token: 0x04002102 RID: 8450
		private int macSize;
	}
}
