using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x02000745 RID: 1861
	public class CompleteCertificateRefs : Asn1Encodable
	{
		// Token: 0x0600433C RID: 17212 RVA: 0x001897A8 File Offset: 0x001879A8
		public static CompleteCertificateRefs GetInstance(object obj)
		{
			if (obj == null || obj is CompleteCertificateRefs)
			{
				return (CompleteCertificateRefs)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CompleteCertificateRefs((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'CompleteCertificateRefs' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x001897F8 File Offset: 0x001879F8
		private CompleteCertificateRefs(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			foreach (object obj in seq)
			{
				OtherCertID.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
			}
			this.otherCertIDs = seq;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x0018986C File Offset: 0x00187A6C
		public CompleteCertificateRefs(params OtherCertID[] otherCertIDs)
		{
			if (otherCertIDs == null)
			{
				throw new ArgumentNullException("otherCertIDs");
			}
			this.otherCertIDs = new DerSequence(otherCertIDs);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x0018989C File Offset: 0x00187A9C
		public CompleteCertificateRefs(IEnumerable otherCertIDs)
		{
			if (otherCertIDs == null)
			{
				throw new ArgumentNullException("otherCertIDs");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(otherCertIDs, typeof(OtherCertID)))
			{
				throw new ArgumentException("Must contain only 'OtherCertID' objects", "otherCertIDs");
			}
			this.otherCertIDs = new DerSequence(Asn1EncodableVector.FromEnumerable(otherCertIDs));
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x001898F0 File Offset: 0x00187AF0
		public OtherCertID[] GetOtherCertIDs()
		{
			OtherCertID[] array = new OtherCertID[this.otherCertIDs.Count];
			for (int i = 0; i < this.otherCertIDs.Count; i++)
			{
				array[i] = OtherCertID.GetInstance(this.otherCertIDs[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0018993E File Offset: 0x00187B3E
		public override Asn1Object ToAsn1Object()
		{
			return this.otherCertIDs;
		}

		// Token: 0x04002C26 RID: 11302
		private readonly Asn1Sequence otherCertIDs;
	}
}
