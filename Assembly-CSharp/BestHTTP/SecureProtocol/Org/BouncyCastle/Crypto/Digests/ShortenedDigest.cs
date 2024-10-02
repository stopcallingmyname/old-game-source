using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BD RID: 1469
	public class ShortenedDigest : IDigest
	{
		// Token: 0x0600385D RID: 14429 RVA: 0x001619F4 File Offset: 0x0015FBF4
		public ShortenedDigest(IDigest baseDigest, int length)
		{
			if (baseDigest == null)
			{
				throw new ArgumentNullException("baseDigest");
			}
			if (length > baseDigest.GetDigestSize())
			{
				throw new ArgumentException("baseDigest output not large enough to support length");
			}
			this.baseDigest = baseDigest;
			this.length = length;
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x0600385E RID: 14430 RVA: 0x00161A2C File Offset: 0x0015FC2C
		public string AlgorithmName
		{
			get
			{
				return string.Concat(new object[]
				{
					this.baseDigest.AlgorithmName,
					"(",
					this.length * 8,
					")"
				});
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00161A67 File Offset: 0x0015FC67
		public int GetDigestSize()
		{
			return this.length;
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x00161A6F File Offset: 0x0015FC6F
		public void Update(byte input)
		{
			this.baseDigest.Update(input);
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x00161A7D File Offset: 0x0015FC7D
		public void BlockUpdate(byte[] input, int inOff, int length)
		{
			this.baseDigest.BlockUpdate(input, inOff, length);
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x00161A90 File Offset: 0x0015FC90
		public int DoFinal(byte[] output, int outOff)
		{
			byte[] array = new byte[this.baseDigest.GetDigestSize()];
			this.baseDigest.DoFinal(array, 0);
			Array.Copy(array, 0, output, outOff, this.length);
			return this.length;
		}

		// Token: 0x06003863 RID: 14435 RVA: 0x00161AD1 File Offset: 0x0015FCD1
		public void Reset()
		{
			this.baseDigest.Reset();
		}

		// Token: 0x06003864 RID: 14436 RVA: 0x00161ADE File Offset: 0x0015FCDE
		public int GetByteLength()
		{
			return this.baseDigest.GetByteLength();
		}

		// Token: 0x040024B7 RID: 9399
		private IDigest baseDigest;

		// Token: 0x040024B8 RID: 9400
		private int length;
	}
}
