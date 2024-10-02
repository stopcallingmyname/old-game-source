using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000544 RID: 1348
	public class BaseKdfBytesGenerator : IDerivationFunction
	{
		// Token: 0x0600331D RID: 13085 RVA: 0x00132CCB File Offset: 0x00130ECB
		public BaseKdfBytesGenerator(int counterStart, IDigest digest)
		{
			this.counterStart = counterStart;
			this.digest = digest;
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00132CE4 File Offset: 0x00130EE4
		public virtual void Init(IDerivationParameters parameters)
		{
			if (parameters is KdfParameters)
			{
				KdfParameters kdfParameters = (KdfParameters)parameters;
				this.shared = kdfParameters.GetSharedSecret();
				this.iv = kdfParameters.GetIV();
				return;
			}
			if (parameters is Iso18033KdfParameters)
			{
				Iso18033KdfParameters iso18033KdfParameters = (Iso18033KdfParameters)parameters;
				this.shared = iso18033KdfParameters.GetSeed();
				this.iv = null;
				return;
			}
			throw new ArgumentException("KDF parameters required for KDF Generator");
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x0600331F RID: 13087 RVA: 0x00132D46 File Offset: 0x00130F46
		public virtual IDigest Digest
		{
			get
			{
				return this.digest;
			}
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x00132D50 File Offset: 0x00130F50
		public virtual int GenerateBytes(byte[] output, int outOff, int length)
		{
			if (output.Length - length < outOff)
			{
				throw new DataLengthException("output buffer too small");
			}
			long num = (long)length;
			int digestSize = this.digest.GetDigestSize();
			if (num > 8589934591L)
			{
				throw new ArgumentException("Output length too large");
			}
			int num2 = (int)((num + (long)digestSize - 1L) / (long)digestSize);
			byte[] array = new byte[this.digest.GetDigestSize()];
			byte[] array2 = new byte[4];
			Pack.UInt32_To_BE((uint)this.counterStart, array2, 0);
			uint num3 = (uint)(this.counterStart & -256);
			for (int i = 0; i < num2; i++)
			{
				this.digest.BlockUpdate(this.shared, 0, this.shared.Length);
				this.digest.BlockUpdate(array2, 0, 4);
				if (this.iv != null)
				{
					this.digest.BlockUpdate(this.iv, 0, this.iv.Length);
				}
				this.digest.DoFinal(array, 0);
				if (length > digestSize)
				{
					Array.Copy(array, 0, output, outOff, digestSize);
					outOff += digestSize;
					length -= digestSize;
				}
				else
				{
					Array.Copy(array, 0, output, outOff, length);
				}
				byte[] array3 = array2;
				int num4 = 3;
				byte b = array3[num4] + 1;
				array3[num4] = b;
				if (b == 0)
				{
					num3 += 256U;
					Pack.UInt32_To_BE(num3, array2, 0);
				}
			}
			this.digest.Reset();
			return (int)num;
		}

		// Token: 0x0400217A RID: 8570
		private int counterStart;

		// Token: 0x0400217B RID: 8571
		private IDigest digest;

		// Token: 0x0400217C RID: 8572
		private byte[] shared;

		// Token: 0x0400217D RID: 8573
		private byte[] iv;
	}
}
