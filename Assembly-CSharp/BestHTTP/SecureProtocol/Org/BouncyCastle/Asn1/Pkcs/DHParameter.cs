using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F0 RID: 1776
	public class DHParameter : Asn1Encodable
	{
		// Token: 0x06004114 RID: 16660 RVA: 0x0018197F File Offset: 0x0017FB7F
		public DHParameter(BigInteger p, BigInteger g, int l)
		{
			this.p = new DerInteger(p);
			this.g = new DerInteger(g);
			if (l != 0)
			{
				this.l = new DerInteger(l);
			}
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x001819B0 File Offset: 0x0017FBB0
		public DHParameter(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.p = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.g = (DerInteger)enumerator.Current;
			if (enumerator.MoveNext())
			{
				this.l = (DerInteger)enumerator.Current;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06004116 RID: 16662 RVA: 0x00181A13 File Offset: 0x0017FC13
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06004117 RID: 16663 RVA: 0x00181A20 File Offset: 0x0017FC20
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06004118 RID: 16664 RVA: 0x00181A2D File Offset: 0x0017FC2D
		public BigInteger L
		{
			get
			{
				if (this.l != null)
				{
					return this.l.PositiveValue;
				}
				return null;
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x00181A44 File Offset: 0x0017FC44
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.p,
				this.g
			});
			if (this.l != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.l
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x040029BD RID: 10685
		internal DerInteger p;

		// Token: 0x040029BE RID: 10686
		internal DerInteger g;

		// Token: 0x040029BF RID: 10687
		internal DerInteger l;
	}
}
