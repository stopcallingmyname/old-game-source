using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000274 RID: 628
	public class ZOutputStream : Stream
	{
		// Token: 0x06001716 RID: 5910 RVA: 0x000B6ACD File Offset: 0x000B4CCD
		private static ZStream GetDefaultZStream(bool nowrap)
		{
			ZStream zstream = new ZStream();
			zstream.inflateInit(nowrap);
			return zstream;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x000B6DDF File Offset: 0x000B4FDF
		public ZOutputStream(Stream output) : this(output, false)
		{
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x000B6DE9 File Offset: 0x000B4FE9
		public ZOutputStream(Stream output, bool nowrap) : this(output, ZOutputStream.GetDefaultZStream(nowrap))
		{
		}

		// Token: 0x06001719 RID: 5913 RVA: 0x000B6DF8 File Offset: 0x000B4FF8
		public ZOutputStream(Stream output, ZStream z)
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
			this.output = output;
			this.compress = (z.istate == null);
			this.z = z;
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x000B6E65 File Offset: 0x000B5065
		public ZOutputStream(Stream output, int level) : this(output, level, false)
		{
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x000B6E70 File Offset: 0x000B5070
		public ZOutputStream(Stream output, int level, bool nowrap)
		{
			this.buf = new byte[512];
			this.buf1 = new byte[1];
			base..ctor();
			this.output = output;
			this.compress = true;
			this.z = new ZStream();
			this.z.deflateInit(level, nowrap);
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600171E RID: 5918 RVA: 0x000B6EC6 File Offset: 0x000B50C6
		public sealed override bool CanWrite
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x000B6ED1 File Offset: 0x000B50D1
		public override void Close()
		{
			if (this.closed)
			{
				return;
			}
			this.DoClose();
			base.Close();
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x000B6EE8 File Offset: 0x000B50E8
		private void DoClose()
		{
			try
			{
				this.Finish();
			}
			catch (IOException)
			{
			}
			finally
			{
				this.closed = true;
				this.End();
				Platform.Dispose(this.output);
				this.output = null;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000B6F3C File Offset: 0x000B513C
		public virtual void End()
		{
			if (this.z == null)
			{
				return;
			}
			if (this.compress)
			{
				this.z.deflateEnd();
			}
			else
			{
				this.z.inflateEnd();
			}
			this.z.free();
			this.z = null;
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x000B6F7C File Offset: 0x000B517C
		public virtual void Finish()
		{
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = this.buf.Length;
				int num = this.compress ? this.z.deflate(4) : this.z.inflate(4);
				if (num != 1 && num != 0)
				{
					break;
				}
				int num2 = this.buf.Length - this.z.avail_out;
				if (num2 > 0)
				{
					this.output.Write(this.buf, 0, num2);
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					goto Block_6;
				}
			}
			throw new IOException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
			Block_6:
			this.Flush();
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x000B7065 File Offset: 0x000B5265
		public override void Flush()
		{
			this.output.Flush();
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001724 RID: 5924 RVA: 0x000B7072 File Offset: 0x000B5272
		// (set) Token: 0x06001725 RID: 5925 RVA: 0x000B707A File Offset: 0x000B527A
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

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001726 RID: 5926 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001727 RID: 5927 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x06001728 RID: 5928 RVA: 0x0008FF0D File Offset: 0x0008E10D
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

		// Token: 0x06001729 RID: 5929 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600172B RID: 5931 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600172C RID: 5932 RVA: 0x000B7083 File Offset: 0x000B5283
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600172D RID: 5933 RVA: 0x000B7090 File Offset: 0x000B5290
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}

		// Token: 0x0600172E RID: 5934 RVA: 0x000B70A0 File Offset: 0x000B52A0
		public override void Write(byte[] b, int off, int len)
		{
			if (len == 0)
			{
				return;
			}
			this.z.next_in = b;
			this.z.next_in_index = off;
			this.z.avail_in = len;
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = this.buf.Length;
				if ((this.compress ? this.z.deflate(this.flushLevel) : this.z.inflate(this.flushLevel)) != 0)
				{
					break;
				}
				this.output.Write(this.buf, 0, this.buf.Length - this.z.avail_out);
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					return;
				}
			}
			throw new IOException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
		}

		// Token: 0x0600172F RID: 5935 RVA: 0x000B71A9 File Offset: 0x000B53A9
		public override void WriteByte(byte b)
		{
			this.buf1[0] = b;
			this.Write(this.buf1, 0, 1);
		}

		// Token: 0x040017C5 RID: 6085
		private const int BufferSize = 512;

		// Token: 0x040017C6 RID: 6086
		protected ZStream z;

		// Token: 0x040017C7 RID: 6087
		protected int flushLevel;

		// Token: 0x040017C8 RID: 6088
		protected byte[] buf;

		// Token: 0x040017C9 RID: 6089
		protected byte[] buf1;

		// Token: 0x040017CA RID: 6090
		protected bool compress;

		// Token: 0x040017CB RID: 6091
		protected Stream output;

		// Token: 0x040017CC RID: 6092
		protected bool closed;
	}
}
