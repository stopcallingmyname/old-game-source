using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000271 RID: 625
	[Obsolete("Use 'ZOutputStream' instead")]
	public class ZDeflaterOutputStream : Stream
	{
		// Token: 0x060016DD RID: 5853 RVA: 0x000B6633 File Offset: 0x000B4833
		public ZDeflaterOutputStream(Stream outp) : this(outp, 6, false)
		{
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x000B663E File Offset: 0x000B483E
		public ZDeflaterOutputStream(Stream outp, int level) : this(outp, level, false)
		{
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x000B664C File Offset: 0x000B484C
		public ZDeflaterOutputStream(Stream outp, int level, bool nowrap)
		{
			this.outp = outp;
			this.z.deflateInit(level, nowrap);
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x060016E2 RID: 5858 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x060016E3 RID: 5859 RVA: 0x0008D53F File Offset: 0x0008B73F
		public override long Length
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x0008D53F File Offset: 0x0008B73F
		// (set) Token: 0x060016E5 RID: 5861 RVA: 0x0000248C File Offset: 0x0000068C
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

		// Token: 0x060016E6 RID: 5862 RVA: 0x000B669C File Offset: 0x000B489C
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
				this.z.avail_out = 4192;
				if (this.z.deflate(this.flushLevel) != 0)
				{
					break;
				}
				if (this.z.avail_out < 4192)
				{
					this.outp.Write(this.buf, 0, 4192 - this.z.avail_out);
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					return;
				}
			}
			throw new IOException("deflating: " + this.z.msg);
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x0008D53F File Offset: 0x0008B73F
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0000248C File Offset: 0x0000068C
		public override void SetLength(long value)
		{
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000B6782 File Offset: 0x000B4982
		public override void Flush()
		{
			this.outp.Flush();
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x000B678F File Offset: 0x000B498F
		public override void WriteByte(byte b)
		{
			this.buf1[0] = b;
			this.Write(this.buf1, 0, 1);
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000B67A8 File Offset: 0x000B49A8
		public void Finish()
		{
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = 4192;
				int num = this.z.deflate(4);
				if (num != 1 && num != 0)
				{
					break;
				}
				if (4192 - this.z.avail_out > 0)
				{
					this.outp.Write(this.buf, 0, 4192 - this.z.avail_out);
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					goto Block_4;
				}
			}
			throw new IOException("deflating: " + this.z.msg);
			Block_4:
			this.Flush();
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x000B686F File Offset: 0x000B4A6F
		public void End()
		{
			if (this.z == null)
			{
				return;
			}
			this.z.deflateEnd();
			this.z.free();
			this.z = null;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x000B6898 File Offset: 0x000B4A98
		public override void Close()
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
				this.End();
				Platform.Dispose(this.outp);
				this.outp = null;
			}
			base.Close();
		}

		// Token: 0x040017AF RID: 6063
		protected ZStream z = new ZStream();

		// Token: 0x040017B0 RID: 6064
		protected int flushLevel;

		// Token: 0x040017B1 RID: 6065
		private const int BUFSIZE = 4192;

		// Token: 0x040017B2 RID: 6066
		protected byte[] buf = new byte[4192];

		// Token: 0x040017B3 RID: 6067
		private byte[] buf1 = new byte[1];

		// Token: 0x040017B4 RID: 6068
		protected Stream outp;
	}
}
