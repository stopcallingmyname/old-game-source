using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Esf
{
	// Token: 0x0200074D RID: 1869
	public class OcspListID : Asn1Encodable
	{
		// Token: 0x0600436C RID: 17260 RVA: 0x0018A378 File Offset: 0x00188578
		public static OcspListID GetInstance(object obj)
		{
			if (obj == null || obj is OcspListID)
			{
				return (OcspListID)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OcspListID((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in 'OcspListID' factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x0018A3C8 File Offset: 0x001885C8
		private OcspListID(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count != 1)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.ocspResponses = (Asn1Sequence)seq[0].ToAsn1Object();
			foreach (object obj in this.ocspResponses)
			{
				OcspResponsesID.GetInstance(((Asn1Encodable)obj).ToAsn1Object());
			}
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x0018A478 File Offset: 0x00188678
		public OcspListID(params OcspResponsesID[] ocspResponses)
		{
			if (ocspResponses == null)
			{
				throw new ArgumentNullException("ocspResponses");
			}
			this.ocspResponses = new DerSequence(ocspResponses);
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x0018A4A8 File Offset: 0x001886A8
		public OcspListID(IEnumerable ocspResponses)
		{
			if (ocspResponses == null)
			{
				throw new ArgumentNullException("ocspResponses");
			}
			if (!CollectionUtilities.CheckElementsAreOfType(ocspResponses, typeof(OcspResponsesID)))
			{
				throw new ArgumentException("Must contain only 'OcspResponsesID' objects", "ocspResponses");
			}
			this.ocspResponses = new DerSequence(Asn1EncodableVector.FromEnumerable(ocspResponses));
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x0018A4FC File Offset: 0x001886FC
		public OcspResponsesID[] GetOcspResponses()
		{
			OcspResponsesID[] array = new OcspResponsesID[this.ocspResponses.Count];
			for (int i = 0; i < this.ocspResponses.Count; i++)
			{
				array[i] = OcspResponsesID.GetInstance(this.ocspResponses[i].ToAsn1Object());
			}
			return array;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x0018A54A File Offset: 0x0018874A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(this.ocspResponses);
		}

		// Token: 0x04002C41 RID: 11329
		private readonly Asn1Sequence ocspResponses;
	}
}
