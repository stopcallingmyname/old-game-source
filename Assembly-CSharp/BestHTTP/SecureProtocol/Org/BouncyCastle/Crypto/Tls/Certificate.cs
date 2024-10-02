using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000402 RID: 1026
	public class Certificate
	{
		// Token: 0x06002975 RID: 10613 RVA: 0x0010E476 File Offset: 0x0010C676
		public Certificate(X509CertificateStructure[] certificateList)
		{
			if (certificateList == null)
			{
				throw new ArgumentNullException("certificateList");
			}
			this.mCertificateList = certificateList;
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x0010E493 File Offset: 0x0010C693
		public virtual X509CertificateStructure[] GetCertificateList()
		{
			return this.CloneCertificateList();
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x0010E49B File Offset: 0x0010C69B
		public virtual X509CertificateStructure GetCertificateAt(int index)
		{
			return this.mCertificateList[index];
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06002978 RID: 10616 RVA: 0x0010E4A5 File Offset: 0x0010C6A5
		public virtual int Length
		{
			get
			{
				return this.mCertificateList.Length;
			}
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x0010E4AF File Offset: 0x0010C6AF
		public virtual bool IsEmpty
		{
			get
			{
				return this.mCertificateList.Length == 0;
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x0010E4BC File Offset: 0x0010C6BC
		public virtual void Encode(Stream output)
		{
			IList list = Platform.CreateArrayList(this.mCertificateList.Length);
			int num = 0;
			X509CertificateStructure[] array = this.mCertificateList;
			for (int i = 0; i < array.Length; i++)
			{
				byte[] encoded = array[i].GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 3;
			}
			TlsUtilities.CheckUint24(num);
			TlsUtilities.WriteUint24(num, output);
			foreach (object obj in list)
			{
				TlsUtilities.WriteOpaque24((byte[])obj, output);
			}
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x0010E568 File Offset: 0x0010C768
		public static Certificate Parse(Stream input)
		{
			int num = TlsUtilities.ReadUint24(input);
			if (num == 0)
			{
				return Certificate.EmptyChain;
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				Asn1Object obj = TlsUtilities.ReadAsn1Object(TlsUtilities.ReadOpaque24(memoryStream));
				list.Add(X509CertificateStructure.GetInstance(obj));
			}
			X509CertificateStructure[] array = new X509CertificateStructure[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				array[i] = (X509CertificateStructure)list[i];
			}
			return new Certificate(array);
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x0010E5FC File Offset: 0x0010C7FC
		protected virtual X509CertificateStructure[] CloneCertificateList()
		{
			return (X509CertificateStructure[])this.mCertificateList.Clone();
		}

		// Token: 0x04001B67 RID: 7015
		public static readonly Certificate EmptyChain = new Certificate(new X509CertificateStructure[0]);

		// Token: 0x04001B68 RID: 7016
		protected readonly X509CertificateStructure[] mCertificateList;
	}
}
