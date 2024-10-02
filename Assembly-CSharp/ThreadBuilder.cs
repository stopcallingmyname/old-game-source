using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class ThreadBuilder : MonoBehaviour
{
	// Token: 0x06000243 RID: 579 RVA: 0x0002B818 File Offset: 0x00029A18
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		for (int i = 0; i < 2; i++)
		{
			this.myJob[i] = new Job();
			this.myJob[i].InData = new Vector3[10];
			this.myJob[i].Start();
		}
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0002B86C File Offset: 0x00029A6C
	private void Update()
	{
		for (int i = 0; i < 2; i++)
		{
			if (this.myJob[i] != null && this.myJob[i].Update())
			{
				this.myJob[i].Restart();
			}
		}
	}

	// Token: 0x040002F4 RID: 756
	private Job[] myJob = new Job[2];
}
