using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C5 RID: 965
	public class BufferedAsymmetricBlockCipher : BufferedCipherBase
	{
		// Token: 0x060027BF RID: 10175 RVA: 0x0010C06C File Offset: 0x0010A26C
		public BufferedAsymmetricBlockCipher(IAsymmetricBlockCipher cipher)
		{
			this.cipher = cipher;
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x0010C07B File Offset: 0x0010A27B
		internal int GetBufferPosition()
		{
			return this.bufOff;
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060027C1 RID: 10177 RVA: 0x0010C083 File Offset: 0x0010A283
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x0010C090 File Offset: 0x0010A290
		public override int GetBlockSize()
		{
			return this.cipher.GetInputBlockSize();
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x0010C09D File Offset: 0x0010A29D
		public override int GetOutputSize(int length)
		{
			return this.cipher.GetOutputBlockSize();
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int GetUpdateOutputSize(int length)
		{
			return 0;
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x0010C0AA File Offset: 0x0010A2AA
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(forEncryption, parameters);
			this.buffer = new byte[this.cipher.GetInputBlockSize() + (forEncryption ? 1 : 0)];
			this.bufOff = 0;
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x0010C0E4 File Offset: 0x0010A2E4
		public override byte[] ProcessByte(byte input)
		{
			if (this.bufOff >= this.buffer.Length)
			{
				throw new DataLengthException("attempt to process message to long for cipher");
			}
			byte[] array = this.buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
			return null;
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x0010C128 File Offset: 0x0010A328
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return null;
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (this.bufOff + length > this.buffer.Length)
			{
				throw new DataLengthException("attempt to process message to long for cipher");
			}
			Array.Copy(input, inOff, this.buffer, this.bufOff, length);
			this.bufOff += length;
			return null;
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x0010C189 File Offset: 0x0010A389
		public override byte[] DoFinal()
		{
			byte[] result = (this.bufOff > 0) ? this.cipher.ProcessBlock(this.buffer, 0, this.bufOff) : BufferedCipherBase.EmptyBuffer;
			this.Reset();
			return result;
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x0010C1B9 File Offset: 0x0010A3B9
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			this.ProcessBytes(input, inOff, length);
			return this.DoFinal();
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x0010C1CB File Offset: 0x0010A3CB
		public override void Reset()
		{
			if (this.buffer != null)
			{
				Array.Clear(this.buffer, 0, this.buffer.Length);
				this.bufOff = 0;
			}
		}

		// Token: 0x04001AF6 RID: 6902
		private readonly IAsymmetricBlockCipher cipher;

		// Token: 0x04001AF7 RID: 6903
		private byte[] buffer;

		// Token: 0x04001AF8 RID: 6904
		private int bufOff;
	}
}
