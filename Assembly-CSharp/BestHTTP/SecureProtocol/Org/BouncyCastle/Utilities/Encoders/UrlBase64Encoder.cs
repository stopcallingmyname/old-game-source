using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders
{
	// Token: 0x02000292 RID: 658
	public class UrlBase64Encoder : Base64Encoder
	{
		// Token: 0x060017F8 RID: 6136 RVA: 0x000B96FA File Offset: 0x000B78FA
		public UrlBase64Encoder()
		{
			this.encodingTable[this.encodingTable.Length - 2] = 45;
			this.encodingTable[this.encodingTable.Length - 1] = 95;
			this.padding = 46;
			base.InitialiseDecodingTable();
		}
	}
}
