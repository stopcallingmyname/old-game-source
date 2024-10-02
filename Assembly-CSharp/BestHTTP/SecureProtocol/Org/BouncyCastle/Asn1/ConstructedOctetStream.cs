using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000646 RID: 1606
	internal class ConstructedOctetStream : BaseInputStream
	{
		// Token: 0x06003C08 RID: 15368 RVA: 0x00170240 File Offset: 0x0016E440
		internal ConstructedOctetStream(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00170258 File Offset: 0x0016E458
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._currentStream == null)
			{
				if (!this._first)
				{
					return 0;
				}
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser == null)
				{
					return 0;
				}
				this._first = false;
				this._currentStream = asn1OctetStringParser.GetOctetStream();
			}
			int num = 0;
			for (;;)
			{
				int num2 = this._currentStream.Read(buffer, offset + num, count - num);
				if (num2 > 0)
				{
					num += num2;
					if (num == count)
					{
						break;
					}
				}
				else
				{
					Asn1OctetStringParser asn1OctetStringParser2 = (Asn1OctetStringParser)this._parser.ReadObject();
					if (asn1OctetStringParser2 == null)
					{
						goto Block_6;
					}
					this._currentStream = asn1OctetStringParser2.GetOctetStream();
				}
			}
			return num;
			Block_6:
			this._currentStream = null;
			return num;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001702F0 File Offset: 0x0016E4F0
		public override int ReadByte()
		{
			if (this._currentStream == null)
			{
				if (!this._first)
				{
					return 0;
				}
				Asn1OctetStringParser asn1OctetStringParser = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser == null)
				{
					return 0;
				}
				this._first = false;
				this._currentStream = asn1OctetStringParser.GetOctetStream();
			}
			int num;
			for (;;)
			{
				num = this._currentStream.ReadByte();
				if (num >= 0)
				{
					break;
				}
				Asn1OctetStringParser asn1OctetStringParser2 = (Asn1OctetStringParser)this._parser.ReadObject();
				if (asn1OctetStringParser2 == null)
				{
					goto Block_5;
				}
				this._currentStream = asn1OctetStringParser2.GetOctetStream();
			}
			return num;
			Block_5:
			this._currentStream = null;
			return -1;
		}

		// Token: 0x040026D0 RID: 9936
		private readonly Asn1StreamParser _parser;

		// Token: 0x040026D1 RID: 9937
		private bool _first = true;

		// Token: 0x040026D2 RID: 9938
		private Stream _currentStream;
	}
}
