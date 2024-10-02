using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000403 RID: 1027
	public class CertificateRequest
	{
		// Token: 0x0600297E RID: 10622 RVA: 0x0010E620 File Offset: 0x0010C820
		public CertificateRequest(byte[] certificateTypes, IList supportedSignatureAlgorithms, IList certificateAuthorities)
		{
			this.mCertificateTypes = certificateTypes;
			this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
			this.mCertificateAuthorities = certificateAuthorities;
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600297F RID: 10623 RVA: 0x0010E63D File Offset: 0x0010C83D
		public virtual byte[] CertificateTypes
		{
			get
			{
				return this.mCertificateTypes;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002980 RID: 10624 RVA: 0x0010E645 File Offset: 0x0010C845
		public virtual IList SupportedSignatureAlgorithms
		{
			get
			{
				return this.mSupportedSignatureAlgorithms;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002981 RID: 10625 RVA: 0x0010E64D File Offset: 0x0010C84D
		public virtual IList CertificateAuthorities
		{
			get
			{
				return this.mCertificateAuthorities;
			}
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x0010E658 File Offset: 0x0010C858
		public virtual void Encode(Stream output)
		{
			if (this.mCertificateTypes == null || this.mCertificateTypes.Length == 0)
			{
				TlsUtilities.WriteUint8(0, output);
			}
			else
			{
				TlsUtilities.WriteUint8ArrayWithUint8Length(this.mCertificateTypes, output);
			}
			if (this.mSupportedSignatureAlgorithms != null)
			{
				TlsUtilities.EncodeSupportedSignatureAlgorithms(this.mSupportedSignatureAlgorithms, false, output);
			}
			if (this.mCertificateAuthorities == null || this.mCertificateAuthorities.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			IList list = Platform.CreateArrayList(this.mCertificateAuthorities.Count);
			int num = 0;
			foreach (object obj in this.mCertificateAuthorities)
			{
				byte[] encoded = ((Asn1Encodable)obj).GetEncoded("DER");
				list.Add(encoded);
				num += encoded.Length + 2;
			}
			TlsUtilities.CheckUint16(num);
			TlsUtilities.WriteUint16(num, output);
			foreach (object obj2 in list)
			{
				TlsUtilities.WriteOpaque16((byte[])obj2, output);
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x0010E784 File Offset: 0x0010C984
		public static CertificateRequest Parse(TlsContext context, Stream input)
		{
			int num = (int)TlsUtilities.ReadUint8(input);
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = TlsUtilities.ReadUint8(input);
			}
			IList supportedSignatureAlgorithms = null;
			if (TlsUtilities.IsTlsV12(context))
			{
				supportedSignatureAlgorithms = TlsUtilities.ParseSupportedSignatureAlgorithms(false, input);
			}
			IList list = Platform.CreateArrayList();
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadOpaque16(input), false);
			while (memoryStream.Position < memoryStream.Length)
			{
				Asn1Object obj = TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream));
				list.Add(X509Name.GetInstance(obj));
			}
			return new CertificateRequest(array, supportedSignatureAlgorithms, list);
		}

		// Token: 0x04001B69 RID: 7017
		protected readonly byte[] mCertificateTypes;

		// Token: 0x04001B6A RID: 7018
		protected readonly IList mSupportedSignatureAlgorithms;

		// Token: 0x04001B6B RID: 7019
		protected readonly IList mCertificateAuthorities;
	}
}
