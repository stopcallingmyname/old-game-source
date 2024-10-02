using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000275 RID: 629
	public sealed class ZStream
	{
		// Token: 0x06001730 RID: 5936 RVA: 0x000B71C2 File Offset: 0x000B53C2
		public int inflateInit()
		{
			return this.inflateInit(15);
		}

		// Token: 0x06001731 RID: 5937 RVA: 0x000B71CC File Offset: 0x000B53CC
		public int inflateInit(bool nowrap)
		{
			return this.inflateInit(15, nowrap);
		}

		// Token: 0x06001732 RID: 5938 RVA: 0x000B71D7 File Offset: 0x000B53D7
		public int inflateInit(int w)
		{
			return this.inflateInit(w, false);
		}

		// Token: 0x06001733 RID: 5939 RVA: 0x000B71E1 File Offset: 0x000B53E1
		public int inflateInit(int w, bool nowrap)
		{
			this.istate = new Inflate();
			return this.istate.inflateInit(this, nowrap ? (-w) : w);
		}

		// Token: 0x06001734 RID: 5940 RVA: 0x000B7202 File Offset: 0x000B5402
		public int inflate(int f)
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflate(this, f);
		}

		// Token: 0x06001735 RID: 5941 RVA: 0x000B721C File Offset: 0x000B541C
		public int inflateEnd()
		{
			if (this.istate == null)
			{
				return -2;
			}
			int result = this.istate.inflateEnd(this);
			this.istate = null;
			return result;
		}

		// Token: 0x06001736 RID: 5942 RVA: 0x000B723C File Offset: 0x000B543C
		public int inflateSync()
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflateSync(this);
		}

		// Token: 0x06001737 RID: 5943 RVA: 0x000B7255 File Offset: 0x000B5455
		public int inflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x06001738 RID: 5944 RVA: 0x000B7270 File Offset: 0x000B5470
		public int deflateInit(int level)
		{
			return this.deflateInit(level, 15);
		}

		// Token: 0x06001739 RID: 5945 RVA: 0x000B727B File Offset: 0x000B547B
		public int deflateInit(int level, bool nowrap)
		{
			return this.deflateInit(level, 15, nowrap);
		}

		// Token: 0x0600173A RID: 5946 RVA: 0x000B7287 File Offset: 0x000B5487
		public int deflateInit(int level, int bits)
		{
			return this.deflateInit(level, bits, false);
		}

		// Token: 0x0600173B RID: 5947 RVA: 0x000B7292 File Offset: 0x000B5492
		public int deflateInit(int level, int bits, bool nowrap)
		{
			this.dstate = new Deflate();
			return this.dstate.deflateInit(this, level, nowrap ? (-bits) : bits);
		}

		// Token: 0x0600173C RID: 5948 RVA: 0x000B72B4 File Offset: 0x000B54B4
		public int deflate(int flush)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflate(this, flush);
		}

		// Token: 0x0600173D RID: 5949 RVA: 0x000B72CE File Offset: 0x000B54CE
		public int deflateEnd()
		{
			if (this.dstate == null)
			{
				return -2;
			}
			int result = this.dstate.deflateEnd();
			this.dstate = null;
			return result;
		}

		// Token: 0x0600173E RID: 5950 RVA: 0x000B72ED File Offset: 0x000B54ED
		public int deflateParams(int level, int strategy)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflateParams(this, level, strategy);
		}

		// Token: 0x0600173F RID: 5951 RVA: 0x000B7308 File Offset: 0x000B5508
		public int deflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x06001740 RID: 5952 RVA: 0x000B7324 File Offset: 0x000B5524
		internal void flush_pending()
		{
			int pending = this.dstate.pending;
			if (pending > this.avail_out)
			{
				pending = this.avail_out;
			}
			if (pending == 0)
			{
				return;
			}
			if (this.dstate.pending_buf.Length > this.dstate.pending_out && this.next_out.Length > this.next_out_index && this.dstate.pending_buf.Length >= this.dstate.pending_out + pending)
			{
				int num = this.next_out.Length;
				int num2 = this.next_out_index + pending;
			}
			Array.Copy(this.dstate.pending_buf, this.dstate.pending_out, this.next_out, this.next_out_index, pending);
			this.next_out_index += pending;
			this.dstate.pending_out += pending;
			this.total_out += (long)pending;
			this.avail_out -= pending;
			this.dstate.pending -= pending;
			if (this.dstate.pending == 0)
			{
				this.dstate.pending_out = 0;
			}
		}

		// Token: 0x06001741 RID: 5953 RVA: 0x000B743C File Offset: 0x000B563C
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.avail_in;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.avail_in -= num;
			if (this.dstate.noheader == 0)
			{
				this.adler = this._adler.adler32(this.adler, this.next_in, this.next_in_index, num);
			}
			Array.Copy(this.next_in, this.next_in_index, buf, start, num);
			this.next_in_index += num;
			this.total_in += (long)num;
			return num;
		}

		// Token: 0x06001742 RID: 5954 RVA: 0x000B74CC File Offset: 0x000B56CC
		public void free()
		{
			this.next_in = null;
			this.next_out = null;
			this.msg = null;
			this._adler = null;
		}

		// Token: 0x040017CD RID: 6093
		private const int MAX_WBITS = 15;

		// Token: 0x040017CE RID: 6094
		private const int DEF_WBITS = 15;

		// Token: 0x040017CF RID: 6095
		private const int Z_NO_FLUSH = 0;

		// Token: 0x040017D0 RID: 6096
		private const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x040017D1 RID: 6097
		private const int Z_SYNC_FLUSH = 2;

		// Token: 0x040017D2 RID: 6098
		private const int Z_FULL_FLUSH = 3;

		// Token: 0x040017D3 RID: 6099
		private const int Z_FINISH = 4;

		// Token: 0x040017D4 RID: 6100
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x040017D5 RID: 6101
		private const int Z_OK = 0;

		// Token: 0x040017D6 RID: 6102
		private const int Z_STREAM_END = 1;

		// Token: 0x040017D7 RID: 6103
		private const int Z_NEED_DICT = 2;

		// Token: 0x040017D8 RID: 6104
		private const int Z_ERRNO = -1;

		// Token: 0x040017D9 RID: 6105
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x040017DA RID: 6106
		private const int Z_DATA_ERROR = -3;

		// Token: 0x040017DB RID: 6107
		private const int Z_MEM_ERROR = -4;

		// Token: 0x040017DC RID: 6108
		private const int Z_BUF_ERROR = -5;

		// Token: 0x040017DD RID: 6109
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x040017DE RID: 6110
		public byte[] next_in;

		// Token: 0x040017DF RID: 6111
		public int next_in_index;

		// Token: 0x040017E0 RID: 6112
		public int avail_in;

		// Token: 0x040017E1 RID: 6113
		public long total_in;

		// Token: 0x040017E2 RID: 6114
		public byte[] next_out;

		// Token: 0x040017E3 RID: 6115
		public int next_out_index;

		// Token: 0x040017E4 RID: 6116
		public int avail_out;

		// Token: 0x040017E5 RID: 6117
		public long total_out;

		// Token: 0x040017E6 RID: 6118
		public string msg;

		// Token: 0x040017E7 RID: 6119
		internal Deflate dstate;

		// Token: 0x040017E8 RID: 6120
		internal Inflate istate;

		// Token: 0x040017E9 RID: 6121
		internal int data_type;

		// Token: 0x040017EA RID: 6122
		public long adler;

		// Token: 0x040017EB RID: 6123
		internal Adler32 _adler = new Adler32();
	}
}
