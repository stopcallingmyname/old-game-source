using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AC RID: 1964
	public class CertifiedKeyPair : Asn1Encodable
	{
		// Token: 0x06004625 RID: 17957 RVA: 0x00192750 File Offset: 0x00190950
		private CertifiedKeyPair(Asn1Sequence seq)
		{
			this.certOrEncCert = CertOrEncCert.GetInstance(seq[0]);
			if (seq.Count >= 2)
			{
				if (seq.Count == 2)
				{
					Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[1]);
					if (instance.TagNo == 0)
					{
						this.privateKey = EncryptedValue.GetInstance(instance.GetObject());
						return;
					}
					this.publicationInfo = PkiPublicationInfo.GetInstance(instance.GetObject());
					return;
				}
				else
				{
					this.privateKey = EncryptedValue.GetInstance(Asn1TaggedObject.GetInstance(seq[1]));
					this.publicationInfo = PkiPublicationInfo.GetInstance(Asn1TaggedObject.GetInstance(seq[2]));
				}
			}
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x001927EE File Offset: 0x001909EE
		public static CertifiedKeyPair GetInstance(object obj)
		{
			if (obj is CertifiedKeyPair)
			{
				return (CertifiedKeyPair)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertifiedKeyPair((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x0019282D File Offset: 0x00190A2D
		public CertifiedKeyPair(CertOrEncCert certOrEncCert) : this(certOrEncCert, null, null)
		{
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x00192838 File Offset: 0x00190A38
		public CertifiedKeyPair(CertOrEncCert certOrEncCert, EncryptedValue privateKey, PkiPublicationInfo publicationInfo)
		{
			if (certOrEncCert == null)
			{
				throw new ArgumentNullException("certOrEncCert");
			}
			this.certOrEncCert = certOrEncCert;
			this.privateKey = privateKey;
			this.publicationInfo = publicationInfo;
		}

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06004629 RID: 17961 RVA: 0x00192863 File Offset: 0x00190A63
		public virtual CertOrEncCert CertOrEncCert
		{
			get
			{
				return this.certOrEncCert;
			}
		}

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x0600462A RID: 17962 RVA: 0x0019286B File Offset: 0x00190A6B
		public virtual EncryptedValue PrivateKey
		{
			get
			{
				return this.privateKey;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x0600462B RID: 17963 RVA: 0x00192873 File Offset: 0x00190A73
		public virtual PkiPublicationInfo PublicationInfo
		{
			get
			{
				return this.publicationInfo;
			}
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x0019287C File Offset: 0x00190A7C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.certOrEncCert
			});
			if (this.privateKey != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.privateKey)
				});
			}
			if (this.publicationInfo != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.publicationInfo)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DA4 RID: 11684
		private readonly CertOrEncCert certOrEncCert;

		// Token: 0x04002DA5 RID: 11685
		private readonly EncryptedValue privateKey;

		// Token: 0x04002DA6 RID: 11686
		private readonly PkiPublicationInfo publicationInfo;
	}
}
