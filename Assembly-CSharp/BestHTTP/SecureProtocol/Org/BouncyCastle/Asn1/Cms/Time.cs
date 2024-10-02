using System;
using System.Globalization;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A4 RID: 1956
	public class Time : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060045F2 RID: 17906 RVA: 0x00191EFC File Offset: 0x001900FC
		public static Time GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Time.GetInstance(obj.GetObject());
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00191F09 File Offset: 0x00190109
		public Time(Asn1Object time)
		{
			if (time == null)
			{
				throw new ArgumentNullException("time");
			}
			if (!(time is DerUtcTime) && !(time is DerGeneralizedTime))
			{
				throw new ArgumentException("unknown object passed to Time");
			}
			this.time = time;
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x00191F44 File Offset: 0x00190144
		public Time(DateTime date)
		{
			string text = date.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
			int num = int.Parse(text.Substring(0, 4));
			if (num < 1950 || num > 2049)
			{
				this.time = new DerGeneralizedTime(text);
				return;
			}
			this.time = new DerUtcTime(text.Substring(2));
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00191FB0 File Offset: 0x001901B0
		public static Time GetInstance(object obj)
		{
			if (obj == null || obj is Time)
			{
				return (Time)obj;
			}
			if (obj is DerUtcTime)
			{
				return new Time((DerUtcTime)obj);
			}
			if (obj is DerGeneralizedTime)
			{
				return new Time((DerGeneralizedTime)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x060045F6 RID: 17910 RVA: 0x00192011 File Offset: 0x00190211
		public string TimeString
		{
			get
			{
				if (this.time is DerUtcTime)
				{
					return ((DerUtcTime)this.time).AdjustedTimeString;
				}
				return ((DerGeneralizedTime)this.time).GetTime();
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x00192044 File Offset: 0x00190244
		public DateTime Date
		{
			get
			{
				DateTime result;
				try
				{
					if (this.time is DerUtcTime)
					{
						result = ((DerUtcTime)this.time).ToAdjustedDateTime();
					}
					else
					{
						result = ((DerGeneralizedTime)this.time).ToDateTime();
					}
				}
				catch (FormatException ex)
				{
					throw new InvalidOperationException("invalid date string: " + ex.Message);
				}
				return result;
			}
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x001920AC File Offset: 0x001902AC
		public override Asn1Object ToAsn1Object()
		{
			return this.time;
		}

		// Token: 0x04002D8F RID: 11663
		private readonly Asn1Object time;
	}
}
