using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C8 RID: 1224
	public class DHPublicKeyParameters : DHKeyParameters
	{
		// Token: 0x06002F9A RID: 12186 RVA: 0x00125DF4 File Offset: 0x00123FF4
		private static BigInteger Validate(BigInteger y, DHParameters dhParams)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			if (y.CompareTo(BigInteger.Two) < 0 || y.CompareTo(dhParams.P.Subtract(BigInteger.Two)) > 0)
			{
				throw new ArgumentException("invalid DH public key", "y");
			}
			if (dhParams.Q != null && !y.ModPow(dhParams.Q, dhParams.P).Equals(BigInteger.One))
			{
				throw new ArgumentException("y value does not appear to be in correct group", "y");
			}
			return y;
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00125E7D File Offset: 0x0012407D
		public DHPublicKeyParameters(BigInteger y, DHParameters parameters) : base(false, parameters)
		{
			this.y = DHPublicKeyParameters.Validate(y, parameters);
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00125E94 File Offset: 0x00124094
		public DHPublicKeyParameters(BigInteger y, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(false, parameters, algorithmOid)
		{
			this.y = DHPublicKeyParameters.Validate(y, parameters);
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002F9D RID: 12189 RVA: 0x00125EAC File Offset: 0x001240AC
		public virtual BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x06002F9E RID: 12190 RVA: 0x00125EB4 File Offset: 0x001240B4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHPublicKeyParameters dhpublicKeyParameters = obj as DHPublicKeyParameters;
			return dhpublicKeyParameters != null && this.Equals(dhpublicKeyParameters);
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00125EDA File Offset: 0x001240DA
		protected bool Equals(DHPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x06002FA0 RID: 12192 RVA: 0x00125EF8 File Offset: 0x001240F8
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001FBC RID: 8124
		private readonly BigInteger y;
	}
}
