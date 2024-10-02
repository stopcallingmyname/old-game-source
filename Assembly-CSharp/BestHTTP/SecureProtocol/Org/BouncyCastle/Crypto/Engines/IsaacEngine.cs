using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200057D RID: 1405
	public class IsaacEngine : IStreamCipher
	{
		// Token: 0x060034FA RID: 13562 RVA: 0x00143E80 File Offset: 0x00142080
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to ISAAC Init - " + Platform.GetTypeName(parameters), "parameters");
			}
			KeyParameter keyParameter = (KeyParameter)parameters;
			this.setKey(keyParameter.GetKey());
		}

		// Token: 0x060034FB RID: 13563 RVA: 0x00143EC4 File Offset: 0x001420C4
		public virtual byte ReturnByte(byte input)
		{
			if (this.index == 0)
			{
				this.isaac();
				this.keyStream = Pack.UInt32_To_BE(this.results);
			}
			byte result = this.keyStream[this.index] ^ input;
			this.index = (this.index + 1 & 1023);
			return result;
		}

		// Token: 0x060034FC RID: 13564 RVA: 0x00143F14 File Offset: 0x00142114
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
				if (this.index == 0)
				{
					this.isaac();
					this.keyStream = Pack.UInt32_To_BE(this.results);
				}
				output[i + outOff] = (this.keyStream[this.index] ^ input[i + inOff]);
				this.index = (this.index + 1 & 1023);
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x060034FD RID: 13565 RVA: 0x00143FB5 File Offset: 0x001421B5
		public virtual string AlgorithmName
		{
			get
			{
				return "ISAAC";
			}
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x00143FBC File Offset: 0x001421BC
		public virtual void Reset()
		{
			this.setKey(this.workingKey);
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x00143FCC File Offset: 0x001421CC
		private void setKey(byte[] keyBytes)
		{
			this.workingKey = keyBytes;
			if (this.engineState == null)
			{
				this.engineState = new uint[IsaacEngine.stateArraySize];
			}
			if (this.results == null)
			{
				this.results = new uint[IsaacEngine.stateArraySize];
			}
			for (int i = 0; i < IsaacEngine.stateArraySize; i++)
			{
				this.engineState[i] = (this.results[i] = 0U);
			}
			this.a = (this.b = (this.c = 0U));
			this.index = 0;
			byte[] array = new byte[keyBytes.Length + (keyBytes.Length & 3)];
			Array.Copy(keyBytes, 0, array, 0, keyBytes.Length);
			for (int i = 0; i < array.Length; i += 4)
			{
				this.results[i >> 2] = Pack.LE_To_UInt32(array, i);
			}
			uint[] array2 = new uint[IsaacEngine.sizeL];
			for (int i = 0; i < IsaacEngine.sizeL; i++)
			{
				array2[i] = 2654435769U;
			}
			for (int i = 0; i < 4; i++)
			{
				this.mix(array2);
			}
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < IsaacEngine.stateArraySize; j += IsaacEngine.sizeL)
				{
					for (int k = 0; k < IsaacEngine.sizeL; k++)
					{
						array2[k] += ((i < 1) ? this.results[j + k] : this.engineState[j + k]);
					}
					this.mix(array2);
					for (int k = 0; k < IsaacEngine.sizeL; k++)
					{
						this.engineState[j + k] = array2[k];
					}
				}
			}
			this.isaac();
			this.initialised = true;
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x00144158 File Offset: 0x00142358
		private void isaac()
		{
			uint num = this.b;
			uint num2 = this.c + 1U;
			this.c = num2;
			this.b = num + num2;
			for (int i = 0; i < IsaacEngine.stateArraySize; i++)
			{
				uint num3 = this.engineState[i];
				switch (i & 3)
				{
				case 0:
					this.a ^= this.a << 13;
					break;
				case 1:
					this.a ^= this.a >> 6;
					break;
				case 2:
					this.a ^= this.a << 2;
					break;
				case 3:
					this.a ^= this.a >> 16;
					break;
				}
				this.a += this.engineState[i + 128 & 255];
				uint num4 = this.engineState[i] = this.engineState[(int)(num3 >> 2 & 255U)] + this.a + this.b;
				this.results[i] = (this.b = this.engineState[(int)(num4 >> 10 & 255U)] + num3);
			}
		}

		// Token: 0x06003501 RID: 13569 RVA: 0x0014428C File Offset: 0x0014248C
		private void mix(uint[] x)
		{
			x[0] ^= x[1] << 11;
			x[3] += x[0];
			x[1] += x[2];
			x[1] ^= x[2] >> 2;
			x[4] += x[1];
			x[2] += x[3];
			x[2] ^= x[3] << 8;
			x[5] += x[2];
			x[3] += x[4];
			x[3] ^= x[4] >> 16;
			x[6] += x[3];
			x[4] += x[5];
			x[4] ^= x[5] << 10;
			x[7] += x[4];
			x[5] += x[6];
			x[5] ^= x[6] >> 4;
			x[0] += x[5];
			x[6] += x[7];
			x[6] ^= x[7] << 8;
			x[1] += x[6];
			x[7] += x[0];
			x[7] ^= x[0] >> 9;
			x[2] += x[7];
			x[0] += x[1];
		}

		// Token: 0x040022AE RID: 8878
		private static readonly int sizeL = 8;

		// Token: 0x040022AF RID: 8879
		private static readonly int stateArraySize = IsaacEngine.sizeL << 5;

		// Token: 0x040022B0 RID: 8880
		private uint[] engineState;

		// Token: 0x040022B1 RID: 8881
		private uint[] results;

		// Token: 0x040022B2 RID: 8882
		private uint a;

		// Token: 0x040022B3 RID: 8883
		private uint b;

		// Token: 0x040022B4 RID: 8884
		private uint c;

		// Token: 0x040022B5 RID: 8885
		private int index;

		// Token: 0x040022B6 RID: 8886
		private byte[] keyStream = new byte[IsaacEngine.stateArraySize << 2];

		// Token: 0x040022B7 RID: 8887
		private byte[] workingKey;

		// Token: 0x040022B8 RID: 8888
		private bool initialised;
	}
}
