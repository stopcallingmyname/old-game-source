using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Misc
{
	// Token: 0x0200071C RID: 1820
	public class Cast5CbcParameters : Asn1Encodable
	{
		// Token: 0x06004251 RID: 16977 RVA: 0x00185B9D File Offset: 0x00183D9D
		public static Cast5CbcParameters GetInstance(object o)
		{
			if (o is Cast5CbcParameters)
			{
				return (Cast5CbcParameters)o;
			}
			if (o is Asn1Sequence)
			{
				return new Cast5CbcParameters((Asn1Sequence)o);
			}
			throw new ArgumentException("unknown object in Cast5CbcParameters factory");
		}

		// Token: 0x06004252 RID: 16978 RVA: 0x00185BCC File Offset: 0x00183DCC
		public Cast5CbcParameters(byte[] iv, int keyLength)
		{
			this.iv = new DerOctetString(iv);
			this.keyLength = new DerInteger(keyLength);
		}

		// Token: 0x06004253 RID: 16979 RVA: 0x00185BEC File Offset: 0x00183DEC
		private Cast5CbcParameters(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.iv = (Asn1OctetString)seq[0];
			this.keyLength = (DerInteger)seq[1];
		}

		// Token: 0x06004254 RID: 16980 RVA: 0x00185C3C File Offset: 0x00183E3C
		public byte[] GetIV()
		{
			return Arrays.Clone(this.iv.GetOctets());
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x00185C4E File Offset: 0x00183E4E
		public int KeyLength
		{
			get
			{
				return this.keyLength.Value.IntValue;
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x00185C60 File Offset: 0x00183E60
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.iv,
				this.keyLength
			});
		}

		// Token: 0x04002B16 RID: 11030
		private readonly DerInteger keyLength;

		// Token: 0x04002B17 RID: 11031
		private readonly Asn1OctetString iv;
	}
}
