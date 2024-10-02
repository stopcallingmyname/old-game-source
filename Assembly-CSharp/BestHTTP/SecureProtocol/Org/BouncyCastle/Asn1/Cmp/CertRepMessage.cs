using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007AE RID: 1966
	public class CertRepMessage : Asn1Encodable
	{
		// Token: 0x06004634 RID: 17972 RVA: 0x00192A04 File Offset: 0x00190C04
		private CertRepMessage(Asn1Sequence seq)
		{
			int index = 0;
			if (seq.Count > 1)
			{
				this.caPubs = Asn1Sequence.GetInstance((Asn1TaggedObject)seq[index++], true);
			}
			this.response = Asn1Sequence.GetInstance(seq[index]);
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x00192A50 File Offset: 0x00190C50
		public static CertRepMessage GetInstance(object obj)
		{
			if (obj is CertRepMessage)
			{
				return (CertRepMessage)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertRepMessage((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00192A90 File Offset: 0x00190C90
		public CertRepMessage(CmpCertificate[] caPubs, CertResponse[] response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			if (caPubs != null)
			{
				this.caPubs = new DerSequence(caPubs);
			}
			this.response = new DerSequence(response);
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00192AD0 File Offset: 0x00190CD0
		public virtual CmpCertificate[] GetCAPubs()
		{
			if (this.caPubs == null)
			{
				return null;
			}
			CmpCertificate[] array = new CmpCertificate[this.caPubs.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CmpCertificate.GetInstance(this.caPubs[num]);
			}
			return array;
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00192B1C File Offset: 0x00190D1C
		public virtual CertResponse[] GetResponse()
		{
			CertResponse[] array = new CertResponse[this.response.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertResponse.GetInstance(this.response[num]);
			}
			return array;
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00192B60 File Offset: 0x00190D60
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.caPubs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.caPubs)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.response
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002DA9 RID: 11689
		private readonly Asn1Sequence caPubs;

		// Token: 0x04002DAA RID: 11690
		private readonly Asn1Sequence response;
	}
}
