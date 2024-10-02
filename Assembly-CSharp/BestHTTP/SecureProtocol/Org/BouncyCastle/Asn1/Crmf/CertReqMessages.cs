using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000766 RID: 1894
	public class CertReqMessages : Asn1Encodable
	{
		// Token: 0x06004410 RID: 17424 RVA: 0x0018CD0C File Offset: 0x0018AF0C
		private CertReqMessages(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x0018CD1B File Offset: 0x0018AF1B
		public static CertReqMessages GetInstance(object obj)
		{
			if (obj is CertReqMessages)
			{
				return (CertReqMessages)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new CertReqMessages((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x0018CD5C File Offset: 0x0018AF5C
		public CertReqMessages(params CertReqMsg[] msgs)
		{
			this.content = new DerSequence(msgs);
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0018CD80 File Offset: 0x0018AF80
		public virtual CertReqMsg[] ToCertReqMsgArray()
		{
			CertReqMsg[] array = new CertReqMsg[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = CertReqMsg.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x0018CDC1 File Offset: 0x0018AFC1
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002CAB RID: 11435
		private readonly Asn1Sequence content;
	}
}
