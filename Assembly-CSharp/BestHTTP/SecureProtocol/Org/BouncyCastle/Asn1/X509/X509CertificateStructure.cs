using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C1 RID: 1729
	public class X509CertificateStructure : Asn1Encodable
	{
		// Token: 0x06003FBE RID: 16318 RVA: 0x0017B165 File Offset: 0x00179365
		public static X509CertificateStructure GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return X509CertificateStructure.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x0017B173 File Offset: 0x00179373
		public static X509CertificateStructure GetInstance(object obj)
		{
			if (obj is X509CertificateStructure)
			{
				return (X509CertificateStructure)obj;
			}
			if (obj == null)
			{
				return null;
			}
			return new X509CertificateStructure(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x0017B194 File Offset: 0x00179394
		public X509CertificateStructure(TbsCertificateStructure tbsCert, AlgorithmIdentifier sigAlgID, DerBitString sig)
		{
			if (tbsCert == null)
			{
				throw new ArgumentNullException("tbsCert");
			}
			if (sigAlgID == null)
			{
				throw new ArgumentNullException("sigAlgID");
			}
			if (sig == null)
			{
				throw new ArgumentNullException("sig");
			}
			this.tbsCert = tbsCert;
			this.sigAlgID = sigAlgID;
			this.sig = sig;
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x0017B1E8 File Offset: 0x001793E8
		private X509CertificateStructure(Asn1Sequence seq)
		{
			if (seq.Count != 3)
			{
				throw new ArgumentException("sequence wrong size for a certificate", "seq");
			}
			this.tbsCert = TbsCertificateStructure.GetInstance(seq[0]);
			this.sigAlgID = AlgorithmIdentifier.GetInstance(seq[1]);
			this.sig = DerBitString.GetInstance(seq[2]);
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x0017B24A File Offset: 0x0017944A
		public TbsCertificateStructure TbsCertificate
		{
			get
			{
				return this.tbsCert;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06003FC3 RID: 16323 RVA: 0x0017B252 File Offset: 0x00179452
		public int Version
		{
			get
			{
				return this.tbsCert.Version;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06003FC4 RID: 16324 RVA: 0x0017B25F File Offset: 0x0017945F
		public DerInteger SerialNumber
		{
			get
			{
				return this.tbsCert.SerialNumber;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x0017B26C File Offset: 0x0017946C
		public X509Name Issuer
		{
			get
			{
				return this.tbsCert.Issuer;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06003FC6 RID: 16326 RVA: 0x0017B279 File Offset: 0x00179479
		public Time StartDate
		{
			get
			{
				return this.tbsCert.StartDate;
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06003FC7 RID: 16327 RVA: 0x0017B286 File Offset: 0x00179486
		public Time EndDate
		{
			get
			{
				return this.tbsCert.EndDate;
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003FC8 RID: 16328 RVA: 0x0017B293 File Offset: 0x00179493
		public X509Name Subject
		{
			get
			{
				return this.tbsCert.Subject;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x0017B2A0 File Offset: 0x001794A0
		public SubjectPublicKeyInfo SubjectPublicKeyInfo
		{
			get
			{
				return this.tbsCert.SubjectPublicKeyInfo;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003FCA RID: 16330 RVA: 0x0017B2AD File Offset: 0x001794AD
		public AlgorithmIdentifier SignatureAlgorithm
		{
			get
			{
				return this.sigAlgID;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06003FCB RID: 16331 RVA: 0x0017B2B5 File Offset: 0x001794B5
		public DerBitString Signature
		{
			get
			{
				return this.sig;
			}
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0017B2BD File Offset: 0x001794BD
		public byte[] GetSignatureOctets()
		{
			return this.sig.GetOctets();
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0017B2CA File Offset: 0x001794CA
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.tbsCert,
				this.sigAlgID,
				this.sig
			});
		}

		// Token: 0x0400285F RID: 10335
		private readonly TbsCertificateStructure tbsCert;

		// Token: 0x04002860 RID: 10336
		private readonly AlgorithmIdentifier sigAlgID;

		// Token: 0x04002861 RID: 10337
		private readonly DerBitString sig;
	}
}
