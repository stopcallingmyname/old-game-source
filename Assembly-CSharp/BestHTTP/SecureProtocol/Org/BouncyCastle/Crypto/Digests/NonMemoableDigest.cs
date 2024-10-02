using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005AF RID: 1455
	public class NonMemoableDigest : IDigest
	{
		// Token: 0x06003791 RID: 14225 RVA: 0x001594E5 File Offset: 0x001576E5
		public NonMemoableDigest(IDigest baseDigest)
		{
			if (baseDigest == null)
			{
				throw new ArgumentNullException("baseDigest");
			}
			this.mBaseDigest = baseDigest;
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x06003792 RID: 14226 RVA: 0x00159502 File Offset: 0x00157702
		public virtual string AlgorithmName
		{
			get
			{
				return this.mBaseDigest.AlgorithmName;
			}
		}

		// Token: 0x06003793 RID: 14227 RVA: 0x0015950F File Offset: 0x0015770F
		public virtual int GetDigestSize()
		{
			return this.mBaseDigest.GetDigestSize();
		}

		// Token: 0x06003794 RID: 14228 RVA: 0x0015951C File Offset: 0x0015771C
		public virtual void Update(byte input)
		{
			this.mBaseDigest.Update(input);
		}

		// Token: 0x06003795 RID: 14229 RVA: 0x0015952A File Offset: 0x0015772A
		public virtual void BlockUpdate(byte[] input, int inOff, int len)
		{
			this.mBaseDigest.BlockUpdate(input, inOff, len);
		}

		// Token: 0x06003796 RID: 14230 RVA: 0x0015953A File Offset: 0x0015773A
		public virtual int DoFinal(byte[] output, int outOff)
		{
			return this.mBaseDigest.DoFinal(output, outOff);
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x00159549 File Offset: 0x00157749
		public virtual void Reset()
		{
			this.mBaseDigest.Reset();
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x00159556 File Offset: 0x00157756
		public virtual int GetByteLength()
		{
			return this.mBaseDigest.GetByteLength();
		}

		// Token: 0x0400245E RID: 9310
		protected readonly IDigest mBaseDigest;
	}
}
