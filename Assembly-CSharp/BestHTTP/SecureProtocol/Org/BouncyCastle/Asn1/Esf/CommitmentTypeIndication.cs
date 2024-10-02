using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000743 RID: 1859
	public class CommitmentTypeIndication : Asn1Encodable
	{
		// Token: 0x0600432E RID: 17198 RVA: 0x001894E0 File Offset: 0x001876E0
		public static CommitmentTypeIndication GetInstance(object obj)
		{
			if (obj == null || obj is CommitmentTypeIndication)
			{
				return (CommitmentTypeIndication)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CommitmentTypeIndication((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CommitmentTypeIndication' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00189530 File Offset: 0x00187730
		public CommitmentTypeIndication(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 1 || seq.Count > 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.commitmentTypeId = (DerObjectIdentifier)seq[0].ToAsn1Object();
			if (seq.Count > 1)
			{
				this.commitmentTypeQualifier = (Asn1Sequence)seq[1].ToAsn1Object();
			}
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x001895BA File Offset: 0x001877BA
		public CommitmentTypeIndication(DerObjectIdentifier commitmentTypeId) : this(commitmentTypeId, null)
		{
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x001895C4 File Offset: 0x001877C4
		public CommitmentTypeIndication(DerObjectIdentifier commitmentTypeId, Asn1Sequence commitmentTypeQualifier)
		{
			if (commitmentTypeId == null)
			{
				throw new ArgumentNullException("commitmentTypeId");
			}
			this.commitmentTypeId = commitmentTypeId;
			if (commitmentTypeQualifier != null)
			{
				this.commitmentTypeQualifier = commitmentTypeQualifier;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06004332 RID: 17202 RVA: 0x001895EB File Offset: 0x001877EB
		public DerObjectIdentifier CommitmentTypeID
		{
			get
			{
				return this.commitmentTypeId;
			}
		}

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06004333 RID: 17203 RVA: 0x001895F3 File Offset: 0x001877F3
		public Asn1Sequence CommitmentTypeQualifier
		{
			get
			{
				return this.commitmentTypeQualifier;
			}
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x001895FC File Offset: 0x001877FC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.commitmentTypeId
			});
			if (this.commitmentTypeQualifier != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.commitmentTypeQualifier
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002C22 RID: 11298
		private readonly DerObjectIdentifier commitmentTypeId;

		// Token: 0x04002C23 RID: 11299
		private readonly Asn1Sequence commitmentTypeQualifier;
	}
}
