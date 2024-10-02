using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000039 RID: 57
public class ContentLoader : MonoBehaviour
{
	// Token: 0x060001D0 RID: 464 RVA: 0x00023D44 File Offset: 0x00021F44
	private void Update()
	{
		if (GameController.STATE != GAME_STATES.LOADING_BUNDLES)
		{
			return;
		}
		if (ContentLoader.downloading)
		{
			return;
		}
		if (!this.get_manifest)
		{
			this.get_manifest = true;
			base.StartCoroutine(this.GetAssetBundlesManifest());
		}
		if (ContentLoader.bundles_count == 0)
		{
			return;
		}
		if (ContentLoader.bundles_list.Count == 0)
		{
			GameController.STATE = GAME_STATES.INIT;
			return;
		}
		if (string.IsNullOrEmpty(ContentLoader.bundles_list[0]))
		{
			ContentLoader.bundles_list.RemoveAt(0);
			return;
		}
		ContentLoader.downloading = true;
		if (ContentLoader.bundles_list.Contains("soundtrack"))
		{
			base.StartCoroutine(this.DownloadAssetBundle(ContentLoader.bundles_list.IndexOf("soundtrack")));
			return;
		}
		if (ContentLoader.bundles_list.Contains("maintheme"))
		{
			base.StartCoroutine(this.DownloadAssetBundle(ContentLoader.bundles_list.IndexOf("maintheme")));
			return;
		}
		if (ContentLoader.bundles_list.Contains("fonts"))
		{
			base.StartCoroutine(this.DownloadAssetBundle(ContentLoader.bundles_list.IndexOf("fonts")));
			return;
		}
		if (ContentLoader.bundles_list.Contains("ui"))
		{
			base.StartCoroutine(this.DownloadAssetBundle(ContentLoader.bundles_list.IndexOf("ui")));
			return;
		}
		base.StartCoroutine(this.DownloadAssetBundle(0));
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00023E84 File Offset: 0x00022084
	private IEnumerator GetAssetBundlesManifest()
	{
		ContentLoader.www = new WWW(CONST.GET_CONTENT_URL() + "Windows");
		yield return ContentLoader.www;
		if (!string.IsNullOrEmpty(ContentLoader.www.error))
		{
			Debug.Log(ContentLoader.www.error);
			this.get_manifest = false;
			yield return new WaitForSeconds(5f);
			yield break;
		}
		this.ABM = (ContentLoader.www.assetBundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest);
		yield return null;
		ContentLoader.www.assetBundle.Unload(false);
		ContentLoader.bundles_list = this.ABM.GetAllAssetBundles().ToList<string>();
		ContentLoader.bundles_count = ContentLoader.bundles_list.Count;
		yield break;
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00023E93 File Offset: 0x00022093
	private IEnumerator DownloadAssetBundle(int index = 0)
	{
		ContentLoader.currentBundle = ContentLoader.bundles_list[index];
		string str = "";
		ContentLoader.www = WWW.LoadFromCacheOrDownload(CONST.GET_CONTENT_URL() + ContentLoader.bundles_list[index] + str, this.ABM.GetAssetBundleHash(ContentLoader.bundles_list[index]));
		yield return ContentLoader.www;
		if (ContentLoader.www == null)
		{
			ContentLoader.bundles_list.RemoveAt(index);
			ContentLoader.downloading = false;
			yield break;
		}
		if (ContentLoader.www.error != null)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"error load file: ",
				ContentLoader.bundles_list[0],
				" error (",
				ContentLoader.www.error,
				")"
			}));
			ContentLoader.bundles_list.RemoveAt(index);
			ContentLoader.downloading = false;
			yield break;
		}
		if (ContentLoader.www.assetBundle == null)
		{
			Debug.LogError("Bundle is null: " + ContentLoader.bundles_list[index]);
			ContentLoader.bundles_list.RemoveAt(index);
			ContentLoader.downloading = false;
			yield break;
		}
		this.ProcessAssetBundle(ContentLoader.www.assetBundle);
		if (ContentLoader.bundles_list[index] == "fonts")
		{
			FontManager.Init();
		}
		else if (ContentLoader.bundles_list[index] == "ui")
		{
			GUIManager.Init(false);
		}
		else if (ContentLoader.bundles_list[index] == "maintheme")
		{
			BackGround.Init();
		}
		ContentLoader.www.Dispose();
		ContentLoader.www = null;
		ContentLoader.bundles_list.RemoveAt(index);
		ContentLoader.downloading = false;
		yield break;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00023EAC File Offset: 0x000220AC
	private void ProcessAssetBundle(AssetBundle bundle)
	{
		AudioClip[] array = bundle.LoadAllAssets<AudioClip>();
		Sprite[] array2 = bundle.LoadAllAssets<Sprite>();
		Texture2D[] array3 = bundle.LoadAllAssets<Texture2D>();
		TextAsset[] array4 = bundle.LoadAllAssets<TextAsset>();
		Material[] array5 = bundle.LoadAllAssets<Material>();
		BlockSet[] array6 = bundle.LoadAllAssets<BlockSet>();
		Font[] array7 = bundle.LoadAllAssets<Font>();
		if (array7.Length != 0)
		{
			foreach (Font font in array7)
			{
				if (!ContentLoader.FontList.ContainsKey(font.name))
				{
					ContentLoader.FontList.Add(font.name, font);
				}
			}
		}
		if (array6.Length != 0)
		{
			foreach (BlockSet blockSet in array6)
			{
				if (!ContentLoader.BlockSetList.ContainsKey(blockSet.name))
				{
					ContentLoader.BlockSetList.Add(blockSet.name, blockSet);
				}
			}
		}
		if (array.Length != 0)
		{
			foreach (AudioClip audioClip in array)
			{
				if (!ContentLoader.SoundList.ContainsKey(audioClip.name))
				{
					ContentLoader.SoundList.Add(audioClip.name, audioClip);
				}
			}
		}
		if (array3.Length != 0)
		{
			foreach (Texture2D texture2D in array3)
			{
				if (!ContentLoader.TextureList.ContainsKey(texture2D.name))
				{
					ContentLoader.TextureList.Add(texture2D.name, texture2D);
				}
			}
		}
		if (array2.Length != 0)
		{
			foreach (Sprite sprite in array2)
			{
				if (!ContentLoader.SpriteList.ContainsKey(sprite.name))
				{
					ContentLoader.SpriteList.Add(sprite.name, sprite);
				}
			}
		}
		if (array4.Length != 0)
		{
			foreach (TextAsset textAsset in array4)
			{
				if (!ContentLoader.TextAssetList.ContainsKey(textAsset.name))
				{
					ContentLoader.TextAssetList.Add(textAsset.name, textAsset);
				}
			}
		}
		if (array5.Length != 0)
		{
			foreach (Material material in array5)
			{
				if (!ContentLoader.MaterialList.ContainsKey(material.name))
				{
					ContentLoader.MaterialList.Add(material.name, material);
				}
			}
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x000240E4 File Offset: 0x000222E4
	public static GameObject LoadGameObject(string _name)
	{
		if (ContentLoader.GameObjectList.ContainsKey(_name))
		{
			return ContentLoader.GameObjectList[_name];
		}
		return null;
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00024100 File Offset: 0x00022300
	public static BlockSet LoadBlockSet(string _name)
	{
		return Resources.Load<BlockSet>(_name);
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00024108 File Offset: 0x00022308
	public static AudioClip LoadSound(string _name)
	{
		if (ContentLoader.SoundList.ContainsKey(_name))
		{
			return ContentLoader.SoundList[_name];
		}
		return null;
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00024124 File Offset: 0x00022324
	public static Font LoadFont(string _name)
	{
		if (ContentLoader.FontList.ContainsKey(_name))
		{
			return ContentLoader.FontList[_name];
		}
		return null;
	}

	// Token: 0x060001D8 RID: 472 RVA: 0x00024140 File Offset: 0x00022340
	public static AnimationClip LoadAnimation(string _name)
	{
		if (ContentLoader.AnimationList.ContainsKey(_name))
		{
			return ContentLoader.AnimationList[_name];
		}
		return null;
	}

	// Token: 0x060001D9 RID: 473 RVA: 0x0002415C File Offset: 0x0002235C
	public static Texture2D LoadTexture(string _name)
	{
		if (ContentLoader.TextureList.ContainsKey(_name))
		{
			return ContentLoader.TextureList[_name];
		}
		return null;
	}

	// Token: 0x060001DA RID: 474 RVA: 0x00024178 File Offset: 0x00022378
	public static Sprite LoadSprite(string _name)
	{
		if (ContentLoader.SpriteList.ContainsKey(_name))
		{
			return ContentLoader.SpriteList[_name];
		}
		return null;
	}

	// Token: 0x060001DB RID: 475 RVA: 0x00024194 File Offset: 0x00022394
	public static TextAsset LoadTextAsset(string _name)
	{
		if (ContentLoader.TextAssetList.ContainsKey(_name))
		{
			return ContentLoader.TextAssetList[_name];
		}
		return null;
	}

	// Token: 0x060001DC RID: 476 RVA: 0x000241B0 File Offset: 0x000223B0
	public static Material LoadMaterial(string _name)
	{
		if (ContentLoader.MaterialList.ContainsKey(_name))
		{
			return ContentLoader.MaterialList[_name];
		}
		return null;
	}

	// Token: 0x060001DD RID: 477 RVA: 0x000241CC File Offset: 0x000223CC
	private void OnGUI()
	{
		if (GameController.STATE != GAME_STATES.LOADING_BUNDLES || !GUIManager.IsReady)
		{
			return;
		}
		GUI.depth = -2;
		GUI.DrawTexture(new Rect((float)Screen.width / 2f - 171f, (float)(Screen.height / 2 - 95), 342f, 68f), GUIManager.tex_loading);
		int num = 2;
		int num2 = 2;
		if (ContentLoader.www != null)
		{
			if (ContentLoader.bundles_count > 0)
			{
				float num3 = ContentLoader.www.progress;
				GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 171), (float)(Screen.height / 2 + 15), 342f, 20f), GUIManager.tex_black);
				int num4 = (int)(num3 * 100f);
				int fontSize = GUIManager.gs_style1.fontSize;
				TextAnchor alignment = GUIManager.gs_style1.alignment;
				GUIManager.gs_style1.fontSize = 16;
				GUIManager.gs_style1.alignment = TextAnchor.UpperCenter;
				GUI.Label(new Rect((float)(Screen.width / 2 - 171), (float)(Screen.height / 2 - 30), 342f, 20f), string.Concat(new string[]
				{
					Lang.GetLabel(209),
					(ContentLoader.bundles_count - ContentLoader.bundles_list.Count - 1).ToString(),
					Lang.GetLabel(210),
					ContentLoader.bundles_count.ToString(),
					Lang.GetLabel(211)
				}), GUIManager.gs_style1);
				GUI.Label(new Rect((float)(Screen.width / 2 - 171), (float)(Screen.height / 2 + 40), 342f, 20f), ContentLoader.GetResName(ContentLoader.currentBundle) + num4.ToString() + "%", GUIManager.gs_style1);
				GUIManager.gs_style1.fontSize = fontSize;
				GUIManager.gs_style1.alignment = alignment;
				num4 /= 10;
				for (int i = 0; i < num4; i++)
				{
					GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 171 + num), (float)(Screen.height / 2 + 17), 32f, 16f), GUIManager.tex_bar_yellow);
					num += 34;
				}
			}
			float num5 = 1f;
			if (ContentLoader.bundles_list.Count > 0)
			{
				num5 = (float)(ContentLoader.bundles_count - ContentLoader.bundles_list.Count) / (float)ContentLoader.bundles_count;
			}
			num5 *= 100f;
			int num6 = (int)num5;
			num6 /= 10;
			GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 171), (float)(Screen.height / 2 - 10), 342f, 20f), GUIManager.tex_black);
			for (int j = 0; j < num6; j++)
			{
				GUI.DrawTexture(new Rect((float)(Screen.width / 2 - 171 + num2), (float)(Screen.height / 2 - 8), 32f, 16f), GUIManager.tex_bar_blue);
				num2 += 34;
			}
		}
	}

	// Token: 0x060001DE RID: 478 RVA: 0x000244B0 File Offset: 0x000226B0
	public static string GetResName(string name)
	{
		string result = "";
		if (name == "" || name == null)
		{
			result = Lang.GetLabel(214) + ": ";
		}
		else if (name.Contains("maintheme"))
		{
			result = Lang.GetLabel(215) + ": ";
		}
		else if (name.Contains("sound"))
		{
			result = Lang.GetLabel(218) + ": ";
		}
		else if (name.Contains("skin"))
		{
			result = Lang.GetLabel(216) + ": ";
		}
		else if (name.Contains("TanksSkins"))
		{
			result = Lang.GetLabel(216) + ": ";
		}
		else if (name.Contains("zoom"))
		{
			result = Lang.GetLabel(217) + ": ";
		}
		else if (name.Contains("Vehicles"))
		{
			result = Lang.GetLabel(219) + ": ";
		}
		else if (name.Contains("tank"))
		{
			result = Lang.GetLabel(219) + ": ";
		}
		else if (name.Contains("WeaponsXML"))
		{
			result = Lang.GetLabel(219) + ": ";
		}
		else if (name.Contains("SkinsXML"))
		{
			result = Lang.GetLabel(219) + ": ";
		}
		else if (name.Contains("VehiclesXML"))
		{
			result = Lang.GetLabel(219) + ": ";
		}
		return result;
	}

	// Token: 0x0400017B RID: 379
	public static WWW www = null;

	// Token: 0x0400017C RID: 380
	public static List<string> bundles_list;

	// Token: 0x0400017D RID: 381
	public static Dictionary<string, Texture2D> TextureList = new Dictionary<string, Texture2D>();

	// Token: 0x0400017E RID: 382
	public static Dictionary<string, AudioClip> SoundList = new Dictionary<string, AudioClip>();

	// Token: 0x0400017F RID: 383
	public static Dictionary<string, Sprite> SpriteList = new Dictionary<string, Sprite>();

	// Token: 0x04000180 RID: 384
	public static Dictionary<string, TextAsset> TextAssetList = new Dictionary<string, TextAsset>();

	// Token: 0x04000181 RID: 385
	public static Dictionary<string, GameObject> GameObjectList = new Dictionary<string, GameObject>();

	// Token: 0x04000182 RID: 386
	public static Dictionary<string, AnimationClip> AnimationList = new Dictionary<string, AnimationClip>();

	// Token: 0x04000183 RID: 387
	public static Dictionary<string, Material> MaterialList = new Dictionary<string, Material>();

	// Token: 0x04000184 RID: 388
	public static Dictionary<string, BlockSet> BlockSetList = new Dictionary<string, BlockSet>();

	// Token: 0x04000185 RID: 389
	public static Dictionary<string, Font> FontList = new Dictionary<string, Font>();

	// Token: 0x04000186 RID: 390
	private static List<byte> tmp;

	// Token: 0x04000187 RID: 391
	private AssetBundleManifest ABM;

	// Token: 0x04000188 RID: 392
	public static bool downloading = false;

	// Token: 0x04000189 RID: 393
	private bool get_manifest;

	// Token: 0x0400018A RID: 394
	private static string currentBundle = string.Empty;

	// Token: 0x0400018B RID: 395
	public static int bundles_count = 0;

	// Token: 0x0400018C RID: 396
	private int total_progress;

	// Token: 0x0400018D RID: 397
	public static float progress;

	// Token: 0x0400018E RID: 398
	private int current_progress;

	// Token: 0x0400018F RID: 399
	private int download_length;
}
