using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x02000279 RID: 633
	public abstract class BaseOutputStream : Stream
	{
		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001762 RID: 5986 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001763 RID: 5987 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001764 RID: 5988 RVA: 0x000B7E20 File Offset: 0x000B6020
		public sealed override bool CanWrite
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000B7E2B File Offset: 0x000B602B
		public override void Close()
		{
			this.closed = true;
			base.Close();
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Flush()
		{
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001767 RID: 5991 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x06001769 RID: 5993 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000B7E3C File Offset: 0x000B603C
		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				this.WriteByte(buffer[i]);
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x000B7E62 File Offset: 0x000B6062
		public virtual void Write(params byte[] buffer)
		{
			this.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000B7E6F File Offset: 0x000B606F
		public override void WriteByte(byte b)
		{
			this.Write(new byte[]
			{
				b
			}, 0, 1);
		}

		// Token: 0x04001806 RID: 6150
		private bool closed;
	}
}
