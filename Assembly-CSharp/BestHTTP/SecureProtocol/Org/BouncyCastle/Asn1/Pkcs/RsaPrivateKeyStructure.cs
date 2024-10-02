using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000700 RID: 1792
	public class RsaPrivateKeyStructure : Asn1Encodable
	{
		// Token: 0x06004181 RID: 16769 RVA: 0x001833DF File Offset: 0x001815DF
		public static RsaPrivateKeyStructure GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return RsaPrivateKeyStructure.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004182 RID: 16770 RVA: 0x001833ED File Offset: 0x001815ED
		public static RsaPrivateKeyStructure GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is RsaPrivateKeyStructure)
			{
				return (RsaPrivateKeyStructure)obj;
			}
			return new RsaPrivateKeyStructure(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00183410 File Offset: 0x00181610
		public RsaPrivateKeyStructure(BigInteger modulus, BigInteger publicExponent, BigInteger privateExponent, BigInteger prime1, BigInteger prime2, BigInteger exponent1, BigInteger exponent2, BigInteger coefficient)
		{
			this.modulus = modulus;
			this.publicExponent = publicExponent;
			this.privateExponent = privateExponent;
			this.prime1 = prime1;
			this.prime2 = prime2;
			this.exponent1 = exponent1;
			this.exponent2 = exponent2;
			this.coefficient = coefficient;
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00183460 File Offset: 0x00181660
		[Obsolete("Use 'GetInstance' method(s) instead")]
		public RsaPrivateKeyStructure(Asn1Sequence seq)
		{
			if (((DerInteger)seq[0]).Value.IntValue != 0)
			{
				throw new ArgumentException("wrong version for RSA private key");
			}
			this.modulus = ((DerInteger)seq[1]).Value;
			this.publicExponent = ((DerInteger)seq[2]).Value;
			this.privateExponent = ((DerInteger)seq[3]).Value;
			this.prime1 = ((DerInteger)seq[4]).Value;
			this.prime2 = ((DerInteger)seq[5]).Value;
			this.exponent1 = ((DerInteger)seq[6]).Value;
			this.exponent2 = ((DerInteger)seq[7]).Value;
			this.coefficient = ((DerInteger)seq[8]).Value;
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06004185 RID: 16773 RVA: 0x0018354E File Offset: 0x0018174E
		public BigInteger Modulus
		{
			get
			{
				return this.modulus;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06004186 RID: 16774 RVA: 0x00183556 File Offset: 0x00181756
		public BigInteger PublicExponent
		{
			get
			{
				return this.publicExponent;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06004187 RID: 16775 RVA: 0x0018355E File Offset: 0x0018175E
		public BigInteger PrivateExponent
		{
			get
			{
				return this.privateExponent;
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06004188 RID: 16776 RVA: 0x00183566 File Offset: 0x00181766
		public BigInteger Prime1
		{
			get
			{
				return this.prime1;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06004189 RID: 16777 RVA: 0x0018356E File Offset: 0x0018176E
		public BigInteger Prime2
		{
			get
			{
				return this.prime2;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600418A RID: 16778 RVA: 0x00183576 File Offset: 0x00181776
		public BigInteger Exponent1
		{
			get
			{
				return this.exponent1;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x0600418B RID: 16779 RVA: 0x0018357E File Offset: 0x0018177E
		public BigInteger Exponent2
		{
			get
			{
				return this.exponent2;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x0600418C RID: 16780 RVA: 0x00183586 File Offset: 0x00181786
		public BigInteger Coefficient
		{
			get
			{
				return this.coefficient;
			}
		}

		// Token: 0x0600418D RID: 16781 RVA: 0x00183590 File Offset: 0x00181790
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(0),
				new DerInteger(this.Modulus),
				new DerInteger(this.PublicExponent),
				new DerInteger(this.PrivateExponent),
				new DerInteger(this.Prime1),
				new DerInteger(this.Prime2),
				new DerInteger(this.Exponent1),
				new DerInteger(this.Exponent2),
				new DerInteger(this.Coefficient)
			});
		}

		// Token: 0x04002A72 RID: 10866
		private readonly BigInteger modulus;

		// Token: 0x04002A73 RID: 10867
		private readonly BigInteger publicExponent;

		// Token: 0x04002A74 RID: 10868
		private readonly BigInteger privateExponent;

		// Token: 0x04002A75 RID: 10869
		private readonly BigInteger prime1;

		// Token: 0x04002A76 RID: 10870
		private readonly BigInteger prime2;

		// Token: 0x04002A77 RID: 10871
		private readonly BigInteger exponent1;

		// Token: 0x04002A78 RID: 10872
		private readonly BigInteger exponent2;

		// Token: 0x04002A79 RID: 10873
		private readonly BigInteger coefficient;
	}
}
