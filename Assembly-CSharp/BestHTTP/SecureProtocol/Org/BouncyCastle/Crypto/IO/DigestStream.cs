using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x0200053F RID: 1343
	public class DigestStream : Stream
	{
		// Token: 0x060032E2 RID: 13026 RVA: 0x00132803 File Offset: 0x00130A03
		public DigestStream(Stream stream, IDigest readDigest, IDigest writeDigest)
		{
			this.stream = stream;
			this.inDigest = readDigest;
			this.outDigest = writeDigest;
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00132820 File Offset: 0x00130A20
		public virtual IDigest ReadDigest()
		{
			return this.inDigest;
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00132828 File Offset: 0x00130A28
		public virtual IDigest WriteDigest()
		{
			return this.outDigest;
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00132830 File Offset: 0x00130A30
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.stream.Read(buffer, offset, count);
			if (this.inDigest != null && num > 0)
			{
				this.inDigest.BlockUpdate(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x00132868 File Offset: 0x00130A68
		public override int ReadByte()
		{
			int num = this.stream.ReadByte();
			if (this.inDigest != null && num >= 0)
			{
				this.inDigest.Update((byte)num);
			}
			return num;
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x0013289B File Offset: 0x00130A9B
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.outDigest != null && count > 0)
			{
				this.outDigest.BlockUpdate(buffer, offset, count);
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x001328C5 File Offset: 0x00130AC5
		public override void WriteByte(byte b)
		{
			if (this.outDigest != null)
			{
				this.outDigest.Update(b);
			}
			this.stream.WriteByte(b);
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x001328E7 File Offset: 0x00130AE7
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x060032EA RID: 13034 RVA: 0x001328F4 File Offset: 0x00130AF4
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x00132901 File Offset: 0x00130B01
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x060032EC RID: 13036 RVA: 0x0013290E File Offset: 0x00130B0E
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x0013291B File Offset: 0x00130B1B
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x00132928 File Offset: 0x00130B28
		public override long Position
		{
			get
			{
				return this.stream.Position;
			}
			set
			{
				this.stream.Position = value;
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x00132936 File Offset: 0x00130B36
		public override void Close()
		{
			Platform.Dispose(this.stream);
			base.Close();
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x00132949 File Offset: 0x00130B49
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x00132956 File Offset: 0x00130B56
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x00132965 File Offset: 0x00130B65
		public override void SetLength(long length)
		{
			this.stream.SetLength(length);
		}

		// Token: 0x0400216F RID: 8559
		protected readonly Stream stream;

		// Token: 0x04002170 RID: 8560
		protected readonly IDigest inDigest;

		// Token: 0x04002171 RID: 8561
		protected readonly IDigest outDigest;
	}
}
