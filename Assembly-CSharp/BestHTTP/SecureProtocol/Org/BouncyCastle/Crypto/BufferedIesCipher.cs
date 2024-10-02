using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C8 RID: 968
	public class BufferedIesCipher : BufferedCipherBase
	{
		// Token: 0x060027EE RID: 10222 RVA: 0x0010C78C File Offset: 0x0010A98C
		public BufferedIesCipher(IesEngine engine)
		{
			if (engine == null)
			{
				throw new ArgumentNullException("engine");
			}
			this.engine = engine;
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060027EF RID: 10223 RVA: 0x0010C7B4 File Offset: 0x0010A9B4
		public override string AlgorithmName
		{
			get
			{
				return "IES";
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x0010C7BB File Offset: 0x0010A9BB
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			throw Platform.CreateNotImplementedException("IES");
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int GetBlockSize()
		{
			return 0;
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x0010C7D0 File Offset: 0x0010A9D0
		public override int GetOutputSize(int inputLen)
		{
			if (this.engine == null)
			{
				throw new InvalidOperationException("cipher not initialised");
			}
			int num = inputLen + (int)this.buffer.Length;
			if (!this.forEncryption)
			{
				return num - 20;
			}
			return num + 20;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int GetUpdateOutputSize(int inputLen)
		{
			return 0;
		}

		// Token: 0x060027F4 RID: 10228 RVA: 0x0010C810 File Offset: 0x0010AA10
		public override byte[] ProcessByte(byte input)
		{
			this.buffer.WriteByte(input);
			return null;
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x0010C820 File Offset: 0x0010AA20
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (inOff < 0)
			{
				throw new ArgumentException("inOff");
			}
			if (length < 0)
			{
				throw new ArgumentException("length");
			}
			if (inOff + length > input.Length)
			{
				throw new ArgumentException("invalid offset/length specified for input array");
			}
			this.buffer.Write(input, inOff, length);
			return null;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x0010C87C File Offset: 0x0010AA7C
		public override byte[] DoFinal()
		{
			byte[] array = this.buffer.ToArray();
			this.Reset();
			return this.engine.ProcessBlock(array, 0, array.Length);
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x0010C1B9 File Offset: 0x0010A3B9
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			this.ProcessBytes(input, inOff, length);
			return this.DoFinal();
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x0010C8AB File Offset: 0x0010AAAB
		public override void Reset()
		{
			this.buffer.SetLength(0L);
		}

		// Token: 0x04001AFE RID: 6910
		private readonly IesEngine engine;

		// Token: 0x04001AFF RID: 6911
		private bool forEncryption;

		// Token: 0x04001B00 RID: 6912
		private MemoryStream buffer = new MemoryStream();
	}
}
