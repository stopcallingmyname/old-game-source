using System;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.X509
{
	// Token: 0x0200023C RID: 572
	internal class PemParser
	{
		// Token: 0x060014A6 RID: 5286 RVA: 0x000AAED0 File Offset: 0x000A90D0
		internal PemParser(string type)
		{
			this._header1 = "-----BEGIN " + type + "-----";
			this._header2 = "-----BEGIN X509 " + type + "-----";
			this._footer1 = "-----END " + type + "-----";
			this._footer2 = "-----END X509 " + type + "-----";
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x000AAF3C File Offset: 0x000A913C
		private string ReadLine(Stream inStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num;
			for (;;)
			{
				if ((num = inStream.ReadByte()) == 13 || num == 10 || num < 0)
				{
					if (num < 0 || stringBuilder.Length != 0)
					{
						break;
					}
				}
				else if (num != 13)
				{
					stringBuilder.Append((char)num);
				}
			}
			if (num < 0)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x000AAF8C File Offset: 0x000A918C
		internal Asn1Sequence ReadPemObject(Stream inStream)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text;
			while ((text = this.ReadLine(inStream)) != null)
			{
				if (Platform.StartsWith(text, this._header1) || Platform.StartsWith(text, this._header2))
				{
					IL_55:
					while ((text = this.ReadLine(inStream)) != null && !Platform.StartsWith(text, this._footer1) && !Platform.StartsWith(text, this._footer2))
					{
						stringBuilder.Append(text);
					}
					if (stringBuilder.Length == 0)
					{
						return null;
					}
					Asn1Object asn1Object = Asn1Object.FromByteArray(Base64.Decode(stringBuilder.ToString()));
					if (!(asn1Object is Asn1Sequence))
					{
						throw new IOException("malformed PEM data encountered");
					}
					return (Asn1Sequence)asn1Object;
				}
			}
			goto IL_55;
		}

		// Token: 0x04001617 RID: 5655
		private readonly string _header1;

		// Token: 0x04001618 RID: 5656
		private readonly string _header2;

		// Token: 0x04001619 RID: 5657
		private readonly string _footer1;

		// Token: 0x0400161A RID: 5658
		private readonly string _footer2;
	}
}
