using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.CryptoPro
{
	// Token: 0x02000760 RID: 1888
	public class Gost28147Parameters : Asn1Encodable
	{
		// Token: 0x060043E7 RID: 17383 RVA: 0x0018C6D6 File Offset: 0x0018A8D6
		public static Gost28147Parameters GetInstance(Asn1TaggedObject obj, bool explicitly)
		{
			return Gost28147Parameters.GetInstance(Asn1Sequence.GetInstance(obj, explicitly));
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0018C6E4 File Offset: 0x0018A8E4
		public static Gost28147Parameters GetInstance(object obj)
		{
			if (obj == null || obj is Gost28147Parameters)
			{
				return (Gost28147Parameters)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new Gost28147Parameters((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid GOST3410Parameter: " + Platform.GetTypeName(obj));
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0018C724 File Offset: 0x0018A924
		private Gost28147Parameters(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Wrong number of elements in sequence", "seq");
			}
			this.iv = Asn1OctetString.GetInstance(seq[0]);
			this.paramSet = DerObjectIdentifier.GetInstance(seq[1]);
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0018C774 File Offset: 0x0018A974
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.iv,
				this.paramSet
			});
		}

		// Token: 0x04002C99 RID: 11417
		private readonly Asn1OctetString iv;

		// Token: 0x04002C9A RID: 11418
		private readonly DerObjectIdentifier paramSet;
	}
}
