using System;
using UnityEngine;

// Token: 0x02000046 RID: 70
public class SoundManager
{
	// Token: 0x06000225 RID: 549 RVA: 0x0002A150 File Offset: 0x00028350
	public static void Init()
	{
		SoundManager.SoundHover = ContentLoader.LoadSound("onbutton");
		SoundManager.SoundClick = ContentLoader.LoadSound("clickbutton");
		SoundManager.snaryadFly = ContentLoader.LoadSound("snaryadFly");
		SoundManager.weapon_tank = ContentLoader.LoadSound("tank_shot");
		SoundManager.TankStand = ContentLoader.LoadSound("tank_generic");
		SoundManager.TankMove = ContentLoader.LoadSound("tank_drive");
		SoundManager.TankStart = ContentLoader.LoadSound("tank_start");
		SoundManager.TankStop = ContentLoader.LoadSound("tank_stop");
		SoundManager.TankEnter = ContentLoader.LoadSound("tank_enter");
		SoundManager.TankTurretMove = ContentLoader.LoadSound("tank_turret_rotation_loop");
		SoundManager.TankTurretStart = ContentLoader.LoadSound("tank_turret_rotation_start");
		SoundManager.TankTurretStop = ContentLoader.LoadSound("tank_turret_rotation_end");
		SoundManager.TankZoom = ContentLoader.LoadSound("tank_zoom");
		SoundManager.TankHit = ContentLoader.LoadSound("tank_hit");
		SoundManager.TankDeath = ContentLoader.LoadSound("tank_death");
		SoundManager.JeepStand = ContentLoader.LoadSound("hammer_idle");
		SoundManager.JeepMove = ContentLoader.LoadSound("hammer_drive");
		SoundManager.JeepStart = ContentLoader.LoadSound("hammer_start");
		SoundManager.JeepStop = ContentLoader.LoadSound("hammer_stop");
		SoundManager.weapon_block = ContentLoader.LoadSound("block");
		SoundManager.weapon_shovel = ContentLoader.LoadSound("shovel");
		SoundManager.weapon_m3 = ContentLoader.LoadSound("m3");
		SoundManager.weapon_m14 = ContentLoader.LoadSound("m14");
		SoundManager.weapon_mp5 = ContentLoader.LoadSound("mp5");
		SoundManager.weapon_ak47 = ContentLoader.LoadSound("ak47");
		SoundManager.weapon_svd = ContentLoader.LoadSound("svd");
		SoundManager.weapon_glock = ContentLoader.LoadSound("glock");
		SoundManager.weapon_deagle = ContentLoader.LoadSound("deagle");
		SoundManager.weapon_shmel = ContentLoader.LoadSound("shmel");
		SoundManager.weapon_asval = ContentLoader.LoadSound("asval");
		SoundManager.weapon_g36c = ContentLoader.LoadSound("g36c");
		SoundManager.weapon_kriss = ContentLoader.LoadSound("kriss");
		SoundManager.weapon_m4a1 = ContentLoader.LoadSound("m4a1");
		SoundManager.weapon_vsk94 = ContentLoader.LoadSound("vsk");
		SoundManager.weapon_m249 = ContentLoader.LoadSound("m249");
		SoundManager.weapon_spas12 = ContentLoader.LoadSound("sps12");
		SoundManager.weapon_vintorez = ContentLoader.LoadSound("vintorez");
		SoundManager.weapon_usp = ContentLoader.LoadSound("usp");
		SoundManager.weapon_barrett = ContentLoader.LoadSound("barrett");
		SoundManager.weapon_tmp = ContentLoader.LoadSound("tmp");
		SoundManager.weapon_auga3 = ContentLoader.LoadSound("aug");
		SoundManager.weapon_sg552 = ContentLoader.LoadSound("sg552");
		SoundManager.weapon_l96a1 = ContentLoader.LoadSound("L96");
		SoundManager.weapon_nova = ContentLoader.LoadSound("nova");
		SoundManager.weapon_kord = ContentLoader.LoadSound("kord");
		SoundManager.weapon_anaconda = ContentLoader.LoadSound("anaconda");
		SoundManager.weapon_scar = ContentLoader.LoadSound("scar");
		SoundManager.weapon_p90 = ContentLoader.LoadSound("p90");
		SoundManager.weapon_rpk = ContentLoader.LoadSound("rpk");
		SoundManager.weapon_hk416 = ContentLoader.LoadSound("hk416");
		SoundManager.weapon_ak102 = ContentLoader.LoadSound("ak102");
		SoundManager.weapon_sr25 = ContentLoader.LoadSound("sr25");
		SoundManager.weapon_mosin = ContentLoader.LoadSound("mosin");
		SoundManager.weapon_ppsh = ContentLoader.LoadSound("ppsh_fire");
		SoundManager.weapon_mp40 = ContentLoader.LoadSound("mp40");
		SoundManager.weapon_l96a1mod = ContentLoader.LoadSound("l96a1mod");
		SoundManager.weapon_kacpdw = ContentLoader.LoadSound("kac_pdw1");
		SoundManager.weapon_famas = ContentLoader.LoadSound("famas");
		SoundManager.weapon_beretta = ContentLoader.LoadSound("beretta");
		SoundManager.weapon_rpg = ContentLoader.LoadSound("rpg7");
		SoundManager.weapon_repair_tool = ContentLoader.LoadSound("RepairTool");
		SoundManager.weapon_aa12 = ContentLoader.LoadSound("AA12");
		SoundManager.weapon_fn57 = ContentLoader.LoadSound("Five-seveN");
		SoundManager.weapon_fs2000 = ContentLoader.LoadSound("FS2000");
		SoundManager.weapon_l85 = ContentLoader.LoadSound("L85");
		SoundManager.weapon_mac10 = ContentLoader.LoadSound("MAC10");
		SoundManager.weapon_pkp = ContentLoader.LoadSound("PKalashnikov");
		SoundManager.weapon_pm = ContentLoader.LoadSound("PM");
		SoundManager.weapon_tar21 = ContentLoader.LoadSound("TAR21");
		SoundManager.weapon_ump45 = ContentLoader.LoadSound("UMP45");
		SoundManager.weapon_ntw20 = ContentLoader.LoadSound("AT_Rifle1");
		SoundManager.weapon_minigun = ContentLoader.LoadSound("minigun_shot");
		SoundManager.weapon_javelin = ContentLoader.LoadSound("javelin_shot");
		SoundManager.weapon_minefly = ContentLoader.LoadSound("mortar");
		SoundManager.weapon_mglmk1 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_L96A1GOLD = ContentLoader.LoadSound("L96");
		SoundManager.weapon_SVT40 = ContentLoader.LoadSound("svt");
		SoundManager.weapon_SVUAS = ContentLoader.LoadSound("svuas");
		SoundManager.weapon_PSG1GOLD = ContentLoader.LoadSound("psg_1");
		SoundManager.weapon_M110 = ContentLoader.LoadSound("m110");
		SoundManager.weapon_KEDR = ContentLoader.LoadSound("pp91");
		SoundManager.weapon_MAC10GOLD = ContentLoader.LoadSound("MAC10");
		SoundManager.weapon_KASHTAN = ContentLoader.LoadSound("aek919k");
		SoundManager.weapon_FMG9GOLD = ContentLoader.LoadSound("fmg9");
		SoundManager.weapon_MP7 = ContentLoader.LoadSound("mp7");
		SoundManager.weapon_PKPGOLD = ContentLoader.LoadSound("PKalashnikov");
		SoundManager.weapon_NEGEV = ContentLoader.LoadSound("negev");
		SoundManager.weapon_XM8 = ContentLoader.LoadSound("xm8");
		SoundManager.weapon_AK74 = ContentLoader.LoadSound("ak74");
		SoundManager.weapon_AK47GOLD = ContentLoader.LoadSound("ak47");
		SoundManager.weapon_M4A1GOLD = ContentLoader.LoadSound("m4a1");
		SoundManager.weapon_ABAKAN = ContentLoader.LoadSound("ah94");
		SoundManager.weapon_AK103 = ContentLoader.LoadSound("ak103");
		SoundManager.weapon_BEKAS12M = ContentLoader.LoadSound("bekas");
		SoundManager.weapon_MOSSBERG500 = ContentLoader.LoadSound("ms500");
		SoundManager.weapon_NOVAGOLD = ContentLoader.LoadSound("nova");
		SoundManager.weapon_NEOSTEAD2000 = ContentLoader.LoadSound("ns2000");
		SoundManager.weapon_HONCHO = ContentLoader.LoadSound("triplehoncho");
		SoundManager.weapon_M2 = ContentLoader.LoadSound("m2");
		SoundManager.weapon_M4 = ContentLoader.LoadSound("m4");
		SoundManager.weapon_SAIGAGOLD = ContentLoader.LoadSound("saiga");
		SoundManager.weapon_BROWNING = ContentLoader.LoadSound("hp");
		SoundManager.weapon_WALTHER = ContentLoader.LoadSound("p99");
		SoundManager.weapon_CZ75 = ContentLoader.LoadSound("cz75");
		SoundManager.weapon_SWM29 = ContentLoader.LoadSound("m29");
		SoundManager.weapon_UZI = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_M1887 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_REMINGTON870 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_VEPR12 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_P226 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_QBU88 = ContentLoader.LoadSound("grenade_launcher_shot");
		SoundManager.weapon_DEAGLEGOLD = ContentLoader.LoadSound("deagle");
		SoundManager.Shovel_Stoneend = ContentLoader.LoadSound("stoneend");
		SoundManager.Shovel_Stone = ContentLoader.LoadSound("stone");
		SoundManager.Shovel_Wood = ContentLoader.LoadSound("wood");
		SoundManager.Shovel_Brick = ContentLoader.LoadSound("brick");
		SoundManager.Shovel_Leaf = ContentLoader.LoadSound("leaf");
		SoundManager.Hit = ContentLoader.LoadSound("hit");
		SoundManager.HitHelmet = ContentLoader.LoadSound("helmet_hit");
		SoundManager.HitHelmet_trace = ContentLoader.LoadSound("helmet_tracehit");
		SoundManager.shield_hit = ContentLoader.LoadSound("shield_hit");
		SoundManager.snow_hit = ContentLoader.LoadSound("snowball_hit");
		SoundManager.Walk = ContentLoader.LoadSound("walk");
		SoundManager.SnowWalk = ContentLoader.LoadSound("snow_walk");
		SoundManager.StoneWalk = ContentLoader.LoadSound("stone_walk");
		SoundManager.MetallWalk = ContentLoader.LoadSound("metall_walk");
		SoundManager.WaterWalk = ContentLoader.LoadSound("water_walk");
		SoundManager.WoodWalk = ContentLoader.LoadSound("wood_walk");
		SoundManager.TankMG = ContentLoader.LoadSound("Tank_MG");
		SoundManager.TankRepairKit = ContentLoader.LoadSound("tank_repairbox");
		SoundManager.Zoom = ContentLoader.LoadSound("zoom");
		SoundManager.ZM_Ambient = ContentLoader.LoadSound("zombie_ingame");
		SoundManager.ZM_Infected = ContentLoader.LoadSound("zombie_infected");
		SoundManager.Death = ContentLoader.LoadSound("death");
		SoundManager.TNT = ContentLoader.LoadSound("tnt_activate");
		SoundManager.gplauncher = ContentLoader.LoadSound("rifleLauncher");
		SoundManager.javelinTargeting = ContentLoader.LoadSound("javelin_lock");
		SoundManager.minigunMotor = ContentLoader.LoadSound("minigun_motor");
		SoundManager.weapon_reload = ContentLoader.LoadSound("reload");
		SoundManager.weapon_noammo = ContentLoader.LoadSound("noammo");
		SoundManager.mine_fly = ContentLoader.LoadSound("rocket_fly1");
		SoundManager.molotov_fly = ContentLoader.LoadSound("molotov_fly");
		SoundManager.molotov_birn = ContentLoader.LoadSound("molotov_burn");
		SoundManager.molotov_explosion = ContentLoader.LoadSound("molotov_explosion");
		SoundManager.select = ContentLoader.LoadSound("select");
		SoundManager.weapon_mauzer = ContentLoader.LoadSound("mauser");
		SoundManager.weapon_crossbow = ContentLoader.LoadSound("crossbow");
		SoundManager.weapon_qbz95 = ContentLoader.LoadSound("qbz95");
		SoundManager.weapon_aksu = ContentLoader.LoadSound("aksu");
		SoundManager.weapon_m700 = ContentLoader.LoadSound("m700");
		SoundManager.weapon_stechkin = ContentLoader.LoadSound("stechkin");
		SoundManager.weapon_dpm = ContentLoader.LoadSound("dpm");
		SoundManager.weapon_m1924 = ContentLoader.LoadSound("m1924");
		SoundManager.weapon_mg42 = ContentLoader.LoadSound("mg42");
		SoundManager.weapon_sten = ContentLoader.LoadSound("sten");
		SoundManager.weapon_m1a1 = ContentLoader.LoadSound("m1a1");
		SoundManager.weapon_type99 = ContentLoader.LoadSound("type99");
		SoundManager.SmokeGrenade = ContentLoader.LoadSound("smoke_grenade");
		SoundManager.mine_place = ContentLoader.LoadSound("mine_place");
		SoundManager.c4_detonator = ContentLoader.LoadSound("c4_detonator");
		SoundManager.FlashLaunch = ContentLoader.LoadSound("module_flares");
		SoundManager.javelinAIM = ContentLoader.LoadSound("tank_rocket_aim");
		SoundManager.javelinMissleAIM = ContentLoader.LoadSound("tank_rocket_indicator");
		SoundManager.weapon_bizon = ContentLoader.LoadSound("bizon");
		SoundManager.weapon_groza = ContentLoader.LoadSound("groza");
		SoundManager.weapon_jackhammer = ContentLoader.LoadSound("jackhammer");
		SoundManager.weapon_pila = ContentLoader.LoadSound("pila");
		SoundManager.weapon_psg_1 = ContentLoader.LoadSound("psg_1");
		SoundManager.weapon_krytac = ContentLoader.LoadSound("krytac");
		SoundManager.weapon_mp5sd = ContentLoader.LoadSound("mp5sd");
		SoundManager.weapon_colts = ContentLoader.LoadSound("colts");
		SoundManager.weapon_magnum_lady = ContentLoader.LoadSound("magnum_lady");
		SoundManager.weapon_scorpion = ContentLoader.LoadSound("scorpion");
		SoundManager.weapon_fmg9 = ContentLoader.LoadSound("fmg9");
		SoundManager.weapon_saiga = ContentLoader.LoadSound("saiga");
		SoundManager.weapon_flamethrower = ContentLoader.LoadSound("flamethrower");
		SoundManager.weapon_flamethrower_start = ContentLoader.LoadSound("flamethrower_start");
		SoundManager.weapon_flamethrower_end = ContentLoader.LoadSound("flamethrower_end");
	}

