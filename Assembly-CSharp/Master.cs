using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200001A RID: 26
public class Master : MonoBehaviour
{
	// Token: 0x060000A2 RID: 162 RVA: 0x0000B5B3 File Offset: 0x000097B3
	private void myGlobalInit()
	{
		this.Active = false;
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x0000B5BC File Offset: 0x000097BC
	private void ResetPos()
	{
		this.x_pos = 0;
		this.y_pos = 0;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x0000B5CC File Offset: 0x000097CC
	private void DrawCategory(ITEMS_CATEGORY cat)
	{
		int num = 0;
		int num2 = this.y_pos;
		int num3 = this.x_pos;
		this.y_pos += 30;
		this.x_pos = 184;
		this.icount = 0;
		foreach (ItemData itemData in ItemsDB.Items)
		{
			if (itemData != null && itemData.Category == (int)cat && itemData.LastCount != 0 && itemData.canUpgrade && itemData.ItemID != 35)
			{
				ItemsDrawer.THIS.DrawItem(itemData.ItemID, ITEMS_THEME.STANDART, new Rect((float)this.x_pos, (float)this.y_pos, 128f, 64f), true);
				this.NextPos(false);
				num++;
			}
		}
		if (num > 0)
		{
			string label = Lang.GetLabel((int)(575 + cat));
			GUI.DrawTexture(new Rect(181f, (float)num2, 400f, 26f), GUIManager.tex_category);
			GUIManager.DrawText(new Rect(197f, (float)num2, 400f, 26f), label, 16, TextAnchor.MiddleLeft, 8);
			this.NextPos(true);
			return;
		}
		this.y_pos = num2;
		this.x_pos = num3;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000B6F8 File Offset: 0x000098F8
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.MAINMENU)
		{
			return;
		}
		if (GM.currGUIState != GUIGS.MASTER)
		{
			return;
		}
		GUI.Window(904, new Rect((float)Screen.width / 2f - 300f, 199f, 600f, GUIManager.YRES(768f) - 180f), new GUI.WindowFunction(this.DoWindow), "", GUIManager.gs_empty);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000B76C File Offset: 0x0000996C
	private void DoWindow(int windowID)
	{
		if (!this.Active)
		{
			return;
		}
		GUI.DrawTexture(new Rect(0f, 0f, 600f, 32f), GUIManager.tex_panel);
		GUI.DrawTexture(new Rect(0f, 32f, 600f, GUIManager.YRES(768f) - 199f), GUIManager.tex_half_black);
		ItemsDrawer.THIS.DrawPlayer();
		this.DrawActiveItem();
		this.y_cat_ofs = 0;
		this.DrawMode(Lang.GetLabel(173), 135, 0, 0, 160);
		this.DrawMode(Lang.GetLabel(174), 300, 0, 1, 160);
		if (this.type == 0)
		{
			this.DrawCategory0();
			return;
		}
		if (this.type == 1)
		{
			this.DrawCategory1();
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000B840 File Offset: 0x00009A40
	private void DrawMode(string name, int x, int y, int id, int length)
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= 192f;
		Rect rect = new Rect((float)x, (float)y, (float)length, 32f);
		if (this.type != id)
		{
			if (rect.Contains(new Vector2(num, num2)))
			{
				if (!this.hovermode[id])
				{
					this.hovermode[id] = true;
					MainMenu.MainAS.PlayOneShot(SoundManager.SoundHover, AudioListener.volume);
				}
			}
			else if (this.hovermode[id])
			{
				this.hovermode[id] = false;
			}
		}
		if (this.type == id)
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, (float)length, 32f), GUIManager.tex_select);
		}
		else if (this.hovermode[id])
		{
			GUI.DrawTexture(new Rect((float)x, (float)y, (float)length, 32f), GUIManager.tex_hover);
		}
		GUIManager.DrawText(rect, name, 17, TextAnchor.MiddleCenter, 8);
		if (GUI.Button(rect, "", GUIManager.gs_empty))
		{
			MainMenu.MainAS.PlayOneShot(SoundManager.SoundClick, AudioListener.volume);
			this.type = id;
		}
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000B97C File Offset: 0x00009B7C
	public void OnActive()
	{
		ItemsDrawer.THIS.GetInvSkin();
		Shop.THIS.currItem = null;
		this.buylock = false;
		this.type = 0;
		if (Time.time < this.lastupdate + 5f)
		{
			return;
		}
		this.lastupdate = Time.time;
		base.StartCoroutine(this.get_inv());
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x0000B9D8 File Offset: 0x00009BD8
	public void DrawActiveItem()
	{
		float num = Input.mousePosition.x;
		float num2 = (float)Screen.height - Input.mousePosition.y;
		num -= (float)Screen.width / 2f - 300f;
		num2 -= 199f;
		GUI.DrawTexture(new Rect(184f, 36f, 412f, 152f), GUIManager.tex_half_black);
		if (Shop.THIS.currItem == null)
		{
			GUIManager.DrawText2(new Rect(184f, 36f, 412f, 152f), Lang.GetLabel(197), 16, TextAnchor.MiddleCenter, Color.green);
			return;
		}
		GUI.DrawTexture(new Rect(464f, 40f, 128f, 64f), Shop.THIS.currItem.icon);
		if (Shop.THIS.currItem.Type == 1)
		{
			bool hdamage = this.DrawButtonUpgrade(new Rect(286f, 110f, 64f, 64f), num, num2, 1, 5);
			bool hclip = this.DrawButtonUpgrade(new Rect(358f, 110f, 64f, 64f), num, num2, 2, 5);
			bool hbackpack = this.DrawButtonUpgrade(new Rect(430f, 110f, 64f, 64f), num, num2, 3, 5);
			this.DrawWeaponData(hdamage, hclip, hbackpack, false);
			GUIManager.DrawText2(new Rect(184f, 36f, 412f, 152f), Lang.GetLabel(199), 10, TextAnchor.LowerCenter, Color.yellow);
			return;
		}
		if (Shop.THIS.currItem.Type == 2)
		{
			bool hlife = this.DrawButtonUpgrade(new Rect(214f, 110f, 64f, 64f), num, num2, 1, 5);
			bool harmor = this.DrawButtonUpgrade(new Rect(286f, 110f, 64f, 64f), num, num2, 2, 5);
			bool hspeed = this.DrawButtonUpgrade(new Rect(358f, 110f, 64f, 64f), num, num2, 3, 5);
			bool hreload = this.DrawButtonUpgrade(new Rect(430f, 110f, 64f, 64f), num, num2, 4, 5);
			bool hturretRotation = this.DrawButtonUpgrade(new Rect(502f, 110f, 64f, 64f), num, num2, 5, 5);
			this.DrawVehicleData(hlife, harmor, hspeed, hreload, hturretRotation);
			GUIManager.DrawText2(new Rect(184f, 36f, 412f, 152f), Lang.GetLabel(200), 10, TextAnchor.LowerCenter, Color.yellow);
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000BC78 File Offset: 0x00009E78
	private IEnumerator get_inv()
	{
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"20&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		if (www.error == null)
		{
			string[] array = www.text.Split(new char[]
			{
				'^'
			});
			int num = array.Length - 1;
			for (int i = 0; i < num; i++)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				string[] array2 = array[i].Split(new char[]
				{
					'|'
				});
				if (!int.TryParse(array2[0], out num2))
				{
					yield break;
				}
				if (!int.TryParse(array2[1], out num3))
				{
					yield break;
				}
				if (!int.TryParse(array2[2], out num4))
				{
					yield break;
				}
				if (!int.TryParse(array2[3], out num5))
				{
					yield break;
				}
				if (!int.TryParse(array2[4], out num6))
				{
					yield break;
				}
				if (!int.TryParse(array2[5], out num7))
				{
					yield break;
				}
				if (num3 > 5)
				{
					num3 = 5;
				}
				if (num4 > 5)
				{
					num4 = 5;
				}
				if (num5 > 5)
				{
					num5 = 5;
				}
				if (num6 > 5)
				{
					num6 = 5;
				}
				if (num7 > 5)
				{
					num7 = 5;
				}
				if (ItemsDB.CheckItem(num2))
				{
					if (ItemsDB.Items[num2].MyUpgrades == null)
					{
						ItemsDB.Items[num2].MyUpgrades = new int[6];
					}
					ItemsDB.Items[num2].MyUpgrades[1] = num3;
					ItemsDB.Items[num2].MyUpgrades[2] = num4;
					ItemsDB.Items[num2].MyUpgrades[3] = num5;
					ItemsDB.Items[num2].MyUpgrades[4] = num6;
					ItemsDB.Items[num2].MyUpgrades[5] = num7;
				}
			}
		}
		yield break;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0000BC80 File Offset: 0x00009E80
	private void DrawWeaponData(bool hdamage, bool hclip, bool hbackpack, bool hzombie)
	{
		int x = 188;
		int num = 8;
		this.DrawBar(x, num + 32, Lang.GetLabel(110), (float)Shop.THIS.currItem.Upgrades[1][0].Val / (float)ItemsDB.Items[1000].Upgrades[1][0].Val, 1, 1, Shop.THIS.currItem.MyUpgrades[1], (float)ItemsDB.Items[1000].Upgrades[1][0].Val, hdamage);
		this.DrawBar(x, num + 45, Lang.GetLabel(111), (float)Shop.THIS.currItem.Upgrades[4][0].Val / (float)ItemsDB.Items[1000].Upgrades[4][0].Val, 0, -1, 0, 0f, false);
		this.DrawBar(x, num + 58, Lang.GetLabel(112), (float)Shop.THIS.currItem.Upgrades[2][0].Val / (float)ItemsDB.Items[1000].Upgrades[2][0].Val, 0, 2, Shop.THIS.currItem.MyUpgrades[2], (float)ItemsDB.Items[1000].Upgrades[2][0].Val, hclip);
		this.DrawBar(x, num + 71, Lang.GetLabel(113), (float)Shop.THIS.currItem.Upgrades[3][0].Val / (float)ItemsDB.Items[1000].Upgrades[3][0].Val, 0, 3, Shop.THIS.currItem.MyUpgrades[3], (float)ItemsDB.Items[1000].Upgrades[3][0].Val, hbackpack);
		this.DrawBar(x, num + 84, Lang.GetLabel(114), (float)Shop.THIS.currItem.Upgrades[5][0].Val / (float)ItemsDB.Items[1000].Upgrades[5][0].Val, 0, -1, 0, 0f, false);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000BE90 File Offset: 0x0000A090
	private void DrawVehicleData(bool hlife, bool harmor, bool hspeed, bool hreload, bool hturretRotation)
	{
		int x = 188;
		int num = 8;
		this.DrawBar(x, num + 32, Lang.GetLabel(186), (float)Shop.THIS.currItem.Upgrades[1][0].Val / (float)ItemsDB.Items[1001].Upgrades[1][0].Val, 1, 1, Shop.THIS.currItem.MyUpgrades[1], (float)ItemsDB.Items[1001].Upgrades[1][0].Val, hlife);
		this.DrawBar(x, num + 45, Lang.GetLabel(187), (float)Shop.THIS.currItem.Upgrades[2][0].Val / (float)ItemsDB.Items[1001].Upgrades[2][0].Val, 0, 2, Shop.THIS.currItem.MyUpgrades[2], (float)ItemsDB.Items[1001].Upgrades[2][0].Val, harmor);
		this.DrawBar(x, num + 58, Lang.GetLabel(188), (float)Shop.THIS.currItem.Upgrades[3][0].Val / (float)ItemsDB.Items[1001].Upgrades[3][0].Val, 0, 3, Shop.THIS.currItem.MyUpgrades[3], (float)ItemsDB.Items[1001].Upgrades[3][0].Val, hspeed);
		this.DrawBar(x, num + 71, Lang.GetLabel(189), (300f - (float)Shop.THIS.currItem.Upgrades[4][0].Val) / (float)ItemsDB.Items[1001].Upgrades[4][0].Val, 0, 4, Shop.THIS.currItem.MyUpgrades[4], (float)ItemsDB.Items[1001].Upgrades[4][0].Val, hreload);
		this.DrawBar(x, num + 84, Lang.GetLabel(190), (300f - (float)Shop.THIS.currItem.Upgrades[5][0].Val) / (float)ItemsDB.Items[1001].Upgrades[5][0].Val, 0, 5, Shop.THIS.currItem.MyUpgrades[5], (float)ItemsDB.Items[1001].Upgrades[5][0].Val, hturretRotation);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000C108 File Offset: 0x0000A308
	private void DrawBar(int x, int y, string text, float procent, int color, int upid, int level, float max, bool drawposlevel)
	{
		GUIManager.DrawText(new Rect((float)x, (float)y, 200f, 12f), text, 16, TextAnchor.MiddleLeft, 8);
		x += 100;
		float num = 160f * procent;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		if (level > 0)
		{
			if (upid == 1)
			{
				num2 = (float)Shop.THIS.currItem.Upgrades[1][level - 1].Val / max;
			}
			else if (upid == 2)
			{
				num2 = (float)Shop.THIS.currItem.Upgrades[2][level - 1].Val / max;
			}
			else if (upid == 3)
			{
				num2 = (float)Shop.THIS.currItem.Upgrades[3][level - 1].Val / max;
			}
			if (Shop.THIS.currItem.Type == 2)
			{
				if (upid == 4)
				{
					num2 = (300f - (float)Shop.THIS.currItem.Upgrades[4][level - 1].Val) / max;
				}
				else if (upid == 5)
				{
					num2 = (300f - (float)Shop.THIS.currItem.Upgrades[5][level - 1].Val) / max;
				}
			}
			num3 = 160f * num2;
			if (upid > 0 && num3 - num < (float)(level * 3))
			{
				num3 = num + (float)(level * 3);
				num2 = num3 / 160f;
			}
		}
		if (drawposlevel)
		{
			int num6 = level + 1;
			if (num6 > 5)
			{
				num6 = 5;
			}
			if (upid == 1)
			{
				num4 = (float)Shop.THIS.currItem.Upgrades[1][num6 - 1].Val / max;
			}
			else if (upid == 2)
			{
				num4 = (float)Shop.THIS.currItem.Upgrades[2][num6 - 1].Val / max;
			}
			else if (upid == 3)
			{
				num4 = (float)Shop.THIS.currItem.Upgrades[3][num6 - 1].Val / max;
			}
			if (Shop.THIS.currItem.Type == 2)
			{
				if (upid == 4)
				{
					num2 = (300f - (float)Shop.THIS.currItem.Upgrades[4][num6 - 1].Val) / max;
				}
				else if (upid == 5)
				{
					num2 = (300f - (float)Shop.THIS.currItem.Upgrades[5][num6 - 1].Val) / max;
				}
			}
			num5 = 160f * num4;
			if (level > 0)
			{
				if (num5 - num3 < 3f)
				{
					num5 = num3 + 3f;
				}
			}
			else if (num5 - num < 3f)
			{
				num5 = num + 3f;
			}
		}
		if (color == 0)
		{
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, 160f, 12f), GUIManager.tex_bars, new Rect(0f, 0.6666666f, 1f, 0.16666667f));
			if (drawposlevel)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num5, 12f), GUIManager.tex_bars, new Rect(0f, 0f, 1f * num4, 0.16666667f));
			}
			if (level > 0)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num3, 12f), GUIManager.tex_bars, new Rect(0f, 0.16666663f, 1f * num2, 0.16666667f));
			}
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num, 12f), GUIManager.tex_bars, new Rect(0f, 0.8333333f, 1f * procent, 0.16666667f));
			if (num > 1f)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x + num - 1f, (float)y, 2f, 12f), GUIManager.tex_bars, new Rect(0.9875f, 0.8333333f, 0.0125f, 0.16666667f));
				return;
			}
		}
		else
		{
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, 160f, 12f), GUIManager.tex_bars, new Rect(0f, 0.3333333f, 1f, 0.16666667f));
			if (drawposlevel)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num5, 12f), GUIManager.tex_bars, new Rect(0f, 0f, 1f * num4, 0.16666667f));
			}
			if (level > 0)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num3, 12f), GUIManager.tex_bars, new Rect(0f, 0.16666663f, 1f * num2, 0.16666667f));
			}
			GUI.DrawTextureWithTexCoords(new Rect((float)x, (float)y, num, 12f), GUIManager.tex_bars, new Rect(0f, 0.5f, 1f * procent, 0.16666667f));
			if (num > 1f)
			{
				GUI.DrawTextureWithTexCoords(new Rect((float)x + num - 1f, (float)y, 2f, 12f), GUIManager.tex_bars, new Rect(0.9875f, 0.5f, 0.0125f, 0.16666667f));
			}
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
	private bool DrawButtonUpgrade(Rect r, float mx, float my, int id, int money)
	{
		bool result = false;
		bool flag = false;
		int num = Shop.THIS.currItem.MyUpgrades[id];
		int num2 = num + 1;
		if (num2 >= Shop.THIS.currItem.Upgrades[id].Length - 1)
		{
			flag = true;
			num2 = Shop.THIS.currItem.Upgrades[id].Length - 1;
		}
		int num3;
		if (Shop.THIS.currItem.Upgrades[id][num2] == null)
		{
			num3 = 0;
		}
		else
		{
			num3 = Shop.THIS.currItem.Upgrades[id][num2].Cost;
		}
		if (num3 == 0)
		{
			flag = true;
		}
		if (flag)
		{
			GUI.DrawTexture(new Rect(r.x, r.y, 64f, 65f), GUIManager.tex_red);
		}
		if (r.Contains(new Vector2(mx, my)) && !flag)
		{
			if (!flag)
			{
				GUI.DrawTexture(new Rect(r.x, r.y, 64f, 65f), GUIManager.tex_half_yellow);
			}
			if (Shop.THIS.currItem.Type == 1)
			{
				GUI.DrawTexture(r, GUIManager.tex_upgrade_active[id]);
			}
			else if (Shop.THIS.currItem.Type == 2)
			{
				GUI.DrawTexture(r, GUIManager.tex_upgrade_vehicle_active[id]);
			}
			GUIManager.DrawText(new Rect(r.x + 12f, r.y + 10f, 20f, 64f), num3.ToString(), 16, TextAnchor.MiddleRight, 8);
			GUI.DrawTexture(new Rect(r.x + 34f, r.y + 32f, 16f, 16f), GUIManager.tex_coin);
			GUI.DrawTextureWithTexCoords(new Rect(r.x + 1f, r.y + 1f, 12.2f * (float)num2, 10f), GUIManager.tex_upgrade_bars, new Rect(0f, 0.375f, 12.2f * (float)num2 / 64f, 0.3125f));
			result = true;
		}
		else if (Shop.THIS.currItem.Type == 1)
		{
			GUI.DrawTexture(r, GUIManager.tex_upgrade[id]);
		}
		else if (Shop.THIS.currItem.Type == 2)
		{
			GUI.DrawTexture(r, GUIManager.tex_upgrade_vehicle[id]);
		}
		GUI.DrawTextureWithTexCoords(new Rect(r.x + 1f, r.y + 1f, 12.2f * (float)num, 10f), GUIManager.tex_upgrade_bars, new Rect(0f, 0.6875f, 12.2f * (float)num / 64f, 0.3125f));
		if (GUI.Button(r, "", GUIManager.gs_empty) && !this.buylock && PlayerProfile.money >= num3 && !flag)
		{
			base.StartCoroutine(this.set_upgrade(Shop.THIS.currItem.ItemID, id));
			base.StartCoroutine(this.set_timeout());
		}
		return result;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x0000C8E4 File Offset: 0x0000AAE4
	private IEnumerator set_upgrade(int wid, int upid)
	{
		this.buylock = true;
		string url = string.Concat(new object[]
		{
			CONST.HANDLER_SERVER,
			"21&id=",
			PlayerProfile.id.ToString(),
			"&key=",
			PlayerProfile.authkey,
			"&session=",
			PlayerProfile.session,
			"&upid=",
			upid.ToString(),
			"&itemid=",
			wid.ToString(),
			"&time=",
			DateTime.Now.Second
		});
		WWW www = new WWW(url);
		yield return www;
		this.buylock = false;
		if (www.error == null)
		{
			string[] array = www.text.Split(new char[]
			{
				'|'
			});
			if (array.Length != 5)
			{
				yield break;
			}
			int num;
			if (!int.TryParse(array[1], out num))
			{
				yield break;
			}
			int num2;
			if (!int.TryParse(array[2], out num2))
			{
				yield break;
			}
			int num3;
			if (!int.TryParse(array[3], out num3))
			{
				yield break;
			}
			int num4;
			if (!int.TryParse(array[4], out num4))
			{
				yield break;
			}
			if (array[0] == "OK")
			{
				if (ItemsDB.CheckItem(num2))
				{
					ItemsDB.Items[num2].MyUpgrades[num3] = num4;
				}
				PlayerProfile.money -= num;
			}
		}
		yield break;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000C901 File Offset: 0x0000AB01
	private IEnumerator set_timeout()
	{
		yield return new WaitForSeconds(5f);
		this.buylock = false;
		yield break;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000C910 File Offset: 0x0000AB10
	private void DrawCategory0()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 192f, 598f, GUIManager.YRES(768f) - 199f - 192f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.PISTOLS);
		this.DrawCategory(ITEMS_CATEGORY.PP);
		this.DrawCategory(ITEMS_CATEGORY.AUTOMATS);
		this.DrawCategory(ITEMS_CATEGORY.MACHINEGUNS);
		this.DrawCategory(ITEMS_CATEGORY.SNIPERS);
		this.DrawCategory(ITEMS_CATEGORY.SHOTGUNS);
		this.DrawCategory(ITEMS_CATEGORY.MELEE);
		this.DrawCategory(ITEMS_CATEGORY.REST);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000C9C4 File Offset: 0x0000ABC4
	private void DrawCategory1()
	{
		this.scrollViewVector = GUIManager.BeginScrollView(new Rect(0f, 192f, 598f, GUIManager.YRES(768f) - 199f - 192f), this.scrollViewVector, new Rect(0f, 0f, 0f, this.sh));
		this.ResetPos();
		this.DrawCategory(ITEMS_CATEGORY.TANKS);
		this.DrawCategory(ITEMS_CATEGORY.CARS);
		GUIManager.EndScrollView();
		this.sh = (float)(this.y_pos + 8);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000CA50 File Offset: 0x0000AC50
	private void NextPos(bool end = false)
	{
		if (end)
		{
			this.x_pos = 184;
			if (this.icount > 0)
			{
				this.y_pos += 68;
			}
			return;
		}
		this.x_pos += 132;
		this.icount++;
		if (this.icount == 3)
		{
			this.icount = 0;
			this.x_pos = 184;
			this.y_pos += 68;
		}
	}

	// Token: 0x0400009A RID: 154
	public bool Active;

	// Token: 0x0400009B RID: 155
	private int type;

	// Token: 0x0400009C RID: 156
	private bool[] hovermode = new bool[6];

	// Token: 0x0400009D RID: 157
	private float lastupdate = -5f;

	// Token: 0x0400009E RID: 158
	private int x_pos;

	// Token: 0x0400009F RID: 159
	private int y_pos;

	// Token: 0x040000A0 RID: 160
	private int icount;

	// Token: 0x040000A1 RID: 161
	private float sh;

	// Token: 0x040000A2 RID: 162
	private float shv;

	// Token: 0x040000A3 RID: 163
	private int y_cat_ofs;

	// Token: 0x040000A4 RID: 164
	private bool buylock = true;

	// Token: 0x040000A5 RID: 165
	private Vector2 scrollViewVector = Vector2.zero;

	// Token: 0x040000A6 RID: 166
	private Vector2 scrollViewVector2 = Vector2.zero;
}
