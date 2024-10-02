using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities
{
	// Token: 0x02000266 RID: 614
	internal abstract class Platform
	{
		// Token: 0x0600166D RID: 5741 RVA: 0x000B0BA2 File Offset: 0x000AEDA2
		private static string GetNewLine()
		{
			return Environment.NewLine;
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000B0BA9 File Offset: 0x000AEDA9
		internal static bool EqualsIgnoreCase(string a, string b)
		{
			return Platform.ToUpperInvariant(a) == Platform.ToUpperInvariant(b);
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x000B0BBC File Offset: 0x000AEDBC
		internal static string GetEnvironmentVariable(string variable)
		{
			string result;
			try
			{
				result = Environment.GetEnvironmentVariable(variable);
			}
			catch (SecurityException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x000B0BE8 File Offset: 0x000AEDE8
		internal static Exception CreateNotImplementedException(string message)
		{
			return new NotImplementedException(message);
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x000B0BF0 File Offset: 0x000AEDF0
		internal static IList CreateArrayList()
		{
			return new ArrayList();
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000B0BF7 File Offset: 0x000AEDF7
		internal static IList CreateArrayList(int capacity)
		{
			return new ArrayList(capacity);
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x000B0BFF File Offset: 0x000AEDFF
		internal static IList CreateArrayList(ICollection collection)
		{
			return new ArrayList(collection);
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x000B0C08 File Offset: 0x000AEE08
		internal static IList CreateArrayList(IEnumerable collection)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object value in collection)
			{
				arrayList.Add(value);
			}
			return arrayList;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000B0C60 File Offset: 0x000AEE60
		internal static IDictionary CreateHashtable()
		{
			return new Hashtable();
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x000B0C67 File Offset: 0x000AEE67
		internal static IDictionary CreateHashtable(int capacity)
		{
			return new Hashtable(capacity);
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000B0C6F File Offset: 0x000AEE6F
		internal static IDictionary CreateHashtable(IDictionary dictionary)
		{
			return new Hashtable(dictionary);
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000B0C77 File Offset: 0x000AEE77
		internal static string ToLowerInvariant(string s)
		{
			return s.ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000B0C84 File Offset: 0x000AEE84
		internal static string ToUpperInvariant(string s)
		{
			return s.ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000B0C91 File Offset: 0x000AEE91
		internal static void Dispose(Stream s)
		{
			s.Close();
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000B0C99 File Offset: 0x000AEE99
		internal static void Dispose(TextWriter t)
		{
			t.Close();
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000B0CA1 File Offset: 0x000AEEA1
		internal static int IndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.IndexOf(source, value, CompareOptions.Ordinal);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x000B0CB4 File Offset: 0x000AEEB4
		internal static int LastIndexOf(string source, string value)
		{
			return Platform.InvariantCompareInfo.LastIndexOf(source, value, CompareOptions.Ordinal);
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x000B0CC7 File Offset: 0x000AEEC7
		internal static bool StartsWith(string source, string prefix)
		{
			return Platform.InvariantCompareInfo.IsPrefix(source, prefix, CompareOptions.Ordinal);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x000B0CDA File Offset: 0x000AEEDA
		internal static bool EndsWith(string source, string suffix)
		{
			return Platform.InvariantCompareInfo.IsSuffix(source, suffix, CompareOptions.Ordinal);
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000B0CED File Offset: 0x000AEEED
		internal static string GetTypeName(object obj)
		{
			return obj.GetType().FullName;
		}

		// Token: 0x04001686 RID: 5766
		private static readonly CompareInfo InvariantCompareInfo = CultureInfo.InvariantCulture.CompareInfo;

		// Token: 0x04001687 RID: 5767
		internal static readonly string NewLine = Platform.GetNewLine();
	}
}
