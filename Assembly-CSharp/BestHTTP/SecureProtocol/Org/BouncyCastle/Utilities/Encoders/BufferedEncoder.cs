using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x0200028B RID: 651
	public class BufferedEncoder
	{
		// Token: 0x060017CF RID: 6095 RVA: 0x000B8FB8 File Offset: 0x000B71B8
		public BufferedEncoder(ITranslator translator, int bufferSize)
		{
			this.translator = translator;
			if (bufferSize % translator.GetEncodedBlockSize() != 0)
			{
				throw new ArgumentException("buffer size not multiple of input block size");
			}
			this.Buffer = new byte[bufferSize];
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000B8FE8 File Offset: 0x000B71E8
		public int ProcessByte(byte input, byte[] outBytes, int outOff)
		{
			int result = 0;
			byte[] buffer = this.Buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			buffer[num] = input;
			if (this.bufOff == this.Buffer.Length)
			{
				result = this.translator.Encode(this.Buffer, 0, this.Buffer.Length, outBytes, outOff);
				this.bufOff = 0;
			}
			return result;
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000B9048 File Offset: 0x000B7248
		public int ProcessBytes(byte[] input, int inOff, int len, byte[] outBytes, int outOff)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int num = 0;
			int num2 = this.Buffer.Length - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.Buffer, this.bufOff, num2);
				num += this.translator.Encode(this.Buffer, 0, this.Buffer.Length, outBytes, outOff);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				outOff += num;
				int num3 = len - len % this.Buffer.Length;
				num += this.translator.Encode(input, inOff, num3, outBytes, outOff);
				len -= num3;
				inOff += num3;
			}
			if (len != 0)
			{
				Array.Copy(input, inOff, this.Buffer, this.bufOff, len);
				this.bufOff += len;
			}
			return num;
		}

		// Token: 0x04001820 RID: 6176
		internal byte[] Buffer;

		// Token: 0x04001821 RID: 6177
		internal int bufOff;

		// Token: 0x04001822 RID: 6178
		internal ITranslator translator;
	}
}
