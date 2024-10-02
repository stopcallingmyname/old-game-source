using System;
using UnityEngine;

namespace AssemblyCSharp
{
	// Token: 0x0200014B RID: 331
	public class CEnt
	{
		// Token: 0x06000B3D RID: 2877 RVA: 0x0008AFC2 File Offset: 0x000891C2
		public CEnt()
		{
			this.Active = false;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0008AFD4 File Offset: 0x000891D4
		~CEnt()
		{
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x0008AFFC File Offset: 0x000891FC
		public void SetActive(bool val)
		{
			this.Active = val;
		}

		// Token: 0x040011BA RID: 4538
		public int uid;

		// Token: 0x040011BB RID: 4539
		public int index;

		// Token: 0x040011BC RID: 4540
		public GameObject go;

		// Token: 0x040011BD RID: 4541
		private bool Active;

		// Token: 0x040011BE RID: 4542
		private string classname;

		// Token: 0x040011BF RID: 4543
		public int classID;

		// Token: 0x040011C0 RID: 4544
		public Vector3 position;

		// Token: 0x040011C1 RID: 4545
		public Vector3 rotation;

		// Token: 0x040011C2 RID: 4546
		public int team;

		// Token: 0x040011C3 RID: 4547
		public int skin;

		// Token: 0x040011C4 RID: 4548
		public int ownerID;
	}
}
