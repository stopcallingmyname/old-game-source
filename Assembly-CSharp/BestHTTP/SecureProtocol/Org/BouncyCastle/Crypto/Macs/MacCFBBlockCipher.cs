using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x02000530 RID: 1328
	internal class MacCFBBlockCipher : IBlockCipher
	{
		// Token: 0x06003242 RID: 12866 RVA: 0x0012F0DC File Offset: 0x0012D2DC
		public MacCFBBlockCipher(IBlockCipher cipher, int bitBlockSize)
		{
			this.cipher = cipher;
			this.blockSize = bitBlockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.cfbV = new byte[cipher.GetBlockSize()];
			this.cfbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x0012F134 File Offset: 0x0012D334
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length < this.IV.Length)
				{
					Array.Copy(iv, 0, this.IV, this.IV.Length - iv.Length, iv.Length);
				}
				else
				{
					Array.Copy(iv, 0, this.IV, 0, this.IV.Length);
				}
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06003244 RID: 12868 RVA: 0x0012F1B1 File Offset: 0x0012D3B1
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x0012F1D5 File Offset: 0x0012D3D5
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x0012F1E0 File Offset: 0x0012D3E0
		public int ProcessBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				outBytes[outOff + i] = (this.cfbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
			Array.Copy(outBytes, outOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x0012F2AE File Offset: 0x0012D4AE
		public void Reset()
		{
			this.IV.CopyTo(this.cfbV, 0);
			this.cipher.Reset();
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x0012F2CD File Offset: 0x0012D4CD
		public void GetMacBlock(byte[] mac)
		{
			this.cipher.ProcessBlock(this.cfbV, 0, mac, 0);
		}

		// Token: 0x040020F8 RID: 8440
		private byte[] IV;

		// Token: 0x040020F9 RID: 8441
		private byte[] cfbV;

		// Token: 0x040020FA RID: 8442
		private byte[] cfbOutV;

		// Token: 0x040020FB RID: 8443
		private readonly int blockSize;

		// Token: 0x040020FC RID: 8444
		private readonly IBlockCipher cipher;
	}
}