	// Token: 0x0400022B RID: 555
	public static AudioClip Soundtrack;

	// Token: 0x0400022C RID: 556
	public static AudioClip SoundHover;

	// Token: 0x0400022D RID: 557
	public static AudioClip SoundClick;

	// Token: 0x0400022E RID: 558
	public static AudioClip weapon_block;

	// Token: 0x0400022F RID: 559
	public static AudioClip weapon_shovel;

	// Token: 0x04000230 RID: 560
	public static AudioClip weapon_m3;

	// Token: 0x04000231 RID: 561
	public static AudioClip weapon_m14;

	// Token: 0x04000232 RID: 562
	public static AudioClip weapon_mp5;

	// Token: 0x04000233 RID: 563
	public static AudioClip weapon_ak47;

	// Token: 0x04000234 RID: 564
	public static AudioClip weapon_svd;

	// Token: 0x04000235 RID: 565
	public static AudioClip weapon_glock;

	// Token: 0x04000236 RID: 566
	public static AudioClip weapon_deagle;

	// Token: 0x04000237 RID: 567
	public static AudioClip weapon_shmel;

	// Token: 0x04000238 RID: 568
	public static AudioClip weapon_asval;

	// Token: 0x04000239 RID: 569
	public static AudioClip weapon_g36c;

