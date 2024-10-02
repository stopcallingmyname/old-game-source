using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000156 RID: 342
	internal struct ObjectMetadata
	{
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0008BFE8 File Offset: 0x0008A1E8
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0008C009 File Offset: 0x0008A209
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

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0008C012 File Offset: 0x0008A212
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0008C01A File Offset: 0x0008A21A
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0008C023 File Offset: 0x0008A223
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0008C02B File Offset: 0x0008A22B
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x0400120D RID: 4621
		private Type element_type;

		// Token: 0x0400120E RID: 4622
		private bool is_dictionary;

		// Token: 0x0400120F RID: 4623
		private IDictionary<string, PropertyMetadata> properties;
	}
}
