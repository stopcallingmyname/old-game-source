using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000748 RID: 1864
	public class CrlListID : Asn1Encodable
	{
		// Token: 0x06004350 RID: 17232 RVA: 0x00189C90 File Offset: 0x00187E90
		public static CrlListID GetInstance(object obj)
		{
			if (obj == null || obj is CrlListID)
			{
				return (CrlListID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CrlListID((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CrlListID' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x00189CE0 File Offset: 0x00187EE0
		private CrlListID(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 1)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.crls = (Asn1Sequence)seq[0].ToAsn1Object();
			foreach (object obj in this.crls)
			{
				CrlValidatedID.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x00189D90 File Offset: 0x00187F90
		public CrlListID(params CrlValidatedID[] crls)
		{
			if (crls == null)
			{
				throw new ArgumentNullException("crls");
			}
			this.crls = new DerSequence(crls);
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00189DC0 File Offset: 0x00187FC0
		public CrlListID(IEnumerable crls)
		{
			if (crls == null)
			{
				throw new ArgumentNullException("crls");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(crls, typeof(CrlValidatedID)))
			{
				throw new ArgumentException("Must contain only 'CrlValidatedID' objects", "crls");
			}
			this.crls = new DerSequence(Asn1EncodableVector.FromEnumerable(crls));
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x00189E14 File Offset: 0x00188014
		public CrlValidatedID[] GetCrls()
		{
			CrlValidatedID[] array = new CrlValidatedID[this.crls.Count];
			for (int i = 0; i < this.crls.Count; i++)
			{
				array[i] = CrlValidatedID.GetInstance(this.crls[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00189E62 File Offset: 0x00188062
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(this.crls);
		}

		// Token: 0x04002C2B RID: 11307
		private readonly Asn1Sequence crls;
	}
}
