using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000638 RID: 1592
	public class BerGenerator : Asn1Generator
	{
		// Token: 0x06003BC3 RID: 15299 RVA: 0x0016F90D File Offset: 0x0016DB0D
		protected BerGenerator(Stream outStream) : base(outStream)
		{
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0016F916 File Offset: 0x0016DB16
		public BerGenerator(Stream outStream, int tagNo, bool isExplicit) : base(outStream)
		{
			this._tagged = true;
			this._isExplicit = isExplicit;
			this._tagNo = tagNo;
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x0016F934 File Offset: 0x0016DB34
		public override void AddObject(Asn1Encodable obj)
		{
			new BerOutputStream(base.Out).WriteObject(obj);
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x0016F947 File Offset: 0x0016DB47
		public override Stream GetRawOutputStream()
		{
			return base.Out;
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x0016F94F File Offset: 0x0016DB4F
		public override void Close()
		{
			this.WriteBerEnd();
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0016F957 File Offset: 0x0016DB57
		private void WriteHdr(int tag)
		{
			base.Out.WriteByte((byte)tag);
			base.Out.WriteByte(128);
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x0016F978 File Offset: 0x0016DB78
		protected void WriteBerHeader(int tag)
		{
			if (!this._tagged)
			{
				this.WriteHdr(tag);
				return;
			}
			int num = this._tagNo | 128;
			if (this._isExplicit)
			{
				this.WriteHdr(num | 32);
				this.WriteHdr(tag);
				return;
			}
			if ((tag & 32) != 0)
			{
				this.WriteHdr(num | 32);
				return;
			}
			this.WriteHdr(num);
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x0016F9D4 File Offset: 0x0016DBD4
		protected void WriteBerBody(Stream contentStream)
		{
			Streams.PipeAll(contentStream, base.Out);
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x0016F9E4 File Offset: 0x0016DBE4
		protected void WriteBerEnd()
		{
			base.Out.WriteByte(0);
			base.Out.WriteByte(0);
			if (this._tagged && this._isExplicit)
			{
				base.Out.WriteByte(0);
				base.Out.WriteByte(0);
			}
		}

		// Token: 0x040026C2 RID: 9922
		private bool _tagged;

		// Token: 0x040026C3 RID: 9923
		private bool _isExplicit;

		// Token: 0x040026C4 RID: 9924
		private int _tagNo;
	}
}
