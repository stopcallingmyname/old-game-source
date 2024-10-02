using System;
using System.Globalization;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000666 RID: 1638
	public class DerUtcTime : Asn1Object
	{
		// Token: 0x06003D1B RID: 15643 RVA: 0x001731C7 File Offset: 0x001713C7
		public static DerUtcTime GetInstance(object obj)
		{
			if (obj == null || obj is DerUtcTime)
			{
				return (DerUtcTime)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003D1C RID: 15644 RVA: 0x001731F0 File Offset: 0x001713F0
		public static DerUtcTime GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerUtcTime)
			{
				return DerUtcTime.GetInstance(@object);
			}
			return new DerUtcTime(((Asn1OctetString)@object).GetOctets());
		}

		// Token: 0x06003D1D RID: 15645 RVA: 0x00173228 File Offset: 0x00171428
		public DerUtcTime(string time)
		{
			if (time == null)
			{
				throw new ArgumentNullException("time");
			}
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

		// Token: 0x06003D1E RID: 15646 RVA: 0x00173280 File Offset: 0x00171480
		public DerUtcTime(DateTime time)
		{
			this.time = time.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture) + "Z";
		}

		// Token: 0x06003D1F RID: 15647 RVA: 0x001732A9 File Offset: 0x001714A9
		internal DerUtcTime(byte[] bytes)
		{
			this.time = Strings.FromAsciiByteArray(bytes);
		}

		// Token: 0x06003D20 RID: 15648 RVA: 0x001732BD File Offset: 0x001714BD
		public DateTime ToDateTime()
		{
			return this.ParseDateString(this.TimeString, "yyMMddHHmmss'GMT'zzz");
		}

		// Token: 0x06003D21 RID: 15649 RVA: 0x001732D0 File Offset: 0x001714D0
		public DateTime ToAdjustedDateTime()
		{
			return this.ParseDateString(this.AdjustedTimeString, "yyyyMMddHHmmss'GMT'zzz");
		}

		// Token: 0x06003D22 RID: 15650 RVA: 0x001732E4 File Offset: 0x001714E4
		private DateTime ParseDateString(string dateStr, string formatStr)
		{
			return DateTime.ParseExact(dateStr, formatStr, DateTimeFormatInfo.InvariantInfo).ToUniversalTime();
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06003D23 RID: 15651 RVA: 0x00173308 File Offset: 0x00171508
		public string TimeString
		{
			get
			{
				if (this.time.IndexOf('-') < 0 && this.time.IndexOf('+') < 0)
				{
					if (this.time.Length == 11)
					{
						return this.time.Substring(0, 10) + "00GMT+00:00";
					}
					return this.time.Substring(0, 12) + "GMT+00:00";
				}
				else
				{
					int num = this.time.IndexOf('-');
					if (num < 0)
					{
						num = this.time.IndexOf('+');
					}
					string text = this.time;
					if (num == this.time.Length - 3)
					{
						text += "00";
					}
					if (num == 10)
					{
						return string.Concat(new string[]
						{
							text.Substring(0, 10),
							"00GMT",
							text.Substring(10, 3),
							":",
							text.Substring(13, 2)
						});
					}
					return string.Concat(new string[]
					{
						text.Substring(0, 12),
						"GMT",
						text.Substring(12, 3),
						":",
						text.Substring(15, 2)
					});
				}
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06003D24 RID: 15652 RVA: 0x0017343D File Offset: 0x0017163D
		[Obsolete("Use 'AdjustedTimeString' property instead")]
		public string AdjustedTime
		{
			get
			{
				return this.AdjustedTimeString;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06003D25 RID: 15653 RVA: 0x00173448 File Offset: 0x00171648
		public string AdjustedTimeString
		{
			get
			{
				string timeString = this.TimeString;
				return ((timeString[0] < '5') ? "20" : "19") + timeString;
			}
		}

		// Token: 0x06003D26 RID: 15654 RVA: 0x00173479 File Offset: 0x00171679
		private byte[] GetOctets()
		{
			return Strings.ToAsciiByteArray(this.time);
		}

		// Token: 0x06003D27 RID: 15655 RVA: 0x00173486 File Offset: 0x00171686
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(23, this.GetOctets());
		}

		// Token: 0x06003D28 RID: 15656 RVA: 0x00173498 File Offset: 0x00171698
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerUtcTime derUtcTime = asn1Object as DerUtcTime;
			return derUtcTime != null && this.time.Equals(derUtcTime.time);
		}

		// Token: 0x06003D29 RID: 15657 RVA: 0x001734C2 File Offset: 0x001716C2
		protected override int Asn1GetHashCode()
		{
			return this.time.GetHashCode();
		}

		// Token: 0x06003D2A RID: 15658 RVA: 0x001734CF File Offset: 0x001716CF
		public override string ToString()
		{
			return this.time;
		}

		// Token: 0x04002703 RID: 9987
		private readonly string time;
	}
}
