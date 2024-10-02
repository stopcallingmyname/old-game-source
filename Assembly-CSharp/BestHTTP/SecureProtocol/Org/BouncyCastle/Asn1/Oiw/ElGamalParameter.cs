using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Oiw
{
	// Token: 0x02000705 RID: 1797
	public class ElGamalParameter : Asn1Encodable
	{
		// Token: 0x060041B4 RID: 16820 RVA: 0x00183E24 File Offset: 0x00182024
		public ElGamalParameter(BigInteger p, BigInteger g)
		{
			this.p = new DerInteger(p);
			this.g = new DerInteger(g);
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x00183E44 File Offset: 0x00182044
		public ElGamalParameter(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.p = DerInteger.GetInstance(seq[0]);
			this.g = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x060041B6 RID: 16822 RVA: 0x00183E94 File Offset: 0x00182094
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x060041B7 RID: 16823 RVA: 0x00183EA1 File Offset: 0x001820A1
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x060041B8 RID: 16824 RVA: 0x00183EAE File Offset: 0x001820AE
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.p,
				this.g
			});
		}

		// Token: 0x04002A92 RID: 10898
		internal DerInteger p;

		// Token: 0x04002A93 RID: 10899
		internal DerInteger g;
	}
}
