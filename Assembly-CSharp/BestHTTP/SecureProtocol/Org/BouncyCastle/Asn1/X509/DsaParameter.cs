using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200069A RID: 1690
	public class DsaParameter : Asn1Encodable
	{
		// Token: 0x06003E7B RID: 15995 RVA: 0x001771A4 File Offset: 0x001753A4
		public static DsaParameter GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return DsaParameter.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x001771B2 File Offset: 0x001753B2
		public static DsaParameter GetInstance(object obj)
		{
			if (obj == null || obj is DsaParameter)
			{
				return (DsaParameter)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new DsaParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid DsaParameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x001771EF File Offset: 0x001753EF
		public DsaParameter(BigInteger p, BigInteger q, BigInteger g)
		{
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.g = new DerInteger(g);
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0017721C File Offset: 0x0017541C
		private DsaParameter(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.p = DerInteger.GetInstance(seq[0]);
			this.q = DerInteger.GetInstance(seq[1]);
			this.g = DerInteger.GetInstance(seq[2]);
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003E7F RID: 15999 RVA: 0x0017728E File Offset: 0x0017548E
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06003E80 RID: 16000 RVA: 0x0017729B File Offset: 0x0017549B
		public BigInteger Q
		{
			get
			{
				return this.q.PositiveValue;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06003E81 RID: 16001 RVA: 0x001772A8 File Offset: 0x001754A8
		public BigInteger G
		{
			get
			{
				return this.g.PositiveValue;
			}
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x001772B5 File Offset: 0x001754B5
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.p,
				this.q,
				this.g
			});
		}

		// Token: 0x040027B3 RID: 10163
		internal readonly DerInteger p;

		// Token: 0x040027B4 RID: 10164
		internal readonly DerInteger q;

		// Token: 0x040027B5 RID: 10165
		internal readonly DerInteger g;
	}
}
