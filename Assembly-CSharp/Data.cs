using System;
using AssemblyCSharp;
using UnityEngine;

// Token: 0x02000028 RID: 40
public class Data : MonoBehaviour
{
	// Token: 0x06000132 RID: 306 RVA: 0x00017063 File Offset: 0x00015263
	public void SetIndex(RemotePlayersUpdater csbc, int id, int _hitzone)
	{
		this.index = id;
		this.data = csbc.Bots[id];
		this.go = csbc.BotsGmObj[id];
		this.hitzone = _hitzone;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0001708F File Offset: 0x0001528F
	public void SetIndex(int id, int _hitzone)
	{
		this.index = id;
		this.hitzone = _hitzone;
	}

	// Token: 0x0400014C RID: 332
	public GameObject go;

	// Token: 0x0400014D RID: 333
	public BotData data;

	// Token: 0x0400014E RID: 334
	public int index;

	// Token: 0x0400014F RID: 335
	public int hitzone;

	// Token: 0x04000150 RID: 336
	public bool isGost;
}
