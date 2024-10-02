using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X9
{
	// Token: 0x02000674 RID: 1652
	public class DHDomainParameters : Asn1Encodable
	{
		// Token: 0x06003D62 RID: 15714 RVA: 0x00173BD7 File Offset: 0x00171DD7
		public static DHDomainParameters GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return DHDomainParameters.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x00173BE8 File Offset: 0x00171DE8
		public static DHDomainParameters GetInstance(object obj)
		{
			if (obj == null || obj is DHDomainParameters)
			{
				return (DHDomainParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DHDomainParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DHDomainParameters: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003D64 RID: 15716 RVA: 0x00173C38 File Offset: 0x00171E38
		public DHDomainParameters(DerInteger p, DerInteger g, DerInteger q, DerInteger j, DHValidationParms validationParms)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.p = p;
			this.g = g;
			this.q = q;
			this.j = j;
			this.validationParms = validationParms;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00173C9C File Offset: 0x00171E9C
		private DHDomainParameters(Asn1Sequence seq)
		{
			if (seq.Count < 3 || seq.Count > 5)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			IEnumerator enumerator = seq.GetEnumerator();
			this.p = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			this.g = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			this.q = DerInteger.GetInstance(DHDomainParameters.GetNext(enumerator));
			Asn1Encodable next = DHDomainParameters.GetNext(enumerator);
			if (next != null && next is DerInteger)
			{
				this.j = DerInteger.GetInstance(next);
				next = DHDomainParameters.GetNext(enumerator);
			}
			if (next != null)
			{
				this.validationParms = DHValidationParms.GetInstance(next.ToAsn1Object());
			}
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x00173D54 File Offset: 0x00171F54
		private static Asn1Encodable GetNext(IEnumerator e)
		{
			if (!e.MoveNext())
			{
				return null;
			}
			return (Asn1Encodable)e.Current;
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06003D67 RID: 15719 RVA: 0x00173D6B File Offset: 0x00171F6B
		public DerInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06003D68 RID: 15720 RVA: 0x00173D73 File Offset: 0x00171F73
		public DerInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x00173D7B File Offset: 0x00171F7B
		public DerInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003D6A RID: 15722 RVA: 0x00173D83 File Offset: 0x00171F83
		public DerInteger J
		{
			get
			{
				return this.j;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x00173D8B File Offset: 0x00171F8B
		public DHValidationParms ValidationParms
		{
			get
			{
				return this.validationParms;
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x00173D94 File Offset: 0x00171F94
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.p,
				this.g,
				this.q
			});
			if (this.j != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.j
				});
			}
			if (this.validationParms != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.validationParms
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x0400270F RID: 9999
		private readonly DerInteger p;

		// Token: 0x04002710 RID: 10000
		private readonly DerInteger g;

		// Token: 0x04002711 RID: 10001
		private readonly DerInteger q;

		// Token: 0x04002712 RID: 10002
		private readonly DerInteger j;

		// Token: 0x04002713 RID: 10003
		private readonly DHValidationParms validationParms;
	}
}
