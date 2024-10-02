using System;
using UnityEngine;

// Token: 0x0200008E RID: 142
public class GUI3
{
	// Token: 0x06000420 RID: 1056 RVA: 0x00051408 File Offset: 0x0004F608
	public static void Init()
	{
		GUI3.bar[0] = (Resources.Load("NewMenu/E_Menu_Slider_Top") as Texture2D);
		GUI3.bar[1] = (Resources.Load("NewMenu/E_Menu_Slider_Middle") as Texture2D);
		GUI3.bar[2] = (Resources.Load("NewMenu/E_Menu_Slider_Bottom") as Texture2D);
		GUI3.blackTex = GUI3.GetTexture2D(Color.black, 100f);
		GUI3.whiteTex = GUI3.GetTexture2D(Color.white, 100f);
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x00051480 File Offset: 0x0004F680
	public static void HSlider()
	{
		GUI.skin.horizontalSliderThumb.normal.background = GUI3.bar[3];
		GUI.skin.horizontalSliderThumb.hover.background = GUI3.bar[4];
		GUI.skin.horizontalSliderThumb.active.background = GUI3.bar[4];
		GUI.skin.horizontalSlider.normal.background = GUI3.bar[5];
		GUI.skin.horizontalSlider.fixedHeight = 12f;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x00051510 File Offset: 0x0004F710
	public static void Toggle()
	{
		GUI.skin.toggle.normal.background = GUI3.bar[6];
		GUI.skin.toggle.onNormal.background = GUI3.bar[7];
		GUI.skin.toggle.hover.background = GUI3.bar[6];
		GUI.skin.toggle.onHover.background = GUI3.bar[7];
		GUI.skin.toggle.active.background = GUI3.bar[7];
		GUI.skin.toggle.onActive.background = GUI3.bar[7];
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x000515C0 File Offset: 0x0004F7C0
	public static Vector2 BeginScrollView(Rect viewzone, Vector2 scrollViewVector, Rect scrollzone, bool debug = false)
	{
		GUI.skin.verticalScrollbar.normal.background = null;
		GUI.skin.verticalScrollbarThumb.normal.background = null;
		GUI.skin.horizontalScrollbar.normal.background = null;
		GUI.skin.horizontalScrollbarThumb.normal.background = null;
		scrollViewVector = GUI.BeginScrollView(viewzone, scrollViewVector, scrollzone);
		float num = viewzone.height / scrollzone.height * viewzone.height;
		float num2 = scrollViewVector.y / scrollzone.height * viewzone.height;
		if (debug)
		{
			Debug.Log(string.Concat(new object[]
			{
				num,
				" ",
				viewzone.height,
				" ",
				scrollzone.height,
				" y=",
				scrollViewVector.y,
				" "
			}));
		}
		if (num - 28f < 0f)
		{
			num = 28f;
		}
		if (scrollzone.height <= viewzone.height)
		{
			num = 0f;
		}
		GUI3.rbar = new Rect(viewzone.x + viewzone.width - 15f, viewzone.y + num2, 14f, num);
		return scrollViewVector;
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0005171B File Offset: 0x0004F91B
	public static void EndScrollView()
	{
		GUI.EndScrollView();
		GUI3.DrawBar(GUI3.rbar);
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x0005172C File Offset: 0x0004F92C
	public static void DrawBar(Rect r)
	{
		if (r.height == 0f)
		{
			return;
		}
		GUI.DrawTexture(new Rect(r.x, r.y, r.width, 14f), GUI3.bar[0]);
		if (r.height - 28f > 0f)
		{
			GUI.DrawTexture(new Rect(r.x, r.y + 14f, r.width, r.height - 28f), GUI3.bar[1]);
		}
		GUI.DrawTexture(new Rect(r.x, r.y + r.height - 14f, r.width, 14f), GUI3.bar[2]);
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x000517FC File Offset: 0x0004F9FC
	public static Texture2D GetTexture2D(Color _color, float _alpha)
	{
		Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		texture2D.SetPixel(0, 0, new Color(_color.r, _color.g, _color.b, _alpha / 100f));
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x040008D6 RID: 2262
	private static Texture2D[] bar = new Texture2D[10];

	// Token: 0x040008D7 RID: 2263
	public static Texture2D blackTex;

	// Token: 0x040008D8 RID: 2264
	public static Texture2D whiteTex;

	// Token: 0x040008D9 RID: 2265
	private static Rect rbar;
}
