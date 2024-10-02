using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class Console : MonoBehaviour
{
	// Token: 0x06000128 RID: 296 RVA: 0x00016B53 File Offset: 0x00014D53
	private void OnEnable()
	{
		Application.RegisterLogCallback(new Application.LogCallback(this.HandleLog));
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00016B66 File Offset: 0x00014D66
	private void OnDisable()
	{
		Application.RegisterLogCallback(null);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00016B6E File Offset: 0x00014D6E
	private void Update()
	{
		if (Input.GetKeyDown(this.toggleKey))
		{
			this.show = !this.show;
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00016B8C File Offset: 0x00014D8C
	private void OnGUI()
	{
		if (!this.show)
		{
			return;
		}
		GUI.depth = -99;
		this.windowRect = new Rect(0f, 0f, (float)Screen.width, (float)(Screen.height / 2));
		GUI.DrawTexture(this.windowRect, this._t);
		GUI.skin.horizontalScrollbar.normal.background = null;
		GUI.skin.horizontalScrollbarThumb.normal.background = null;
		this.scrollPosition = GUI.BeginScrollView(new Rect(this.windowRect.x + 5f, this.windowRect.y + 5f, this.windowRect.width - 10f, this.windowRect.height - 25f - 10f), this.scrollPosition, new Rect(0f, 0f, (float)(Screen.width - 15), (float)(this.logs.Count * 25)));
		int num = 0;
		for (int i = 0; i < this.logs.Count; i++)
		{
			Console.Log log = this.logs[i];
			if (!this.collapse || i <= 0 || !(log.message == this.logs[i - 1].message))
			{
				this.gui_style.normal.textColor = Console.logTypeColors[log.type];
				GUIContent content = new GUIContent(log.message);
				float num2 = this.gui_style.CalcHeight(content, (float)(Screen.width - 15));
				GUI.TextArea(new Rect(0f, (float)num, (float)(Screen.width - 15), num2), log.message, this.gui_style);
				num += (int)num2 + 10;
			}
		}
		GUI.EndScrollView();
		GUI.contentColor = Color.white;
		Rect position = new Rect(this.windowRect.x + 5f, this.windowRect.yMax - 25f, this.windowRect.width - 10f - 100f, 20f);
		Rect position2 = new Rect(this.windowRect.xMax - 100f, this.windowRect.yMax - 25f, 100f, 20f);
		Vector2 point = new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y);
		if (position.Contains(point))
		{
			GUI.DrawTexture(position, this._t_h);
		}
		else
		{
			GUI.DrawTexture(position, this._t);
		}
		TextAnchor alignment = this.gui_style.alignment;
		this.gui_style.alignment = TextAnchor.MiddleCenter;
		if (GUI.Button(position, this.clearLabel, this.gui_style))
		{
			this.logs.Clear();
		}
		this.gui_style.alignment = alignment;
		this.collapse = GUI.Toggle(position2, this.collapse, this.collapseLabel);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00016E98 File Offset: 0x00015098
	private void HandleLog(string message, string stackTrace, LogType type)
	{
		this.logs.Add(new Console.Log
		{
			message = message,
			stackTrace = stackTrace,
			type = type
		});
	}

	// Token: 0x0600012D RID: 301 RVA: 0x00016ED4 File Offset: 0x000150D4
	private void Awake()
	{
		this.gui_style = new GUIStyle();
		this.gui_style.font = FontManager.font[0];
		this.gui_style.fontSize = 16;
		this.gui_style.normal.textColor = new Color(255f, 255f, 255f, 255f);
		this.gui_style.alignment = TextAnchor.MiddleLeft;
		this.gui_style.margin.bottom = 15;
		this._t = GUI3.GetTexture2D(Color.black, 70f);
		this._t_h = GUI3.GetTexture2D(Color.white, 70f);
	}

	// Token: 0x0400013E RID: 318
	public KeyCode toggleKey = KeyCode.BackQuote;

	// Token: 0x0400013F RID: 319
	private List<Console.Log> logs = new List<Console.Log>();

	// Token: 0x04000140 RID: 320
	private Vector2 scrollPosition;

	// Token: 0x04000141 RID: 321
	private bool show;

	// Token: 0x04000142 RID: 322
	private bool collapse;

	// Token: 0x04000143 RID: 323
	private GUIStyle gui_style;

	// Token: 0x04000144 RID: 324
	private Texture2D _t;

	// Token: 0x04000145 RID: 325
	private Texture2D _t_h;

	// Token: 0x04000146 RID: 326
	private static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>
	{
		{
			LogType.Assert,
			Color.white
		},
		{
			LogType.Error,
			Color.red
		},
		{
			LogType.Exception,
			Color.red
		},
		{
			LogType.Log,
			Color.white
		},
		{
			LogType.Warning,
			Color.yellow
		}
	};

	// Token: 0x04000147 RID: 327
	private const int margin = 20;

	// Token: 0x04000148 RID: 328
	private Rect windowRect = new Rect(0f, 0f, (float)Screen.width, (float)(Screen.height / 2));

	// Token: 0x04000149 RID: 329
	private Rect titleBarRect = new Rect(0f, 0f, 10000f, 20f);

	// Token: 0x0400014A RID: 330
	private GUIContent clearLabel = new GUIContent("Очиска", "Очистить содержимое консоли.");

	// Token: 0x0400014B RID: 331
	private GUIContent collapseLabel = new GUIContent("Свернуть", "Свернуть повторяющиеся строки.");

	// Token: 0x02000846 RID: 2118
	private struct Log
	{
		// Token: 0x04003254 RID: 12884
		public string message;

		// Token: 0x04003255 RID: 12885
		public string stackTrace;

		// Token: 0x04003256 RID: 12886
		public LogType type;
	}
}
