using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200048F RID: 1167
	public class UrlAndHash
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x0011EA90 File Offset: 0x0011CC90
		public UrlAndHash(string url, byte[] sha1Hash)
		{
			if (url == null || url.Length < 1 || url.Length >= 65536)
			{
				throw new ArgumentException("must have length from 1 to (2^16 - 1)", "url");
			}
			if (sha1Hash != null && sha1Hash.Length != 20)
			{
				throw new ArgumentException("must have length == 20, if present", "sha1Hash");
			}
			this.mUrl = url;
			this.mSha1Hash = sha1Hash;
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x06002DF6 RID: 11766 RVA: 0x0011EAF4 File Offset: 0x0011CCF4
		public virtual string Url
		{
			get
			{
				return this.mUrl;
			}
		}

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x06002DF7 RID: 11767 RVA: 0x0011EAFC File Offset: 0x0011CCFC
		public virtual byte[] Sha1Hash
		{
			get
			{
				return this.mSha1Hash;
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0011EB04 File Offset: 0x0011CD04
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteOpaque16(Strings.ToByteArray(this.mUrl), output);
			if (this.mSha1Hash == null)
			{
				TlsUtilities.WriteUint8(0, output);
				return;
			}
			TlsUtilities.WriteUint8(1, output);
			output.Write(this.mSha1Hash, 0, this.mSha1Hash.Length);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x0011EB44 File Offset: 0x0011CD44
		public static UrlAndHash Parse(TlsContext context, Stream input)
		{
			byte[] array = TlsUtilities.ReadOpaque16(input);
			if (array.Length < 1)
			{
				throw new TlsFatalAlert(47);
			}
			string url = Strings.FromByteArray(array);
			byte[] sha1Hash = null;
			byte b = TlsUtilities.ReadUint8(input);
			if (b != 0)
			{
				if (b != 1)
				{
					throw new TlsFatalAlert(47);
				}
				sha1Hash = TlsUtilities.ReadFully(20, input);
			}
			else if (TlsUtilities.IsTlsV12(context))
			{
				throw new TlsFatalAlert(47);
			}
			return new UrlAndHash(url, sha1Hash);
		}

		// Token: 0x04001EBB RID: 7867
		protected readonly string mUrl;

		// Token: 0x04001EBC RID: 7868
		protected readonly byte[] mSha1Hash;
	}
}
