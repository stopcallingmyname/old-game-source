using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000689 RID: 1673
	public class AttributeCertificate : Asn1Encodable
	{
		// Token: 0x06003DF6 RID: 15862 RVA: 0x00175A35 File Offset: 0x00173C35
		public static AttributeCertificate GetInstance(object obj)
		{
			if (obj is AttributeCertificate)
			{
				return (AttributeCertificate)obj;
			}
			if (obj != null)
			{
				return new AttributeCertificate(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00175A56 File Offset: 0x00173C56
		public AttributeCertificate(AttributeCertificateInfo acinfo, AlgorithmIdentifier signatureAlgorithm, DerBitString signatureValue)
		{
			this.acinfo = acinfo;
			this.signatureAlgorithm = signatureAlgorithm;
			this.signatureValue = signatureValue;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00175A74 File Offset: 0x00173C74
		private AttributeCertificate(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.acinfo = AttributeCertificateInfo.GetInstance(seq[0]);
			this.signatureAlgorithm = AlgorithmIdentifier.GetInstance(seq[1]);
			this.signatureValue = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x00175AE1 File Offset: 0x00173CE1
		public AttributeCertificateInfo ACInfo
		{
			get
			{
				return this.acinfo;
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x00175AE9 File Offset: 0x00173CE9
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.signatureAlgorithm;
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x00175AF1 File Offset: 0x00173CF1
		public DerBitString SignatureValue
		{
			get
			{
				return this.signatureValue;
			}
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00175AF9 File Offset: 0x00173CF9
		public byte[] GetSignatureOctets()
		{
			return this.signatureValue.GetOctets();
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x00175B06 File Offset: 0x00173D06
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.acinfo,
				this.signatureAlgorithm,
				this.signatureValue
			});
		}

		// Token: 0x0400277E RID: 10110
		private readonly AttributeCertificateInfo acinfo;

		// Token: 0x0400277F RID: 10111
		private readonly AlgorithmIdentifier signatureAlgorithm;

		// Token: 0x04002780 RID: 10112
		private readonly DerBitString signatureValue;
	}
}
