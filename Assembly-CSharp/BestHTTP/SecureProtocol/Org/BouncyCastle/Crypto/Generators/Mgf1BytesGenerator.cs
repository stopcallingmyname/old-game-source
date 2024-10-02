using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000559 RID: 1369
	public class Mgf1BytesGenerator : IDerivationFunction
	{
		// Token: 0x06003380 RID: 13184 RVA: 0x00135560 File Offset: 0x00133760
		public Mgf1BytesGenerator(IDigest digest)
		{
			this.digest = digest;
			this.hLen = digest.GetDigestSize();
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x0013557C File Offset: 0x0013377C
		public void Init(IDerivationParameters parameters)
		{
			if (!typeof(MgfParameters).IsInstanceOfType(parameters))
			{
				throw new ArgumentException("MGF parameters required for MGF1Generator");
			}
			MgfParameters mgfParameters = (MgfParameters)parameters;
			this.seed = mgfParameters.GetSeed();
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x001355B9 File Offset: 0x001337B9
		public IDigest Digest
		{
			get
			{
				return this.digest;
			}
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x001212E0 File Offset: 0x0011F4E0
		private void ItoOSP(int i, byte[] sp)
		{
			sp[0] = (byte)((uint)i >> 24);
			sp[1] = (byte)((uint)i >> 16);
			sp[2] = (byte)((uint)i >> 8);
			sp[3] = (byte)i;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x001355C4 File Offset: 0x001337C4
		public int GenerateBytes(byte[] output, int outOff, int length)
		{
			if (output.Length - length < outOff)
			{
				throw new DataLengthException("output buffer too small");
			}
			byte[] array = new byte[this.hLen];
			byte[] array2 = new byte[4];
			int num = 0;
			this.digest.Reset();
			if (length > this.hLen)
			{
				do
				{
					this.ItoOSP(num, array2);
					this.digest.BlockUpdate(this.seed, 0, this.seed.Length);
					this.digest.BlockUpdate(array2, 0, array2.Length);
					this.digest.DoFinal(array, 0);
					Array.Copy(array, 0, output, outOff + num * this.hLen, this.hLen);
				}
				while (++num < length / this.hLen);
			}
			if (num * this.hLen < length)
			{
				this.ItoOSP(num, array2);
				this.digest.BlockUpdate(this.seed, 0, this.seed.Length);
				this.digest.BlockUpdate(array2, 0, array2.Length);
				this.digest.DoFinal(array, 0);
				Array.Copy(array, 0, output, outOff + num * this.hLen, length - num * this.hLen);
			}
			return length;
		}

		// Token: 0x040021B6 RID: 8630
		private IDigest digest;

		// Token: 0x040021B7 RID: 8631
		private byte[] seed;

		// Token: 0x040021B8 RID: 8632
		private int hLen;
	}
}
