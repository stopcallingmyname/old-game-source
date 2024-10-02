using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044A RID: 1098
	internal class SignerInputBuffer : MemoryStream
	{
		// Token: 0x06002B36 RID: 11062 RVA: 0x0011458C File Offset: 0x0011278C
		internal void UpdateSigner(ISigner s)
		{
			Streams.WriteBufTo(this, new SignerInputBuffer.SigStream(s));
		}

		// Token: 0x02000948 RID: 2376
		private class SigStream : BaseOutputStream
		{
			// Token: 0x06004EEA RID: 20202 RVA: 0x001B35A6 File Offset: 0x001B17A6
			internal SigStream(ISigner s)
			{
				this.s = s;
			}

			// Token: 0x06004EEB RID: 20203 RVA: 0x001B35B5 File Offset: 0x001B17B5
			public override void WriteByte(byte b)
			{
				this.s.Update(b);
			}

			// Token: 0x06004EEC RID: 20204 RVA: 0x001B35C3 File Offset: 0x001B17C3
			public override void Write(byte[] buf, int off, int len)
			{
				this.s.BlockUpdate(buf, off, len);
			}

			// Token: 0x04003620 RID: 13856
			private readonly ISigner s;
		}
	}
}
