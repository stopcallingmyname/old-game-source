using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Encoders;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000629 RID: 1577
	public abstract class Asn1OctetString : Asn1Object, Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06003B6F RID: 15215 RVA: 0x0016EA08 File Offset: 0x0016CC08
		public static Asn1OctetString GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is Asn1OctetString)
			{
				return Asn1OctetString.GetInstance(@object);
			}
			return BerOctetString.FromSequence(Asn1Sequence.GetInstance(@object));
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x0016EA3C File Offset: 0x0016CC3C
		public static Asn1OctetString GetInstance(object obj)
		{
			if (obj == null || obj is Asn1OctetString)
			{
				return (Asn1OctetString)obj;
			}
			if (obj is Asn1TaggedObject)
			{
				return Asn1OctetString.GetInstance(((Asn1TaggedObject)obj).GetObject());
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x0016EA89 File Offset: 0x0016CC89
		internal Asn1OctetString(byte[] str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x0016EAA8 File Offset: 0x0016CCA8
		internal Asn1OctetString(Asn1Encodable obj)
		{
			try
			{
				this.str = obj.GetEncoded("DER");
			}
			catch (IOException ex)
			{
				throw new ArgumentException("Error processing object : " + ex.ToString());
			}
		}

		// Token: 0x06003B73 RID: 15219 RVA: 0x0016EAF8 File Offset: 0x0016CCF8
		public Stream GetOctetStream()
		{
			return new MemoryStream(this.str, false);
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x000947CE File Offset: 0x000929CE
		public Asn1OctetStringParser Parser
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003B75 RID: 15221 RVA: 0x0016EB06 File Offset: 0x0016CD06
		public virtual byte[] GetOctets()
		{
			return this.str;
		}

		// Token: 0x06003B76 RID: 15222 RVA: 0x0016EB0E File Offset: 0x0016CD0E
		protected override int Asn1GetHashCode()
		{
			return Arrays.GetHashCode(this.GetOctets());
		}

		// Token: 0x06003B77 RID: 15223 RVA: 0x0016EB1C File Offset: 0x0016CD1C
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerOctetString derOctetString = asn1Object as DerOctetString;
			return derOctetString != null && Arrays.AreEqual(this.GetOctets(), derOctetString.GetOctets());
		}

		// Token: 0x06003B78 RID: 15224 RVA: 0x0016EB46 File Offset: 0x0016CD46
		public override string ToString()
		{
			return "#" + Hex.ToHexString(this.str);
		}

		// Token: 0x0400269B RID: 9883
		internal byte[] str;
	}
}
