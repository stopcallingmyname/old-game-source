using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005B0 RID: 1456
	public class NullDigest : IDigest
	{
		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x06003799 RID: 14233 RVA: 0x00159563 File Offset: 0x00157763
		public string AlgorithmName
		{
			get
			{
				return "NULL";
			}
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public int GetByteLength()
		{
			return 0;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x0015956A File Offset: 0x0015776A
		public int GetDigestSize()
		{
			return (int)this.bOut.Length;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x00159578 File Offset: 0x00157778
		public void Update(byte b)
		{
			this.bOut.WriteByte(b);
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00159586 File Offset: 0x00157786
		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			this.bOut.Write(inBytes, inOff, len);
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x00159598 File Offset: 0x00157798
		public int DoFinal(byte[] outBytes, int outOff)
		{
			int result;
			try
			{
				result = Streams.WriteBufTo(this.bOut, outBytes, outOff);
			}
			finally
			{
				this.Reset();
			}
			return result;
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x001595D0 File Offset: 0x001577D0
		public void Reset()
		{
			this.bOut.SetLength(0L);
		}

		// Token: 0x0400245F RID: 9311
		private readonly MemoryStream bOut = new MemoryStream();
	}
}
