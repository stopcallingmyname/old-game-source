using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class BackGround : MonoBehaviour
{
	// Token: 0x06000041 RID: 65 RVA: 0x00002EF0 File Offset: 0x000010F0
	private void OnGUI()
	{
		if (BackGround.background[0] == null || GameController.STATE == GAME_STATES.BANNED || GameController.STATE == GAME_STATES.CLIENTINITERROR || GameController.STATE == GAME_STATES.AUTH_ERROR)
		{
			return;
		}
		Vector2 vector = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		Vector2 vector2 = new Vector2((vector.x - (float)Screen.width / 2f) / 200f, (vector.y - (float)Screen.height / 2f) / 200f);
		float num = (float)BackGround.background[2].width / (float)BackGround.background[2].height;
		Rect position = new Rect((float)Screen.width / 2f - (float)Screen.height * 1.1f, (float)Screen.height / 2f - (float)Screen.height * 1.1f / 2f, (float)Screen.height * 1.1f * 2f, (float)Screen.height * 1.1f);
		Rect position2 = new Rect(position.x - vector2.x, position.y - vector2.y, (float)Screen.height * 1.2f * 2f, (float)Screen.height * 1.2f);
		position.center = new Vector2((float)Screen.width, (float)Screen.height) / 2f;
		Rect texCoords = new Rect(this.x1, 0f, 1f, 1f);
		Rect texCoords2 = new Rect(this.x2, 0f, 1f, 1f);
		GUI.DrawTexture(position, BackGround.background[0]);
		GUI.DrawTextureWithTexCoords(position, BackGround.background[1], texCoords);
		GUI.DrawTexture(position2, BackGround.background[2]);
		GUI.DrawTextureWithTexCoords(position, BackGround.background[3], texCoords2);
		this.x1 += 0.02f * Time.deltaTime;
		this.x2 -= 0.015f * Time.deltaTime;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000030FC File Offset: 0x000012FC
	public static void Init()
	{
		BackGround.background[0] = ContentLoader.LoadTexture("layer_wall");
		BackGround.background[1] = ContentLoader.LoadTexture("layer_back");
		BackGround.background[2] = ContentLoader.LoadTexture("layer_player");
		BackGround.background[3] = ContentLoader.LoadTexture("layer_front");
	}

	// Token: 0x04000027 RID: 39
	public static Texture2D[] background = new Texture2D[4];

	// Token: 0x04000028 RID: 40
	private Rect r;

	// Token: 0x04000029 RID: 41
	private float x1;

	// Token: 0x0400002A RID: 42
	private float x2;

	// Token: 0x0400002B RID: 43
	private float y1;

	// Token: 0x0400002C RID: 44
	private float y2;
}