	// Token: 0x0400023A RID: 570
	public static AudioClip weapon_kriss;

	// Token: 0x0400023B RID: 571
	public static AudioClip weapon_m4a1;

	// Token: 0x0400023C RID: 572
	public static AudioClip weapon_vsk94;

	// Token: 0x0400023D RID: 573
	public static AudioClip weapon_m249;

	// Token: 0x0400023E RID: 574
	public static AudioClip weapon_spas12;

	// Token: 0x0400023F RID: 575
	public static AudioClip weapon_vintorez;

	// Token: 0x04000240 RID: 576
	public static AudioClip weapon_usp;

	// Token: 0x04000241 RID: 577
	public static AudioClip weapon_barrett;

	// Token: 0x04000242 RID: 578
	public static AudioClip weapon_tmp;

	// Token: 0x04000243 RID: 579
	public static AudioClip weapon_auga3;

	// Token: 0x04000244 RID: 580
	public static AudioClip weapon_sg552;

	// Token: 0x04000245 RID: 581
	public static AudioClip weapon_l96a1;

	// Token: 0x04000246 RID: 582
	public static AudioClip weapon_nova;

	// Token: 0x04000247 RID: 583
	public static AudioClip weapon_kord;

	// Token: 0x04000248 RID: 584
	public static AudioClip weapon_anaconda;

