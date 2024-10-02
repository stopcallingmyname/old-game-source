using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000625 RID: 1573
	public abstract class Asn1Generator
	{
		// Token: 0x06003B51 RID: 15185 RVA: 0x0016E3BA File Offset: 0x0016C5BA
		protected Asn1Generator(Stream outStream)
		{
			this._out = outStream;
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06003B52 RID: 15186 RVA: 0x0016E3C9 File Offset: 0x0016C5C9
		protected Stream Out
		{
			get
			{
				return this._out;
			}
		}

		// Token: 0x06003B53 RID: 15187
		public abstract void AddObject(Asn1Encodable obj);

		// Token: 0x06003B54 RID: 15188
		public abstract Stream GetRawOutputStream();

		// Token: 0x06003B55 RID: 15189
		public abstract void Close();

		// Token: 0x04002698 RID: 9880
		private Stream _out;
	}
}
