using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp
{
	// Token: 0x02000717 RID: 1815
	public class TbsRequest : Asn1Encodable
	{
		// Token: 0x06004236 RID: 16950 RVA: 0x001852DE File Offset: 0x001834DE
		public static TbsRequest GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return TbsRequest.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x06004237 RID: 16951 RVA: 0x001852EC File Offset: 0x001834EC
		public static TbsRequest GetInstance(object obj)
		{
			if (obj == null || obj is TbsRequest)
			{
				return (TbsRequest)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new TbsRequest((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004238 RID: 16952 RVA: 0x00185339 File Offset: 0x00183539
		public TbsRequest(GeneralName requestorName, Asn1Sequence requestList, X509Extensions requestExtensions)
		{
			this.version = TbsRequest.V1;
			this.requestorName = requestorName;
			this.requestList = requestList;
			this.requestExtensions = requestExtensions;
		}

		// Token: 0x06004239 RID: 16953 RVA: 0x00185364 File Offset: 0x00183564
		private TbsRequest(Asn1Sequence seq)
		{
			int num = 0;
			Asn1Encodable asn1Encodable = seq[0];
			if (asn1Encodable is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
				if (asn1TaggedObject.TagNo == 0)
				{
					this.versionSet = true;
					this.version = DerInteger.GetInstance(asn1TaggedObject, true);
					num++;
				}
				else
				{
					this.version = TbsRequest.V1;
				}
			}
			else
			{
				this.version = TbsRequest.V1;
			}
			if (seq[num] is Asn1TaggedObject)
			{
				this.requestorName = GeneralName.GetInstance((Asn1TaggedObject)seq[num++], true);
			}
			this.requestList = (Asn1Sequence)seq[num++];
			if (seq.Count == num + 1)
			{
				this.requestExtensions = X509Extensions.GetInstance((Asn1TaggedObject)seq[num], true);
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x0600423A RID: 16954 RVA: 0x0018542D File Offset: 0x0018362D
		public DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x0600423B RID: 16955 RVA: 0x00185435 File Offset: 0x00183635
		public GeneralName RequestorName
		{
			get
			{
				return this.requestorName;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x0600423C RID: 16956 RVA: 0x0018543D File Offset: 0x0018363D
		public Asn1Sequence RequestList
		{
			get
			{
				return this.requestList;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x0600423D RID: 16957 RVA: 0x00185445 File Offset: 0x00183645
		public X509Extensions RequestExtensions
		{
			get
			{
				return this.requestExtensions;
			}
		}

		// Token: 0x0600423E RID: 16958 RVA: 0x00185450 File Offset: 0x00183650
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (!this.version.Equals(TbsRequest.V1) || this.versionSet)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 0, this.version)
				});
			}
			if (this.requestorName != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 1, this.requestorName)
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.requestList
			});
			if (this.requestExtensions != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(true, 2, this.requestExtensions)
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002AD8 RID: 10968
		private static readonly DerInteger V1 = new DerInteger(0);

		// Token: 0x04002AD9 RID: 10969
		private readonly DerInteger version;

		// Token: 0x04002ADA RID: 10970
		private readonly GeneralName requestorName;

		// Token: 0x04002ADB RID: 10971
		private readonly Asn1Sequence requestList;

		// Token: 0x04002ADC RID: 10972
		private readonly X509Extensions requestExtensions;

		// Token: 0x04002ADD RID: 10973
		private bool versionSet;
	}
}
