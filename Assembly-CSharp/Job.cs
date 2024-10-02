using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class Job : ThreadedJob
{
	// Token: 0x06000240 RID: 576 RVA: 0x0002B794 File Offset: 0x00029994
	protected override void ThreadFunction()
	{
		for (int i = 0; i < 100000000; i++)
		{
			this.InData[i % this.InData.Length] += this.InData[(i + 1) % this.InData.Length];
		}
	}

	// Token: 0x06000241 RID: 577 RVA: 0x0002B7F0 File Offset: 0x000299F0
	protected override void OnFinished()
	{
		for (int i = 0; i < this.InData.Length; i++)
		{
		}
	}

	// Token: 0x040002F2 RID: 754
	public Vector3[] InData;

	// Token: 0x040002F3 RID: 755
	public Vector3[] OutData;
}