	// Token: 0x04000249 RID: 585
	public static AudioClip weapon_scar;

	// Token: 0x0400024A RID: 586
	public static AudioClip weapon_p90;

	// Token: 0x0400024B RID: 587
	public static AudioClip weapon_rpk;

	// Token: 0x0400024C RID: 588
	public static AudioClip weapon_hk416;

	// Token: 0x0400024D RID: 589
	public static AudioClip weapon_ak102;

	// Token: 0x0400024E RID: 590
	public static AudioClip weapon_sr25;

	// Token: 0x0400024F RID: 591
	public static AudioClip weapon_mosin;

	// Token: 0x04000250 RID: 592
	public static AudioClip weapon_ppsh;

	// Token: 0x04000251 RID: 593
	public static AudioClip weapon_mp40;

	// Token: 0x04000252 RID: 594
	public static AudioClip weapon_l96a1mod;

	// Token: 0x04000253 RID: 595
	public static AudioClip weapon_kacpdw;

	// Token: 0x04000254 RID: 596
	public static AudioClip weapon_famas;

	// Token: 0x04000255 RID: 597
	public static AudioClip weapon_beretta;

	// Token: 0x04000256 RID: 598
	public static AudioClip weapon_tank;

	// Token: 0x04000257 RID: 599
	public static AudioClip weapon_rpg;

