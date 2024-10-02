using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000400 RID: 1024
	public class ByteQueueStream : Stream
	{
		// Token: 0x06002960 RID: 10592 RVA: 0x0010E345 File Offset: 0x0010C545
		public ByteQueueStream()
		{
			this.buffer = new ByteQueue();
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06002961 RID: 10593 RVA: 0x0010E358 File Offset: 0x0010C558
		public virtual int Available
		{
			get
			{
				return this.buffer.Available;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06002962 RID: 10594 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06002963 RID: 10595 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06002964 RID: 10596 RVA: 0x0006AE98 File Offset: 0x00069098
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0000248C File Offset: 0x0000068C
		public override void Flush()
		{
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06002966 RID: 10598 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x0010E368 File Offset: 0x0010C568
		public virtual int Peek(byte[] buf)
		{
			int num = Math.Min(this.buffer.Available, buf.Length);
			this.buffer.Read(buf, 0, num, 0);
			return num;
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06002968 RID: 10600 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x06002969 RID: 10601 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override long Position
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

		// Token: 0x0600296A RID: 10602 RVA: 0x0010E399 File Offset: 0x0010C599
		public virtual int Read(byte[] buf)
		{
			return this.Read(buf, 0, buf.Length);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0010E3A8 File Offset: 0x0010C5A8
		public override int Read(byte[] buf, int off, int len)
		{
			int num = Math.Min(this.buffer.Available, len);
			this.buffer.RemoveData(buf, off, num, 0);
			return num;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x0010E3D7 File Offset: 0x0010C5D7
		public override int ReadByte()
		{
			if (this.buffer.Available == 0)
			{
				return -1;
			}
			return (int)(this.buffer.RemoveData(1, 0)[0] & byte.MaxValue);
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x0010E400 File Offset: 0x0010C600
		public virtual int Skip(int n)
		{
			int num = Math.Min(this.buffer.Available, n);
			this.buffer.RemoveData(num);
			return num;
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x0010E42C File Offset: 0x0010C62C
		public virtual void Write(byte[] buf)
		{
			this.buffer.AddData(buf, 0, buf.Length);
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x0010E43E File Offset: 0x0010C63E
		public override void Write(byte[] buf, int off, int len)
		{
			this.buffer.AddData(buf, off, len);
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0010E44E File Offset: 0x0010C64E
		public override void WriteByte(byte b)
		{
			this.buffer.AddData(new byte[]
			{
				b
			}, 0, 1);
		}

		// Token: 0x04001B64 RID: 7012
		private readonly ByteQueue buffer;
	}
}
