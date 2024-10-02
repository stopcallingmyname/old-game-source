using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.IO
{
	// Token: 0x02000543 RID: 1347
	public class SignerStream : Stream
	{
		// Token: 0x0600330C RID: 13068 RVA: 0x00132B58 File Offset: 0x00130D58
		public SignerStream(Stream stream, ISigner readSigner, ISigner writeSigner)
		{
			this.stream = stream;
			this.inSigner = readSigner;
			this.outSigner = writeSigner;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x00132B75 File Offset: 0x00130D75
		public virtual ISigner ReadSigner()
		{
			return this.inSigner;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00132B7D File Offset: 0x00130D7D
		public virtual ISigner WriteSigner()
		{
			return this.outSigner;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00132B88 File Offset: 0x00130D88
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this.stream.Read(buffer, offset, count);
			if (this.inSigner != null && num > 0)
			{
				this.inSigner.BlockUpdate(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00132BC0 File Offset: 0x00130DC0
		public override int ReadByte()
		{
			int num = this.stream.ReadByte();
			if (this.inSigner != null && num >= 0)
			{
				this.inSigner.Update((byte)num);
			}
			return num;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x00132BF3 File Offset: 0x00130DF3
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.outSigner != null && count > 0)
			{
				this.outSigner.BlockUpdate(buffer, offset, count);
			}
			this.stream.Write(buffer, offset, count);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00132C1D File Offset: 0x00130E1D
		public override void WriteByte(byte b)
		{
			if (this.outSigner != null)
			{
				this.outSigner.Update(b);
			}
			this.stream.WriteByte(b);
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x00132C3F File Offset: 0x00130E3F
		public override bool CanRead
		{
			get
			{
				return this.stream.CanRead;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06003314 RID: 13076 RVA: 0x00132C4C File Offset: 0x00130E4C
		public override bool CanWrite
		{
			get
			{
				return this.stream.CanWrite;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x00132C59 File Offset: 0x00130E59
		public override bool CanSeek
		{
			get
			{
				return this.stream.CanSeek;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06003316 RID: 13078 RVA: 0x00132C66 File Offset: 0x00130E66
		public override long Length
		{
			get
			{
				return this.stream.Length;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x00132C73 File Offset: 0x00130E73
		// (set) Token: 0x06003318 RID: 13080 RVA: 0x00132C80 File Offset: 0x00130E80
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

		// Token: 0x06003319 RID: 13081 RVA: 0x00132C8E File Offset: 0x00130E8E
		public override void Close()
		{
			Platform.Dispose(this.stream);
			base.Close();
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00132CA1 File Offset: 0x00130EA1
		public override void Flush()
		{
			this.stream.Flush();
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00132CAE File Offset: 0x00130EAE
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.stream.Seek(offset, origin);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00132CBD File Offset: 0x00130EBD
		public override void SetLength(long length)
		{
			this.stream.SetLength(length);
		}

		// Token: 0x04002177 RID: 8567
		protected readonly Stream stream;

		// Token: 0x04002178 RID: 8568
		protected readonly ISigner inSigner;

		// Token: 0x04002179 RID: 8569
		protected readonly ISigner outSigner;
	}
}
