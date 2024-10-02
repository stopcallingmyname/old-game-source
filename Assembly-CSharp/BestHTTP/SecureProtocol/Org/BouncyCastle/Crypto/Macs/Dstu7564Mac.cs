using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000533 RID: 1331
	public class Dstu7564Mac : IMac
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x0012F97A File Offset: 0x0012DB7A
		public string AlgorithmName
		{
			get
			{
				return "DSTU7564Mac";
			}
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x0012F981 File Offset: 0x0012DB81
		public Dstu7564Mac(int macSizeBits)
		{
			this.engine = new Dstu7564Digest(macSizeBits);
			this.macSize = macSizeBits / 8;
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x0012F9A0 File Offset: 0x0012DBA0
		public void Init(ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				byte[] key = ((KeyParameter)parameters).GetKey();
				this.invertedKey = new byte[key.Length];
				this.paddedKey = this.PadKey(key);
				for (int i = 0; i < this.invertedKey.Length; i++)
				{
					this.invertedKey[i] = (key[i] ^ byte.MaxValue);
				}
				this.engine.BlockUpdate(this.paddedKey, 0, this.paddedKey.Length);
				return;
			}
			throw new ArgumentException("Bad parameter passed");
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x0012FA28 File Offset: 0x0012DC28
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x0012FA30 File Offset: 0x0012DC30
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			Check.DataLength(input, inOff, len, "Input buffer too short");
			if (this.paddedKey == null)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			this.engine.BlockUpdate(input, inOff, len);
			this.inputLength += (ulong)((long)len);
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x0012FA85 File Offset: 0x0012DC85
		public void Update(byte input)
		{
			this.engine.Update(input);
			this.inputLength += 1UL;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0012FAA4 File Offset: 0x0012DCA4
		public int DoFinal(byte[] output, int outOff)
		{
			Check.OutputLength(output, outOff, this.macSize, "Output buffer too short");
			if (this.paddedKey == null)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			this.Pad();
			this.engine.BlockUpdate(this.invertedKey, 0, this.invertedKey.Length);
			this.inputLength = 0UL;
			return this.engine.DoFinal(output, outOff);
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x0012FB16 File Offset: 0x0012DD16
		public void Reset()
		{
			this.inputLength = 0UL;
			this.engine.Reset();
			if (this.paddedKey != null)
			{
				this.engine.BlockUpdate(this.paddedKey, 0, this.paddedKey.Length);
			}
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x0012FB50 File Offset: 0x0012DD50
		private void Pad()
		{
			int num = this.engine.GetByteLength() - (int)(this.inputLength % (ulong)((long)this.engine.GetByteLength()));
			if (num < 13)
			{
				num += this.engine.GetByteLength();
			}
			byte[] array = new byte[num];
			array[0] = 128;
			Pack.UInt64_To_LE(this.inputLength * 8UL, array, array.Length - 12);
			this.engine.BlockUpdate(array, 0, array.Length);
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x0012FBC8 File Offset: 0x0012DDC8
		private byte[] PadKey(byte[] input)
		{
			int num = (input.Length + this.engine.GetByteLength() - 1) / this.engine.GetByteLength() * this.engine.GetByteLength();
			if (this.engine.GetByteLength() - input.Length % this.engine.GetByteLength() < 13)
			{
				num += this.engine.GetByteLength();
			}
			byte[] array = new byte[num];
			Array.Copy(input, 0, array, 0, input.Length);
			array[input.Length] = 128;
			Pack.UInt32_To_LE((uint)(input.Length * 8), array, array.Length - 12);
			return array;
		}

		// Token: 0x0400210E RID: 8462
		private Dstu7564Digest engine;

		// Token: 0x0400210F RID: 8463
		private int macSize;

		// Token: 0x04002110 RID: 8464
		private ulong inputLength;

		// Token: 0x04002111 RID: 8465
		private byte[] paddedKey;

		// Token: 0x04002112 RID: 8466
		private byte[] invertedKey;
	}
}
