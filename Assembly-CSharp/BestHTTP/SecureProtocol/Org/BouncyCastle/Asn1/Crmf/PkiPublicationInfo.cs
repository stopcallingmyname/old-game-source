using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000772 RID: 1906
	public class PkiPublicationInfo : Asn1Encodable
	{
		// Token: 0x0600446E RID: 17518 RVA: 0x0018DB08 File Offset: 0x0018BD08
		private PkiPublicationInfo(Asn1Sequence seq)
		{
			this.action = DerInteger.GetInstance(seq[0]);
			this.pubInfos = Asn1Sequence.GetInstance(seq[1]);
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0018DB34 File Offset: 0x0018BD34
		public static PkiPublicationInfo GetInstance(object obj)
		{
			if (obj is PkiPublicationInfo)
			{
				return (PkiPublicationInfo)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PkiPublicationInfo((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06004470 RID: 17520 RVA: 0x0018DB73 File Offset: 0x0018BD73
		public virtual DerInteger Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x06004471 RID: 17521 RVA: 0x0018DB7C File Offset: 0x0018BD7C
		public virtual SinglePubInfo[] GetPubInfos()
		{
			if (this.pubInfos == null)
			{
				return null;
			}
			SinglePubInfo[] array = new SinglePubInfo[this.pubInfos.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = SinglePubInfo.GetInstance(this.pubInfos[num]);
			}
			return array;
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x0018DBC7 File Offset: 0x0018BDC7
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.action,
				this.pubInfos
			});
		}

		// Token: 0x04002CE0 RID: 11488
		private readonly DerInteger action;

		// Token: 0x04002CE1 RID: 11489
		private readonly Asn1Sequence pubInfos;
	}
}
