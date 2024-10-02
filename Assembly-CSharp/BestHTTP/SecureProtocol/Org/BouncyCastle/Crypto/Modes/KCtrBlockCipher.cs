using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000522 RID: 1314
	public class KCtrBlockCipher : IStreamCipher, IBlockCipher
	{
		// Token: 0x060031BE RID: 12734 RVA: 0x0012C7E8 File Offset: 0x0012A9E8
		public KCtrBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.IV = new byte[cipher.GetBlockSize()];
			this.blockSize = cipher.GetBlockSize();
			this.ofbV = new byte[cipher.GetBlockSize()];
			this.ofbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x0012C841 File Offset: 0x0012AA41
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x0012C84C File Offset: 0x0012AA4C
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.initialised = true;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				int destinationIndex = this.IV.Length - iv.Length;
				Array.Clear(this.IV, 0, this.IV.Length);
				Array.Copy(iv, 0, this.IV, destinationIndex, iv.Length);
				parameters = parametersWithIV.Parameters;
				if (parameters != null)
				{
					this.cipher.Init(true, parameters);
				}
				this.Reset();
				return;
			}
			throw new ArgumentException("Invalid parameter passed");
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060031C1 RID: 12737 RVA: 0x0012C8D1 File Offset: 0x0012AAD1
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/KCTR";
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060031C2 RID: 12738 RVA: 0x0006AE98 File Offset: 0x00069098
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060031C3 RID: 12739 RVA: 0x0012C8E8 File Offset: 0x0012AAE8
		public int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x060031C4 RID: 12740 RVA: 0x0012C8F5 File Offset: 0x0012AAF5
		public byte ReturnByte(byte input)
		{
			return this.CalculateByte(input);
		}

		// Token: 0x060031C5 RID: 12741 RVA: 0x0012C900 File Offset: 0x0012AB00
		public void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			if (outOff + len > output.Length)
			{
				throw new DataLengthException("Output buffer too short");
			}
			if (inOff + len > input.Length)
			{
				throw new DataLengthException("Input buffer too small");
			}
			int i = inOff;
			int num = inOff + len;
			int num2 = outOff;
			while (i < num)
			{
				output[num2++] = this.CalculateByte(input[i++]);
			}
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x0012C95C File Offset: 0x0012AB5C
		protected byte CalculateByte(byte b)
		{
			int num;
			if (this.byteCount == 0)
			{
				this.incrementCounterAt(0);
				this.checkCounter();
				this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
				byte[] array = this.ofbOutV;
				num = this.byteCount;
				this.byteCount = num + 1;
				return array[num] ^ b;
			}
			byte[] array2 = this.ofbOutV;
			num = this.byteCount;
			this.byteCount = num + 1;
			byte result = array2[num] ^ b;
			if (this.byteCount == this.ofbV.Length)
			{
				this.byteCount = 0;
			}
			return result;
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x0012C9E8 File Offset: 0x0012ABE8
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (input.Length - inOff < this.GetBlockSize())
			{
				throw new DataLengthException("Input buffer too short");
			}
			if (output.Length - outOff < this.GetBlockSize())
			{
				throw new DataLengthException("Output buffer too short");
			}
			this.ProcessBytes(input, inOff, this.GetBlockSize(), output, outOff);
			return this.GetBlockSize();
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x0012CA3D File Offset: 0x0012AC3D
		public void Reset()
		{
			if (this.initialised)
			{
				this.cipher.ProcessBlock(this.IV, 0, this.ofbV, 0);
			}
			this.cipher.Reset();
			this.byteCount = 0;
		}

		// Token: 0x060031C9 RID: 12745 RVA: 0x0012CA74 File Offset: 0x0012AC74
		private void incrementCounterAt(int pos)
		{
			int i = pos;
			while (i < this.ofbV.Length)
			{
				byte[] array = this.ofbV;
				int num = i++;
				byte b = array[num] + 1;
				array[num] = b;
				if (b != 0)
				{
					break;
				}
			}
		}

		// Token: 0x060031CA RID: 12746 RVA: 0x0000248C File Offset: 0x0000068C
		private void checkCounter()
		{
		}

		// Token: 0x040020B9 RID: 8377
		private byte[] IV;

		// Token: 0x040020BA RID: 8378
		private byte[] ofbV;

		// Token: 0x040020BB RID: 8379
		private byte[] ofbOutV;

		// Token: 0x040020BC RID: 8380
		private bool initialised;

		// Token: 0x040020BD RID: 8381
		private int byteCount;

		// Token: 0x040020BE RID: 8382
		private readonly int blockSize;

		// Token: 0x040020BF RID: 8383
		private readonly IBlockCipher cipher;
	}
}
