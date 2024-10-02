using System;
using System.IO;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F9 RID: 2041
	public sealed class WriteOnlyBufferedStream : Stream
	{
		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x0600488E RID: 18574 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06004890 RID: 18576 RVA: 0x00199A5E File Offset: 0x00197C5E
		public override long Length
		{
			get
			{
				return (long)this.buffer.Length;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06004891 RID: 18577 RVA: 0x00199A69 File Offset: 0x00197C69
		// (set) Token: 0x06004892 RID: 18578 RVA: 0x001992E4 File Offset: 0x001974E4
		public override long Position
		{
			get
			{
				return (long)this._position;
			}
			set
			{
				throw new NotImplementedException("Position set");
			}
		}

		// Token: 0x06004893 RID: 18579 RVA: 0x00199A72 File Offset: 0x00197C72
		public WriteOnlyBufferedStream(Stream stream, int bufferSize)
		{
			this.stream = stream;
			this.buffer = VariableSizedBufferPool.Get((long)bufferSize, true);
			this._position = 0;
		}

		// Token: 0x06004894 RID: 18580 RVA: 0x00199A96 File Offset: 0x00197C96
		public override void Flush()
		{
			if (this._position > 0)
			{
				this.stream.Write(this.buffer, 0, this._position);
				this._position = 0;
			}
		}

		// Token: 0x06004895 RID: 18581 RVA: 0x00199AC0 File Offset: 0x00197CC0
		public override void Write(byte[] bufferFrom, int offset, int count)
		{
			while (count > 0)
			{
				int num = Math.Min(count, this.buffer.Length - this._position);
				Array.Copy(bufferFrom, offset, this.buffer, this._position, num);
				this._position += num;
				offset += num;
				count -= num;
				if (this._position == this.buffer.Length)
				{
					this.Flush();
				}
			}
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x0008D53F File Offset: 0x0008B73F
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x0000248C File Offset: 0x0000068C
		public override void SetLength(long value)
		{
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00199B2B File Offset: 0x00197D2B
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (this.buffer != null)
			{
				VariableSizedBufferPool.Release(this.buffer);
			}
			this.buffer = null;
		}

		// Token: 0x04002F25 RID: 12069
		private int _position;

		// Token: 0x04002F26 RID: 12070
		private byte[] buffer;

		// Token: 0x04002F27 RID: 12071
		private Stream stream;
	}
}
