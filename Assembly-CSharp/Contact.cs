using System;
using UnityEngine;

// Token: 0x02000129 RID: 297
public struct Contact
{
	// Token: 0x17000062 RID: 98
	// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00085204 File Offset: 0x00083404
	public float sqrDistance
	{
		get
		{
			return (this.a - this.b).sqrMagnitude;
		}
	}

	// Token: 0x17000063 RID: 99
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0008522A File Offset: 0x0008342A
	public Vector3 delta
	{
		get
		{
			return this.a - this.b;
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0008523D File Offset: 0x0008343D
	public Contact(Vector3 a, Vector3 b)
	{
		this.a = a;
		this.b = b;
	}

	// Token: 0x0400107E RID: 4222
	public Vector3 a;

	// Token: 0x0400107F RID: 4223
	public Vector3 b;
}
