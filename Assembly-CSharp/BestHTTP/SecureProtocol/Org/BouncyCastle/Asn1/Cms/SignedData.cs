using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x020007A0 RID: 1952
	public class SignedData : Asn1Encodable
	{
		// Token: 0x060045CA RID: 17866 RVA: 0x001914EB File Offset: 0x0018F6EB
		public static SignedData GetInstance(object obj)
		{
			if (obj is SignedData)
			{
				return (SignedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new SignedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0019152C File Offset: 0x0018F72C
		public SignedData(Asn1Set digestAlgorithms, ContentInfo contentInfo, Asn1Set certificates, Asn1Set crls, Asn1Set signerInfos)
		{
			this.version = this.CalculateVersion(contentInfo.ContentType, certificates, crls, signerInfos);
			this.digestAlgorithms = digestAlgorithms;
			this.contentInfo = contentInfo;
			this.certificates = certificates;
			this.crls = crls;
			this.signerInfos = signerInfos;
			this.crlsBer = (crls is BerSet);
			this.certsBer = (certificates is BerSet);
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x0019159C File Offset: 0x0018F79C
		private DerInteger CalculateVersion(DerObjectIdentifier contentOid, Asn1Set certs, Asn1Set crls, Asn1Set signerInfs)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			if (certs != null)
			{
				foreach (object obj in certs)
				{
					if (obj is Asn1TaggedObject)
					{
						Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)obj;
						if (asn1TaggedObject.TagNo == 1)
						{
							flag3 = true;
						}
						else if (asn1TaggedObject.TagNo == 2)
						{
							flag4 = true;
						}
						else if (asn1TaggedObject.TagNo == 3)
						{
							flag = true;
							break;
						}
					}
				}
			}
			if (flag)
			{
				return SignedData.Version5;
			}
			if (crls != null)
			{
				using (IEnumerator enumerator = crls.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current is Asn1TaggedObject)
						{
							flag2 = true;
							break;
						}
					}
				}
			}
			if (flag2)
			{
				return SignedData.Version5;
			}
			if (flag4)
			{
				return SignedData.Version4;
			}
			if (flag3 || !CmsObjectIdentifiers.Data.Equals(contentOid) || this.CheckForVersion3(signerInfs))
			{
				return SignedData.Version3;
			}
			return SignedData.Version1;
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x001916C0 File Offset: 0x0018F8C0
		private bool CheckForVersion3(Asn1Set signerInfs)
		{
			using (IEnumerator enumerator = signerInfs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (SignerInfo.GetInstance(enumerator.Current).Version.Value.IntValue == 3)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060045CE RID: 17870 RVA: 0x00191728 File Offset: 0x0018F928
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
				if (asn1Object is Asn1TaggedObject)
				{
					Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Object;
					int tagNo = asn1TaggedObject.TagNo;
					if (tagNo != 0)
					{
						if (tagNo != 1)
						{
							throw new ArgumentException("unknown tag value " + asn1TaggedObject.TagNo);
						}
						this.crlsBer = (asn1TaggedObject is BerTaggedObject);
						this.crls = Asn1Set.GetInstance(asn1TaggedObject, false);
					}
					else
					{
						this.certsBer = (asn1TaggedObject is BerTaggedObject);
						this.certificates = Asn1Set.GetInstance(asn1TaggedObject, false);
					}
				}
				else
				{
					this.signerInfos = (Asn1Set)asn1Object;
				}
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x060045CF RID: 17871 RVA: 0x00191828 File Offset: 0x0018FA28
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x00191830 File Offset: 0x0018FA30
		public Asn1Set DigestAlgorithms
		{
			get
			{
				return this.digestAlgorithms;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x060045D1 RID: 17873 RVA: 0x00191838 File Offset: 0x0018FA38
		public ContentInfo EncapContentInfo
		{
			get
			{
				return this.contentInfo;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x00191840 File Offset: 0x0018FA40
		public Asn1Set Certificates
		{
			get
			{
				return this.certificates;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x060045D3 RID: 17875 RVA: 0x00191848 File Offset: 0x0018FA48
		public Asn1Set CRLs
		{
			get
			{
				return this.crls;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x060045D4 RID: 17876 RVA: 0x00191850 File Offset: 0x0018FA50
		public Asn1Set SignerInfos
		{
			get
			{
				return this.signerInfos;
			}
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x00191858 File Offset: 0x0018FA58
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
				if (this.certsBer)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new BerTaggedObject(false, 0, this.certificates)
					});
				}
				else
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(false, 0, this.certificates)
					});
				}
			}
			if (this.crls != null)
			{
				if (this.crlsBer)
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new BerTaggedObject(false, 1, this.crls)
					});
				}
				else
				{
					asn1EncodableVector.Add(new Asn1Encodable[]
					{
						new DerTaggedObject(false, 1, this.crls)
					});
				}
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.signerInfos
			});
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D76 RID: 11638
		private static readonly DerInteger Version1 = new DerInteger(1);

		// Token: 0x04002D77 RID: 11639
		private static readonly DerInteger Version3 = new DerInteger(3);

		// Token: 0x04002D78 RID: 11640
		private static readonly DerInteger Version4 = new DerInteger(4);

		// Token: 0x04002D79 RID: 11641
		private static readonly DerInteger Version5 = new DerInteger(5);

		// Token: 0x04002D7A RID: 11642
		private readonly DerInteger version;

		// Token: 0x04002D7B RID: 11643
		private readonly Asn1Set digestAlgorithms;

		// Token: 0x04002D7C RID: 11644
		private readonly ContentInfo contentInfo;

		// Token: 0x04002D7D RID: 11645
		private readonly Asn1Set certificates;

		// Token: 0x04002D7E RID: 11646
		private readonly Asn1Set crls;

		// Token: 0x04002D7F RID: 11647
		private readonly Asn1Set signerInfos;

		// Token: 0x04002D80 RID: 11648
		private readonly bool certsBer;

		// Token: 0x04002D81 RID: 11649
		private readonly bool crlsBer;
	}
}
