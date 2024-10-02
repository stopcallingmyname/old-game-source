using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security.Certificates;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x02000242 RID: 578
	public class X509CertificatePair
	{
		// Token: 0x060014E3 RID: 5347 RVA: 0x000AC1C2 File Offset: 0x000AA3C2
		public X509CertificatePair(X509Certificate forward, X509Certificate reverse)
		{
			this.forward = forward;
			this.reverse = reverse;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x000AC1D8 File Offset: 0x000AA3D8
		public X509CertificatePair(CertificatePair pair)
		{
			if (pair.Forward != null)
			{
				this.forward = new X509Certificate(pair.Forward);
			}
			if (pair.Reverse != null)
			{
				this.reverse = new X509Certificate(pair.Reverse);
			}
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x000AC214 File Offset: 0x000AA414
		public byte[] GetEncoded()
		{
			byte[] derEncoded;
			try
			{
				X509CertificateStructure x509CertificateStructure = null;
				X509CertificateStructure x509CertificateStructure2 = null;
				if (this.forward != null)
				{
					x509CertificateStructure = X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(this.forward.GetEncoded()));
					if (x509CertificateStructure == null)
					{
						throw new CertificateEncodingException("unable to get encoding for forward");
					}
				}
				if (this.reverse != null)
				{
					x509CertificateStructure2 = X509CertificateStructure.GetInstance(Asn1Object.FromByteArray(this.reverse.GetEncoded()));
					if (x509CertificateStructure2 == null)
					{
						throw new CertificateEncodingException("unable to get encoding for reverse");
					}
				}
				derEncoded = new CertificatePair(x509CertificateStructure, x509CertificateStructure2).GetDerEncoded();
			}
			catch (Exception ex)
			{
				throw new CertificateEncodingException(ex.Message, ex);
			}
			return derEncoded;
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060014E6 RID: 5350 RVA: 0x000AC2AC File Offset: 0x000AA4AC
		public X509Certificate Forward
		{
			get
			{
				return this.forward;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060014E7 RID: 5351 RVA: 0x000AC2B4 File Offset: 0x000AA4B4
		public X509Certificate Reverse
		{
			get
			{
				return this.reverse;
			}
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x000AC2BC File Offset: 0x000AA4BC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			X509CertificatePair x509CertificatePair = obj as X509CertificatePair;
			return x509CertificatePair != null && object.Equals(this.forward, x509CertificatePair.forward) && object.Equals(this.reverse, x509CertificatePair.reverse);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x000AC304 File Offset: 0x000AA504
		public override int GetHashCode()
		{
			int num = -1;
			if (this.forward != null)
			{
				num ^= this.forward.GetHashCode();
			}
			if (this.reverse != null)
			{
				num *= 17;
				num ^= this.reverse.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001625 RID: 5669
		private readonly X509Certificate forward;

		// Token: 0x04001626 RID: 5670
		private readonly X509Certificate reverse;
	}
}
