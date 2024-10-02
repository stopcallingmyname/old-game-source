using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000532 RID: 1330
	public class CMac : IMac
	{
		// Token: 0x06003255 RID: 12885 RVA: 0x0012F5A6 File Offset: 0x0012D7A6
		public CMac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8)
		{
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x0012F5B8 File Offset: 0x0012D7B8
		public CMac(IBlockCipher cipher, int macSizeInBits)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			if (macSizeInBits > cipher.GetBlockSize() * 8)
			{
				throw new ArgumentException("MAC size must be less or equal to " + cipher.GetBlockSize() * 8);
			}
			if (cipher.GetBlockSize() != 8 && cipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("Block size must be either 64 or 128 bits");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.macSize = macSizeInBits / 8;
			this.mac = new byte[cipher.GetBlockSize()];
			this.buf = new byte[cipher.GetBlockSize()];
			this.ZEROES = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x0012F670 File Offset: 0x0012D870
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0012F680 File Offset: 0x0012D880
		private static int ShiftLeft(byte[] block, byte[] output)
		{
			int num = block.Length;
			uint num2 = 0U;
			while (--num >= 0)
			{
				uint num3 = (uint)block[num];
				output[num] = (byte)(num3 << 1 | num2);
				num2 = (num3 >> 7 & 1U);
			}
			return (int)num2;
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x0012F6B4 File Offset: 0x0012D8B4
		private static byte[] DoubleLu(byte[] input)
		{
			byte[] array = new byte[input.Length];
			int num = CMac.ShiftLeft(input, array);
			int num2 = (input.Length == 16) ? 135 : 27;
			byte[] array2 = array;
			int num3 = input.Length - 1;
			array2[num3] ^= (byte)(num2 >> (1 - num << 3));
			return array;
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x0012F700 File Offset: 0x0012D900
		public void Init(ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				this.cipher.Init(true, parameters);
				this.L = new byte[this.ZEROES.Length];
				this.cipher.ProcessBlock(this.ZEROES, 0, this.L, 0);
				this.Lu = CMac.DoubleLu(this.L);
				this.Lu2 = CMac.DoubleLu(this.Lu);
			}
			else if (parameters != null)
			{
				throw new ArgumentException("CMac mode only permits key to be set.", "parameters");
			}
			this.Reset();
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x0012F78C File Offset: 0x0012D98C
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600325C RID: 12892 RVA: 0x0012F794 File Offset: 0x0012D994
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x0012F7EC File Offset: 0x0012D9EC
		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = blockSize - this.bufOff;
			if (len > num)
			{
				Array.Copy(inBytes, inOff, this.buf, this.bufOff, num);
				this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num;
				inOff += num;
				while (len > blockSize)
				{
					this.cipher.ProcessBlock(inBytes, inOff, this.mac, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(inBytes, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x0012F8A8 File Offset: 0x0012DAA8
		public int DoFinal(byte[] outBytes, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			byte[] array;
			if (this.bufOff == blockSize)
			{
				array = this.Lu;
			}
			else
			{
				new ISO7816d4Padding().AddPadding(this.buf, this.bufOff);
				array = this.Lu2;
			}
			for (int i = 0; i < this.mac.Length; i++)
			{
				byte[] array2 = this.buf;
				int num = i;
				array2[num] ^= array[i];
			}
			this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
			Array.Copy(this.mac, 0, outBytes, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x0012F952 File Offset: 0x0012DB52
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x04002103 RID: 8451
		private const byte CONSTANT_128 = 135;

		// Token: 0x04002104 RID: 8452
		private const byte CONSTANT_64 = 27;

		// Token: 0x04002105 RID: 8453
		private byte[] ZEROES;

		// Token: 0x04002106 RID: 8454
		private byte[] mac;

		// Token: 0x04002107 RID: 8455
		private byte[] buf;

		// Token: 0x04002108 RID: 8456
		private int bufOff;

		// Token: 0x04002109 RID: 8457
		private IBlockCipher cipher;

		// Token: 0x0400210A RID: 8458
		private int macSize;

		// Token: 0x0400210B RID: 8459
		private byte[] L;

		// Token: 0x0400210C RID: 8460
		private byte[] Lu;

		// Token: 0x0400210D RID: 8461
		private byte[] Lu2;
	}
}
