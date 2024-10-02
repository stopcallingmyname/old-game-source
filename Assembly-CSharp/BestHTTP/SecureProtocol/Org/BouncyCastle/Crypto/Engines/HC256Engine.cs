using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057A RID: 1402
	public class HC256Engine : IStreamCipher
	{
		// Token: 0x060034D8 RID: 13528 RVA: 0x00142F14 File Offset: 0x00141114
		private uint Step()
		{
			uint num = this.cnt & 1023U;
			uint result;
			if (this.cnt < 1024U)
			{
				uint num2 = this.p[(int)(num - 3U & 1023U)];
				uint num3 = this.p[(int)(num - 1023U & 1023U)];
				this.p[(int)num] += this.p[(int)(num - 10U & 1023U)] + (HC256Engine.RotateRight(num2, 10) ^ HC256Engine.RotateRight(num3, 23)) + this.q[(int)((num2 ^ num3) & 1023U)];
				num2 = this.p[(int)(num - 12U & 1023U)];
				result = (this.q[(int)(num2 & 255U)] + this.q[(int)((num2 >> 8 & 255U) + 256U)] + this.q[(int)((num2 >> 16 & 255U) + 512U)] + this.q[(int)((num2 >> 24 & 255U) + 768U)] ^ this.p[(int)num]);
			}
			else
			{
				uint num4 = this.q[(int)(num - 3U & 1023U)];
				uint num5 = this.q[(int)(num - 1023U & 1023U)];
				this.q[(int)num] += this.q[(int)(num - 10U & 1023U)] + (HC256Engine.RotateRight(num4, 10) ^ HC256Engine.RotateRight(num5, 23)) + this.p[(int)((num4 ^ num5) & 1023U)];
				num4 = this.q[(int)(num - 12U & 1023U)];
				result = (this.p[(int)(num4 & 255U)] + this.p[(int)((num4 >> 8 & 255U) + 256U)] + this.p[(int)((num4 >> 16 & 255U) + 512U)] + this.p[(int)((num4 >> 24 & 255U) + 768U)] ^ this.q[(int)num]);
			}
			this.cnt = (this.cnt + 1U & 2047U);
			return result;
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0014311C File Offset: 0x0014131C
		private void Init()
		{
			if (this.key.Length != 32 && this.key.Length != 16)
			{
				throw new ArgumentException("The key must be 128/256 bits long");
			}
			if (this.iv.Length < 16)
			{
				throw new ArgumentException("The IV must be at least 128 bits long");
			}
			if (this.key.Length != 32)
			{
				byte[] destinationArray = new byte[32];
				Array.Copy(this.key, 0, destinationArray, 0, this.key.Length);
				Array.Copy(this.key, 0, destinationArray, 16, this.key.Length);
				this.key = destinationArray;
			}
			if (this.iv.Length < 32)
			{
				byte[] array = new byte[32];
				Array.Copy(this.iv, 0, array, 0, this.iv.Length);
				Array.Copy(this.iv, 0, array, this.iv.Length, array.Length - this.iv.Length);
				this.iv = array;
			}
			this.idx = 0;
			this.cnt = 0U;
			uint[] array2 = new uint[2560];
			for (int i = 0; i < 32; i++)
			{
				array2[i >> 2] |= (uint)((uint)this.key[i] << 8 * (i & 3));
			}
			for (int j = 0; j < 32; j++)
			{
				array2[(j >> 2) + 8] |= (uint)((uint)this.iv[j] << 8 * (j & 3));
			}
			for (uint num = 16U; num < 2560U; num += 1U)
			{
				uint num2 = array2[(int)(num - 2U)];
				uint num3 = array2[(int)(num - 15U)];
				array2[(int)num] = (HC256Engine.RotateRight(num2, 17) ^ HC256Engine.RotateRight(num2, 19) ^ num2 >> 10) + array2[(int)(num - 7U)] + (HC256Engine.RotateRight(num3, 7) ^ HC256Engine.RotateRight(num3, 18) ^ num3 >> 3) + array2[(int)(num - 16U)] + num;
			}
			Array.Copy(array2, 512, this.p, 0, 1024);
			Array.Copy(array2, 1536, this.q, 0, 1024);
			for (int k = 0; k < 4096; k++)
			{
				this.Step();
			}
			this.cnt = 0U;
		}

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x060034DA RID: 13530 RVA: 0x00143332 File Offset: 0x00141532
		public virtual string AlgorithmName
		{
			get
			{
				return "HC-256";
			}
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0014333C File Offset: 0x0014153C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			ICipherParameters cipherParameters = parameters;
			if (parameters is ParametersWithIV)
			{
				this.iv = ((ParametersWithIV)parameters).GetIV();
				cipherParameters = ((ParametersWithIV)parameters).Parameters;
			}
			else
			{
				this.iv = new byte[0];
			}
			if (cipherParameters is KeyParameter)
			{
				this.key = ((KeyParameter)cipherParameters).GetKey();
				this.Init();
				this.initialised = true;
				return;
			}
			throw new ArgumentException("Invalid parameter passed to HC256 init - " + Platform.GetTypeName(parameters), "parameters");
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x001433C1 File Offset: 0x001415C1
		private byte GetByte()
		{
			if (this.idx == 0)
			{
				Pack.UInt32_To_LE(this.Step(), this.buf);
			}
			byte result = this.buf[this.idx];
			this.idx = (this.idx + 1 & 3);
			return result;
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x001433FC File Offset: 0x001415FC
		public virtual void ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			if (!this.initialised)
			{
				throw new InvalidOperationException(this.AlgorithmName + " not initialised");
			}
			Check.DataLength(input, inOff, len, "input buffer too short");
			Check.OutputLength(output, outOff, len, "output buffer too short");
			for (int i = 0; i < len; i++)
			{
				output[outOff + i] = (input[inOff + i] ^ this.GetByte());
			}
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x00143463 File Offset: 0x00141663
		public virtual void Reset()
		{
			this.Init();
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x0014346B File Offset: 0x0014166B
		public virtual byte ReturnByte(byte input)
		{
			return input ^ this.GetByte();
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x00142AB4 File Offset: 0x00140CB4
		private static uint RotateRight(uint x, int bits)
		{
			return x >> bits | x << -bits;
		}

		// Token: 0x04002299 RID: 8857
		private uint[] p = new uint[1024];

		// Token: 0x0400229A RID: 8858
		private uint[] q = new uint[1024];

		// Token: 0x0400229B RID: 8859
		private uint cnt;

		// Token: 0x0400229C RID: 8860
		private byte[] key;

		// Token: 0x0400229D RID: 8861
		private byte[] iv;

		// Token: 0x0400229E RID: 8862
		private bool initialised;

		// Token: 0x0400229F RID: 8863
		private byte[] buf = new byte[4];

		// Token: 0x040022A0 RID: 8864
		private int idx;
	}
}
