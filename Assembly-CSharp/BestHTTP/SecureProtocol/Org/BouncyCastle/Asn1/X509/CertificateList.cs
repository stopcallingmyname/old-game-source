using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200068F RID: 1679
	public class CertificateList : Asn1Encodable
	{
		// Token: 0x06003E2F RID: 15919 RVA: 0x001764E9 File Offset: 0x001746E9
		public static CertificateList GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return CertificateList.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003E30 RID: 15920 RVA: 0x001764F7 File Offset: 0x001746F7
		public static CertificateList GetInstance(object obj)
		{
			if (obj is CertificateList)
			{
				return (CertificateList)obj;
			}
			if (obj != null)
			{
				return new CertificateList(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x06003E31 RID: 15921 RVA: 0x00176518 File Offset: 0x00174718
		private CertificateList(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("sequence wrong size for CertificateList", "seq");
			}
			this.tbsCertList = TbsCertificateList.GetInstance(seq[0]);
			this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
			this.sig = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06003E32 RID: 15922 RVA: 0x0017657A File Offset: 0x0017477A
		public TbsCertificateList TbsCertList
		{
			get
			{
				return this.tbsCertList;
			}
		}

		// Token: 0x06003E33 RID: 15923 RVA: 0x00176582 File Offset: 0x00174782
		public CrlEntry[] GetRevokedCertificates()
		{
			return this.tbsCertList.GetRevokedCertificates();
		}

		// Token: 0x06003E34 RID: 15924 RVA: 0x0017658F File Offset: 0x0017478F
		public IEnumerable GetRevokedCertificateEnumeration()
		{
			return this.tbsCertList.GetRevokedCertificateEnumeration();
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06003E35 RID: 15925 RVA: 0x0017659C File Offset: 0x0017479C
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.sigAlgID;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06003E36 RID: 15926 RVA: 0x001765A4 File Offset: 0x001747A4
		public DerBitString Signature
		{
			get
			{
				return this.sig;
			}
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x001765AC File Offset: 0x001747AC
		public byte[] GetSignatureOctets()
		{
			return this.sig.GetOctets();
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06003E38 RID: 15928 RVA: 0x001765B9 File Offset: 0x001747B9
		public int Version
		{
			get
			{
				return this.tbsCertList.Version;
			}
		}

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x001765C6 File Offset: 0x001747C6
		public X509Name Issuer
		{
			get
			{
				return this.tbsCertList.Issuer;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06003E3A RID: 15930 RVA: 0x001765D3 File Offset: 0x001747D3
		public Time ThisUpdate
		{
			get
			{
				return this.tbsCertList.ThisUpdate;
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x001765E0 File Offset: 0x001747E0
		public Time NextUpdate
		{
			get
			{
				return this.tbsCertList.NextUpdate;
			}
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x001765ED File Offset: 0x001747ED
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.tbsCertList,
				this.sigAlgID,
				this.sig
			});
		}

		// Token: 0x04002791 RID: 10129
		private readonly TbsCertificateList tbsCertList;

		// Token: 0x04002792 RID: 10130
		private readonly AlgorithmIdentifier sigAlgID;

		// Token: 0x04002793 RID: 10131
		private readonly DerBitString sig;
	}
}
