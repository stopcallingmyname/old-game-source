using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000753 RID: 1875
	public class OtherRevVals : Asn1Encodable
	{
		// Token: 0x06004394 RID: 17300 RVA: 0x0018ABA8 File Offset: 0x00188DA8
		public static OtherRevVals GetInstance(object obj)
		{
			if (obj == null || obj is OtherRevVals)
			{
				return (OtherRevVals)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OtherRevVals((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OtherRevVals' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0018ABF8 File Offset: 0x00188DF8
		private OtherRevVals(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.otherRevValType = (DerObjectIdentifier)seq[0].ToAsn1Object();
			this.otherRevVals = seq[1].ToAsn1Object();
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0018AC6B File Offset: 0x00188E6B
		public OtherRevVals(DerObjectIdentifier otherRevValType, Asn1Encodable otherRevVals)
		{
			if (otherRevValType == null)
			{
				throw new ArgumentNullException("otherRevValType");
			}
			if (otherRevVals == null)
			{
				throw new ArgumentNullException("otherRevVals");
			}
			this.otherRevValType = otherRevValType;
			this.otherRevVals = otherRevVals.ToAsn1Object();
		}

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06004397 RID: 17303 RVA: 0x0018ACA2 File Offset: 0x00188EA2
		public DerObjectIdentifier OtherRevValType
		{
			get
			{
				return this.otherRevValType;
			}
		}

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06004398 RID: 17304 RVA: 0x0018ACAA File Offset: 0x00188EAA
		public Asn1Object OtherRevValsObject
		{
			get
			{
				return this.otherRevVals;
			}
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0018ACB2 File Offset: 0x00188EB2
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.otherRevValType,
				this.otherRevVals
			});
		}

		// Token: 0x04002C4C RID: 11340
		private readonly DerObjectIdentifier otherRevValType;

		// Token: 0x04002C4D RID: 11341
		private readonly Asn1Object otherRevVals;
	}
}
