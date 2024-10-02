using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027C RID: 636
	public class PushbackStream : FilterStream
	{
		// Token: 0x06001783 RID: 6019 RVA: 0x000B7F61 File Offset: 0x000B6161
		public PushbackStream(Stream s) : base(s)
		{
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000B7F71 File Offset: 0x000B6171
		public override int ReadByte()
		{
			if (this.buf != -1)
			{
				int result = this.buf;
				this.buf = -1;
				return result;
			}
			return base.ReadByte();
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x000B7F90 File Offset: 0x000B6190
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.buf != -1 && count > 0)
			{
				buffer[offset] = (byte)this.buf;
				this.buf = -1;
				return 1;
			}
			return base.Read(buffer, offset, count);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x000B7FBB File Offset: 0x000B61BB
		public virtual void Unread(int b)
		{
			if (this.buf != -1)
			{
				throw new InvalidOperationException("Can only push back one byte");
			}
			this.buf = (b & 255);
		}

		// Token: 0x04001808 RID: 6152
		private int buf = -1;
	}
}
