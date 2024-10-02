using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200067E RID: 1662
	public abstract class X9ECParametersHolder
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x00174CC0 File Offset: 0x00172EC0
		public X9ECParameters Parameters
		{
			get
			{
				X9ECParameters result;
				lock (this)
				{
					if (this.parameters == null)
					{
						this.parameters = this.CreateParameters();
					}
					result = this.parameters;
				}
				return result;
			}
		}

		// Token: 0x06003DB3 RID: 15795
		protected abstract X9ECParameters CreateParameters();

		// Token: 0x04002729 RID: 10025
		private X9ECParameters parameters;
	}
}
