using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000272 RID: 626
	[Obsolete("Use 'ZInputStream' instead")]
	public class ZInflaterInputStream : Stream
	{
		// Token: 0x060016EF RID: 5871 RVA: 0x000B68EC File Offset: 0x000B4AEC
		public ZInflaterInputStream(Stream inp) : this(inp, false)
		{
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x000B68F8 File Offset: 0x000B4AF8
		public ZInflaterInputStream(Stream inp, bool nowrap)
		{
			this.inp = inp;
			this.z.inflateInit(nowrap);
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060016F1 RID: 5873 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060016F2 RID: 5874 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060016F3 RID: 5875 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x0008D53F File Offset: 0x0008B73F
		public override long Length
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060016F5 RID: 5877 RVA: 0x0008D53F File Offset: 0x0008B73F
		// (set) Token: 0x060016F6 RID: 5878 RVA: 0x0000248C File Offset: 0x0000068C
		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Write(byte[] b, int off, int len)
		{
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x0008D53F File Offset: 0x0008B73F
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x0000248C File Offset: 0x0000068C
		public override void SetLength(long value)
		{
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x000B6970 File Offset: 0x000B4B70
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
					this.z.avail_in = this.inp.Read(this.buf, 0, 4192);
					if (this.z.avail_in <= 0)
					{
						this.z.avail_in = 0;
						this.nomoreinput = true;
					}
				}
				int num = this.z.inflate(this.flushLevel);
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_8;
				}
				if ((this.nomoreinput || num == 1) && this.z.avail_out == len)
				{
					return 0;
				}
				if (this.z.avail_out != len || num != 0)
				{
					goto IL_100;
				}
			}
			return 0;
			Block_8:
			throw new IOException("inflating: " + this.z.msg);
			IL_100:
			return len - this.z.avail_out;
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x000B6A8A File Offset: 0x000B4C8A
		public override void Flush()
		{
			this.inp.Flush();
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0000248C File Offset: 0x0000068C
		public override void WriteByte(byte b)
		{
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000B6A97 File Offset: 0x000B4C97
		public override void Close()
		{
			Platform.Dispose(this.inp);
			base.Close();
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000B6AAA File Offset: 0x000B4CAA
		public override int ReadByte()
		{
			if (this.Read(this.buf1, 0, 1) <= 0)
			{
				return -1;
			}
			return (int)(this.buf1[0] & byte.MaxValue);
		}

		// Token: 0x040017B5 RID: 6069
		protected ZStream z = new ZStream();

		// Token: 0x040017B6 RID: 6070
		protected int flushLevel;

		// Token: 0x040017B7 RID: 6071
		private const int BUFSIZE = 4192;

		// Token: 0x040017B8 RID: 6072
		protected byte[] buf = new byte[4192];

		// Token: 0x040017B9 RID: 6073
		private byte[] buf1 = new byte[1];

		// Token: 0x040017BA RID: 6074
		protected Stream inp;

		// Token: 0x040017BB RID: 6075
		private bool nomoreinput;
	}
}
