using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Zlib;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Cms
{
	// Token: 0x020005EC RID: 1516
	public class CmsCompressedDataStreamGenerator
	{
		// Token: 0x060039AF RID: 14767 RVA: 0x00167A20 File Offset: 0x00165C20
		public void SetBufferSize(int bufferSize)
		{
			this._bufferSize = bufferSize;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x00167A29 File Offset: 0x00165C29
		public Stream Open(Stream outStream, string compressionOID)
		{
			return this.Open(outStream, CmsObjectIdentifiers.Data.Id, compressionOID);
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x00167A40 File Offset: 0x00165C40
		public Stream Open(Stream outStream, string contentOID, string compressionOID)
		{
			BerSequenceGenerator berSequenceGenerator = new BerSequenceGenerator(outStream);
			berSequenceGenerator.AddObject(CmsObjectIdentifiers.CompressedData);
			BerSequenceGenerator berSequenceGenerator2 = new BerSequenceGenerator(berSequenceGenerator.GetRawOutputStream(), 0, true);
			berSequenceGenerator2.AddObject(new DerInteger(0));
			berSequenceGenerator2.AddObject(new AlgorithmIdentifier(new DerObjectIdentifier("1.2.840.113549.1.9.16.3.8")));
			BerSequenceGenerator berSequenceGenerator3 = new BerSequenceGenerator(berSequenceGenerator2.GetRawOutputStream());
			berSequenceGenerator3.AddObject(new DerObjectIdentifier(contentOID));
			return new CmsCompressedDataStreamGenerator.CmsCompressedOutputStream(new ZOutputStream(CmsUtilities.CreateBerOctetOutputStream(berSequenceGenerator3.GetRawOutputStream(), 0, true, this._bufferSize), -1), berSequenceGenerator, berSequenceGenerator2, berSequenceGenerator3);
		}

		// Token: 0x040025D7 RID: 9687
		public const string ZLib = "1.2.840.113549.1.9.16.3.8";

		// Token: 0x040025D8 RID: 9688
		private int _bufferSize;

		// Token: 0x02000980 RID: 2432
		private class CmsCompressedOutputStream : BaseOutputStream
		{
			// Token: 0x06004FA7 RID: 20391 RVA: 0x001B6E20 File Offset: 0x001B5020
			internal CmsCompressedOutputStream(ZOutputStream outStream, BerSequenceGenerator sGen, BerSequenceGenerator cGen, BerSequenceGenerator eiGen)
			{
				this._out = outStream;
				this._sGen = sGen;
				this._cGen = cGen;
				this._eiGen = eiGen;
			}

			// Token: 0x06004FA8 RID: 20392 RVA: 0x001B6E45 File Offset: 0x001B5045
			public override void WriteByte(byte b)
			{
				this._out.WriteByte(b);
			}

			// Token: 0x06004FA9 RID: 20393 RVA: 0x001B6E53 File Offset: 0x001B5053
			public override void Write(byte[] bytes, int off, int len)
			{
				this._out.Write(bytes, off, len);
			}

			// Token: 0x06004FAA RID: 20394 RVA: 0x001B6E63 File Offset: 0x001B5063
			public override void Close()
			{
				Platform.Dispose(this._out);
				this._eiGen.Close();
				this._cGen.Close();
				this._sGen.Close();
				base.Close();
			}

			// Token: 0x040036E0 RID: 14048
			private ZOutputStream _out;

			// Token: 0x040036E1 RID: 14049
			private BerSequenceGenerator _sGen;

			// Token: 0x040036E2 RID: 14050
			private BerSequenceGenerator _cGen;

			// Token: 0x040036E3 RID: 14051
			private BerSequenceGenerator _eiGen;
		}
	}
}
