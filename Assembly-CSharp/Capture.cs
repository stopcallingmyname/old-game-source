using System;
using UnityEngine;

// Token: 0x02000031 RID: 49
public class Capture : MonoBehaviour
{
	// Token: 0x06000196 RID: 406 RVA: 0x00023008 File Offset: 0x00021208
	public static void Create()
	{
		int width = Screen.width;
		int height = Screen.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0);
		texture2D.Apply();
		Object.Destroy(texture2D);
	}
}
