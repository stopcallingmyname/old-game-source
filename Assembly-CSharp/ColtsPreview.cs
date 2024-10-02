using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class ColtsPreview : MonoBehaviour
{
	// Token: 0x06000018 RID: 24 RVA: 0x00002496 File Offset: 0x00000696
	private void OnEnable()
	{
		this.tmp.SetActive(true);
	}

	// Token: 0x06000019 RID: 25 RVA: 0x000024A4 File Offset: 0x000006A4
	private void OnDisable()
	{
		this.tmp.SetActive(false);
	}

	// Token: 0x0400000B RID: 11
	public GameObject tmp;
}
