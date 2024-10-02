using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000675 RID: 1653
	public class DHPublicKey : Asn1Encodable
	{
		// Token: 0x06003D6D RID: 15725 RVA: 0x00173E08 File Offset: 0x00172008
		public static DHPublicKey GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHPublicKey.GetInstance(DerInteger.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00173E18 File Offset: 0x00172018
		public static DHPublicKey GetInstance(object obj)
		{
			if (obj == null || obj is DHPublicKey)
			{
				return (DHPublicKey)obj;
			}
			if (obj is DerInteger)
			{
				return new DHPublicKey((DerInteger)obj);
			}
			throw new ArgumentException("Invalid DHPublicKey: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00173E65 File Offset: 0x00172065
		public DHPublicKey(DerInteger y)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x00173E82 File Offset: 0x00172082
		public DerInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00173E82 File Offset: 0x00172082
		public override Asn1Object ToAsn1Object()
		{
			return this.y;
		}

		// Token: 0x04002714 RID: 10004
		private readonly DerInteger y;
	}
}
