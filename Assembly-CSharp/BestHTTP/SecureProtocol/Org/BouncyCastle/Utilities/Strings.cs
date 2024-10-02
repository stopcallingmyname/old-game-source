using System;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000267 RID: 615
	public abstract class Strings
	{
		// Token: 0x06001683 RID: 5763 RVA: 0x000B0D18 File Offset: 0x000AEF18
		internal static bool IsOneOf(string s, params string[] candidates)
		{
			foreach (string b in candidates)
			{
				if (s == b)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x000B0D48 File Offset: 0x000AEF48
		public static string FromByteArray(byte[] bs)
		{
			char[] array = new char[bs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToChar(bs[i]);
			}
			return new string(array);
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x000B0D80 File Offset: 0x000AEF80
		public static byte[] ToByteArray(char[] cs)
		{
			byte[] array = new byte[cs.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(cs[i]);
			}
			return array;
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000B0DB0 File Offset: 0x000AEFB0
		public static byte[] ToByteArray(string s)
		{
			byte[] array = new byte[s.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(s[i]);
			}
			return array;
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000B0DE7 File Offset: 0x000AEFE7
		public static string FromAsciiByteArray(byte[] bytes)
		{
			return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x000B0DF8 File Offset: 0x000AEFF8
		public static byte[] ToAsciiByteArray(char[] cs)
		{
			return Encoding.ASCII.GetBytes(cs);
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000B0E05 File Offset: 0x000AF005
		public static byte[] ToAsciiByteArray(string s)
		{
			return Encoding.ASCII.GetBytes(s);
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x000B0E12 File Offset: 0x000AF012
		public static string FromUtf8ByteArray(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000B0E23 File Offset: 0x000AF023
		public static byte[] ToUtf8ByteArray(char[] cs)
		{
			return Encoding.UTF8.GetBytes(cs);
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000B0E30 File Offset: 0x000AF030
		public static byte[] ToUtf8ByteArray(string s)
		{
			return Encoding.UTF8.GetBytes(s);
		}
	}
}
