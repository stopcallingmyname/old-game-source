using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BestHTTP.Extensions
{
	// Token: 0x020007EC RID: 2028
	public static class Extensions
	{
		// Token: 0x06004823 RID: 18467 RVA: 0x001981DC File Offset: 0x001963DC
		public static string AsciiToString(this byte[] bytes)
		{
			StringBuilder stringBuilder = new StringBuilder(bytes.Length);
			foreach (byte b in bytes)
			{
				stringBuilder.Append((char)((b <= 127) ? b : 63));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x00198220 File Offset: 0x00196420
		public static byte[] GetASCIIBytes(this string str)
		{
			byte[] array = VariableSizedBufferPool.Get((long)str.Length, false);
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				array[i] = (byte)((c < '\u0080') ? c : '?');
			}
			return array;
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x00198268 File Offset: 0x00196468
		public static void SendAsASCII(this BinaryWriter stream, string str)
		{
			foreach (char c in str)
			{
				stream.Write((byte)((c < '\u0080') ? c : '?'));
			}
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x001982A2 File Offset: 0x001964A2
		public static void WriteLine(this Stream fs)
		{
			fs.Write(HTTPRequest.EOL, 0, 2);
		}

		// Token: 0x06004827 RID: 18471 RVA: 0x001982B4 File Offset: 0x001964B4
		public static void WriteLine(this Stream fs, string line)
		{
			byte[] asciibytes = line.GetASCIIBytes();
			fs.Write(asciibytes, 0, asciibytes.Length);
			fs.WriteLine();
			VariableSizedBufferPool.Release(asciibytes);
		}

		// Token: 0x06004828 RID: 18472 RVA: 0x001982E0 File Offset: 0x001964E0
		public static void WriteLine(this Stream fs, string format, params object[] values)
		{
			byte[] asciibytes = string.Format(format, values).GetASCIIBytes();
			fs.Write(asciibytes, 0, asciibytes.Length);
			fs.WriteLine();
			VariableSizedBufferPool.Release(asciibytes);
		}

		// Token: 0x06004829 RID: 18473 RVA: 0x00198314 File Offset: 0x00196514
		public static string GetRequestPathAndQueryURL(this Uri uri)
		{
			string text = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);
			if (string.IsNullOrEmpty(text))
			{
				text = "/";
			}
			return text;
		}

		// Token: 0x0600482A RID: 18474 RVA: 0x0019833C File Offset: 0x0019653C
		public static string[] FindOption(this string str, string option)
		{
			string[] array = str.ToLower().Split(new char[]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries);
			option = option.ToLower();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Contains(option))
				{
					return array[i].Split(new char[]
					{
						'='
					}, StringSplitOptions.RemoveEmptyEntries);
				}
			}
			return null;
		}

		// Token: 0x0600482B RID: 18475 RVA: 0x000B7E62 File Offset: 0x000B6062
		public static void WriteArray(this Stream stream, byte[] array)
		{
			stream.Write(array, 0, array.Length);
		}

		// Token: 0x0600482C RID: 18476 RVA: 0x00198398 File Offset: 0x00196598
		public static bool IsHostIsAnIPAddress(this Uri uri)
		{
			return !(uri == null) && (Extensions.IsIpV4AddressValid(uri.Host) || Extensions.IsIpV6AddressValid(uri.Host));
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x001983BF File Offset: 0x001965BF
		public static bool IsIpV4AddressValid(string address)
		{
			return !string.IsNullOrEmpty(address) && Extensions.validIpV4AddressRegex.IsMatch(address.Trim());
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x001983DC File Offset: 0x001965DC
		public static bool IsIpV6AddressValid(string address)
		{
			IPAddress ipaddress;
			return !string.IsNullOrEmpty(address) && IPAddress.TryParse(address, out ipaddress) && ipaddress.AddressFamily == AddressFamily.InterNetworkV6;
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x00198408 File Offset: 0x00196608
		public static int ToInt32(this string str, int defaultValue = 0)
		{
			if (str == null)
			{
				return defaultValue;
			}
			int result;
			try
			{
				result = int.Parse(str);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x0019843C File Offset: 0x0019663C
		public static long ToInt64(this string str, long defaultValue = 0L)
		{
			if (str == null)
			{
				return defaultValue;
			}
			long result;
			try
			{
				result = long.Parse(str);
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x00198470 File Offset: 0x00196670
		public static DateTime ToDateTime(this string str, DateTime defaultValue = default(DateTime))
		{
			if (str == null)
			{
				return defaultValue;
			}
			DateTime result;
			try
			{
				DateTime.TryParse(str, out defaultValue);
				result = defaultValue.ToUniversalTime();
			}
			catch
			{
				result = defaultValue;
			}
			return result;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x001984AC File Offset: 0x001966AC
		public static string ToStrOrEmpty(this string str)
		{
			if (str == null)
			{
				return string.Empty;
			}
			return str;
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x001984B8 File Offset: 0x001966B8
		public static string ToBinaryStr(this byte value)
		{
			return Convert.ToString(value, 2).PadLeft(8, '0');
		}

		// Token: 0x06004834 RID: 18484 RVA: 0x001984CC File Offset: 0x001966CC
		public static string CalculateMD5Hash(this string input)
		{
			byte[] asciibytes = input.GetASCIIBytes();
			string result = asciibytes.CalculateMD5Hash();
			VariableSizedBufferPool.Release(asciibytes);
			return result;
		}

		// Token: 0x06004835 RID: 18485 RVA: 0x001984EC File Offset: 0x001966EC
		public static string CalculateMD5Hash(this byte[] input)
		{
			string result;
			using (MD5 md = MD5.Create())
			{
				byte[] array = md.ComputeHash(input);
				StringBuilder stringBuilder = new StringBuilder(array.Length);
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				VariableSizedBufferPool.Release(array);
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x06004836 RID: 18486 RVA: 0x00198564 File Offset: 0x00196764
		internal static string Read(this string str, ref int pos, char block, bool needResult = true)
		{
			return str.Read(ref pos, (char ch) => ch != block, needResult);
		}

		// Token: 0x06004837 RID: 18487 RVA: 0x00198594 File Offset: 0x00196794
		internal static string Read(this string str, ref int pos, Func<char, bool> block, bool needResult = true)
		{
			if (pos >= str.Length)
			{
				return string.Empty;
			}
			str.SkipWhiteSpace(ref pos);
			int num = pos;
			while (pos < str.Length && block(str[pos]))
			{
				pos++;
			}
			string result = needResult ? str.Substring(num, pos - num) : null;
			pos++;
			return result;
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x001985F4 File Offset: 0x001967F4
		internal static string ReadPossibleQuotedText(this string str, ref int pos)
		{
			string result = string.Empty;
			if (str == null)
			{
				return result;
			}
			if (str[pos] == '"')
			{
				str.Read(ref pos, '"', false);
				result = str.Read(ref pos, '"', true);
				str.Read(ref pos, ',', false);
			}
			else
			{
				result = str.Read(ref pos, (char ch) => ch != ',' && ch != ';', true);
			}
			return result;
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00198664 File Offset: 0x00196864
		internal static void SkipWhiteSpace(this string str, ref int pos)
		{
			if (pos >= str.Length)
			{
				return;
			}
			while (pos < str.Length && char.IsWhiteSpace(str[pos]))
			{
				pos++;
			}
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00198690 File Offset: 0x00196890
		internal static string TrimAndLower(this string str)
		{
			if (str == null)
			{
				return null;
			}
			char[] array = new char[str.Length];
			int length = 0;
			foreach (char c in str)
			{
				if (!char.IsWhiteSpace(c) && !char.IsControl(c))
				{
					array[length++] = char.ToLowerInvariant(c);
				}
			}
			return new string(array, 0, length);
		}

		// Token: 0x0600483B RID: 18491 RVA: 0x001986F0 File Offset: 0x001968F0
		internal static char? Peek(this string str, int pos)
		{
			if (pos < 0 || pos >= str.Length)
			{
				return null;
			}
			return new char?(str[pos]);
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x00198720 File Offset: 0x00196920
		internal static List<HeaderValue> ParseOptionalHeader(this string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != '=' && ch != ',', true).TrimAndLower());
				if (str[i - 1] == '=')
				{
					headerValue.Value = str.ReadPossibleQuotedText(ref i);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x0019879C File Offset: 0x0019699C
		internal static List<HeaderValue> ParseQualityParams(this string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str == null)
			{
				return list;
			}
			int i = 0;
			while (i < str.Length)
			{
				HeaderValue headerValue = new HeaderValue(str.Read(ref i, (char ch) => ch != ',' && ch != ';', true).TrimAndLower());
				if (str[i - 1] == ';')
				{
					str.Read(ref i, '=', false);
					headerValue.Value = str.Read(ref i, ',', true);
				}
				list.Add(headerValue);
			}
			return list;
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x00198828 File Offset: 0x00196A28
		public static void ReadBuffer(this Stream stream, byte[] buffer)
		{
			int num = 0;
			for (;;)
			{
				int num2 = stream.Read(buffer, num, buffer.Length - num);
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				if (num >= buffer.Length)
				{
					return;
				}
			}
			throw ExceptionHelper.ServerClosedTCPStream();
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0019885C File Offset: 0x00196A5C
		public static void ReadBuffer(this Stream stream, byte[] buffer, int length)
		{
			int num = 0;
			for (;;)
			{
				int num2 = stream.Read(buffer, num, length - num);
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
				if (num >= length)
				{
					return;
				}
			}
			throw ExceptionHelper.ServerClosedTCPStream();
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0019888C File Offset: 0x00196A8C
		public static void WriteString(this BufferPoolMemoryStream ms, string str)
		{
			int byteCount = Encoding.UTF8.GetByteCount(str);
			byte[] array = VariableSizedBufferPool.Get((long)byteCount, true);
			Encoding.UTF8.GetBytes(str, 0, str.Length, array, 0);
			ms.Write(array, 0, byteCount);
			VariableSizedBufferPool.Release(array);
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x001988D2 File Offset: 0x00196AD2
		public static void WriteLine(this BufferPoolMemoryStream ms)
		{
			ms.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x001988E7 File Offset: 0x00196AE7
		public static void WriteLine(this BufferPoolMemoryStream ms, string str)
		{
			ms.WriteString(str);
			ms.Write(HTTPRequest.EOL, 0, HTTPRequest.EOL.Length);
		}

		// Token: 0x04002EFE RID: 12030
		private static readonly Regex validIpV4AddressRegex = new Regex("\\b(?:\\d{1,3}\\.){3}\\d{1,3}\\b", RegexOptions.IgnoreCase);
	}
}
