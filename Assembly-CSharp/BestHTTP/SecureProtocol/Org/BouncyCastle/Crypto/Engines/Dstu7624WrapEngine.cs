using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000576 RID: 1398
	public class Dstu7624WrapEngine : IWrapper
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x00141D3D File Offset: 0x0013FF3D
		public Dstu7624WrapEngine(int blockSizeBits)
		{
			this.engine = new Dstu7624Engine(blockSizeBits);
			this.param = null;
			this.blockSize = blockSizeBits / 8;
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x060034AA RID: 13482 RVA: 0x00141D61 File Offset: 0x0013FF61
		public string AlgorithmName
		{
			get
			{
				return "Dstu7624WrapEngine";
			}
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00141D68 File Offset: 0x0013FF68
		public void Init(bool forWrapping, ICipherParameters parameters)
		{
			this.forWrapping = forWrapping;
			if (parameters is KeyParameter)
			{
				this.param = (KeyParameter)parameters;
				this.engine.Init(forWrapping, this.param);
				return;
			}
			throw new ArgumentException("Bad parameters passed to Dstu7624WrapEngine");
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x00141DA4 File Offset: 0x0013FFA4
		public byte[] Wrap(byte[] input, int inOff, int length)
		{
			if (!this.forWrapping)
			{
				throw new InvalidOperationException("Not set for wrapping");
			}
			if (length % this.blockSize != 0)
			{
				throw new ArgumentException("Padding not supported");
			}
			int num = 2 * (1 + length / this.blockSize);
			int num2 = (num - 1) * 6;
			byte[] array = new byte[length + this.blockSize];
			Array.Copy(input, inOff, array, 0, length);
			byte[] array2 = new byte[this.blockSize / 2];
			Array.Copy(array, 0, array2, 0, this.blockSize / 2);
			IList list = Platform.CreateArrayList();
			int num3 = array.Length - this.blockSize / 2;
			int num4 = this.blockSize / 2;
			while (num3 != 0)
			{
				byte[] array3 = new byte[this.blockSize / 2];
				Array.Copy(array, num4, array3, 0, this.blockSize / 2);
				list.Add(array3);
				num3 -= this.blockSize / 2;
				num4 += this.blockSize / 2;
			}
			for (int i = 0; i < num2; i++)
			{
				Array.Copy(array2, 0, array, 0, this.blockSize / 2);
				Array.Copy((byte[])list[0], 0, array, this.blockSize / 2, this.blockSize / 2);
				this.engine.ProcessBlock(array, 0, array, 0);
				byte[] array4 = Pack.UInt32_To_LE((uint)(i + 1));
				for (int j = 0; j < array4.Length; j++)
				{
					byte[] array5 = array;
					int num5 = j + this.blockSize / 2;
					array5[num5] ^= array4[j];
				}
				Array.Copy(array, this.blockSize / 2, array2, 0, this.blockSize / 2);
				for (int k = 2; k < num; k++)
				{
					Array.Copy((byte[])list[k - 1], 0, (byte[])list[k - 2], 0, this.blockSize / 2);
				}
				Array.Copy(array, 0, (byte[])list[num - 2], 0, this.blockSize / 2);
			}
			Array.Copy(array2, 0, array, 0, this.blockSize / 2);
			num4 = this.blockSize / 2;
			for (int l = 0; l < num - 1; l++)
			{
				Array.Copy((byte[])list[l], 0, array, num4, this.blockSize / 2);
				num4 += this.blockSize / 2;
			}
			return array;
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00141FF4 File Offset: 0x001401F4
		public byte[] Unwrap(byte[] input, int inOff, int length)
		{
			if (this.forWrapping)
			{
				throw new InvalidOperationException("not set for unwrapping");
			}
			if (length % this.blockSize != 0)
			{
				throw new ArgumentException("Padding not supported");
			}
			int num = 2 * length / this.blockSize;
			int num2 = (num - 1) * 6;
			byte[] array = new byte[length];
			Array.Copy(input, inOff, array, 0, length);
			byte[] array2 = new byte[this.blockSize / 2];
			Array.Copy(array, 0, array2, 0, this.blockSize / 2);
			IList list = Platform.CreateArrayList();
			int num3 = array.Length - this.blockSize / 2;
			int num4 = this.blockSize / 2;
			while (num3 != 0)
			{
				byte[] array3 = new byte[this.blockSize / 2];
				Array.Copy(array, num4, array3, 0, this.blockSize / 2);
				list.Add(array3);
				num3 -= this.blockSize / 2;
				num4 += this.blockSize / 2;
			}
			for (int i = 0; i < num2; i++)
			{
				Array.Copy((byte[])list[num - 2], 0, array, 0, this.blockSize / 2);
				Array.Copy(array2, 0, array, this.blockSize / 2, this.blockSize / 2);
				byte[] array4 = Pack.UInt32_To_LE((uint)(num2 - i));
				for (int j = 0; j < array4.Length; j++)
				{
					byte[] array5 = array;
					int num5 = j + this.blockSize / 2;
					array5[num5] ^= array4[j];
				}
				this.engine.ProcessBlock(array, 0, array, 0);
				Array.Copy(array, 0, array2, 0, this.blockSize / 2);
				for (int k = 2; k < num; k++)
				{
					Array.Copy((byte[])list[num - k - 1], 0, (byte[])list[num - k], 0, this.blockSize / 2);
				}
				Array.Copy(array, this.blockSize / 2, (byte[])list[0], 0, this.blockSize / 2);
			}
			Array.Copy(array2, 0, array, 0, this.blockSize / 2);
			num4 = this.blockSize / 2;
			for (int l = 0; l < num - 1; l++)
			{
				Array.Copy((byte[])list[l], 0, array, num4, this.blockSize / 2);
				num4 += this.blockSize / 2;
			}
			byte b = 0;
			for (int m = array.Length - this.blockSize; m < array.Length; m++)
			{
				b |= array[m];
			}
			if (b != 0)
			{
				throw new InvalidCipherTextException("checksum failed");
			}
			return Arrays.CopyOfRange(array, 0, array.Length - this.blockSize);
		}

		// Token: 0x0400227C RID: 8828
		private KeyParameter param;

		// Token: 0x0400227D RID: 8829
		private Dstu7624Engine engine;

		// Token: 0x0400227E RID: 8830
		private bool forWrapping;

		// Token: 0x0400227F RID: 8831
		private int blockSize;
	}
}
