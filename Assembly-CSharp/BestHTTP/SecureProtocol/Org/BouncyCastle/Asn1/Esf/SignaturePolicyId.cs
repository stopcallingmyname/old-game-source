using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000756 RID: 1878
	public class SignaturePolicyId : Asn1Encodable
	{
		// Token: 0x060043AB RID: 17323 RVA: 0x0018B350 File Offset: 0x00189550
		public static SignaturePolicyId GetInstance(object obj)
		{
			if (obj == null || obj is SignaturePolicyId)
			{
				return (SignaturePolicyId)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SignaturePolicyId((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'SignaturePolicyId' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x0018B3A0 File Offset: 0x001895A0
		private SignaturePolicyId(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 2 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.sigPolicyIdentifier = (DerObjectIdentifier)seq[0].ToAsn1Object();
			this.sigPolicyHash = OtherHashAlgAndValue.GetInstance(seq[1].ToAsn1Object());
			if (seq.Count > 2)
			{
				this.sigPolicyQualifiers = (Asn1Sequence)seq[2].ToAsn1Object();
			}
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x0018B441 File Offset: 0x00189641
		public SignaturePolicyId(DerObjectIdentifier sigPolicyIdentifier, OtherHashAlgAndValue sigPolicyHash) : this(sigPolicyIdentifier, sigPolicyHash, null)
		{
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x0018B44C File Offset: 0x0018964C
		public SignaturePolicyId(DerObjectIdentifier sigPolicyIdentifier, OtherHashAlgAndValue sigPolicyHash, params SigPolicyQualifierInfo[] sigPolicyQualifiers)
		{
			if (sigPolicyIdentifier == null)
			{
				throw new ArgumentNullException("sigPolicyIdentifier");
			}
			if (sigPolicyHash == null)
			{
				throw new ArgumentNullException("sigPolicyHash");
			}
			this.sigPolicyIdentifier = sigPolicyIdentifier;
			this.sigPolicyHash = sigPolicyHash;
			if (sigPolicyQualifiers != null)
			{
				this.sigPolicyQualifiers = new DerSequence(sigPolicyQualifiers);
			}
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x0018B49C File Offset: 0x0018969C
		public SignaturePolicyId(DerObjectIdentifier sigPolicyIdentifier, OtherHashAlgAndValue sigPolicyHash, IEnumerable sigPolicyQualifiers)
		{
			if (sigPolicyIdentifier == null)
			{
				throw new ArgumentNullException("sigPolicyIdentifier");
			}
			if (sigPolicyHash == null)
			{
				throw new ArgumentNullException("sigPolicyHash");
			}
			this.sigPolicyIdentifier = sigPolicyIdentifier;
			this.sigPolicyHash = sigPolicyHash;
			if (sigPolicyQualifiers != null)
			{
				if (!CollectionUtilities.CheckElementsAreOfType(sigPolicyQualifiers, typeof(SigPolicyQualifierInfo)))
				{
					throw new ArgumentException("Must contain only 'SigPolicyQualifierInfo' objects", "sigPolicyQualifiers");
				}
				this.sigPolicyQualifiers = new DerSequence(Asn1EncodableVector.FromEnumerable(sigPolicyQualifiers));
			}
		}

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x0018B50F File Offset: 0x0018970F
		public DerObjectIdentifier SigPolicyIdentifier
		{
			get
			{
				return this.sigPolicyIdentifier;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0018B517 File Offset: 0x00189717
		public OtherHashAlgAndValue SigPolicyHash
		{
			get
			{
				return this.sigPolicyHash;
			}
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x0018B520 File Offset: 0x00189720
		public SigPolicyQualifierInfo[] GetSigPolicyQualifiers()
		{
			if (this.sigPolicyQualifiers == null)
			{
				return null;
			}
			SigPolicyQualifierInfo[] array = new SigPolicyQualifierInfo[this.sigPolicyQualifiers.Count];
			for (int i = 0; i < this.sigPolicyQualifiers.Count; i++)
			{
				array[i] = SigPolicyQualifierInfo.GetInstance(this.sigPolicyQualifiers[i]);
			}
			return array;
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0018B574 File Offset: 0x00189774
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.sigPolicyIdentifier,
				this.sigPolicyHash.ToAsn1Object()
			});
			if (this.sigPolicyQualifiers != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.sigPolicyQualifiers.ToAsn1Object()
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C53 RID: 11347
		private readonly DerObjectIdentifier sigPolicyIdentifier;

		// Token: 0x04002C54 RID: 11348
		private readonly OtherHashAlgAndValue sigPolicyHash;

		// Token: 0x04002C55 RID: 11349
		private readonly Asn1Sequence sigPolicyQualifiers;
	}
}
