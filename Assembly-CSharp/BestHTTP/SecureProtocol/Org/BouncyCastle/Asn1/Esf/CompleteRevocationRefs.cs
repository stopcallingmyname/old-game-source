using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000746 RID: 1862
	public class CompleteRevocationRefs : Asn1Encodable
	{
		// Token: 0x06004342 RID: 17218 RVA: 0x00189948 File Offset: 0x00187B48
		public static CompleteRevocationRefs GetInstance(object obj)
		{
			if (obj == null || obj is CompleteRevocationRefs)
			{
				return (CompleteRevocationRefs)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CompleteRevocationRefs((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CompleteRevocationRefs' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x00189998 File Offset: 0x00187B98
		private CompleteRevocationRefs(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			foreach (object obj in seq)
			{
				CrlOcspRef.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
			}
			this.crlOcspRefs = seq;
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00189A0C File Offset: 0x00187C0C
		public CompleteRevocationRefs(params CrlOcspRef[] crlOcspRefs)
		{
			if (crlOcspRefs == null)
			{
				throw new ArgumentNullException("crlOcspRefs");
			}
			this.crlOcspRefs = new DerSequence(crlOcspRefs);
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00189A3C File Offset: 0x00187C3C
		public CompleteRevocationRefs(IEnumerable crlOcspRefs)
		{
			if (crlOcspRefs == null)
			{
				throw new ArgumentNullException("crlOcspRefs");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(crlOcspRefs, typeof(CrlOcspRef)))
			{
				throw new ArgumentException("Must contain only 'CrlOcspRef' objects", "crlOcspRefs");
			}
			this.crlOcspRefs = new DerSequence(Asn1EncodableVector.FromEnumerable(crlOcspRefs));
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00189A90 File Offset: 0x00187C90
		public CrlOcspRef[] GetCrlOcspRefs()
		{
			CrlOcspRef[] array = new CrlOcspRef[this.crlOcspRefs.Count];
			for (int i = 0; i < this.crlOcspRefs.Count; i++)
			{
				array[i] = CrlOcspRef.GetInstance(this.crlOcspRefs[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00189ADE File Offset: 0x00187CDE
		public override Asn1Object ToAsn1Object()
		{
			return this.crlOcspRefs;
		}

		// Token: 0x04002C27 RID: 11303
		private readonly Asn1Sequence crlOcspRefs;
	}
}
