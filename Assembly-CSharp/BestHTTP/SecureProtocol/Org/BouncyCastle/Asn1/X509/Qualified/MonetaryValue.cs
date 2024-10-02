using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.Qualified
{
	// Token: 0x020006D0 RID: 1744
	public class MonetaryValue : Asn1Encodable
	{
		// Token: 0x06004042 RID: 16450 RVA: 0x0017DF18 File Offset: 0x0017C118
		public static MonetaryValue GetInstance(object obj)
		{
			if (obj == null || obj is MonetaryValue)
			{
				return (MonetaryValue)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new MonetaryValue(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0017DF68 File Offset: 0x0017C168
		private MonetaryValue(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.currency = Iso4217CurrencyCode.GetInstance(seq[0]);
			this.amount = DerInteger.GetInstance(seq[1]);
			this.exponent = DerInteger.GetInstance(seq[2]);
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x0017DFDA File Offset: 0x0017C1DA
		public MonetaryValue(Iso4217CurrencyCode currency, int amount, int exponent)
		{
			this.currency = currency;
			this.amount = new DerInteger(amount);
			this.exponent = new DerInteger(exponent);
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0017E001 File Offset: 0x0017C201
		public Iso4217CurrencyCode Currency
		{
			get
			{
				return this.currency;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06004046 RID: 16454 RVA: 0x0017E009 File Offset: 0x0017C209
		public BigInteger Amount
		{
			get
			{
				return this.amount.Value;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x0017E016 File Offset: 0x0017C216
		public BigInteger Exponent
		{
			get
			{
				return this.exponent.Value;
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0017E023 File Offset: 0x0017C223
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.currency,
				this.amount,
				this.exponent
			});
		}

		// Token: 0x040028EB RID: 10475
		internal Iso4217CurrencyCode currency;

		// Token: 0x040028EC RID: 10476
		internal DerInteger amount;

		// Token: 0x040028ED RID: 10477
		internal DerInteger exponent;
	}
}
