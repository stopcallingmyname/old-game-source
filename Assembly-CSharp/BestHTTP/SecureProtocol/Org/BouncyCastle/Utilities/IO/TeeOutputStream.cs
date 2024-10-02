using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x02000280 RID: 640
	public class TeeOutputStream : BaseOutputStream
	{
		// Token: 0x06001799 RID: 6041 RVA: 0x000B8227 File Offset: 0x000B6427
		public TeeOutputStream(Stream output, Stream tee)
		{
			this.output = output;
			this.tee = tee;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000B823D File Offset: 0x000B643D
		public override void Close()
		{
			Platform.Dispose(this.output);
			Platform.Dispose(this.tee);
			base.Close();
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x000B825B File Offset: 0x000B645B
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.output.Write(buffer, offset, count);
			this.tee.Write(buffer, offset, count);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x000B8279 File Offset: 0x000B6479
		public override void WriteByte(byte b)
		{
			this.output.WriteByte(b);
			this.tee.WriteByte(b);
		}

		// Token: 0x0400180C RID: 6156
		private readonly Stream output;

		// Token: 0x0400180D RID: 6157
		private readonly Stream tee;
	}
}
