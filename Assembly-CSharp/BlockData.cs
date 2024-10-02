using System;

// Token: 0x02000101 RID: 257
public struct BlockData
{
	// Token: 0x0600093F RID: 2367 RVA: 0x0007EC3E File Offset: 0x0007CE3E
	public BlockData(Block block)
	{
		this.block = block;
		this.direction = BlockDirection.Z_PLUS;
		this.health = 0;
	}

	// Token: 0x06000940 RID: 2368 RVA: 0x0007EC55 File Offset: 0x0007CE55
	public void SetDirection(BlockDirection direction)
	{
		this.direction = direction;
	}

	// Token: 0x06000941 RID: 2369 RVA: 0x0007EC5E File Offset: 0x0007CE5E
	public BlockDirection GetDirection()
	{
		return this.direction;
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x0007EC66 File Offset: 0x0007CE66
	public byte GetLight()
	{
		if (this.block == null)
		{
			return 0;
		}
		return this.block.GetLight();
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0007EC7D File Offset: 0x0007CE7D
	public bool IsEmpty()
	{
		return this.block == null;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x0007EC88 File Offset: 0x0007CE88
	public bool IsAlpha()
	{
		return this.IsEmpty() || this.block.IsAlpha();
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x0007EC9F File Offset: 0x0007CE9F
	public bool IsSolid()
	{
		return this.block != null && this.block.IsSolid();
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x0007ECB6 File Offset: 0x0007CEB6
	public bool IsFluid()
	{
		return this.block is FluidBlock;
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x0007ECC6 File Offset: 0x0007CEC6
	public void SetHealth(byte _health)
	{
		this.health = _health;
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x0007ECCF File Offset: 0x0007CECF
	public byte GetHealth()
	{
		return this.health;
	}

	// Token: 0x04000F5D RID: 3933
	public Block block;

	// Token: 0x04000F5E RID: 3934
	private BlockDirection direction;

	// Token: 0x04000F5F RID: 3935
	private byte health;
}
