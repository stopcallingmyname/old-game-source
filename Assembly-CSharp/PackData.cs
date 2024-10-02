using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020000B7 RID: 183
public class PackData : MonoBehaviour
{
	// Token: 0x060005E1 RID: 1505 RVA: 0x0000248C File Offset: 0x0000068C
	private void Awake()
	{
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x00068A58 File Offset: 0x00066C58
	public void PackPlayerPos(int id, float a, float b, float c)
	{
		this.hashpos[id] = "Player" + (a + b + c).ToString("0");
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x00068A8C File Offset: 0x00066C8C
	public void CheckPlayerPos(int id)
	{
		if ("Player" + (RemotePlayersUpdater.Instance.Bots[id].position.x + RemotePlayersUpdater.Instance.Bots[id].position.y + RemotePlayersUpdater.Instance.Bots[id].position.z).ToString("0") != this.hashpos[id])
		{
			SceneManager.LoadScene(0);
		}
	}

	// Token: 0x04000C49 RID: 3145
	private string[] hashpos = new string[32];
}
