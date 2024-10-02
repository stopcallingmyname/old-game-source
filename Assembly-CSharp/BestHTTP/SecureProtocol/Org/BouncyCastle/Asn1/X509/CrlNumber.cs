using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000694 RID: 1684
	public class CrlNumber : DerInteger
	{
		// Token: 0x06003E53 RID: 15955 RVA: 0x00176A56 File Offset: 0x00174C56
		public CrlNumber(BigInteger number) : base(number)
		{
		}

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06003E54 RID: 15956 RVA: 0x00176A5F File Offset: 0x00174C5F
		public BigInteger Number
		{
			get
			{
				return base.PositiveValue;
			}
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x00176A67 File Offset: 0x00174C67
		public override string ToString()
		{
			return "CRLNumber: " + this.Number;
		}
	}
}
