using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000420 RID: 1056
	internal class DtlsEpoch
	{
		// Token: 0x06002A3D RID: 10813 RVA: 0x00111160 File Offset: 0x0010F360
		internal DtlsEpoch(int epoch, TlsCipher cipher)
		{
			if (epoch < 0)
			{
				throw new ArgumentException("must be >= 0", "epoch");
			}
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			this.mEpoch = epoch;
			this.mCipher = cipher;
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x001111B0 File Offset: 0x0010F3B0
		internal long AllocateSequenceNumber()
		{
			long num = this.mSequenceNumber;
			this.mSequenceNumber = num + 1L;
			return num;
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06002A3F RID: 10815 RVA: 0x001111CF File Offset: 0x0010F3CF
		internal TlsCipher Cipher
		{
			get
			{
				return this.mCipher;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06002A40 RID: 10816 RVA: 0x001111D7 File Offset: 0x0010F3D7
		internal int Epoch
		{
			get
			{
				return this.mEpoch;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06002A41 RID: 10817 RVA: 0x001111DF File Offset: 0x0010F3DF
		internal DtlsReplayWindow ReplayWindow
		{
			get
			{
				return this.mReplayWindow;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x001111E7 File Offset: 0x0010F3E7
		internal long SequenceNumber
		{
			get
			{
				return this.mSequenceNumber;
			}
		}

		// Token: 0x04001CC1 RID: 7361
		private readonly DtlsReplayWindow mReplayWindow = new DtlsReplayWindow();

		// Token: 0x04001CC2 RID: 7362
		private readonly int mEpoch;

		// Token: 0x04001CC3 RID: 7363
		private readonly TlsCipher mCipher;

		// Token: 0x04001CC4 RID: 7364
		private long mSequenceNumber;
	}
}
