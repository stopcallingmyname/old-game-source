using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class Sound : MonoBehaviour
{
	// Token: 0x06000156 RID: 342 RVA: 0x0001987C File Offset: 0x00017A7C
	private void Awake()
	{
		this.csas_loop = base.gameObject.GetComponent<AudioSource>();
		this.csas_loop.playOnAwake = false;
		this.csas_loop.loop = true;
		this.currPitch = 1f;
		this.csas = base.gameObject.AddComponent<AudioSource>();
		this.csas.playOnAwake = false;
		this.csas.loop = false;
		this.csas.maxDistance = 30f;
		this.csas.spatialBlend = 1f;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x00019908 File Offset: 0x00017B08
	public void PlaySound_Weapon(int weaponid)
	{
		if (weaponid == 35)
		{
			return;
		}
		AudioClip audioClip = this.ReturnSound_Weapon(weaponid);
		if (audioClip != null)
		{
			this.csas.PlayOneShot(audioClip, AudioListener.volume);
		}
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00019940 File Offset: 0x00017B40
	public AudioClip ReturnSound_Weapon(int weaponid)
	{
		if (weaponid == 0)
		{
			return SoundManager.weapon_block;
		}
		if (weaponid == 33)
		{
			return SoundManager.weapon_shovel;
		}
		if (weaponid == 43)
		{
			return SoundManager.weapon_m3;
		}
		if (weaponid == 44)
		{
			return SoundManager.weapon_m14;
		}
		if (weaponid == 45)
		{
			return SoundManager.weapon_mp5;
		}
		if (weaponid == 2)
		{
			return SoundManager.weapon_ak47;
		}
		if (weaponid == 329)
		{
			return SoundManager.weapon_ak47;
		}
		if (weaponid == 3)
		{
			return SoundManager.weapon_svd;
		}
		if (weaponid == 46)
		{
			return SoundManager.weapon_glock;
		}
		if (weaponid == 9)
		{
			return SoundManager.weapon_deagle;
		}
		if (weaponid == 10)
		{
			return SoundManager.weapon_shmel;
		}
		if (weaponid == 12)
		{
			return SoundManager.weapon_asval;
		}
		if (weaponid == 13)
		{
			return SoundManager.weapon_g36c;
		}
		if (weaponid == 14)
		{
			return SoundManager.weapon_kriss;
		}
		if (weaponid == 15)
		{
			return SoundManager.weapon_m4a1;
		}
		if (weaponid == 19)
		{
			return SoundManager.weapon_vsk94;
		}
		if (weaponid == 16)
		{
			return SoundManager.weapon_m249;
		}
		if (weaponid == 17)
		{
			return SoundManager.weapon_spas12;
		}
		if (weaponid == 18)
		{
			return SoundManager.weapon_vintorez;
		}
		if (weaponid == 40)
		{
			return SoundManager.weapon_usp;
		}
		if (weaponid == 333)
		{
			return SoundManager.weapon_usp;
		}
		if (weaponid == 47)
		{
			return SoundManager.weapon_barrett;
		}
		if (weaponid == 48)
		{
			return SoundManager.weapon_tmp;
		}
		if (weaponid == 62)
		{
			return SoundManager.weapon_minefly;
		}
		if (weaponid == 60)
		{
			return SoundManager.weapon_auga3;
		}
		if (weaponid == 61)
		{
			return SoundManager.weapon_sg552;
		}
		if (weaponid == 68)
		{
			return SoundManager.weapon_m14;
		}
		if (weaponid == 69)
		{
			return SoundManager.weapon_l96a1;
		}
		if (weaponid == 70)
		{
			return SoundManager.weapon_nova;
		}
		if (weaponid == 71)
		{
			return SoundManager.weapon_kord;
		}
		if (weaponid == 72)
		{
			return SoundManager.weapon_anaconda;
		}
		if (weaponid == 73)
		{
			return SoundManager.weapon_scar;
		}
		if (weaponid == 74)
		{
			return SoundManager.weapon_p90;
		}
		if (weaponid == 330)
		{
			return SoundManager.weapon_p90;
		}
		if (weaponid == 78)
		{
			return SoundManager.weapon_rpk;
		}
		if (weaponid == 79)
		{
			return SoundManager.weapon_hk416;
		}
		if (weaponid == 80)
		{
			return SoundManager.weapon_ak102;
		}
		if (weaponid == 81)
		{
			return SoundManager.weapon_sr25;
		}
		if (weaponid == 332)
		{
			return SoundManager.weapon_sr25;
		}
		if (weaponid == 82)
		{
			return SoundManager.weapon_mglmk1;
		}
		if (weaponid == 89)
		{
			return SoundManager.weapon_mosin;
		}
		if (weaponid == 90)
		{
			return SoundManager.weapon_ppsh;
		}
		if (weaponid == 91)
		{
			return SoundManager.weapon_mp40;
		}
		if (weaponid == 34)
		{
			return SoundManager.weapon_l96a1mod;
		}
		if (weaponid == 93)
		{
			return SoundManager.weapon_kacpdw;
		}
		if (weaponid == 94)
		{
			return SoundManager.weapon_famas;
		}
		if (weaponid == 95)
		{
			return SoundManager.weapon_beretta;
		}
		if (weaponid == 100)
		{
			return SoundManager.weapon_rpg;
		}
		if (weaponid == 101)
		{
			return SoundManager.weapon_repair_tool;
		}
		if (weaponid == 102)
		{
			return SoundManager.weapon_aa12;
		}
		if (weaponid == 103)
		{
			return SoundManager.weapon_fn57;
		}
		if (weaponid == 104)
		{
			return SoundManager.weapon_fs2000;
		}
		if (weaponid == 105)
		{
			return SoundManager.weapon_l85;
		}
		if (weaponid == 106)
		{
			return SoundManager.weapon_mac10;
		}
		if (weaponid == 107)
		{
			return SoundManager.weapon_pkp;
		}
		if (weaponid == 108)
		{
			return SoundManager.weapon_pm;
		}
		if (weaponid == 109)
		{
			return SoundManager.weapon_tar21;
		}
		if (weaponid == 110)
		{
			return SoundManager.weapon_ump45;
		}
		if (weaponid == 111)
		{
			return SoundManager.weapon_ntw20;
		}
		if (weaponid == 112)
		{
			return SoundManager.weapon_vintorez;
		}
		if (weaponid == 137)
		{
			return SoundManager.weapon_minigun;
		}
		if (weaponid == 138)
		{
			return SoundManager.weapon_javelin;
		}
		if (weaponid == 139)
		{
			return SoundManager.weapon_aa12;
		}
		if (weaponid == 140)
		{
			return SoundManager.weapon_asval;
		}
		if (weaponid == 141)
		{
			return SoundManager.weapon_fn57;
		}
		if (weaponid == 142)
		{
			return SoundManager.weapon_kord;
		}
		if (weaponid == 143)
		{
			return SoundManager.weapon_m249;
		}
		if (weaponid == 144)
		{
			return SoundManager.weapon_minigun;
		}
		if (weaponid == 145)
		{
			return SoundManager.weapon_spas12;
		}
		if (weaponid == 160)
		{
			return SoundManager.weapon_mauzer;
		}
		if (weaponid == 161)
		{
			return SoundManager.weapon_crossbow;
		}
		if (weaponid == 162)
		{
			return SoundManager.weapon_qbz95;
		}
		if (weaponid == 175)
		{
			return SoundManager.weapon_aksu;
		}
		if (weaponid == 176)
		{
			return SoundManager.weapon_m700;
		}
		if (weaponid == 177)
		{
			return SoundManager.weapon_stechkin;
		}
		if (weaponid == 188)
		{
			return SoundManager.weapon_dpm;
		}
		if (weaponid == 189)
		{
			return SoundManager.weapon_m1924;
		}
		if (weaponid == 190)
		{
			return SoundManager.weapon_mg42;
		}
		if (weaponid == 191)
		{
			return SoundManager.weapon_sten;
		}
		if (weaponid == 192)
		{
			return SoundManager.weapon_m1a1;
		}
		if (weaponid == 193)
		{
			return SoundManager.weapon_type99;
		}
		if (weaponid == 185)
		{
			return SoundManager.weapon_shmel;
		}
		if (weaponid == 207)
		{
			return SoundManager.weapon_bizon;
		}
		if (weaponid == 208)
		{
			return SoundManager.weapon_groza;
		}
		if (weaponid == 209)
		{
			return SoundManager.weapon_jackhammer;
		}
		if (weaponid == 218)
		{
			return SoundManager.weapon_psg_1;
		}
		if (weaponid == 219)
		{
			return SoundManager.weapon_krytac;
		}
		if (weaponid == 220)
		{
			return SoundManager.weapon_mp5sd;
		}
		if (weaponid == 221)
		{
			return SoundManager.weapon_colts;
		}
		if (weaponid == 301)
		{
			return SoundManager.weapon_jackhammer;
		}
		if (weaponid == 302)
		{
			return SoundManager.weapon_m700;
		}
		if (weaponid == 303)
		{
			return SoundManager.weapon_mg42;
		}
		if (weaponid == 305)
		{
			return SoundManager.weapon_magnum_lady;
		}
		if (weaponid == 308)
		{
			return SoundManager.weapon_scorpion;
		}
		if (weaponid == 309)
		{
			return SoundManager.weapon_g36c;
		}
		if (weaponid == 313)
		{
			return SoundManager.weapon_fmg9;
		}
		if (weaponid == 314)
		{
			return SoundManager.weapon_saiga;
		}
		if (weaponid == 331)
		{
			return SoundManager.weapon_saiga;
		}
		if (weaponid == 315)
		{
			return SoundManager.weapon_flamethrower;
		}
		if (weaponid == 39)
		{
			return SoundManager.weapon_mosin;
		}
		if (weaponid == 154)
		{
			return SoundManager.weapon_mg42;
		}
		if (weaponid == 155)
		{
			return SoundManager.weapon_rpk;
		}
		if (weaponid == 92)
		{
			return SoundManager.weapon_stechkin;
		}
		if (weaponid == 334)
		{
			return SoundManager.weapon_L96A1GOLD;
		}
		if (weaponid == 335)
		{
			return SoundManager.weapon_SVT40;
		}
		if (weaponid == 336)
		{
			return SoundManager.weapon_SVUAS;
		}
		if (weaponid == 337)
		{
			return SoundManager.weapon_PSG1GOLD;
		}
		if (weaponid == 338)
		{
			return SoundManager.weapon_M110;
		}
		if (weaponid == 339)
		{
			return SoundManager.weapon_KEDR;
		}
		if (weaponid == 340)
		{
			return SoundManager.weapon_MAC10GOLD;
		}
		if (weaponid == 341)
		{
			return SoundManager.weapon_KASHTAN;
		}
		if (weaponid == 342)
		{
			return SoundManager.weapon_FMG9GOLD;
		}
		if (weaponid == 343)
		{
			return SoundManager.weapon_MP7;
		}
		if (weaponid == 344)
		{
			return SoundManager.weapon_PKPGOLD;
		}
		if (weaponid == 345)
		{
			return SoundManager.weapon_NEGEV;
		}
		if (weaponid == 346)
		{
			return SoundManager.weapon_XM8;
		}
		if (weaponid == 347)
		{
			return SoundManager.weapon_AK74;
		}
		if (weaponid == 348)
		{
			return SoundManager.weapon_AK47GOLD;
		}
		if (weaponid == 349)
		{
			return SoundManager.weapon_M4A1GOLD;
		}
		if (weaponid == 350)
		{
			return SoundManager.weapon_ABAKAN;
		}
		if (weaponid == 351)
		{
			return SoundManager.weapon_AK103;
		}
		if (weaponid == 352)
		{
			return SoundManager.weapon_BEKAS12M;
		}
		if (weaponid == 353)
		{
			return SoundManager.weapon_MOSSBERG500;
		}
		if (weaponid == 354)
		{
			return SoundManager.weapon_NOVAGOLD;
		}
		if (weaponid == 355)
		{
			return SoundManager.weapon_NEOSTEAD2000;
		}
		if (weaponid == 356)
		{
			return SoundManager.weapon_HONCHO;
		}
		if (weaponid == 357)
		{
			return SoundManager.weapon_M2;
		}
		if (weaponid == 358)
		{
			return SoundManager.weapon_M4;
		}
		if (weaponid == 359)
		{
			return SoundManager.weapon_SAIGAGOLD;
		}
		if (weaponid == 360)
		{
			return SoundManager.weapon_BROWNING;
		}
		if (weaponid == 361)
		{
			return SoundManager.weapon_WALTHER;
		}
		if (weaponid == 362)
		{
			return SoundManager.weapon_CZ75;
		}
		if (weaponid == 363)
		{
			return SoundManager.weapon_SWM29;
		}
		if (weaponid == 364)
		{
			return SoundManager.weapon_UZI;
		}
		if (weaponid == 365)
		{
			return SoundManager.weapon_M1887;
		}
		if (weaponid == 366)
		{
			return SoundManager.weapon_REMINGTON870;
		}
		if (weaponid == 367)
		{
			return SoundManager.weapon_VEPR12;
		}
		if (weaponid == 368)
		{
			return SoundManager.weapon_P226;
		}
		if (weaponid == 369)
		{
			return SoundManager.weapon_QBU88;
		}
		if (weaponid == 370)
		{
			return SoundManager.weapon_DEAGLEGOLD;
		}
		return null;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0001A044 File Offset: 0x00018244
	public void PlaySound_Block(string blockname)
	{
		if (blockname == "Stoneend")
		{
			this.csas.PlayOneShot(SoundManager.Shovel_Stoneend, AudioListener.volume);
		}
		else if (blockname == "Leaf")
		{
			this.csas.PlayOneShot(SoundManager.Shovel_Leaf, AudioListener.volume);
		}
		else if (blockname == "Stone")
		{
			this.csas.PlayOneShot(SoundManager.Shovel_Stone, AudioListener.volume);
		}
		else if (blockname == "Wood")
		{
			this.csas.PlayOneShot(SoundManager.Shovel_Wood, AudioListener.volume);
		}
		if (blockname == "Brick")
		{
			this.csas.PlayOneShot(SoundManager.Shovel_Brick, AudioListener.volume);
		}
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0001A101 File Offset: 0x00018301
	public void PlaySound_Hit()
	{
		this.csas.PlayOneShot(SoundManager.Hit, AudioListener.volume);
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0001A118 File Offset: 0x00018318
	public void PlaySound_HitHelmet()
	{
		this.csas.PlayOneShot(SoundManager.HitHelmet, AudioListener.volume);
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0001A12F File Offset: 0x0001832F
	public void PlaySound_TraceHelmet()
	{
		this.csas.PlayOneShot(SoundManager.HitHelmet_trace, AudioListener.volume);
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0001A148 File Offset: 0x00018348
	public void PlaySound_Walk(int type)
	{
		if (this.csas_loop.isPlaying)
		{
			return;
		}
		if (type == 1)
		{
			this.csas_loop.clip = SoundManager.Walk;
		}
		else if (type == 2)
		{
			this.csas_loop.clip = SoundManager.SnowWalk;
		}
		else if (type == 3)
		{
			this.csas_loop.clip = SoundManager.StoneWalk;
		}
		else if (type == 4)
		{
			this.csas_loop.clip = SoundManager.MetallWalk;
		}
		else if (type == 5)
		{
			this.csas_loop.clip = SoundManager.WaterWalk;
		}
		else if (type == 6)
		{
			this.csas_loop.clip = SoundManager.WoodWalk;
		}
		this.csas_loop.Play();
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0001A1F0 File Offset: 0x000183F0
	public void PlaySound_TankEnter(AudioSource AS)
	{
		AS.Stop();
		AS.clip = SoundManager.TankEnter;
		AS.Play();
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0001A209 File Offset: 0x00018409
	public void PlaySound_TankStand(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.TankStand;
		AS.Play();
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0001A225 File Offset: 0x00018425
	public void PlaySound_TankMove(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.TankMove;
		AS.Play();
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0001A241 File Offset: 0x00018441
	public void PlaySound_TankStart(AudioSource AS)
	{
		AS.Stop();
		AS.clip = SoundManager.TankStart;
		AS.Play();
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0001A25A File Offset: 0x0001845A
	public void PlaySound_TankStop(AudioSource AS)
	{
		AS.Stop();
		AS.clip = SoundManager.TankStop;
		AS.Play();
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0001A273 File Offset: 0x00018473
	public void PlaySound_JeepStand(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.JeepStand;
		AS.Play();
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0001A28F File Offset: 0x0001848F
	public void PlaySound_JeepMove(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.JeepMove;
		AS.Play();
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0001A2AB File Offset: 0x000184AB
	public void PlaySound_JeepStart(AudioSource AS)
	{
		AS.Stop();
		AS.clip = SoundManager.JeepStart;
		AS.Play();
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0001A2C4 File Offset: 0x000184C4
	public void PlaySound_JeepStop(AudioSource AS)
	{
		AS.Stop();
		AS.clip = SoundManager.JeepStop;
		AS.Play();
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0001A2DD File Offset: 0x000184DD
	public void PlaySound_SnaryadFly(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.snaryadFly;
		AS.Play();
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0001A2F9 File Offset: 0x000184F9
	public void PlaySound_Pich(float koef, AudioSource AS)
	{
		AS.pitch = this.currPitch + koef;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0001A309 File Offset: 0x00018509
	public void PlaySound_TurretStart(AudioSource AS)
	{
		AS.Stop();
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0001A311 File Offset: 0x00018511
	public void PlaySound_TurretMove(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.TankTurretMove;
		AS.Play();
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0001A309 File Offset: 0x00018509
	public void PlaySound_TurretStop(AudioSource AS)
	{
		AS.Stop();
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0001A32D File Offset: 0x0001852D
	public void PlaySound_Stop(AudioSource AS = null)
	{
		if (AS == null)
		{
			this.csas_loop.Stop();
			return;
		}
		AS.Stop();
		AS.pitch = 1f;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0001A355 File Offset: 0x00018555
	public void PlaySound_Zoom()
	{
		this.csas.PlayOneShot(SoundManager.Zoom, AudioListener.volume);
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0001A36C File Offset: 0x0001856C
	public void PlaySound_TankZoom(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.TankZoom, AudioListener.volume);
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0001A37E File Offset: 0x0001857E
	public void PlaySound_ZM_Infected()
	{
		this.csas.PlayOneShot(SoundManager.ZM_Infected, AudioListener.volume);
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0001A395 File Offset: 0x00018595
	public void PlaySound_ZM_Ambient()
	{
		this.csas.PlayOneShot(SoundManager.ZM_Ambient, AudioListener.volume);
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0001A3AC File Offset: 0x000185AC
	public void PlaySound_Death()
	{
		this.csas.PlayOneShot(SoundManager.Death, AudioListener.volume);
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0001A3C3 File Offset: 0x000185C3
	public void PlaySound_TNT()
	{
		this.csas.PlayOneShot(SoundManager.TNT, AudioListener.volume);
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0001A3DA File Offset: 0x000185DA
	public void PlaySound_C4_Detonator()
	{
		this.csas.PlayOneShot(SoundManager.c4_detonator, AudioListener.volume);
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0001A3F1 File Offset: 0x000185F1
	public void PlaySound_MinePlace()
	{
		this.csas.PlayOneShot(SoundManager.mine_place, AudioListener.volume);
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0001A408 File Offset: 0x00018608
	public void PlaySound_GPLauncher()
	{
		this.csas.PlayOneShot(SoundManager.gplauncher, AudioListener.volume);
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0001A41F File Offset: 0x0001861F
	public void PlaySound_WeaponTank(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.weapon_tank, AudioListener.volume);
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0001A431 File Offset: 0x00018631
	public void PlaySound_WeaponMGTank(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.TankMG, AudioListener.volume);
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0001A444 File Offset: 0x00018644
	public void PlaySound_UseModul(AudioSource AS, int module_index)
	{
		if (module_index == CONST.VEHICLES.VEHICLE_MODUL_REPAIR_KIT)
		{
			AS.PlayOneShot(SoundManager.TankRepairKit, AudioListener.volume);
		}
		if (module_index == CONST.VEHICLES.VEHICLE_MODUL_ANTI_MISSLE)
		{
			AS.PlayOneShot(SoundManager.FlashLaunch, AudioListener.volume);
		}
		if (module_index == CONST.VEHICLES.VEHICLE_MODUL_SMOKE)
		{
			AS.PlayOneShot(SoundManager.SmokeGrenade, AudioListener.volume);
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0001A499 File Offset: 0x00018699
	public void PlaySound_TankMGNoAmmo(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.weapon_noammo, AudioListener.volume);
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0001A4AB File Offset: 0x000186AB
	public void PlaySound_TankMGReload(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.weapon_reload, AudioListener.volume);
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0001A4BD File Offset: 0x000186BD
	public void PlaySound_Present(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.podarok, AudioListener.volume);
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0001A4CF File Offset: 0x000186CF
	public void PlaySound_TankHit(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.TankHit, AudioListener.volume);
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0001A4E1 File Offset: 0x000186E1
	public void PlaySound_JavelinTargeting(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.javelinTargeting;
		AS.Play();
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0001A4FD File Offset: 0x000186FD
	public void PlaySound_JavelinAIM(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.javelinAIM;
		AS.Play();
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0001A519 File Offset: 0x00018719
	public void PlaySound_JavelinMissleAIM(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.javelinMissleAIM;
		AS.Play();
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0001A535 File Offset: 0x00018735
	public void PlaySound_MinigunMotor(AudioSource AS)
	{
		if (AS.isPlaying)
		{
			return;
		}
		AS.clip = SoundManager.minigunMotor;
		AS.loop = true;
		AS.Play();
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0001A558 File Offset: 0x00018758
	public AudioClip GetDryFire()
	{
		return SoundManager.weapon_noammo;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0001A55F File Offset: 0x0001875F
	public AudioClip GetReload()
	{
		return SoundManager.weapon_reload;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0001A566 File Offset: 0x00018766
	public AudioClip GetMineFly()
	{
		return SoundManager.mine_fly;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0001A56D File Offset: 0x0001876D
	public AudioClip GetMolotovFly()
	{
		return SoundManager.molotov_fly;
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0001A574 File Offset: 0x00018774
	public AudioClip GetMolotovBurn()
	{
		return SoundManager.molotov_birn;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0001A57B File Offset: 0x0001877B
	public AudioClip GetMolotovExplosion()
	{
		return SoundManager.molotov_explosion;
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0001A582 File Offset: 0x00018782
	public AudioClip GetSelect()
	{
		return SoundManager.select;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0001A589 File Offset: 0x00018789
	public AudioClip GetDeath()
	{
		return SoundManager.Death;
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0001A590 File Offset: 0x00018790
	public void PlaySound_SmokeGrenade(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.SmokeGrenade, AudioListener.volume);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0001A5A2 File Offset: 0x000187A2
	public void PlaySound_ShieldHit(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.shield_hit, AudioListener.volume);
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0001A5B4 File Offset: 0x000187B4
	public void PlaySound_SnowHit(AudioSource AS)
	{
		AS.PlayOneShot(SoundManager.snow_hit, AudioListener.volume);
	}

	// Token: 0x04000167 RID: 359
	public AudioSource csas;

	// Token: 0x04000168 RID: 360
	public AudioSource csas_loop;

	// Token: 0x04000169 RID: 361
	private float currPitch;

	// Token: 0x0400016A RID: 362
	private float currVolume;
}
