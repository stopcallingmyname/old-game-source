using System;
using System.IO;

namespace BestHTTP.Extensions
{
	// Token: 0x020007F4 RID: 2036
	public sealed class StreamList : Stream
	{
		// Token: 0x0600486C RID: 18540 RVA: 0x001990D4 File Offset: 0x001972D4
		public StreamList(params Stream[] streams)
		{
			this.Streams = streams;
			this.CurrentIdx = 0;
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x0600486D RID: 18541 RVA: 0x001990EA File Offset: 0x001972EA
		public override bool CanRead
		{
			get
			{
				return this.CurrentIdx < this.Streams.Length && this.Streams[this.CurrentIdx].CanRead;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x0600486E RID: 18542 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x0600486F RID: 18543 RVA: 0x00199110 File Offset: 0x00197310
		public override bool CanWrite
		{
			get
			{
				return this.CurrentIdx < this.Streams.Length && this.Streams[this.CurrentIdx].CanWrite;
			}
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x00199138 File Offset: 0x00197338
		public override void Flush()
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return;
			}
			for (int i = 0; i <= this.CurrentIdx; i++)
			{
				this.Streams[i].Flush();
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06004871 RID: 18545 RVA: 0x00199174 File Offset: 0x00197374
		public override long Length
		{
			get
			{
				if (this.CurrentIdx >= this.Streams.Length)
				{
					return 0L;
				}
				long num = 0L;
				for (int i = 0; i < this.Streams.Length; i++)
				{
					num += this.Streams[i].Length;
				}
				return num;
			}
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x001991BC File Offset: 0x001973BC
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return -1;
			}
			int i;
			for (i = this.Streams[this.CurrentIdx].Read(buffer, offset, count); i < count; i += this.Streams[this.CurrentIdx].Read(buffer, offset + i, count - i))
			{
				int currentIdx = this.CurrentIdx;
				this.CurrentIdx = currentIdx + 1;
				if (currentIdx >= this.Streams.Length)
				{
					break;
				}
			}
			return i;
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x0019922F File Offset: 0x0019742F
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return;
			}
			this.Streams[this.CurrentIdx].Write(buffer, offset, count);
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x00199258 File Offset: 0x00197458
		public void Write(string str)
		{
			byte[] asciibytes = str.GetASCIIBytes();
			this.Write(asciibytes, 0, asciibytes.Length);
			VariableSizedBufferPool.Release(asciibytes);
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x00199280 File Offset: 0x00197480
		protected override void Dispose(bool disposing)
		{
			for (int i = 0; i < this.Streams.Length; i++)
			{
				try
				{
					this.Streams[i].Dispose();
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("StreamList", "Dispose", ex);
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06004876 RID: 18550 RVA: 0x001992D8 File Offset: 0x001974D8
		// (set) Token: 0x06004877 RID: 18551 RVA: 0x001992E4 File Offset: 0x001974E4
		public override long Position
		{
			get
			{
				throw new NotImplementedException("Position get");
			}
			set
			{
				throw new NotImplementedException("Position set");
			}
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x001992F0 File Offset: 0x001974F0
		public override long Seek(long offset, SeekOrigin origin)
		{
			if (this.CurrentIdx >= this.Streams.Length)
			{
				return 0L;
			}
			return this.Streams[this.CurrentIdx].Seek(offset, origin);
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x00199319 File Offset: 0x00197519
		public override void SetLength(long value)
		{
			throw new NotImplementedException("SetLength");
		}

		// Token: 0x04002F0C RID: 12044
		private Stream[] Streams;

		// Token: 0x04002F0D RID: 12045
		private int CurrentIdx;
	}
}
