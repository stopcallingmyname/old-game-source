using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065C RID: 1628
	public class DerSequence : Asn1Sequence
	{
		// Token: 0x06003CE3 RID: 15587 RVA: 0x00172B18 File Offset: 0x00170D18
		public static DerSequence FromVector(Asn1EncodableVector v)
		{
			if (v.Count >= 1)
			{
				return new DerSequence(v);
			}
			return DerSequence.Empty;
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x00172B2F File Offset: 0x00170D2F
		public DerSequence() : base(0)
		{
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x00172B38 File Offset: 0x00170D38
		public DerSequence(Asn1Encodable obj) : base(1)
		{
			base.AddObject(obj);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x00172B48 File Offset: 0x00170D48
		public DerSequence(params Asn1Encodable[] v) : base(v.Length)
		{
			foreach (Asn1Encodable obj in v)
			{
				base.AddObject(obj);
			}
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x00172B7C File Offset: 0x00170D7C
		public DerSequence(Asn1EncodableVector v) : base(v.Count)
		{
			foreach (object obj in v)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				base.AddObject(obj2);
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x00172BDC File Offset: 0x00170DDC
		internal override void Encode(DerOutputStream derOut)
		{
			MemoryStream memoryStream = new MemoryStream();
			DerOutputStream derOutputStream = new DerOutputStream(memoryStream);
			foreach (object obj in this)
			{
				Asn1Encodable obj2 = (Asn1Encodable)obj;
				derOutputStream.WriteObject(obj2);
			}
			Platform.Dispose(derOutputStream);
			byte[] bytes = memoryStream.ToArray();
			derOut.WriteEncoded(48, bytes);
		}

		// Token: 0x040026FA RID: 9978
		public static readonly DerSequence Empty = new DerSequence();
	}
}
