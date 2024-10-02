using System;

namespace LitJson
{
	// Token: 0x02000155 RID: 341
	internal struct ArrayMetadata
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0008BF9C File Offset: 0x0008A19C
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0008BFBD File Offset: 0x0008A1BD
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0008BFC6 File Offset: 0x0008A1C6
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0008BFCE File Offset: 0x0008A1CE
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0008BFD7 File Offset: 0x0008A1D7
		// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0008BFDF File Offset: 0x0008A1DF
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x0400120A RID: 4618
		private Type element_type;

		// Token: 0x0400120B RID: 4619
		private bool is_array;

		// Token: 0x0400120C RID: 4620
		private bool is_list;
	}
}
