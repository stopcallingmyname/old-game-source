using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000676 RID: 1654
	public class DHValidationParms : Asn1Encodable
	{
		// Token: 0x06003D72 RID: 15730 RVA: 0x00173E8A File Offset: 0x0017208A
		public static DHValidationParms GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHValidationParms.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x00173E98 File Offset: 0x00172098
		public static DHValidationParms GetInstance(object obj)
		{
			if (obj == null || obj is DHDomainParameters)
			{
				return (DHValidationParms)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DHValidationParms((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DHValidationParms: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x00173EE5 File Offset: 0x001720E5
		public DHValidationParms(DerBitString seed, DerInteger pgenCounter)
		{
			if (seed == null)
			{
				throw new ArgumentNullException("seed");
			}
			if (pgenCounter == null)
			{
				throw new ArgumentNullException("pgenCounter");
			}
			this.seed = seed;
			this.pgenCounter = pgenCounter;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x00173F18 File Offset: 0x00172118
		private DHValidationParms(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.seed = DerBitString.GetInstance(seq[0]);
			this.pgenCounter = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003D76 RID: 15734 RVA: 0x00173F78 File Offset: 0x00172178
		public DerBitString Seed
		{
			get
			{
				return this.seed;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x00173F80 File Offset: 0x00172180
		public DerInteger PgenCounter
		{
			get
			{
				return this.pgenCounter;
			}
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x00173F88 File Offset: 0x00172188
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.seed,
				this.pgenCounter
			});
		}

		// Token: 0x04002715 RID: 10005
		private readonly DerBitString seed;

		// Token: 0x04002716 RID: 10006
		private readonly DerInteger pgenCounter;
	}
}
