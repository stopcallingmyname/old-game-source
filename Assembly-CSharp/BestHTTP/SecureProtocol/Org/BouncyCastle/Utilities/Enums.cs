using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000262 RID: 610
	internal abstract class Enums
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x000B0AA8 File Offset: 0x000AECA8
		internal static Enum GetEnumValue(Type enumType, string s)
		{
			if (!Enums.IsEnumType(enumType))
			{
				throw new ArgumentException("Not an enumeration type", "enumType");
			}
			if (s.Length > 0 && char.IsLetter(s[0]) && s.IndexOf(',') < 0)
			{
				s = s.Replace('-', '_');
				s = s.Replace('/', '_');
				return (Enum)Enum.Parse(enumType, s, false);
			}
			throw new ArgumentException();
		}

		// Token: 0x06001661 RID: 5729 RVA: 0x000B0B1A File Offset: 0x000AED1A
		internal static Array GetEnumValues(Type enumType)
		{
			if (!Enums.IsEnumType(enumType))
			{
				throw new ArgumentException("Not an enumeration type", "enumType");
			}
			return Enum.GetValues(enumType);
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x000B0B3C File Offset: 0x000AED3C
		internal static Enum GetArbitraryValue(Type enumType)
		{
			Array enumValues = Enums.GetEnumValues(enumType);
			int index = (int)(DateTimeUtilities.CurrentUnixMs() & 2147483647L) % enumValues.Length;
			return (Enum)enumValues.GetValue(index);
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x000B0B71 File Offset: 0x000AED71
		internal static bool IsEnumType(Type t)
		{
			return t.IsEnum;
		}
	}
}
