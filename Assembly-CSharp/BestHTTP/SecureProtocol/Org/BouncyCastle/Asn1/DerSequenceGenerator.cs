using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065D RID: 1629
	public class DerSequenceGenerator : DerGenerator
	{
		// Token: 0x06003CEA RID: 15594 RVA: 0x00172C64 File Offset: 0x00170E64
		public DerSequenceGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x00172C78 File Offset: 0x00170E78
		public DerSequenceGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x00172C8E File Offset: 0x00170E8E
		public override void AddObject(Asn1Encodable obj)
		{
			new DerOutputStream(this._bOut).WriteObject(obj);
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x00172CA1 File Offset: 0x00170EA1
		public override Stream GetRawOutputStream()
		{
			return this._bOut;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x00172CA9 File Offset: 0x00170EA9
		public override void Close()
		{
			base.WriteDerEncoded(48, this._bOut.ToArray());
		}

		// Token: 0x040026FB RID: 9979
		private readonly MemoryStream _bOut = new MemoryStream();
	}
}
