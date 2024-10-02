using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BB RID: 1979
	public class PbmParameter : Asn1Encodable
	{
		// Token: 0x06004682 RID: 18050 RVA: 0x0019395C File Offset: 0x00191B5C
		private PbmParameter(Asn1Sequence seq)
		{
			this.salt = Asn1OctetString.GetInstance(seq[0]);
			this.owf = AlgorithmIdentifier.GetInstance(seq[1]);
			this.iterationCount = DerInteger.GetInstance(seq[2]);
			this.mac = AlgorithmIdentifier.GetInstance(seq[3]);
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x001939B7 File Offset: 0x00191BB7
		public static PbmParameter GetInstance(object obj)
		{
			if (obj is PbmParameter)
			{
				return (PbmParameter)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PbmParameter((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x001939F6 File Offset: 0x00191BF6
		public PbmParameter(byte[] salt, AlgorithmIdentifier owf, int iterationCount, AlgorithmIdentifier mac) : this(new DerOctetString(salt), owf, new DerInteger(iterationCount), mac)
		{
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x00193A0D File Offset: 0x00191C0D
		public PbmParameter(Asn1OctetString salt, AlgorithmIdentifier owf, DerInteger iterationCount, AlgorithmIdentifier mac)
		{
			this.salt = salt;
			this.owf = owf;
			this.iterationCount = iterationCount;
			this.mac = mac;
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06004686 RID: 18054 RVA: 0x00193A32 File Offset: 0x00191C32
		public virtual Asn1OctetString Salt
		{
			get
			{
				return this.salt;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x00193A3A File Offset: 0x00191C3A
		public virtual AlgorithmIdentifier Owf
		{
			get
			{
				return this.owf;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06004688 RID: 18056 RVA: 0x00193A42 File Offset: 0x00191C42
		public virtual DerInteger IterationCount
		{
			get
			{
				return this.iterationCount;
			}
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x00193A4A File Offset: 0x00191C4A
		public virtual AlgorithmIdentifier Mac
		{
			get
			{
				return this.mac;
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00193A52 File Offset: 0x00191C52
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.salt,
				this.owf,
				this.iterationCount,
				this.mac
			});
		}

		// Token: 0x04002DE0 RID: 11744
		private Asn1OctetString salt;

		// Token: 0x04002DE1 RID: 11745
		private AlgorithmIdentifier owf;

		// Token: 0x04002DE2 RID: 11746
		private DerInteger iterationCount;

		// Token: 0x04002DE3 RID: 11747
		private AlgorithmIdentifier mac;
	}
}
