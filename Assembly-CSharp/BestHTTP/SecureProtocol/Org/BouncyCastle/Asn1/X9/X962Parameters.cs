using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200067B RID: 1659
	public class X962Parameters : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003D93 RID: 15763 RVA: 0x001745D8 File Offset: 0x001727D8
		public static X962Parameters GetInstance(object obj)
		{
			if (obj == null || obj is X962Parameters)
			{
				return (X962Parameters)obj;
			}
			if (obj is Asn1Object)
			{
				return new X962Parameters((Asn1Object)obj);
			}
			if (obj is byte[])
			{
				try
				{
					return new X962Parameters(Asn1Object.FromByteArray((byte[])obj));
				}
				catch (Exception ex)
				{
					throw new ArgumentException("unable to parse encoded data: " + ex.Message, ex);
				}
			}
			throw new ArgumentException("unknown object in getInstance()");
		}

		// Token: 0x06003D94 RID: 15764 RVA: 0x0017465C File Offset: 0x0017285C
		public X962Parameters(X9ECParameters ecParameters)
		{
			this._params = ecParameters.ToAsn1Object();
		}

		// Token: 0x06003D95 RID: 15765 RVA: 0x00174670 File Offset: 0x00172870
		public X962Parameters(DerObjectIdentifier namedCurve)
		{
			this._params = namedCurve;
		}

		// Token: 0x06003D96 RID: 15766 RVA: 0x00174670 File Offset: 0x00172870
		public X962Parameters(Asn1Object obj)
		{
			this._params = obj;
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003D97 RID: 15767 RVA: 0x0017467F File Offset: 0x0017287F
		public bool IsNamedCurve
		{
			get
			{
				return this._params is DerObjectIdentifier;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003D98 RID: 15768 RVA: 0x0017468F File Offset: 0x0017288F
		public bool IsImplicitlyCA
		{
			get
			{
				return this._params is Asn1Null;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06003D99 RID: 15769 RVA: 0x0017469F File Offset: 0x0017289F
		public Asn1Object Parameters
		{
			get
			{
				return this._params;
			}
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x0017469F File Offset: 0x0017289F
		public override Asn1Object ToAsn1Object()
		{
			return this._params;
		}

		// Token: 0x0400271F RID: 10015
		private readonly Asn1Object _params;
	}
}
