using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027F RID: 639
	public class TeeInputStream : BaseInputStream
	{
		// Token: 0x06001795 RID: 6037 RVA: 0x000B8198 File Offset: 0x000B6398
		public TeeInputStream(Stream input, Stream tee)
		{
			this.input = input;
			this.tee = tee;
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000B81AE File Offset: 0x000B63AE
		public override void Close()
		{
			Platform.Dispose(this.input);
			Platform.Dispose(this.tee);
			base.Close();
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000B81CC File Offset: 0x000B63CC
		public override int Read(byte[] buf, int off, int len)
		{
			int num = this.input.Read(buf, off, len);
			if (num > 0)
			{
				this.tee.Write(buf, off, num);
			}
			return num;
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000B81FC File Offset: 0x000B63FC
		public override int ReadByte()
		{
			int num = this.input.ReadByte();
			if (num >= 0)
			{
				this.tee.WriteByte((byte)num);
			}
			return num;
		}

		// Token: 0x0400180A RID: 6154
		private readonly Stream input;

		// Token: 0x0400180B RID: 6155
		private readonly Stream tee;
	}
}
