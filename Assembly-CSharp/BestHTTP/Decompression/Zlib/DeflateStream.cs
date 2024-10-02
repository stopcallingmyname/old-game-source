using System;
using System.IO;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020007FE RID: 2046
	public class DeflateStream : Stream
	{
		// Token: 0x060048C5 RID: 18629 RVA: 0x0019C065 File Offset: 0x0019A265
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x0019C071 File Offset: 0x0019A271
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0019C07D File Offset: 0x0019A27D
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0019C089 File Offset: 0x0019A289
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0019C0AD File Offset: 0x0019A2AD
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen, int windowBits)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen, windowBits);
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x0019C0D3 File Offset: 0x0019A2D3
		// (set) Token: 0x060048CB RID: 18635 RVA: 0x0019C0E0 File Offset: 0x0019A2E0
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060048CC RID: 18636 RVA: 0x0019C101 File Offset: 0x0019A301
		// (set) Token: 0x060048CD RID: 18637 RVA: 0x0019C110 File Offset: 0x0019A310
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x060048CE RID: 18638 RVA: 0x0019C17C File Offset: 0x0019A37C
		// (set) Token: 0x060048CF RID: 18639 RVA: 0x0019C189 File Offset: 0x0019A389
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x060048D0 RID: 18640 RVA: 0x0019C1AA File Offset: 0x0019A3AA
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x060048D1 RID: 18641 RVA: 0x0019C1BC File Offset: 0x0019A3BC
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0019C1D0 File Offset: 0x0019A3D0
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x0019C21C File Offset: 0x0019A41C
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x060048D5 RID: 18645 RVA: 0x0019C241 File Offset: 0x0019A441
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0019C266 File Offset: 0x0019A466
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060048D7 RID: 18647 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060048D8 RID: 18648 RVA: 0x0019C288 File Offset: 0x0019A488
		// (set) Token: 0x060048D9 RID: 18649 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0019C2D4 File Offset: 0x0019A4D4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0019C2F7 File Offset: 0x0019A4F7
		public override void SetLength(long value)
		{
			this._baseStream.SetLength(value);
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x0019C305 File Offset: 0x0019A505
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x04002F7B RID: 12155
		internal ZlibBaseStream _baseStream;

		// Token: 0x04002F7C RID: 12156
		internal Stream _innerStream;

		// Token: 0x04002F7D RID: 12157
		private bool _disposed;
	}
}
