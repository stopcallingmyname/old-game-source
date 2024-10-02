using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto
{
	// Token: 0x020003C3 RID: 963
	public abstract class AsymmetricKeyParameter : ICipherParameters
	{
		// Token: 0x060027AC RID: 10156 RVA: 0x0010BE05 File Offset: 0x0010A005
		protected AsymmetricKeyParameter(bool privateKey)
		{
			this.privateKey = privateKey;
		}

		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060027AD RID: 10157 RVA: 0x0010BE14 File Offset: 0x0010A014
		public bool IsPrivate
		{
			get
			{
				return this.privateKey;
			}
		}

		// Token: 0x060027AE RID: 10158 RVA: 0x0010BE1C File Offset: 0x0010A01C
		public override bool Equals(object obj)
		{
			AsymmetricKeyParameter asymmetricKeyParameter = obj as AsymmetricKeyParameter;
			return asymmetricKeyParameter != null && this.Equals(asymmetricKeyParameter);
		}

		// Token: 0x060027AF RID: 10159 RVA: 0x0010BE3C File Offset: 0x0010A03C
		protected bool Equals(AsymmetricKeyParameter other)
		{
			return this.privateKey == other.privateKey;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x0010BE4C File Offset: 0x0010A04C
		public override int GetHashCode()
		{
			return this.privateKey.GetHashCode();
		}

		// Token: 0x04001AF4 RID: 6900
		private readonly bool privateKey;
	}
}
