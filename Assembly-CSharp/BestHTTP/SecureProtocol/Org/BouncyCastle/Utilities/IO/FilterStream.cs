using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200027A RID: 634
	public class FilterStream : Stream
	{
		// Token: 0x06001771 RID: 6001 RVA: 0x000B7E83 File Offset: 0x000B6083
		public FilterStream(Stream s)
		{
			this.s = s;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06001772 RID: 6002 RVA: 0x000B7E92 File Offset: 0x000B6092
		public override bool CanRead
		{
			get
			{
				return this.s.CanRead;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06001773 RID: 6003 RVA: 0x000B7E9F File Offset: 0x000B609F
		public override bool CanSeek
		{
			get
			{
				return this.s.CanSeek;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001774 RID: 6004 RVA: 0x000B7EAC File Offset: 0x000B60AC
		public override bool CanWrite
		{
			get
			{
				return this.s.CanWrite;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001775 RID: 6005 RVA: 0x000B7EB9 File Offset: 0x000B60B9
		public override long Length
		{
			get
			{
				return this.s.Length;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x000B7EC6 File Offset: 0x000B60C6
		// (set) Token: 0x06001777 RID: 6007 RVA: 0x000B7ED3 File Offset: 0x000B60D3
		public override long Position
		{
			get
			{
				return this.s.Position;
			}
			set
			{
				this.s.Position = value;
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000B7EE1 File Offset: 0x000B60E1
		public override void Close()
		{
			Platform.Dispose(this.s);
			base.Close();
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000B7EF4 File Offset: 0x000B60F4
		public override void Flush()
		{
			this.s.Flush();
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000B7F01 File Offset: 0x000B6101
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.s.Seek(offset, origin);
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000B7F10 File Offset: 0x000B6110
		public override void SetLength(long value)
		{
			this.s.SetLength(value);
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000B7F1E File Offset: 0x000B611E
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.s.Read(buffer, offset, count);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x000B7F2E File Offset: 0x000B612E
		public override int ReadByte()
		{
			return this.s.ReadByte();
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000B7F3B File Offset: 0x000B613B
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.s.Write(buffer, offset, count);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000B7F4B File Offset: 0x000B614B
		public override void WriteByte(byte value)
		{
			this.s.WriteByte(value);
		}

		// Token: 0x04001807 RID: 6151
		protected readonly Stream s;
	}
}
