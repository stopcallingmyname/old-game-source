using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004DD RID: 1245
	public class ElGamalKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x0600301D RID: 12317 RVA: 0x00126F0D File Offset: 0x0012510D
		protected ElGamalKeyParameters(bool isPrivate, ElGamalParameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x00126F1D File Offset: 0x0012511D
		public ElGamalParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x00126F28 File Offset: 0x00125128
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalKeyParameters elGamalKeyParameters = obj as ElGamalKeyParameters;
			return elGamalKeyParameters != null && this.Equals(elGamalKeyParameters);
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x00126F4E File Offset: 0x0012514E
		protected bool Equals(ElGamalKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x00126F6C File Offset: 0x0012516C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001FEA RID: 8170
		private readonly ElGamalParameters parameters;
	}
}
