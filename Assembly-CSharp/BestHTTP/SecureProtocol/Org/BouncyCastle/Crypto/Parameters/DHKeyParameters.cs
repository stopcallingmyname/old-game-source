using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004C5 RID: 1221
	public class DHKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x06002F7C RID: 12156 RVA: 0x00125A42 File Offset: 0x00123C42
		protected DHKeyParameters(bool isPrivate, DHParameters parameters) : this(isPrivate, parameters, PkcsObjectIdentifiers.DhKeyAgreement)
		{
		}

		// Token: 0x06002F7D RID: 12157 RVA: 0x00125A51 File Offset: 0x00123C51
		protected DHKeyParameters(bool isPrivate, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(isPrivate)
		{
			this.parameters = parameters;
			this.algorithmOid = algorithmOid;
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002F7E RID: 12158 RVA: 0x00125A68 File Offset: 0x00123C68
		public DHParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002F7F RID: 12159 RVA: 0x00125A70 File Offset: 0x00123C70
		public DerObjectIdentifier AlgorithmOid
		{
			get
			{
				return this.algorithmOid;
			}
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x00125A78 File Offset: 0x00123C78
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHKeyParameters dhkeyParameters = obj as DHKeyParameters;
			return dhkeyParameters != null && this.Equals(dhkeyParameters);
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x00125A9E File Offset: 0x00123C9E
		protected bool Equals(DHKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x00125ABC File Offset: 0x00123CBC
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001FB1 RID: 8113
		private readonly DHParameters parameters;

		// Token: 0x04001FB2 RID: 8114
		private readonly DerObjectIdentifier algorithmOid;
	}
}
