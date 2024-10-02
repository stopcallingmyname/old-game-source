using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms
{
	// Token: 0x02000789 RID: 1929
	public class EncryptedData : Asn1Encodable
	{
		// Token: 0x0600450D RID: 17677 RVA: 0x0018F898 File Offset: 0x0018DA98
		public static EncryptedData GetInstance(object obj)
		{
			if (obj is EncryptedData)
			{
				return (EncryptedData)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new EncryptedData((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid EncryptedData: " + Platform.GetTypeName(obj));
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x0018F8D2 File Offset: 0x0018DAD2
		public EncryptedData(EncryptedContentInfo encInfo) : this(encInfo, null)
		{
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x0018F8DC File Offset: 0x0018DADC
		public EncryptedData(EncryptedContentInfo encInfo, Asn1Set unprotectedAttrs)
		{
			if (encInfo == null)
			{
				throw new ArgumentNullException("encInfo");
			}
			this.version = new DerInteger((unprotectedAttrs == null) ? 0 : 2);
			this.encryptedContentInfo = encInfo;
			this.unprotectedAttrs = unprotectedAttrs;
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x0018F914 File Offset: 0x0018DB14
		private EncryptedData(Asn1Sequence seq)
		{
			if (seq == null)
			{
				throw new ArgumentNullException("seq");
			}
			if (seq.Count < 2 || seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count, "seq");
			}
			this.version = DerInteger.GetInstance(seq[0]);
			this.encryptedContentInfo = EncryptedContentInfo.GetInstance(seq[1]);
			if (seq.Count > 2)
			{
				this.unprotectedAttrs = Asn1Set.GetInstance((Asn1TaggedObject)seq[2], false);
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06004511 RID: 17681 RVA: 0x0018F9AC File Offset: 0x0018DBAC
		public virtual DerInteger Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06004512 RID: 17682 RVA: 0x0018F9B4 File Offset: 0x0018DBB4
		public virtual EncryptedContentInfo EncryptedContentInfo
		{
			get
			{
				return this.encryptedContentInfo;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06004513 RID: 17683 RVA: 0x0018F9BC File Offset: 0x0018DBBC
		public virtual Asn1Set UnprotectedAttrs
		{
			get
			{
				return this.unprotectedAttrs;
			}
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x0018F9C4 File Offset: 0x0018DBC4
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(new Asn1Encodable[]
			{
				this.version,
				this.encryptedContentInfo
			});
			if (this.unprotectedAttrs != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new BerTaggedObject(false, 1, this.unprotectedAttrs)
				});
			}
			return new BerSequence(asn1EncodableVector);
		}

		// Token: 0x04002D39 RID: 11577
		private readonly DerInteger version;

		// Token: 0x04002D3A RID: 11578
		private readonly EncryptedContentInfo encryptedContentInfo;

		// Token: 0x04002D3B RID: 11579
		private readonly Asn1Set unprotectedAttrs;
	}
}
