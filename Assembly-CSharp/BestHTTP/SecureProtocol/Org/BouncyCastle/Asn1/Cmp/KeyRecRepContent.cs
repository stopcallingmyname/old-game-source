using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007B9 RID: 1977
	public class KeyRecRepContent : Asn1Encodable
	{
		// Token: 0x06004674 RID: 18036 RVA: 0x00193638 File Offset: 0x00191838
		private KeyRecRepContent(Asn1Sequence seq)
		{
			this.status = PkiStatusInfo.GetInstance(seq[0]);
			for (int i = 1; i < seq.Count; i++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[i]);
				switch (instance.TagNo)
				{
				case 0:
					this.newSigCert = CmpCertificate.GetInstance(instance.GetObject());
					break;
				case 1:
					this.caCerts = Asn1Sequence.GetInstance(instance.GetObject());
					break;
				case 2:
					this.keyPairHist = Asn1Sequence.GetInstance(instance.GetObject());
					break;
				default:
					throw new ArgumentException("unknown tag number: " + instance.TagNo, "seq");
				}
			}
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001936F5 File Offset: 0x001918F5
		public static KeyRecRepContent GetInstance(object obj)
		{
			if (obj is KeyRecRepContent)
			{
				return (KeyRecRepContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new KeyRecRepContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06004676 RID: 18038 RVA: 0x00193734 File Offset: 0x00191934
		public virtual PkiStatusInfo Status
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x0019373C File Offset: 0x0019193C
		public virtual CmpCertificate NewSigCert
		{
			get
			{
				return this.newSigCert;
			}
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x00193744 File Offset: 0x00191944
		public virtual CmpCertificate[] GetCACerts()
		{
			if (this.caCerts == null)
			{
				return null;
			}
			CmpCertificate[] array = new CmpCertificate[this.caCerts.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CmpCertificate.GetInstance(this.caCerts[num]);
			}
			return array;
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x00193790 File Offset: 0x00191990
		public virtual CertifiedKeyPair[] GetKeyPairHist()
		{
			if (this.keyPairHist == null)
			{
				return null;
			}
			CertifiedKeyPair[] array = new CertifiedKeyPair[this.keyPairHist.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertifiedKeyPair.GetInstance(this.keyPairHist[num]);
			}
			return array;
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x001937DC File Offset: 0x001919DC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.status
			});
			this.AddOptional(v, 0, this.newSigCert);
			this.AddOptional(v, 1, this.caCerts);
			this.AddOptional(v, 2, this.keyPairHist);
			return new DerSequence(v);
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0019382E File Offset: 0x00191A2E
		private void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, tagNo, obj)
				});
			}
		}

		// Token: 0x04002DD9 RID: 11737
		private readonly PkiStatusInfo status;

		// Token: 0x04002DDA RID: 11738
		private readonly CmpCertificate newSigCert;

		// Token: 0x04002DDB RID: 11739
		private readonly Asn1Sequence caCerts;

		// Token: 0x04002DDC RID: 11740
		private readonly Asn1Sequence keyPairHist;
	}
}
