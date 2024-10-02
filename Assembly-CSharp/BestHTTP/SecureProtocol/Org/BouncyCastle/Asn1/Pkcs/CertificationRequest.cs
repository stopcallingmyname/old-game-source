using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x020006ED RID: 1773
	public class CertificationRequest : Asn1Encodable
	{
		// Token: 0x060040FC RID: 16636 RVA: 0x00181590 File Offset: 0x0017F790
		public static CertificationRequest GetInstance(object obj)
		{
			if (obj is CertificationRequest)
			{
				return (CertificationRequest)obj;
			}
			if (obj != null)
			{
				return new CertificationRequest((Asn1Sequence)obj);
			}
			return null;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0016E9FD File Offset: 0x0016CBFD
		protected CertificationRequest()
		{
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x001815B1 File Offset: 0x0017F7B1
		public CertificationRequest(CertificationRequestInfo requestInfo, AlgorithmIdentifier algorithm, DerBitString signature)
		{
			this.reqInfo = requestInfo;
			this.sigAlgId = algorithm;
			this.sigBits = signature;
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001815D0 File Offset: 0x0017F7D0
		[Obsolete("Use 'GetInstance' instead")]
		public CertificationRequest(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.reqInfo = CertificationRequestInfo.GetInstance(seq[0]);
			this.sigAlgId = AlgorithmIdentifier.GetInstance(seq[1]);
			this.sigBits = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00181632 File Offset: 0x0017F832
		public CertificationRequestInfo GetCertificationRequestInfo()
		{
			return this.reqInfo;
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06004101 RID: 16641 RVA: 0x0018163A File Offset: 0x0017F83A
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.sigAlgId;
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06004102 RID: 16642 RVA: 0x00181642 File Offset: 0x0017F842
		public DerBitString Signature
		{
			get
			{
				return this.sigBits;
			}
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x0018164A File Offset: 0x0017F84A
		public byte[] GetSignatureOctets()
		{
			return this.sigBits.GetOctets();
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00181657 File Offset: 0x0017F857
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.reqInfo,
				this.sigAlgId,
				this.sigBits
			});
		}

		// Token: 0x040029B4 RID: 10676
		protected CertificationRequestInfo reqInfo;

		// Token: 0x040029B5 RID: 10677
		protected AlgorithmIdentifier sigAlgId;

		// Token: 0x040029B6 RID: 10678
		protected DerBitString sigBits;
	}
}
