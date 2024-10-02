using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200045F RID: 1119
	public class TlsDeflateCompression : TlsCompression
	{
		// Token: 0x06002BA7 RID: 11175 RVA: 0x00116350 File Offset: 0x00114550
		public TlsDeflateCompression() : this(-1)
		{
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x00116359 File Offset: 0x00114559
		public TlsDeflateCompression(int level)
		{
			this.zIn = new ZStream();
			this.zIn.inflateInit();
			this.zOut = new ZStream();
			this.zOut.deflateInit(level);
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x00116390 File Offset: 0x00114590
		public virtual Stream Compress(Stream output)
		{
			return new TlsDeflateCompression.DeflateOutputStream(output, this.zOut, true);
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x0011639F File Offset: 0x0011459F
		public virtual Stream Decompress(Stream output)
		{
			return new TlsDeflateCompression.DeflateOutputStream(output, this.zIn, false);
		}

		// Token: 0x04001E20 RID: 7712
		public const int LEVEL_NONE = 0;

		// Token: 0x04001E21 RID: 7713
		public const int LEVEL_FASTEST = 1;

		// Token: 0x04001E22 RID: 7714
		public const int LEVEL_SMALLEST = 9;

		// Token: 0x04001E23 RID: 7715
		public const int LEVEL_DEFAULT = -1;

		// Token: 0x04001E24 RID: 7716
		protected readonly ZStream zIn;

		// Token: 0x04001E25 RID: 7717
		protected readonly ZStream zOut;

		// Token: 0x02000949 RID: 2377
		protected class DeflateOutputStream : ZOutputStream
		{
			// Token: 0x06004EED RID: 20205 RVA: 0x001B35D3 File Offset: 0x001B17D3
			public DeflateOutputStream(Stream output, ZStream z, bool compress) : base(output, z)
			{
				this.compress = compress;
				this.FlushMode = 2;
			}

			// Token: 0x06004EEE RID: 20206 RVA: 0x0000248C File Offset: 0x0000068C
			public override void Flush()
			{
			}
		}
	}
}
