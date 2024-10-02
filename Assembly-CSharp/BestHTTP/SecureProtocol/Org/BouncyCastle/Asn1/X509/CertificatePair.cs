using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000690 RID: 1680
	public class CertificatePair : Asn1Encodable
	{
		// Token: 0x06003E3D RID: 15933 RVA: 0x00176618 File Offset: 0x00174818
		public static CertificatePair GetInstance(object obj)
		{
			if (obj == null || obj is CertificatePair)
			{
				return (CertificatePair)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertificatePair((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x00176668 File Offset: 0x00174868
		private CertificatePair(Asn1Sequence seq)
		{
			if (seq.Count != 1 && seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			foreach (object obj in seq)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(obj);
				if (instance.TagNo == 0)
				{
					this.forward = X509CertificateStructure.GetInstance(instance, true);
				}
				else
				{
					if (instance.TagNo != 1)
					{
						throw new ArgumentException("Bad tag number: " + instance.TagNo);
					}
					this.reverse = X509CertificateStructure.GetInstance(instance, true);
				}
			}
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x00176738 File Offset: 0x00174938
		public CertificatePair(X509CertificateStructure forward, X509CertificateStructure reverse)
		{
			this.forward = forward;
			this.reverse = reverse;
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x00176750 File Offset: 0x00174950
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.forward != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(0, this.forward)
				});
			}
			if (this.reverse != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(1, this.reverse)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x001767B4 File Offset: 0x001749B4
		public X509CertificateStructure Forward
		{
			get
			{
				return this.forward;
			}
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06003E42 RID: 15938 RVA: 0x001767BC File Offset: 0x001749BC
		public X509CertificateStructure Reverse
		{
			get
			{
				return this.reverse;
			}
		}

		// Token: 0x04002794 RID: 10132
		private X509CertificateStructure forward;

		// Token: 0x04002795 RID: 10133
		private X509CertificateStructure reverse;
	}
}
