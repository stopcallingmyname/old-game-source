using System;
using System.Collections;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections
{
	// Token: 0x02000295 RID: 661
	public abstract class CollectionUtilities
	{
		// Token: 0x06001801 RID: 6145 RVA: 0x000B97FC File Offset: 0x000B79FC
		public static void AddRange(IList to, IEnumerable range)
		{
			foreach (object value in range)
			{
				to.Add(value);
			}
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000B984C File Offset: 0x000B7A4C
		public static bool CheckElementsAreOfType(IEnumerable e, Type t)
		{
			foreach (object o in e)
			{
				if (!t.IsInstanceOfType(o))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000B98A4 File Offset: 0x000B7AA4
		public static IDictionary ReadOnly(IDictionary d)
		{
			return new UnmodifiableDictionaryProxy(d);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000B98AC File Offset: 0x000B7AAC
		public static IList ReadOnly(IList l)
		{
			return new UnmodifiableListProxy(l);
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000B98B4 File Offset: 0x000B7AB4
		public static ISet ReadOnly(ISet s)
		{
			return new UnmodifiableSetProxy(s);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000B98BC File Offset: 0x000B7ABC
		public static object RequireNext(IEnumerator e)
		{
			if (!e.MoveNext())
			{
				throw new InvalidOperationException();
			}
			return e.Current;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000B98D4 File Offset: 0x000B7AD4
		public static string ToString(IEnumerable c)
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			IEnumerator enumerator = c.GetEnumerator();
			if (enumerator.MoveNext())
			{
				stringBuilder.Append(enumerator.Current.ToString());
				while (enumerator.MoveNext())
				{
					stringBuilder.Append(", ");
					stringBuilder.Append(enumerator.Current.ToString());
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}
	}
}
