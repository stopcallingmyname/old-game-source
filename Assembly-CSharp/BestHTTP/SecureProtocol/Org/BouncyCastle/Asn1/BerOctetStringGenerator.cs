using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063B RID: 1595
	public class BerOctetStringGenerator : BerGenerator
	{
		// Token: 0x06003BDB RID: 15323 RVA: 0x0016FC98 File Offset: 0x0016DE98
		public BerOctetStringGenerator(Stream outStream) : base(outStream)
		{
			base.WriteBerHeader(36);
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x0016FCA9 File Offset: 0x0016DEA9
		public BerOctetStringGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
			base.WriteBerHeader(36);
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x0016FCBC File Offset: 0x0016DEBC
		public Stream GetOctetOutputStream()
		{
			return this.GetOctetOutputStream(new byte[1000]);
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x0016FCCE File Offset: 0x0016DECE
		public Stream GetOctetOutputStream(int bufSize)
		{
			if (bufSize >= 1)
			{
				return this.GetOctetOutputStream(new byte[bufSize]);
			}
			return this.GetOctetOutputStream();
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x0016FCE7 File Offset: 0x0016DEE7
		public Stream GetOctetOutputStream(byte[] buf)
		{
			return new BerOctetStringGenerator.BufferedBerOctetStream(this, buf);
		}

		// Token: 0x0200098C RID: 2444
		private class BufferedBerOctetStream : BaseOutputStream
		{
			// Token: 0x06004FD2 RID: 20434 RVA: 0x001B7CC9 File Offset: 0x001B5EC9
			internal BufferedBerOctetStream(BerOctetStringGenerator gen, byte[] buf)
			{
				this._gen = gen;
				this._buf = buf;
				this._off = 0;
				this._derOut = new DerOutputStream(this._gen.Out);
			}

			// Token: 0x06004FD3 RID: 20435 RVA: 0x001B7CFC File Offset: 0x001B5EFC
			public override void WriteByte(byte b)
			{
				byte[] buf = this._buf;
				int off = this._off;
				this._off = off + 1;
				buf[off] = b;
				if (this._off == this._buf.Length)
				{
					DerOctetString.Encode(this._derOut, this._buf, 0, this._off);
					this._off = 0;
				}
			}

			// Token: 0x06004FD4 RID: 20436 RVA: 0x001B7D54 File Offset: 0x001B5F54
			public override void Write(byte[] buf, int offset, int len)
			{
				while (len > 0)
				{
					int num = Math.Min(len, this._buf.Length - this._off);
					if (num == this._buf.Length)
					{
						DerOctetString.Encode(this._derOut, buf, offset, num);
					}
					else
					{
						Array.Copy(buf, offset, this._buf, this._off, num);
						this._off += num;
						if (this._off < this._buf.Length)
						{
							break;
						}
						DerOctetString.Encode(this._derOut, this._buf, 0, this._off);
						this._off = 0;
					}
					offset += num;
					len -= num;
				}
			}

			// Token: 0x06004FD5 RID: 20437 RVA: 0x001B7DF9 File Offset: 0x001B5FF9
			public override void Close()
			{
				if (this._off != 0)
				{
					DerOctetString.Encode(this._derOut, this._buf, 0, this._off);
				}
				this._gen.WriteBerEnd();
				base.Close();
			}

			// Token: 0x0400370D RID: 14093
			private byte[] _buf;

			// Token: 0x0400370E RID: 14094
			private int _off;

			// Token: 0x0400370F RID: 14095
			private readonly BerOctetStringGenerator _gen;

			// Token: 0x04003710 RID: 14096
			private readonly DerOutputStream _derOut;
		}
	}
}
