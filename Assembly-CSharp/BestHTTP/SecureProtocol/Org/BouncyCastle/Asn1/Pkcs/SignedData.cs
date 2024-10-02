using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Pkcs
{
	// Token: 0x02000703 RID: 1795
	public class SignedData : Asn1Encodable
	{
		// Token: 0x0600419F RID: 16799 RVA: 0x001839D4 File Offset: 0x00181BD4
		public static SignedData GetInstance(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			SignedData signedData = obj as SignedData;
			if (signedData != null)
			{
				return signedData;
			}
			return new SignedData(Asn1Sequence.GetInstance(obj));
		}

		// Token: 0x060041A0 RID: 16800 RVA: 0x001839FD File Offset: 0x00181BFD
		public SignedData(DerInteger _version, Asn1Set _digestAlgorithms, ContentInfo _contentInfo, Asn1Set _certificates, Asn1Set _crls, Asn1Set _signerInfos)
		{
			this.version = _version;
			this.digestAlgorithms = _digestAlgorithms;
			this.contentInfo = _contentInfo;
			this.certificates = _certificates;
			this.crls = _crls;
			this.signerInfos = _signerInfos;
		}

		// Token: 0x060041A1 RID: 16801 RVA: 0x00183A34 File Offset: 0x00181C34
		private SignedData(Asn1Sequence seq)
		{
			IEnumerator enumerator = seq.GetEnumerator();
			enumerator.MoveNext();
			this.version = (DerInteger)enumerator.Current;
			enumerator.MoveNext();
			this.digestAlgorithms = (Asn1Set)enumerator.Current;
			enumerator.MoveNext();
			this.contentInfo = ContentInfo.GetInstance(enumerator.Current);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Asn1Object asn1Object = (Asn1Object)obj;
				if (asn1Object is DerTaggedObject)
				{
					DerTaggedObject derTaggedObject = (DerTaggedObject)asn1Object;
					int tagNo = derTaggedObject.TagNo;
					if (tagNo != 0)
					{
						if (tagNo != 1)
						{
							throw new ArgumentException("unknown tag value " + derTaggedObject.TagNo);
						}
						this.crls = Asn1Set.GetInstance(derTaggedObject, false);
					}
					else
					{
						this.certificates = Asn1Set.GetInstance(derTaggedObject, false);
					}
				}
				else
				{
					this.signerInfos = (Asn1Set)asn1Object;
				}
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x060041A2 RID: 16802 RVA: 0x00183B10 File Offset: 0x00181D10
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x060041A3 RID: 16803 RVA: 0x00183B18 File Offset: 0x00181D18
		public Asn1Set DigestAlgorithms
		{
			get
			{
				return this.digestAlgorithms;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x060041A4 RID: 16804 RVA: 0x00183B20 File Offset: 0x00181D20
		public ContentInfo ContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x060041A5 RID: 16805 RVA: 0x00183B28 File Offset: 0x00181D28
		public Asn1Set Certificates
		{
			get
			{
				return this.certificates;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x060041A6 RID: 16806 RVA: 0x00183B30 File Offset: 0x00181D30
		public Asn1Set Crls
		{
			get
			{
				return this.crls;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x060041A7 RID: 16807 RVA: 0x00183B38 File Offset: 0x00181D38
		public Asn1Set SignerInfos
		{
			get
			{
				return this.signerInfos;
			}
		}

		// Token: 0x060041A8 RID: 16808 RVA: 0x00183B40 File Offset: 0x00181D40
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.digestAlgorithms,
				this.contentInfo
			});
			if (this.certificates != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.certificates)
				});
			}
			if (this.crls != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 1, this.crls)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.signerInfos
			});
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002A85 RID: 10885
		private readonly DerInteger version;

		// Token: 0x04002A86 RID: 10886
		private readonly Asn1Set digestAlgorithms;

		// Token: 0x04002A87 RID: 10887
		private readonly ContentInfo contentInfo;

		// Token: 0x04002A88 RID: 10888
		private readonly Asn1Set certificates;

		// Token: 0x04002A89 RID: 10889
		private readonly Asn1Set crls;

		// Token: 0x04002A8A RID: 10890
		private readonly Asn1Set signerInfos;
	}
}
