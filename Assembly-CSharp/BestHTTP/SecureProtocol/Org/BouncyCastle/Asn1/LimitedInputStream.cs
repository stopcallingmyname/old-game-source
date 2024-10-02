using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000672 RID: 1650
	internal abstract class LimitedInputStream : BaseInputStream
	{
		// Token: 0x06003D5C RID: 15708 RVA: 0x00173B10 File Offset: 0x00171D10
		internal LimitedInputStream(Stream inStream, int limit)
		{
			this._in = inStream;
			this._limit = limit;
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x00173B26 File Offset: 0x00171D26
		internal virtual int GetRemaining()
		{
			return this._limit;
		}

		// Token: 0x06003D5E RID: 15710 RVA: 0x00173B2E File Offset: 0x00171D2E
		protected virtual void SetParentEofDetect(bool on)
		{
			if (this._in is IndefiniteLengthInputStream)
			{
				((IndefiniteLengthInputStream)this._in).SetEofOn00(on);
			}
		}

		// Token: 0x0400270B RID: 9995
		protected readonly Stream _in;

		// Token: 0x0400270C RID: 9996
		private int _limit;
	}
}
