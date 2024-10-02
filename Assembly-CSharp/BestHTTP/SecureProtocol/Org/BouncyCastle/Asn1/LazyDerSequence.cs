using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000670 RID: 1648
	internal class LazyDerSequence : DerSequence
	{
		// Token: 0x06003D50 RID: 15696 RVA: 0x00173927 File Offset: 0x00171B27
		internal LazyDerSequence(byte[] encoded)
		{
			this.encoded = encoded;
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00173938 File Offset: 0x00171B38
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

		// Token: 0x170007D4 RID: 2004
		public override Asn1Encodable this[int index]
		{
			get
			{
				this.Parse();
				return base[index];
			}
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x001739AB File Offset: 0x00171BAB
		public override IEnumerator GetEnumerator()
		{
			this.Parse();
			return base.GetEnumerator();
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06003D54 RID: 15700 RVA: 0x001739B9 File Offset: 0x00171BB9
		public override int Count
		{
			get
			{
				this.Parse();
				return base.Count;
			}
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x001739C8 File Offset: 0x00171BC8
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
					derOut.WriteEncoded(48, this.encoded);
				}
			}
		}

		// Token: 0x04002709 RID: 9993
		private byte[] encoded;
	}
}
