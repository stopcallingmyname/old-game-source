using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000273 RID: 627
	public class ZInputStream : Stream
	{
		// Token: 0x060016FF RID: 5887 RVA: 0x000B6ACD File Offset: 0x000B4CCD
		private static ZStream GetDefaultZStream(bool nowrap)
		{
			ZStream zstream = new ZStream();
			zstream.inflateInit(nowrap);
			return zstream;
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000B6ADC File Offset: 0x000B4CDC
		public ZInputStream(Stream input) : this(input, false)
		{
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000B6AE6 File Offset: 0x000B4CE6
		public ZInputStream(Stream input, bool nowrap) : this(input, ZInputStream.GetDefaultZStream(nowrap))
		{
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x000B6AF8 File Offset: 0x000B4CF8
		public ZInputStream(Stream input, ZStream z)
		{
			this.buf = new byte[512];
			this.buf1 = new byte[1];
			base..ctor();
			if (z == null)
			{
				z = new ZStream();
			}
			if (z.istate == null && z.dstate == null)
			{
				z.inflateInit();
			}
			this.input = input;
			this.compress = (z.istate == null);
			this.z = z;
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x000B6B8E File Offset: 0x000B4D8E
		public ZInputStream(Stream input, int level) : this(input, level, false)
		{
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x000B6B9C File Offset: 0x000B4D9C
		public ZInputStream(Stream input, int level, bool nowrap)
		{
			this.buf = new byte[512];
			this.buf1 = new byte[1];
			base..ctor();
			this.input = input;
			this.compress = true;
			this.z = new ZStream();
			this.z.deflateInit(level, nowrap);
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06001705 RID: 5893 RVA: 0x000B6C1B File Offset: 0x000B4E1B
		public sealed override bool CanRead
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06001706 RID: 5894 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06001707 RID: 5895 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x000B6C26 File Offset: 0x000B4E26
		public override void Close()
		{
			if (this.closed)
			{
				return;
			}
			this.closed = true;
			Platform.Dispose(this.input);
			base.Close();
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x0000248C File Offset: 0x0000068C
		public sealed override void Flush()
		{
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x000B6C49 File Offset: 0x000B4E49
		// (set) Token: 0x0600170B RID: 5899 RVA: 0x000B6C51 File Offset: 0x000B4E51
		public virtual int FlushMode
		{
			get
			{
				return this.flushLevel;
			}
			set
			{
				this.flushLevel = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x0600170E RID: 5902 RVA: 0x0008FF0D File Offset: 0x0008E10D
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

		// Token: 0x0600170F RID: 5903 RVA: 0x000B6C5C File Offset: 0x000B4E5C
		public override int Read(byte[] b, int off, int len)
		{
			if (len == 0)
			{
				return 0;
			}
			this.z.next_out = b;
			this.z.next_out_index = off;
			this.z.avail_out = len;
			for (;;)
			{
				if (this.z.avail_in == 0 && !this.nomoreinput)
				{
					this.z.next_in_index = 0;
					this.z.avail_in = this.input.Read(this.buf, 0, this.buf.Length);
					if (this.z.avail_in <= 0)
					{
						this.z.avail_in = 0;
						this.nomoreinput = true;
					}
				}
				int num = this.compress ? this.z.deflate(this.flushLevel) : this.z.inflate(this.flushLevel);
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_9;
				}
				if ((this.nomoreinput || num == 1) && this.z.avail_out == len)
				{
					return 0;
				}
				if (this.z.avail_out != len || num != 0)
				{
					goto IL_132;
				}
			}
			return 0;
			Block_9:
			throw new IOException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
			IL_132:
			return len - this.z.avail_out;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x000B6DA8 File Offset: 0x000B4FA8
		public override int ReadByte()
		{
			if (this.Read(this.buf1, 0, 1) <= 0)
			{
				return -1;
			}
			return (int)this.buf1[0];
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001713 RID: 5907 RVA: 0x000B6DC5 File Offset: 0x000B4FC5
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x000B6DD2 File Offset: 0x000B4FD2
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040017BC RID: 6076
		private const int BufferSize = 512;

		// Token: 0x040017BD RID: 6077
		protected ZStream z;

		// Token: 0x040017BE RID: 6078
		protected int flushLevel;

		// Token: 0x040017BF RID: 6079
		protected byte[] buf;

		// Token: 0x040017C0 RID: 6080
		protected byte[] buf1;

		// Token: 0x040017C1 RID: 6081
		protected bool compress;

		// Token: 0x040017C2 RID: 6082
		protected Stream input;

		// Token: 0x040017C3 RID: 6083
		protected bool closed;

		// Token: 0x040017C4 RID: 6084
		private bool nomoreinput;
	}
}
