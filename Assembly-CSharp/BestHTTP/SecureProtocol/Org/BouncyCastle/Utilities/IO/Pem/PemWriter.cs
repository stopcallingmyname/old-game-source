using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x02000287 RID: 647
	public class PemWriter
	{
		// Token: 0x060017B2 RID: 6066 RVA: 0x000B84DE File Offset: 0x000B66DE
		public PemWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.nlLength = Platform.NewLine.Length;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x060017B3 RID: 6067 RVA: 0x000B8518 File Offset: 0x000B6718
		public TextWriter Writer
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000B8520 File Offset: 0x000B6720
		public int GetOutputSize(PemObject obj)
		{
			int num = 2 * (obj.Type.Length + 10 + this.nlLength) + 6 + 4;
			if (obj.Headers.Count > 0)
			{
				foreach (object obj2 in obj.Headers)
				{
					PemHeader pemHeader = (PemHeader)obj2;
					num += pemHeader.Name.Length + ": ".Length + pemHeader.Value.Length + this.nlLength;
				}
				num += this.nlLength;
			}
			int num2 = (obj.Content.Length + 2) / 3 * 4;
			num += num2 + (num2 + 64 - 1) / 64 * this.nlLength;
			return num;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000B85FC File Offset: 0x000B67FC
		public void WriteObject(PemObjectGenerator objGen)
		{
			PemObject pemObject = objGen.Generate();
			this.WritePreEncapsulationBoundary(pemObject.Type);
			if (pemObject.Headers.Count > 0)
			{
				foreach (object obj in pemObject.Headers)
				{
					PemHeader pemHeader = (PemHeader)obj;
					this.writer.Write(pemHeader.Name);
					this.writer.Write(": ");
					this.writer.WriteLine(pemHeader.Value);
				}
				this.writer.WriteLine();
			}
			this.WriteEncoded(pemObject.Content);
			this.WritePostEncapsulationBoundary(pemObject.Type);
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000B86C4 File Offset: 0x000B68C4
		private void WriteEncoded(byte[] bytes)
		{
			bytes = Base64.Encode(bytes);
			for (int i = 0; i < bytes.Length; i += this.buf.Length)
			{
				int num = 0;
				while (num != this.buf.Length && i + num < bytes.Length)
				{
					this.buf[num] = (char)bytes[i + num];
					num++;
				}
				this.writer.WriteLine(this.buf, 0, num);
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000B8729 File Offset: 0x000B6929
		private void WritePreEncapsulationBoundary(string type)
		{
			this.writer.WriteLine("-----BEGIN " + type + "-----");
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000B8746 File Offset: 0x000B6946
		private void WritePostEncapsulationBoundary(string type)
		{
			this.writer.WriteLine("-----END " + type + "-----");
		}

		// Token: 0x04001816 RID: 6166
		private const int LineLength = 64;

		// Token: 0x04001817 RID: 6167
		private readonly TextWriter writer;

		// Token: 0x04001818 RID: 6168
		private readonly int nlLength;

		// Token: 0x04001819 RID: 6169
		private char[] buf = new char[64];
	}
}
