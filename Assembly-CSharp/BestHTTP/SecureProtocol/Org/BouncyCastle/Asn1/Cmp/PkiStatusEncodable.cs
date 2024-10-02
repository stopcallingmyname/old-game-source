using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C5 RID: 1989
	public class PkiStatusEncodable : Asn1Encodable
	{
		// Token: 0x060046DA RID: 18138 RVA: 0x001945E5 File Offset: 0x001927E5
		private PkiStatusEncodable(PkiStatus status) : this(new DerInteger((int)status))
		{
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x001945F3 File Offset: 0x001927F3
		private PkiStatusEncodable(DerInteger status)
		{
			this.status = status;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00194602 File Offset: 0x00192802
		public static PkiStatusEncodable GetInstance(object obj)
		{
			if (obj is PkiStatusEncodable)
			{
				return (PkiStatusEncodable)obj;
			}
			if (obj is DerInteger)
			{
				return new PkiStatusEncodable((DerInteger)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060046DD RID: 18141 RVA: 0x00194641 File Offset: 0x00192841
		public virtual BigInteger Value
		{
			get
			{
				return this.status.Value;
			}
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x0019464E File Offset: 0x0019284E
		public override Asn1Object ToAsn1Object()
		{
			return this.status;
		}

		// Token: 0x04002E45 RID: 11845
		public static readonly PkiStatusEncodable granted = new PkiStatusEncodable(PkiStatus.Granted);

		// Token: 0x04002E46 RID: 11846
		public static readonly PkiStatusEncodable grantedWithMods = new PkiStatusEncodable(PkiStatus.GrantedWithMods);

		// Token: 0x04002E47 RID: 11847
		public static readonly PkiStatusEncodable rejection = new PkiStatusEncodable(PkiStatus.Rejection);

		// Token: 0x04002E48 RID: 11848
		public static readonly PkiStatusEncodable waiting = new PkiStatusEncodable(PkiStatus.Waiting);

		// Token: 0x04002E49 RID: 11849
		public static readonly PkiStatusEncodable revocationWarning = new PkiStatusEncodable(PkiStatus.RevocationWarning);

		// Token: 0x04002E4A RID: 11850
		public static readonly PkiStatusEncodable revocationNotification = new PkiStatusEncodable(PkiStatus.RevocationNotification);

		// Token: 0x04002E4B RID: 11851
		public static readonly PkiStatusEncodable keyUpdateWaiting = new PkiStatusEncodable(PkiStatus.KeyUpdateWarning);

		// Token: 0x04002E4C RID: 11852
		private readonly DerInteger status;
	}
}
