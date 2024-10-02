using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP.Decompression.Crc;
using BestHTTP.Extensions;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200080F RID: 2063
	internal class ZlibBaseStream : Stream
	{
		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06004928 RID: 18728 RVA: 0x001A007A File Offset: 0x0019E27A
		internal int Crc32
		{
			get
			{
				if (this.crc == null)
				{
					return 0;
				}
				return this.crc.Crc32Result;
			}
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x001A0091 File Offset: 0x0019E291
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen) : this(stream, compressionMode, level, flavor, leaveOpen, 15)
		{
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x001A00A4 File Offset: 0x0019E2A4
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen, int windowBits)
		{
			this._flushMode = FlushType.None;
			this._stream = stream;
			this._leaveOpen = leaveOpen;
			this._compressionMode = compressionMode;
			this._flavor = flavor;
			this._level = level;
			this.windowBitsMax = windowBits;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				this.crc = new CRC32();
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x0600492B RID: 18731 RVA: 0x001A011D File Offset: 0x0019E31D
		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x0600492C RID: 18732 RVA: 0x001A0128 File Offset: 0x0019E328
		private ZlibCodec z
		{
			get
			{
				if (this._z == null)
				{
					bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
					this._z = new ZlibCodec();
					if (this._compressionMode == CompressionMode.Decompress)
					{
						this._z.InitializeInflate(this.windowBitsMax, flag);
					}
					else
					{
						this._z.Strategy = this.Strategy;
						this._z.InitializeDeflate(this._level, this.windowBitsMax, flag);
					}
				}
				return this._z;
			}
		}

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x0600492D RID: 18733 RVA: 0x001A01A4 File Offset: 0x0019E3A4
		private byte[] workingBuffer
		{
			get
			{
				if (this._workingBuffer == null)
				{
					this._workingBuffer = VariableSizedBufferPool.Get((long)this._bufferSize, true);
				}
				return this._workingBuffer;
			}
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x001A01C8 File Offset: 0x0019E3C8
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				this._streamMode = ZlibBaseStream.StreamMode.Writer;
			}
			else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			this.z.InputBuffer = buffer;
			this._z.NextIn = offset;
			this._z.AvailableBytesIn = count;
			for (;;)
			{
				this._z.OutputBuffer = this.workingBuffer;
				this._z.NextOut = 0;
				this._z.AvailableBytesOut = this._workingBuffer.Length;
				int num = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
				if (num != 0 && num != 1)
				{
					break;
				}
				this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
				bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
				if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
				{
					flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
				}
				if (flag)
				{
					return;
				}
			}
			throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x001A0350 File Offset: 0x0019E550
		private void finish()
		{
			if (this._z == null)
			{
				return;
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				int num;
				for (;;)
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					num = (this._wantCompress ? this._z.Deflate(FlushType.Finish) : this._z.Inflate(FlushType.Finish));
					if (num != 1 && num != 0)
					{
						break;
					}
					if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
					{
						this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					}
					bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
					if (flag)
					{
						goto Block_12;
					}
				}
				string text = (this._wantCompress ? "de" : "in") + "flating";
				if (this._z.Message == null)
				{
					throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
				}
				throw new ZlibException(text + ": " + this._z.Message);
				Block_12:
				this.Flush();
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					if (this._wantCompress)
					{
						int crc32Result = this.crc.Crc32Result;
						this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
						int value = (int)(this.crc.TotalBytesRead & (long)((ulong)-1));
						this._stream.Write(BitConverter.GetBytes(value), 0, 4);
						return;
					}
					throw new ZlibException("Writing with decompression is not supported.");
				}
			}
			else if (this._streamMode == ZlibBaseStream.StreamMode.Reader && this._flavor == ZlibStreamFlavor.GZIP)
			{
				if (this._wantCompress)
				{
					throw new ZlibException("Reading with compression is not supported.");
				}
				if (this._z.TotalBytesOut == 0L)
				{
					return;
				}
				byte[] array = VariableSizedBufferPool.Get(8L, true);
				if (this._z.AvailableBytesIn < 8)
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, this._z.AvailableBytesIn);
					int num2 = 8 - this._z.AvailableBytesIn;
					int num3 = this._stream.Read(array, this._z.AvailableBytesIn, num2);
					if (num2 != num3)
					{
						VariableSizedBufferPool.Release(array);
						throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + num3));
					}
				}
				else
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, 8);
				}
				int num4 = BitConverter.ToInt32(array, 0);
				int crc32Result2 = this.crc.Crc32Result;
				int num5 = BitConverter.ToInt32(array, 4);
				int num6 = (int)(this._z.TotalBytesOut & (long)((ulong)-1));
				if (crc32Result2 != num4)
				{
					VariableSizedBufferPool.Release(array);
					throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
				}
				if (num6 != num5)
				{
					VariableSizedBufferPool.Release(array);
					throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
				}
				VariableSizedBufferPool.Release(array);
				return;
			}
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x001A06C0 File Offset: 0x0019E8C0
		private void end()
		{
			if (this.z == null)
			{
				return;
			}
			if (this._wantCompress)
			{
				this._z.EndDeflate();
			}
			else
			{
				this._z.EndInflate();
			}
			this._z = null;
			VariableSizedBufferPool.Release(this._workingBuffer);
			this._workingBuffer = null;
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x001A0714 File Offset: 0x0019E914
		public override void Close()
		{
			if (this._stream == null)
			{
				return;
			}
			try
			{
				this.finish();
			}
			finally
			{
				this.end();
				if (!this._leaveOpen)
				{
					this._stream.Dispose();
				}
				this._stream = null;
			}
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x001A0764 File Offset: 0x0019E964
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x000947C7 File Offset: 0x000929C7
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x001A0771 File Offset: 0x0019E971
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
			this.nomoreinput = false;
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x001A0788 File Offset: 0x0019E988
		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			while (this._stream.Read(this._buf1, 0, 1) == 1)
			{
				if (this._buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this._buf1[0]);
				}
				if (flag)
				{
					byte[] array = list.ToArray();
					return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x001A07F4 File Offset: 0x0019E9F4
		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = VariableSizedBufferPool.Get(10L, true);
			int num2 = this._stream.Read(array, 0, 10);
			if (num2 == 0)
			{
				VariableSizedBufferPool.Release(array);
				return 0;
			}
			if (num2 != 10)
			{
				VariableSizedBufferPool.Release(array);
				throw new ZlibException("Not a valid GZIP stream.");
			}
			if (array[0] != 31 || array[1] != 139 || array[2] != 8)
			{
				VariableSizedBufferPool.Release(array);
				throw new ZlibException("Bad GZIP header.");
			}
			int num3 = BitConverter.ToInt32(array, 4);
			this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
			num += num2;
			if ((array[3] & 4) == 4)
			{
				num2 = this._stream.Read(array, 0, 2);
				num += num2;
				short num4 = (short)((int)array[0] + (int)array[1] * 256);
				byte[] buffer = VariableSizedBufferPool.Get((long)num4, true);
				num2 = this._stream.Read(buffer, 0, (int)num4);
				if (num2 != (int)num4)
				{
					VariableSizedBufferPool.Release(buffer);
					VariableSizedBufferPool.Release(array);
					throw new ZlibException("Unexpected end-of-file reading GZIP header.");
				}
				num += num2;
			}
			if ((array[3] & 8) == 8)
			{
				this._GzipFileName = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 16) == 16)
			{
				this._GzipComment = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 2) == 2)
			{
				this.Read(this._buf1, 0, 1);
			}
			VariableSizedBufferPool.Release(array);
			return num;
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x001A0938 File Offset: 0x0019EB38
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = ZlibBaseStream.StreamMode.Reader;
				this.z.AvailableBytesIn = 0;
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
					if (this._gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.nomoreinput && this._wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < buffer.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._z.OutputBuffer = buffer;
			this._z.NextOut = offset;
			this._z.AvailableBytesOut = count;
			this._z.InputBuffer = this.workingBuffer;
			int num;
			for (;;)
			{
				if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
				{
					this._z.NextIn = 0;
					this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
					if (this._z.AvailableBytesIn == 0)
					{
						this.nomoreinput = true;
					}
				}
				num = (this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode));
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_20;
				}
				if (((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count) || this._z.AvailableBytesOut <= 0 || this.nomoreinput || num != 0)
				{
					goto IL_20A;
				}
			}
			return 0;
			Block_20:
			throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", num, this._z.Message));
			IL_20A:
			if (this._z.AvailableBytesOut > 0)
			{
				if (num == 0)
				{
					int availableBytesIn = this._z.AvailableBytesIn;
				}
				if (this.nomoreinput && this._wantCompress)
				{
					num = this._z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
					}
				}
			}
			num = count - this._z.AvailableBytesOut;
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x06004938 RID: 18744 RVA: 0x001A0BD6 File Offset: 0x0019EDD6
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x06004939 RID: 18745 RVA: 0x001A0BE3 File Offset: 0x0019EDE3
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x0600493A RID: 18746 RVA: 0x001A0BF0 File Offset: 0x0019EDF0
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x0600493B RID: 18747 RVA: 0x001A0BFD File Offset: 0x0019EDFD
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x0600493C RID: 18748 RVA: 0x000947C7 File Offset: 0x000929C7
		// (set) Token: 0x0600493D RID: 18749 RVA: 0x000947C7 File Offset: 0x000929C7
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

		// Token: 0x04003012 RID: 12306
		protected internal ZlibCodec _z;

		// Token: 0x04003013 RID: 12307
		protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;

		// Token: 0x04003014 RID: 12308
		protected internal FlushType _flushMode;

		// Token: 0x04003015 RID: 12309
		protected internal ZlibStreamFlavor _flavor;

		// Token: 0x04003016 RID: 12310
		protected internal CompressionMode _compressionMode;

		// Token: 0x04003017 RID: 12311
		protected internal CompressionLevel _level;

		// Token: 0x04003018 RID: 12312
		protected internal bool _leaveOpen;

		// Token: 0x04003019 RID: 12313
		protected internal byte[] _workingBuffer;

		// Token: 0x0400301A RID: 12314
		protected internal int _bufferSize = 16384;

		// Token: 0x0400301B RID: 12315
		protected internal int windowBitsMax;

		// Token: 0x0400301C RID: 12316
		protected internal byte[] _buf1 = new byte[1];

		// Token: 0x0400301D RID: 12317
		protected internal Stream _stream;

		// Token: 0x0400301E RID: 12318
		protected internal CompressionStrategy Strategy;

		// Token: 0x0400301F RID: 12319
		private CRC32 crc;

		// Token: 0x04003020 RID: 12320
		protected internal string _GzipFileName;

		// Token: 0x04003021 RID: 12321
		protected internal string _GzipComment;

		// Token: 0x04003022 RID: 12322
		protected internal DateTime _GzipMtime;

		// Token: 0x04003023 RID: 12323
		protected internal int _gzipHeaderByteCount;

		// Token: 0x04003024 RID: 12324
		private bool nomoreinput;

		// Token: 0x020009EB RID: 2539
		internal enum StreamMode
		{
			// Token: 0x040037DD RID: 14301
			Writer,
			// Token: 0x040037DE RID: 14302
			Reader,
			// Token: 0x040037DF RID: 14303
			Undefined
		}
	}
}
