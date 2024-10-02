using System;
using System.IO;
using System.Text;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200080A RID: 2058
	internal class SharedUtils
	{
		// Token: 0x0600491D RID: 18717 RVA: 0x0019FD62 File Offset: 0x0019DF62
		public static int URShift(int number, int bits)
		{
			return (int)((uint)number >> bits);
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x0019FD6C File Offset: 0x0019DF6C
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			if (target.Length == 0)
			{
				return 0;
			}
			char[] array = new char[target.Length];
			int num = sourceTextReader.Read(array, start, count);
			if (num == 0)
			{
				return -1;
			}
			for (int i = start; i < start + num; i++)
			{
				target[i] = (byte)array[i];
			}
			return num;
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x000B0E30 File Offset: 0x000AF030
		internal static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x0019FDAD File Offset: 0x0019DFAD
		internal static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