	// Token: 0x04000258 RID: 600
	public static AudioClip weapon_repair_tool;

	// Token: 0x04000259 RID: 601
	public static AudioClip weapon_aa12;

	// Token: 0x0400025A RID: 602
	public static AudioClip weapon_fn57;

	// Token: 0x0400025B RID: 603
	public static AudioClip weapon_fs2000;

	// Token: 0x0400025C RID: 604
	public static AudioClip weapon_l85;

	// Token: 0x0400025D RID: 605
	public static AudioClip weapon_mac10;

	// Token: 0x0400025E RID: 606
	public static AudioClip weapon_pkp;

	// Token: 0x0400025F RID: 607
	public static AudioClip weapon_pm;

	// Token: 0x04000260 RID: 608
	public static AudioClip weapon_tar21;

	// Token: 0x04000261 RID: 609
	public static AudioClip weapon_ump45;

	// Token: 0x04000262 RID: 610
	public static AudioClip weapon_ntw20;

	// Token: 0x04000263 RID: 611
	public static AudioClip weapon_minigun;

	// Token: 0x04000264 RID: 612
	public static AudioClip weapon_javelin;

	// Token: 0x04000265 RID: 613
	public static AudioClip weapon_minefly;

	// Token: 0x04000266 RID: 614
	public static AudioClip weapon_mglmk1;

