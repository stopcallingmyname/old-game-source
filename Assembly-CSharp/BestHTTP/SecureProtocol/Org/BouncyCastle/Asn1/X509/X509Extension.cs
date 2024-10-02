using System;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x020006C3 RID: 1731
	public class X509Extension
	{
		// Token: 0x06003FD0 RID: 16336 RVA: 0x0017B3E8 File Offset: 0x001795E8
		public X509Extension(DerBoolean critical, Asn1OctetString value)
		{
			if (critical == null)
			{
				throw new ArgumentNullException("critical");
			}
			this.critical = critical.IsTrue;
			this.value = value;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0017B411 File Offset: 0x00179611
		public X509Extension(bool critical, Asn1OctetString value)
		{
			this.critical = critical;
			this.value = value;
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06003FD2 RID: 16338 RVA: 0x0017B427 File Offset: 0x00179627
		public bool IsCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06003FD3 RID: 16339 RVA: 0x0017B42F File Offset: 0x0017962F
		public Asn1OctetString Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x0017B437 File Offset: 0x00179637
		public Asn1Encodable GetParsedValue()
		{
			return X509Extension.ConvertValueToObject(this);
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0017B440 File Offset: 0x00179640
		public override int GetHashCode()
		{
			int hashCode = this.Value.GetHashCode();
			if (!this.IsCritical)
			{
				return ~hashCode;
			}
			return hashCode;
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0017B468 File Offset: 0x00179668
		public override bool Equals(object obj)
		{
			X509Extension x509Extension = obj as X509Extension;
			return x509Extension != null && this.Value.Equals(x509Extension.Value) && this.IsCritical == x509Extension.IsCritical;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0017B4A4 File Offset: 0x001796A4
		public static Asn1Object ConvertValueToObject(X509Extension ext)
		{
			Asn1Object result;
			try
			{
				result = Asn1Object.FromByteArray(ext.Value.GetOctets());
			}
			catch (Exception innerException)
			{
				throw new ArgumentException("can't convert extension", innerException);
			}
			return result;
		}

		// Token: 0x04002862 RID: 10338
		internal bool critical;

		// Token: 0x04002863 RID: 10339
		internal Asn1OctetString value;
	}
}
