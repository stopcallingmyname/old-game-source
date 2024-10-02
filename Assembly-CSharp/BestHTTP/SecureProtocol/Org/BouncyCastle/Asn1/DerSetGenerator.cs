using System;
using System.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000660 RID: 1632
	public class DerSetGenerator : DerGenerator
	{
		// Token: 0x06003CFB RID: 15611 RVA: 0x00172E6C File Offset: 0x0017106C
		public DerSetGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x00172E80 File Offset: 0x00171080
		public DerSetGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream, tagNo, isExplicit)
		{
		}

		// Token: 0x06003CFD RID: 15613 RVA: 0x00172E96 File Offset: 0x00171096
		public override void AddObject(Asn1Encodable obj)
		{
			new DerOutputStream(this._bOut).WriteObject(obj);
		}

		// Token: 0x06003CFE RID: 15614 RVA: 0x00172EA9 File Offset: 0x001710A9
		public override Stream GetRawOutputStream()
		{
			return this._bOut;
		}

		// Token: 0x06003CFF RID: 15615 RVA: 0x00172EB1 File Offset: 0x001710B1
		public override void Close()
		{
			base.WriteDerEncoded(49, this._bOut.ToArray());
		}

		// Token: 0x040026FE RID: 9982
		private readonly MemoryStream _bOut = new MemoryStream();
	}
}
