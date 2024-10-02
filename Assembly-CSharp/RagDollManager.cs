using System;
using AssemblyCSharp;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class RagDollManager : MonoBehaviour
{
	// Token: 0x060001FC RID: 508 RVA: 0x00024FAC File Offset: 0x000231AC
	public GameObject CreatePlayerRagDoll(GameObject original, GameObject attacker, int victimID, bool hide, int team, int skin, int weaponid, bool helmet, bool self, bool tykva, bool kolpak, bool roga, bool mask_bear, bool mask_fox, bool mask_rabbit)
	{
		bool flag = false;
		GameObject gameObject2;
		if (original.GetComponentInChildren<Tank>() != null)
		{
			GameObject.Find(original.name + "/Bip001").layer = 0;
			Transform[] componentsInChildren = GameObject.Find(original.name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			GameObject.Find(original.name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = true;
			GameObject gameObject = Object.Instantiate<GameObject>(ContentLoader.LoadGameObject("Dtank2"));
			gameObject2 = Object.Instantiate<GameObject>(this.pgoPlayerRagDoll);
			gameObject.AddComponent<ragdoll_wreck>();
			gameObject.name = "deadbody_" + this.g_deadbody_index.ToString();
			gameObject2.name = "deadbody_tankist_" + this.g_deadbody_index.ToString();
			gameObject.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
			gameObject.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 0.7f, 0f);
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			original.GetComponentInChildren<Tank>().KillSelf();
			gameObject.GetComponentInChildren<Detonator>().enabled = true;
		}
		else if (original.GetComponentInChildren<Car>() != null)
		{
			Car componentInChildren = original.GetComponentInChildren<Car>();
			GameObject.Find(original.name + "/Bip001").layer = 0;
			Transform[] componentsInChildren = GameObject.Find(original.name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			GameObject.Find(original.name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = true;
			GameObject gameObject = Object.Instantiate<GameObject>(this.pgoDJeep);
			gameObject2 = Object.Instantiate<GameObject>(this.pgoPlayerRagDoll);
			gameObject.AddComponent<ragdoll_wreck>();
			gameObject.name = "deadbody_" + this.g_deadbody_index.ToString();
			gameObject2.name = "deadbody_tankist_" + this.g_deadbody_index.ToString();
			gameObject.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
			gameObject.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
			gameObject2.transform.parent = gameObject.transform;
			gameObject2.transform.localPosition = new Vector3(0f, 0.7f, 0f);
			gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
			original.transform.parent = null;
			componentInChildren.KillSelf();
			gameObject.GetComponentInChildren<Detonator>().enabled = true;
		}
		else if (original.GetComponentInParent<Car>() != null)
		{
			Car componentInParent = original.GetComponentInParent<Car>();
			GameObject.Find(original.name + "/Bip001").layer = 0;
			Transform[] componentsInChildren = GameObject.Find(original.name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			GameObject.Find(original.name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = true;
			if (componentInParent.slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] == victimID && (componentInParent.slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] != CONST.VEHICLES.POSITION_NONE || componentInParent.slots[CONST.VEHICLES.POSITION_JEEP_PASS] != CONST.VEHICLES.POSITION_NONE))
			{
				gameObject2 = Object.Instantiate<GameObject>(this.pgoPlayerRagDoll);
				gameObject2.name = "deadbody_" + this.g_deadbody_index.ToString();
				gameObject2.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
				gameObject2.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
				original.transform.parent = null;
			}
			else
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.pgoDJeep);
				gameObject2 = Object.Instantiate<GameObject>(this.pgoPlayerRagDoll);
				gameObject.AddComponent<ragdoll_wreck>();
				gameObject.name = "deadbody_" + this.g_deadbody_index.ToString();
				gameObject2.name = "deadbody_tankist_" + this.g_deadbody_index.ToString();
				gameObject.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
				gameObject.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
				gameObject2.transform.parent = gameObject.transform;
				gameObject2.transform.localPosition = new Vector3(0f, 0.7f, 0f);
				gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
				original.transform.parent = null;
				componentInParent.KillSelf();
				gameObject.GetComponentInChildren<Detonator>().enabled = true;
			}
		}
		else
		{
			gameObject2 = Object.Instantiate<GameObject>(this.pgoPlayerRagDoll);
			gameObject2.name = "deadbody_" + this.g_deadbody_index.ToString();
			gameObject2.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
			gameObject2.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
		}
		GameObject gameObject3 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head");
		GameObject gameObject4 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine");
		attacker.transform.LookAt(gameObject3.transform.position);
		if (weaponid == 10 || weaponid == 7 || weaponid == 168 || weaponid == 170 || weaponid == 185 || weaponid == 100 || weaponid == 55 || weaponid == 172 || weaponid == 77 || weaponid == 138 || weaponid == 62 || weaponid == 183 || weaponid == 171 || weaponid == 156)
		{
			gameObject3.GetComponent<Rigidbody>().AddForce(attacker.transform.up * 10000f);
			gameObject3.GetComponent<Rigidbody>().AddTorque(attacker.transform.right * 500f + attacker.transform.forward * 1000f);
		}
		else if (weaponid == 43 || weaponid == 17 || weaponid == 70 || weaponid == 102 || weaponid == 139 || weaponid == 145)
		{
			if (self)
			{
				gameObject4.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 6000f + Camera.main.transform.up * 1000f);
				gameObject4.GetComponent<Rigidbody>().AddTorque(Camera.main.transform.up * 1000f);
			}
			else
			{
				gameObject4.GetComponent<Rigidbody>().AddForce(attacker.transform.forward * 6000f + attacker.transform.up * 1000f);
				gameObject4.GetComponent<Rigidbody>().AddTorque(attacker.transform.up * 1000f);
			}
		}
		else if (weaponid == 47 || weaponid == 111)
		{
			if (self)
			{
				gameObject4.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 10000f + Camera.main.transform.up * 1000f);
				gameObject4.GetComponent<Rigidbody>().AddTorque(Camera.main.transform.up * 1000f);
			}
			else
			{
				gameObject4.GetComponent<Rigidbody>().AddForce(attacker.transform.forward * 10000f + attacker.transform.up * 1000f);
				gameObject4.GetComponent<Rigidbody>().AddTorque(attacker.transform.up * 1000f);
			}
		}
		else if (self)
		{
			gameObject3.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 3000f + Camera.main.transform.up * 500f);
			gameObject3.GetComponent<Rigidbody>().AddTorque(Camera.main.transform.up * 1000f);
		}
		else
		{
			gameObject3.GetComponent<Rigidbody>().AddForce(attacker.transform.forward * 3000f + attacker.transform.up * 500f);
			gameObject3.GetComponent<Rigidbody>().AddTorque(attacker.transform.up * 1000f);
		}
		if (!flag)
		{
			GameObject gameObject5 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Helmet");
			GameObject gameObject6 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Cap");
			GameObject gameObject7 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/Tykva");
			GameObject gameObject8 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/KOLPAK");
			GameObject gameObject9 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/ROGA");
			GameObject gameObject10 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_BEAR");
			GameObject gameObject11 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_FOX");
			GameObject gameObject12 = GameObject.Find(gameObject2.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/MASK_RABBIT");
			if (original.GetComponentInChildren<Arrow>() != null)
			{
				Arrow[] componentsInChildren2 = original.GetComponentsInChildren<Arrow>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].KillSelf();
				}
			}
			if (hide)
			{
				foreach (object obj in gameObject2.transform)
				{
					((Transform)obj).gameObject.layer = 9;
				}
				gameObject5.gameObject.layer = 9;
				gameObject6.gameObject.layer = 9;
				gameObject7.gameObject.layer = 9;
				gameObject8.gameObject.layer = 9;
				gameObject9.gameObject.layer = 9;
				gameObject10.gameObject.layer = 9;
				gameObject11.gameObject.layer = 9;
				gameObject12.gameObject.layer = 9;
			}
			GameObject gameObject13 = GameObject.Find(gameObject2.name + "/trooper");
			if (gameObject13)
			{
				SkinnedMeshRenderer component = gameObject13.GetComponent<SkinnedMeshRenderer>();
				component.material.mainTexture = SkinManager.GetSkin(team, skin);
				if (helmet)
				{
					gameObject5.GetComponent<Renderer>().material.mainTexture = component.material.mainTexture;
					gameObject6.GetComponent<Renderer>().material.mainTexture = component.material.mainTexture;
				}
				if (skin == 311)
				{
					Color value = Color.white;
					if (team == 0)
					{
						value = new Color(0f, 0.45f, 1f);
					}
					else if (team == 1)
					{
						value = Color.red;
					}
					else if (team == 2)
					{
						value = Color.green;
					}
					else if (team == 3)
					{
						value = Color.yellow;
					}
					component.material.SetColor("_EmissionColor", value);
					gameObject5.GetComponent<Renderer>().material.SetColor("_EmissionColor", value);
				}
				else
				{
					component.material.SetColor("_EmissionColor", Color.black);
					gameObject5.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
				}
			}
			if (gameObject2)
			{
				GameObject gameObject14 = GameObject.Find(gameObject2.name + "/trooper");
				gameObject14.GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetSkin(team, skin);
				if (helmet)
				{
					gameObject5.GetComponent<Renderer>().material.mainTexture = SkinManager.GetSkin(team, skin);
				}
				if (skin == 311)
				{
					Color value2 = Color.white;
					if (team == 0)
					{
						value2 = new Color(0f, 0.45f, 1f);
					}
					else if (team == 1)
					{
						value2 = Color.red;
					}
					else if (team == 2)
					{
						value2 = Color.green;
					}
					else if (team == 3)
					{
						value2 = Color.yellow;
					}
					gameObject14.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", value2);
					gameObject5.GetComponent<Renderer>().material.SetColor("_EmissionColor", value2);
				}
				else
				{
					gameObject14.GetComponent<SkinnedMeshRenderer>().material.SetColor("_EmissionColor", Color.black);
					gameObject5.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
				}
			}
			if (!helmet)
			{
				gameObject5.GetComponent<Renderer>().enabled = false;
				gameObject6.GetComponent<Renderer>().enabled = false;
			}
			else if (skin == 97 || skin == 98 || skin == 99)
			{
				gameObject5.GetComponent<Renderer>().enabled = false;
			}
			else
			{
				gameObject6.GetComponent<Renderer>().enabled = false;
			}
			if (!tykva)
			{
				gameObject7.GetComponent<Renderer>().enabled = false;
			}
			if (!kolpak)
			{
				gameObject8.GetComponent<Renderer>().enabled = false;
			}
			if (!roga)
			{
				gameObject9.GetComponent<Renderer>().enabled = false;
			}
			if (!mask_bear)
			{
				gameObject10.GetComponent<Renderer>().enabled = false;
			}
			if (!mask_fox)
			{
				gameObject11.GetComponent<Renderer>().enabled = false;
			}
			if (!mask_rabbit)
			{
				gameObject12.GetComponent<Renderer>().enabled = false;
			}
		}
		original.transform.position = new Vector3(-1000f, -1000f, -1000f);
		this.g_deadbody_index++;
		return gameObject3;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00026000 File Offset: 0x00024200
	public void CreateHelmetRagDoll(GameObject original, int team, int skin, int _headAttach = 0)
	{
		GameObject gameObject = GameObject.Find(original.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head");
		GameObject gameObject2;
		if (_headAttach > 0)
		{
			if (_headAttach == 211)
			{
				gameObject2 = Object.Instantiate<GameObject>(this.pgoTykvaRagDoll);
			}
			else
			{
				gameObject2 = Object.Instantiate<GameObject>(this.pgoHelmetRagDoll);
			}
		}
		else if (skin == 97 || skin == 98 || skin == 99)
		{
			gameObject2 = Object.Instantiate<GameObject>(this.pgoCapRagDoll);
		}
		else
		{
			gameObject2 = Object.Instantiate<GameObject>(this.pgoHelmetRagDoll);
		}
		gameObject2.layer = 10;
		gameObject2.transform.position = gameObject.transform.position + gameObject.transform.up * 0.35f;
		gameObject2.transform.rotation = gameObject.transform.rotation;
		gameObject2.GetComponent<Rigidbody>().AddForce(original.transform.up * 300f + original.transform.forward * -50f);
		gameObject2.GetComponent<Rigidbody>().AddTorque(original.transform.up * 150f + -original.transform.forward * 150f);
		if (_headAttach != 211)
		{
			gameObject2.GetComponent<Renderer>().material.mainTexture = SkinManager.GetSkin(team, skin);
			if (skin == 311)
			{
				Color value = Color.white;
				if (team == 0)
				{
					value = new Color(0f, 0.45f, 1f);
				}
				else if (team == 1)
				{
					value = Color.red;
				}
				else if (team == 2)
				{
					value = Color.green;
				}
				else if (team == 3)
				{
					value = Color.yellow;
				}
				gameObject2.GetComponent<Renderer>().material.SetColor("_EmissionColor", value);
			}
			else
			{
				gameObject2.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.black);
			}
		}
		gameObject2.AddComponent<ragdoll_wreck>();
	}

	// Token: 0x060001FE RID: 510 RVA: 0x000261E4 File Offset: 0x000243E4
	public void CreateGrenade(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoGrenade, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_GRENADE;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.GetComponent<M61>().id = id;
		cent.go.GetComponent<M61>().uid = uid;
		cent.go.GetComponent<M61>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x060001FF RID: 511 RVA: 0x000262B8 File Offset: 0x000244B8
	public void CreateSTIELHANDGRANATE(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoSTIELHANDGRANATE, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_STIELHANDGRANATE;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<M61>();
		cent.go.GetComponent<M61>().id = id;
		cent.go.GetComponent<M61>().uid = uid;
		cent.go.GetComponent<M61>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00026398 File Offset: 0x00024598
	public void CreateM18(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoM18, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_SMOKE_GRENADE;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<SmokeGrenade>();
		cent.go.GetComponent<SmokeGrenade>().id = id;
		cent.go.GetComponent<SmokeGrenade>().uid = uid;
		cent.go.GetComponent<SmokeGrenade>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000201 RID: 513 RVA: 0x00026478 File Offset: 0x00024678
	public void CreateGG(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoGG, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_GAZ_GRENADE;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<SmokeGrenade>();
		cent.go.GetComponent<SmokeGrenade>().id = id;
		cent.go.GetComponent<SmokeGrenade>().uid = uid;
		cent.go.GetComponent<SmokeGrenade>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000202 RID: 514 RVA: 0x00026558 File Offset: 0x00024758
	public void CreateMolotov(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoMOLOTOV, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_MOLOTOV;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.GetComponent<Molotov>().id = id;
		cent.go.GetComponent<Molotov>().uid = uid;
		cent.go.GetComponent<Molotov>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000203 RID: 515 RVA: 0x0002662C File Offset: 0x0002482C
	public void CreateMK3(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoMK3, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_HE_GRENADE;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<M61>();
		cent.go.GetComponent<M61>().id = id;
		cent.go.GetComponent<M61>().uid = uid;
		cent.go.GetComponent<M61>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000204 RID: 516 RVA: 0x0002670C File Offset: 0x0002490C
	public void CreateRKG3(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoRKG3, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_RKG3;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<GrenadeRKG3>();
		cent.go.GetComponent<GrenadeRKG3>().id = id;
		cent.go.GetComponent<GrenadeRKG3>().uid = uid;
		cent.go.GetComponent<GrenadeRKG3>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000205 RID: 517 RVA: 0x000267EC File Offset: 0x000249EC
	public void CreateRocket(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoRocket, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_SHMEL;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.GetComponent<Rocket>().id = id;
		cent.go.GetComponent<Rocket>().uid = uid;
		cent.go.GetComponent<Rocket>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
		GameObject gameObject = GameObject.Find("/" + cent.go.name + "/rpg");
		gameObject.GetComponent<Rocket>().id = id;
		gameObject.GetComponent<Rocket>().uid = uid;
		gameObject.GetComponent<Rocket>().entid = cent.index;
	}

	// Token: 0x06000206 RID: 518 RVA: 0x00026920 File Offset: 0x00024B20
	public void CreateM202Rocket(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoM202, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_M202;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponent<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.GetComponent<M202Rocket>().id = id;
		cent.go.GetComponent<M202Rocket>().uid = uid;
		cent.go.GetComponent<M202Rocket>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000207 RID: 519 RVA: 0x00026A0C File Offset: 0x00024C0C
	public void CreateZombie(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoZombie, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_ZOMBIE;
		if (cent == null)
		{
			return;
		}
		cent.go.name = "ent_" + uid.ToString();
		cent.uid = uid;
		cent.position = new Vector3(px, py, pz);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head").AddComponent<Data>().SetIndex(cent.uid, 1);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine").AddComponent<Data>().SetIndex(cent.uid, 0);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh").AddComponent<Data>().SetIndex(cent.uid, 11);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf").AddComponent<Data>().SetIndex(cent.uid, 12);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf/Bip001 L Foot").AddComponent<Data>().SetIndex(cent.uid, 13);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh").AddComponent<Data>().SetIndex(cent.uid, 8);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf").AddComponent<Data>().SetIndex(cent.uid, 9);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf/Bip001 R Foot").AddComponent<Data>().SetIndex(cent.uid, 10);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm").AddComponent<Data>().SetIndex(cent.uid, 5);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm").AddComponent<Data>().SetIndex(cent.uid, 6);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand").AddComponent<Data>().SetIndex(cent.uid, 7);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm").AddComponent<Data>().SetIndex(cent.uid, 2);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm").AddComponent<Data>().SetIndex(cent.uid, 3);
		GameObject.Find(cent.go.name + "/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand").AddComponent<Data>().SetIndex(cent.uid, 4);
		cent.team = 1;
		cent.skin = Random.Range(1, 4);
		SkinnedMeshRenderer componentInChildren = cent.go.GetComponentInChildren<SkinnedMeshRenderer>();
		componentInChildren.material.mainTexture = SkinManager.GetSkin(cent.team, cent.skin);
		if (cent.skin == 311)
		{
			Color value = Color.white;
			if (cent.team == 0)
			{
				value = new Color(0f, 0.45f, 1f);
			}
			else if (cent.team == 1)
			{
				value = Color.red;
			}
			else if (cent.team == 2)
			{
				value = Color.green;
			}
			else if (cent.team == 3)
			{
				value = Color.yellow;
			}
			componentInChildren.material.SetColor("_EmissionColor", value);
		}
		else
		{
			componentInChildren.material.SetColor("_EmissionColor", Color.black);
		}
		cent.go.AddComponent<NpcLerp>().ent = cent;
		cent.go.AddComponent<FX>().Infect();
		cent.go.GetComponent<Animator>().SetBool("isZombie", true);
		cent.go.AddComponent<Sound>().PlaySound_ZM_Infected();
		cent.go.GetComponent<AudioSource>().clip = ContentLoader.LoadSound("zombie_walk");
		cent.go.GetComponent<AudioSource>().Play();
	}

	// Token: 0x06000208 RID: 520 RVA: 0x00026E24 File Offset: 0x00025024
	public void CreateGP(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoGP, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_GP;
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.GetComponent<GP>().id = id;
		cent.go.GetComponent<GP>().uid = uid;
		cent.go.GetComponent<GP>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
		cent.go.layer = 8;
		Collider[] componentsInChildren = cent.go.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 8;
		}
	}

	// Token: 0x06000209 RID: 521 RVA: 0x00026F2C File Offset: 0x0002512C
	public void CreateWeapon(GameObject _pos, Vector3 _rot, Transform attacker, int wid, int killerwid, Block _block)
	{
		int num = -1;
		int i = 0;
		int num2 = this.pgoWeapons.Length;
		while (i < num2)
		{
			if (!(this.pgoWeapons[i] == null))
			{
				string name = this.pgoWeapons[i].name;
				ITEM item = (ITEM)wid;
				if (!(name != item.ToString()))
				{
					num = i;
					break;
				}
			}
			i++;
		}
		if (num < 0)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>(this.pgoWeapons[num]);
		if (!gameObject)
		{
			return;
		}
		gameObject.transform.position = _pos.transform.position;
		gameObject.transform.eulerAngles = _rot;
		if (wid == 0 || wid == 198)
		{
			MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
			MeshRenderer componentInChildren = _pos.GetComponentInChildren<MeshRenderer>();
			if (!component || !componentInChildren)
			{
				Object.Destroy(gameObject);
				return;
			}
			component.material.mainTexture = _block.GetTexture();
			Rect previewFace = _block.GetPreviewFace();
			component.material.mainTextureOffset = new Vector2(previewFace.x, previewFace.y + 0.0625f);
			component.material.mainTextureScale = new Vector2(previewFace.width, previewFace.height);
		}
		if (killerwid == 10 || killerwid == 7 || killerwid == 168 || killerwid == 170 || killerwid == 185 || killerwid == 100 || killerwid == 55 || killerwid == 172 || killerwid == 77 || killerwid == 138 || killerwid == 62 || killerwid == 183 || killerwid == 171 || killerwid == 156)
		{
			if (wid == 221)
			{
				foreach (Rigidbody rigidbody in gameObject.GetComponentsInChildren<Rigidbody>())
				{
					rigidbody.AddForce(Vector3.up * 1000f);
					rigidbody.AddTorque(attacker.right * 50f + attacker.forward * 100f);
				}
				return;
			}
			gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000f);
			gameObject.GetComponent<Rigidbody>().AddTorque(attacker.right * 50f + attacker.forward * 100f);
			return;
		}
		else
		{
			if (wid == 221)
			{
				foreach (Rigidbody rigidbody2 in gameObject.GetComponentsInChildren<Rigidbody>())
				{
					rigidbody2.AddForce(attacker.forward * -200f);
					rigidbody2.AddTorque(attacker.right * 50f);
				}
				return;
			}
			gameObject.GetComponent<Rigidbody>().AddForce(attacker.forward * -200f);
			gameObject.GetComponent<Rigidbody>().AddTorque(attacker.right * 50f);
			return;
		}
	}

	// Token: 0x0600020A RID: 522 RVA: 0x00027210 File Offset: 0x00025410
	public void CreateMINEN(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoMinen, new Vector3(px, py, pz), new Vector3(-90f, ry, 0f));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_SHTURM_MINEN;
		cent.go.AddComponent<Minen>();
		cent.go.GetComponent<Minen>().id = id;
		cent.go.GetComponent<Minen>().uid = uid;
		cent.go.GetComponent<Minen>().entid = cent.index;
		cent.go.GetComponent<Minen>().targetPosition = new Vector3(fx, fy, fz);
		cent.go.name = "MINA_" + cent.index.ToString();
	}

	// Token: 0x0600020B RID: 523 RVA: 0x000272DC File Offset: 0x000254DC
	public void CreateTank(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(ContentLoader.LoadGameObject("TANK MEDIUM"), new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_TANK;
		cent.go.AddComponent<Tank>();
		cent.go.GetComponent<Tank>().id = id;
		cent.go.GetComponent<Tank>().uid = uid;
		cent.go.GetComponent<Tank>().entid = cent.index;
		cent.go.GetComponent<Tank>().dlina = 4.4f;
		cent.go.GetComponent<Tank>().shirina = 3.6f;
		cent.go.GetComponent<Tank>().classID = 13;
		cent.go.GetComponent<Tank>().tank_type = 1;
		cent.go.GetComponentInChildren<ParticleSystem>().Stop();
		cent.go.name = "TANK_" + cent.index.ToString();
		cent.go.GetComponent<Tank>().MG = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG").transform;
		cent.go.GetComponent<Tank>().MGFlash = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG/flash").transform;
		cent.go.GetComponent<Tank>().MG.gameObject.SetActive(false);
		cent.go.GetComponent<Tank>().MGFlash.gameObject.SetActive(false);
		if (id != 200)
		{
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject.Find(cent.go.name + "/blockade_tank_default").GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(1, Mathf.RoundToInt(tz));
			cent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[id].transform;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			cent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
			cent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		cent.go.layer = 20;
		cent.go.tag = "Tank";
		foreach (Collider collider in cent.go.GetComponentsInChildren<Collider>())
		{
			collider.tag = "Tank";
			collider.gameObject.layer = 20;
		}
	}

	// Token: 0x0600020C RID: 524 RVA: 0x00027648 File Offset: 0x00025848
	public void CreateTankLight(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(ContentLoader.LoadGameObject("TANK LIGHT"), new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_TANK_LIGHT;
		cent.go.AddComponent<Tank>();
		cent.go.GetComponent<Tank>().id = id;
		cent.go.GetComponent<Tank>().uid = uid;
		cent.go.GetComponent<Tank>().entid = cent.index;
		cent.go.GetComponent<Tank>().dlina = 4.4f;
		cent.go.GetComponent<Tank>().shirina = 3f;
		cent.go.GetComponent<Tank>().classID = 16;
		cent.go.GetComponent<Tank>().tank_type = 0;
		cent.go.GetComponentInChildren<ParticleSystem>().Stop();
		cent.go.name = "TANK_" + cent.index.ToString();
		cent.go.GetComponent<Tank>().MG = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG").transform;
		cent.go.GetComponent<Tank>().MGFlash = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG/flash").transform;
		cent.go.GetComponent<Tank>().MG.gameObject.SetActive(false);
		cent.go.GetComponent<Tank>().MGFlash.gameObject.SetActive(false);
		if (id != 200 && Mathf.RoundToInt(tx) > 0)
		{
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject.Find(cent.go.name + "/blockade_tank_default").GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			cent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[id].transform;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			cent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
			cent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		cent.go.layer = 20;
		cent.go.tag = "Tank";
		foreach (Collider collider in cent.go.GetComponentsInChildren<Collider>())
		{
			collider.tag = "Tank";
			collider.gameObject.layer = 20;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x000279C4 File Offset: 0x00025BC4
	public void CreateTankMedium(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(ContentLoader.LoadGameObject("TANK MEDIUM"), new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_TANK_MEDIUM;
		cent.go.AddComponent<Tank>();
		cent.go.GetComponent<Tank>().id = id;
		cent.go.GetComponent<Tank>().uid = uid;
		cent.go.GetComponent<Tank>().entid = cent.index;
		cent.go.GetComponent<Tank>().dlina = 4.4f;
		cent.go.GetComponent<Tank>().shirina = 3.6f;
		cent.go.GetComponent<Tank>().classID = 17;
		cent.go.GetComponent<Tank>().tank_type = 1;
		cent.go.GetComponentInChildren<ParticleSystem>().Stop();
		cent.go.name = "TANK_" + cent.index.ToString();
		cent.go.GetComponent<Tank>().MG = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG").transform;
		cent.go.GetComponent<Tank>().MGFlash = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG/flash").transform;
		cent.go.GetComponent<Tank>().MG.gameObject.SetActive(false);
		cent.go.GetComponent<Tank>().MGFlash.gameObject.SetActive(false);
		if (id != 200 && Mathf.RoundToInt(tx) > 0)
		{
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject.Find(cent.go.name + "/blockade_tank_default").GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			cent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[id].transform;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			cent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
			cent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		cent.go.layer = 20;
		cent.go.tag = "Tank";
		foreach (Collider collider in cent.go.GetComponentsInChildren<Collider>())
		{
			collider.tag = "Tank";
			collider.gameObject.layer = 20;
		}
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00027D40 File Offset: 0x00025F40
	public void CreateTankHeavy(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(ContentLoader.LoadGameObject("TANK HEAVY"), new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_TANK_HEAVY;
		cent.go.AddComponent<Tank>();
		cent.go.GetComponent<Tank>().id = id;
		cent.go.GetComponent<Tank>().uid = uid;
		cent.go.GetComponent<Tank>().entid = cent.index;
		cent.go.GetComponent<Tank>().dlina = 4.4f;
		cent.go.GetComponent<Tank>().shirina = 4.5f;
		cent.go.GetComponent<Tank>().classID = 18;
		cent.go.GetComponent<Tank>().tank_type = 2;
		cent.go.GetComponentInChildren<ParticleSystem>().Stop();
		cent.go.name = "TANK_" + cent.index.ToString();
		cent.go.GetComponent<Tank>().MG = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG").transform;
		cent.go.GetComponent<Tank>().MGFlash = GameObject.Find(cent.go.name + "/root/turret/barrel/module1/TANK MG/flash").transform;
		cent.go.GetComponent<Tank>().MG.gameObject.SetActive(false);
		cent.go.GetComponent<Tank>().MGFlash.gameObject.SetActive(false);
		if (id != 200 && Mathf.RoundToInt(tx) > 0)
		{
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject.Find(cent.go.name + "/blockade_tank_default").GetComponent<SkinnedMeshRenderer>().material.mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			cent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[id].transform;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			cent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
			cent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		cent.go.layer = 20;
		cent.go.tag = "Tank";
		foreach (Collider collider in cent.go.GetComponentsInChildren<Collider>())
		{
			collider.tag = "Tank";
			collider.gameObject.layer = 20;
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x000280BC File Offset: 0x000262BC
	public void CreateJeep(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz, int _team)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoJeep, new Vector3(px, py, pz), new Vector3(rx + 90f, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_JEEP;
		cent.go.GetComponent<Car>().id = id;
		cent.go.GetComponent<Car>().uid = uid;
		cent.go.GetComponent<Car>().entid = cent.index;
		cent.go.GetComponent<Car>().dlina = 4f;
		cent.go.GetComponent<Car>().shirina = 3f;
		cent.go.GetComponent<Car>().classID = CONST.ENTS.ENT_JEEP;
		cent.go.GetComponent<Car>().tank_type = 3;
		cent.go.GetComponent<Car>().team = _team;
		cent.go.GetComponentInChildren<ParticleSystem>().Stop();
		cent.go.name = "TANK_" + cent.index.ToString();
		cent.go.GetComponent<Car>().MGFlash = GameObject.Find(cent.go.name + "/root/turret/flash").transform;
		cent.go.GetComponent<Car>().MGFlash.gameObject.SetActive(false);
		int num = Mathf.RoundToInt(fx);
		int num2 = Mathf.RoundToInt(fy);
		int num3 = Mathf.RoundToInt(fz);
		if (num != CONST.VEHICLES.POSITION_NONE && cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE)
		{
			cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] = num;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject gameObject = cent.go.GetComponent<Car>().JeepMesh.gameObject;
			if (cent.ownerID == num)
			{
				gameObject.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			}
			else if (num2 == CONST.VEHICLES.POSITION_NONE && num3 == CONST.VEHICLES.POSITION_NONE)
			{
				gameObject.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, Mathf.RoundToInt(tz));
			}
			cent.go.transform.parent = RemotePlayersUpdater.Instance.BotsGmObj[id].transform;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			cent.go.transform.localPosition = new Vector3(0f, 0f, 0f);
			cent.go.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		if (num2 != CONST.VEHICLES.POSITION_NONE && cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE)
		{
			cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_GUNNER] = num2;
			GameObject gameObject2 = cent.go.GetComponent<Car>().JeepMesh.gameObject;
			if (cent.ownerID == num2)
			{
				gameObject2.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			}
			else if (num == CONST.VEHICLES.POSITION_NONE && num3 == CONST.VEHICLES.POSITION_NONE)
			{
				gameObject2.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, Mathf.RoundToInt(tz));
			}
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.parent = cent.go.GetComponent<Car>().turret;
			Transform[] componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.localPosition = new Vector3(0f, 0f, 0f);
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.localRotation = Quaternion.Euler(new Vector3(0f, -45f, 0f));
		}
		if (num3 != CONST.VEHICLES.POSITION_NONE && cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_DRIVER] == CONST.VEHICLES.POSITION_NONE)
		{
			cent.go.GetComponent<Car>().slots[CONST.VEHICLES.POSITION_JEEP_PASS] = num3;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/trooper").GetComponent<SkinnedMeshRenderer>().enabled = false;
			GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").layer = 28;
			Transform[] componentsInChildren = GameObject.Find(RemotePlayersUpdater.Instance.BotsGmObj[id].name + "/Bip001").GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 28;
			}
			GameObject gameObject3 = cent.go.GetComponent<Car>().JeepMesh.gameObject;
			if (cent.ownerID == num3)
			{
				gameObject3.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(Mathf.RoundToInt(ty), Mathf.RoundToInt(tz));
			}
			else if (num == CONST.VEHICLES.POSITION_NONE && num2 == CONST.VEHICLES.POSITION_NONE)
			{
				gameObject3.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, Mathf.RoundToInt(tz));
			}
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.parent = cent.go.GetComponent<Car>().turret;
			componentsInChildren = cent.go.GetComponentsInChildren<Transform>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].gameObject.layer = 0;
			}
			cent.go.layer = 0;
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.localPosition = new Vector3(0f, 0f, 0f);
			RemotePlayersUpdater.Instance.BotsGmObj[id].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		if (num != CONST.VEHICLES.POSITION_NONE && num2 != CONST.VEHICLES.POSITION_NONE && num3 != CONST.VEHICLES.POSITION_NONE && cent.ownerID != num && cent.ownerID != num2 && cent.ownerID != num3)
		{
			cent.go.GetComponent<Car>().JeepMesh.gameObject.GetComponent<SkinnedMeshRenderer>().materials[1].mainTexture = SkinManager.GetTankSkin(3, Mathf.RoundToInt(tz));
		}
		cent.go.layer = 20;
		cent.go.tag = "Tank";
		foreach (Collider collider in cent.go.GetComponentsInChildren<Collider>())
		{
			collider.tag = "Tank";
			collider.gameObject.layer = 20;
		}
	}

	// Token: 0x06000210 RID: 528 RVA: 0x0002887C File Offset: 0x00026A7C
	public void CreateEmptyTankRagDoll(GameObject original, byte epic)
	{
		Tank tank = null;
		if (original != null)
		{
			tank = original.GetComponent<Tank>();
		}
		if (epic != 0)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(ContentLoader.LoadGameObject("Dtank2"));
			gameObject.AddComponent<ragdoll_wreck>();
			gameObject.name = "deadbody_" + this.g_deadbody_index.ToString();
			gameObject.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
			gameObject.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
			if (tank != null)
			{
				tank.KillSelf();
			}
			else
			{
				Object.Destroy(original);
			}
			gameObject.GetComponentInChildren<Detonator>().enabled = true;
			return;
		}
		if (tank != null)
		{
			tank.KillSelf();
			return;
		}
		Object.Destroy(original);
	}

	// Token: 0x06000211 RID: 529 RVA: 0x000289A0 File Offset: 0x00026BA0
	public void CreateEmptyJeepRagDoll(GameObject original, byte epic)
	{
		Car car = null;
		if (original != null)
		{
			car = original.GetComponent<Car>();
		}
		if (epic != 0)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.pgoDJeep);
			gameObject.AddComponent<ragdoll_wreck>();
			gameObject.name = "deadbody_" + this.g_deadbody_index.ToString();
			gameObject.transform.position = new Vector3(original.transform.position.x, original.transform.position.y, original.transform.position.z);
			gameObject.transform.eulerAngles = new Vector3(original.transform.rotation.eulerAngles.x, original.transform.rotation.eulerAngles.y, original.transform.rotation.eulerAngles.z);
			if (car != null)
			{
				car.KillSelf();
			}
			else
			{
				Object.Destroy(original);
			}
			gameObject.GetComponentInChildren<Detonator>().enabled = true;
			return;
		}
		if (car != null)
		{
			car.KillSelf();
			return;
		}
		Object.Destroy(original);
	}

	// Token: 0x06000212 RID: 530 RVA: 0x00028AC0 File Offset: 0x00026CC0
	public void CreateRPG7(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoRPG7, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_RPG;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<RPG7>();
		cent.go.GetComponent<RPG7>().id = id;
		cent.go.GetComponent<RPG7>().uid = uid;
		cent.go.GetComponent<RPG7>().entid = cent.index;
		cent.go.name = "RPG7_" + uid.ToString();
	}

	// Token: 0x06000213 RID: 531 RVA: 0x00028BB8 File Offset: 0x00026DB8
	public void CreateSnaryad(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoSnaryad, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_TANK_SNARYAD;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<Snaryad>();
		cent.go.GetComponent<Snaryad>().id = id;
		cent.go.GetComponent<Snaryad>().uid = uid;
		cent.go.GetComponent<Snaryad>().entid = cent.index;
		cent.go.GetComponent<Snaryad>().classid = 14;
		cent.go.name = "SNARYAD_" + cent.index.ToString();
		Sound sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		AudioSource component = cent.go.GetComponent<AudioSource>();
		sound.PlaySound_SnaryadFly(component);
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] != null && (EntManager.Ent[i].classID == 13 || cent.classID == 16 || cent.classID == 17 || cent.classID == 18) && EntManager.Ent[i].go.GetComponent<Tank>().id == id && !EntManager.Ent[i].go.GetComponent<Tank>().client)
			{
				if (EntManager.Ent[i].go.GetComponent<AudioSource>() == null)
				{
					EntManager.Ent[i].go.AddComponent<AudioSource>();
				}
				component = EntManager.Ent[i].go.GetComponent<AudioSource>();
				component.maxDistance = 35f;
				component.spatialBlend = 1f;
				sound.PlaySound_WeaponTank(component);
			}
		}
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00028DCC File Offset: 0x00026FCC
	public void CreateZBK18M(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoAT, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_ZBK18M;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<Snaryad>();
		cent.go.GetComponent<Snaryad>().id = id;
		cent.go.GetComponent<Snaryad>().uid = uid;
		cent.go.GetComponent<Snaryad>().entid = cent.index;
		cent.go.GetComponent<Snaryad>().classid = 19;
		cent.go.name = "SNARYAD_" + cent.index.ToString();
		Sound sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		AudioSource component = cent.go.GetComponent<AudioSource>();
		sound.PlaySound_SnaryadFly(component);
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] != null && (EntManager.Ent[i].classID == 13 || EntManager.Ent[i].classID == 16 || EntManager.Ent[i].classID == 17 || EntManager.Ent[i].classID == 18) && EntManager.Ent[i].go.GetComponent<Tank>().id == id && !EntManager.Ent[i].go.GetComponent<Tank>().client)
			{
				if (EntManager.Ent[i].go.GetComponent<AudioSource>() == null)
				{
					EntManager.Ent[i].go.AddComponent<AudioSource>();
				}
				component = EntManager.Ent[i].go.GetComponent<AudioSource>();
				component.maxDistance = 35f;
				component.spatialBlend = 1f;
				sound.PlaySound_WeaponTank(component);
			}
		}
	}

	// Token: 0x06000215 RID: 533 RVA: 0x00028FF4 File Offset: 0x000271F4
	public void CreateZOF26(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoFUGAS, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_ZOF26;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<Snaryad>();
		cent.go.GetComponent<Snaryad>().id = id;
		cent.go.GetComponent<Snaryad>().uid = uid;
		cent.go.GetComponent<Snaryad>().entid = cent.index;
		cent.go.GetComponent<Snaryad>().classid = 20;
		cent.go.name = "SNARYAD_" + cent.index.ToString();
		Sound sound = (Sound)Object.FindObjectOfType(typeof(Sound));
		AudioSource component = cent.go.GetComponent<AudioSource>();
		sound.PlaySound_SnaryadFly(component);
		for (int i = 0; i < 512; i++)
		{
			if (EntManager.Ent[i] != null && (EntManager.Ent[i].classID == 13 || EntManager.Ent[i].classID == 16 || EntManager.Ent[i].classID == 17 || EntManager.Ent[i].classID == 18) && EntManager.Ent[i].go.GetComponent<Tank>().id == id && !EntManager.Ent[i].go.GetComponent<Tank>().client)
			{
				if (EntManager.Ent[i].go.GetComponent<AudioSource>() == null)
				{
					EntManager.Ent[i].go.AddComponent<AudioSource>();
				}
				component = EntManager.Ent[i].go.GetComponent<AudioSource>();
				component.maxDistance = 35f;
				component.spatialBlend = 1f;
				sound.PlaySound_WeaponTank(component);
			}
		}
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0002921C File Offset: 0x0002741C
	public void CreateMinefly(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoMinefly, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_MINEFLY;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponent<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<Minefly>();
		cent.go.GetComponent<Minefly>().id = id;
		cent.go.GetComponent<Minefly>().uid = uid;
		cent.go.GetComponent<Minefly>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x06000217 RID: 535 RVA: 0x00029314 File Offset: 0x00027514
	public void CreateMine(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoMine, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_MINE;
		cent.go.AddComponent<Mine>();
		cent.go.GetComponent<Mine>().id = id;
		cent.go.GetComponent<Mine>().uid = uid;
		cent.go.GetComponent<Mine>().entid = cent.index;
		cent.go.name = "MINE_" + uid.ToString();
	}

	// Token: 0x06000218 RID: 536 RVA: 0x000293BC File Offset: 0x000275BC
	public void CreateATMine(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoATMine, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_AT_MINE;
		cent.go.AddComponent<Mine>();
		cent.go.GetComponent<Mine>().id = id;
		cent.go.GetComponent<Mine>().uid = uid;
		cent.go.GetComponent<Mine>().entid = cent.index;
		cent.go.name = "MINE_" + uid.ToString();
	}

	// Token: 0x06000219 RID: 537 RVA: 0x00029464 File Offset: 0x00027664
	public void CreateC4(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoC4, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_C4;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<Rigidbody>().AddForce(new Vector3(fx, fy, fz));
		cent.go.GetComponent<Rigidbody>().AddTorque(new Vector3(tx, ty, tz));
		cent.go.AddComponent<C4>();
		cent.go.GetComponent<C4>().id = id;
		cent.go.GetComponent<C4>().uid = uid;
		cent.go.GetComponent<C4>().entid = cent.index;
		cent.go.name = "C4_" + uid.ToString();
	}

	// Token: 0x0600021A RID: 538 RVA: 0x0002955C File Offset: 0x0002775C
	public void CreateSMOKE(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoSmoke, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_SMOKE;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<VehicleSmoke>().id = id;
		cent.go.GetComponent<VehicleSmoke>().uid = uid;
		cent.go.GetComponent<VehicleSmoke>().entid = cent.index;
		cent.go.name = "SMOKE_" + uid.ToString();
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00029614 File Offset: 0x00027814
	public void CreateFLASH(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoAntiMissle, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_ANTI_MISSLE;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<Rigidbody>().AddForce(new Vector3(fx, 1500f, fz));
		cent.go.AddComponent<VehicleFlash>();
		cent.go.GetComponent<VehicleFlash>().id = id;
		cent.go.GetComponent<VehicleFlash>().uid = uid;
		cent.go.GetComponent<VehicleFlash>().entid = cent.index;
		cent.go.name = "FLASH_" + uid.ToString();
		for (int i = 0; i < CONST.ENTS.MAX_ENTS; i++)
		{
			if (EntManager.Ent[i] != null && !(EntManager.Ent[i].go == null) && !(EntManager.Ent[i].go.GetComponent<JavelinMissle>() == null) && EntManager.Ent[i].go.GetComponent<JavelinMissle>().targetPlayerID == id)
			{
				EntManager.Ent[i].go.GetComponent<JavelinMissle>().timeTarget = cent.go.transform;
			}
		}
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00029774 File Offset: 0x00027974
	public void CreateCrossbowArrow(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoArrow, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_ARROW;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.GetComponent<Arrow>().id = id;
		cent.go.GetComponent<Arrow>().uid = uid;
		cent.go.GetComponent<Arrow>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
	}

	// Token: 0x0600021D RID: 541 RVA: 0x0002982C File Offset: 0x00027A2C
	public void CreateJavelinMissle(int id, int uid, float px, float py, float pz, float rx, float ry, float rz, float fx, float fy, float fz, float tx, float ty, float tz)
	{
		CEnt cent = EntManager.CreateEnt(this.pgoJavelinMissle, new Vector3(px, py, pz), new Vector3(rx, ry, rz));
		cent.uid = uid;
		cent.ownerID = id;
		cent.classID = CONST.ENTS.ENT_JAVELIN;
		cent.go.transform.eulerAngles = new Vector3(rx, ry, rz);
		cent.go.AddComponent<JavelinMissle>();
		cent.go.GetComponent<JavelinMissle>().id = id;
		cent.go.GetComponent<JavelinMissle>().uid = uid;
		cent.go.GetComponent<JavelinMissle>().entid = cent.index;
		cent.go.name = "ent_" + uid.ToString();
		CEnt ent = EntManager.GetEnt(Mathf.RoundToInt(tz));
		if (ent != null)
		{
			cent.go.GetComponent<JavelinMissle>().target = ent.go.transform;
			cent.go.GetComponent<JavelinMissle>().targetPlayerID = ent.ownerID;
			cent.go.GetComponent<JavelinMissle>().targetEntClassID = ent.classID;
		}
		cent.go.layer = 8;
		Collider[] componentsInChildren = cent.go.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = 8;
		}
		Client client = null;
		if (client == null)
		{
			client = (Client)Object.FindObjectOfType(typeof(Client));
		}
		if (ent.ownerID == client.myindex)
		{
			if (ent.classID != CONST.ENTS.ENT_JEEP)
			{
				TankController tankController = (TankController)Object.FindObjectOfType(typeof(TankController));
				if (tankController.enabled)
				{
					tankController.missle = cent.go.transform;
					tankController.javelinAIM(2);
					return;
				}
			}
			else
			{
				CarController carController = (CarController)Object.FindObjectOfType(typeof(CarController));
				if (carController.enabled)
				{
					carController.missle = cent.go.transform;
					carController.javelinAIM(2);
				}
			}
		}
	}

	// Token: 0x04000206 RID: 518
	public GameObject pgoPlayerRagDoll;

	// Token: 0x04000207 RID: 519
	public GameObject pgoHelmetRagDoll;

	// Token: 0x04000208 RID: 520
	public GameObject pgoCapRagDoll;

	// Token: 0x04000209 RID: 521
	public GameObject pgoTykvaRagDoll;

	// Token: 0x0400020A RID: 522
	public GameObject[] pgoWeapons;

	// Token: 0x0400020B RID: 523
	public GameObject pgoArrow;

	// Token: 0x0400020C RID: 524
	public GameObject pgoAntiMissle;

	// Token: 0x0400020D RID: 525
	public GameObject pgoSmoke;

	// Token: 0x0400020E RID: 526
	public GameObject pgoATMine;

	// Token: 0x0400020F RID: 527
	public GameObject pgoMOLOTOV;

	// Token: 0x04000210 RID: 528
	public GameObject pgoM202;

	// Token: 0x04000211 RID: 529
	public GameObject pgoGG;

	// Token: 0x04000212 RID: 530
	public GameObject pgoMine;

	// Token: 0x04000213 RID: 531
	public GameObject pgoC4;

	// Token: 0x04000214 RID: 532
	public GameObject pgoM18;

	// Token: 0x04000215 RID: 533
	public GameObject pgoM18Pink;

	// Token: 0x04000216 RID: 534
	public GameObject pgoMK3;

	// Token: 0x04000217 RID: 535
	public GameObject pgoRKG3;

	// Token: 0x04000218 RID: 536
	public GameObject pgoGrenade;

	// Token: 0x04000219 RID: 537
	public GameObject pgoRocket;

	// Token: 0x0400021A RID: 538
	public GameObject pgoZombie;

	// Token: 0x0400021B RID: 539
	public GameObject pgoGP;

	// Token: 0x0400021C RID: 540
	public GameObject pgoMinen;

	// Token: 0x0400021D RID: 541
	public GameObject pgoSnaryad;

	// Token: 0x0400021E RID: 542
	public GameObject pgoRPG7;

	// Token: 0x0400021F RID: 543
	public GameObject pgoAT;

	// Token: 0x04000220 RID: 544
	public GameObject pgoFUGAS;

	// Token: 0x04000221 RID: 545
	public GameObject pgoMinefly;

	// Token: 0x04000222 RID: 546
	public GameObject pgoJavelinMissle;

	// Token: 0x04000223 RID: 547
	public GameObject pgoSTIELHANDGRANATE;

	// Token: 0x04000224 RID: 548
	public GameObject pgoJeep;

	// Token: 0x04000225 RID: 549
	public GameObject pgoDJeep;

	// Token: 0x04000226 RID: 550
	private int g_deadbody_index;

	// Token: 0x04000227 RID: 551
	private int g_deadhelmet_index;
}
