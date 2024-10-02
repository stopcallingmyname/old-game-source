using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x02000727 RID: 1831
	public class AdmissionSyntax : Asn1Encodable
	{
		// Token: 0x06004277 RID: 17015 RVA: 0x00186660 File Offset: 0x00184860
		public static AdmissionSyntax GetInstance(object obj)
		{
			if (obj == null || obj is AdmissionSyntax)
			{
				return (AdmissionSyntax)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new AdmissionSyntax((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x001866B0 File Offset: 0x001848B0
		private AdmissionSyntax(Asn1Sequence seq)
		{
			int count = seq.Count;
			if (count == 1)
			{
				this.contentsOfAdmissions = Asn1Sequence.GetInstance(seq[0]);
				return;
			}
			if (count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			this.admissionAuthority = GeneralName.GetInstance(seq[0]);
			this.contentsOfAdmissions = Asn1Sequence.GetInstance(seq[1]);
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00186726 File Offset: 0x00184926
		public AdmissionSyntax(GeneralName admissionAuthority, Asn1Sequence contentsOfAdmissions)
		{
			this.admissionAuthority = admissionAuthority;
			this.contentsOfAdmissions = contentsOfAdmissions;
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x0018673C File Offset: 0x0018493C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.admissionAuthority != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.admissionAuthority
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.contentsOfAdmissions
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x0600427B RID: 17019 RVA: 0x0018678C File Offset: 0x0018498C
		public virtual GeneralName AdmissionAuthority
		{
			get
			{
				return this.admissionAuthority;
			}
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x00186794 File Offset: 0x00184994
		public virtual Admissions[] GetContentsOfAdmissions()
		{
			Admissions[] array = new Admissions[this.contentsOfAdmissions.Count];
			for (int i = 0; i < this.contentsOfAdmissions.Count; i++)
			{
				array[i] = Admissions.GetInstance(this.contentsOfAdmissions[i]);
			}
			return array;
		}

		// Token: 0x04002B67 RID: 11111
		private readonly GeneralName admissionAuthority;

		// Token: 0x04002B68 RID: 11112
		private readonly Asn1Sequence contentsOfAdmissions;
	}
}
