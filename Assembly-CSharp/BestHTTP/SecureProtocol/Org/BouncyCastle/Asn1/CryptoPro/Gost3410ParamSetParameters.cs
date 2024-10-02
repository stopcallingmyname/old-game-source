using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000762 RID: 1890
	public class Gost3410ParamSetParameters : Asn1Encodable
	{
		// Token: 0x060043F1 RID: 17393 RVA: 0x0018C91D File Offset: 0x0018AB1D
		public static Gost3410ParamSetParameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Gost3410ParamSetParameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0018C92B File Offset: 0x0018AB2B
		public static Gost3410ParamSetParameters GetInstance(object obj)
		{
			if (obj == null || obj is Gost3410ParamSetParameters)
			{
				return (Gost3410ParamSetParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Gost3410ParamSetParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0018C968 File Offset: 0x0018AB68
		public Gost3410ParamSetParameters(int keySize, BigInteger p, BigInteger q, BigInteger a)
		{
			this.keySize = keySize;
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.a = new DerInteger(a);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0018C99C File Offset: 0x0018AB9C
		private Gost3410ParamSetParameters(Asn1Sequence seq)
		{
			if (seq.Count != 4)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.keySize = DerInteger.GetInstance(seq[0]).Value.IntValue;
			this.p = DerInteger.GetInstance(seq[1]);
			this.q = DerInteger.GetInstance(seq[2]);
			this.a = DerInteger.GetInstance(seq[3]);
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060043F5 RID: 17397 RVA: 0x0018CA1A File Offset: 0x0018AC1A
		public int KeySize
		{
			get
			{
				return this.keySize;
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0018CA22 File Offset: 0x0018AC22
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060043F7 RID: 17399 RVA: 0x0018CA2F File Offset: 0x0018AC2F
		public BigInteger Q
		{
			get
			{
				return this.q.PositiveValue;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0018CA3C File Offset: 0x0018AC3C
		public BigInteger A
		{
			get
			{
				return this.a.PositiveValue;
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0018CA49 File Offset: 0x0018AC49
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				new DerInteger(this.keySize),
				this.p,
				this.q,
				this.a
			});
		}

		// Token: 0x04002CA0 RID: 11424
		private readonly int keySize;

		// Token: 0x04002CA1 RID: 11425
		private readonly DerInteger p;

		// Token: 0x04002CA2 RID: 11426
		private readonly DerInteger q;

		// Token: 0x04002CA3 RID: 11427
		private readonly DerInteger a;
	}
}
