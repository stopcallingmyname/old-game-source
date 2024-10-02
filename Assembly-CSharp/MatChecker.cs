using System;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class MatChecker : MonoBehaviour
{
	// Token: 0x06000030 RID: 48 RVA: 0x00002940 File Offset: 0x00000B40
	private void Start()
	{
		this.myRenderer = base.GetComponent<Renderer>();
		if (this.myRenderer == null)
		{
			return;
		}
		this.myMat = this.myRenderer.material;
		if (this.myMat == null)
		{
			return;
		}
		this.myShader = this.myMat.shader;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0000299C File Offset: 0x00000B9C
	private void FixedUpdate()
	{
		if (this.myRenderer == null)
		{
			return;
		}
		if (this.myMat == null)
		{
			return;
		}
		if (this.myMat.shader == this.myShader)
		{
			return;
		}
		this.myMat.shader = this.myShader;
	}

	// Token: 0x0400001D RID: 29
	private Renderer myRenderer;

	// Token: 0x0400001E RID: 30
	private Material myMat;

	// Token: 0x0400001F RID: 31
	private Shader myShader;
}
