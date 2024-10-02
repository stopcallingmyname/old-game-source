using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math.EC;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004D1 RID: 1233
	public class ECDomainParameters
	{
		// Token: 0x06002FD1 RID: 12241 RVA: 0x00126427 File Offset: 0x00124627
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, BigInteger.One, null)
		{
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x00126438 File Offset: 0x00124638
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00126448 File Offset: 0x00124648
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed)
		{
			if (curve == null)
			{
				throw new ArgumentNullException("curve");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (n == null)
			{
				throw new ArgumentNullException("n");
			}
			this.curve = curve;
			this.g = ECDomainParameters.Validate(curve, g);
			this.n = n;
			this.h = h;
			this.seed = Arrays.Clone(seed);
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002FD4 RID: 12244 RVA: 0x001264B5 File Offset: 0x001246B5
		public ECCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06002FD5 RID: 12245 RVA: 0x001264BD File Offset: 0x001246BD
		public ECPoint G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x001264C5 File Offset: 0x001246C5
		public BigInteger N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x001264CD File Offset: 0x001246CD
		public BigInteger H
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06002FD8 RID: 12248 RVA: 0x001264D8 File Offset: 0x001246D8
		public BigInteger HInv
		{
			get
			{
				BigInteger result;
				lock (this)
				{
					if (this.hInv == null)
					{
						this.hInv = this.h.ModInverse(this.n);
					}
					result = this.hInv;
				}
				return result;
			}
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x00126534 File Offset: 0x00124734
		public byte[] GetSeed()
		{
			return Arrays.Clone(this.seed);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x00126544 File Offset: 0x00124744
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECDomainParameters ecdomainParameters = obj as ECDomainParameters;
			return ecdomainParameters != null && this.Equals(ecdomainParameters);
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x0012656C File Offset: 0x0012476C
		protected virtual bool Equals(ECDomainParameters other)
		{
			return this.curve.Equals(other.curve) && this.g.Equals(other.g) && this.n.Equals(other.n) && this.h.Equals(other.h);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x001265C5 File Offset: 0x001247C5
		public override int GetHashCode()
		{
			return ((this.curve.GetHashCode() * 37 ^ this.g.GetHashCode()) * 37 ^ this.n.GetHashCode()) * 37 ^ this.h.GetHashCode();
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x00126600 File Offset: 0x00124800
		internal static ECPoint Validate(ECCurve c, ECPoint q)
		{
			if (q == null)
			{
				throw new ArgumentException("Point has null value", "q");
			}
			q = ECAlgorithms.ImportPoint(c, q).Normalize();
			if (q.IsInfinity)
			{
				throw new ArgumentException("Point at infinity", "q");
			}
			if (!q.IsValid())
			{
				throw new ArgumentException("Point not on curve", "q");
			}
			return q;
		}

		// Token: 0x04001FD1 RID: 8145
		internal ECCurve curve;

		// Token: 0x04001FD2 RID: 8146
		internal byte[] seed;

		// Token: 0x04001FD3 RID: 8147
		internal ECPoint g;

		// Token: 0x04001FD4 RID: 8148
		internal BigInteger n;

		// Token: 0x04001FD5 RID: 8149
		internal BigInteger h;

		// Token: 0x04001FD6 RID: 8150
		internal BigInteger hInv;
	}
}
