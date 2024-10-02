using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000671 RID: 1649
	internal class LazyDerSet : DerSet
	{
		// Token: 0x06003D56 RID: 15702 RVA: 0x00173A1C File Offset: 0x00171C1C
		internal LazyDerSet(byte[] encoded)
		{
			this.encoded = encoded;
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x00173A2C File Offset: 0x00171C2C
		private void Parse()
		{
			lock (this)
			{
				if (this.encoded != null)
				{
					Asn1InputStream asn1InputStream = new LazyAsn1InputStream(this.encoded);
					Asn1Object obj;
					while ((obj = asn1InputStream.ReadObject()) != null)
					{
						base.AddObject(obj);
					}
					this.encoded = null;
				}
			}
		}

		// Token: 0x170007D6 RID: 2006
		public override Asn1Encodable this[int index]
		{
			get
			{
				this.Parse();
				return base[index];
			}
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x00173A9F File Offset: 0x00171C9F
		public override IEnumerator GetEnumerator()
		{
			this.Parse();
			return base.GetEnumerator();
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06003D5A RID: 15706 RVA: 0x00173AAD File Offset: 0x00171CAD
		public override int Count
		{
			get
			{
				this.Parse();
				return base.Count;
			}
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x00173ABC File Offset: 0x00171CBC
		internal override void Encode(DerOutputStream derOut)
		{
			lock (this)
			{
				if (this.encoded == null)
				{
					base.Encode(derOut);
				}
				else
				{
					derOut.WriteEncoded(49, this.encoded);
				}
			}
		}

		// Token: 0x0400270A RID: 9994
		private byte[] encoded;
	}
}
