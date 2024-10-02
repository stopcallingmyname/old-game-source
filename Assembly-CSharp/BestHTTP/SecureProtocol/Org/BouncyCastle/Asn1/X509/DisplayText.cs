using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000697 RID: 1687
	public class DisplayText : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x06003E61 RID: 15969 RVA: 0x00176C38 File Offset: 0x00174E38
		public DisplayText(int type, string text)
		{
			if (text.Length > 200)
			{
				text = text.Substring(0, 200);
			}
			this.contentType = type;
			switch (type)
			{
			case 0:
				this.contents = new DerIA5String(text);
				return;
			case 1:
				this.contents = new DerBmpString(text);
				return;
			case 2:
				this.contents = new DerUtf8String(text);
				return;
			case 3:
				this.contents = new DerVisibleString(text);
				return;
			default:
				this.contents = new DerUtf8String(text);
				return;
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00176CC5 File Offset: 0x00174EC5
		public DisplayText(string text)
		{
			if (text.Length > 200)
			{
				text = text.Substring(0, 200);
			}
			this.contentType = 2;
			this.contents = new DerUtf8String(text);
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00176CFB File Offset: 0x00174EFB
		public DisplayText(IAsn1String contents)
		{
			this.contents = contents;
		}

		// Token: 0x06003E64 RID: 15972 RVA: 0x00176D0A File Offset: 0x00174F0A
		public static DisplayText GetInstance(object obj)
		{
			if (obj is IAsn1String)
			{
				return new DisplayText((IAsn1String)obj);
			}
			if (obj is DisplayText)
			{
				return (DisplayText)obj;
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06003E65 RID: 15973 RVA: 0x00176D49 File Offset: 0x00174F49
		public override Asn1Object ToAsn1Object()
		{
			return (Asn1Object)this.contents;
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x00176D56 File Offset: 0x00174F56
		public string GetString()
		{
			return this.contents.GetString();
		}

		// Token: 0x040027A5 RID: 10149
		public const int ContentTypeIA5String = 0;

		// Token: 0x040027A6 RID: 10150
		public const int ContentTypeBmpString = 1;

		// Token: 0x040027A7 RID: 10151
		public const int ContentTypeUtf8String = 2;

		// Token: 0x040027A8 RID: 10152
		public const int ContentTypeVisibleString = 3;

		// Token: 0x040027A9 RID: 10153
		public const int DisplayTextMaximumSize = 200;

		// Token: 0x040027AA RID: 10154
		internal readonly int contentType;

		// Token: 0x040027AB RID: 10155
		internal readonly IAsn1String contents;
	}
}
