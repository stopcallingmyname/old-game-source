using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x0200075F RID: 1887
	public class ECGost3410ParamSetParameters : Asn1Encodable
	{
		// Token: 0x060043DF RID: 17375 RVA: 0x0018C51B File Offset: 0x0018A71B
		public static ECGost3410ParamSetParameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return ECGost3410ParamSetParameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0018C529 File Offset: 0x0018A729
		public static ECGost3410ParamSetParameters GetInstance(object obj)
		{
			if (obj == null || obj is ECGost3410ParamSetParameters)
			{
				return (ECGost3410ParamSetParameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ECGost3410ParamSetParameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0018C568 File Offset: 0x0018A768
		public ECGost3410ParamSetParameters(BigInteger a, BigInteger b, BigInteger p, BigInteger q, int x, BigInteger y)
		{
			this.a = new DerInteger(a);
			this.b = new DerInteger(b);
			this.p = new DerInteger(p);
			this.q = new DerInteger(q);
			this.x = new DerInteger(x);
			this.y = new DerInteger(y);
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0018C5C8 File Offset: 0x0018A7C8
		public ECGost3410ParamSetParameters(Asn1Sequence seq)
		{
			if (seq.Count != 6)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.a = DerInteger.GetInstance(seq[0]);
			this.b = DerInteger.GetInstance(seq[1]);
			this.p = DerInteger.GetInstance(seq[2]);
			this.q = DerInteger.GetInstance(seq[3]);
			this.x = DerInteger.GetInstance(seq[4]);
			this.y = DerInteger.GetInstance(seq[5]);
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x060043E3 RID: 17379 RVA: 0x0018C660 File Offset: 0x0018A860
		public BigInteger P
		{
			get
			{
				return this.p.PositiveValue;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x0018C66D File Offset: 0x0018A86D
		public BigInteger Q
		{
			get
			{
				return this.q.PositiveValue;
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060043E5 RID: 17381 RVA: 0x0018C67A File Offset: 0x0018A87A
		public BigInteger A
		{
			get
			{
				return this.a.PositiveValue;
			}
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0018C688 File Offset: 0x0018A888
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.a,
				this.b,
				this.p,
				this.q,
				this.x,
				this.y
			});
		}

		// Token: 0x04002C93 RID: 11411
		internal readonly DerInteger p;

		// Token: 0x04002C94 RID: 11412
		internal readonly DerInteger q;

		// Token: 0x04002C95 RID: 11413
		internal readonly DerInteger a;

		// Token: 0x04002C96 RID: 11414
		internal readonly DerInteger b;

		// Token: 0x04002C97 RID: 11415
		internal readonly DerInteger x;

		// Token: 0x04002C98 RID: 11416
		internal readonly DerInteger y;
	}
}
