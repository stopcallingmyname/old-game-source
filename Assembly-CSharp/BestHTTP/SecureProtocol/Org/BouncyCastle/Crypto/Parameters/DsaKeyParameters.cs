using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004CB RID: 1227
	public abstract class DsaKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002FA9 RID: 12201 RVA: 0x00125FEC File Offset: 0x001241EC
		protected DsaKeyParameters(bool isPrivate, DsaParameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002FAA RID: 12202 RVA: 0x00125FFC File Offset: 0x001241FC
		public DsaParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x06002FAB RID: 12203 RVA: 0x00126004 File Offset: 0x00124204
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaKeyParameters dsaKeyParameters = obj as DsaKeyParameters;
			return dsaKeyParameters != null && this.Equals(dsaKeyParameters);
		}

		// Token: 0x06002FAC RID: 12204 RVA: 0x0012602A File Offset: 0x0012422A
		protected bool Equals(DsaKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x06002FAD RID: 12205 RVA: 0x00126048 File Offset: 0x00124248
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001FC0 RID: 8128
		private readonly DsaParameters parameters;
	}
}
