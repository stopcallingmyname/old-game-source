using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000757 RID: 1879
	public class SignaturePolicyIdentifier : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x060043B4 RID: 17332 RVA: 0x0018B5CC File Offset: 0x001897CC
		public static SignaturePolicyIdentifier GetInstance(object obj)
		{
			if (obj == null || obj is SignaturePolicyIdentifier)
			{
				return (SignaturePolicyIdentifier)obj;
			}
			if (obj is SignaturePolicyId)
			{
				return new SignaturePolicyIdentifier((SignaturePolicyId)obj);
			}
			if (obj is Asn1Null)
			{
				return new SignaturePolicyIdentifier();
			}
			throw new ArgumentException("Unknown object in 'SignaturePolicyIdentifier' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0018B627 File Offset: 0x00189827
		public SignaturePolicyIdentifier()
		{
			this.sigPolicy = null;
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x0018B636 File Offset: 0x00189836
		public SignaturePolicyIdentifier(SignaturePolicyId signaturePolicyId)
		{
			if (signaturePolicyId == null)
			{
				throw new ArgumentNullException("signaturePolicyId");
			}
			this.sigPolicy = signaturePolicyId;
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x060043B7 RID: 17335 RVA: 0x0018B653 File Offset: 0x00189853
		public SignaturePolicyId SignaturePolicyId
		{
			get
			{
				return this.sigPolicy;
			}
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x0018B65B File Offset: 0x0018985B
		public override Asn1Object ToAsn1Object()
		{
			if (this.sigPolicy != null)
			{
				return this.sigPolicy.ToAsn1Object();
			}
			return DerNull.Instance;
		}

		// Token: 0x04002C56 RID: 11350
		private readonly SignaturePolicyId sigPolicy;
	}
}
