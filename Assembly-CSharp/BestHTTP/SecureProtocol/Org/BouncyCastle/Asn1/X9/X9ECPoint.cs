using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x0200067F RID: 1663
	public class X9ECPoint : Asn1Encodable
	{
		// Token: 0x06003DB5 RID: 15797 RVA: 0x00174D14 File Offset: 0x00172F14
		public X9ECPoint(ECPoint p) : this(p, false)
		{
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00174D1E File Offset: 0x00172F1E
		public X9ECPoint(ECPoint p, bool compressed)
		{
			this.p = p.Normalize();
			this.encoding = new DerOctetString(p.GetEncoded(compressed));
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00174D44 File Offset: 0x00172F44
		public X9ECPoint(ECCurve c, byte[] encoding)
		{
			this.c = c;
			this.encoding = new DerOctetString(Arrays.Clone(encoding));
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00174D64 File Offset: 0x00172F64
		public X9ECPoint(ECCurve c, Asn1OctetString s) : this(c, s.GetOctets())
		{
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00174D73 File Offset: 0x00172F73
		public byte[] GetPointEncoding()
		{
			return Arrays.Clone(this.encoding.GetOctets());
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x00174D85 File Offset: 0x00172F85
		public ECPoint Point
		{
			get
			{
				if (this.p == null)
				{
					this.p = this.c.DecodePoint(this.encoding.GetOctets()).Normalize();
				}
				return this.p;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003DBB RID: 15803 RVA: 0x00174DB8 File Offset: 0x00172FB8
		public bool IsPointCompressed
		{
			get
			{
				byte[] octets = this.encoding.GetOctets();
				return octets != null && octets.Length != 0 && (octets[0] == 2 || octets[0] == 3);
			}
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00174DE8 File Offset: 0x00172FE8
		public override Asn1Object ToAsn1Object()
		{
			return this.encoding;
		}

		// Token: 0x0400272A RID: 10026
		private readonly Asn1OctetString encoding;

		// Token: 0x0400272B RID: 10027
		private ECCurve c;

		// Token: 0x0400272C RID: 10028
		private ECPoint p;
	}
}
