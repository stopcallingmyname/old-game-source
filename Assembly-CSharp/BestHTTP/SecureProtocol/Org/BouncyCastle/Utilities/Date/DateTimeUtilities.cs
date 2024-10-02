using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date
{
	// Token: 0x02000294 RID: 660
	public class DateTimeUtilities
	{
		// Token: 0x060017FC RID: 6140 RVA: 0x00022F1F File Offset: 0x0002111F
		private DateTimeUtilities()
		{
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x000B976C File Offset: 0x000B796C
		public static long DateTimeToUnixMs(DateTime dateTime)
		{
			if (dateTime.CompareTo(DateTimeUtilities.UnixEpoch) < 0)
			{
				throw new ArgumentException("DateTime value may not be before the epoch", "dateTime");
			}
			return (dateTime.Ticks - DateTimeUtilities.UnixEpoch.Ticks) / 10000L;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x000B97B4 File Offset: 0x000B79B4
		public static DateTime UnixMsToDateTime(long unixMs)
		{
			return new DateTime(unixMs * 10000L + DateTimeUtilities.UnixEpoch.Ticks);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x000B97DC File Offset: 0x000B79DC
		public static long CurrentUnixMs()
		{
			return DateTimeUtilities.DateTimeToUnixMs(DateTime.UtcNow);
		}

		// Token: 0x04001829 RID: 6185
		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
	}
}
