using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200041D RID: 1053
	internal class DigestInputBuffer : MemoryStream
	{
		// Token: 0x06002A24 RID: 10788 RVA: 0x001101C1 File Offset: 0x0010E3C1
		internal void UpdateDigest(IDigest d)
		{
			Streams.WriteBufTo(this, new DigestInputBuffer.DigStream(d));
		}

		// Token: 0x0200093D RID: 2365
		private class DigStream : BaseOutputStream
		{
			// Token: 0x06004EC7 RID: 20167 RVA: 0x001B32E5 File Offset: 0x001B14E5
			internal DigStream(IDigest d)
			{
				this.d = d;
			}

			// Token: 0x06004EC8 RID: 20168 RVA: 0x001B32F4 File Offset: 0x001B14F4
			public override void WriteByte(byte b)
			{
				this.d.Update(b);
			}

			// Token: 0x06004EC9 RID: 20169 RVA: 0x001B3302 File Offset: 0x001B1502
			public override void Write(byte[] buf, int off, int len)
			{
				this.d.BlockUpdate(buf, off, len);
			}

			// Token: 0x040035EA RID: 13802
			private readonly IDigest d;
		}
	}
}
