using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x02000810 RID: 2064
	internal sealed class ZlibCodec
	{
		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x0600493E RID: 18750 RVA: 0x001A0C0A File Offset: 0x0019EE0A
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x001A0C12 File Offset: 0x0019EE12
		public ZlibCodec()
		{
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x001A0C2C File Offset: 0x0019EE2C
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x001A0C86 File Offset: 0x0019EE86
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x001A0C94 File Offset: 0x0019EE94
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x001A0CA3 File Offset: 0x0019EEA3
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x001A0CB4 File Offset: 0x0019EEB4
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x001A0CE9 File Offset: 0x0019EEE9
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x001A0D0A File Offset: 0x0019EF0A
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x001A0D31 File Offset: 0x0019EF31
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x001A0D51 File Offset: 0x0019EF51
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x001A0D5A File Offset: 0x0019EF5A
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x001A0D6A File Offset: 0x0019EF6A
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x001A0D7A File Offset: 0x0019EF7A
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x001A0D91 File Offset: 0x0019EF91
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x001A0DA8 File Offset: 0x0019EFA8
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x001A0DFD File Offset: 0x0019EFFD
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x001A0E1E File Offset: 0x0019F01E
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x001A0E3B File Offset: 0x0019F03B
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x001A0E5B File Offset: 0x0019F05B
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x001A0E7D File Offset: 0x0019F07D
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x001A0EB4 File Offset: 0x0019F0B4
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x001A1000 File Offset: 0x0019F200
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x04003025 RID: 12325
		public byte[] InputBuffer;

		// Token: 0x04003026 RID: 12326
		public int NextIn;

		// Token: 0x04003027 RID: 12327
		public int AvailableBytesIn;

		// Token: 0x04003028 RID: 12328
		public long TotalBytesIn;

		// Token: 0x04003029 RID: 12329
		public byte[] OutputBuffer;

		// Token: 0x0400302A RID: 12330
		public int NextOut;

		// Token: 0x0400302B RID: 12331
		public int AvailableBytesOut;

		// Token: 0x0400302C RID: 12332
		public long TotalBytesOut;

		// Token: 0x0400302D RID: 12333
		public string Message;

		// Token: 0x0400302E RID: 12334
		internal DeflateManager dstate;

		// Token: 0x0400302F RID: 12335
		internal InflateManager istate;

		// Token: 0x04003030 RID: 12336
		internal uint _Adler32;

		// Token: 0x04003031 RID: 12337
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x04003032 RID: 12338
		public int WindowBits = 15;

		// Token: 0x04003033 RID: 12339
		public CompressionStrategy Strategy;
	}
}
