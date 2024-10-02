using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C7 RID: 1223
	public class DHPrivateKeyParameters : DHKeyParameters
	{
		// Token: 0x06002F94 RID: 12180 RVA: 0x00125D70 File Offset: 0x00123F70
		public DHPrivateKeyParameters(BigInteger x, DHParameters parameters) : base(true, parameters)
		{
			this.x = x;
		}

		// Token: 0x06002F95 RID: 12181 RVA: 0x00125D81 File Offset: 0x00123F81
		public DHPrivateKeyParameters(BigInteger x, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(true, parameters, algorithmOid)
		{
			this.x = x;
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002F96 RID: 12182 RVA: 0x00125D93 File Offset: 0x00123F93
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x06002F97 RID: 12183 RVA: 0x00125D9C File Offset: 0x00123F9C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHPrivateKeyParameters dhprivateKeyParameters = obj as DHPrivateKeyParameters;
			return dhprivateKeyParameters != null && this.Equals(dhprivateKeyParameters);
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00125DC2 File Offset: 0x00123FC2
		protected bool Equals(DHPrivateKeyParameters other)
		{
			return this.x.Equals(other.x) && base.Equals(other);
		}

		// Token: 0x06002F99 RID: 12185 RVA: 0x00125DE0 File Offset: 0x00123FE0
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FBB RID: 8123
		private readonly BigInteger x;
	}
}
