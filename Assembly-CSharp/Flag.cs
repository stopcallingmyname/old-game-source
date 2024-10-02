using System;
using UnityEngine;

// Token: 0x02000117 RID: 279
public class Flag : MonoBehaviour
{
	// Token: 0x06000A11 RID: 2577 RVA: 0x000814C0 File Offset: 0x0007F6C0
	public void SetFlag(Vector3i _pos, int _t1, int _t2)
	{
		this.map = Object.FindObjectOfType<Map>();
		if (this.map == null)
		{
			return;
		}
		this.pos = _pos;
		for (int i = this.pos.y; i <= this.pos.y + this.maxOffset; i++)
		{
			this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock(4)), new Vector3i(this.pos.x, i, this.pos.z));
		}
		this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock(4)), new Vector3i(this.pos.x + 1, this.pos.y, this.pos.z));
		this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock(4)), new Vector3i(this.pos.x - 1, this.pos.y, this.pos.z));
		this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock(4)), new Vector3i(this.pos.x, this.pos.y, this.pos.z + 1));
		this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock(4)), new Vector3i(this.pos.x, this.pos.y, this.pos.z - 1));
		for (int j = this.pos.x + 1; j <= this.pos.x + 4; j++)
		{
			this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock((_t2 > _t1) ? 37 : 38)), new Vector3i(j, this.pos.y + this.maxOffset, this.pos.z));
			this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock((_t2 > _t1) ? 37 : 38)), new Vector3i(j, this.pos.y + this.maxOffset - 1, this.pos.z));
		}
		if (_t1 < 150)
		{
			this.flagObject = HoloBase.Create(this.pos, 777);
			this.FP = this.flagObject.GetComponent<FlagPlase>();
			if (this.FP != null && _t1 > _t2)
			{
				this.FP.team = 0;
			}
		}
		this.timer[0] = _t1;
		this.timer[1] = _t2;
		this.inited = true;
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x000817A8 File Offset: 0x0007F9A8
	public void UpdateFlag(int _t1, int _t2)
	{
		this.timer[0] = _t1;
		this.timer[1] = _t2;
		int num = 1;
		int num2 = this.timer[1];
		if (this.timer[0] >= num2)
		{
			num2 = this.timer[0];
			num = 0;
		}
		num2 = num2 * 10 / 150 + 10;
		if (num2 != this.maxOffset || num2 == 20)
		{
			for (int i = this.pos.x + 1; i <= this.pos.x + 4; i++)
			{
				this.map.SetBlockAndRecompute(new BlockData(null), new Vector3i(i, this.pos.y + this.maxOffset, this.pos.z));
				this.map.SetBlockAndRecompute(new BlockData(null), new Vector3i(i, this.pos.y + this.maxOffset - 1, this.pos.z));
				this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock((num == 1) ? 37 : 38)), new Vector3i(i, this.pos.y + num2, this.pos.z));
				this.map.SetBlockAndRecompute(new BlockData(this.map.GetBlockSet().GetBlock((num == 1) ? 37 : 38)), new Vector3i(i, this.pos.y + num2 - 1, this.pos.z));
			}
			this.maxOffset = num2;
		}
		if (this.FP != null)
		{
			this.FP.accepted = true;
			if (_t1 > _t2)
			{
				this.FP.team = 0;
			}
			else
			{
				this.FP.team = 1;
			}
		}
		if (this.timer[0] == 150)
		{
			Object.Destroy(this.flagObject);
		}
	}

	// Token: 0x04000FBF RID: 4031
	public int[] timer = new int[2];

	// Token: 0x04000FC0 RID: 4032
	public Vector3i pos;

	// Token: 0x04000FC1 RID: 4033
	private Map map;

	// Token: 0x04000FC2 RID: 4034
	public bool inited;

	// Token: 0x04000FC3 RID: 4035
	public int maxOffset = 20;

	// Token: 0x04000FC4 RID: 4036
	public GameObject flagObject;

	// Token: 0x04000FC5 RID: 4037
	public FlagPlase FP;
}
