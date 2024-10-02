using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000579 RID: 1401
	public class HC128Engine : IStreamCipher
	{
		// Token: 0x060034C4 RID: 13508 RVA: 0x00142A40 File Offset: 0x00140C40
		private static uint F1(uint x)
		{
			return HC128Engine.RotateRight(x, 7) ^ HC128Engine.RotateRight(x, 18) ^ x >> 3;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x00142A56 File Offset: 0x00140C56
		private static uint F2(uint x)
		{
			return HC128Engine.RotateRight(x, 17) ^ HC128Engine.RotateRight(x, 19) ^ x >> 10;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00142A6E File Offset: 0x00140C6E
		private uint G1(uint x, uint y, uint z)
		{
			return (HC128Engine.RotateRight(x, 10) ^ HC128Engine.RotateRight(z, 23)) + HC128Engine.RotateRight(y, 8);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x00142A89 File Offset: 0x00140C89
		private uint G2(uint x, uint y, uint z)
		{
			return (HC128Engine.RotateLeft(x, 10) ^ HC128Engine.RotateLeft(z, 23)) + HC128Engine.RotateLeft(y, 8);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00142AA4 File Offset: 0x00140CA4
		private static uint RotateLeft(uint x, int bits)
		{
			return x << bits | x >> -bits;
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x00142AB4 File Offset: 0x00140CB4
		private static uint RotateRight(uint x, int bits)
		{
			return x >> bits | x << -bits;
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x00142AC4 File Offset: 0x00140CC4
		private uint H1(uint x)
		{
			return this.q[(int)(x & 255U)] + this.q[(int)((x >> 16 & 255U) + 256U)];
		}

		// Token: 0x060034CB RID: 13515 RVA: 0x00142AEC File Offset: 0x00140CEC
		private uint H2(uint x)
		{
			return this.p[(int)(x & 255U)] + this.p[(int)((x >> 16 & 255U) + 256U)];
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x00142B14 File Offset: 0x00140D14
		private static uint Mod1024(uint x)
		{
			return x & 1023U;
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x00142B1D File Offset: 0x00140D1D
		private static uint Mod512(uint x)
		{
			return x & 511U;
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x00142B26 File Offset: 0x00140D26
		private static uint Dim(uint x, uint y)
		{
			return HC128Engine.Mod512(x - y);
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x00142B30 File Offset: 0x00140D30
		private uint Step()
		{
			uint num = HC128Engine.Mod512(this.cnt);
			uint result;
			if (this.cnt < 512U)
			{
				this.p[(int)num] += this.G1(this.p[(int)HC128Engine.Dim(num, 3U)], this.p[(int)HC128Engine.Dim(num, 10U)], this.p[(int)HC128Engine.Dim(num, 511U)]);
				result = (this.H1(this.p[(int)HC128Engine.Dim(num, 12U)]) ^ this.p[(int)num]);
			}
			else
			{
				this.q[(int)num] += this.G2(this.q[(int)HC128Engine.Dim(num, 3U)], this.q[(int)HC128Engine.Dim(num, 10U)], this.q[(int)HC128Engine.Dim(num, 511U)]);
				result = (this.H2(this.q[(int)HC128Engine.Dim(num, 12U)]) ^ this.q[(int)num]);
			}
			this.cnt = HC128Engine.Mod1024(this.cnt + 1U);
			return result;
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00142C34 File Offset: 0x00140E34
		private void Init()
		{
			if (this.key.Length != 16)
			{
				throw new ArgumentException("The key must be 128 bits long");
			}
			this.idx = 0;
			this.cnt = 0U;
			uint[] array = new uint[1280];
			for (int i = 0; i < 16; i++)
			{
				array[i >> 2] |= (uint)((uint)this.key[i] << 8 * (i & 3));
			}
			Array.Copy(array, 0, array, 4, 4);
			int num = 0;
			while (num < this.iv.Length && num < 16)
			{
				array[(num >> 2) + 8] |= (uint)((uint)this.iv[num] << 8 * (num & 3));
				num++;
			}
			Array.Copy(array, 8, array, 12, 4);
			for (uint num2 = 16U; num2 < 1280U; num2 += 1U)
			{
				array[(int)num2] = HC128Engine.F2(array[(int)(num2 - 2U)]) + array[(int)(num2 - 7U)] + HC128Engine.F1(array[(int)(num2 - 15U)]) + array[(int)(num2 - 16U)] + num2;
			}
			Array.Copy(array, 256, this.p, 0, 512);
			Array.Copy(array, 768, this.q, 0, 512);
			for (int j = 0; j < 512; j++)
			{
				this.p[j] = this.Step();
			}
			for (int k = 0; k < 512; k++)
			{
				this.q[k] = this.Step();
			}
			this.cnt = 0U;
		}

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x060034D1 RID: 13521 RVA: 0x00142D9D File Offset: 0x00140F9D
		public virtual string AlgorithmName
		{
			get
			{
				return "HC-128";
			}
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x00142DA4 File Offset: 0x00140FA4
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
			throw new ArgumentException("Invalid parameter passed to HC128 init - " + Platform.GetTypeName(parameters), "parameters");
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x00142E29 File Offset: 0x00141029
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

		// Token: 0x060034D4 RID: 13524 RVA: 0x00142E64 File Offset: 0x00141064
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

		// Token: 0x060034D5 RID: 13525 RVA: 0x00142ECB File Offset: 0x001410CB
		public virtual void Reset()
		{
			this.Init();
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x00142ED3 File Offset: 0x001410D3
		public virtual byte ReturnByte(byte input)
		{
			return input ^ this.GetByte();
		}

		// Token: 0x04002291 RID: 8849
		private uint[] p = new uint[512];

		// Token: 0x04002292 RID: 8850
		private uint[] q = new uint[512];

		// Token: 0x04002293 RID: 8851
		private uint cnt;

		// Token: 0x04002294 RID: 8852
		private byte[] key;

		// Token: 0x04002295 RID: 8853
		private byte[] iv;

		// Token: 0x04002296 RID: 8854
		private bool initialised;

		// Token: 0x04002297 RID: 8855
		private byte[] buf = new byte[4];

		// Token: 0x04002298 RID: 8856
		private int idx;
	}
}
