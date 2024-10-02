using System;
using UnityEngine;

// Token: 0x0200007E RID: 126
public class DeathMsg : MonoBehaviour
{
	// Token: 0x0600037E RID: 894 RVA: 0x0003C92C File Offset: 0x0003AB2C
	private void Awake()
	{
		this.Map = GameObject.Find("Map");
		this.lastupdate = Time.time;
		for (int i = 0; i < 4; i++)
		{
			this.d_attacker[i] = "";
			this.d_victim[i] = "";
		}
		this.gui_style2 = new GUIStyle();
		this.gui_style2.font = FontManager.font[2];
		this.gui_style2.fontSize = 26;
		this.gui_style2.normal.textColor = new Color(1f, 1f, 1f, 1f);
		this.hudmsg = ContentLoader.LoadTexture("hudmsg");
		this.weapon_ntw20 = ContentLoader.LoadTexture("weapon_ntw20");
		this.weapon_vss_desert = ContentLoader.LoadTexture("weapon_vss_desert");
		this.weapon_tank_light = ContentLoader.LoadTexture("weapon_tank_light");
		this.weapon_tank_medium = ContentLoader.LoadTexture("weapon_tank_medium");
		this.weapon_tank_heavy = ContentLoader.LoadTexture("weapon_tank_heavy");
		this.weapon_minigun = ContentLoader.LoadTexture("weapon_minigun");
		this.weapon_minefly = ContentLoader.LoadTexture("weapon_minefly");
		this.weapon_javelin = ContentLoader.LoadTexture("weapon_javelin");
		this.weapon_zaa12 = ContentLoader.LoadTexture("weapon_zaa12");
		this.weapon_zasval = ContentLoader.LoadTexture("weapon_zasval");
		this.weapon_zfn57 = ContentLoader.LoadTexture("weapon_zfn57");
		this.weapon_zkord = ContentLoader.LoadTexture("weapon_zkord");
		this.weapon_zm249 = ContentLoader.LoadTexture("weapon_zm249");
		this.weapon_zminigun = ContentLoader.LoadTexture("weapon_zminigun");
		this.weapon_zsps12 = ContentLoader.LoadTexture("weapon_zsps12");
		this.weapon_mk3 = ContentLoader.LoadTexture("weapon_mk3");
		this.weapon_rkg3 = ContentLoader.LoadTexture("weapon_rkg3");
		this.weapon_tube = ContentLoader.LoadTexture("weapon_tube");
		this.weapon_bulava = ContentLoader.LoadTexture("weapon_bulava");
		this.weapon_katana = ContentLoader.LoadTexture("weapon_katana");
		this.weapon_crossbow = ContentLoader.LoadTexture("weapon_crossbow");
		this.weapon_mauzer = ContentLoader.LoadTexture("weapon_mauzer");
		this.weapon_qbz95 = ContentLoader.LoadTexture("weapon_qbz95");
		this.weapon_mine = ContentLoader.LoadTexture("weapon_mine");
		this.weapon_c4 = ContentLoader.LoadTexture("weapon_c4");
		this.weapon_chopper = ContentLoader.LoadTexture("weapon_chopper");
		this.weapon_shield = ContentLoader.LoadTexture("weapon_shield");
		this.weapon_aksu = ContentLoader.LoadTexture("weapon_aksu");
		this.weapon_m700 = ContentLoader.LoadTexture("weapon_m700");
		this.weapon_stechkin = ContentLoader.LoadTexture("weapon_stechkin");
		this.weapon_at_mine = ContentLoader.LoadTexture("TankMine_kills");
		this.weapon_molotov = ContentLoader.LoadTexture("molotov_kills");
		this.weapon_m202 = ContentLoader.LoadTexture("m202_kills");
		this.weapon_gg = ContentLoader.LoadTexture("gas_gren_kill");
		this.weapon_dpm = ContentLoader.LoadTexture("dpmg_kills");
		this.weapon_m1924 = ContentLoader.LoadTexture("mac1924_kills");
		this.weapon_mg42 = ContentLoader.LoadTexture("mg42_kills");
		this.weapon_sten = ContentLoader.LoadTexture("stenmk2_kills");
		this.weapon_m1a1 = ContentLoader.LoadTexture("m1a1_kills");
		this.weapon_type99 = ContentLoader.LoadTexture("type99_kills");
		this.weapon_jeep = ContentLoader.LoadTexture("humvee");
		this.weapon_bizon = ContentLoader.LoadTexture("weapon_bizon");
		this.weapon_pila = ContentLoader.LoadTexture("weapon_pila");
		this.weapon_groza = ContentLoader.LoadTexture("weapon_groza");
		this.weapon_jackhammer = ContentLoader.LoadTexture("weapon_jackhammer");
		this.weapon_tykva = ContentLoader.LoadTexture("weapon_tykva");
		this.weapon_psg_1 = ContentLoader.LoadTexture("weapon_psg_1");
		this.weapon_krytac = ContentLoader.LoadTexture("weapon_krytac");
		this.weapon_mp5sd = ContentLoader.LoadTexture("weapon_mp5sd");
		this.weapon_colts = ContentLoader.LoadTexture("weapon_colts");
		this.weapon_jackhammer_lady = ContentLoader.LoadTexture("weapon_jackhammer_lady");
		this.weapon_m700_lady = ContentLoader.LoadTexture("weapon_m700kitty");
		this.weapon_mg42_lady = ContentLoader.LoadTexture("weapon_mg42_lady");
		this.weapon_magnum_lady = ContentLoader.LoadTexture("weapon_magnum_lady");
		this.weapon_scorpion = ContentLoader.LoadTexture("weapon_scorpion");
		this.weapon_g36c_veteran = ContentLoader.LoadTexture("weapon_g36c_veteran");
		this.weapon_snowball = ContentLoader.LoadTexture("weapon_snowball");
		this.weapon_fmg9 = ContentLoader.LoadTexture("weapon_fmg9");
		this.weapon_saiga = ContentLoader.LoadTexture("weapon_saiga");
		this.weapon_flamethrower = ContentLoader.LoadTexture("weapon_flamethrower");
		this.weapon_ak47_snow = ContentLoader.LoadTexture("weapon_ak47_snow");
		this.weapon_p90_snow = ContentLoader.LoadTexture("weapon_p90_snow");
		this.weapon_saiga_snow = ContentLoader.LoadTexture("weapon_saiga_snow");
		this.weapon_sr25 = ContentLoader.LoadTexture("weapon_sr25");
		this.weapon_sr25_snow = ContentLoader.LoadTexture("weapon_sr25_snow");
		this.weapon_usp_snow = ContentLoader.LoadTexture("weapon_usp_snow");
		this.weapon_kar98k = ContentLoader.LoadTexture("weapon_kar98k");
		this.weapon_mg13 = ContentLoader.LoadTexture("weapon_mg13");
		this.weapon_rpd = ContentLoader.LoadTexture("weapon_rpd");
		this.weapon_tt = ContentLoader.LoadTexture("weapon_tt");
		this.weapon_handgrenade = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_L96A1GOLD = ContentLoader.LoadTexture("weapon_l96a1_gold");
		this.weapon_SVT40 = ContentLoader.LoadTexture("weapon_svt");
		this.weapon_SVUAS = ContentLoader.LoadTexture("weapon_svu");
		this.weapon_PSG1GOLD = ContentLoader.LoadTexture("weapon_psg1_gold");
		this.weapon_M110 = ContentLoader.LoadTexture("weapon_m110");
		this.weapon_KEDR = ContentLoader.LoadTexture("weapon_pp91");
		this.weapon_MAC10GOLD = ContentLoader.LoadTexture("weapon_mac10_gold");
		this.weapon_KASHTAN = ContentLoader.LoadTexture("weapon_aek919");
		this.weapon_FMG9GOLD = ContentLoader.LoadTexture("weapon_fmg9_gold");
		this.weapon_MP7 = ContentLoader.LoadTexture("weapon_mp7");
		this.weapon_PKPGOLD = ContentLoader.LoadTexture("weapon_pkp_gold");
		this.weapon_NEGEV = ContentLoader.LoadTexture("weapon_negev");
		this.weapon_XM8 = ContentLoader.LoadTexture("weapon_xm8");
		this.weapon_AK74 = ContentLoader.LoadTexture("weapon_ak74");
		this.weapon_AK47GOLD = ContentLoader.LoadTexture("weapon_ak47_gold");
		this.weapon_M4A1GOLD = ContentLoader.LoadTexture("weapon_m4a1_gold");
		this.weapon_ABAKAN = ContentLoader.LoadTexture("weapon_ah94");
		this.weapon_AK103 = ContentLoader.LoadTexture("weapon_ak103");
		this.weapon_BEKAS12M = ContentLoader.LoadTexture("weapon_bekas");
		this.weapon_MOSSBERG500 = ContentLoader.LoadTexture("weapon_ms500");
		this.weapon_NOVAGOLD = ContentLoader.LoadTexture("weapon_nova_gold");
		this.weapon_NEOSTEAD2000 = ContentLoader.LoadTexture("weapon_ns2000");
		this.weapon_HONCHO = ContentLoader.LoadTexture("weapon_honcho");
		this.weapon_M2 = ContentLoader.LoadTexture("weapon_m2");
		this.weapon_M4 = ContentLoader.LoadTexture("weapon_m4");
		this.weapon_SAIGAGOLD = ContentLoader.LoadTexture("weapon_saiga_gold");
		this.weapon_BROWNING = ContentLoader.LoadTexture("weapon_hp");
		this.weapon_WALTHER = ContentLoader.LoadTexture("weapon_p99");
		this.weapon_CZ75 = ContentLoader.LoadTexture("weapon_cz75");
		this.weapon_SWM29 = ContentLoader.LoadTexture("weapon_m29");
		this.weapon_UZI = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_M1887 = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_REMINGTON870 = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_VEPR12 = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_P226 = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_QBU88 = ContentLoader.LoadTexture("weapon_handgranade");
		this.weapon_DEAGLEGOLD = ContentLoader.LoadTexture("weapon_deagle_gold");
		this.weapon_KUKRI = ContentLoader.LoadTexture("weapon_kukri");
		this.weapon_FALSHION = ContentLoader.LoadTexture("weapon_falshion");
	}

