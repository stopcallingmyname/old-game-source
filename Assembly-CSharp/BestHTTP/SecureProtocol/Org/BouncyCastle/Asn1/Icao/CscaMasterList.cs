using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Icao
{
	// Token: 0x02000730 RID: 1840
	public class CscaMasterList : Asn1Encodable
	{
		// Token: 0x060042BD RID: 17085 RVA: 0x001878AC File Offset: 0x00185AAC
		public static CscaMasterList GetInstance(object obj)
		{
			if (obj is CscaMasterList)
			{
				return (CscaMasterList)obj;
			}
			if (obj != null)
			{
				return new CscaMasterList(Asn1Sequence.GetInstance(obj));
			}
			return null;
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x001878D0 File Offset: 0x00185AD0
		private CscaMasterList(Asn1Sequence seq)
		{
			if (seq == null || seq.Count == 0)
			{
				throw new ArgumentException("null or empty sequence passed.");
			}
			if (seq.Count != 2)
			{
				throw new ArgumentException("Incorrect sequence size: " + seq.Count);
			}
			this.version = DerInteger.GetInstance(seq[0]);
			Asn1Set instance = Asn1Set.GetInstance(seq[1]);
			this.certList = new X509CertificateStructure[instance.Count];
			for (int i = 0; i < this.certList.Length; i++)
			{
				this.certList[i] = X509CertificateStructure.GetInstance(instance[i]);
			}
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00187980 File Offset: 0x00185B80
		public CscaMasterList(X509CertificateStructure[] certStructs)
		{
			this.certList = CscaMasterList.CopyCertList(certStructs);
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x060042C0 RID: 17088 RVA: 0x001879A0 File Offset: 0x00185BA0
		public virtual int Version
		{
			get
			{
				return this.version.Value.IntValue;
			}
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x001879B2 File Offset: 0x00185BB2
		public X509CertificateStructure[] GetCertStructs()
		{
			return CscaMasterList.CopyCertList(this.certList);
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x001879BF File Offset: 0x00185BBF
		private static X509CertificateStructure[] CopyCertList(X509CertificateStructure[] orig)
		{
			return (X509CertificateStructure[])orig.Clone();
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x001879CC File Offset: 0x00185BCC
		public override Asn1Object ToAsn1Object()
		{
			Asn1Encodable[] array = new Asn1Encodable[2];
			array[0] = this.version;
			int num = 1;
			Asn1Encodable[] v = this.certList;
			array[num] = new DerSet(v);
			return new DerSequence(array);
		}

		// Token: 0x04002B93 RID: 11155
		private DerInteger version = new DerInteger(0);

		// Token: 0x04002B94 RID: 11156
		private X509CertificateStructure[] certList;
	}
}
