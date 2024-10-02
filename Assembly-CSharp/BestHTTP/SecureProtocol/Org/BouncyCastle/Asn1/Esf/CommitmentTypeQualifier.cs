using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000744 RID: 1860
	public class CommitmentTypeQualifier : Asn1Encodable
	{
		// Token: 0x06004335 RID: 17205 RVA: 0x00189641 File Offset: 0x00187841
		public CommitmentTypeQualifier(DerObjectIdentifier commitmentTypeIdentifier) : this(commitmentTypeIdentifier, null)
		{
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0018964B File Offset: 0x0018784B
		public CommitmentTypeQualifier(DerObjectIdentifier commitmentTypeIdentifier, Asn1Encodable qualifier)
		{
			if (commitmentTypeIdentifier == null)
			{
				throw new ArgumentNullException("commitmentTypeIdentifier");
			}
			this.commitmentTypeIdentifier = commitmentTypeIdentifier;
			if (qualifier != null)
			{
				this.qualifier = qualifier.ToAsn1Object();
			}
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x00189678 File Offset: 0x00187878
		public CommitmentTypeQualifier(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.commitmentTypeIdentifier = (DerObjectIdentifier)seq[0].ToAsn1Object();
			if (seq.Count > 1)
			{
				this.qualifier = seq[1].ToAsn1Object();
			}
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x00189700 File Offset: 0x00187900
		public static CommitmentTypeQualifier GetInstance(object obj)
		{
			if (obj == null || obj is CommitmentTypeQualifier)
			{
				return (CommitmentTypeQualifier)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CommitmentTypeQualifier((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CommitmentTypeQualifier' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06004339 RID: 17209 RVA: 0x0018974D File Offset: 0x0018794D
		public DerObjectIdentifier CommitmentTypeIdentifier
		{
			get
			{
				return this.commitmentTypeIdentifier;
			}
		}

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600433A RID: 17210 RVA: 0x00189755 File Offset: 0x00187955
		public Asn1Object Qualifier
		{
			get
			{
				return this.qualifier;
			}
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x00189760 File Offset: 0x00187960
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.commitmentTypeIdentifier
			});
			if (this.qualifier != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.qualifier
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C24 RID: 11300
		private readonly DerObjectIdentifier commitmentTypeIdentifier;

		// Token: 0x04002C25 RID: 11301
		private readonly Asn1Object qualifier;
	}
}