	// Token: 0x0600037F RID: 895 RVA: 0x0003D0C0 File Offset: 0x0003B2C0
	private void OnGUI()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!(this.d_attacker[i] == "") && !(this.d_victim[i] == ""))
			{
				float num = GUIManager.YRES(132f) + 8f;
				if (this.d_attacker[i] != "^8WORLD")
				{
					num += GUIManager.DrawColorText(num, (float)(10 + i * 20), this.d_attacker[i], TextAnchor.MiddleLeft);
				}
				num += 4f;
				ITEM item = (ITEM)this.d_weaponid[i];
				switch (item)
				{
				case ITEM.AK47:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 55f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.8671875f, 0.21484375f, 0.0625f));
					num += 55f;
					break;
				case ITEM.SVD:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 63f, 16.5f), this.hudmsg, new Rect(0.22265625f, 0.8671875f, 0.24609375f, 0.0625f));
					num += 63f;
					break;
				case ITEM.SKIN_SOLDIER:
				case ITEM.SKIN_GHOST:
				case ITEM.PREMIUM:
				case ITEM.SKIN_SAS:
				case ITEM.SKIN_TERROR:
				case ITEM.SKIN_INSURGENT:
				case ITEM.SKIN_PILOT:
				case ITEM.SKIN_REBEL:
				case ITEM.SKIN_RIDER:
				case ITEM.SKIN_SNIPER:
				case ITEM.SKIN_SPEC:
				case ITEM.SKIN_STALKER:
				case ITEM.SKIN_USMC:
				case ITEM.SKIN_DESANT:
				case ITEM.SKIN_CASPER:
				case ITEM.SKIN_JACK:
				case ITEM.SKIN_SLENDER:
				case ITEM.SKIN_DEFAULT:
				case ITEM.MEDKIT_S:
				case ITEM.MEDKIT_M:
				case ITEM.MEDKIT_L:
				case ITEM.SKIN_ARCTIC:
				case ITEM.SKIN_1337:
				case ITEM.VEST:
				case ITEM.SKIN_ELF:
				case ITEM.SKIN_SNOWMAN:
				case ITEM.SKIN_SANTAGIRL:
				case ITEM.SKIN_SANTA:
				case (ITEM)63:
				case ITEM.SKIN_BOMBER:
				case ITEM.SKIN_SURVIVOR:
				case ITEM.SKIN_BANDIT:
				case ITEM.SKIN_MERCENARY:
				case ITEM.SKIN_PRISONER:
				case ITEM.SKIN_MERCGIRL:
				case ITEM.MODE_CONTRA:
				case ITEM.SKIN_KILLER:
				case ITEM.SKIN_COP:
				case ITEM.SKIN_BLOKADOVEC:
				case ITEM.SKIN_SOVIET:
				case ITEM.SKIN_GERMAN:
				case ITEM.SKIN_SF1000:
				case ITEM.SKIN_SF1207:
				case ITEM.SKIN_SF1122:
				case ITEM.SKIN_FREDDY:
				case ITEM.SKIN_JASON:
				case ITEM.SKIN_RORSCHACH:
				case ITEM.SKIN_ZBOY:
				case ITEM.SKIN_ZGIRL:
				case ITEM.SKIN_LT_DESERT:
				case ITEM.SKIN_LT_HEXAGON:
				case ITEM.SKIN_LT_MULTICAM:
				case ITEM.SKIN_LT_TIGER:
				case ITEM.SKIN_LT_WOOD:
				case ITEM.SKIN_MT_DESERT:
				case ITEM.SKIN_MT_HEXAGON:
				case ITEM.SKIN_MT_MULTICAM:
				case ITEM.SKIN_MT_TIGER:
				case ITEM.SKIN_MT_WOOD:
				case ITEM.SKIN_HT_DESERT:
				case ITEM.SKIN_HT_HEXAGON:
				case ITEM.SKIN_HT_MULTICAM:
				case ITEM.SKIN_HT_TIGER:
				case ITEM.SKIN_HT_WOOD:
				case ITEM.ZBK18M:
				case ITEM.ZOF26:
				case ITEM.VEHICLE_REPAIR_KIT:
				case ITEM.TANK_MG:
				case ITEM.HELMETPLUS:
				case ITEM.VESTPLUS:
				case ITEM.SKIN_BELSNICKEL:
				case ITEM.SKIN_RABBIT_BOY:
				case ITEM.SKIN_RABBIT_GIRL:
				case ITEM.SKIN_LT_WINTER:
				case ITEM.SKIN_MT_WINTER:
				case ITEM.SKIN_HT_WINTER:
				case ITEM.SKIN_ANARCHIST:
				case ITEM.SKIN_REBELTERROR:
				case ITEM.SKIN_MARINE:
				case ITEM.SKIN_VMF:
				case ITEM.SKIN_VVS:
				case ITEM.M18:
				case ITEM.SKIN_APRIL:
				case ITEM.SKIN_MULATKA:
				case ITEM.SKIN_UBIVASHKA:
				case ITEM.SKIN_TERRORISTKA:
				case ITEM.SKIN_TOXIK:
				case ITEM.WEAPONSMEGAPACK:
				case ITEM.SKIN_BRITANEC:
				case ITEM.SKIN_FRENCH:
				case ITEM.SKIN_JAPONIA:
				case ITEM.SKIN_US:
				case ITEM.ARMORED_BLOCK:
				case (ITEM)199:
				case (ITEM)200:
				case ITEM.TANK_LIGHT:
				case ITEM.TANK_MEDIUM:
				case ITEM.TANK_HEAVY:
				case ITEM.JEEP:
				case ITEM.MODULE_ANTI_MISSLE:
				case ITEM.MODULE_SMOKE:
				case ITEM.TYKVA:
				case (ITEM)212:
				case ITEM.SKIN_DRACULA:
				case ITEM.SKIN_MUMMY:
				case ITEM.SKIN_WEREWOLF:
				case ITEM.SKIN_ALICE:
				case (ITEM)217:
					break;
				case ITEM.M61:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 13f, 16.5f), this.hudmsg, new Rect(0.47265625f, 0.8671875f, 0.05078125f, 0.0625f));
					num += 13f;
					break;
				case ITEM.DEAGLE:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 20f, 16.5f), this.hudmsg, new Rect(0.66796875f, 0.8671875f, 0.078125f, 0.0625f));
					num += 20f;
					break;
				case ITEM.SHMEL:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 58f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.80078125f, 0.2265625f, 0.0625f));
					num += 58f;
					break;
				case ITEM.ASVAL:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 56f, 16.5f), this.hudmsg, new Rect(0.234375f, 0.80078125f, 0.21875f, 0.0625f));
					num += 56f;
					break;
				case ITEM.G36C:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 42f, 16.5f), this.hudmsg, new Rect(0.45703125f, 0.80078125f, 0.1640625f, 0.0625f));
					num += 42f;
					break;
				case ITEM.KRISS:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 41f, 16.5f), this.hudmsg, new Rect(0.62109375f, 0.80078125f, 0.16015625f, 0.0625f));
					num += 41f;
					break;
				case ITEM.M4A1:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 57f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.734375f, 0.22265625f, 0.0625f));
					num += 57f;
					break;
				case ITEM.M249:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 52f, 16.5f), this.hudmsg, new Rect(0.46875f, 0.734375f, 0.203125f, 0.0625f));
					num += 52f;
					break;
				case ITEM.SPAS12:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 51f, 16.5f), this.hudmsg, new Rect(0.75f, 0.8671875f, 0.19921875f, 0.0625f));
					num += 51f;
					break;
				case ITEM.VINTOREZ:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 60f, 16.5f), this.hudmsg, new Rect(0.23046875f, 0.734375f, 0.234375f, 0.0625f));
					num += 60f;
					break;
				case ITEM.VSK94:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 52f, 16.5f), this.hudmsg, new Rect(0.7890625f, 0.80078125f, 0.203125f, 0.0625f));
					num += 52f;
					break;
				case ITEM.SHOVEL:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 37f, 16.5f), this.hudmsg, new Rect(0.1640625f, 0.93359375f, 0.14453125f, 0.0625f));
					num += 37f;
					break;
				case ITEM.L96A1MOD:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 75f, 16.5f), this.hudmsg, new Rect(0.140625f, 0.3359375f, 0.29296875f, 0.0625f));
					num += 75f;
					break;
				case ITEM.ZOMBIE:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 16f, 16.5f), this.hudmsg, new Rect(0.5234375f, 0.8671875f, 0.0625f, 0.0625f));
					num += 16f;
					break;
				case ITEM.KAR98K:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_kar98k.width / this.weapon_kar98k.height), 16f), this.weapon_kar98k);
					num += (float)(16 * this.weapon_kar98k.width / this.weapon_kar98k.height);
					break;
				case ITEM.USP:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 31f, 16.5f), this.hudmsg, new Rect(0.67578125f, 0.734375f, 0.12109375f, 0.0625f));
					num += 31f;
					break;
				case ITEM.M3:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 60f, 16.5f), this.hudmsg, new Rect(0.3125f, 0.93359375f, 0.234375f, 0.0625f));
					num += 60f;
					break;
				case ITEM.M14:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 63f, 16.5f), this.hudmsg, new Rect(0.55078125f, 0.93359375f, 0.24609375f, 0.0625f));
					num += 63f;
					break;
				case ITEM.MP5:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 33f, 16.5f), this.hudmsg, new Rect(0.80078125f, 0.93359375f, 0.12890625f, 0.0625f));
					num += 33f;
					break;
				case ITEM.GLOCK:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 18f, 16.5f), this.hudmsg, new Rect(0.59375f, 0.8671875f, 0.0703125f, 0.0625f));
					num += 18f;
					break;
				case ITEM.BARRETT:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 64f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.66796875f, 0.25f, 0.0625f));
					num += 64f;
					break;
				case ITEM.TMP:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 24f, 16.5f), this.hudmsg, new Rect(0.80078125f, 0.734375f, 0.09375f, 0.0625f));
					num += 24f;
					break;
				case ITEM.KNIFE:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 19f, 16.5f), this.hudmsg, new Rect(0.7890625f, 0.66796875f, 0.07421875f, 0.0625f));
					num += 19f;
					break;
				case ITEM.AXE:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 31f, 16.5f), this.hudmsg, new Rect(0.3828125f, 0.66796875f, 0.12109375f, 0.0625f));
					num += 31f;
					break;
				case ITEM.BAT:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 34f, 16.5f), this.hudmsg, new Rect(0.65234375f, 0.66796875f, 0.1328125f, 0.0625f));
					num += 34f;
					break;
				case ITEM.CROWBAR:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 36f, 16.5f), this.hudmsg, new Rect(0.5078125f, 0.66796875f, 0.140625f, 0.0625f));
					num += 36f;
					break;
				case ITEM.CARAMEL:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 31f, 16.5f), this.hudmsg, new Rect(0.2578125f, 0.66796875f, 0.12109375f, 0.0625f));
					num += 31f;
					break;
				case ITEM.TNT:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 16f, 16.5f), this.hudmsg, new Rect(0.8671875f, 0.66796875f, 0.0625f, 0.0625f));
					num += 16f;
					break;
				case ITEM.AUGA3:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 44f, 16.5f), this.hudmsg, new Rect(0.203125f, 0.6015625f, 0.171875f, 0.0625f));
					num += 44f;
					break;
				case ITEM.SG552:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 50f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.6015625f, 0.1953125f, 0.0625f));
					num += 50f;
					break;
				case ITEM.MORTAR:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_minefly.width / this.weapon_minefly.height), 16f), this.weapon_minefly);
					num += (float)(16 * this.weapon_minefly.width / this.weapon_minefly.height);
					break;
				case ITEM.M14EBR:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 63f, 16.5f), this.hudmsg, new Rect(0.5390625f, 0.6015625f, 0.24609375f, 0.0625f));
					num += 63f;
					break;
				case ITEM.L96A1:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 62f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.53515625f, 0.2421875f, 0.0625f));
					num += 62f;
					break;
				case ITEM.NOVA:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 62f, 16.5f), this.hudmsg, new Rect(0.64453125f, 0.53515625f, 0.2421875f, 0.0625f));
					num += 62f;
					break;
				case ITEM.KORD:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 55f, 16.5f), this.hudmsg, new Rect(0.42578125f, 0.53515625f, 0.21484375f, 0.0625f));
					num += 55f;
					break;
				case ITEM.ANACONDA:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 27f, 16.5f), this.hudmsg, new Rect(0.890625f, 0.53515625f, 0.10546875f, 0.0625f));
					num += 27f;
					break;
				case ITEM.SCAR_H:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 60f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.46875f, 0.234375f, 0.0625f));
					num += 60f;
					break;
				case ITEM.P90:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 44f, 16.5f), this.hudmsg, new Rect(0.25f, 0.53515625f, 0.171875f, 0.0625f));
					num += 44f;
					break;
				case ITEM.GP:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 16f, 16.5f), this.hudmsg, new Rect(0.38671875f, 0.40234375f, 0.0625f, 0.0625f));
					num += 16f;
					break;
				case ITEM.RPK:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 64f, 16.5f), this.hudmsg, new Rect(0.2421875f, 0.46875f, 0.25f, 0.0625f));
					num += 64f;
					break;
				case ITEM.HK416:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 47f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.40234375f, 0.18359375f, 0.0625f));
					num += 47f;
					break;
				case ITEM.AK102:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 53f, 16.5f), this.hudmsg, new Rect(0.75f, 0.46875f, 0.20703125f, 0.0625f));
					num += 53f;
					break;
				case ITEM.SR25:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_sr25.width / this.weapon_sr25.height), 16f), this.weapon_sr25);
					num += (float)(16 * this.weapon_sr25.width / this.weapon_sr25.height);
					break;
				case ITEM.MGLMK1:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 49f, 16.5f), this.hudmsg, new Rect(0.19140625f, 0.40234375f, 0.19140625f, 0.0625f));
					num += 49f;
					break;
				case ITEM.MOSIN:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 63f, 16.5f), this.hudmsg, new Rect(0.453125f, 0.40234375f, 0.24609375f, 0.0625f));
					num += 63f;
					break;
				case ITEM.PPSH:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 64f, 16.5f), this.hudmsg, new Rect(0.703125f, 0.40234375f, 0.25f, 0.0625f));
					num += 64f;
					break;
				case ITEM.MP40:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 34f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.3359375f, 0.1328125f, 0.0625f));
					num += 34f;
					break;
				case ITEM.TT:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_tt.width / this.weapon_tt.height), 16f), this.weapon_tt);
					num += (float)(16 * this.weapon_tt.width / this.weapon_tt.height);
					break;
				case ITEM.KACPDW:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 45f, 16.5f), this.hudmsg, new Rect(0.4375f, 0.3359375f, 0.17578125f, 0.0625f));
					num += 45f;
					break;
				case ITEM.FAMAS:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 53f, 16.5f), this.hudmsg, new Rect(0.6171875f, 0.3359375f, 0.20703125f, 0.0625f));
					num += 53f;
					break;
				case ITEM.BERETTA:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 18f, 16.5f), this.hudmsg, new Rect(0.828125f, 0.3359375f, 0.0703125f, 0.0625f));
					num += 18f;
					break;
				case ITEM.MACHETE:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 30f, 16.5f), this.hudmsg, new Rect(0.7890625f, 0.6015625f, 0.1171875f, 0.0625f));
					num += 30f;
					break;
				case ITEM.RPG:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 62f, 16.5f), this.hudmsg, new Rect(0.1171875f, 0.26953125f, 0.2421875f, 0.0625f));
					num += 62f;
					break;
				case ITEM.WRENCH:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 33f, 16.5f), this.hudmsg, new Rect(0.36328125f, 0.26953125f, 0.12890625f, 0.0625f));
					num += 33f;
					break;
				case ITEM.AA12:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 62f, 20.5f), this.hudmsg, new Rect(0.5f, 0.25f, 0.2421875f, 0.078125f));
					num += 62f;
					break;
				case ITEM.FN57:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 30f, 16.5f), this.hudmsg, new Rect(0.7421875f, 0.26171875f, 0.1171875f, 0.0625f));
					num += 30f;
					break;
				case ITEM.FS2000:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 48f, 24.5f), this.hudmsg, new Rect(0.00390625f, 0.1640625f, 0.1875f, 0.09375f));
					num += 48f;
					break;
				case ITEM.L85:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 56f, 16.5f), this.hudmsg, new Rect(0.19140625f, 0.171875f, 0.21875f, 0.08203125f));
					num += 56f;
					break;
				case ITEM.MAC10:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 22f, 21.5f), this.hudmsg, new Rect(0.859375f, 0.26953125f, 0.0859375f, 0.0625f));
					num += 22f;
					break;
				case ITEM.PKP:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 62f, 16.5f), this.hudmsg, new Rect(0.4140625f, 0.171875f, 0.2421875f, 0.0625f));
					num += 62f;
					break;
				case ITEM.PM:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 16f, 16.5f), this.hudmsg, new Rect(0.66015625f, 0.171875f, 0.06640625f, 0.0625f));
					num += 16f;
					break;
				case ITEM.TAR21:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 45f, 20.5f), this.hudmsg, new Rect(0.73046875f, 0.16796875f, 0.17578125f, 0.078125f));
					num += 45f;
					break;
				case ITEM.UMP45:
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 47f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.1015625f, 0.18359375f, 0.0625f));
					num += 47f;
					break;
				case ITEM.NTW20:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(18 * this.weapon_ntw20.width / this.weapon_ntw20.height), 18f), this.weapon_ntw20);
					num += (float)(18 * this.weapon_ntw20.width / this.weapon_ntw20.height);
					break;
				case ITEM.VINTOREZ_DESERT:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_vss_desert.width / this.weapon_vss_desert.height), 16f), this.weapon_vss_desert);
					num += (float)(16 * this.weapon_vss_desert.width / this.weapon_vss_desert.height);
					break;
				case ITEM.MINIGUN:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_minigun.width / this.weapon_minigun.height), 16f), this.weapon_minigun);
					num += (float)(16 * this.weapon_minigun.width / this.weapon_minigun.height);
					break;
				case ITEM.JAVELIN:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_javelin.width / this.weapon_javelin.height), 16f), this.weapon_javelin);
					num += (float)(16 * this.weapon_javelin.width / this.weapon_javelin.height);
					break;
				case ITEM.ZAA12:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zaa12.width / this.weapon_zaa12.height), 16f), this.weapon_zaa12);
					num += (float)(16 * this.weapon_zaa12.width / this.weapon_zaa12.height);
					break;
				case ITEM.ZASVAL:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zasval.width / this.weapon_zasval.height), 16f), this.weapon_zasval);
					num += (float)(16 * this.weapon_zasval.width / this.weapon_zasval.height);
					break;
				case ITEM.ZFN57:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zfn57.width / this.weapon_zfn57.height), 16f), this.weapon_zfn57);
					num += (float)(16 * this.weapon_zfn57.width / this.weapon_zfn57.height);
					break;
				case ITEM.ZKORD:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zkord.width / this.weapon_zkord.height), 16f), this.weapon_zkord);
					num += (float)(16 * this.weapon_zkord.width / this.weapon_zkord.height);
					break;
				case ITEM.ZM249:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zm249.width / this.weapon_zm249.height), 16f), this.weapon_zm249);
					num += (float)(16 * this.weapon_zm249.width / this.weapon_zm249.height);
					break;
				case ITEM.ZMINIGUN:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zminigun.width / this.weapon_zminigun.height), 16f), this.weapon_zminigun);
					num += (float)(16 * this.weapon_zminigun.width / this.weapon_zminigun.height);
					break;
				case ITEM.ZSPAS12:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_zsps12.width / this.weapon_zsps12.height), 16f), this.weapon_zsps12);
					num += (float)(16 * this.weapon_zsps12.width / this.weapon_zsps12.height);
					break;
				case ITEM.MG13:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mg13.width / this.weapon_mg13.height), 16f), this.weapon_mg13);
					num += (float)(16 * this.weapon_mg13.width / this.weapon_mg13.height);
					break;
				case ITEM.RPD:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_rpd.width / this.weapon_rpd.height), 16f), this.weapon_rpd);
					num += (float)(16 * this.weapon_rpd.width / this.weapon_rpd.height);
					break;
				case ITEM.STIELHANDGRANATE:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_handgrenade.width / this.weapon_handgrenade.height), 16f), this.weapon_handgrenade);
					num += (float)(16 * this.weapon_handgrenade.width / this.weapon_handgrenade.height);
					break;
				case ITEM.TUBE:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_tube.width / this.weapon_tube.height), 16f), this.weapon_tube);
					num += (float)(16 * this.weapon_tube.width / this.weapon_tube.height);
					break;
				case ITEM.BULAVA:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_bulava.width / this.weapon_bulava.height), 16f), this.weapon_bulava);
					num += (float)(16 * this.weapon_bulava.width / this.weapon_bulava.height);
					break;
				case ITEM.KATANA:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_katana.width / this.weapon_katana.height), 16f), this.weapon_katana);
					num += (float)(16 * this.weapon_katana.width / this.weapon_katana.height);
					break;
				case ITEM.MAUZER:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mauzer.width / this.weapon_mauzer.height), 16f), this.weapon_mauzer);
					num += (float)(16 * this.weapon_mauzer.width / this.weapon_mauzer.height);
					break;
				case ITEM.CROSSBOW:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_crossbow.width / this.weapon_crossbow.height), 16f), this.weapon_crossbow);
					num += (float)(16 * this.weapon_crossbow.width / this.weapon_crossbow.height);
					break;
				case ITEM.QBZ95:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_qbz95.width / this.weapon_qbz95.height), 16f), this.weapon_qbz95);
					num += (float)(16 * this.weapon_qbz95.width / this.weapon_qbz95.height);
					break;
				case ITEM.MK3:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mk3.width / this.weapon_mk3.height), 16f), this.weapon_mk3);
					num += (float)(16 * this.weapon_mk3.width / this.weapon_mk3.height);
					break;
				case ITEM.RKG3:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_rkg3.width / this.weapon_rkg3.height), 16f), this.weapon_rkg3);
					num += (float)(16 * this.weapon_rkg3.width / this.weapon_rkg3.height);
					break;
				case ITEM.MINE:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mine.width / this.weapon_mine.height), 16f), this.weapon_mine);
					num += (float)(16 * this.weapon_mine.width / this.weapon_mine.height);
					break;
				case ITEM.C4:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_c4.width / this.weapon_c4.height), 16f), this.weapon_c4);
					num += (float)(16 * this.weapon_c4.width / this.weapon_c4.height);
					break;
				case ITEM.CHOPPER:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_chopper.width / this.weapon_chopper.height), 16f), this.weapon_chopper);
					num += (float)(16 * this.weapon_chopper.width / this.weapon_chopper.height);
					break;
				case ITEM.SHIELD:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_shield.width / this.weapon_shield.height), 16f), this.weapon_shield);
					num += (float)(16 * this.weapon_shield.width / this.weapon_shield.height);
					break;
				case ITEM.AKSU:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_aksu.width / this.weapon_aksu.height), 16f), this.weapon_aksu);
					num += (float)(16 * this.weapon_aksu.width / this.weapon_aksu.height);
					break;
				case ITEM.M700:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_m700.width / this.weapon_m700.height), 16f), this.weapon_m700);
					num += (float)(16 * this.weapon_m700.width / this.weapon_m700.height);
					break;
				case ITEM.STECHKIN:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_stechkin.width / this.weapon_stechkin.height), 16f), this.weapon_stechkin);
					num += (float)(16 * this.weapon_stechkin.width / this.weapon_stechkin.height);
					break;
				case ITEM.AT_MINE:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_at_mine.width / this.weapon_at_mine.height), 16f), this.weapon_at_mine);
					num += (float)(16 * this.weapon_at_mine.width / this.weapon_at_mine.height);
					break;
				case ITEM.MOLOTOV:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_molotov.width / this.weapon_molotov.height), 16f), this.weapon_molotov);
					num += (float)(16 * this.weapon_molotov.width / this.weapon_molotov.height);
					break;
				case ITEM.M202:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_m202.width / this.weapon_m202.height), 16f), this.weapon_m202);
					num += (float)(16 * this.weapon_m202.width / this.weapon_m202.height);
					break;
				case ITEM.M7A2:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_gg.width / this.weapon_gg.height), 16f), this.weapon_gg);
					num += (float)(16 * this.weapon_gg.width / this.weapon_gg.height);
					break;
				case ITEM.DPM:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_dpm.width / this.weapon_dpm.height), 16f), this.weapon_dpm);
					num += (float)(16 * this.weapon_dpm.width / this.weapon_dpm.height);
					break;
				case ITEM.M1924:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_m1924.width / this.weapon_m1924.height), 16f), this.weapon_m1924);
					num += (float)(16 * this.weapon_m1924.width / this.weapon_m1924.height);
					break;
				case ITEM.MG42:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mg42.width / this.weapon_mg42.height), 16f), this.weapon_mg42);
					num += (float)(16 * this.weapon_mg42.width / this.weapon_mg42.height);
					break;
				case ITEM.STEN_MK2:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_sten.width / this.weapon_sten.height), 16f), this.weapon_sten);
					num += (float)(16 * this.weapon_sten.width / this.weapon_sten.height);
					break;
				case ITEM.M1A1:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_m1a1.width / this.weapon_m1a1.height), 16f), this.weapon_m1a1);
					num += (float)(16 * this.weapon_m1a1.width / this.weapon_m1a1.height);
					break;
				case ITEM.TYPE99:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_type99.width / this.weapon_type99.height), 16f), this.weapon_type99);
					num += (float)(16 * this.weapon_type99.width / this.weapon_type99.height);
					break;
				case ITEM.BIZON:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_bizon.width / this.weapon_bizon.height), 16f), this.weapon_bizon);
					num += (float)(16 * this.weapon_bizon.width / this.weapon_bizon.height);
					break;
				case ITEM.GROZA:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_groza.width / this.weapon_groza.height), 16f), this.weapon_groza);
					num += (float)(16 * this.weapon_groza.width / this.weapon_groza.height);
					break;
				case ITEM.JACKHAMMER:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_jackhammer.width / this.weapon_jackhammer.height), 16f), this.weapon_jackhammer);
					num += (float)(16 * this.weapon_jackhammer.width / this.weapon_jackhammer.height);
					break;
				case ITEM.CHAINSAW:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_pila.width / this.weapon_pila.height), 16f), this.weapon_pila);
					num += (float)(16 * this.weapon_pila.width / this.weapon_pila.height);
					break;
				case ITEM.PSG_1:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_psg_1.width / this.weapon_psg_1.height), 16f), this.weapon_psg_1);
					num += (float)(16 * this.weapon_psg_1.width / this.weapon_psg_1.height);
					break;
				case ITEM.KRYTAC:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_krytac.width / this.weapon_krytac.height), 16f), this.weapon_krytac);
					num += (float)(16 * this.weapon_krytac.width / this.weapon_krytac.height);
					break;
				case ITEM.MP5SD:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mp5sd.width / this.weapon_mp5sd.height), 16f), this.weapon_mp5sd);
					num += (float)(16 * this.weapon_mp5sd.width / this.weapon_mp5sd.height);
					break;
				case ITEM.COLTS:
					GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_colts.width / this.weapon_colts.height), 16f), this.weapon_colts);
					num += (float)(16 * this.weapon_colts.width / this.weapon_colts.height);
					break;
				default:
					if (item != ITEM.DEFAULT_DEATH)
					{
						switch (item)
						{
						case ITEM.JACKHAMMER_LADY:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_jackhammer_lady.width / this.weapon_jackhammer_lady.height), 16f), this.weapon_jackhammer_lady);
							num += (float)(16 * this.weapon_jackhammer_lady.width / this.weapon_jackhammer_lady.height);
							break;
						case ITEM.M700_LADY:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_m700_lady.width / this.weapon_m700_lady.height), 16f), this.weapon_m700_lady);
							num += (float)(16 * this.weapon_m700_lady.width / this.weapon_m700_lady.height);
							break;
						case ITEM.MG42_LADY:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_mg42_lady.width / this.weapon_mg42_lady.height), 16f), this.weapon_mg42_lady);
							num += (float)(16 * this.weapon_mg42_lady.width / this.weapon_mg42_lady.height);
							break;
						case ITEM.MAGNUM_LADY:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_magnum_lady.width / this.weapon_magnum_lady.height), 16f), this.weapon_magnum_lady);
							num += (float)(16 * this.weapon_magnum_lady.width / this.weapon_magnum_lady.height);
							break;
						case ITEM.SCORPION:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_scorpion.width / this.weapon_scorpion.height), 16f), this.weapon_scorpion);
							num += (float)(16 * this.weapon_scorpion.width / this.weapon_scorpion.height);
							break;
						case ITEM.G36C_VETERAN:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_g36c_veteran.width / this.weapon_g36c_veteran.height), 16f), this.weapon_g36c_veteran);
							num += (float)(16 * this.weapon_g36c_veteran.width / this.weapon_g36c_veteran.height);
							break;
						case ITEM.FMG9:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_fmg9.width / this.weapon_fmg9.height), 16f), this.weapon_fmg9);
							num += (float)(16 * this.weapon_fmg9.width / this.weapon_fmg9.height);
							break;
						case ITEM.SAIGA:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_saiga.width / this.weapon_saiga.height), 16f), this.weapon_saiga);
							num += (float)(16 * this.weapon_saiga.width / this.weapon_saiga.height);
							break;
						case ITEM.FLAMETHROWER:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_flamethrower.width / this.weapon_flamethrower.height), 16f), this.weapon_flamethrower);
							num += (float)(16 * this.weapon_flamethrower.width / this.weapon_flamethrower.height);
							break;
						case ITEM.AK47_SNOW:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_ak47_snow.width / this.weapon_ak47_snow.height), 16f), this.weapon_ak47_snow);
							num += (float)(16 * this.weapon_ak47_snow.width / this.weapon_ak47_snow.height);
							break;
						case ITEM.P90_SNOW:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_p90_snow.width / this.weapon_p90_snow.height), 16f), this.weapon_p90_snow);
							num += (float)(16 * this.weapon_p90_snow.width / this.weapon_p90_snow.height);
							break;
						case ITEM.SAIGA_SNOW:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_saiga_snow.width / this.weapon_saiga_snow.height), 16f), this.weapon_saiga_snow);
							num += (float)(16 * this.weapon_saiga_snow.width / this.weapon_saiga_snow.height);
							break;
						case ITEM.SR25_SNOW:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_sr25_snow.width / this.weapon_sr25_snow.height), 16f), this.weapon_sr25_snow);
							num += (float)(16 * this.weapon_sr25_snow.width / this.weapon_sr25_snow.height);
							break;
						case ITEM.USP_SNOW:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_usp_snow.width / this.weapon_usp_snow.height), 16f), this.weapon_usp_snow);
							num += (float)(16 * this.weapon_usp_snow.width / this.weapon_usp_snow.height);
							break;
						case ITEM.L96A1_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_L96A1GOLD.width / this.weapon_L96A1GOLD.height), 16f), this.weapon_L96A1GOLD);
							num += (float)(16 * this.weapon_L96A1GOLD.width / this.weapon_L96A1GOLD.height);
							break;
						case ITEM.SVT40:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_SVT40.width / this.weapon_SVT40.height), 16f), this.weapon_SVT40);
							num += (float)(16 * this.weapon_SVT40.width / this.weapon_SVT40.height);
							break;
						case ITEM.SVUAS:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_SVUAS.width / this.weapon_SVUAS.height), 16f), this.weapon_SVUAS);
							num += (float)(16 * this.weapon_SVUAS.width / this.weapon_SVUAS.height);
							break;
						case ITEM.PSG1_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_PSG1GOLD.width / this.weapon_PSG1GOLD.height), 16f), this.weapon_PSG1GOLD);
							num += (float)(16 * this.weapon_PSG1GOLD.width / this.weapon_PSG1GOLD.height);
							break;
						case ITEM.M110:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_M110.width / this.weapon_M110.height), 16f), this.weapon_M110);
							num += (float)(16 * this.weapon_M110.width / this.weapon_M110.height);
							break;
						case ITEM.KEDR:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_KEDR.width / this.weapon_KEDR.height), 16f), this.weapon_KEDR);
							num += (float)(16 * this.weapon_KEDR.width / this.weapon_KEDR.height);
							break;
						case ITEM.MAC10_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_MAC10GOLD.width / this.weapon_MAC10GOLD.height), 16f), this.weapon_MAC10GOLD);
							num += (float)(16 * this.weapon_MAC10GOLD.width / this.weapon_MAC10GOLD.height);
							break;
						case ITEM.KASHTAN:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_KASHTAN.width / this.weapon_KASHTAN.height), 16f), this.weapon_KASHTAN);
							num += (float)(16 * this.weapon_KASHTAN.width / this.weapon_KASHTAN.height);
							break;
						case ITEM.FMG9_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_FMG9GOLD.width / this.weapon_FMG9GOLD.height), 16f), this.weapon_FMG9GOLD);
							num += (float)(16 * this.weapon_FMG9GOLD.width / this.weapon_FMG9GOLD.height);
							break;
						case ITEM.MP7:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_MP7.width / this.weapon_MP7.height), 16f), this.weapon_MP7);
							num += (float)(16 * this.weapon_MP7.width / this.weapon_MP7.height);
							break;
						case ITEM.PKP_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_PKPGOLD.width / this.weapon_PKPGOLD.height), 16f), this.weapon_PKPGOLD);
							num += (float)(16 * this.weapon_PKPGOLD.width / this.weapon_PKPGOLD.height);
							break;
						case ITEM.NEGEV:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_NEGEV.width / this.weapon_NEGEV.height), 16f), this.weapon_NEGEV);
							num += (float)(16 * this.weapon_NEGEV.width / this.weapon_NEGEV.height);
							break;
						case ITEM.XM8:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_XM8.width / this.weapon_XM8.height), 16f), this.weapon_XM8);
							num += (float)(16 * this.weapon_XM8.width / this.weapon_XM8.height);
							break;
						case ITEM.AK74:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_AK74.width / this.weapon_AK74.height), 16f), this.weapon_AK74);
							num += (float)(16 * this.weapon_AK74.width / this.weapon_AK74.height);
							break;
						case ITEM.AK47_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_AK47GOLD.width / this.weapon_AK47GOLD.height), 16f), this.weapon_AK47GOLD);
							num += (float)(16 * this.weapon_AK47GOLD.width / this.weapon_AK47GOLD.height);
							break;
						case ITEM.M4A1_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_M4A1GOLD.width / this.weapon_M4A1GOLD.height), 16f), this.weapon_M4A1GOLD);
							num += (float)(16 * this.weapon_M4A1GOLD.width / this.weapon_M4A1GOLD.height);
							break;
						case ITEM.ABAKAN:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_ABAKAN.width / this.weapon_ABAKAN.height), 16f), this.weapon_ABAKAN);
							num += (float)(16 * this.weapon_ABAKAN.width / this.weapon_ABAKAN.height);
							break;
						case ITEM.AK103:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_AK103.width / this.weapon_AK103.height), 16f), this.weapon_AK103);
							num += (float)(16 * this.weapon_AK103.width / this.weapon_AK103.height);
							break;
						case ITEM.BEKAS12M:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_BEKAS12M.width / this.weapon_BEKAS12M.height), 16f), this.weapon_BEKAS12M);
							num += (float)(16 * this.weapon_BEKAS12M.width / this.weapon_BEKAS12M.height);
							break;
						case ITEM.MOSSBERG500:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_MOSSBERG500.width / this.weapon_MOSSBERG500.height), 16f), this.weapon_MOSSBERG500);
							num += (float)(16 * this.weapon_MOSSBERG500.width / this.weapon_MOSSBERG500.height);
							break;
						case ITEM.NOVA_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_NOVAGOLD.width / this.weapon_NOVAGOLD.height), 16f), this.weapon_NOVAGOLD);
							num += (float)(16 * this.weapon_NOVAGOLD.width / this.weapon_NOVAGOLD.height);
							break;
						case ITEM.NEOSTEAD2000:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_NEOSTEAD2000.width / this.weapon_NEOSTEAD2000.height), 16f), this.weapon_NEOSTEAD2000);
							num += (float)(16 * this.weapon_NEOSTEAD2000.width / this.weapon_NEOSTEAD2000.height);
							break;
						case ITEM.HONCHO:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_HONCHO.width / this.weapon_HONCHO.height), 16f), this.weapon_HONCHO);
							num += (float)(16 * this.weapon_HONCHO.width / this.weapon_HONCHO.height);
							break;
						case ITEM.M2:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_M2.width / this.weapon_M2.height), 16f), this.weapon_M2);
							num += (float)(16 * this.weapon_M2.width / this.weapon_M2.height);
							break;
						case ITEM.M4:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_M4.width / this.weapon_M4.height), 16f), this.weapon_M4);
							num += (float)(16 * this.weapon_M4.width / this.weapon_M4.height);
							break;
						case ITEM.SAIGA_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_SAIGAGOLD.width / this.weapon_SAIGAGOLD.height), 16f), this.weapon_SAIGAGOLD);
							num += (float)(16 * this.weapon_SAIGAGOLD.width / this.weapon_SAIGAGOLD.height);
							break;
						case ITEM.BROWNING:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_BROWNING.width / this.weapon_BROWNING.height), 16f), this.weapon_BROWNING);
							num += (float)(16 * this.weapon_BROWNING.width / this.weapon_BROWNING.height);
							break;
						case ITEM.WALTHER:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_WALTHER.width / this.weapon_WALTHER.height), 16f), this.weapon_WALTHER);
							num += (float)(16 * this.weapon_WALTHER.width / this.weapon_WALTHER.height);
							break;
						case ITEM.CZ75:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_CZ75.width / this.weapon_CZ75.height), 16f), this.weapon_CZ75);
							num += (float)(16 * this.weapon_CZ75.width / this.weapon_CZ75.height);
							break;
						case ITEM.SWM29:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_SWM29.width / this.weapon_SWM29.height), 16f), this.weapon_SWM29);
							num += (float)(16 * this.weapon_SWM29.width / this.weapon_SWM29.height);
							break;
						case ITEM.UZI:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_UZI.width / this.weapon_UZI.height), 16f), this.weapon_UZI);
							num += (float)(16 * this.weapon_UZI.width / this.weapon_UZI.height);
							break;
						case ITEM.M1887:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_M1887.width / this.weapon_M1887.height), 16f), this.weapon_M1887);
							num += (float)(16 * this.weapon_M1887.width / this.weapon_M1887.height);
							break;
						case ITEM.REMINGTON870:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_REMINGTON870.width / this.weapon_REMINGTON870.height), 16f), this.weapon_REMINGTON870);
							num += (float)(16 * this.weapon_REMINGTON870.width / this.weapon_REMINGTON870.height);
							break;
						case ITEM.VEPR12:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_VEPR12.width / this.weapon_VEPR12.height), 16f), this.weapon_VEPR12);
							num += (float)(16 * this.weapon_VEPR12.width / this.weapon_VEPR12.height);
							break;
						case ITEM.P226:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_P226.width / this.weapon_P226.height), 16f), this.weapon_P226);
							num += (float)(16 * this.weapon_P226.width / this.weapon_P226.height);
							break;
						case ITEM.QBU88:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_QBU88.width / this.weapon_QBU88.height), 16f), this.weapon_QBU88);
							num += (float)(16 * this.weapon_QBU88.width / this.weapon_QBU88.height);
							break;
						case ITEM.DEAGLE_GOLD:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_DEAGLEGOLD.width / this.weapon_DEAGLEGOLD.height), 16f), this.weapon_DEAGLEGOLD);
							num += (float)(16 * this.weapon_DEAGLEGOLD.width / this.weapon_DEAGLEGOLD.height);
							break;
						case ITEM.KUKRI:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_KUKRI.width / this.weapon_KUKRI.height), 16f), this.weapon_KUKRI);
							num += (float)(16 * this.weapon_KUKRI.width / this.weapon_KUKRI.height);
							break;
						case ITEM.FALSHION:
							GUI.DrawTexture(new Rect(num, (float)(10 + i * 20), (float)(16 * this.weapon_FALSHION.width / this.weapon_FALSHION.height), 16f), this.weapon_FALSHION);
							num += (float)(16 * this.weapon_FALSHION.width / this.weapon_FALSHION.height);
							break;
						}
					}
					else
					{
						GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 16f, 16.5f), this.hudmsg, new Rect(0.90234375f, 0.3359375f, 0.0625f, 0.0625f));
						num += 16f;
					}
					break;
				}
				num += 4f;
				if (this.d_headshot[i])
				{
					GUI.DrawTextureWithTexCoords(new Rect(num, (float)(10 + i * 20), 40f, 16.5f), this.hudmsg, new Rect(0.00390625f, 0.93359375f, 0.15625f, 0.0625f));
					num += 40f;
					num += 4f;
				}
				GUIManager.DrawColorText(num, (float)(10 + i * 20), this.d_victim[i], TextAnchor.MiddleLeft);
			}
		}
		if (this.killstreak_draw)
		{
			if (this.killstreak_time + 3f > Time.time)
			{
				this.gui_style2.normal.textColor = new Color(0f, 0f, 0f, 1f);
				GUI.Label(new Rect((float)Screen.width / 2f - 20f + 1f, GUIManager.YRES(220f) + 1f, 200f, 32f), "+" + this.killstreak.ToString(), this.gui_style2);
				if (this.killstreak_color == 0)
				{
					this.gui_style2.normal.textColor = new Color(0f, 0f, 1f, 1f);
				}
				else if (this.killstreak_color == 1)
				{
					this.gui_style2.normal.textColor = new Color(1f, 0f, 0f, 1f);
				}
				else if (this.killstreak_color == 2)
				{
					this.gui_style2.normal.textColor = new Color(0f, 1f, 0f, 1f);
				}
				else if (this.killstreak_color == 3)
				{
					this.gui_style2.normal.textColor = new Color(1f, 1f, 0f, 1f);
				}
				GUI.Label(new Rect((float)Screen.width / 2f - 20f, GUIManager.YRES(220f), 200f, 32f), "+" + this.killstreak.ToString(), this.gui_style2);
				return;
			}
			this.killstreak_draw = false;
		}
	}

	// Token: 0x06000380 RID: 896 RVA: 0x00040F54 File Offset: 0x0003F154
	public void AddDeathMsg(int attackerid, int victimid, int weaponid, int hitbox)
	{
		if (this.cscl == null)
		{
			this.cscl = (Client)Object.FindObjectOfType(typeof(Client));
		}
		for (int i = 0; i < 3; i++)
		{
			this.d_attacker[i] = this.d_attacker[i + 1];
			this.d_victim[i] = this.d_victim[i + 1];
			this.d_weaponid[i] = this.d_weaponid[i + 1];
			this.d_headshot[i] = this.d_headshot[i + 1];
		}
		this.d_weaponid[3] = weaponid;
		if (hitbox == 1)
		{
			this.d_headshot[3] = true;
		}
		else
		{
			this.d_headshot[3] = false;
		}
		if (attackerid != 255 && attackerid != 254 && attackerid != 253 && attackerid != 252)
		{
			this.d_attacker[3] = "^" + RemotePlayersUpdater.Instance.Bots[attackerid].Team.ToString() + RemotePlayersUpdater.Instance.Bots[attackerid].Name;
		}
		if (victimid != 253 && victimid != 252)
		{
			this.d_victim[3] = "^" + RemotePlayersUpdater.Instance.Bots[victimid].Team.ToString() + RemotePlayersUpdater.Instance.Bots[victimid].Name;
		}
		if (victimid == 252)
		{
			this.d_victim[3] = "^1" + Lang.GetLabel(557) + " " + Lang.GetLabel(558);
			return;
		}
		if (victimid == 253)
		{
			this.d_victim[3] = "^1" + Lang.GetLabel(557);
			return;
		}
		if (attackerid == 252)
		{
			this.d_attacker[3] = "^1" + Lang.GetLabel(557) + " " + Lang.GetLabel(558);
			return;
		}
		if (attackerid == 253)
		{
			this.d_attacker[3] = "^1" + Lang.GetLabel(557);
			return;
		}
		if (attackerid == 254)
		{
			this.d_attacker[3] = "^1[" + this.cscl.zs_wave.ToString() + "]" + Lang.GetLabel(556);
			return;
		}
		if (attackerid == 255)
		{
			this.d_attacker[3] = "^8WORLD";
			return;
		}
		if (attackerid == this.cscl.myindex && attackerid != victimid)
		{
			this.killstreak++;
			this.killstreak_time = Time.time;
			this.killstreak_draw = true;
			this.killstreak_color = (int)RemotePlayersUpdater.Instance.Bots[victimid].Team;
		}
		if (victimid == this.cscl.myindex)
		{
			this.killstreak = 0;
		}
	}

	// Token: 0x0400067E RID: 1662
	private Client cscl;

	// Token: 0x0400067F RID: 1663
	private GameObject Map;

	// Token: 0x04000680 RID: 1664
	private GUIStyle gui_style2;

	// Token: 0x04000681 RID: 1665
	private string[] d_attacker = new string[4];

	// Token: 0x04000682 RID: 1666
	private string[] d_victim = new string[4];

	// Token: 0x04000683 RID: 1667
	private int[] d_weaponid = new int[4];

	// Token: 0x04000684 RID: 1668
	private bool[] d_headshot = new bool[4];

	// Token: 0x04000685 RID: 1669
	private int[] d_flag = new int[4];

	// Token: 0x04000686 RID: 1670
	private float lastupdate;

	// Token: 0x04000687 RID: 1671
	public int killstreak;

	// Token: 0x04000688 RID: 1672
	private bool killstreak_draw;

	// Token: 0x04000689 RID: 1673
	private float killstreak_time;

	// Token: 0x0400068A RID: 1674
	private int killstreak_color;

	// Token: 0x0400068B RID: 1675
	private Texture2D hudmsg;

	// Token: 0x0400068C RID: 1676
	private Texture2D weapon_ntw20;

	// Token: 0x0400068D RID: 1677
	private Texture2D weapon_vss_desert;

	// Token: 0x0400068E RID: 1678
	private Texture2D weapon_tank_light;

	// Token: 0x0400068F RID: 1679
	private Texture2D weapon_tank_medium;

	// Token: 0x04000690 RID: 1680
	private Texture2D weapon_tank_heavy;

	// Token: 0x04000691 RID: 1681
	private Texture2D weapon_minigun;

	// Token: 0x04000692 RID: 1682
	private Texture2D weapon_minefly;

	// Token: 0x04000693 RID: 1683
	private Texture2D weapon_javelin;

	// Token: 0x04000694 RID: 1684
	private Texture2D weapon_zaa12;

	// Token: 0x04000695 RID: 1685
	private Texture2D weapon_zasval;

	// Token: 0x04000696 RID: 1686
	private Texture2D weapon_zfn57;

	// Token: 0x04000697 RID: 1687
	private Texture2D weapon_zkord;

	// Token: 0x04000698 RID: 1688
	private Texture2D weapon_zm249;

	// Token: 0x04000699 RID: 1689
	private Texture2D weapon_zminigun;

	// Token: 0x0400069A RID: 1690
	private Texture2D weapon_zsps12;

	// Token: 0x0400069B RID: 1691
	private Texture2D weapon_mk3;

	// Token: 0x0400069C RID: 1692
	private Texture2D weapon_rkg3;

	// Token: 0x0400069D RID: 1693
	private Texture2D weapon_tube;

	// Token: 0x0400069E RID: 1694
	private Texture2D weapon_bulava;

	// Token: 0x0400069F RID: 1695
	private Texture2D weapon_katana;

	// Token: 0x040006A0 RID: 1696
	private Texture2D weapon_crossbow;

	// Token: 0x040006A1 RID: 1697
	private Texture2D weapon_mauzer;

	// Token: 0x040006A2 RID: 1698
	private Texture2D weapon_qbz95;

	// Token: 0x040006A3 RID: 1699
	private Texture2D weapon_mine;

	// Token: 0x040006A4 RID: 1700
	private Texture2D weapon_c4;

	// Token: 0x040006A5 RID: 1701
	private Texture2D weapon_chopper;

	// Token: 0x040006A6 RID: 1702
	private Texture2D weapon_shield;

	// Token: 0x040006A7 RID: 1703
	private Texture2D weapon_aksu;

	// Token: 0x040006A8 RID: 1704
	private Texture2D weapon_m700;

	// Token: 0x040006A9 RID: 1705
	private Texture2D weapon_stechkin;

	// Token: 0x040006AA RID: 1706
	private Texture2D weapon_at_mine;

	// Token: 0x040006AB RID: 1707
	private Texture2D weapon_molotov;

	// Token: 0x040006AC RID: 1708
	private Texture2D weapon_m202;

	// Token: 0x040006AD RID: 1709
	private Texture2D weapon_gg;

	// Token: 0x040006AE RID: 1710
	private Texture2D weapon_dpm;

	// Token: 0x040006AF RID: 1711
	private Texture2D weapon_m1924;

	// Token: 0x040006B0 RID: 1712
	private Texture2D weapon_mg42;

	// Token: 0x040006B1 RID: 1713
	private Texture2D weapon_sten;

	// Token: 0x040006B2 RID: 1714
	private Texture2D weapon_m1a1;

	// Token: 0x040006B3 RID: 1715
	private Texture2D weapon_type99;

	// Token: 0x040006B4 RID: 1716
	private Texture2D weapon_jeep;

	// Token: 0x040006B5 RID: 1717
	private Texture2D weapon_bizon;

	// Token: 0x040006B6 RID: 1718
	private Texture2D weapon_pila;

	// Token: 0x040006B7 RID: 1719
	private Texture2D weapon_groza;

	// Token: 0x040006B8 RID: 1720
	private Texture2D weapon_jackhammer;

	// Token: 0x040006B9 RID: 1721
	private Texture2D weapon_tykva;

	// Token: 0x040006BA RID: 1722
	private Texture2D weapon_psg_1;

	// Token: 0x040006BB RID: 1723
	private Texture2D weapon_krytac;

	// Token: 0x040006BC RID: 1724
	private Texture2D weapon_mp5sd;

	// Token: 0x040006BD RID: 1725
	private Texture2D weapon_colts;

	// Token: 0x040006BE RID: 1726
	private Texture2D weapon_jackhammer_lady;

	// Token: 0x040006BF RID: 1727
	private Texture2D weapon_m700_lady;

	// Token: 0x040006C0 RID: 1728
	private Texture2D weapon_mg42_lady;

	// Token: 0x040006C1 RID: 1729
	private Texture2D weapon_magnum_lady;

	// Token: 0x040006C2 RID: 1730
	private Texture2D weapon_scorpion;

	// Token: 0x040006C3 RID: 1731
	private Texture2D weapon_g36c_veteran;

	// Token: 0x040006C4 RID: 1732
	private Texture2D weapon_snowball;

	// Token: 0x040006C5 RID: 1733
	private Texture2D weapon_fmg9;

	// Token: 0x040006C6 RID: 1734
	private Texture2D weapon_saiga;

	// Token: 0x040006C7 RID: 1735
	private Texture2D weapon_flamethrower;

	// Token: 0x040006C8 RID: 1736
	private Texture2D weapon_ak47_snow;

	// Token: 0x040006C9 RID: 1737
	private Texture2D weapon_p90_snow;

	// Token: 0x040006CA RID: 1738
	private Texture2D weapon_saiga_snow;

	// Token: 0x040006CB RID: 1739
	private Texture2D weapon_sr25_snow;

	// Token: 0x040006CC RID: 1740
	private Texture2D weapon_usp_snow;

	// Token: 0x040006CD RID: 1741
	private Texture2D weapon_kar98k;

	// Token: 0x040006CE RID: 1742
	private Texture2D weapon_mg13;

	// Token: 0x040006CF RID: 1743
	private Texture2D weapon_rpd;

	// Token: 0x040006D0 RID: 1744
	private Texture2D weapon_tt;

	// Token: 0x040006D1 RID: 1745
	private Texture2D weapon_handgrenade;

	// Token: 0x040006D2 RID: 1746
	private Texture2D weapon_L96A1GOLD;

	// Token: 0x040006D3 RID: 1747
	private Texture2D weapon_SVT40;

	// Token: 0x040006D4 RID: 1748
	private Texture2D weapon_SVUAS;

	// Token: 0x040006D5 RID: 1749
	private Texture2D weapon_PSG1GOLD;

	// Token: 0x040006D6 RID: 1750
	private Texture2D weapon_M110;

	// Token: 0x040006D7 RID: 1751
	private Texture2D weapon_KEDR;

	// Token: 0x040006D8 RID: 1752
	private Texture2D weapon_MAC10GOLD;

	// Token: 0x040006D9 RID: 1753
	private Texture2D weapon_KASHTAN;

	// Token: 0x040006DA RID: 1754
	private Texture2D weapon_FMG9GOLD;

	// Token: 0x040006DB RID: 1755
	private Texture2D weapon_MP7;

	// Token: 0x040006DC RID: 1756
	private Texture2D weapon_PKPGOLD;

	// Token: 0x040006DD RID: 1757
	private Texture2D weapon_NEGEV;

	// Token: 0x040006DE RID: 1758
	private Texture2D weapon_XM8;

	// Token: 0x040006DF RID: 1759
	private Texture2D weapon_AK74;

	// Token: 0x040006E0 RID: 1760
	private Texture2D weapon_AK47GOLD;

	// Token: 0x040006E1 RID: 1761
	private Texture2D weapon_M4A1GOLD;

	// Token: 0x040006E2 RID: 1762
	private Texture2D weapon_ABAKAN;

	// Token: 0x040006E3 RID: 1763
	private Texture2D weapon_AK103;

	// Token: 0x040006E4 RID: 1764
	private Texture2D weapon_BEKAS12M;

	// Token: 0x040006E5 RID: 1765
	private Texture2D weapon_MOSSBERG500;

	// Token: 0x040006E6 RID: 1766
	private Texture2D weapon_NOVAGOLD;

	// Token: 0x040006E7 RID: 1767
	private Texture2D weapon_NEOSTEAD2000;

	// Token: 0x040006E8 RID: 1768
	private Texture2D weapon_HONCHO;

	// Token: 0x040006E9 RID: 1769
	private Texture2D weapon_M2;

	// Token: 0x040006EA RID: 1770
	private Texture2D weapon_M4;

	// Token: 0x040006EB RID: 1771
	private Texture2D weapon_SAIGAGOLD;

	// Token: 0x040006EC RID: 1772
	private Texture2D weapon_BROWNING;

	// Token: 0x040006ED RID: 1773
	private Texture2D weapon_WALTHER;

	// Token: 0x040006EE RID: 1774
	private Texture2D weapon_CZ75;

	// Token: 0x040006EF RID: 1775
	private Texture2D weapon_SWM29;

	// Token: 0x040006F0 RID: 1776
	private Texture2D weapon_UZI;

	// Token: 0x040006F1 RID: 1777
	private Texture2D weapon_M1887;

	// Token: 0x040006F2 RID: 1778
	private Texture2D weapon_REMINGTON870;

	// Token: 0x040006F3 RID: 1779
	private Texture2D weapon_VEPR12;

	// Token: 0x040006F4 RID: 1780
	private Texture2D weapon_P226;

	// Token: 0x040006F5 RID: 1781
	private Texture2D weapon_QBU88;

	// Token: 0x040006F6 RID: 1782
	private Texture2D weapon_DEAGLEGOLD;

	// Token: 0x040006F7 RID: 1783
	private Texture2D weapon_KUKRI;

	// Token: 0x040006F8 RID: 1784
	private Texture2D weapon_FALSHION;

	// Token: 0x040006F9 RID: 1785
	private Texture2D weapon_sr25;
}
