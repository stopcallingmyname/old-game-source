using System;
using System.IO;
using System.Text;
using BestHTTP.Extensions;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020007FF RID: 2047
	public class GZipStream : Stream
	{
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060048DE RID: 18654 RVA: 0x0019C328 File Offset: 0x0019A528
		// (set) Token: 0x060048DF RID: 18655 RVA: 0x0019C330 File Offset: 0x0019A530
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060048E0 RID: 18656 RVA: 0x0019C34C File Offset: 0x0019A54C
		// (set) Token: 0x060048E1 RID: 18657 RVA: 0x0019C354 File Offset: 0x0019A554
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060048E2 RID: 18658 RVA: 0x0019C3F3 File Offset: 0x0019A5F3
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0019C3FB File Offset: 0x0019A5FB
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0019C407 File Offset: 0x0019A607
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0019C413 File Offset: 0x0019A613
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0019C41F File Offset: 0x0019A61F
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060048E7 RID: 18663 RVA: 0x0019C43C File Offset: 0x0019A63C
		// (set) Token: 0x060048E8 RID: 18664 RVA: 0x0019C449 File Offset: 0x0019A649
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x0019C46A File Offset: 0x0019A66A
		// (set) Token: 0x060048EA RID: 18666 RVA: 0x0019C478 File Offset: 0x0019A678
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x0019C4E4 File Offset: 0x0019A6E4
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060048EC RID: 18668 RVA: 0x0019C4F6 File Offset: 0x0019A6F6
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0019C508 File Offset: 0x0019A708
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060048EE RID: 18670 RVA: 0x0019C568 File Offset: 0x0019A768
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060048EF RID: 18671 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x060048F0 RID: 18672 RVA: 0x0019C58D File Offset: 0x0019A78D
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0019C5B2 File Offset: 0x0019A7B2
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060048F2 RID: 18674 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060048F3 RID: 18675 RVA: 0x0019C5D4 File Offset: 0x0019A7D4
		// (set) Token: 0x060048F4 RID: 18676 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x0019C638 File Offset: 0x0019A838
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x0019C697 File Offset: 0x0019A897
		public override void SetLength(long value)
		{
			this._baseStream.SetLength(value);
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0019C6A8 File Offset: 0x0019A8A8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x0019C708 File Offset: 0x0019A908
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = VariableSizedBufferPool.Get((long)(10 + num + num2), false);
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			int result = array3.Length;
			VariableSizedBufferPool.Release(array3);
			return result;
		}

		// Token: 0x04002F7E RID: 12158
		public DateTime? LastModified;

		// Token: 0x04002F7F RID: 12159
		private int _headerByteCount;

		// Token: 0x04002F80 RID: 12160
		internal ZlibBaseStream _baseStream;

		// Token: 0x04002F81 RID: 12161
		private bool _disposed;

		// Token: 0x04002F82 RID: 12162
		private bool _firstReadDone;

		// Token: 0x04002F83 RID: 12163
		private string _FileName;

		// Token: 0x04002F84 RID: 12164
		private string _Comment;

		// Token: 0x04002F85 RID: 12165
		private int _Crc32;

		// Token: 0x04002F86 RID: 12166
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x04002F87 RID: 12167
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
