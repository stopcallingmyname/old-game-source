using System;
using System.Globalization;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200064F RID: 1615
	public class DerGeneralizedTime : Asn1Object
	{
		// Token: 0x06003C64 RID: 15460 RVA: 0x001714F0 File Offset: 0x0016F6F0
		public static DerGeneralizedTime GetInstance(object obj)
		{
			if (obj == null || obj is DerGeneralizedTime)
			{
				return (DerGeneralizedTime)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x00171520 File Offset: 0x0016F720
		public static DerGeneralizedTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerGeneralizedTime)
			{
				return DerGeneralizedTime.GetInstance(@object);
			}
			return new DerGeneralizedTime(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x00171558 File Offset: 0x0016F758
		public DerGeneralizedTime(string time)
		{
			this.time = time;
			try
			{
				this.ToDateTime();
			}
			catch (FormatException ex)
			{
				throw new ArgumentException("invalid date string: " + ex.Message);
			}
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x001715A4 File Offset: 0x0016F7A4
		public DerGeneralizedTime(DateTime time)
		{
			this.time = time.ToString("yyyyMMddHHmmss\\Z");
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x001715BE File Offset: 0x0016F7BE
		internal DerGeneralizedTime(byte[] bytes)
		{
			this.time = Strings.FromAsciiByteArray(bytes);
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06003C69 RID: 15465 RVA: 0x001715D2 File Offset: 0x0016F7D2
		public string TimeString
		{
			get
			{
				return this.time;
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x001715DC File Offset: 0x0016F7DC
		public string GetTime()
		{
			if (this.time[this.time.Length - 1] == 'Z')
			{
				return this.time.Substring(0, this.time.Length - 1) + "GMT+00:00";
			}
			int num = this.time.Length - 5;
			char c = this.time[num];
			if (c == '-' || c == '+')
			{
				return string.Concat(new string[]
				{
					this.time.Substring(0, num),
					"GMT",
					this.time.Substring(num, 3),
					":",
					this.time.Substring(num + 3)
				});
			}
			num = this.time.Length - 3;
			c = this.time[num];
			if (c == '-' || c == '+')
			{
				return this.time.Substring(0, num) + "GMT" + this.time.Substring(num) + ":00";
			}
			return this.time + this.CalculateGmtOffset();
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x001716FC File Offset: 0x0016F8FC
		private string CalculateGmtOffset()
		{
			char c = '+';
			DateTime dateTime = this.ToDateTime();
			TimeSpan timeSpan = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
			if (timeSpan.CompareTo(TimeSpan.Zero) < 0)
			{
				c = '-';
				timeSpan = timeSpan.Duration();
			}
			int hours = timeSpan.Hours;
			int minutes = timeSpan.Minutes;
			return string.Concat(new string[]
			{
				"GMT",
				c.ToString(),
				DerGeneralizedTime.Convert(hours),
				":",
				DerGeneralizedTime.Convert(minutes)
			});
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x00171782 File Offset: 0x0016F982
		private static string Convert(int time)
		{
			if (time < 10)
			{
				return "0" + time;
			}
			return time.ToString();
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x001717A4 File Offset: 0x0016F9A4
		public DateTime ToDateTime()
		{
			string text = this.time;
			bool makeUniversal = false;
			string format;
			if (Platform.EndsWith(text, "Z"))
			{
				if (this.HasFractionalSeconds)
				{
					int count = text.Length - text.IndexOf('.') - 2;
					format = "yyyyMMddHHmmss." + this.FString(count) + "\\Z";
				}
				else
				{
					format = "yyyyMMddHHmmss\\Z";
				}
			}
			else if (this.time.IndexOf('-') > 0 || this.time.IndexOf('+') > 0)
			{
				text = this.GetTime();
				makeUniversal = true;
				if (this.HasFractionalSeconds)
				{
					int count2 = Platform.IndexOf(text, "GMT") - 1 - text.IndexOf('.');
					format = "yyyyMMddHHmmss." + this.FString(count2) + "'GMT'zzz";
				}
				else
				{
					format = "yyyyMMddHHmmss'GMT'zzz";
				}
			}
			else if (this.HasFractionalSeconds)
			{
				int count3 = text.Length - 1 - text.IndexOf('.');
				format = "yyyyMMddHHmmss." + this.FString(count3);
			}
			else
			{
				format = "yyyyMMddHHmmss";
			}
			return this.ParseDateString(text, format, makeUniversal);
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x001718B4 File Offset: 0x0016FAB4
		private string FString(int count)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				stringBuilder.Append('f');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x001718E4 File Offset: 0x0016FAE4
		private DateTime ParseDateString(string s, string format, bool makeUniversal)
		{
			DateTimeStyles dateTimeStyles = DateTimeStyles.None;
			if (Platform.EndsWith(format, "Z"))
			{
				try
				{
					dateTimeStyles = (DateTimeStyles)Enums.GetEnumValue(typeof(DateTimeStyles), "AssumeUniversal");
				}
				catch (Exception)
				{
				}
				dateTimeStyles |= DateTimeStyles.AdjustToUniversal;
			}
			DateTime result = DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, dateTimeStyles);
			if (!makeUniversal)
			{
				return result;
			}
			return result.ToUniversalTime();
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06003C70 RID: 15472 RVA: 0x00171950 File Offset: 0x0016FB50
		private bool HasFractionalSeconds
		{
			get
			{
				return this.time.IndexOf('.') == 14;
			}
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x00171963 File Offset: 0x0016FB63
		private byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.time);
		}

		// Token: 0x06003C72 RID: 15474 RVA: 0x00171970 File Offset: 0x0016FB70
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(24, this.GetOctets());
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x00171980 File Offset: 0x0016FB80
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerGeneralizedTime derGeneralizedTime = asn1Object as DerGeneralizedTime;
			return derGeneralizedTime != null && this.time.Equals(derGeneralizedTime.time);
		}

		// Token: 0x06003C74 RID: 15476 RVA: 0x001719AA File Offset: 0x0016FBAA
		protected override int Asn1GetHashCode()
		{
			return this.time.GetHashCode();
		}

		// Token: 0x040026E8 RID: 9960
		private readonly string time;
	}
}
