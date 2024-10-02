using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000426 RID: 1062
	internal class DtlsReplayWindow
	{
		// Token: 0x06002A7A RID: 10874 RVA: 0x00112348 File Offset: 0x00110548
		internal bool ShouldDiscard(long seq)
		{
			if ((seq & 281474976710655L) != seq)
			{
				return true;
			}
			if (seq <= this.mLatestConfirmedSeq)
			{
				long num = this.mLatestConfirmedSeq - seq;
				if (num >= 64L)
				{
					return true;
				}
				if ((this.mBitmap & 1L << (int)num) != 0L)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x00112394 File Offset: 0x00110594
		internal void ReportAuthenticated(long seq)
		{
			if ((seq & 281474976710655L) != seq)
			{
				throw new ArgumentException("out of range", "seq");
			}
			if (seq <= this.mLatestConfirmedSeq)
			{
				long num = this.mLatestConfirmedSeq - seq;
				if (num < 64L)
				{
					this.mBitmap |= 1L << (int)num;
					return;
				}
			}
			else
			{
				long num2 = seq - this.mLatestConfirmedSeq;
				if (num2 >= 64L)
				{
					this.mBitmap = 1L;
				}
				else
				{
					this.mBitmap <<= (int)num2;
					this.mBitmap |= 1L;
				}
				this.mLatestConfirmedSeq = seq;
			}
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x0011242E File Offset: 0x0011062E
		internal void Reset()
		{
			this.mLatestConfirmedSeq = -1L;
			this.mBitmap = 0L;
		}

		// Token: 0x04001CE8 RID: 7400
		private const long VALID_SEQ_MASK = 281474976710655L;

		// Token: 0x04001CE9 RID: 7401
		private const long WINDOW_SIZE = 64L;

		// Token: 0x04001CEA RID: 7402
		private long mLatestConfirmedSeq = -1L;

		// Token: 0x04001CEB RID: 7403
		private long mBitmap;
	}
}
