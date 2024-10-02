using System;
using UnityEngine;

namespace AssemblyCSharp
{
	// Token: 0x0200014C RID: 332
	public class BotData
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x0008B005 File Offset: 0x00089205
		public BotData()
		{
			this.Active = false;
			this.AnimState = 0;
			this.uid = "";
		}

		// Token: 0x040011C5 RID: 4549
		public bool Active;

		// Token: 0x040011C6 RID: 4550
		public int State;

		// Token: 0x040011C7 RID: 4551
		public int AnimState;

		// Token: 0x040011C8 RID: 4552
		public byte Team;

		// Token: 0x040011C9 RID: 4553
		public byte Dead;

		// Token: 0x040011CA RID: 4554
		public byte Helmet;

		// Token: 0x040011CB RID: 4555
		public int Skin;

		// Token: 0x040011CC RID: 4556
		public int Znak;

		// Token: 0x040011CD RID: 4557
		public byte CountryID;

		// Token: 0x040011CE RID: 4558
		public int WeaponID;

		// Token: 0x040011CF RID: 4559
		public Vector3 oldpos;

		// Token: 0x040011D0 RID: 4560
		public Vector3 position;

		// Token: 0x040011D1 RID: 4561
		public Vector3 rotation;

		// Token: 0x040011D2 RID: 4562
		public int Stats_Kills;

		// Token: 0x040011D3 RID: 4563
		public int Stats_Deads;

		// Token: 0x040011D4 RID: 4564
		public string Name;

		// Token: 0x040011D5 RID: 4565
		public string uid;

		// Token: 0x040011D6 RID: 4566
		public bool inVehicle;

		// Token: 0x040011D7 RID: 4567
		public int inVehiclePos;

		// Token: 0x040011D8 RID: 4568
		public string ClanName;

		// Token: 0x040011D9 RID: 4569
		public int ClanID;

		// Token: 0x040011DA RID: 4570
		public int[] Item;

		// Token: 0x040011DB RID: 4571
		public GameObject[] weapon;

		// Token: 0x040011DC RID: 4572
		public GameObject[] flash;

		// Token: 0x040011DD RID: 4573
		public float flash_time;

		// Token: 0x040011DE RID: 4574
		public ParticleSystem flamePS;

		// Token: 0x040011DF RID: 4575
		public Sound mySound;

		// Token: 0x040011E0 RID: 4576
		public GameObject goHelmet;

		// Token: 0x040011E1 RID: 4577
		public GameObject goCap;

		// Token: 0x040011E2 RID: 4578
		public GameObject goTykva;

		// Token: 0x040011E3 RID: 4579
		public GameObject goKolpak;

		// Token: 0x040011E4 RID: 4580
		public GameObject goRoga;

		// Token: 0x040011E5 RID: 4581
		public GameObject goMaskBear;

		// Token: 0x040011E6 RID: 4582
		public GameObject goMaskFox;

		// Token: 0x040011E7 RID: 4583
		public GameObject goMaskRabbit;

		// Token: 0x040011E8 RID: 4584
		public GameObject m_Top;

		// Token: 0x040011E9 RID: 4585
		public GameObject m_Face;

		// Token: 0x040011EA RID: 4586
		public GameObject SpecView;

		// Token: 0x040011EB RID: 4587
		public bool zombie;

		// Token: 0x040011EC RID: 4588
		public int blockFlag = -1;

		// Token: 0x040011ED RID: 4589
		public int currBlockType = 1;

		// Token: 0x040011EE RID: 4590
		public Block b;

		// Token: 0x040011EF RID: 4591
		public Block bUp;
	}
}
