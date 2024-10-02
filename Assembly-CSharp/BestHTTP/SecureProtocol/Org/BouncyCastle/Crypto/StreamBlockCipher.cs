using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003EE RID: 1006
	public class StreamBlockCipher : IStreamCipher
	{
		// Token: 0x0600289A RID: 10394 RVA: 0x0010CC5C File Offset: 0x0010AE5C
		public StreamBlockCipher(IBlockCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			if (cipher.GetBlockSize() != 1)
			{
				throw new ArgumentException("block cipher block size != 1.", "cipher");
			}
			this.cipher = cipher;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x0010CCA9 File Offset: 0x0010AEA9
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.cipher.Init(forEncryption, parameters);
		}

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x0010CCB8 File Offset: 0x0010AEB8
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x0010CCC5 File Offset: 0x0010AEC5
		public byte ReturnByte(byte input)
		{
			this.oneByte[0] = input;
			this.cipher.ProcessBlock(this.oneByte, 0, this.oneByte, 0);
			return this.oneByte[0];
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x0010CCF4 File Offset: 0x0010AEF4
		public void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (outOff + length > output.Length)
			{
				throw new DataLengthException("output buffer too small in ProcessBytes()");
			}
			for (int num = 0; num != length; num++)
			{
				this.cipher.ProcessBlock(input, inOff + num, output, outOff + num);
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x0010CD38 File Offset: 0x0010AF38
		public void Reset()
		{
			this.cipher.Reset();
		}

		// Token: 0x04001B0B RID: 6923
		private readonly IBlockCipher cipher;

		// Token: 0x04001B0C RID: 6924
		private readonly byte[] oneByte = new byte[1];
	}
}
