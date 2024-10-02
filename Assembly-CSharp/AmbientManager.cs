using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class AmbientManager : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002484 File Offset: 0x00000684
	private void Start()
	{
		AmbientManager.THIS = this;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000248C File Offset: 0x0000068C
	private void Update()
	{
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000248C File Offset: 0x0000068C
	public void StartAmbient()
	{
	}

	// Token: 0x04000008 RID: 8
	public static AmbientManager THIS;

	// Token: 0x04000009 RID: 9
	public AudioSource AmbientAudio;

	// Token: 0x0400000A RID: 10
	private float timer;
}
