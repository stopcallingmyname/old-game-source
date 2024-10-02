using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Utilities
{
	// Token: 0x020006D7 RID: 1751
	[Obsolete("Use BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.FilterStream")]
	public class FilterStream : Stream
	{
		// Token: 0x06004070 RID: 16496 RVA: 0x0017F175 File Offset: 0x0017D375
		[Obsolete("Use BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.FilterStream")]
		public FilterStream(Stream s)
		{
			this.s = s;
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0017F184 File Offset: 0x0017D384
		public override bool CanRead
		{
			get
			{
				return this.s.CanRead;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0017F191 File Offset: 0x0017D391
		public override bool CanSeek
		{
			get
			{
				return this.s.CanSeek;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06004073 RID: 16499 RVA: 0x0017F19E File Offset: 0x0017D39E
		public override bool CanWrite
		{
			get
			{
				return this.s.CanWrite;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06004074 RID: 16500 RVA: 0x0017F1AB File Offset: 0x0017D3AB
		public override long Length
		{
			get
			{
				return this.s.Length;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06004075 RID: 16501 RVA: 0x0017F1B8 File Offset: 0x0017D3B8
		// (set) Token: 0x06004076 RID: 16502 RVA: 0x0017F1C5 File Offset: 0x0017D3C5
		public override long Position
		{
			get
			{
				return this.s.Position;
			}
			set
			{
				this.s.Position = value;
			}
		}

		// Token: 0x06004077 RID: 16503 RVA: 0x0017F1D3 File Offset: 0x0017D3D3
		public override void Close()
		{
			Platform.Dispose(this.s);
			base.Close();
		}

		// Token: 0x06004078 RID: 16504 RVA: 0x0017F1E6 File Offset: 0x0017D3E6
		public override void Flush()
		{
			this.s.Flush();
		}

		// Token: 0x06004079 RID: 16505 RVA: 0x0017F1F3 File Offset: 0x0017D3F3
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.s.Seek(offset, origin);
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0017F202 File Offset: 0x0017D402
		public override void SetLength(long value)
		{
			this.s.SetLength(value);
		}

		// Token: 0x0600407B RID: 16507 RVA: 0x0017F210 File Offset: 0x0017D410
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.s.Read(buffer, offset, count);
		}

		// Token: 0x0600407C RID: 16508 RVA: 0x0017F220 File Offset: 0x0017D420
		public override int ReadByte()
		{
			return this.s.ReadByte();
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0017F22D File Offset: 0x0017D42D
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.s.Write(buffer, offset, count);
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0017F23D File Offset: 0x0017D43D
		public override void WriteByte(byte value)
		{
			this.s.WriteByte(value);
		}

		// Token: 0x040028FC RID: 10492
		protected readonly Stream s;
	}
}
