using System;
using UnityEngine;

namespace Facebook.Unity.Example
{
	// Token: 0x0200014A RID: 330
	internal class Pay : MenuBase
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x0008AF3D File Offset: 0x0008913D
		protected override void GetGui()
		{
			base.LabelAndTextField("Product: ", ref this.payProduct);
			if (base.Button("Call Pay"))
			{
				this.CallFBPay();
			}
			GUILayout.Space(10f);
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x0008AF70 File Offset: 0x00089170
		private void CallFBPay()
		{
			FB.Canvas.Pay(this.payProduct, "purchaseitem", 1, null, null, null, null, null, new FacebookDelegate<IPayResult>(base.HandleResult));
		}

		// Token: 0x040011B9 RID: 4537
		private string payProduct = string.Empty;
	}
}
