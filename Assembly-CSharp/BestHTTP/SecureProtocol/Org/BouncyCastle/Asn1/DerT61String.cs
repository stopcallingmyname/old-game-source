using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x02000663 RID: 1635
	public class DerT61String : DerStringBase
	{
		// Token: 0x06003D07 RID: 15623 RVA: 0x00172F0A File Offset: 0x0017110A
		public static DerT61String GetInstance(object obj)
		{
			if (obj == null || obj is DerT61String)
			{
				return (DerT61String)obj;
			}
			throw new ArgumentException("illegal object in GetInstance: " + Platform.GetTypeName(obj));
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x00172F34 File Offset: 0x00171134
		public static DerT61String GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			Asn1Object @object = obj.GetObject();
			if (isExplicit || @object is DerT61String)
			{
				return DerT61String.GetInstance(@object);
			}
			return new DerT61String(Asn1OctetString.GetInstance(@object).GetOctets());
		}

		// Token: 0x06003D09 RID: 15625 RVA: 0x00172F6A File Offset: 0x0017116A
		public DerT61String(byte[] str) : this(Strings.FromByteArray(str))
		{
		}

		// Token: 0x06003D0A RID: 15626 RVA: 0x00172F78 File Offset: 0x00171178
		public DerT61String(string str)
		{
			if (str == null)
			{
				throw new ArgumentNullException("str");
			}
			this.str = str;
		}

		// Token: 0x06003D0B RID: 15627 RVA: 0x00172F95 File Offset: 0x00171195
		public override string GetString()
		{
			return this.str;
		}

		// Token: 0x06003D0C RID: 15628 RVA: 0x00172F9D File Offset: 0x0017119D
		internal override void Encode(DerOutputStream derOut)
		{
			derOut.WriteEncoded(20, this.GetOctets());
		}

		// Token: 0x06003D0D RID: 15629 RVA: 0x00172FAD File Offset: 0x001711AD
		public byte[] GetOctets()
		{
			return Strings.ToByteArray(this.str);
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x00172FBC File Offset: 0x001711BC
		protected override bool Asn1Equals(Asn1Object asn1Object)
		{
			DerT61String derT61String = asn1Object as DerT61String;
			return derT61String != null && this.str.Equals(derT61String.str);
		}

		// Token: 0x04002700 RID: 9984
		private readonly string str;
	}
}
