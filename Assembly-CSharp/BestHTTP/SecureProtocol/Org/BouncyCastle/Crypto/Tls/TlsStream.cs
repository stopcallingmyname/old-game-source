using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200048C RID: 1164
	internal class TlsStream : Stream
	{
		// Token: 0x06002D5B RID: 11611 RVA: 0x0011BBA7 File Offset: 0x00119DA7
		internal TlsStream(TlsProtocol handler)
		{
			this.handler = handler;
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002D5C RID: 11612 RVA: 0x0011BBB6 File Offset: 0x00119DB6
		public override bool CanRead
		{
			get
			{
				return !this.handler.IsClosed;
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06002D5D RID: 11613 RVA: 0x0007D96F File Offset: 0x0007BB6F
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x0011BBB6 File Offset: 0x00119DB6
		public override bool CanWrite
		{
			get
			{
				return !this.handler.IsClosed;
			}
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x0011BBC6 File Offset: 0x00119DC6
		public override void Close()
		{
			this.handler.Close();
			base.Close();
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x0011BBD9 File Offset: 0x00119DD9
		public override void Flush()
		{
			this.handler.Flush();
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06002D62 RID: 11618 RVA: 0x0008FF0D File Offset: 0x0008E10D
		// (set) Token: 0x06002D63 RID: 11619 RVA: 0x0008FF0D File Offset: 0x0008E10D
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

		// Token: 0x06002D64 RID: 11620 RVA: 0x0011BBE6 File Offset: 0x00119DE6
		public override int Read(byte[] buf, int off, int len)
		{
			return this.handler.ReadApplicationData(buf, off, len);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x0011BBF8 File Offset: 0x00119DF8
		public override int ReadByte()
		{
			byte[] array = new byte[1];
			if (this.Read(array, 0, 1) <= 0)
			{
				return -1;
			}
			return (int)array[0];
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x0008FF0D File Offset: 0x0008E10D
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x0011BC1D File Offset: 0x00119E1D
		public override void Write(byte[] buf, int off, int len)
		{
			this.handler.WriteData(buf, off, len);
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x0011BC2D File Offset: 0x00119E2D
		public override void WriteByte(byte b)
		{
			this.handler.WriteData(new byte[]
			{
				b
			}, 0, 1);
		}

		// Token: 0x04001EAD RID: 7853
		private readonly TlsProtocol handler;
	}
}
