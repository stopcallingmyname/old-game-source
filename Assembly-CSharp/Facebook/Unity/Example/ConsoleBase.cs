using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Facebook.Unity.Example
{
	// Token: 0x0200013F RID: 319
	internal class ConsoleBase : MonoBehaviour
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00089A96 File Offset: 0x00087C96
		protected static int ButtonHeight
		{
			get
			{
				if (!Constants.IsMobile)
				{
					return 24;
				}
				return 60;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00089AA4 File Offset: 0x00087CA4
		protected static int MainWindowWidth
		{
			get
			{
				if (!Constants.IsMobile)
				{
					return 700;
				}
				return Screen.width - 30;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00089ABB File Offset: 0x00087CBB
		protected static int MainWindowFullWidth
		{
			get
			{
				if (!Constants.IsMobile)
				{
					return 760;
				}
				return Screen.width;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00089ACF File Offset: 0x00087CCF
		protected static int MarginFix
		{
			get
			{
				if (!Constants.IsMobile)
				{
					return 48;
				}
				return 0;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00089ADC File Offset: 0x00087CDC
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x00089AE3 File Offset: 0x00087CE3
		protected static Stack<string> MenuStack
		{
			get
			{
				return ConsoleBase.menuStack;
			}
			set
			{
				ConsoleBase.menuStack = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00089AEB File Offset: 0x00087CEB
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00089AF3 File Offset: 0x00087CF3
		protected string Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00089AFC File Offset: 0x00087CFC
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00089B04 File Offset: 0x00087D04
		protected Texture2D LastResponseTexture { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00089B0D File Offset: 0x00087D0D
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00089B15 File Offset: 0x00087D15
		protected string LastResponse
		{
			get
			{
				return this.lastResponse;
			}
			set
			{
				this.lastResponse = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00089B1E File Offset: 0x00087D1E
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00089B26 File Offset: 0x00087D26
		protected Vector2 ScrollPosition
		{
			get
			{
				return this.scrollPosition;
			}
			set
			{
				this.scrollPosition = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00089B2F File Offset: 0x00087D2F
		protected float ScaleFactor
		{
			get
			{
				if (this.scaleFactor == null)
				{
					this.scaleFactor = new float?(Screen.dpi / 160f);
				}
				return this.scaleFactor.Value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00089B5F File Offset: 0x00087D5F
		protected int FontSize
		{
			get
			{
				return (int)Math.Round((double)(this.ScaleFactor * 16f));
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00089B74 File Offset: 0x00087D74
		protected GUIStyle TextStyle
		{
			get
			{
				if (this.textStyle == null)
				{
					this.textStyle = new GUIStyle(GUI.skin.textArea);
					this.textStyle.alignment = TextAnchor.UpperLeft;
					this.textStyle.wordWrap = true;
					this.textStyle.padding = new RectOffset(10, 10, 10, 10);
					this.textStyle.stretchHeight = true;
					this.textStyle.stretchWidth = false;
					this.textStyle.fontSize = this.FontSize;
				}
				return this.textStyle;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00089BFD File Offset: 0x00087DFD
		protected GUIStyle ButtonStyle
		{
			get
			{
				if (this.buttonStyle == null)
				{
					this.buttonStyle = new GUIStyle(GUI.skin.button);
					this.buttonStyle.fontSize = this.FontSize;
				}
				return this.buttonStyle;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00089C33 File Offset: 0x00087E33
		protected GUIStyle TextInputStyle
		{
			get
			{
				if (this.textInputStyle == null)
				{
					this.textInputStyle = new GUIStyle(GUI.skin.textField);
					this.textInputStyle.fontSize = this.FontSize;
				}
				return this.textInputStyle;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00089C69 File Offset: 0x00087E69
		protected GUIStyle LabelStyle
		{
			get
			{
				if (this.labelStyle == null)
				{
					this.labelStyle = new GUIStyle(GUI.skin.label);
					this.labelStyle.fontSize = this.FontSize;
				}
				return this.labelStyle;
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00089C9F File Offset: 0x00087E9F
		protected virtual void Awake()
		{
			Application.targetFrameRate = 60;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00089CA8 File Offset: 0x00087EA8
		protected bool Button(string label)
		{
			return GUILayout.Button(label, this.ButtonStyle, new GUILayoutOption[]
			{
				GUILayout.MinHeight((float)ConsoleBase.ButtonHeight * this.ScaleFactor),
				GUILayout.MaxWidth((float)ConsoleBase.MainWindowWidth)
			});
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00089CE0 File Offset: 0x00087EE0
		protected void LabelAndTextField(string label, ref string text)
		{
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label(label, this.LabelStyle, new GUILayoutOption[]
			{
				GUILayout.MaxWidth(200f * this.ScaleFactor)
			});
			text = GUILayout.TextField(text, this.TextInputStyle, new GUILayoutOption[]
			{
				GUILayout.MaxWidth((float)(ConsoleBase.MainWindowWidth - 150))
			});
			GUILayout.EndHorizontal();
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x0006AE98 File Offset: 0x00069098
		protected bool IsHorizontalLayout()
		{
			return true;
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x00089D4B File Offset: 0x00087F4B
		protected void SwitchMenu(Type menuClass)
		{
			ConsoleBase.menuStack.Push(base.GetType().Name);
			SceneManager.LoadScene(menuClass.Name);
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00089D6D File Offset: 0x00087F6D
		protected void GoBack()
		{
			if (ConsoleBase.menuStack.Any<string>())
			{
				SceneManager.LoadScene(ConsoleBase.menuStack.Pop());
			}
		}

		// Token: 0x04001194 RID: 4500
		private const int DpiScalingFactor = 160;

		// Token: 0x04001195 RID: 4501
		private static Stack<string> menuStack = new Stack<string>();

		// Token: 0x04001196 RID: 4502
		private string status = "Ready";

		// Token: 0x04001197 RID: 4503
		private string lastResponse = string.Empty;

		// Token: 0x04001198 RID: 4504
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001199 RID: 4505
		private float? scaleFactor;

		// Token: 0x0400119A RID: 4506
		private GUIStyle textStyle;

		// Token: 0x0400119B RID: 4507
		private GUIStyle buttonStyle;

		// Token: 0x0400119C RID: 4508
		private GUIStyle textInputStyle;

		// Token: 0x0400119D RID: 4509
		private GUIStyle labelStyle;
	}
}
