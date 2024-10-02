using System;
using UnityEngine;

// Token: 0x02000009 RID: 9
public class CustomAnimation : MonoBehaviour
{
	// Token: 0x06000022 RID: 34 RVA: 0x0000271B File Offset: 0x0000091B
	private void Awake()
	{
		this.rightColtMuzzle.SetActive(false);
		this.leftColtMuzzle.SetActive(false);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002738 File Offset: 0x00000938
	public void DrawIn()
	{
		this.rightColtMuzzle.SetActive(false);
		this.leftColtMuzzle.SetActive(false);
		this.rightColt.GetComponent<Animation>().Play("DRAW_IN");
		this.leftColt.GetComponent<Animation>().Play("DRAW_IN");
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002789 File Offset: 0x00000989
	public void RightColtFire()
	{
		this.rightColt.GetComponent<Animation>().Play("FIRE");
		this.rightColtMuzzle.SetActive(true);
		this.muzzleTime = Time.time + 0.05f;
		this.rightFiring = true;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000027C5 File Offset: 0x000009C5
	public void LeftColtFire()
	{
		this.leftColt.GetComponent<Animation>().Play("FIRE");
		this.leftColtMuzzle.SetActive(true);
		this.muzzleTime = Time.time + 0.05f;
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000027FA File Offset: 0x000009FA
	private void Update()
	{
		if (this.muzzleTime > 0f && this.muzzleTime < Time.time)
		{
			this.rightColtMuzzle.SetActive(false);
			this.leftColtMuzzle.SetActive(false);
			this.muzzleTime = 0f;
		}
	}

	// Token: 0x04000013 RID: 19
	public GameObject rightColt;

	// Token: 0x04000014 RID: 20
	public GameObject leftColt;

	// Token: 0x04000015 RID: 21
	public GameObject rightColtMuzzle;

	// Token: 0x04000016 RID: 22
	public GameObject leftColtMuzzle;

	// Token: 0x04000017 RID: 23
	private float muzzleTime;

	// Token: 0x04000018 RID: 24
	public bool rightFiring;
}
