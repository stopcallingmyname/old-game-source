using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A3 RID: 675
	public class GenTimeAccuracy
	{
		// Token: 0x06001896 RID: 6294 RVA: 0x000B9FD4 File Offset: 0x000B81D4
		public GenTimeAccuracy(Accuracy accuracy)
		{
			this.accuracy = accuracy;
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001897 RID: 6295 RVA: 0x000B9FE3 File Offset: 0x000B81E3
		public int Seconds
		{
			get
			{
				return this.GetTimeComponent(this.accuracy.Seconds);
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001898 RID: 6296 RVA: 0x000B9FF6 File Offset: 0x000B81F6
		public int Millis
		{
			get
			{
				return this.GetTimeComponent(this.accuracy.Millis);
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001899 RID: 6297 RVA: 0x000BA009 File Offset: 0x000B8209
		public int Micros
		{
			get
			{
				return this.GetTimeComponent(this.accuracy.Micros);
			}
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000BA01C File Offset: 0x000B821C
		private int GetTimeComponent(DerInteger time)
		{
			if (time != null)
			{
				return time.Value.IntValue;
			}
			return 0;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000BA030 File Offset: 0x000B8230
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				this.Seconds,
				".",
				this.Millis.ToString("000"),
				this.Micros.ToString("000")
			});
		}

		// Token: 0x04001835 RID: 6197
		private Accuracy accuracy;
	}
}
