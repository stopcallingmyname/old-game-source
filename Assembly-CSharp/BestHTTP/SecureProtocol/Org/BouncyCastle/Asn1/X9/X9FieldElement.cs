using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000680 RID: 1664
	public class X9FieldElement : Asn1Encodable
	{
		// Token: 0x06003DBD RID: 15805 RVA: 0x00174DF0 File Offset: 0x00172FF0
		public X9FieldElement(ECFieldElement f)
		{
			this.f = f;
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00174DFF File Offset: 0x00172FFF
		[Obsolete("Will be removed")]
		public X9FieldElement(BigInteger p, Asn1OctetString s) : this(new FpFieldElement(p, new BigInteger(1, s.GetOctets())))
		{
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00174E19 File Offset: 0x00173019
		[Obsolete("Will be removed")]
		public X9FieldElement(int m, int k1, int k2, int k3, Asn1OctetString s) : this(new F2mFieldElement(m, k1, k2, k3, new BigInteger(1, s.GetOctets())))
		{
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x00174E38 File Offset: 0x00173038
		public ECFieldElement Value
		{
			get
			{
				return this.f;
			}
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x00174E40 File Offset: 0x00173040
		public override Asn1Object ToAsn1Object()
		{
			int byteLength = X9IntegerConverter.GetByteLength(this.f);
			return new DerOctetString(X9IntegerConverter.IntegerToBytes(this.f.ToBigInteger(), byteLength));
		}

		// Token: 0x0400272D RID: 10029
		private ECFieldElement f;
	}
}