	// Token: 0x04000267 RID: 615
	public static AudioClip weapon_mauzer;

	// Token: 0x04000268 RID: 616
	public static AudioClip weapon_crossbow;

	// Token: 0x04000269 RID: 617
	public static AudioClip weapon_qbz95;

	// Token: 0x0400026A RID: 618
	public static AudioClip shield_hit;

	// Token: 0x0400026B RID: 619
	public static AudioClip snow_hit;

	// Token: 0x0400026C RID: 620
	public static AudioClip weapon_aksu;

	// Token: 0x0400026D RID: 621
	public static AudioClip weapon_m700;

	// Token: 0x0400026E RID: 622
	public static AudioClip weapon_stechkin;

	// Token: 0x0400026F RID: 623
	public static AudioClip weapon_dpm;

	// Token: 0x04000270 RID: 624
	public static AudioClip weapon_m1924;

	// Token: 0x04000271 RID: 625
	public static AudioClip weapon_mg42;

	// Token: 0x04000272 RID: 626
	public static AudioClip weapon_sten;

	// Token: 0x04000273 RID: 627
	public static AudioClip weapon_m1a1;

	// Token: 0x04000274 RID: 628
	public static AudioClip weapon_type99;

	// Token: 0x04000275 RID: 629
	public static AudioClip weapon_bizon;

	// Token: 0x04000276 RID: 630
	public static AudioClip weapon_groza;

	// Token: 0x04000277 RID: 631
	public static AudioClip weapon_jackhammer;

	// Token: 0x04000278 RID: 632
	public static AudioClip weapon_pila;

	// Token: 0x04000279 RID: 633
	public static AudioClip weapon_psg_1;

	// Token: 0x0400027A RID: 634
	public static AudioClip weapon_krytac;

	// Token: 0x0400027B RID: 635
	public static AudioClip weapon_mp5sd;

	// Token: 0x0400027C RID: 636
	public static AudioClip weapon_colts;

	// Token: 0x0400027D RID: 637
	public static AudioClip weapon_magnum_lady;

	// Token: 0x0400027E RID: 638
	public static AudioClip weapon_scorpion;

	// Token: 0x0400027F RID: 639
	public static AudioClip weapon_fmg9;

	// Token: 0x04000280 RID: 640
	public static AudioClip weapon_saiga;

	// Token: 0x04000281 RID: 641
	public static AudioClip weapon_flamethrower;

	// Token: 0x04000282 RID: 642
	public static AudioClip weapon_flamethrower_start;

	// Token: 0x04000283 RID: 643
	public static AudioClip weapon_flamethrower_end;

	// Token: 0x04000284 RID: 644
	public static AudioClip Shovel_Stoneend;

	// Token: 0x04000285 RID: 645
	public static AudioClip Shovel_Stone;

	// Token: 0x04000286 RID: 646
	public static AudioClip Shovel_Wood;

	// Token: 0x04000287 RID: 647
	public static AudioClip Shovel_Brick;

