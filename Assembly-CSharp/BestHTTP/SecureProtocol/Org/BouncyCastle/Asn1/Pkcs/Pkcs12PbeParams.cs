using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006FB RID: 1787
	public class Pkcs12PbeParams : Asn1Encodable
	{
		// Token: 0x0600415B RID: 16731 RVA: 0x001824CA File Offset: 0x001806CA
		public Pkcs12PbeParams(byte[] salt, int iterations)
		{
			this.iv = new DerOctetString(salt);
			this.iterations = new DerInteger(iterations);
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x001824EC File Offset: 0x001806EC
		private Pkcs12PbeParams(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.iv = Asn1OctetString.GetInstance(seq[0]);
			this.iterations = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0018253C File Offset: 0x0018073C
		public static Pkcs12PbeParams GetInstance(object obj)
		{
			if (obj is Pkcs12PbeParams)
			{
				return (Pkcs12PbeParams)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Pkcs12PbeParams((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x0018257B File Offset: 0x0018077B
		public BigInteger Iterations
		{
			get
			{
				return this.iterations.Value;
			}
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00182588 File Offset: 0x00180788
		public byte[] GetIV()
		{
			return this.iv.GetOctets();
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00182595 File Offset: 0x00180795
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.iv,
				this.iterations
			});
		}

		// Token: 0x040029D3 RID: 10707
		private readonly DerInteger iterations;

		// Token: 0x040029D4 RID: 10708
		private readonly Asn1OctetString iv;
	}
}
