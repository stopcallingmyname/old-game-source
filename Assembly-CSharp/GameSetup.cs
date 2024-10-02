using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000033 RID: 51
public class GameSetup : MonoBehaviour
{
	// Token: 0x0600019B RID: 411 RVA: 0x00023066 File Offset: 0x00021266
	private void OnLevelLoad(Scene scene, LoadSceneMode mode)
	{
		if (GameSetup.blockSet != null)
		{
			base.GetComponent<Map>().SetBlockSet(GameSetup.blockSet);
		}
	}

	// Token: 0x0600019C RID: 412 RVA: 0x00023085 File Offset: 0x00021285
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.OnLevelLoad;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x00023098 File Offset: 0x00021298
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.OnLevelLoad;
	}

	// Token: 0x04000171 RID: 369
	public static BlockSet blockSet;
}
