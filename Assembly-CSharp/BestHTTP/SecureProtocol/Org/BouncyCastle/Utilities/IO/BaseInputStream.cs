using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x02000278 RID: 632
	public abstract class BaseInputStream : Stream
	{
		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001755 RID: 5973 RVA: 0x000B7DB1 File Offset: 0x000B5FB1
		public sealed override bool CanRead
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001756 RID: 5974 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001757 RID: 5975 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x000B7DBC File Offset: 0x000B5FBC
		public override void Close()
		{
			this.closed = true;
			base.Close();
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0000248C File Offset: 0x0000068C
		public sealed override void Flush()
		{
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600175A RID: 5978 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600175B RID: 5979 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x0600175C RID: 5980 RVA: 0x0008FF0D File Offset: 0x0008E10D
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

		// Token: 0x0600175D RID: 5981 RVA: 0x000B7DCC File Offset: 0x000B5FCC
		public override int Read(byte[] buffer, int offset, int count)
		{
			int i = offset;
			try
			{
				int num = offset + count;
				while (i < num)
				{
					int num2 = this.ReadByte();
					if (num2 == -1)
					{
						break;
					}
					buffer[i++] = (byte)num2;
				}
			}
			catch (IOException)
			{
				if (i == offset)
				{
					throw;
				}
			}
			return i - offset;
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04001805 RID: 6149
		private bool closed;
	}
}
