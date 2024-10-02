using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x0200028A RID: 650
	public class BufferedDecoder
	{
		// Token: 0x060017CC RID: 6092 RVA: 0x000B8E58 File Offset: 0x000B7058
		public BufferedDecoder(ITranslator translator, int bufferSize)
		{
			this.translator = translator;
			if (bufferSize % translator.GetEncodedBlockSize() != 0)
			{
				throw new ArgumentException("buffer size not multiple of input block size");
			}
			this.buffer = new byte[bufferSize];
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000B8E88 File Offset: 0x000B7088
		public int ProcessByte(byte input, byte[] output, int outOff)
		{
			int result = 0;
			byte[] array = this.buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
			if (this.bufOff == this.buffer.Length)
			{
				result = this.translator.Decode(this.buffer, 0, this.buffer.Length, output, outOff);
				this.bufOff = 0;
			}
			return result;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000B8EE8 File Offset: 0x000B70E8
		public int ProcessBytes(byte[] input, int inOff, int len, byte[] outBytes, int outOff)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int num = 0;
			int num2 = this.buffer.Length - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.buffer, this.bufOff, num2);
				num += this.translator.Decode(this.buffer, 0, this.buffer.Length, outBytes, outOff);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				outOff += num;
				int num3 = len - len % this.buffer.Length;
				num += this.translator.Decode(input, inOff, num3, outBytes, outOff);
				len -= num3;
				inOff += num3;
			}
			if (len != 0)
			{
				Array.Copy(input, inOff, this.buffer, this.bufOff, len);
				this.bufOff += len;
			}
			return num;
		}

		// Token: 0x0400181D RID: 6173
		internal byte[] buffer;

		// Token: 0x0400181E RID: 6174
		internal int bufOff;

		// Token: 0x0400181F RID: 6175
		internal ITranslator translator;
	}
}
