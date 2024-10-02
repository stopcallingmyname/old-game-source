using System;
using System.Collections;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x02000286 RID: 646
	public class PemReader
	{
		// Token: 0x060017AE RID: 6062 RVA: 0x000B8378 File Offset: 0x000B6578
		public PemReader(TextReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.reader = reader;
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x060017AF RID: 6063 RVA: 0x000B8395 File Offset: 0x000B6595
		public TextReader Reader
		{
			get
			{
				return this.reader;
			}
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000B83A0 File Offset: 0x000B65A0
		public PemObject ReadPemObject()
		{
			string text = this.reader.ReadLine();
			if (text != null && Platform.StartsWith(text, "-----BEGIN "))
			{
				text = text.Substring("-----BEGIN ".Length);
				int num = text.IndexOf('-');
				string type = text.Substring(0, num);
				if (num > 0)
				{
					return this.LoadObject(type);
				}
			}
			return null;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000B83FC File Offset: 0x000B65FC
		private PemObject LoadObject(string type)
		{
			string text = "-----END " + type;
			IList list = Platform.CreateArrayList();
			StringBuilder stringBuilder = new StringBuilder();
			string text2;
			while ((text2 = this.reader.ReadLine()) != null && Platform.IndexOf(text2, text) == -1)
			{
				int num = text2.IndexOf(':');
				if (num == -1)
				{
					stringBuilder.Append(text2.Trim());
				}
				else
				{
					string text3 = text2.Substring(0, num).Trim();
					if (Platform.StartsWith(text3, "X-"))
					{
						text3 = text3.Substring(2);
					}
					string val = text2.Substring(num + 1).Trim();
					list.Add(new PemHeader(text3, val));
				}
			}
			if (text2 == null)
			{
				throw new IOException(text + " not found");
			}
			if (stringBuilder.Length % 4 != 0)
			{
				throw new IOException("base64 data appears to be truncated");
			}
			return new PemObject(type, list, Base64.Decode(stringBuilder.ToString()));
		}

		// Token: 0x04001813 RID: 6163
		private const string BeginString = "-----BEGIN ";

		// Token: 0x04001814 RID: 6164
		private const string EndString = "-----END ";

		// Token: 0x04001815 RID: 6165
		private readonly TextReader reader;
	}
}