	// Token: 0x04000288 RID: 648
	public static AudioClip Shovel_Leaf;

	// Token: 0x04000289 RID: 649
	public static AudioClip Hit;

	// Token: 0x0400028A RID: 650
	public static AudioClip HitHelmet;

	// Token: 0x0400028B RID: 651
	public static AudioClip HitHelmet_trace;

	// Token: 0x0400028C RID: 652
	public static AudioClip TankStand;

	// Token: 0x0400028D RID: 653
	public static AudioClip TankMove;

	// Token: 0x0400028E RID: 654
	public static AudioClip TankStart;

	// Token: 0x0400028F RID: 655
	public static AudioClip TankStop;

	// Token: 0x04000290 RID: 656
	public static AudioClip TankEnter;

	// Token: 0x04000291 RID: 657
	public static AudioClip TankTurretMove;

	// Token: 0x04000292 RID: 658
	public static AudioClip TankTurretStart;

	// Token: 0x04000293 RID: 659
	public static AudioClip TankTurretStop;

	// Token: 0x04000294 RID: 660
	public static AudioClip TankZoom;

	// Token: 0x04000295 RID: 661
	public static AudioClip TankHit;

	// Token: 0x04000296 RID: 662
	public static AudioClip TankDeath;

	// Token: 0x04000297 RID: 663
	public static AudioClip JeepStand;

	// Token: 0x04000298 RID: 664
	public static AudioClip JeepMove;

	// Token: 0x04000299 RID: 665
	public static AudioClip JeepStart;

	// Token: 0x0400029A RID: 666
	public static AudioClip JeepStop;

	// Token: 0x0400029B RID: 667
	public static AudioClip SmokeGrenade;

	// Token: 0x0400029C RID: 668
	public static AudioClip Walk;

	// Token: 0x0400029D RID: 669
	public static AudioClip SnowWalk;

	// Token: 0x0400029E RID: 670
	public static AudioClip StoneWalk;

	// Token: 0x0400029F RID: 671
	public static AudioClip MetallWalk;

	// Token: 0x040002A0 RID: 672
	public static AudioClip WaterWalk;

	// Token: 0x040002A1 RID: 673
	public static AudioClip WoodWalk;

	// Token: 0x040002A2 RID: 674
	public static AudioClip TankMG;

	// Token: 0x040002A3 RID: 675
	public static AudioClip TankRepairKit;

	// Token: 0x040002A4 RID: 676
	public static AudioClip Zoom;

	// Token: 0x040002A5 RID: 677
	public static AudioClip ZM_Ambient;

	// Token: 0x040002A6 RID: 678
	public static AudioClip ZM_Infected;

	// Token: 0x040002A7 RID: 679
	public static AudioClip Death;

	// Token: 0x040002A8 RID: 680
	public static AudioClip TNT;

	// Token: 0x040002A9 RID: 681
	public static AudioClip gplauncher;

	// Token: 0x040002AA RID: 682
	public static AudioClip javelinTargeting;

	// Token: 0x040002AB RID: 683
	public static AudioClip javelinAIM;

	// Token: 0x040002AC RID: 684
	public static AudioClip javelinMissleAIM;

	// Token: 0x040002AD RID: 685
	public static AudioClip minigunMotor;

	// Token: 0x040002AE RID: 686
	public static AudioClip snaryadFly;

	// Token: 0x040002AF RID: 687
	public static AudioClip weapon_reload;

	// Token: 0x040002B0 RID: 688
	public static AudioClip weapon_noammo;

	// Token: 0x040002B1 RID: 689
	public static AudioClip mine_fly;

	// Token: 0x040002B2 RID: 690
	public static AudioClip molotov_fly;

	// Token: 0x040002B3 RID: 691
	public static AudioClip molotov_birn;

	// Token: 0x040002B4 RID: 692
	public static AudioClip molotov_explosion;

