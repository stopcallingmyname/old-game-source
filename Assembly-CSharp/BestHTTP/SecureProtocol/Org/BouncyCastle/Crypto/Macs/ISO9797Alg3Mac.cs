using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Paddings;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000538 RID: 1336
	public class ISO9797Alg3Mac : IMac
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x001309A6 File Offset: 0x0012EBA6
		public ISO9797Alg3Mac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8, null)
		{
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x001309B8 File Offset: 0x0012EBB8
		public ISO9797Alg3Mac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, cipher.GetBlockSize() * 8, padding)
		{
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x001309CA File Offset: 0x0012EBCA
		public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits) : this(cipher, macSizeInBits, null)
		{
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x001309D8 File Offset: 0x0012EBD8
		public ISO9797Alg3Mac(IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			if (!(cipher is DesEngine))
			{
				throw new ArgumentException("cipher must be instance of DesEngine");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.mac = new byte[cipher.GetBlockSize()];
			this.buf = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x00130A53 File Offset: 0x0012EC53
		public string AlgorithmName
		{
			get
			{
				return "ISO9797Alg3";
			}
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00130A5C File Offset: 0x0012EC5C
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			if (!(parameters is KeyParameter) && !(parameters is ParametersWithIV))
			{
				throw new ArgumentException("parameters must be an instance of KeyParameter or ParametersWithIV");
			}
			KeyParameter keyParameter;
			if (parameters is KeyParameter)
			{
				keyParameter = (KeyParameter)parameters;
			}
			else
			{
				keyParameter = (KeyParameter)((ParametersWithIV)parameters).Parameters;
			}
			byte[] key = keyParameter.GetKey();
			KeyParameter parameters2;
			if (key.Length == 16)
			{
				parameters2 = new KeyParameter(key, 0, 8);
				this.lastKey2 = new KeyParameter(key, 8, 8);
				this.lastKey3 = parameters2;
			}
			else
			{
				if (key.Length != 24)
				{
					throw new ArgumentException("Key must be either 112 or 168 bit long");
				}
				parameters2 = new KeyParameter(key, 0, 8);
				this.lastKey2 = new KeyParameter(key, 8, 8);
				this.lastKey3 = new KeyParameter(key, 16, 8);
			}
			if (parameters is ParametersWithIV)
			{
				this.cipher.Init(true, new ParametersWithIV(parameters2, ((ParametersWithIV)parameters).GetIV()));
				return;
			}
			this.cipher.Init(true, parameters2);
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00130B47 File Offset: 0x0012ED47
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00130B50 File Offset: 0x0012ED50
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

		// Token: 0x0600329D RID: 12957 RVA: 0x00130BA8 File Offset: 0x0012EDA8
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
				Array.Copy(input, inOff, this.buf, this.bufOff, num2);
				num += this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
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
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x00130C6C File Offset: 0x0012EE6C
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] array = this.buf;
					int num = this.bufOff;
					this.bufOff = num + 1;
					array[num] = 0;
				}
			}
			else
			{
				if (this.bufOff == blockSize)
				{
					this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
					this.bufOff = 0;
				}
				this.padding.AddPadding(this.buf, this.bufOff);
			}
			this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
			DesEngine desEngine = new DesEngine();
			desEngine.Init(false, this.lastKey2);
			desEngine.ProcessBlock(this.mac, 0, this.mac, 0);
			desEngine.Init(true, this.lastKey3);
			desEngine.ProcessBlock(this.mac, 0, this.mac, 0);
			Array.Copy(this.mac, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x00130D77 File Offset: 0x0012EF77
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x0400212F RID: 8495
		private byte[] mac;

		// Token: 0x04002130 RID: 8496
		private byte[] buf;

		// Token: 0x04002131 RID: 8497
		private int bufOff;

		// Token: 0x04002132 RID: 8498
		private IBlockCipher cipher;

		// Token: 0x04002133 RID: 8499
		private IBlockCipherPadding padding;

		// Token: 0x04002134 RID: 8500
		private int macSize;

		// Token: 0x04002135 RID: 8501
		private KeyParameter lastKey2;

		// Token: 0x04002136 RID: 8502
		private KeyParameter lastKey3;
	}
}
