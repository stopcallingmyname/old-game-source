using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x02000282 RID: 642
	public class PemHeader
	{
		// Token: 0x060017A0 RID: 6048 RVA: 0x000B8293 File Offset: 0x000B6493
		public PemHeader(string name, string val)
		{
			this.name = name;
			this.val = val;
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x060017A1 RID: 6049 RVA: 0x000B82A9 File Offset: 0x000B64A9
		public virtual string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x060017A2 RID: 6050 RVA: 0x000B82B1 File Offset: 0x000B64B1
		public virtual string Value
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000B82B9 File Offset: 0x000B64B9
		public override int GetHashCode()
		{
			return this.GetHashCode(this.name) + 31 * this.GetHashCode(this.val);
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000B82D8 File Offset: 0x000B64D8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			if (!(obj is PemHeader))
			{
				return false;
			}
			PemHeader pemHeader = (PemHeader)obj;
			return object.Equals(this.name, pemHeader.name) && object.Equals(this.val, pemHeader.val);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000B8322 File Offset: 0x000B6522
		private int GetHashCode(string s)
		{
			if (s == null)
			{
				return 1;
			}
			return s.GetHashCode();
		}

		// Token: 0x0400180E RID: 6158
		private string name;

		// Token: 0x0400180F RID: 6159
		private string val;
	}
}
