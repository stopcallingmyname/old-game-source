using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x02000604 RID: 1540
	public class CmsTypedStream
	{
		// Token: 0x06003A91 RID: 14993 RVA: 0x0016B5FF File Offset: 0x001697FF
		public CmsTypedStream(Stream inStream) : this(PkcsObjectIdentifiers.Data.Id, inStream, 32768)
		{
		}

		// Token: 0x06003A92 RID: 14994 RVA: 0x0016B617 File Offset: 0x00169817
		public CmsTypedStream(string oid, Stream inStream) : this(oid, inStream, 32768)
		{
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x0016B626 File Offset: 0x00169826
		public CmsTypedStream(string oid, Stream inStream, int bufSize)
		{
			this._oid = oid;
			this._in = new CmsTypedStream.FullReaderStream(new BufferedStream(inStream, bufSize));
		}

		// Token: 0x1700078B RID: 1931
		// (get) Token: 0x06003A94 RID: 14996 RVA: 0x0016B647 File Offset: 0x00169847
		public string ContentType
		{
			get
			{
				return this._oid;
			}
		}

		// Token: 0x1700078C RID: 1932
		// (get) Token: 0x06003A95 RID: 14997 RVA: 0x0016B64F File Offset: 0x0016984F
		public Stream ContentStream
		{
			get
			{
				return this._in;
			}
		}

		// Token: 0x06003A96 RID: 14998 RVA: 0x0016B657 File Offset: 0x00169857
		public void Drain()
		{
			Streams.Drain(this._in);
			Platform.Dispose(this._in);
		}

		// Token: 0x04002651 RID: 9809
		private const int BufferSize = 32768;

		// Token: 0x04002652 RID: 9810
		private readonly string _oid;

		// Token: 0x04002653 RID: 9811
		private readonly Stream _in;

		// Token: 0x02000988 RID: 2440
		private class FullReaderStream : FilterStream
		{
			// Token: 0x06004FC7 RID: 20423 RVA: 0x00172794 File Offset: 0x00170994
			internal FullReaderStream(Stream input) : base(input)
			{
			}

			// Token: 0x06004FC8 RID: 20424 RVA: 0x001B7B18 File Offset: 0x001B5D18
			public override int Read(byte[] buf, int off, int len)
			{
				return Streams.ReadFully(this.s, buf, off, len);
			}
		}
	}
}
