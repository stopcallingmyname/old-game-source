using System;
using System.Globalization;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006B9 RID: 1721
	public class Time : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003F6F RID: 16239 RVA: 0x0017A48C File Offset: 0x0017868C
		public static Time GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Time.GetInstance(obj.GetObject());
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x0017A499 File Offset: 0x00178699
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

		// Token: 0x06003F71 RID: 16241 RVA: 0x0017A4D4 File Offset: 0x001786D4
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

		// Token: 0x06003F72 RID: 16242 RVA: 0x0017A540 File Offset: 0x00178740
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

		// Token: 0x06003F73 RID: 16243 RVA: 0x0017A5A1 File Offset: 0x001787A1
		public string GetTime()
		{
			if (this.time is DerUtcTime)
			{
				return ((DerUtcTime)this.time).AdjustedTimeString;
			}
			return ((DerGeneralizedTime)this.time).GetTime();
		}

		// Token: 0x06003F74 RID: 16244 RVA: 0x0017A5D4 File Offset: 0x001787D4
		public DateTime ToDateTime()
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

		// Token: 0x06003F75 RID: 16245 RVA: 0x0017A63C File Offset: 0x0017883C
		public override Asn1Object ToAsn1Object()
		{
			return this.time;
		}

		// Token: 0x06003F76 RID: 16246 RVA: 0x0017A644 File Offset: 0x00178844
		public override string ToString()
		{
			return this.GetTime();
		}

		// Token: 0x04002833 RID: 10291
		private readonly Asn1Object time;
	}
}
