using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BD RID: 1981
	public class PkiConfirmContent : Asn1Encodable
	{
		// Token: 0x06004692 RID: 18066 RVA: 0x00193C8B File Offset: 0x00191E8B
		public static PkiConfirmContent GetInstance(object obj)
		{
			if (obj is PkiConfirmContent)
			{
				return (PkiConfirmContent)obj;
			}
			if (obj is Asn1Null)
			{
				return new PkiConfirmContent();
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00193CC4 File Offset: 0x00191EC4
		public override Asn1Object ToAsn1Object()
		{
			return DerNull.Instance;
		}
	}
}
