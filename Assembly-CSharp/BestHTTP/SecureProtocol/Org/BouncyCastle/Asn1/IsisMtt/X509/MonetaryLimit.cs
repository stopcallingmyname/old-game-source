using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x02000729 RID: 1833
	public class MonetaryLimit : Asn1Encodable
	{
		// Token: 0x06004287 RID: 17031 RVA: 0x00186994 File Offset: 0x00184B94
		public static MonetaryLimit GetInstance(object obj)
		{
			if (obj == null || obj is MonetaryLimit)
			{
				return (MonetaryLimit)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new MonetaryLimit(Asn1Sequence.GetInstance(obj));
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x001869E4 File Offset: 0x00184BE4
		private MonetaryLimit(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.currency = DerPrintableString.GetInstance(seq[0]);
			this.amount = DerInteger.GetInstance(seq[1]);
			this.exponent = DerInteger.GetInstance(seq[2]);
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x00186A51 File Offset: 0x00184C51
		public MonetaryLimit(string currency, int amount, int exponent)
		{
			this.currency = new DerPrintableString(currency, true);
			this.amount = new DerInteger(amount);
			this.exponent = new DerInteger(exponent);
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600428A RID: 17034 RVA: 0x00186A7E File Offset: 0x00184C7E
		public virtual string Currency
		{
			get
			{
				return this.currency.GetString();
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600428B RID: 17035 RVA: 0x00186A8B File Offset: 0x00184C8B
		public virtual BigInteger Amount
		{
			get
			{
				return this.amount.Value;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600428C RID: 17036 RVA: 0x00186A98 File Offset: 0x00184C98
		public virtual BigInteger Exponent
		{
			get
			{
				return this.exponent.Value;
			}
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x00186AA5 File Offset: 0x00184CA5
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.currency,
				this.amount,
				this.exponent
			});
		}

		// Token: 0x04002B6A RID: 11114
		private readonly DerPrintableString currency;

		// Token: 0x04002B6B RID: 11115
		private readonly DerInteger amount;

		// Token: 0x04002B6C RID: 11116
		private readonly DerInteger exponent;
	}
}
