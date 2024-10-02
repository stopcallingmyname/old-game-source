using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200051C RID: 1308
	public class CtsBlockCipher : BufferedBlockCipher
	{
		// Token: 0x0600315E RID: 12638 RVA: 0x0012A1EC File Offset: 0x001283EC
		public CtsBlockCipher(IBlockCipher cipher)
		{
			if (cipher is OfbBlockCipher || cipher is CfbBlockCipher)
			{
				throw new ArgumentException("CtsBlockCipher can only accept ECB, or CBC ciphers");
			}
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.buf = new byte[this.blockSize * 2];
			this.bufOff = 0;
		}

		// Token: 0x0600315F RID: 12639 RVA: 0x0012A248 File Offset: 0x00128448
		public override int GetUpdateOutputSize(int length)
		{
			int num = length + this.bufOff;
			int num2 = num % this.buf.Length;
			if (num2 == 0)
			{
				return num - this.buf.Length;
			}
			return num - num2;
		}

		// Token: 0x06003160 RID: 12640 RVA: 0x0010C2A6 File Offset: 0x0010A4A6
		public override int GetOutputSize(int length)
		{
			return length + this.bufOff;
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x0012A27C File Offset: 0x0012847C
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			int result = 0;
			if (this.bufOff == this.buf.Length)
			{
				result = this.cipher.ProcessBlock(this.buf, 0, output, outOff);
				Array.Copy(this.buf, this.blockSize, this.buf, 0, this.blockSize);
				this.bufOff = this.blockSize;
			}
			byte[] buf = this.buf;
			int bufOff = this.bufOff;
			this.bufOff = bufOff + 1;
			buf[bufOff] = input;
			return result;
		}

		// Token: 0x06003162 RID: 12642 RVA: 0x0012A2F4 File Offset: 0x001284F4
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (length < 0)
			{
				throw new ArgumentException("Can't have a negative input outLength!");
			}
			int num = this.GetBlockSize();
			int updateOutputSize = this.GetUpdateOutputSize(length);
			if (updateOutputSize > 0 && outOff + updateOutputSize > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			int num2 = 0;
			int num3 = this.buf.Length - this.bufOff;
			if (length > num3)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num3);
				num2 += this.cipher.ProcessBlock(this.buf, 0, output, outOff);
				Array.Copy(this.buf, num, this.buf, 0, num);
				this.bufOff = num;
				length -= num3;
				inOff += num3;
				while (length > num)
				{
					Array.Copy(input, inOff, this.buf, this.bufOff, num);
					num2 += this.cipher.ProcessBlock(this.buf, 0, output, outOff + num2);
					Array.Copy(this.buf, num, this.buf, 0, num);
					length -= num;
					inOff += num;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, length);
			this.bufOff += length;
			return num2;
		}

		// Token: 0x06003163 RID: 12643 RVA: 0x0012A41C File Offset: 0x0012861C
		public override int DoFinal(byte[] output, int outOff)
		{
			if (this.bufOff + outOff > output.Length)
			{
				throw new DataLengthException("output buffer too small in doFinal");
			}
			int num = this.cipher.GetBlockSize();
			int length = this.bufOff - num;
			byte[] array = new byte[num];
			if (this.forEncryption)
			{
				this.cipher.ProcessBlock(this.buf, 0, array, 0);
				if (this.bufOff < num)
				{
					throw new DataLengthException("need at least one block of input for CTS");
				}
				for (int num2 = this.bufOff; num2 != this.buf.Length; num2++)
				{
					this.buf[num2] = array[num2 - num];
				}
				for (int num3 = num; num3 != this.bufOff; num3++)
				{
					byte[] buf = this.buf;
					int num4 = num3;
					buf[num4] ^= array[num3 - num];
				}
				((this.cipher is CbcBlockCipher) ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock(this.buf, num, output, outOff);
				Array.Copy(array, 0, output, outOff + num, length);
			}
			else
			{
				byte[] array2 = new byte[num];
				((this.cipher is CbcBlockCipher) ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock(this.buf, 0, array, 0);
				for (int num5 = num; num5 != this.bufOff; num5++)
				{
					array2[num5 - num] = (array[num5 - num] ^ this.buf[num5]);
				}
				Array.Copy(this.buf, num, array, 0, length);
				this.cipher.ProcessBlock(array, 0, output, outOff);
				Array.Copy(array2, 0, output, outOff + num, length);
			}
			int bufOff = this.bufOff;
			this.Reset();
			return bufOff;
		}

		// Token: 0x04002076 RID: 8310
		private readonly int blockSize;
	}
}
