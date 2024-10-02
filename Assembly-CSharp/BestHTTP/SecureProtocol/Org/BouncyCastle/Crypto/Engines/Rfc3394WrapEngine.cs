using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000588 RID: 1416
	public class Rfc3394WrapEngine : IWrapper
	{
		// Token: 0x06003574 RID: 13684 RVA: 0x00147002 File Offset: 0x00145202
		public Rfc3394WrapEngine(IBlockCipher engine)
		{
			this.engine = engine;
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x00147028 File Offset: 0x00145228
		public virtual void Init(bool forWrapping, ICipherParameters parameters)
		{
			this.forWrapping = forWrapping;
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (parameters is KeyParameter)
			{
				this.param = (KeyParameter)parameters;
				return;
			}
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] array = parametersWithIV.GetIV();
				if (array.Length != 8)
				{
					throw new ArgumentException("IV length not equal to 8", "parameters");
				}
				this.iv = array;
				this.param = (KeyParameter)parametersWithIV.Parameters;
			}
		}

		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x001470AA File Offset: 0x001452AA
		public virtual string AlgorithmName
		{
			get
			{
				return this.engine.AlgorithmName;
			}
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x001470B8 File Offset: 0x001452B8
		public virtual byte[] Wrap(byte[] input, int inOff, int inLen)
		{
			if (!this.forWrapping)
			{
				throw new InvalidOperationException("not set for wrapping");
			}
			int num = inLen / 8;
			if (num * 8 != inLen)
			{
				throw new DataLengthException("wrap data must be a multiple of 8 bytes");
			}
			byte[] array = new byte[inLen + this.iv.Length];
			byte[] array2 = new byte[8 + this.iv.Length];
			Array.Copy(this.iv, 0, array, 0, this.iv.Length);
			Array.Copy(input, inOff, array, this.iv.Length, inLen);
			this.engine.Init(true, this.param);
			for (int num2 = 0; num2 != 6; num2++)
			{
				for (int i = 1; i <= num; i++)
				{
					Array.Copy(array, 0, array2, 0, this.iv.Length);
					Array.Copy(array, 8 * i, array2, this.iv.Length, 8);
					this.engine.ProcessBlock(array2, 0, array2, 0);
					int num3 = num * num2 + i;
					int num4 = 1;
					while (num3 != 0)
					{
						byte b = (byte)num3;
						byte[] array3 = array2;
						int num5 = this.iv.Length - num4;
						array3[num5] ^= b;
						num3 = (int)((uint)num3 >> 8);
						num4++;
					}
					Array.Copy(array2, 0, array, 0, 8);
					Array.Copy(array2, 8, array, 8 * i, 8);
				}
			}
			return array;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x001471F8 File Offset: 0x001453F8
		public virtual byte[] Unwrap(byte[] input, int inOff, int inLen)
		{
			if (this.forWrapping)
			{
				throw new InvalidOperationException("not set for unwrapping");
			}
			int num = inLen / 8;
			if (num * 8 != inLen)
			{
				throw new InvalidCipherTextException("unwrap data must be a multiple of 8 bytes");
			}
			byte[] array = new byte[inLen - this.iv.Length];
			byte[] array2 = new byte[this.iv.Length];
			byte[] array3 = new byte[8 + this.iv.Length];
			Array.Copy(input, inOff, array2, 0, this.iv.Length);
			Array.Copy(input, inOff + this.iv.Length, array, 0, inLen - this.iv.Length);
			this.engine.Init(false, this.param);
			num--;
			for (int i = 5; i >= 0; i--)
			{
				for (int j = num; j >= 1; j--)
				{
					Array.Copy(array2, 0, array3, 0, this.iv.Length);
					Array.Copy(array, 8 * (j - 1), array3, this.iv.Length, 8);
					int num2 = num * i + j;
					int num3 = 1;
					while (num2 != 0)
					{
						byte b = (byte)num2;
						byte[] array4 = array3;
						int num4 = this.iv.Length - num3;
						array4[num4] ^= b;
						num2 = (int)((uint)num2 >> 8);
						num3++;
					}
					this.engine.ProcessBlock(array3, 0, array3, 0);
					Array.Copy(array3, 0, array2, 0, 8);
					Array.Copy(array3, 8, array, 8 * (j - 1), 8);
				}
			}
			if (!Arrays.ConstantTimeAreEqual(array2, this.iv))
			{
				throw new InvalidCipherTextException("checksum failed");
			}
			return array;
		}

		// Token: 0x040022F0 RID: 8944
		private readonly IBlockCipher engine;

		// Token: 0x040022F1 RID: 8945
		private KeyParameter param;

		// Token: 0x040022F2 RID: 8946
		private bool forWrapping;

		// Token: 0x040022F3 RID: 8947
		private byte[] iv = new byte[]
		{
			166,
			166,
			166,
			166,
			166,
			166,
			166,
			166
		};
	}
}
