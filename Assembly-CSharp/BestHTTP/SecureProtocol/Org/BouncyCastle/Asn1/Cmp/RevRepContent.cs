using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CE RID: 1998
	public class RevRepContent : Asn1Encodable
	{
		// Token: 0x06004714 RID: 18196 RVA: 0x00194E6C File Offset: 0x0019306C
		private RevRepContent(Asn1Sequence seq)
		{
			this.status = Asn1Sequence.GetInstance(seq[0]);
			for (int i = 1; i < seq.Count; i++)
			{
				Asn1TaggedObject instance = Asn1TaggedObject.GetInstance(seq[i]);
				if (instance.TagNo == 0)
				{
					this.revCerts = Asn1Sequence.GetInstance(instance, true);
				}
				else
				{
					this.crls = Asn1Sequence.GetInstance(instance, true);
				}
			}
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x00194ED3 File Offset: 0x001930D3
		public static RevRepContent GetInstance(object obj)
		{
			if (obj is RevRepContent)
			{
				return (RevRepContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new RevRepContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x00194F14 File Offset: 0x00193114
		public virtual PkiStatusInfo[] GetStatus()
		{
			PkiStatusInfo[] array = new PkiStatusInfo[this.status.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = PkiStatusInfo.GetInstance(this.status[num]);
			}
			return array;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00194F58 File Offset: 0x00193158
		public virtual CertId[] GetRevCerts()
		{
			if (this.revCerts == null)
			{
				return null;
			}
			CertId[] array = new CertId[this.revCerts.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertId.GetInstance(this.revCerts[num]);
			}
			return array;
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x00194FA4 File Offset: 0x001931A4
		public virtual CertificateList[] GetCrls()
		{
			if (this.crls == null)
			{
				return null;
			}
			CertificateList[] array = new CertificateList[this.crls.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertificateList.GetInstance(this.crls[num]);
			}
			return array;
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x00194FF0 File Offset: 0x001931F0
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector v = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.status
			});
			this.AddOptional(v, 0, this.revCerts);
			this.AddOptional(v, 1, this.crls);
			return new DerSequence(v);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0019382E File Offset: 0x00191A2E
		private void AddOptional(Asn1EncodableVector v, int tagNo, Asn1Encodable obj)
		{
			if (obj != null)
			{
				v.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, tagNo, obj)
				});
			}
		}

		// Token: 0x04002E5F RID: 11871
		private readonly Asn1Sequence status;

		// Token: 0x04002E60 RID: 11872
		private readonly Asn1Sequence revCerts;

		// Token: 0x04002E61 RID: 11873
		private readonly Asn1Sequence crls;
	}
}
