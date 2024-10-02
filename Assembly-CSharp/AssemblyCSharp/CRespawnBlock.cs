using System;
using UnityEngine;

namespace AssemblyCSharp
{
	// Token: 0x0200014D RID: 333
	public class CRespawnBlock
	{
		// Token: 0x06000B41 RID: 2881 RVA: 0x0008B034 File Offset: 0x00089234
		public CRespawnBlock(int t, int x, int y, int z, int gm)
		{
			this.mode = gm;
			this.team = t;
			this.pos = new Vector3((float)x, (float)y, (float)z);
		}

		// Token: 0x040011F0 RID: 4592
		public int mode;

		// Token: 0x040011F1 RID: 4593
		public int team;

		// Token: 0x040011F2 RID: 4594
		public Vector3 pos;
	}
}
