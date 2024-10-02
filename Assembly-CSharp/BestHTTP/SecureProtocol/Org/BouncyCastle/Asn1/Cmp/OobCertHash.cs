using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007BA RID: 1978
	public class OobCertHash : Asn1Encodable
	{
		// Token: 0x0600467C RID: 18044 RVA: 0x0019384C File Offset: 0x00191A4C
		private OobCertHash(Asn1Sequence seq)
		{
			int num = seq.Count - 1;
			this.hashVal = DerBitString.GetInstance(seq[num--]);
			for (int i = num; i >= 0; i--)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)seq[i];
				if (asn1TaggedObject.TagNo == 0)
				{
					this.hashAlg = AlgorithmIdentifier.GetInstance(asn1TaggedObject, true);
				}
				else
				{
					this.certId = CertId.GetInstance(asn1TaggedObject, true);
				}
			}
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x001938BB File Offset: 0x00191ABB
		public static OobCertHash GetInstance(object obj)
		{
			if (obj is OobCertHash)
			{
				return (OobCertHash)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new OobCertHash((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x0600467E RID: 18046 RVA: 0x001938FA File Offset: 0x00191AFA
		public virtual AlgorithmIdentifier HashAlg
		{
			get
			{
				return this.hashAlg;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x00193902 File Offset: 0x00191B02
		public virtual CertId CertID
		{
			get
			{
				return this.certId;
			}
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x0019390C File Offset: 0x00191B0C
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			this.AddOptional(asn1EncodableVector, 0, this.hashAlg);
			this.AddOptional(asn1EncodableVector, 1, this.certId);
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.hashVal
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x0019382E File Offset: 0x00191A2E
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

		// Token: 0x04002DDD RID: 11741
		private readonly AlgorithmIdentifier hashAlg;

		// Token: 0x04002DDE RID: 11742
		private readonly CertId certId;

		// Token: 0x04002DDF RID: 11743
		private readonly DerBitString hashVal;
	}
}
