using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class RemotePlayersUpdater : MonoBehaviour
{
	// Token: 0x0600013C RID: 316 RVA: 0x00017100 File Offset: 0x00015300
	private void Awake()
	{
		RemotePlayersUpdater.Instance = this;
		this.Gui = GameObject.Find("GUI");
		this.pd = (PackData)Object.FindObjectOfType(typeof(PackData));
		this.map = base.GetComponent<Map>();
		this.csrm = base.GetComponent<RagDollManager>();
		this.SkinManager = base.GetComponent<SpawnManager>();
		this.csig = this.Gui.GetComponent<MainGUI>();
		BlockSet blockSet = this.map.GetBlockSet();
		this.teamblock[0] = blockSet.GetBlock("Brick_blue");
		this.teamblock[1] = blockSet.GetBlock("Brick_red");
		this.teamblock[2] = blockSet.GetBlock("Brick_green");
		this.teamblock[3] = blockSet.GetBlock("Brick_yellow");
		this.teamblock[4] = blockSet.GetBlock("ArmoredBrickBlue");
		this.teamblock[5] = blockSet.GetBlock("ArmoredBrickRed");
		this.teamblock[6] = blockSet.GetBlock("ArmoredBrickGreen");
		this.teamblock[7] = blockSet.GetBlock("ArmoredBrickYellow");
		this.BotsGmObj = new GameObject[32];
		for (int i = 0; i < 32; i++)
		{
			this.Bots[i] = new BotData();
			this.CreatePlayer(i);
		}
		this.PlayersLoaded = true;
		this.CreateLocalPlayer();
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00017254 File Offset: 0x00015454
	private void CreateLocalPlayer()
	{
		this.CurrentPlayer = Object.Instantiate<GameObject>(this.pgoLocalPlayer, new Vector3(-1000f, -1000f, -1000f), this.pgoLocalPlayer.transform.rotation);
		this.CurrentPlayer.name = "Player";
		this.cscl = this.CurrentPlayer.AddComponent<Client>();
		this.CurrentPlayer.AddComponent<PlayerControl>();
		this.CurrentPlayer.AddComponent<Sound>();
		this.CurrentPlayer.AddComponent<FX>();
		this.csws = this.CurrentPlayer.AddComponent<WeaponSystem>();
		this.CurrentPlayer.GetComponent<vp_FPController>().client = this.cscl;
		float num = 0f;
		int distpos = Config.distpos;
		if (distpos == 2)
		{
			num = 140f;
		}
		else if (distpos == 1)
		{
			num = 80f;
		}
		else if (distpos == 0)
		{
			num = 40f;
		}
		float[] array = new float[32];
		for (int i = 0; i < 32; i++)
		{
			array[i] = num;
		}
		Camera.main.layerCullDistances = array;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00017354 File Offset: 0x00015554
	private void CreatePlayer(int index)
	{
		this.BotsGmObj[index] = Object.Instantiate<GameObject>(this.pgoPlayer, new Vector3(-1000f, -1000f, -1000f), this.pgoPlayer.transform.rotation);
		this.BotsGmObj[index].AddComponent<Data>();
		this.BotsGmObj[index].AddComponent<TeamColor>();
		this.BotsGmObj[index].AddComponent<Sound>();
		this.BotsGmObj[index].AddComponent<FX>();
		this.BotsGmObj[index].name = "Player_" + index.ToString();
		this.Bots[index].SpecView = GameObject.Find(this.BotsGmObj[index].name + "/specview");
		this.Bots[index].goHelmet = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Helmet");
		this.Bots[index].goCap = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Cap");
		this.Bots[index].goTykva = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Tykva");
		this.Bots[index].goKolpak = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/KOLPAK");
		this.Bots[index].goRoga = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/ROGA");
		this.Bots[index].goMaskBear = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_BEAR");
		this.Bots[index].goMaskFox = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_FOX");
		this.Bots[index].goMaskRabbit = GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_RABBIT");
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine").AddComponent<Data>().SetIndex(this, index, 0);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head").AddComponent<Data>().SetIndex(this, index, 1);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh").AddComponent<Data>().SetIndex(this, index, 11);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf").AddComponent<Data>().SetIndex(this, index, 12);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf/Bip001 L Foot").AddComponent<Data>().SetIndex(this, index, 13);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh").AddComponent<Data>().SetIndex(this, index, 8);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf").AddComponent<Data>().SetIndex(this, index, 9);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf/Bip001 R Foot").AddComponent<Data>().SetIndex(this, index, 10);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm").AddComponent<Data>().SetIndex(this, index, 5);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm").AddComponent<Data>().SetIndex(this, index, 6);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand").AddComponent<Data>().SetIndex(this, index, 7);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm").AddComponent<Data>().SetIndex(this, index, 2);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm").AddComponent<Data>().SetIndex(this, index, 3);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand").AddComponent<Data>().SetIndex(this, index, 4);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/SHIELD/weapon").AddComponent<Data>().SetIndex(this, index, 77);
		GameObject.Find(this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/SHIELD_LADY/weapon").AddComponent<Data>().SetIndex(this, index, 77);
		string text = this.BotsGmObj[index].name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand/";
		this.Bots[index].weapon = new GameObject[500];
		this.Bots[index].flash = new GameObject[500];
		for (int i = 0; i < 500; i++)
		{
			GameObject[] weapon = this.Bots[index].weapon;
			int num = i;
			string str = text;
			ITEM item = (ITEM)i;
			weapon[num] = GameObject.Find(str + item.ToString() + "/weapon");
			GameObject[] flash = this.Bots[index].flash;
			int num2 = i;
			string str2 = text;
			item = (ITEM)i;
			flash[num2] = GameObject.Find(str2 + item.ToString() + "/flash");
		}
		this.Bots[index].flamePS = this.Bots[index].flash[315].GetComponent<ParticleSystem>();
		this.Bots[index].m_Top = GameObject.Find(text + "xblock/top");
		this.Bots[index].m_Face = GameObject.Find(text + "xblock/face");
		this.Bots[index].Item = new int[500];
		this.SetPlayerActive(index, false);
		this.Bots[index].position = new Vector3(-1000f, -1000f, -1000f);
		this.pd.PackPlayerPos(index, -1000f, -1000f, -1000f);
		this.SetCurrentWeapon(index, 8);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0001796C File Offset: 0x00015B6C
	public void SetPlayerActive(int index, bool val)
	{
		if (val)
		{
			this.BotsGmObj[index].layer = 10;
			this.Bots[index].Active = true;
			this.BotsGmObj[index].GetComponent<Animator>().enabled = true;
			foreach (Renderer renderer in this.BotsGmObj[index].GetComponentsInChildren<Renderer>())
			{
				if (!(renderer.gameObject.name == "weapon") && !(renderer.gameObject.name == "face") && !(renderer.gameObject.name == "top") && !(renderer.gameObject.name == "flash") && !(renderer.gameObject.name == "rocket_rpg7") && !(renderer.gameObject.name == "rpg7") && !(renderer.gameObject.name == "crossbow") && !(renderer.gameObject.name == "arrow") && !(renderer.gameObject.name == "Snow"))
				{
					renderer.gameObject.layer = 10;
				}
			}
			foreach (Collider collider in this.BotsGmObj[index].GetComponentsInChildren<Collider>())
			{
				if (collider.transform.parent.GetComponent<Tank>() != null)
				{
					collider.gameObject.layer = 0;
				}
				else if (collider.transform.parent.GetComponent<Car>() != null)
				{
					collider.gameObject.layer = 0;
				}
				else
				{
					collider.gameObject.layer = 10;
				}
			}
			this.SetCurrentWeapon(index, 1);
			return;
		}
		this.BotsGmObj[index].layer = 9;
		this.BotsGmObj[index].GetComponent<Animator>().enabled = false;
		this.Bots[index].Active = false;
		this.Bots[index].Helmet = 0;
		this.Bots[index].Skin = 0;
		foreach (Renderer renderer2 in this.BotsGmObj[index].GetComponentsInChildren<Renderer>())
		{
			if (!(renderer2.gameObject.name == "weapon") && !(renderer2.gameObject.name == "face") && !(renderer2.gameObject.name == "top") && !(renderer2.gameObject.name == "flash") && !(renderer2.gameObject.name == "rocket_rpg7") && !(renderer2.gameObject.name == "rpg7") && !(renderer2.gameObject.name == "crossbow") && !(renderer2.gameObject.name == "arrow") && !(renderer2.gameObject.name == "Snow"))
			{
				renderer2.gameObject.layer = 9;
			}
		}
		foreach (Collider collider2 in this.BotsGmObj[index].GetComponentsInChildren<Collider>())
		{
			if (!(collider2.gameObject.name == "weapon"))
			{
				collider2.gameObject.layer = 9;
			}
		}
		this.BotsGmObj[index].transform.position = new Vector3(-1000f, -1000f, -1000f);
		this.SetCurrentWeapon(index, 8);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00017D20 File Offset: 0x00015F20
	private void Update()
	{
		if (!this.PlayersLoaded)
		{
			return;
		}
		for (int i = 0; i < 32; i++)
		{
			if (this.Bots[i] != null && this.Bots[i].Active)
			{
				if (this.Bots[i].Dead == 1)
				{
					if (this.Bots[i].flamePS != null && this.Bots[i].flamePS.isPlaying)
					{
						this.Bots[i].flamePS.Stop(true);
					}
					if (this.Bots[i].mySound != null && this.Bots[i].mySound.csas != null && this.Bots[i].mySound.csas.isPlaying && this.Bots[i].mySound.csas.clip != SoundManager.weapon_flamethrower_end)
					{
						this.Bots[i].mySound.csas.pitch = 1f;
						this.Bots[i].mySound.csas.loop = false;
						this.Bots[i].mySound.csas.clip = SoundManager.weapon_flamethrower_end;
						this.Bots[i].mySound.csas.Play();
					}
					if (this.Bots[i].flash[this.Bots[i].WeaponID] != null)
					{
						this.Bots[i].flash[this.Bots[i].WeaponID].SetActive(false);
					}
				}
				else if (this.cscl.myindex != i)
				{
					if (this.Bots[i].mySound == null)
					{
						this.Bots[i].mySound = this.BotsGmObj[i].GetComponent<Sound>();
					}
					if (Time.time > this.Bots[i].flash_time)
					{
						if (this.Bots[i].flamePS != null && this.Bots[i].flamePS.isPlaying)
						{
							this.Bots[i].flamePS.Stop(true);
						}
						if (this.Bots[i].mySound != null && this.Bots[i].mySound.csas != null && this.Bots[i].mySound.csas.isPlaying && this.Bots[i].mySound.csas.clip != SoundManager.weapon_flamethrower_end)
						{
							this.Bots[i].mySound.csas.pitch = 1f;
							this.Bots[i].mySound.csas.loop = false;
							this.Bots[i].mySound.csas.clip = SoundManager.weapon_flamethrower_end;
							this.Bots[i].mySound.csas.Play();
						}
						if (this.Bots[i].flash[this.Bots[i].WeaponID] != null)
						{
							this.Bots[i].flash[this.Bots[i].WeaponID].SetActive(false);
						}
					}
					if (Time.time < this.Bots[i].flash_time && this.Bots[i].WeaponID == 315 && !this.Bots[i].zombie && this.Bots[i].mySound != null && this.Bots[i].mySound.csas != null && !this.Bots[i].mySound.csas.isPlaying && this.Bots[i].mySound.csas.clip != SoundManager.weapon_flamethrower_start)
					{
						this.Bots[i].mySound.csas.pitch = 1f;
						this.Bots[i].mySound.csas.loop = false;
						this.Bots[i].mySound.csas.clip = SoundManager.weapon_flamethrower_start;
						this.Bots[i].mySound.csas.Play();
						vp_Timer.In(SoundManager.weapon_flamethrower_start.length - 0.1f, delegate(object _index)
						{
							if (this.Bots[(int)_index].flash_time > Time.time && this.Bots[(int)_index].mySound.csas.clip != SoundManager.weapon_flamethrower)
							{
								this.Bots[(int)_index].mySound.csas.pitch = 1f;
								this.Bots[(int)_index].mySound.csas.loop = true;
								this.Bots[(int)_index].mySound.csas.clip = SoundManager.weapon_flamethrower;
								this.Bots[(int)_index].mySound.csas.Play();
							}
						}, i, null);
					}
					Vector3 vector = this.Bots[i].oldpos - this.Bots[i].position;
					int num = 0;
					if (this.Bots[i].inVehicle)
					{
						num = 5;
					}
					else
					{
						if (this.Bots[i].State == 2)
						{
							num = 3;
						}
						if ((double)vector.magnitude > 0.05)
						{
							if (this.Bots[i].State == 1 || this.Bots[i].State == 3 || this.Bots[i].State == 4)
							{
								num = 1;
							}
							else if (this.Bots[i].State == 2)
							{
								num = 4;
							}
							else
							{
								num = 2;
							}
						}
					}
					if (num != this.Bots[i].AnimState)
					{
						this.Bots[i].AnimState = num;
						if (num == 0)
						{
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(0f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(false);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(false);
						}
						else if (num == 1)
						{
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(50f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(false);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(false);
						}
						else if (num == 2)
						{
							if (this.map == null)
							{
								this.map = Object.FindObjectOfType<Map>();
							}
							this.Bots[i].b = this.map.GetBlock((int)this.BotsGmObj[i].transform.position.x, (int)this.BotsGmObj[i].transform.position.y, (int)this.BotsGmObj[i].transform.position.z).block;
							this.Bots[i].bUp = this.map.GetBlock((int)this.BotsGmObj[i].transform.position.x, (int)this.BotsGmObj[i].transform.position.y + 1, (int)this.BotsGmObj[i].transform.position.z).block;
							if (this.Bots[i].bUp != null && this.Bots[i].bUp.GetName() == "!Water")
							{
								this.Bots[i].b = this.Bots[i].bUp;
							}
							if (this.Bots[i].b != null)
							{
								if (ZipLoader.GetBlockType(this.Bots[i].b) != this.Bots[i].currBlockType)
								{
									this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
									this.Bots[i].currBlockType = ZipLoader.GetBlockType(this.Bots[i].b);
								}
							}
							else if (1 != this.Bots[i].currBlockType)
							{
								this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
								this.Bots[i].currBlockType = 1;
							}
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Walk(this.Bots[i].currBlockType);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(150f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(false);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(false);
						}
						else if (num == 3)
						{
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(0f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(true);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(false);
						}
						else if (num == 4)
						{
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(50f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(true);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(false);
						}
						else if (num == 5)
						{
							this.BotsGmObj[i].GetComponent<Sound>().PlaySound_Stop(null);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetSpeed(0f);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetCrouch(false);
							this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetJeepGunner(true);
						}
					}
					if (num != 5 || this.Bots[i].inVehiclePos == CONST.VEHICLES.POSITION_JEEP_DRIVER)
					{
						this.BotsGmObj[i].transform.position = Vector3.Lerp(this.BotsGmObj[i].transform.position, this.Bots[i].position, Time.deltaTime * 5f);
						float num2 = this.BotsGmObj[i].transform.eulerAngles.y;
						float num3 = this.Bots[i].rotation.y - num2;
						if (num3 > 180f)
						{
							num2 += 360f;
						}
						if (num3 < -180f)
						{
							num2 -= 360f;
						}
						num2 = Mathf.Lerp(num2, this.Bots[i].rotation.y, Time.deltaTime * 5f);
						this.BotsGmObj[i].transform.eulerAngles = new Vector3(0f, num2, 0f);
						this.BotsGmObj[i].GetComponent<PlayerAnimation>().SetRotation(this.Bots[i].rotation.x);
					}
				}
			}
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0001878C File Offset: 0x0001698C
	public void SetPosition(int index, float pX, float pY, float pZ)
	{
		this.pd.PackPlayerPos(index, pX, pY, pZ);
		this.BotsGmObj[index].transform.position = new Vector3(pX, pY, pZ);
		this.Bots[index].oldpos = this.Bots[index].position;
		this.Bots[index].position = new Vector3(pX, pY, pZ);
	}

	// Token: 0x06000142 RID: 322 RVA: 0x000187F4 File Offset: 0x000169F4
	public void UpdatePosition(int index, float pX, float pY, float pZ, float rX, float rY, float rZ, int state)
	{
		if (this.Bots[index].Dead == 1)
		{
			return;
		}
		if (this.Bots[index].Team == 255)
		{
			return;
		}
		this.Bots[index].State = state;
		this.pd.CheckPlayerPos(index);
		this.pd.PackPlayerPos(index, pX, pY, pZ);
		if (!this.Bots[index].Active)
		{
			this.BotsGmObj[index].transform.position = new Vector3(pX, pY, pZ);
			this.BotsGmObj[index].transform.eulerAngles = new Vector3(0f, rY, 0f);
		}
		this.Bots[index].oldpos = this.Bots[index].position;
		this.Bots[index].position = new Vector3(pX, pY, pZ);
		this.Bots[index].rotation = new Vector3(rX, rY, 0f);
		if (this.Bots[index].zombie && Time.time > this.zmupdate)
		{
			this.zmupdate = Time.time + 10f;
			this.BotsGmObj[index].GetComponent<Sound>().PlaySound_ZM_Ambient();
		}
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00018929 File Offset: 0x00016B29
	public void UpdateBlock(int x, int y, int z, int health, bool fx)
	{
		if (health == 0)
		{
			base.StartCoroutine(this.UpdateBlock_coroutine(x, y, z, health, fx, true));
			return;
		}
		base.StartCoroutine(this.UpdateBlock_coroutine(x, y, z, health, fx, false));
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0001895A File Offset: 0x00016B5A
	private IEnumerator UpdateBlock_coroutine(int x, int y, int z, int health, bool fx, bool del)
	{
		float time = Time.time;
		if (time < this.lastupdate + 0.01f)
		{
			this.lastupdate = time;
			yield return new WaitForSeconds(0.01f);
		}
		else
		{
			this.lastupdate = time;
		}
		if (del)
		{
			this.map.SetBlockAndRecompute(default(BlockData), new Vector3i(x, y, z));
		}
		yield break;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x00018988 File Offset: 0x00016B88
	public void JoinTeamClass(int index, int _team)
	{
		this.Bots[index].Team = (byte)_team;
		this.BotsGmObj[index].GetComponent<TeamColor>().SetTeam(_team, this.Bots[index].Skin, this.Bots[index].goHelmet, this.Bots[index].goCap, this.Bots[index].Znak);
	}

	// Token: 0x06000146 RID: 326 RVA: 0x000189EC File Offset: 0x00016BEC
	public void SetAttack(int index, int weaponid)
	{
		if (weaponid == CONST.VEHICLES.VEHICLE_MODUL_TANK_MG)
		{
			if (this.BotsGmObj[index].GetComponentInChildren<Tank>() == null)
			{
				return;
			}
			this.BotsGmObj[index].GetComponent<Sound>().PlaySound_WeaponMGTank(this.BotsGmObj[index].GetComponent<AudioSource>());
			this.BotsGmObj[index].GetComponentInChildren<Tank>().MGFlash.gameObject.SetActive(true);
			this.BotsGmObj[index].GetComponentInChildren<Tank>().FlashTime = Time.time + 0.05f;
			return;
		}
		else
		{
			if (weaponid != 136)
			{
				if (this.Bots[index].WeaponID != weaponid)
				{
					this.SetCurrentWeapon(index, weaponid);
					this.Bots[index].WeaponID = weaponid;
				}
				if (this.Bots[index].mySound == null)
				{
					this.Bots[index].mySound = this.BotsGmObj[index].GetComponent<Sound>();
				}
				if (weaponid != 125)
				{
					if (this.Bots[index].mySound.csas.loop)
					{
						this.Bots[index].mySound.csas.loop = false;
					}
					this.Bots[index].mySound.PlaySound_Weapon(weaponid);
				}
				if (this.Bots[index].flash[weaponid] != null)
				{
					this.Bots[index].flash[weaponid].SetActive(true);
					if (weaponid == 1)
					{
						this.Bots[index].flash_time = Time.time + 0.05f;
						return;
					}
					if (weaponid == 125)
					{
						if (this.Bots[index].flamePS != null && !this.Bots[index].flamePS.isPlaying)
						{
							this.Bots[index].flamePS.Play(true);
						}
						this.Bots[index].flash_time = Time.time + 0.15f;
						return;
					}
					this.Bots[index].flash_time = Time.time + 0.01f;
				}
				return;
			}
			Car car = this.BotsGmObj[index].GetComponentInChildren<Car>();
			if (car == null)
			{
				car = this.BotsGmObj[index].GetComponentInParent<Car>();
			}
			if (car == null)
			{
				return;
			}
			this.BotsGmObj[index].GetComponent<Sound>().PlaySound_WeaponMGTank(this.BotsGmObj[index].GetComponent<AudioSource>());
			car.MGFlash.gameObject.SetActive(true);
			car.FlashTime = Time.time + 0.05f;
			return;
		}
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00018C48 File Offset: 0x00016E48
	public void SetBlock(int x, int y, int z, int team)
	{
		if (team < 0 || team > 7)
		{
			return;
		}
		if (x < 0 || x >= 256 || y < 0 || y >= 64 || z < 0 || z >= 256)
		{
			return;
		}
		BlockData block = new BlockData(this.teamblock[team]);
		block.SetDirection(RemotePlayersUpdater.GetDirection(-base.transform.forward));
		this.map.SetBlockAndRecompute(block, new Vector3i(x, y, z));
	}

	// Token: 0x06000148 RID: 328 RVA: 0x00018CC4 File Offset: 0x00016EC4
	public void CreateDeadEvent(int attackerid, int victimid, int weaponid)
	{
		if (attackerid >= 32)
		{
			attackerid = victimid;
		}
		this.Bots[victimid].Dead = 1;
		int team = (int)this.Bots[victimid].Team;
		int skin = this.Bots[victimid].Skin;
		if (this.Bots[victimid].zombie)
		{
			team = 0;
			skin = 1;
		}
		bool self = false;
		if (attackerid == this.cscl.myindex)
		{
			self = true;
		}
		if (this.Bots[victimid].weapon[this.Bots[victimid].WeaponID] != null && this.BotsGmObj[attackerid] != null)
		{
			int num = 0;
			if (this.Bots[victimid].Item[198] > 0)
			{
				num = 4;
			}
			this.csrm.CreateWeapon(this.Bots[victimid].weapon[this.Bots[victimid].WeaponID], this.Bots[victimid].weapon[this.Bots[victimid].WeaponID].transform.eulerAngles, this.BotsGmObj[attackerid].transform, this.Bots[victimid].WeaponID, weaponid, this.csig.GetBlockTextureTeam((int)this.Bots[victimid].Team + num));
		}
		this.csrm.CreatePlayerRagDoll(this.BotsGmObj[victimid], this.BotsGmObj[attackerid], victimid, false, team, skin, weaponid, this.Bots[victimid].goHelmet.GetComponent<Renderer>().enabled || this.Bots[victimid].goCap.GetComponent<Renderer>().enabled, self, this.Bots[victimid].goTykva.GetComponent<Renderer>().enabled, this.Bots[victimid].goKolpak.GetComponent<Renderer>().enabled, this.Bots[victimid].goRoga.GetComponent<Renderer>().enabled, this.Bots[victimid].goMaskBear.GetComponent<Renderer>().enabled, this.Bots[victimid].goMaskFox.GetComponent<Renderer>().enabled, this.Bots[victimid].goMaskRabbit.GetComponent<Renderer>().enabled);
	}

	// Token: 0x06000149 RID: 329 RVA: 0x00018EDC File Offset: 0x000170DC
	public void CreateDeadEventSelf(int attackerid, int victimid, int weaponid)
	{
		if (attackerid >= 32)
		{
			attackerid = victimid;
		}
		if (this.CurrentPlayer.GetComponentInChildren<Tank>() != null)
		{
			this.CurrentPlayer.GetComponent<TankController>().currTank = null;
			this.CurrentPlayer.GetComponent<TankController>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.NONE;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().zoom = false;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().enabled = false;
			GameObject.Find("RayCastBox").transform.position = new Vector3(-1000f, -1000f, -1000f);
			GameObject.Find("RayCastBox").transform.eulerAngles = new Vector3(-1000f, -1000f, -1000f);
			this.CurrentPlayer.GetComponentInChildren<Tank>().KillSelf();
		}
		else if (this.CurrentPlayer.GetComponentInChildren<Car>() != null)
		{
			this.CurrentPlayer.GetComponent<CarController>().currCar = null;
			this.CurrentPlayer.GetComponent<CarController>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.NONE;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().zoom = false;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().enabled = false;
			GameObject.Find("RayCastBox").transform.position = new Vector3(-1000f, -1000f, -1000f);
			GameObject.Find("RayCastBox").transform.eulerAngles = new Vector3(-1000f, -1000f, -1000f);
			this.CurrentPlayer.GetComponentInChildren<Car>().KillSelf();
		}
		else if (this.CurrentPlayer.GetComponentInParent<Car>() != null)
		{
			Car componentInParent = this.CurrentPlayer.GetComponentInParent<Car>();
			this.CurrentPlayer.GetComponent<CarController>().currCar = null;
			this.CurrentPlayer.GetComponent<CarController>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().enabled = false;
			this.CurrentPlayer.GetComponent<TransportExit>().vehicleType = CONST.VEHICLES.NONE;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().zoom = false;
			this.CurrentPlayer.GetComponentInChildren<OrbitCam>().enabled = false;
			GameObject.Find("RayCastBox").transform.position = new Vector3(-1000f, -1000f, -1000f);
			GameObject.Find("RayCastBox").transform.eulerAngles = new Vector3(-1000f, -1000f, -1000f);
			this.CurrentPlayer.transform.parent = null;
			if (this.CurrentPlayer.GetComponent<CarController>().myPosition != CONST.VEHICLES.POSITION_JEEP_GUNNER)
			{
				componentInParent.KillSelf();
			}
		}
		this.Bots[victimid].Dead = 1;
		this.BotsGmObj[victimid].transform.position = this.CurrentPlayer.transform.position;
		if (this.MG == null)
		{
			this.MG = (Minigun)Object.FindObjectOfType(typeof(Minigun));
		}
		if (this.MG != null)
		{
			this.MG.speedUp = false;
		}
		GameObject head = this.csrm.CreatePlayerRagDoll(this.BotsGmObj[victimid], this.BotsGmObj[attackerid], victimid, true, 0, 0, weaponid, false, false, false, false, false, false, false, false);
		this.CurrentPlayer.GetComponentInChildren<vp_FPCamera>().enabled = false;
		this.SkinManager.SpawnCamera(head);
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00019280 File Offset: 0x00017480
	public void SetCurrentWeapon(int id, int weaponid)
	{
		for (int i = 0; i < 500; i++)
		{
			if (this.Bots[id].weapon[i])
			{
				this.Bots[id].weapon[i].SetActive(false);
			}
			if (this.Bots[id].flash[i] && i != 315)
			{
				this.Bots[id].flash[i].SetActive(false);
			}
		}
		this.Bots[id].WeaponID = weaponid;
		if (this.cscl && this.cscl.myindex == id)
		{
			return;
		}
		if (this.Bots[id].weapon[weaponid])
		{
			this.Bots[id].weapon[weaponid].SetActive(true);
		}
		if (weaponid == 0 && id != PlayerProfile.myindex && ConnectionInfo.mode != 2)
		{
			int num = 0;
			if (this.Bots[id].Item[198] > 0)
			{
				num = 4;
			}
			this.csig.SetBlockTextureTeam(this.Bots[id].m_Face, this.Bots[id].m_Top, (int)this.Bots[id].Team + num, false);
			return;
		}
		if (id != PlayerProfile.myindex && ConnectionInfo.mode == 2)
		{
			this.csig.SetBlockTextureForBuild(this.Bots[id].m_Face, this.Bots[id].m_Top, 0);
			this.Bots[id].blockFlag = 0;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x000193F7 File Offset: 0x000175F7
	public void SetCurrentWeaponBlock(int id, int flag)
	{
		if (flag < 0)
		{
			flag = 0;
		}
		this.csig.SetBlockTextureForBuild(this.Bots[id].m_Face, this.Bots[id].m_Top, flag);
		this.Bots[id].blockFlag = flag;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x00019434 File Offset: 0x00017634
	private static BlockDirection GetDirection(Vector3 dir)
	{
		if (Mathf.Abs(dir.z) >= Mathf.Abs(dir.x))
		{
			if (dir.z >= 0f)
			{
				return BlockDirection.Z_PLUS;
			}
			return BlockDirection.Z_MINUS;
		}
		else
		{
			if (dir.x >= 0f)
			{
				return BlockDirection.X_PLUS;
			}
			return BlockDirection.X_MINUS;
		}
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00019470 File Offset: 0x00017670
	public void PhysicsBlock(List<Vector3i> pos)
	{
		if (pos.Count == 0)
		{
			return;
		}
		Vector3 vector = Camera.main.transform.position - pos[0];
		if (vector.magnitude <= 128f)
		{
			GameObject gameObject = new GameObject("destroyed", new Type[]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			BlockSet blockSet = this.map.GetBlockSet();
			MeshBuilder meshBuilder = new MeshBuilder();
			meshBuilder.Clear();
			foreach (Vector3i vector3i in pos)
			{
				CubeBuilder.Build(vector3i, vector3i, this.map, meshBuilder, false);
			}
			component.sharedMesh = meshBuilder.ToMesh(component.sharedMesh);
			if (component.sharedMesh == null)
			{
				Object.Destroy(gameObject);
				return;
			}
			gameObject.GetComponent<Renderer>().sharedMaterials = blockSet.GetMaterials(component.sharedMesh.subMeshCount);
			gameObject.AddComponent<dk>();
			gameObject.AddComponent<Rigidbody>();
			gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
			if (pos.Count < 32 && vector.magnitude < 64f)
			{
				gameObject.AddComponent<BoxCollider>();
				gameObject.layer = 8;
				gameObject.GetComponent<Rigidbody>().AddTorque(gameObject.transform.right * 40f + gameObject.transform.forward * 20f);
			}
		}
		foreach (Vector3i vector3i2 in pos)
		{
			base.StartCoroutine(this.UpdateBlock_coroutine(vector3i2.x, vector3i2.y, vector3i2.z, 0, false, true));
		}
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00019668 File Offset: 0x00017868
	public void CreateFX(float pX, float pY, float pZ)
	{
		new GameObject("destroyed_fx", new Type[]
		{
			typeof(Explode)
		}).transform.position = new Vector3(pX, pY, pZ);
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000248C File Offset: 0x0000068C
	public void SetController(int id, int cid)
	{
	}

	// Token: 0x06000150 RID: 336 RVA: 0x00019699 File Offset: 0x00017899
	public void SetZombie()
	{
		this.csws.SetZombieWeapon();
	}

	// Token: 0x04000152 RID: 338
	public static RemotePlayersUpdater Instance;

	// Token: 0x04000153 RID: 339
	public GameObject pgoPlayer;

	// Token: 0x04000154 RID: 340
	public GameObject pgoLocalPlayer;

	// Token: 0x04000155 RID: 341
	public GameObject pgoPlayerCreated;

	// Token: 0x04000156 RID: 342
	private GameObject CurrentPlayer;

	// Token: 0x04000157 RID: 343
	private GameObject Gui;

	// Token: 0x04000158 RID: 344
	public GameObject[] BotsGmObj;

	// Token: 0x04000159 RID: 345
	public BotData[] Bots = new BotData[32];

	// Token: 0x0400015A RID: 346
	private Block[] teamblock = new Block[8];

	// Token: 0x0400015B RID: 347
	private bool PlayersLoaded;

	// Token: 0x0400015C RID: 348
	private PackData pd;

	// Token: 0x0400015D RID: 349
	private SpawnManager SkinManager;

	// Token: 0x0400015E RID: 350
	private RagDollManager csrm;

	// Token: 0x0400015F RID: 351
	private MainGUI csig;

	// Token: 0x04000160 RID: 352
	private Map map;

	// Token: 0x04000161 RID: 353
	private Client cscl;

	// Token: 0x04000162 RID: 354
	private Minigun MG;

	// Token: 0x04000163 RID: 355
	private WeaponSystem csws;

	// Token: 0x04000164 RID: 356
	private float zmupdate;

	// Token: 0x04000165 RID: 357
	private float lastupdate;
}