	// Token: 0x040002B5 RID: 693
	public static AudioClip FlashLaunch;

	// Token: 0x040002B6 RID: 694
	public static AudioClip select;

	// Token: 0x040002B7 RID: 695
	public static AudioClip podarok;

	// Token: 0x040002B8 RID: 696
	public static AudioClip mine_place;

	// Token: 0x040002B9 RID: 697
	public static AudioClip c4_detonator;

	// Token: 0x040002BA RID: 698
	public static AudioClip weapon_L96A1GOLD;

	// Token: 0x040002BB RID: 699
	public static AudioClip weapon_SVT40;

	// Token: 0x040002BC RID: 700
	public static AudioClip weapon_SVUAS;

	// Token: 0x040002BD RID: 701
	public static AudioClip weapon_PSG1GOLD;

	// Token: 0x040002BE RID: 702
	public static AudioClip weapon_M110;

	// Token: 0x040002BF RID: 703
	public static AudioClip weapon_KEDR;

	// Token: 0x040002C0 RID: 704
	public static AudioClip weapon_MAC10GOLD;

	// Token: 0x040002C1 RID: 705
	public static AudioClip weapon_KASHTAN;

	// Token: 0x040002C2 RID: 706
	public static AudioClip weapon_FMG9GOLD;

	// Token: 0x040002C3 RID: 707
	public static AudioClip weapon_MP7;

	// Token: 0x040002C4 RID: 708
	public static AudioClip weapon_PKPGOLD;

	// Token: 0x040002C5 RID: 709
	public static AudioClip weapon_NEGEV;

	// Token: 0x040002C6 RID: 710
	public static AudioClip weapon_XM8;

	// Token: 0x040002C7 RID: 711
	public static AudioClip weapon_AK74;

	// Token: 0x040002C8 RID: 712
	public static AudioClip weapon_AK47GOLD;

	// Token: 0x040002C9 RID: 713
	public static AudioClip weapon_M4A1GOLD;

	// Token: 0x040002CA RID: 714
	public static AudioClip weapon_ABAKAN;

	// Token: 0x040002CB RID: 715
	public static AudioClip weapon_AK103;

	// Token: 0x040002CC RID: 716
	public static AudioClip weapon_BEKAS12M;

	// Token: 0x040002CD RID: 717
	public static AudioClip weapon_MOSSBERG500;

	// Token: 0x040002CE RID: 718
	public static AudioClip weapon_NOVAGOLD;

	// Token: 0x040002CF RID: 719
	public static AudioClip weapon_NEOSTEAD2000;

	// Token: 0x040002D0 RID: 720
	public static AudioClip weapon_HONCHO;

	// Token: 0x040002D1 RID: 721
	public static AudioClip weapon_M2;

	// Token: 0x040002D2 RID: 722
	public static AudioClip weapon_M4;

	// Token: 0x040002D3 RID: 723
	public static AudioClip weapon_SAIGAGOLD;

	// Token: 0x040002D4 RID: 724
	public static AudioClip weapon_BROWNING;

	// Token: 0x040002D5 RID: 725
	public static AudioClip weapon_WALTHER;

	// Token: 0x040002D6 RID: 726
	public static AudioClip weapon_CZ75;

	// Token: 0x040002D7 RID: 727
	public static AudioClip weapon_SWM29;

	// Token: 0x040002D8 RID: 728
	public static AudioClip weapon_UZI;

	// Token: 0x040002D9 RID: 729
	public static AudioClip weapon_M1887;

	// Token: 0x040002DA RID: 730
	public static AudioClip weapon_REMINGTON870;

	// Token: 0x040002DB RID: 731
	public static AudioClip weapon_VEPR12;

	// Token: 0x040002DC RID: 732
	public static AudioClip weapon_P226;

	// Token: 0x040002DD RID: 733
	public static AudioClip weapon_QBU88;

	// Token: 0x040002DE RID: 734
	public static AudioClip weapon_DEAGLEGOLD;
}
