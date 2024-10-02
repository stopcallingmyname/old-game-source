using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200066E RID: 1646
	internal class IndefiniteLengthInputStream : LimitedInputStream
	{
		// Token: 0x06003D46 RID: 15686 RVA: 0x001737E7 File Offset: 0x001719E7
		internal IndefiniteLengthInputStream(Stream inStream, int limit) : base(inStream, limit)
		{
			this._lookAhead = this.RequireByte();
			this.CheckForEof();
		}

		// Token: 0x06003D47 RID: 15687 RVA: 0x0017380B File Offset: 0x00171A0B
		internal void SetEofOn00(bool eofOn00)
		{
			this._eofOn00 = eofOn00;
			if (this._eofOn00)
			{
				this.CheckForEof();
			}
		}

		// Token: 0x06003D48 RID: 15688 RVA: 0x00173823 File Offset: 0x00171A23
		private bool CheckForEof()
		{
			if (this._lookAhead != 0)
			{
				return this._lookAhead < 0;
			}
			if (this.RequireByte() != 0)
			{
				throw new IOException("malformed end-of-contents marker");
			}
			this._lookAhead = -1;
			this.SetParentEofDetect(true);
			return true;
		}

		// Token: 0x06003D49 RID: 15689 RVA: 0x0017385C File Offset: 0x00171A5C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._eofOn00 || count <= 1)
			{
				return base.Read(buffer, offset, count);
			}
			if (this._lookAhead < 0)
			{
				return 0;
			}
			int num = this._in.Read(buffer, offset + 1, count - 1);
			if (num <= 0)
			{
				throw new EndOfStreamException();
			}
			buffer[offset] = (byte)this._lookAhead;
			this._lookAhead = this.RequireByte();
			return num + 1;
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x001738BE File Offset: 0x00171ABE
		public override int ReadByte()
		{
			if (this._eofOn00 && this.CheckForEof())
			{
				return -1;
			}
			int lookAhead = this._lookAhead;
			this._lookAhead = this.RequireByte();
			return lookAhead;
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x001738E4 File Offset: 0x00171AE4
		private int RequireByte()
		{
			int num = this._in.ReadByte();
			if (num < 0)
			{
				throw new EndOfStreamException();
			}
			return num;
		}

		// Token: 0x04002707 RID: 9991
		private int _lookAhead;

		// Token: 0x04002708 RID: 9992
		private bool _eofOn00 = true;
	}
}
