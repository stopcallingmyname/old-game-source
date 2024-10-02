using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x02000725 RID: 1829
	public class AdditionalInformationSyntax : Asn1Encodable
	{
		// Token: 0x0600426B RID: 17003 RVA: 0x0018630D File Offset: 0x0018450D
		public static AdditionalInformationSyntax GetInstance(object obj)
		{
			if (obj is AdditionalInformationSyntax)
			{
				return (AdditionalInformationSyntax)obj;
			}
			if (obj is IAsn1String)
			{
				return new AdditionalInformationSyntax(DirectoryString.GetInstance(obj));
			}
			throw new ArgumentException("Unknown object in GetInstance: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x0018634C File Offset: 0x0018454C
		private AdditionalInformationSyntax(DirectoryString information)
		{
			this.information = information;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x0018635B File Offset: 0x0018455B
		public AdditionalInformationSyntax(string information)
		{
			this.information = new DirectoryString(information);
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600426E RID: 17006 RVA: 0x0018636F File Offset: 0x0018456F
		public virtual DirectoryString Information
		{
			get
			{
				return this.information;
			}
		}

		// Token: 0x0600426F RID: 17007 RVA: 0x00186377 File Offset: 0x00184577
		public override Asn1Object ToAsn1Object()
		{
			return this.information.ToAsn1Object();
		}

		// Token: 0x04002B63 RID: 11107
		private readonly DirectoryString information;
	}
}
