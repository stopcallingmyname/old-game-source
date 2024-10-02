using System;
using System.IO;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F3 RID: 2035
	public sealed class ReadOnlyBufferedStream : Stream
	{
		// Token: 0x0600485D RID: 18525 RVA: 0x00198E64 File Offset: 0x00197064
		public ReadOnlyBufferedStream(Stream nstream) : this(nstream, 8192)
		{
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x00198E72 File Offset: 0x00197072
		public ReadOnlyBufferedStream(Stream nstream, int bufferSize)
		{
			this.stream = nstream;
			this.buf = VariableSizedBufferPool.Get((long)bufferSize, true);
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x00198E90 File Offset: 0x00197090
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (size <= this.available)
			{
				Array.Copy(this.buf, this.pos, buffer, offset, size);
				this.available -= size;
				this.pos += size;
				return size;
			}
			int num = 0;
			if (this.available > 0)
			{
				Array.Copy(this.buf, this.pos, buffer, offset, this.available);
				offset += this.available;
				num += this.available;
				this.available = 0;
				this.pos = 0;
			}
			int num2;
			for (;;)
			{
				try
				{
					this.available = this.stream.Read(this.buf, 0, this.buf.Length);
					this.pos = 0;
				}
				catch (Exception ex)
				{
					if (num > 0)
					{
						return num;
					}
					throw ex;
				}
				if (this.available < 1)
				{
					break;
				}
				num2 = size - num;
				if (num2 <= this.available)
				{
					goto Block_6;
				}
				Array.Copy(this.buf, this.pos, buffer, offset, this.available);
				offset += this.available;
				num += this.available;
				this.pos = 0;
				this.available = 0;
			}
			if (num > 0)
			{
				return num;
			}
			return 0;
			Block_6:
			Array.Copy(this.buf, this.pos, buffer, offset, num2);
			this.available -= num2;
			this.pos += num2;
			num += num2;
			return num;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x00198FFC File Offset: 0x001971FC
		public override int ReadByte()
		{
			if (this.available > 0)
			{
				this.available--;
				this.pos++;
				return (int)this.buf[this.pos - 1];
			}
			try
			{
				this.available = this.stream.Read(this.buf, 0, this.buf.Length);
				this.pos = 0;
			}
			catch
			{
				return -1;
			}
			if (this.available < 1)
			{
				return -1;
			}
			this.available--;
			this.pos++;
			return (int)this.buf[this.pos - 1];
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x001990B8 File Offset: 0x001972B8
		protected override void Dispose(bool disposing)
		{
			if (this.buf != null)
			{
				VariableSizedBufferPool.Release(this.buf);
			}
			this.buf = null;
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06004862 RID: 18530 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x000947C7 File Offset: 0x000929C7
		public override bool CanSeek
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06004864 RID: 18532 RVA: 0x000947C7 File Offset: 0x000929C7
		public override bool CanWrite
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06004866 RID: 18534 RVA: 0x000947C7 File Offset: 0x000929C7
		// (set) Token: 0x06004867 RID: 18535 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x000947C7 File Offset: 0x000929C7
		public override void Flush()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x000947C7 File Offset: 0x000929C7
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x000947C7 File Offset: 0x000929C7
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x04002F07 RID: 12039
		private Stream stream;

		// Token: 0x04002F08 RID: 12040
		public const int READBUFFER = 8192;

		// Token: 0x04002F09 RID: 12041
		private byte[] buf;

		// Token: 0x04002F0A RID: 12042
		private int available;

		// Token: 0x04002F0B RID: 12043
		private int pos;
	}
}
