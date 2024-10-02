using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C5 RID: 1733
	public class X509ExtensionsGenerator
	{
		// Token: 0x06003FEC RID: 16364 RVA: 0x0017BD59 File Offset: 0x00179F59
		public void Reset()
		{
			this.extensions = Platform.CreateHashtable();
			this.extOrdering = Platform.CreateArrayList();
		}

		// Token: 0x06003FED RID: 16365 RVA: 0x0017BD74 File Offset: 0x00179F74
		public void AddExtension(DerObjectIdentifier oid, bool critical, Asn1Encodable extValue)
		{
			byte[] derEncoded;
			try
			{
				derEncoded = extValue.GetDerEncoded();
			}
			catch (Exception arg)
			{
				throw new ArgumentException("error encoding value: " + arg);
			}
			this.AddExtension(oid, critical, derEncoded);
		}

		// Token: 0x06003FEE RID: 16366 RVA: 0x0017BDB8 File Offset: 0x00179FB8
		public void AddExtension(DerObjectIdentifier oid, bool critical, byte[] extValue)
		{
			if (this.extensions.Contains(oid))
			{
				throw new ArgumentException("extension " + oid + " already added");
			}
			this.extOrdering.Add(oid);
			this.extensions.Add(oid, new X509Extension(critical, new DerOctetString(extValue)));
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x0017BE0E File Offset: 0x0017A00E
		public bool IsEmpty
		{
			get
			{
				return this.extOrdering.Count < 1;
			}
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x0017BE1E File Offset: 0x0017A01E
		public X509Extensions Generate()
		{
			return new X509Extensions(this.extOrdering, this.extensions);
		}

		// Token: 0x04002886 RID: 10374
		private IDictionary extensions = Platform.CreateHashtable();

		// Token: 0x04002887 RID: 10375
		private IList extOrdering = Platform.CreateArrayList();
	}
}
