using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
public class flakesnow
{
	// Token: 0x06000584 RID: 1412 RVA: 0x00066C0B File Offset: 0x00064E0B
	public flakesnow(Texture2D tex_fl, Rect rec_fl)
	{
		this.tex_flake = tex_fl;
		this.rec_flake = rec_fl;
	}

	// Token: 0x04000A03 RID: 2563
	public Rect rec_flake;

	// Token: 0x04000A04 RID: 2564
	public Texture2D tex_flake;
}
