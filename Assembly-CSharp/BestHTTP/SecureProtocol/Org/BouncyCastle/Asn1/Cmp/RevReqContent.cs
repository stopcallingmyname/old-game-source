using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007D0 RID: 2000
	public class RevReqContent : Asn1Encodable
	{
		// Token: 0x06004720 RID: 18208 RVA: 0x00195191 File Offset: 0x00193391
		private RevReqContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x001951A0 File Offset: 0x001933A0
		public static RevReqContent GetInstance(object obj)
		{
			if (obj is RevReqContent)
			{
				return (RevReqContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevReqContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x001951E0 File Offset: 0x001933E0
		public RevReqContent(params RevDetails[] revDetails)
		{
			this.content = new DerSequence(revDetails);
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x00195204 File Offset: 0x00193404
		public virtual RevDetails[] ToRevDetailsArray()
		{
			RevDetails[] array = new RevDetails[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = RevDetails.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x00195245 File Offset: 0x00193445
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002E65 RID: 11877
		private readonly Asn1Sequence content;
	}
}
