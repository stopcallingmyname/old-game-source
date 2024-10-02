using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006F7 RID: 1783
	public class PbeParameter : Asn1Encodable
	{
		// Token: 0x0600413D RID: 16701 RVA: 0x00181FA0 File Offset: 0x001801A0
		public static PbeParameter GetInstance(object obj)
		{
			if (obj is PbeParameter || obj == null)
			{
				return (PbeParameter)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PbeParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00181FF0 File Offset: 0x001801F0
		private PbeParameter(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.salt = Asn1OctetString.GetInstance(seq[0]);
			this.iterationCount = DerInteger.GetInstance(seq[1]);
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00182040 File Offset: 0x00180240
		public PbeParameter(byte[] salt, int iterationCount)
		{
			this.salt = new DerOctetString(salt);
			this.iterationCount = new DerInteger(iterationCount);
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00182060 File Offset: 0x00180260
		public byte[] GetSalt()
		{
			return this.salt.GetOctets();
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x0018206D File Offset: 0x0018026D
		public BigInteger IterationCount
		{
			get
			{
				return this.iterationCount.Value;
			}
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0018207A File Offset: 0x0018027A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.salt,
				this.iterationCount
			});
		}

		// Token: 0x040029C8 RID: 10696
		private readonly Asn1OctetString salt;

		// Token: 0x040029C9 RID: 10697
		private readonly DerInteger iterationCount;
	}
}
