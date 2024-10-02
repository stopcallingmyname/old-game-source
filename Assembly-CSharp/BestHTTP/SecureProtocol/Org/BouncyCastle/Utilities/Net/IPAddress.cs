using System;
using System.Globalization;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Net
{
	// Token: 0x02000277 RID: 631
	public class IPAddress
	{
		// Token: 0x0600174B RID: 5963 RVA: 0x000B7ADF File Offset: 0x000B5CDF
		public static bool IsValid(string address)
		{
			return IPAddress.IsValidIPv4(address) || IPAddress.IsValidIPv6(address);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000B7AF1 File Offset: 0x000B5CF1
		public static bool IsValidWithNetMask(string address)
		{
			return IPAddress.IsValidIPv4WithNetmask(address) || IPAddress.IsValidIPv6WithNetmask(address);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x000B7B04 File Offset: 0x000B5D04
		public static bool IsValidIPv4(string address)
		{
			try
			{
				return IPAddress.unsafeIsValidIPv4(address);
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x000B7B40 File Offset: 0x000B5D40
		private static bool unsafeIsValidIPv4(string address)
		{
			if (address.Length == 0)
			{
				return false;
			}
			int num = 0;
			string text = address + ".";
			int num2 = 0;
			int num3;
			while (num2 < text.Length && (num3 = text.IndexOf('.', num2)) > num2)
			{
				if (num == 4)
				{
					return false;
				}
				int num4 = int.Parse(text.Substring(num2, num3 - num2));
				if (num4 < 0 || num4 > 255)
				{
					return false;
				}
				num2 = num3 + 1;
				num++;
			}
			return num == 4;
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x000B7BB4 File Offset: 0x000B5DB4
		public static bool IsValidIPv4WithNetmask(string address)
		{
			int num = address.IndexOf('/');
			string text = address.Substring(num + 1);
			return num > 0 && IPAddress.IsValidIPv4(address.Substring(0, num)) && (IPAddress.IsValidIPv4(text) || IPAddress.IsMaskValue(text, 32));
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000B7BFC File Offset: 0x000B5DFC
		public static bool IsValidIPv6WithNetmask(string address)
		{
			int num = address.IndexOf('/');
			string text = address.Substring(num + 1);
			return num > 0 && IPAddress.IsValidIPv6(address.Substring(0, num)) && (IPAddress.IsValidIPv6(text) || IPAddress.IsMaskValue(text, 128));
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x000B7C48 File Offset: 0x000B5E48
		private static bool IsMaskValue(string component, int size)
		{
			int num = int.Parse(component);
			try
			{
				return num >= 0 && num <= size;
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000B7C94 File Offset: 0x000B5E94
		public static bool IsValidIPv6(string address)
		{
			try
			{
				return IPAddress.unsafeIsValidIPv6(address);
			}
			catch (FormatException)
			{
			}
			catch (OverflowException)
			{
			}
			return false;
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000B7CD0 File Offset: 0x000B5ED0
		private static bool unsafeIsValidIPv6(string address)
		{
			if (address.Length == 0)
			{
				return false;
			}
			int num = 0;
			string text = address + ":";
			bool flag = false;
			int num2 = 0;
			int num3;
			while (num2 < text.Length && (num3 = text.IndexOf(':', num2)) >= num2)
			{
				if (num == 8)
				{
					return false;
				}
				if (num2 != num3)
				{
					string text2 = text.Substring(num2, num3 - num2);
					if (num3 == text.Length - 1 && text2.IndexOf('.') > 0)
					{
						if (!IPAddress.IsValidIPv4(text2))
						{
							return false;
						}
						num++;
					}
					else
					{
						int num4 = int.Parse(text.Substring(num2, num3 - num2), NumberStyles.AllowHexSpecifier);
						if (num4 < 0 || num4 > 65535)
						{
							return false;
						}
					}
				}
				else
				{
					if (num3 != 1 && num3 != text.Length - 1 && flag)
					{
						return false;
					}
					flag = true;
				}
				num2 = num3 + 1;
				num++;
			}
			return num == 8 || flag;
		}
	}
}
