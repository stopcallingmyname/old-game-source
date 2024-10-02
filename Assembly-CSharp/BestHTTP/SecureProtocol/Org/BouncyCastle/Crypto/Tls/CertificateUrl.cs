using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000408 RID: 1032
	public class CertificateUrl
	{
		// Token: 0x06002994 RID: 10644 RVA: 0x0010EA14 File Offset: 0x0010CC14
		public CertificateUrl(byte type, IList urlAndHashList)
		{
			if (!CertChainType.IsValid(type))
			{
				throw new ArgumentException("not a valid CertChainType value", "type");
			}
			if (urlAndHashList == null || urlAndHashList.Count < 1)
			{
				throw new ArgumentException("must have length > 0", "urlAndHashList");
			}
			this.mType = type;
			this.mUrlAndHashList = urlAndHashList;
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x0010EA69 File Offset: 0x0010CC69
		public virtual byte Type
		{
			get
			{
				return this.mType;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06002996 RID: 10646 RVA: 0x0010EA71 File Offset: 0x0010CC71
		public virtual IList UrlAndHashList
		{
			get
			{
				return this.mUrlAndHashList;
			}
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0010EA7C File Offset: 0x0010CC7C
		public virtual void Encode(Stream output)
		{
			TlsUtilities.WriteUint8(this.mType, output);
			CertificateUrl.ListBuffer16 listBuffer = new CertificateUrl.ListBuffer16();
			foreach (object obj in this.mUrlAndHashList)
			{
				((UrlAndHash)obj).Encode(listBuffer);
			}
			listBuffer.EncodeTo(output);
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x0010EAEC File Offset: 0x0010CCEC
		public static CertificateUrl parse(TlsContext context, Stream input)
		{
			byte b = TlsUtilities.ReadUint8(input);
			if (!CertChainType.IsValid(b))
			{
				throw new TlsFatalAlert(50);
			}
			int num = TlsUtilities.ReadUint16(input);
			if (num < 1)
			{
				throw new TlsFatalAlert(50);
			}
			MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				UrlAndHash value = UrlAndHash.Parse(context, memoryStream);
				list.Add(value);
			}
			return new CertificateUrl(b, list);
		}

		// Token: 0x04001B74 RID: 7028
		protected readonly byte mType;

		// Token: 0x04001B75 RID: 7029
		protected readonly IList mUrlAndHashList;

		// Token: 0x0200093C RID: 2364
		internal class ListBuffer16 : MemoryStream
		{
			// Token: 0x06004EC5 RID: 20165 RVA: 0x001B32A9 File Offset: 0x001B14A9
			internal ListBuffer16()
			{
				TlsUtilities.WriteUint16(0, this);
			}

			// Token: 0x06004EC6 RID: 20166 RVA: 0x001B32B8 File Offset: 0x001B14B8
			internal void EncodeTo(Stream output)
			{
				long num = this.Length - 2L;
				TlsUtilities.CheckUint16(num);
				this.Position = 0L;
				TlsUtilities.WriteUint16((int)num, this);
				Streams.WriteBufTo(this, output);
				Platform.Dispose(this);
			}
		}
	}
}
