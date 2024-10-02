using System;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Pkix
{
	// Token: 0x020002C5 RID: 709
	public class TrustAnchor
	{
		// Token: 0x06001A18 RID: 6680 RVA: 0x000C3DDC File Offset: 0x000C1FDC
		public TrustAnchor(X509Certificate trustedCert, byte[] nameConstraints)
		{
			if (trustedCert == null)
			{
				throw new ArgumentNullException("trustedCert");
			}
			this.trustedCert = trustedCert;
			this.pubKey = null;
			this.caName = null;
			this.caPrincipal = null;
			this.setNameConstraints(nameConstraints);
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x000C3E18 File Offset: 0x000C2018
		public TrustAnchor(X509Name caPrincipal, AsymmetricKeyParameter pubKey, byte[] nameConstraints)
		{
			if (caPrincipal == null)
			{
				throw new ArgumentNullException("caPrincipal");
			}
			if (pubKey == null)
			{
				throw new ArgumentNullException("pubKey");
			}
			this.trustedCert = null;
			this.caPrincipal = caPrincipal;
			this.caName = caPrincipal.ToString();
			this.pubKey = pubKey;
			this.setNameConstraints(nameConstraints);
		}

		// Token: 0x06001A1A RID: 6682 RVA: 0x000C3E70 File Offset: 0x000C2070
		public TrustAnchor(string caName, AsymmetricKeyParameter pubKey, byte[] nameConstraints)
		{
			if (caName == null)
			{
				throw new ArgumentNullException("caName");
			}
			if (pubKey == null)
			{
				throw new ArgumentNullException("pubKey");
			}
			if (caName.Length == 0)
			{
				throw new ArgumentException("caName can not be an empty string");
			}
			this.caPrincipal = new X509Name(caName);
			this.pubKey = pubKey;
			this.caName = caName;
			this.trustedCert = null;
			this.setNameConstraints(nameConstraints);
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x000C3EDA File Offset: 0x000C20DA
		public X509Certificate TrustedCert
		{
			get
			{
				return this.trustedCert;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x000C3EE2 File Offset: 0x000C20E2
		public X509Name CA
		{
			get
			{
				return this.caPrincipal;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x000C3EEA File Offset: 0x000C20EA
		public string CAName
		{
			get
			{
				return this.caName;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001A1E RID: 6686 RVA: 0x000C3EF2 File Offset: 0x000C20F2
		public AsymmetricKeyParameter CAPublicKey
		{
			get
			{
				return this.pubKey;
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x000C3EFA File Offset: 0x000C20FA
		private void setNameConstraints(byte[] bytes)
		{
			if (bytes == null)
			{
				this.ncBytes = null;
				this.nc = null;
				return;
			}
			this.ncBytes = (byte[])bytes.Clone();
			this.nc = NameConstraints.GetInstance(Asn1Object.FromByteArray(bytes));
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001A20 RID: 6688 RVA: 0x000C3F30 File Offset: 0x000C2130
		public byte[] GetNameConstraints
		{
			get
			{
				return Arrays.Clone(this.ncBytes);
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x000C3F40 File Offset: 0x000C2140
		public override string ToString()
		{
			string newLine = Platform.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[");
			stringBuilder.Append(newLine);
			if (this.pubKey != null)
			{
				stringBuilder.Append("  Trusted CA Public Key: ").Append(this.pubKey).Append(newLine);
				stringBuilder.Append("  Trusted CA Issuer Name: ").Append(this.caName).Append(newLine);
			}
			else
			{
				stringBuilder.Append("  Trusted CA cert: ").Append(this.TrustedCert).Append(newLine);
			}
			if (this.nc != null)
			{
				stringBuilder.Append("  Name Constraints: ").Append(this.nc).Append(newLine);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040018B0 RID: 6320
		private readonly AsymmetricKeyParameter pubKey;

		// Token: 0x040018B1 RID: 6321
		private readonly string caName;

		// Token: 0x040018B2 RID: 6322
		private readonly X509Name caPrincipal;

		// Token: 0x040018B3 RID: 6323
		private readonly X509Certificate trustedCert;

		// Token: 0x040018B4 RID: 6324
		private byte[] ncBytes;

		// Token: 0x040018B5 RID: 6325
		private NameConstraints nc;
	}
}
