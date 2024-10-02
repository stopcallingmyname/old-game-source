using System;
using UnityEngine;

// Token: 0x0200007F RID: 127
public class GUIManager
{
	// Token: 0x06000382 RID: 898 RVA: 0x00041253 File Offset: 0x0003F453
	public static float XRES(float x)
	{
		return x * ((float)Screen.width / 1024f) + 0.5f;
	}

	// Token: 0x06000383 RID: 899 RVA: 0x00041269 File Offset: 0x0003F469
	public static float YRES(float y)
	{
		return y * ((float)Screen.height / 768f) + 0.5f;
	}

	// Token: 0x06000384 RID: 900 RVA: 0x0004127F File Offset: 0x0003F47F
	public static float ASPECTRATIO()
	{
		return (float)(Screen.width / Screen.height);
	}

	// Token: 0x06000385 RID: 901 RVA: 0x0004128D File Offset: 0x0003F48D
	public static void PreInit()
	{
		GUIManager.tex_red = new Texture2D(1, 1);
		GUIManager.tex_red.SetPixel(0, 0, new Color(1f, 0f, 0f, 1f));
		GUIManager.tex_red.Apply();
	}

	// Token: 0x06000386 RID: 902 RVA: 0x000412CC File Offset: 0x0003F4CC
	public static void Init(bool _only_localize = false)
	{
		GUIManager.logo = ContentLoader.LoadTexture("logo" + Lang.current.ToString());
		GUIManager.prazd_logo = ContentLoader.LoadTexture("winter_logo_eng" + Lang.current.ToString());
		GUIManager.tex_sound_off = ContentLoader.LoadTexture("sound_off" + Lang.current.ToString());
		GUIManager.tex_sound_on = ContentLoader.LoadTexture("sound_on" + Lang.current.ToString());
		GUIManager.tex_exit = ContentLoader.LoadTexture("exit" + Lang.current.ToString());
		GUIManager.tex_help = ContentLoader.LoadTexture("help" + Lang.current.ToString());
		GUIManager.tex_fullscreen = ContentLoader.LoadTexture("fullscreen" + Lang.current.ToString());
		GUIManager.tex_menu_start = ContentLoader.LoadTexture("menu_start" + Lang.current.ToString());
		GUIManager.tex_menu_option = ContentLoader.LoadTexture("menu_options" + Lang.current.ToString());
		GUIManager.tex_menu_profile = ContentLoader.LoadTexture("menu_profle" + Lang.current.ToString());
		GUIManager.tex_menu_shop = ContentLoader.LoadTexture("menu_shop" + Lang.current.ToString());
		GUIManager.tex_menu_playerstats = ContentLoader.LoadTexture("menu_stats" + Lang.current.ToString());
		GUIManager.tex_menu_inv = ContentLoader.LoadTexture("menu_inv" + Lang.current.ToString());
		GUIManager.tex_menu_clan = ContentLoader.LoadTexture("menu_clan" + Lang.current.ToString());
		GUIManager.tex_menu_master = ContentLoader.LoadTexture("menu_master" + Lang.current.ToString());
		GUIManager.tex_icon_premium = ContentLoader.LoadTexture("premium_icon" + Lang.current.ToString());
		GUIManager.tex_loading = ContentLoader.LoadTexture("loading" + Lang.current.ToString());
		if (DateTime.UtcNow >= CONST.EXT.TIME_START && DateTime.UtcNow < CONST.EXT.TIME_END)
		{
			GUIManager.tex_top_plus = ContentLoader.LoadTexture("action_coin_menu_plus" + Lang.current.ToString());
			GUIManager.tex_top_plus2 = ContentLoader.LoadTexture("action_coin_menu_plus2" + Lang.current.ToString());
			GUIManager.tex_action_banner = ContentLoader.LoadTexture("Header" + Lang.current.ToString());
		}
		else
		{
			GUIManager.tex_top_plus = ContentLoader.LoadTexture("coin_menu_plus" + Lang.current.ToString());
			GUIManager.tex_top_plus2 = ContentLoader.LoadTexture("coin_menu_plus2" + Lang.current.ToString());
			GUIManager.tex_action_banner = null;
		}
		GUIManager.tex_menu_zadanie = ContentLoader.LoadTexture("menu_zadanie" + Lang.current.ToString());
		GUIManager.tex_item_select = ContentLoader.LoadTexture("select" + Lang.current.ToString());
		GUIManager.tex_item_open = ContentLoader.LoadTexture("open" + Lang.current.ToString());
		GUIManager.tex_premium_big = ContentLoader.LoadTexture("newprem" + Lang.current.ToString());
		GUIManager.tex_megapack = ContentLoader.LoadTexture("megapack" + Lang.current.ToString());
		GUIManager.tex_clan_manage = ContentLoader.LoadTexture("manage" + Lang.current.ToString());
		GUIManager.tex_clan_delete = ContentLoader.LoadTexture("delete" + Lang.current.ToString());
		GUIManager.tex_clan_accept = ContentLoader.LoadTexture("accept" + Lang.current.ToString());
		GUIManager.tex_clan_decline = ContentLoader.LoadTexture("decline" + Lang.current.ToString());
		GUIManager.tex_clan_invite = ContentLoader.LoadTexture("invite" + Lang.current.ToString());
		GUIManager.tex_upgrade[1] = ContentLoader.LoadTexture("button_damage" + Lang.current.ToString());
		GUIManager.tex_upgrade[2] = ContentLoader.LoadTexture("button_clip" + Lang.current.ToString());
		GUIManager.tex_upgrade[3] = ContentLoader.LoadTexture("button_backpack" + Lang.current.ToString());
		GUIManager.tex_upgrade[4] = ContentLoader.LoadTexture("button_zombie" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle[1] = ContentLoader.LoadTexture("button_life" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle[2] = ContentLoader.LoadTexture("button_armor" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle[3] = ContentLoader.LoadTexture("button_speed" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle[4] = ContentLoader.LoadTexture("button_tank_reloadspeed" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle[5] = ContentLoader.LoadTexture("button_turret" + Lang.current.ToString());
		GUIManager.tex_upgrade_active[1] = ContentLoader.LoadTexture("button_damage_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_active[2] = ContentLoader.LoadTexture("button_clip_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_active[3] = ContentLoader.LoadTexture("button_backpack_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_active[4] = ContentLoader.LoadTexture("button_zombie_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle_active[1] = ContentLoader.LoadTexture("button_tank_health_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle_active[2] = ContentLoader.LoadTexture("button_tank_shield_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle_active[3] = ContentLoader.LoadTexture("button_tank_speed_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle_active[4] = ContentLoader.LoadTexture("button_tank_reloadspeed_buy" + Lang.current.ToString());
		GUIManager.tex_upgrade_vehicle_active[5] = ContentLoader.LoadTexture("button_tank_aimspeed_buy" + Lang.current.ToString());
		GUIManager.tex_bonus = ContentLoader.LoadTexture("bonus" + Lang.current);
		GUIManager.tex_discount = ContentLoader.LoadTexture("discount" + Lang.current);
		GUIManager.start_back = ContentLoader.LoadTexture("start_bonus" + Lang.current.ToString());
		GUIManager.day_back = ContentLoader.LoadTexture("day_bonus" + Lang.current.ToString());
		GUIManager.week_back = ContentLoader.LoadTexture("week_bonus" + Lang.current.ToString());
		GUIManager.level_back = ContentLoader.LoadTexture("level_bonus" + Lang.current.ToString());
		if (!_only_localize)
		{
			GUIManager.tex_button = ContentLoader.LoadTexture("ingame_disconnect");
			GUIManager.tex_button_hover = ContentLoader.LoadTexture("ingame");
			GUIManager.goPlayer = GameObject.Find("Player");
			GUIManager.goMainCamera = GameObject.Find("Main Camera");
			GUIManager.gs_empty = new GUIStyle();
			GUIManager.gs_style1 = new GUIStyle();
			GUIManager.gs_style2 = new GUIStyle();
			GUIManager.gs_style3 = new GUIStyle();
			GUIManager.gs_style1.font = FontManager.font[2];
			GUIManager.gs_style1.fontSize = 16;
			GUIManager.gs_style1.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUIManager.gs_style1.alignment = TextAnchor.MiddleCenter;
			GUIManager.gs_style1.richText = false;
			GUIManager.gs_style2.font = FontManager.font[3];
			GUIManager.gs_style2.fontSize = 14;
			GUIManager.gs_style2.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUIManager.gs_style2.alignment = TextAnchor.MiddleLeft;
			GUIManager.gs_style2.richText = false;
			GUIManager.gs_style3.font = FontManager.font[0];
			GUIManager.gs_style3.fontSize = 16;
			GUIManager.gs_style3.normal.textColor = new Color(1f, 1f, 1f, 1f);
			GUIManager.gs_style3.alignment = TextAnchor.MiddleCenter;
			GUIManager.gs_style3.richText = false;
			GUIManager.c[0] = new Color(0f, 0f, 1f, 1f);
			GUIManager.c[1] = new Color(1f, 0f, 0f, 1f);
			GUIManager.c[2] = new Color(0f, 1f, 0f, 1f);
			GUIManager.c[3] = new Color(1f, 1f, 0f, 1f);
			GUIManager.c[4] = new Color(1f, 1f, 1f, 1f);
			GUIManager.c[5] = new Color(1f, 1f, 1f, 1f);
			GUIManager.c[6] = new Color(1f, 1f, 1f, 1f);
			GUIManager.c[7] = new Color(1f, 1f, 1f, 1f);
			GUIManager.c[8] = new Color(1f, 1f, 1f, 1f);
			GUIManager.c[9] = new Color(0f, 0f, 0f, 1f);
			GUIManager.tex_black = new Texture2D(1, 1);
			GUIManager.tex_black.SetPixel(0, 0, new Color(0f, 0f, 0f, 1f));
			GUIManager.tex_black.Apply();
			GUIManager.tex_half_black = new Texture2D(1, 1);
			GUIManager.tex_half_black.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
			GUIManager.tex_half_black.Apply();
			GUIManager.tex_blue = new Texture2D(1, 1);
			GUIManager.tex_blue.SetPixel(0, 0, new Color(0f, 0f, 1f, 1f));
			GUIManager.tex_blue.Apply();
			GUIManager.tex_green = new Texture2D(1, 1);
			GUIManager.tex_green.SetPixel(0, 0, new Color(0f, 1f, 0f, 1f));
			GUIManager.tex_green.Apply();
			GUIManager.tex_yellow = new Texture2D(1, 1);
			GUIManager.tex_yellow.SetPixel(0, 0, new Color(1f, 1f, 0f, 1f));
			GUIManager.tex_yellow.Apply();
			GUIManager.tex_half_blue = new Texture2D(1, 1);
			GUIManager.tex_half_blue.SetPixel(0, 0, new Color(0f, 0f, 1f, 0.5f));
			GUIManager.tex_half_blue.Apply();
			GUIManager.tex_half_red = new Texture2D(1, 1);
			GUIManager.tex_half_red.SetPixel(0, 0, new Color(1f, 0f, 0f, 0.5f));
			GUIManager.tex_half_red.Apply();
			GUIManager.tex_half_green = new Texture2D(1, 1);
			GUIManager.tex_half_green.SetPixel(0, 0, new Color(0f, 1f, 0f, 0.5f));
			GUIManager.tex_half_green.Apply();
			GUIManager.tex_half_yellow = new Texture2D(1, 1);
			GUIManager.tex_half_yellow.SetPixel(0, 0, new Color(1f, 1f, 0f, 0.5f));
			GUIManager.tex_half_yellow.Apply();
			for (int i = 0; i < 10; i++)
			{
				GUIManager.tex_gold_variants[i] = ContentLoader.LoadTexture("gold_variant" + i.ToString());
			}
			GUIManager.tex_menu_social = ContentLoader.LoadTexture("menu_social");
			GUIManager.tex_face_icon = ContentLoader.LoadTexture("face_icon");
			GUIManager.tex_edit_icon = ContentLoader.LoadTexture("edit");
			GUIManager.tex_save_icon = ContentLoader.LoadTexture("save");
			GUIManager.tex_icon_load = ContentLoader.LoadTexture("icon_load");
			GUIManager.tex_menu_back = ContentLoader.LoadTexture("menu_back");
			GUIManager.tex_button256 = ContentLoader.LoadTexture("button256");
			GUIManager.tex_button128 = ContentLoader.LoadTexture("button128");
			GUIManager.tex_button96 = ContentLoader.LoadTexture("button96");
			GUIManager.tex_button22 = ContentLoader.LoadTexture("button22");
			GUIManager.tex_edit96 = ContentLoader.LoadTexture("edit96");
			GUIManager.tex_button_blue = ContentLoader.LoadTexture("button_blue");
			GUIManager.tex_button_green = ContentLoader.LoadTexture("button_green");
			GUIManager.tex_button_red = ContentLoader.LoadTexture("menu_red");
			GUIManager.tex_bar_blue = ContentLoader.LoadTexture("barb");
			GUIManager.tex_bar_yellow = ContentLoader.LoadTexture("bary");
			GUIManager.tex_panel = ContentLoader.LoadTexture("menu_panel");
			GUIManager.tex_select = ContentLoader.LoadTexture("menu_select");
			GUIManager.tex_hover = ContentLoader.LoadTexture("menu_hover");
			GUIManager.tex_server = ContentLoader.LoadTexture("server2");
			GUIManager.tex_server_hover = ContentLoader.LoadTexture("server_hover");
			GUIManager.tex_hint = ContentLoader.LoadTexture("hint");
			GUIManager.gm0 = ContentLoader.LoadTexture("gm0");
			GUIManager.gm1 = ContentLoader.LoadTexture("gm1");
			GUIManager.gm2 = ContentLoader.LoadTexture("gm2");
			GUIManager.gm3 = ContentLoader.LoadTexture("gm3");
			GUIManager.gm4 = ContentLoader.LoadTexture("gm4");
			GUIManager.gm5 = ContentLoader.LoadTexture("gm5");
			GUIManager.gm6 = ContentLoader.LoadTexture("gm6");
			GUIManager.gm7 = ContentLoader.LoadTexture("gm7");
			GUIManager.gm8 = ContentLoader.LoadTexture("gm8");
			GUIManager.gm9 = ContentLoader.LoadTexture("gm9");
			GUIManager.gm10 = ContentLoader.LoadTexture("gm10");
			GUIManager.gm11 = ContentLoader.LoadTexture("gm11");
			GUIManager.gm12 = ContentLoader.LoadTexture("gm12");
			GUIManager.gm13 = ContentLoader.LoadTexture("gm13");
			GUIManager.pro = ContentLoader.LoadTexture("pro" + Lang.current.ToString());
			GUIManager.select_glow = ContentLoader.LoadTexture("select_glow");
			GUIManager.hover_glow = ContentLoader.LoadTexture("hover_glow");
			GUIManager.hover_part_glow = ContentLoader.LoadTexture("select_glow_part");
			GUIManager.tex_padlock = ContentLoader.LoadTexture("padlock");
			GUIManager.tex_fb = ContentLoader.LoadTexture("net_fb");
			GUIManager.tex_kg = ContentLoader.LoadTexture("net_kg");
			GUIManager.tex_mm = ContentLoader.LoadTexture("net_mm");
			GUIManager.tex_nl = ContentLoader.LoadTexture("net_nl");
			GUIManager.tex_ok = ContentLoader.LoadTexture("net_ok");
			GUIManager.tex_st = ContentLoader.LoadTexture("net_st");
			GUIManager.tex_vk = ContentLoader.LoadTexture("net_vk");
			GUIManager.tex_kr = ContentLoader.LoadTexture("net_kr");
			GUIManager.tex_flags_filter[0] = ContentLoader.LoadTexture("RUSSIA");
			GUIManager.tex_flags_filter[1] = ContentLoader.LoadTexture("EUROPE");
			GUIManager.tex_flags_filter[2] = ContentLoader.LoadTexture("USA");
			GUIManager.tex_flags[0] = ContentLoader.LoadTexture("flag_xx");
			GUIManager.tex_flags[1] = ContentLoader.LoadTexture("flag__ru");
			GUIManager.tex_flags[2] = ContentLoader.LoadTexture("__ua");
			GUIManager.tex_flags[3] = ContentLoader.LoadTexture("__by");
			GUIManager.tex_flags[4] = ContentLoader.LoadTexture("__kz");
			GUIManager.tex_flags[5] = ContentLoader.LoadTexture("__md");
			GUIManager.tex_flags[6] = ContentLoader.LoadTexture("__ee");
			GUIManager.tex_flags[7] = ContentLoader.LoadTexture("__lv");
			GUIManager.tex_flags[8] = ContentLoader.LoadTexture("__de");
			GUIManager.tex_flags[9] = ContentLoader.LoadTexture("__am");
			GUIManager.tex_flags[10] = ContentLoader.LoadTexture("flag__us");
			GUIManager.tex_flags[11] = ContentLoader.LoadTexture("ad");
			GUIManager.tex_flags[12] = ContentLoader.LoadTexture("ae");
			GUIManager.tex_flags[13] = ContentLoader.LoadTexture("af");
			GUIManager.tex_flags[14] = ContentLoader.LoadTexture("ag");
			GUIManager.tex_flags[15] = ContentLoader.LoadTexture("ai");
			GUIManager.tex_flags[16] = ContentLoader.LoadTexture("al");
			GUIManager.tex_flags[17] = ContentLoader.LoadTexture("an");
			GUIManager.tex_flags[18] = ContentLoader.LoadTexture("ao");
			GUIManager.tex_flags[19] = ContentLoader.LoadTexture("ar");
			GUIManager.tex_flags[20] = ContentLoader.LoadTexture("as");
			GUIManager.tex_flags[21] = ContentLoader.LoadTexture("at");
			GUIManager.tex_flags[22] = ContentLoader.LoadTexture("au");
			GUIManager.tex_flags[23] = ContentLoader.LoadTexture("aw");
			GUIManager.tex_flags[24] = ContentLoader.LoadTexture("ax");
			GUIManager.tex_flags[25] = ContentLoader.LoadTexture("az");
			GUIManager.tex_flags[26] = ContentLoader.LoadTexture("ba");
			GUIManager.tex_flags[27] = ContentLoader.LoadTexture("bb");
			GUIManager.tex_flags[28] = ContentLoader.LoadTexture("bd");
			GUIManager.tex_flags[29] = ContentLoader.LoadTexture("be");
			GUIManager.tex_flags[30] = ContentLoader.LoadTexture("bf");
			GUIManager.tex_flags[31] = ContentLoader.LoadTexture("bg");
			GUIManager.tex_flags[32] = ContentLoader.LoadTexture("bh");
			GUIManager.tex_flags[33] = ContentLoader.LoadTexture("bi");
			GUIManager.tex_flags[34] = ContentLoader.LoadTexture("bj");
			GUIManager.tex_flags[35] = ContentLoader.LoadTexture("bm");
			GUIManager.tex_flags[36] = ContentLoader.LoadTexture("bn");
			GUIManager.tex_flags[37] = ContentLoader.LoadTexture("bo");
			GUIManager.tex_flags[38] = ContentLoader.LoadTexture("br");
			GUIManager.tex_flags[39] = ContentLoader.LoadTexture("bs");
			GUIManager.tex_flags[40] = ContentLoader.LoadTexture("bt");
			GUIManager.tex_flags[41] = ContentLoader.LoadTexture("bv");
			GUIManager.tex_flags[42] = ContentLoader.LoadTexture("bw");
			GUIManager.tex_flags[43] = ContentLoader.LoadTexture("bz");
			GUIManager.tex_flags[44] = ContentLoader.LoadTexture("ca");
			GUIManager.tex_flags[45] = ContentLoader.LoadTexture("cc");
			GUIManager.tex_flags[46] = ContentLoader.LoadTexture("cd");
			GUIManager.tex_flags[47] = ContentLoader.LoadTexture("cf");
			GUIManager.tex_flags[48] = ContentLoader.LoadTexture("cg");
			GUIManager.tex_flags[49] = ContentLoader.LoadTexture("ch");
			GUIManager.tex_flags[50] = ContentLoader.LoadTexture("ci");
			GUIManager.tex_flags[51] = ContentLoader.LoadTexture("ck");
			GUIManager.tex_flags[52] = ContentLoader.LoadTexture("cl");
			GUIManager.tex_flags[53] = ContentLoader.LoadTexture("cm");
			GUIManager.tex_flags[54] = ContentLoader.LoadTexture("cn");
			GUIManager.tex_flags[55] = ContentLoader.LoadTexture("co");
			GUIManager.tex_flags[56] = ContentLoader.LoadTexture("cr");
			GUIManager.tex_flags[57] = ContentLoader.LoadTexture("cs");
			GUIManager.tex_flags[58] = ContentLoader.LoadTexture("cu");
			GUIManager.tex_flags[59] = ContentLoader.LoadTexture("cv");
			GUIManager.tex_flags[60] = ContentLoader.LoadTexture("cx");
			GUIManager.tex_flags[61] = ContentLoader.LoadTexture("cy");
			GUIManager.tex_flags[62] = ContentLoader.LoadTexture("cz");
			GUIManager.tex_flags[63] = ContentLoader.LoadTexture("dj");
			GUIManager.tex_flags[64] = ContentLoader.LoadTexture("dk");
			GUIManager.tex_flags[65] = ContentLoader.LoadTexture("dm");
			GUIManager.tex_flags[66] = ContentLoader.LoadTexture("do");
			GUIManager.tex_flags[67] = ContentLoader.LoadTexture("dz");
			GUIManager.tex_flags[68] = ContentLoader.LoadTexture("ec");
			GUIManager.tex_flags[69] = ContentLoader.LoadTexture("eg");
			GUIManager.tex_flags[70] = ContentLoader.LoadTexture("eh");
			GUIManager.tex_flags[71] = ContentLoader.LoadTexture("er");
			GUIManager.tex_flags[72] = ContentLoader.LoadTexture("es");
			GUIManager.tex_flags[73] = ContentLoader.LoadTexture("et");
			GUIManager.tex_flags[74] = ContentLoader.LoadTexture("fi");
			GUIManager.tex_flags[75] = ContentLoader.LoadTexture("fj");
			GUIManager.tex_flags[76] = ContentLoader.LoadTexture("fk");
			GUIManager.tex_flags[77] = ContentLoader.LoadTexture("fm");
			GUIManager.tex_flags[78] = ContentLoader.LoadTexture("fo");
			GUIManager.tex_flags[79] = ContentLoader.LoadTexture("fr");
			GUIManager.tex_flags[80] = ContentLoader.LoadTexture("ga");
			GUIManager.tex_flags[81] = ContentLoader.LoadTexture("gb");
			GUIManager.tex_flags[82] = ContentLoader.LoadTexture("gd");
			GUIManager.tex_flags[83] = ContentLoader.LoadTexture("ge");
			GUIManager.tex_flags[84] = ContentLoader.LoadTexture("gf");
			GUIManager.tex_flags[85] = ContentLoader.LoadTexture("gh");
			GUIManager.tex_flags[86] = ContentLoader.LoadTexture("gi");
			GUIManager.tex_flags[87] = ContentLoader.LoadTexture("gl");
			GUIManager.tex_flags[88] = ContentLoader.LoadTexture("gm");
			GUIManager.tex_flags[89] = ContentLoader.LoadTexture("gn");
			GUIManager.tex_flags[90] = ContentLoader.LoadTexture("gp");
			GUIManager.tex_flags[91] = ContentLoader.LoadTexture("gq");
			GUIManager.tex_flags[92] = ContentLoader.LoadTexture("gr");
			GUIManager.tex_flags[93] = ContentLoader.LoadTexture("gs");
			GUIManager.tex_flags[94] = ContentLoader.LoadTexture("gt");
			GUIManager.tex_flags[95] = ContentLoader.LoadTexture("gu");
			GUIManager.tex_flags[96] = ContentLoader.LoadTexture("gw");
			GUIManager.tex_flags[97] = ContentLoader.LoadTexture("gy");
			GUIManager.tex_flags[98] = ContentLoader.LoadTexture("hk");
			GUIManager.tex_flags[99] = ContentLoader.LoadTexture("hm");
			GUIManager.tex_flags[100] = ContentLoader.LoadTexture("hn");
			GUIManager.tex_flags[101] = ContentLoader.LoadTexture("hr");
			GUIManager.tex_flags[102] = ContentLoader.LoadTexture("ht");
			GUIManager.tex_flags[103] = ContentLoader.LoadTexture("hu");
			GUIManager.tex_flags[104] = ContentLoader.LoadTexture("id");
			GUIManager.tex_flags[105] = ContentLoader.LoadTexture("ie");
			GUIManager.tex_flags[106] = ContentLoader.LoadTexture("il");
			GUIManager.tex_flags[107] = ContentLoader.LoadTexture("in");
			GUIManager.tex_flags[108] = ContentLoader.LoadTexture("io");
			GUIManager.tex_flags[109] = ContentLoader.LoadTexture("iq");
			GUIManager.tex_flags[110] = ContentLoader.LoadTexture("ir");
			GUIManager.tex_flags[111] = ContentLoader.LoadTexture("is");
			GUIManager.tex_flags[112] = ContentLoader.LoadTexture("it");
			GUIManager.tex_flags[113] = ContentLoader.LoadTexture("jm");
			GUIManager.tex_flags[114] = ContentLoader.LoadTexture("jo");
			GUIManager.tex_flags[115] = ContentLoader.LoadTexture("jp");
			GUIManager.tex_flags[116] = ContentLoader.LoadTexture("ke");
			GUIManager.tex_flags[117] = ContentLoader.LoadTexture("kg");
			GUIManager.tex_flags[118] = ContentLoader.LoadTexture("kh");
			GUIManager.tex_flags[119] = ContentLoader.LoadTexture("ki");
			GUIManager.tex_flags[120] = ContentLoader.LoadTexture("km");
			GUIManager.tex_flags[121] = ContentLoader.LoadTexture("kn");
			GUIManager.tex_flags[122] = ContentLoader.LoadTexture("kp");
			GUIManager.tex_flags[123] = ContentLoader.LoadTexture("kr");
			GUIManager.tex_flags[124] = ContentLoader.LoadTexture("kw");
			GUIManager.tex_flags[125] = ContentLoader.LoadTexture("ky");
			GUIManager.tex_flags[126] = ContentLoader.LoadTexture("la");
			GUIManager.tex_flags[127] = ContentLoader.LoadTexture("lb");
			GUIManager.tex_flags[128] = ContentLoader.LoadTexture("lc");
			GUIManager.tex_flags[129] = ContentLoader.LoadTexture("li");
			GUIManager.tex_flags[130] = ContentLoader.LoadTexture("lk");
			GUIManager.tex_flags[131] = ContentLoader.LoadTexture("lr");
			GUIManager.tex_flags[132] = ContentLoader.LoadTexture("ls");
			GUIManager.tex_flags[133] = ContentLoader.LoadTexture("lt");
			GUIManager.tex_flags[134] = ContentLoader.LoadTexture("lu");
			GUIManager.tex_flags[135] = ContentLoader.LoadTexture("ly");
			GUIManager.tex_flags[136] = ContentLoader.LoadTexture("ma");
			GUIManager.tex_flags[137] = ContentLoader.LoadTexture("mc");
			GUIManager.tex_flags[138] = ContentLoader.LoadTexture("me");
			GUIManager.tex_flags[139] = ContentLoader.LoadTexture("mg");
			GUIManager.tex_flags[140] = ContentLoader.LoadTexture("mh");
			GUIManager.tex_flags[141] = ContentLoader.LoadTexture("mk");
			GUIManager.tex_flags[142] = ContentLoader.LoadTexture("ml");
			GUIManager.tex_flags[143] = ContentLoader.LoadTexture("mm");
			GUIManager.tex_flags[144] = ContentLoader.LoadTexture("mn");
			GUIManager.tex_flags[145] = ContentLoader.LoadTexture("mo");
			GUIManager.tex_flags[146] = ContentLoader.LoadTexture("mp");
			GUIManager.tex_flags[147] = ContentLoader.LoadTexture("mq");
			GUIManager.tex_flags[148] = ContentLoader.LoadTexture("mr");
			GUIManager.tex_flags[149] = ContentLoader.LoadTexture("ms");
			GUIManager.tex_flags[150] = ContentLoader.LoadTexture("mt");
			GUIManager.tex_flags[151] = ContentLoader.LoadTexture("mu");
			GUIManager.tex_flags[152] = ContentLoader.LoadTexture("mv");
			GUIManager.tex_flags[153] = ContentLoader.LoadTexture("mw");
			GUIManager.tex_flags[154] = ContentLoader.LoadTexture("mx");
			GUIManager.tex_flags[155] = ContentLoader.LoadTexture("my");
			GUIManager.tex_flags[156] = ContentLoader.LoadTexture("mz");
			GUIManager.tex_flags[157] = ContentLoader.LoadTexture("na");
			GUIManager.tex_flags[158] = ContentLoader.LoadTexture("nc");
			GUIManager.tex_flags[159] = ContentLoader.LoadTexture("ne");
			GUIManager.tex_flags[160] = ContentLoader.LoadTexture("nf");
			GUIManager.tex_flags[161] = ContentLoader.LoadTexture("ng");
			GUIManager.tex_flags[162] = ContentLoader.LoadTexture("ni");
			GUIManager.tex_flags[163] = ContentLoader.LoadTexture("nl");
			GUIManager.tex_flags[164] = ContentLoader.LoadTexture("no");
			GUIManager.tex_flags[165] = ContentLoader.LoadTexture("np");
			GUIManager.tex_flags[166] = ContentLoader.LoadTexture("nr");
			GUIManager.tex_flags[167] = ContentLoader.LoadTexture("nu");
			GUIManager.tex_flags[168] = ContentLoader.LoadTexture("nz");
			GUIManager.tex_flags[169] = ContentLoader.LoadTexture("om");
			GUIManager.tex_flags[170] = ContentLoader.LoadTexture("pa");
			GUIManager.tex_flags[171] = ContentLoader.LoadTexture("pe");
			GUIManager.tex_flags[172] = ContentLoader.LoadTexture("pf");
			GUIManager.tex_flags[173] = ContentLoader.LoadTexture("pg");
			GUIManager.tex_flags[174] = ContentLoader.LoadTexture("ph");
			GUIManager.tex_flags[175] = ContentLoader.LoadTexture("pk");
			GUIManager.tex_flags[176] = ContentLoader.LoadTexture("pl");
			GUIManager.tex_flags[177] = ContentLoader.LoadTexture("pm");
			GUIManager.tex_flags[178] = ContentLoader.LoadTexture("pn");
			GUIManager.tex_flags[179] = ContentLoader.LoadTexture("pr");
			GUIManager.tex_flags[180] = ContentLoader.LoadTexture("ps");
			GUIManager.tex_flags[181] = ContentLoader.LoadTexture("pt");
			GUIManager.tex_flags[182] = ContentLoader.LoadTexture("pw");
			GUIManager.tex_flags[183] = ContentLoader.LoadTexture("py");
			GUIManager.tex_flags[184] = ContentLoader.LoadTexture("qa");
			GUIManager.tex_flags[185] = ContentLoader.LoadTexture("re");
			GUIManager.tex_flags[186] = ContentLoader.LoadTexture("ro");
			GUIManager.tex_flags[187] = ContentLoader.LoadTexture("rs");
			GUIManager.tex_flags[188] = ContentLoader.LoadTexture("rw");
			GUIManager.tex_flags[189] = ContentLoader.LoadTexture("sa");
			GUIManager.tex_flags[190] = ContentLoader.LoadTexture("sb");
			GUIManager.tex_flags[191] = ContentLoader.LoadTexture("sc");
			GUIManager.tex_flags[192] = ContentLoader.LoadTexture("sd");
			GUIManager.tex_flags[193] = ContentLoader.LoadTexture("se");
			GUIManager.tex_flags[194] = ContentLoader.LoadTexture("sg");
			GUIManager.tex_flags[195] = ContentLoader.LoadTexture("sh");
			GUIManager.tex_flags[196] = ContentLoader.LoadTexture("si");
			GUIManager.tex_flags[197] = ContentLoader.LoadTexture("sj");
			GUIManager.tex_flags[198] = ContentLoader.LoadTexture("sk");
			GUIManager.tex_flags[199] = ContentLoader.LoadTexture("sl");
			GUIManager.tex_flags[200] = ContentLoader.LoadTexture("sm");
			GUIManager.tex_flags[201] = ContentLoader.LoadTexture("sn");
			GUIManager.tex_flags[202] = ContentLoader.LoadTexture("so");
			GUIManager.tex_flags[203] = ContentLoader.LoadTexture("sr");
			GUIManager.tex_flags[204] = ContentLoader.LoadTexture("st");
			GUIManager.tex_flags[205] = ContentLoader.LoadTexture("sv");
			GUIManager.tex_flags[206] = ContentLoader.LoadTexture("sy");
			GUIManager.tex_flags[207] = ContentLoader.LoadTexture("sz");
			GUIManager.tex_flags[208] = ContentLoader.LoadTexture("tc");
			GUIManager.tex_flags[209] = ContentLoader.LoadTexture("td");
			GUIManager.tex_flags[210] = ContentLoader.LoadTexture("tf");
			GUIManager.tex_flags[211] = ContentLoader.LoadTexture("tg");
			GUIManager.tex_flags[212] = ContentLoader.LoadTexture("th");
			GUIManager.tex_flags[213] = ContentLoader.LoadTexture("tj");
			GUIManager.tex_flags[214] = ContentLoader.LoadTexture("tk");
			GUIManager.tex_flags[215] = ContentLoader.LoadTexture("tl");
			GUIManager.tex_flags[216] = ContentLoader.LoadTexture("tm");
			GUIManager.tex_flags[217] = ContentLoader.LoadTexture("tn");
			GUIManager.tex_flags[218] = ContentLoader.LoadTexture("to");
			GUIManager.tex_flags[219] = ContentLoader.LoadTexture("tr");
			GUIManager.tex_flags[220] = ContentLoader.LoadTexture("tt");
			GUIManager.tex_flags[221] = ContentLoader.LoadTexture("tv");
			GUIManager.tex_flags[222] = ContentLoader.LoadTexture("tw");
			GUIManager.tex_flags[223] = ContentLoader.LoadTexture("tz");
			GUIManager.tex_flags[224] = ContentLoader.LoadTexture("ug");
			GUIManager.tex_flags[225] = ContentLoader.LoadTexture("um");
			GUIManager.tex_flags[226] = ContentLoader.LoadTexture("uy");
			GUIManager.tex_flags[227] = ContentLoader.LoadTexture("uz");
			GUIManager.tex_flags[228] = ContentLoader.LoadTexture("va");
			GUIManager.tex_flags[229] = ContentLoader.LoadTexture("vc");
			GUIManager.tex_flags[230] = ContentLoader.LoadTexture("ve");
			GUIManager.tex_flags[231] = ContentLoader.LoadTexture("vg");
			GUIManager.tex_flags[232] = ContentLoader.LoadTexture("vi");
			GUIManager.tex_flags[233] = ContentLoader.LoadTexture("vn");
			GUIManager.tex_flags[234] = ContentLoader.LoadTexture("vu");
			GUIManager.tex_flags[235] = ContentLoader.LoadTexture("wf");
			GUIManager.tex_flags[236] = ContentLoader.LoadTexture("ws");
			GUIManager.tex_flags[237] = ContentLoader.LoadTexture("ye");
			GUIManager.tex_flags[238] = ContentLoader.LoadTexture("yt");
			GUIManager.tex_flags[239] = ContentLoader.LoadTexture("za");
			GUIManager.tex_flags[240] = ContentLoader.LoadTexture("zm");
			GUIManager.tex_flags[241] = ContentLoader.LoadTexture("zw");
			GUIManager.tex_button_mode = ContentLoader.LoadTexture("button_mode");
			GUIManager.tex_menubar = ContentLoader.LoadTexture("menubar");
			GUIManager.tex_warning = ContentLoader.LoadTexture("warningbar");
			GUIManager.tex_warning2 = ContentLoader.LoadTexture("warningbar2");
			GUIManager.tex_arrow = ContentLoader.LoadTexture("arrow");
			GUIManager.tex_coin = ContentLoader.LoadTexture("coin");
			GUIManager.tex_back_discount = ContentLoader.LoadTexture("discount_30");
			GUIManager.tex_bars = ContentLoader.LoadTexture("bars");
			GUIManager.tex_category = ContentLoader.LoadTexture("bar_category");
			GUIManager.tex_prazd_ZM = ContentLoader.LoadTexture("bar_category");
			GUIManager.tex_prazd_NY = ContentLoader.LoadTexture("bar_categoryNY");
			GUIManager.tex_prazd_HL = ContentLoader.LoadTexture("bar_categoryHL");
			GUIManager.tex_prazd_WWII = ContentLoader.LoadTexture("bar_categoryWWII");
			GUIManager.tex_prazd_LADY = ContentLoader.LoadTexture("bar_category");
			GUIManager.tex_buy_active = ContentLoader.LoadTexture("buy_active");
			GUIManager.tex_buy_blocked = ContentLoader.LoadTexture("buy_blocked");
			GUIManager.tex_weaponback = ContentLoader.LoadTexture("weaponback");
			GUIManager.tex_item_back = ContentLoader.LoadTexture("itemback");
			GUIManager.tex_item_back_ZM = ContentLoader.LoadTexture("itemback");
			GUIManager.tex_item_back_HL = ContentLoader.LoadTexture("itembackHL");
			GUIManager.tex_item_back_WWII = ContentLoader.LoadTexture("itembackWWII");
			GUIManager.tex_item_back_LADY = ContentLoader.LoadTexture("itemback");
			GUIManager.tex_item_back_NY = ContentLoader.LoadTexture("itembackNY");
			GUIManager.tex_item_back_discount = ContentLoader.LoadTexture("itemback_30");
			GUIManager.tex_item_back_new = ContentLoader.LoadTexture("itembackNEW");
			GUIManager.tex_item_back_lvl = ContentLoader.LoadTexture("itembackLVL");
			GUIManager.tex_playerback = ContentLoader.LoadTexture("playerback");
			GUIManager.tex_clock = ContentLoader.LoadTexture("clock");
			GUIManager.tex_atlas1 = ContentLoader.LoadTexture("atlas1");
			GUIManager.tex_atlas2 = ContentLoader.LoadTexture("atlas2");
			GUIManager.tex_atlas3 = ContentLoader.LoadTexture("atlas3");
			GUIManager.tex_atlas4 = ContentLoader.LoadTexture("atlas4");
			GUIManager.tex_atlas5 = ContentLoader.LoadTexture("atlas5");
			GUIManager.tex_proceed = ContentLoader.LoadTexture("proceedbar");
			GUIManager.tex_crossPrevBackground = ContentLoader.LoadTexture("background");
			GUIManager.tex_crossPalette = ContentLoader.LoadTexture("palette");
			GUIManager.NY2017REWARD = ContentLoader.LoadTexture("NY2017REWARD");
			GUIManager.VD2017REWARD = ContentLoader.LoadTexture("VD2017REWARD");
			GUIManager.NY2018REWARD = ContentLoader.LoadTexture("NY2018REWARD");
			GUIManager.tex_clan_exit = ContentLoader.LoadTexture("exit");
			GUIManager.tex_clan_find = ContentLoader.LoadTexture("buy_valid");
			GUIManager.tex_upgrade_bars = ContentLoader.LoadTexture("upgrade_bar");
			GUIManager.bar[0] = ContentLoader.LoadTexture("bar_top");
			GUIManager.bar[1] = ContentLoader.LoadTexture("bar_middle");
			GUIManager.bar[2] = ContentLoader.LoadTexture("bar_bottom");
			GUIManager.bar[3] = ContentLoader.LoadTexture("slider_normal");
			GUIManager.bar[4] = ContentLoader.LoadTexture("slider_active");
			GUIManager.bar[5] = ContentLoader.LoadTexture("slider_back");
			GUIManager.bar[6] = ContentLoader.LoadTexture("toggle_normal");
			GUIManager.bar[7] = ContentLoader.LoadTexture("toggle_active");
			GUIManager.tex_social = ContentLoader.LoadTexture("button_social");
			GUIManager.tex_crossline = ContentLoader.LoadTexture("crossline");
			GUIManager.tex_soundbar = ContentLoader.LoadTexture("soundbar");
			GUIManager.tex_sensbar = ContentLoader.LoadTexture("sensbar");
			GUIManager.loadingRoll = new TweenButton(GUIGS.NULL, new Vector2(0f, 0f), OFFSET_TYPE.FROM_CENTER, ContentLoader.LoadTexture("Loading"), null, null, null, null, null, null, 0f, 20f, 50, 64f, 64f, 0f);
			GUIManager.loadingRoll.tt.Add(TWEEN_TYPE.ROTATE);
			GUIManager.IsReady = true;
		}
	}

	// Token: 0x06000387 RID: 903 RVA: 0x000437E1 File Offset: 0x000419E1
	public static void DrawLoading()
	{
		GUIManager.loadingRoll.DrawButton(Vector2.zero);
	}

	// Token: 0x06000388 RID: 904 RVA: 0x000437F4 File Offset: 0x000419F4
	public static bool DrawButton(Rect r, Vector2 mpos, Color c, string label)
	{
		Texture2D image = null;
		if (r.width == 256f)
		{
			image = GUIManager.tex_button256;
		}
		else if (r.width == 128f)
		{
			image = GUIManager.tex_button128;
		}
		else if (r.width == 96f)
		{
			image = GUIManager.tex_button96;
		}
		else if (r.width == 22f || r.width == 20f)
		{
			image = GUIManager.tex_button22;
		}
		Color color = new Color(1f, 1f, 1f, 1f);
		bool result = false;
		bool flag = false;
		if (r.Contains(mpos))
		{
			flag = true;
		}
		if (!flag)
		{
			c *= 0.8f;
			color *= 0.8f;
		}
		GUI.color = c;
		GUI.DrawTexture(r, image);
		if (GUI.Button(r, "", GUIManager.gs_empty))
		{
			result = true;
		}
		GUI.color = new Color(0f, 0f, 0f, 1f);
		Rect position = r;
		float num = position.x;
		position.x = num + 1f;
		num = position.y;
		position.y = num + 1f;
		GUI.Label(position, label, GUIManager.gs_style1);
		GUI.color = color;
		GUI.Label(r, label, GUIManager.gs_style1);
		GUI.color = new Color(1f, 1f, 1f, 1f);
		return result;
	}

	// Token: 0x06000389 RID: 905 RVA: 0x0004395C File Offset: 0x00041B5C
	public static string DrawEdit(Rect r, Vector2 mpos, Color c, string label, int limit)
	{
		Texture2D image = null;
		if (r.width == 96f)
		{
			image = GUIManager.tex_edit96;
		}
		GUI.DrawTexture(r, image);
		return GUI.TextField(r, label, limit, GUIManager.gs_style1);
	}

	// Token: 0x0600038A RID: 906 RVA: 0x00043994 File Offset: 0x00041B94
	public static float DrawColorText(float x, float y, string text, TextAnchor align)
	{
		if (text == null)
		{
			return 0f;
		}
		if (text == "")
		{
			return 0f;
		}
		GUIManager.gs_style2.alignment = align;
		string[] array = text.Split(new char[]
		{
			'^'
		});
		float num = 0f;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null && !(array[i] == ""))
			{
				int num2;
				if (int.TryParse(array[i][0].ToString(), out num2))
				{
					if (num2 < 0 || num2 > 9)
					{
						num2 = 8;
					}
					array[i] = array[i].Substring(1, array[i].Length - 1);
				}
				else
				{
					num2 = 8;
				}
				Vector2 vector = GUIManager.gs_style2.CalcSize(new GUIContent(array[i]));
				GUIManager.gs_style2.normal.textColor = GUIManager.c[9];
				GUI.Label(new Rect(x + 1f, y + 1f, 256f, 20f), array[i], GUIManager.gs_style2);
				GUIManager.gs_style2.normal.textColor = GUIManager.c[num2];
				GUI.Label(new Rect(x, y, 256f, 20f), array[i], GUIManager.gs_style2);
				x += vector.x;
				num += vector.x;
			}
		}
		return num;
	}

	// Token: 0x0600038B RID: 907 RVA: 0x00043AF8 File Offset: 0x00041CF8
	public static void DrawText(Rect r, string text, int size, TextAnchor align, int color = 8)
	{
		TextAnchor alignment = GUIManager.gs_style3.alignment;
		int fontSize = GUIManager.gs_style3.fontSize;
		GUIManager.gs_style3.alignment = align;
		GUIManager.gs_style3.fontSize = size;
		GUIManager.gs_style3.normal.textColor = GUIManager.c[9];
		GUI.Label(new Rect(r.x + 1f, r.y + 1f, r.width, r.height), text, GUIManager.gs_style3);
		GUIManager.gs_style3.normal.textColor = GUIManager.c[color];
		GUI.Label(r, text, GUIManager.gs_style3);
		GUIManager.gs_style3.alignment = alignment;
		GUIManager.gs_style3.fontSize = fontSize;
	}

	// Token: 0x0600038C RID: 908 RVA: 0x00043BC4 File Offset: 0x00041DC4
	public static void DrawText2(Rect r, string text, int size, TextAnchor align, Color _c)
	{
		TextAnchor alignment = GUIManager.gs_style2.alignment;
		int fontSize = GUIManager.gs_style2.fontSize;
		GUIManager.gs_style2.alignment = align;
		GUIManager.gs_style2.fontSize = size;
		GUIManager.gs_style2.normal.textColor = GUIManager.c[9];
		GUI.Label(new Rect(r.x + 1f, r.y + 1f, r.width, r.height), text, GUIManager.gs_style2);
		GUIManager.gs_style2.normal.textColor = _c;
		GUI.Label(r, text, GUIManager.gs_style2);
		GUIManager.gs_style2.alignment = alignment;
		GUIManager.gs_style2.fontSize = fontSize;
	}

	// Token: 0x0600038D RID: 909 RVA: 0x00043C84 File Offset: 0x00041E84
	public static void DrawTextVsStyle(Rect r, string text, int _size, TextAnchor _align, Color _color, GUIStyle _gs)
	{
		TextAnchor alignment = _gs.alignment;
		int fontSize = _gs.fontSize;
		Color textColor = _gs.normal.textColor;
		_gs.alignment = _align;
		_gs.fontSize = _size;
		_gs.normal.textColor = Color.black;
		GUI.Label(new Rect(r.x + 1f, r.y + 1f, r.width, r.height), text, _gs);
		_gs.normal.textColor = _color;
		GUI.Label(r, text, _gs);
		_gs.alignment = alignment;
		_gs.fontSize = fontSize;
		_gs.normal.textColor = textColor;
	}

	// Token: 0x0600038E RID: 910 RVA: 0x00043D38 File Offset: 0x00041F38
	public static bool DrawButton2(Vector2 pos, Vector2 mpos, string label, int type)
	{
		Texture2D image = null;
		if (type == 0)
		{
			image = GUIManager.tex_button_blue;
		}
		else if (type == 1)
		{
			image = GUIManager.tex_button_green;
		}
		bool result = false;
		Rect rect = new Rect(pos.x, pos.y, 192f, 32f);
		GUI.DrawTexture(rect, image);
		if (GUI.Button(rect, "", GUIManager.gs_empty))
		{
			result = true;
		}
		GUIManager.DrawText(rect, label, 22, TextAnchor.MiddleCenter, 8);
		return result;
	}

	// Token: 0x0600038F RID: 911 RVA: 0x00043DA0 File Offset: 0x00041FA0
	public static void HSlider()
	{
		GUI.skin.horizontalSliderThumb.normal.background = GUIManager.bar[3];
		GUI.skin.horizontalSliderThumb.hover.background = GUIManager.bar[4];
		GUI.skin.horizontalSliderThumb.active.background = GUIManager.bar[4];
		GUI.skin.horizontalSlider.normal.background = GUIManager.bar[5];
		GUI.skin.horizontalSlider.fixedHeight = 12f;
	}

	// Token: 0x06000390 RID: 912 RVA: 0x00043E30 File Offset: 0x00042030
	public static void Toggle()
	{
		GUI.skin.toggle.normal.background = GUIManager.bar[6];
		GUI.skin.toggle.onNormal.background = GUIManager.bar[7];
		GUI.skin.toggle.hover.background = GUIManager.bar[6];
		GUI.skin.toggle.onHover.background = GUIManager.bar[7];
		GUI.skin.toggle.active.background = GUIManager.bar[7];
		GUI.skin.toggle.onActive.background = GUIManager.bar[7];
	}

	// Token: 0x06000391 RID: 913 RVA: 0x00043EE0 File Offset: 0x000420E0
	public static Vector2 BeginScrollView(Rect viewzone, Vector2 scrollViewVector, Rect scrollzone)
	{
		GUI.skin.verticalScrollbar.normal.background = null;
		GUI.skin.verticalScrollbarThumb.normal.background = null;
		scrollViewVector = GUI.BeginScrollView(viewzone, scrollViewVector, scrollzone);
		float height = viewzone.height / scrollzone.height * viewzone.height;
		float num = scrollViewVector.y / scrollzone.height * viewzone.height;
		if (scrollzone.height <= viewzone.height)
		{
			GUIManager.rbar.height = 0f;
		}
		else
		{
			GUIManager.rbar = new Rect(viewzone.x + viewzone.width - 16f, viewzone.y + num, 16f, height);
		}
		return scrollViewVector;
	}

	// Token: 0x06000392 RID: 914 RVA: 0x00043FA0 File Offset: 0x000421A0
	public static void EndScrollView()
	{
		GUI.EndScrollView();
		GUIManager.DrawBar(GUIManager.rbar);
	}

	// Token: 0x06000393 RID: 915 RVA: 0x00043FB4 File Offset: 0x000421B4
	public static void DrawBar(Rect r)
	{
		if (r.height == 0f)
		{
			return;
		}
		GUI.DrawTexture(new Rect(r.x, r.y, r.width, 4f), GUIManager.bar[0]);
		if (r.height - 8f > 0f)
		{
			GUI.DrawTexture(new Rect(r.x, r.y + 4f, r.width, r.height - 8f), GUIManager.bar[1]);
		}
		GUI.DrawTexture(new Rect(r.x, r.y + r.height - 4f, r.width, 4f), GUIManager.bar[2]);
	}

	// Token: 0x040006FA RID: 1786
	public static bool IsReady = false;

	// Token: 0x040006FB RID: 1787
	public static Texture2D tex_black;

	// Token: 0x040006FC RID: 1788
	public static Texture2D tex_half_black;

	// Token: 0x040006FD RID: 1789
	public static Texture2D tex_red;

	// Token: 0x040006FE RID: 1790
	public static Texture2D tex_half_red;

	// Token: 0x040006FF RID: 1791
	public static Texture2D tex_green;

	// Token: 0x04000700 RID: 1792
	public static Texture2D tex_half_green;

	// Token: 0x04000701 RID: 1793
	public static Texture2D tex_blue;

	// Token: 0x04000702 RID: 1794
	public static Texture2D tex_half_blue;

	// Token: 0x04000703 RID: 1795
	public static Texture2D tex_yellow;

	// Token: 0x04000704 RID: 1796
	public static Texture2D tex_half_yellow;

	// Token: 0x04000705 RID: 1797
	public static Texture2D tex_button256;

	// Token: 0x04000706 RID: 1798
	public static Texture2D tex_button128;

	// Token: 0x04000707 RID: 1799
	public static Texture2D tex_button96;

	// Token: 0x04000708 RID: 1800
	public static Texture2D tex_button22;

	// Token: 0x04000709 RID: 1801
	public static Texture2D tex_edit96;

	// Token: 0x0400070A RID: 1802
	public static Texture2D tex_padlock;

	// Token: 0x0400070B RID: 1803
	public static Texture2D tex_bar_blue;

	// Token: 0x0400070C RID: 1804
	public static Texture2D tex_bar_yellow;

	// Token: 0x0400070D RID: 1805
	public static Texture2D tex_face_icon;

	// Token: 0x0400070E RID: 1806
	public static Texture2D tex_edit_icon;

	// Token: 0x0400070F RID: 1807
	public static Texture2D tex_save_icon;

	// Token: 0x04000710 RID: 1808
	public static Texture2D tex_icon_load;

	// Token: 0x04000711 RID: 1809
	public static Texture2D tex_icon_premium;

	// Token: 0x04000712 RID: 1810
	public static Texture2D tex_menu_back;

	// Token: 0x04000713 RID: 1811
	public static Texture2D logo;

	// Token: 0x04000714 RID: 1812
	public static Texture2D prazd_logo;

	// Token: 0x04000715 RID: 1813
	public static Texture2D under_logo;

	// Token: 0x04000716 RID: 1814
	public static Texture2D tex_sound_off;

	// Token: 0x04000717 RID: 1815
	public static Texture2D tex_sound_on;

	// Token: 0x04000718 RID: 1816
	public static Texture2D tex_exit;

	// Token: 0x04000719 RID: 1817
	public static Texture2D tex_help;

	// Token: 0x0400071A RID: 1818
	public static Texture2D tex_fullscreen;

	// Token: 0x0400071B RID: 1819
	public static Texture2D tex_menu_start;

	// Token: 0x0400071C RID: 1820
	public static Texture2D tex_menu_option;

	// Token: 0x0400071D RID: 1821
	public static Texture2D tex_menu_profile;

	// Token: 0x0400071E RID: 1822
	public static Texture2D tex_menu_shop;

	// Token: 0x0400071F RID: 1823
	public static Texture2D tex_menu_playerstats;

	// Token: 0x04000720 RID: 1824
	public static Texture2D tex_menu_inv;

	// Token: 0x04000721 RID: 1825
	public static Texture2D tex_menu_clan;

	// Token: 0x04000722 RID: 1826
	public static Texture2D tex_menu_master;

	// Token: 0x04000723 RID: 1827
	public static Texture2D tex_button;

	// Token: 0x04000724 RID: 1828
	public static Texture2D tex_button_red;

	// Token: 0x04000725 RID: 1829
	public static Texture2D tex_button_hover;

	// Token: 0x04000726 RID: 1830
	public static Texture2D tex_loading;

	// Token: 0x04000727 RID: 1831
	public static Texture2D tex_menu_social;

	// Token: 0x04000728 RID: 1832
	public static Texture2D tex_menu_zadanie;

	// Token: 0x04000729 RID: 1833
	public static Texture2D tex_top_plus;

	// Token: 0x0400072A RID: 1834
	public static Texture2D tex_top_plus2;

	// Token: 0x0400072B RID: 1835
	public static Texture2D tex_panel;

	// Token: 0x0400072C RID: 1836
	public static Texture2D tex_select;

	// Token: 0x0400072D RID: 1837
	public static Texture2D tex_hover;

	// Token: 0x0400072E RID: 1838
	public static Texture2D tex_server;

	// Token: 0x0400072F RID: 1839
	public static Texture2D tex_server_hover;

	// Token: 0x04000730 RID: 1840
	public static Texture2D tex_hint;

	// Token: 0x04000731 RID: 1841
	public static Texture2D gm0;

	// Token: 0x04000732 RID: 1842
	public static Texture2D gm1;

	// Token: 0x04000733 RID: 1843
	public static Texture2D gm2;

	// Token: 0x04000734 RID: 1844
	public static Texture2D gm3;

	// Token: 0x04000735 RID: 1845
	public static Texture2D gm4;

	// Token: 0x04000736 RID: 1846
	public static Texture2D gm5;

	// Token: 0x04000737 RID: 1847
	public static Texture2D gm6;

	// Token: 0x04000738 RID: 1848
	public static Texture2D gm7;

	// Token: 0x04000739 RID: 1849
	public static Texture2D gm8;

	// Token: 0x0400073A RID: 1850
	public static Texture2D gm9;

	// Token: 0x0400073B RID: 1851
	public static Texture2D gm10;

	// Token: 0x0400073C RID: 1852
	public static Texture2D gm11;

	// Token: 0x0400073D RID: 1853
	public static Texture2D gm12;

	// Token: 0x0400073E RID: 1854
	public static Texture2D gm13;

	// Token: 0x0400073F RID: 1855
	public static Texture2D tex_warning;

	// Token: 0x04000740 RID: 1856
	public static Texture2D tex_warning2;

	// Token: 0x04000741 RID: 1857
	public static Texture2D pro;

	// Token: 0x04000742 RID: 1858
	public static Texture2D select_glow;

	// Token: 0x04000743 RID: 1859
	public static Texture2D hover_glow;

	// Token: 0x04000744 RID: 1860
	public static Texture2D hover_part_glow;

	// Token: 0x04000745 RID: 1861
	public static Texture2D[] tex_flags = new Texture2D[245];

	// Token: 0x04000746 RID: 1862
	public static Texture2D[] tex_flags_filter = new Texture2D[3];

	// Token: 0x04000747 RID: 1863
	public static Texture2D[] tex_gold_variants = new Texture2D[10];

	// Token: 0x04000748 RID: 1864
	public static Texture2D tex_button_mode;

	// Token: 0x04000749 RID: 1865
	public static Texture2D tex_menubar;

	// Token: 0x0400074A RID: 1866
	public static Texture2D tex_arrow;

	// Token: 0x0400074B RID: 1867
	public static Texture2D tex_coin;

	// Token: 0x0400074C RID: 1868
	public static Texture2D tex_premium_big;

	// Token: 0x0400074D RID: 1869
	public static Texture2D tex_category;

	// Token: 0x0400074E RID: 1870
	public static Texture2D tex_prazd_ZM;

	// Token: 0x0400074F RID: 1871
	public static Texture2D tex_prazd_NY;

	// Token: 0x04000750 RID: 1872
	public static Texture2D tex_prazd_HL;

	// Token: 0x04000751 RID: 1873
	public static Texture2D tex_prazd_WWII;

	// Token: 0x04000752 RID: 1874
	public static Texture2D tex_prazd_LADY;

	// Token: 0x04000753 RID: 1875
	public static Texture2D tex_buy_active;

	// Token: 0x04000754 RID: 1876
	public static Texture2D tex_buy_blocked;

	// Token: 0x04000755 RID: 1877
	public static Texture2D tex_back_discount;

	// Token: 0x04000756 RID: 1878
	public static Texture2D tex_megapack;

	// Token: 0x04000757 RID: 1879
	public static Texture2D tex_weaponback;

	// Token: 0x04000758 RID: 1880
	public static Texture2D tex_bars;

	// Token: 0x04000759 RID: 1881
	public static Texture2D tex_item_back;

	// Token: 0x0400075A RID: 1882
	public static Texture2D tex_item_back_ZM;

	// Token: 0x0400075B RID: 1883
	public static Texture2D tex_item_back_NY;

	// Token: 0x0400075C RID: 1884
	public static Texture2D tex_item_back_HL;

	// Token: 0x0400075D RID: 1885
	public static Texture2D tex_item_back_WWII;

	// Token: 0x0400075E RID: 1886
	public static Texture2D tex_item_back_LADY;

	// Token: 0x0400075F RID: 1887
	public static Texture2D tex_item_back_discount;

	// Token: 0x04000760 RID: 1888
	public static Texture2D tex_item_back_new;

	// Token: 0x04000761 RID: 1889
	public static Texture2D tex_item_back_lvl;

	// Token: 0x04000762 RID: 1890
	public static Texture2D tex_item_select;

	// Token: 0x04000763 RID: 1891
	public static Texture2D tex_item_open;

	// Token: 0x04000764 RID: 1892
	public static Texture2D tex_playerback;

	// Token: 0x04000765 RID: 1893
	public static Texture2D tex_clock;

	// Token: 0x04000766 RID: 1894
	public static Texture2D tex_atlas1;

	// Token: 0x04000767 RID: 1895
	public static Texture2D tex_atlas2;

	// Token: 0x04000768 RID: 1896
	public static Texture2D tex_atlas3;

	// Token: 0x04000769 RID: 1897
	public static Texture2D tex_atlas4;

	// Token: 0x0400076A RID: 1898
	public static Texture2D tex_atlas5;

	// Token: 0x0400076B RID: 1899
	public static Texture2D tex_proceed;

	// Token: 0x0400076C RID: 1900
	public static Texture2D tex_crossPrevBackground;

	// Token: 0x0400076D RID: 1901
	public static Texture2D tex_crossPalette;

	// Token: 0x0400076E RID: 1902
	public static Texture2D NY2017REWARD;

	// Token: 0x0400076F RID: 1903
	public static Texture2D VD2017REWARD;

	// Token: 0x04000770 RID: 1904
	public static Texture2D NY2018REWARD;

	// Token: 0x04000771 RID: 1905
	public static Texture2D tex_clan_manage;

	// Token: 0x04000772 RID: 1906
	public static Texture2D tex_clan_exit;

	// Token: 0x04000773 RID: 1907
	public static Texture2D tex_clan_accept;

	// Token: 0x04000774 RID: 1908
	public static Texture2D tex_clan_decline;

	// Token: 0x04000775 RID: 1909
	public static Texture2D tex_clan_delete;

	// Token: 0x04000776 RID: 1910
	public static Texture2D tex_clan_invite;

	// Token: 0x04000777 RID: 1911
	public static Texture2D tex_clan_find;

	// Token: 0x04000778 RID: 1912
	public static Texture2D[] tex_upgrade = new Texture2D[5];

	// Token: 0x04000779 RID: 1913
	public static Texture2D[] tex_upgrade_active = new Texture2D[5];

	// Token: 0x0400077A RID: 1914
	public static Texture2D[] tex_upgrade_vehicle = new Texture2D[6];

	// Token: 0x0400077B RID: 1915
	public static Texture2D[] tex_upgrade_vehicle_active = new Texture2D[6];

	// Token: 0x0400077C RID: 1916
	public static Texture2D tex_upgrade_bars;

	// Token: 0x0400077D RID: 1917
	public static Texture2D tex_social;

	// Token: 0x0400077E RID: 1918
	public static Texture2D tex_action_banner;

	// Token: 0x0400077F RID: 1919
	public static Texture2D tex_bonus;

	// Token: 0x04000780 RID: 1920
	public static Texture2D tex_discount;

	// Token: 0x04000781 RID: 1921
	public static Texture2D tex_crossline;

	// Token: 0x04000782 RID: 1922
	public static Texture2D tex_soundbar;

	// Token: 0x04000783 RID: 1923
	public static Texture2D tex_sensbar;

	// Token: 0x04000784 RID: 1924
	public static Texture2D tex_fb;

	// Token: 0x04000785 RID: 1925
	public static Texture2D tex_kg;

	// Token: 0x04000786 RID: 1926
	public static Texture2D tex_mm;

	// Token: 0x04000787 RID: 1927
	public static Texture2D tex_nl;

	// Token: 0x04000788 RID: 1928
	public static Texture2D tex_ok;

	// Token: 0x04000789 RID: 1929
	public static Texture2D tex_st;

	// Token: 0x0400078A RID: 1930
	public static Texture2D tex_vk;

	// Token: 0x0400078B RID: 1931
	public static Texture2D tex_kr;

	// Token: 0x0400078C RID: 1932
	public static GUIStyle gs_empty;

	// Token: 0x0400078D RID: 1933
	public static GUIStyle gs_style1;

	// Token: 0x0400078E RID: 1934
	public static GUIStyle gs_style2;

	// Token: 0x0400078F RID: 1935
	public static GUIStyle gs_style3;

	// Token: 0x04000790 RID: 1936
	public static Color[] c = new Color[10];

	// Token: 0x04000791 RID: 1937
	public static Texture2D tex_button_blue;

	// Token: 0x04000792 RID: 1938
	public static Texture2D tex_button_green;

	// Token: 0x04000793 RID: 1939
	private static Texture2D[] bar = new Texture2D[10];

	// Token: 0x04000794 RID: 1940
	private static Rect rbar;

	// Token: 0x04000795 RID: 1941
	public static Texture2D start_back;

	// Token: 0x04000796 RID: 1942
	public static Texture2D day_back;

	// Token: 0x04000797 RID: 1943
	public static Texture2D week_back;

	// Token: 0x04000798 RID: 1944
	public static Texture2D level_back;

	// Token: 0x04000799 RID: 1945
	private static GameObject goPlayer;

	// Token: 0x0400079A RID: 1946
	private static GameObject goMainCamera;

	// Token: 0x0400079B RID: 1947
	private static float lastwidth = 0f;

	// Token: 0x0400079C RID: 1948
	private static TweenButton loadingRoll;
}
