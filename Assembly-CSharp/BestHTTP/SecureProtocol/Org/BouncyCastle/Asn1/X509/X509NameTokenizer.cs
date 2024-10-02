using System;
using System.Text;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C8 RID: 1736
	public class X509NameTokenizer
	{
		// Token: 0x06004015 RID: 16405 RVA: 0x0017D3C1 File Offset: 0x0017B5C1
		public X509NameTokenizer(string oid) : this(oid, ',')
		{
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x0017D3CC File Offset: 0x0017B5CC
		public X509NameTokenizer(string oid, char separator)
		{
			this.value = oid;
			this.index = -1;
			this.separator = separator;
		}

		// Token: 0x06004017 RID: 16407 RVA: 0x0017D3F4 File Offset: 0x0017B5F4
		public bool HasMoreTokens()
		{
			return this.index != this.value.Length;
		}

		// Token: 0x06004018 RID: 16408 RVA: 0x0017D40C File Offset: 0x0017B60C
		public string NextToken()
		{
			if (this.index == this.value.Length)
			{
				return null;
			}
			int num = this.index + 1;
			bool flag = false;
			bool flag2 = false;
			this.buffer.Remove(0, this.buffer.Length);
			while (num != this.value.Length)
			{
				char c = this.value[num];
				if (c == '"')
				{
					if (!flag2)
					{
						flag = !flag;
					}
					else
					{
						this.buffer.Append(c);
						flag2 = false;
					}
				}
				else if (flag2 || flag)
				{
					if (c == '#' && this.buffer[this.buffer.Length - 1] == '=')
					{
						this.buffer.Append('\\');
					}
					else if (c == '+' && this.separator != '+')
					{
						this.buffer.Append('\\');
					}
					this.buffer.Append(c);
					flag2 = false;
				}
				else if (c == '\\')
				{
					flag2 = true;
				}
				else
				{
					if (c == this.separator)
					{
						break;
					}
					this.buffer.Append(c);
				}
				num++;
			}
			this.index = num;
			return this.buffer.ToString().Trim();
		}

		// Token: 0x040028B5 RID: 10421
		private string value;

		// Token: 0x040028B6 RID: 10422
		private int index;

		// Token: 0x040028B7 RID: 10423
		private char separator;

		// Token: 0x040028B8 RID: 10424
		private StringBuilder buffer = new StringBuilder();
	}
}
