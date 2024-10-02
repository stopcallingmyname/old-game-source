using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020007EF RID: 2031
	public sealed class HeaderValue
	{
		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06004847 RID: 18503 RVA: 0x00198998 File Offset: 0x00196B98
		// (set) Token: 0x06004848 RID: 18504 RVA: 0x001989A0 File Offset: 0x00196BA0
		public string Key { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06004849 RID: 18505 RVA: 0x001989A9 File Offset: 0x00196BA9
		// (set) Token: 0x0600484A RID: 18506 RVA: 0x001989B1 File Offset: 0x00196BB1
		public string Value { get; set; }

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x0600484B RID: 18507 RVA: 0x001989BA File Offset: 0x00196BBA
		// (set) Token: 0x0600484C RID: 18508 RVA: 0x001989C2 File Offset: 0x00196BC2
		public List<HeaderValue> Options { get; set; }

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x0600484D RID: 18509 RVA: 0x001989CB File Offset: 0x00196BCB
		public bool HasValue
		{
			get
			{
				return !string.IsNullOrEmpty(this.Value);
			}
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x00022F1F File Offset: 0x0002111F
		public HeaderValue()
		{
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001989DB File Offset: 0x00196BDB
		public HeaderValue(string key)
		{
			this.Key = key;
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x001989EA File Offset: 0x00196BEA
		public void Parse(string headerStr, ref int pos)
		{
			this.ParseImplementation(headerStr, ref pos, true);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001989F8 File Offset: 0x00196BF8
		public bool TryGetOption(string key, out HeaderValue option)
		{
			option = null;
			if (this.Options == null || this.Options.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.Options.Count; i++)
			{
				if (string.Equals(this.Options[i].Key, key, StringComparison.OrdinalIgnoreCase))
				{
					option = this.Options[i];
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x00198A60 File Offset: 0x00196C60
		private void ParseImplementation(string headerStr, ref int pos, bool isOptionIsAnOption)
		{
			string key = headerStr.Read(ref pos, (char ch) => ch != ';' && ch != '=' && ch != ',', true);
			this.Key = key;
			char? c = headerStr.Peek(pos - 1);
			char? c2 = c;
			int? num = (c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null;
			int num2 = 61;
			bool flag = num.GetValueOrDefault() == num2 & num != null;
			bool flag2;
			if (isOptionIsAnOption)
			{
				c2 = c;
				num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				num2 = 59;
				flag2 = (num.GetValueOrDefault() == num2 & num != null);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			while ((c != null && flag) || flag3)
			{
				if (flag)
				{
					string value = headerStr.ReadPossibleQuotedText(ref pos);
					this.Value = value;
				}
				else if (flag3)
				{
					HeaderValue headerValue = new HeaderValue();
					headerValue.ParseImplementation(headerStr, ref pos, false);
					if (this.Options == null)
					{
						this.Options = new List<HeaderValue>();
					}
					this.Options.Add(headerValue);
				}
				if (!isOptionIsAnOption)
				{
					return;
				}
				c = headerStr.Peek(pos - 1);
				c2 = c;
				num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
				num2 = 61;
				flag = (num.GetValueOrDefault() == num2 & num != null);
				bool flag4;
				if (isOptionIsAnOption)
				{
					c2 = c;
					num = ((c2 != null) ? new int?((int)c2.GetValueOrDefault()) : null);
					num2 = 59;
					flag4 = (num.GetValueOrDefault() == num2 & num != null);
				}
				else
				{
					flag4 = false;
				}
				flag3 = flag4;
			}
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x00198C19 File Offset: 0x00196E19
		public override string ToString()
		{
			if (!string.IsNullOrEmpty(this.Value))
			{
				return this.Key + '=' + this.Value;
			}
			return this.Key;
		}
	}
}
