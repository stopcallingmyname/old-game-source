using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x02000198 RID: 408
	public sealed class SampleDescriptor
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00097B27 File Offset: 0x00095D27
		// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x00097B2F File Offset: 0x00095D2F
		public bool IsLabel { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00097B38 File Offset: 0x00095D38
		// (set) Token: 0x06000ED7 RID: 3799 RVA: 0x00097B40 File Offset: 0x00095D40
		public Type Type { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x00097B49 File Offset: 0x00095D49
		// (set) Token: 0x06000ED9 RID: 3801 RVA: 0x00097B51 File Offset: 0x00095D51
		public string DisplayName { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000EDA RID: 3802 RVA: 0x00097B5A File Offset: 0x00095D5A
		// (set) Token: 0x06000EDB RID: 3803 RVA: 0x00097B62 File Offset: 0x00095D62
		public string Description { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000EDC RID: 3804 RVA: 0x00097B6B File Offset: 0x00095D6B
		// (set) Token: 0x06000EDD RID: 3805 RVA: 0x00097B73 File Offset: 0x00095D73
		public bool IsSelected { get; set; }

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x00097B7C File Offset: 0x00095D7C
		// (set) Token: 0x06000EDF RID: 3807 RVA: 0x00097B84 File Offset: 0x00095D84
		public GameObject UnityObject { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000EE0 RID: 3808 RVA: 0x00097B8D File Offset: 0x00095D8D
		public bool IsRunning
		{
			get
			{
				return this.UnityObject != null;
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x00097B9B File Offset: 0x00095D9B
		public SampleDescriptor(Type type, string displayName, string description)
		{
			this.Type = type;
			this.DisplayName = displayName;
			this.Description = description;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00097BB8 File Offset: 0x00095DB8
		public void CreateUnityObject()
		{
			if (this.UnityObject != null)
			{
				return;
			}
			this.UnityObject = new GameObject(this.DisplayName);
			this.UnityObject.AddComponent(this.Type);
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00097BEC File Offset: 0x00095DEC
		public void DestroyUnityObject()
		{
			if (this.UnityObject != null)
			{
				Object.Destroy(this.UnityObject);
				this.UnityObject = null;
			}
		}
	}
}
