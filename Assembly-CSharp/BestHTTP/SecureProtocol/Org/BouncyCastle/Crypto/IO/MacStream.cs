using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x02000541 RID: 1345
	public class MacStream : Stream
	{
		// Token: 0x060032F7 RID: 13047 RVA: 0x001329AC File Offset: 0x00130BAC
		public MacStream(Stream stream, IMac readMac, IMac writeMac)
		{
			this.stream = stream;
			this.inMac = readMac;
			this.outMac = writeMac;
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x001329C9 File Offset: 0x00130BC9
		public virtual IMac ReadMac()
		{
			return this.inMac;
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x001329D1 File Offset: 0x00130BD1
		public virtual IMac WriteMac()
		{
			return this.outMac;
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x001329DC File Offset: 0x00130BDC
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.stream.Read(buffer, offset, count);
			if (this.inMac != null && num > 0)
			{
				this.inMac.BlockUpdate(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x00132A14 File Offset: 0x00130C14
		public override int ReadByte()
		{
			int num = this.stream.ReadByte();
			if (this.inMac != null && num >= 0)
			{
				this.inMac.Update((byte)num);
			}
			return num;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x00132A47 File Offset: 0x00130C47
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.outMac != null && count > 0)
			{
				this.outMac.BlockUpdate(buffer, offset, count);
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00132A71 File Offset: 0x00130C71
		public override void WriteByte(byte b)
		{
			if (this.outMac != null)
			{
				this.outMac.Update(b);
			}
			this.stream.WriteByte(b);
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x00132A93 File Offset: 0x00130C93
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x00132AA0 File Offset: 0x00130CA0
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06003300 RID: 13056 RVA: 0x00132AAD File Offset: 0x00130CAD
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x00132ABA File Offset: 0x00130CBA
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x00132AC7 File Offset: 0x00130CC7
		// (set) Token: 0x06003303 RID: 13059 RVA: 0x00132AD4 File Offset: 0x00130CD4
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

		// Token: 0x06003304 RID: 13060 RVA: 0x00132AE2 File Offset: 0x00130CE2
		public override void Close()
		{
			Platform.Dispose(this.stream);
			base.Close();
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x00132AF5 File Offset: 0x00130CF5
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x00132B02 File Offset: 0x00130D02
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x00132B11 File Offset: 0x00130D11
		public override void SetLength(long length)
		{
			this.stream.SetLength(length);
		}

		// Token: 0x04002173 RID: 8563
		protected readonly Stream stream;

		// Token: 0x04002174 RID: 8564
		protected readonly IMac inMac;

		// Token: 0x04002175 RID: 8565
		protected readonly IMac outMac;
	}
}
