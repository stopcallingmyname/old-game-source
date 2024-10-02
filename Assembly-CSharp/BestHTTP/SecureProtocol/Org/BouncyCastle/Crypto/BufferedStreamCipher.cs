using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C9 RID: 969
	public class BufferedStreamCipher : BufferedCipherBase
	{
		// Token: 0x060027F9 RID: 10233 RVA: 0x0010C8BA File Offset: 0x0010AABA
		public BufferedStreamCipher(IStreamCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			this.cipher = cipher;
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x0010C8D7 File Offset: 0x0010AAD7
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x0010C8E4 File Offset: 0x0010AAE4
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.cipher.Init(forEncryption, parameters);
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int GetBlockSize()
		{
			return 0;
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public override int GetOutputSize(int inputLen)
		{
			return inputLen;
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000A54F9 File Offset: 0x000A36F9
		public override int GetUpdateOutputSize(int inputLen)
		{
			return inputLen;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x0010C908 File Offset: 0x0010AB08
		public override byte[] ProcessByte(byte input)
		{
			return new byte[]
			{
				this.cipher.ReturnByte(input)
			};
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x0010C91F File Offset: 0x0010AB1F
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			if (outOff >= output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			output[outOff] = this.cipher.ReturnByte(input);
			return 1;
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x0010C944 File Offset: 0x0010AB44
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return null;
			}
			byte[] array = new byte[length];
			this.cipher.ProcessBytes(input, inOff, length, array, 0);
			return array;
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x0010C96F File Offset: 0x0010AB6F
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (length < 1)
			{
				return 0;
			}
			if (length > 0)
			{
				this.cipher.ProcessBytes(input, inOff, length, output, outOff);
			}
			return length;
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x0010C98E File Offset: 0x0010AB8E
		public override byte[] DoFinal()
		{
			this.Reset();
			return BufferedCipherBase.EmptyBuffer;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x0010C99B File Offset: 0x0010AB9B
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return BufferedCipherBase.EmptyBuffer;
			}
			byte[] result = this.ProcessBytes(input, inOff, length);
			this.Reset();
			return result;
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x0010C9B6 File Offset: 0x0010ABB6
		public override void Reset()
		{
			this.cipher.Reset();
		}

		// Token: 0x04001B01 RID: 6913
		private readonly IStreamCipher cipher;
	}
}
