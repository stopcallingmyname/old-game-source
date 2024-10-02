using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200063A RID: 1594
	public class BerOctetString : DerOctetString, IEnumerable
	{
		// Token: 0x06003BD0 RID: 15312 RVA: 0x0016FA70 File Offset: 0x0016DC70
		public static BerOctetString FromSequence(Asn1Sequence seq)
		{
			IList list = Platform.CreateArrayList();
			foreach (object obj in seq)
			{
				Asn1Encodable value = (Asn1Encodable)obj;
				list.Add(value);
			}
			return new BerOctetString(list);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x0016FAD4 File Offset: 0x0016DCD4
		private static byte[] ToBytes(IEnumerable octs)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in octs)
			{
				byte[] octets = ((DerOctetString)obj).GetOctets();
				memoryStream.Write(octets, 0, octets.Length);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0016FB40 File Offset: 0x0016DD40
		public BerOctetString(byte[] str) : base(str)
		{
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x0016FB49 File Offset: 0x0016DD49
		public BerOctetString(IEnumerable octets) : base(BerOctetString.ToBytes(octets))
		{
			this.octs = octets;
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x0016FB5E File Offset: 0x0016DD5E
		public BerOctetString(Asn1Object obj) : base(obj)
		{
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x0016FB67 File Offset: 0x0016DD67
		public BerOctetString(Asn1Encodable obj) : base(obj.ToAsn1Object())
		{
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0016EB06 File Offset: 0x0016CD06
		public override byte[] GetOctets()
		{
			return this.str;
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0016FB75 File Offset: 0x0016DD75
		public IEnumerator GetEnumerator()
		{
			if (this.octs == null)
			{
				return this.GenerateOcts().GetEnumerator();
			}
			return this.octs.GetEnumerator();
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0016FB96 File Offset: 0x0016DD96
		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0016FBA0 File Offset: 0x0016DDA0
		private IList GenerateOcts()
		{
			IList list = Platform.CreateArrayList();
			for (int i = 0; i < this.str.Length; i += 1000)
			{
				byte[] array = new byte[Math.Min(this.str.Length, i + 1000) - i];
				Array.Copy(this.str, i, array, 0, array.Length);
				list.Add(new DerOctetString(array));
			}
			return list;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x0016FC08 File Offset: 0x0016DE08
		internal override void Encode(DerOutputStream derOut)
		{
			if (derOut is Asn1OutputStream || derOut is BerOutputStream)
			{
				derOut.WriteByte(36);
				derOut.WriteByte(128);
				foreach (object obj in this)
				{
					DerOctetString obj2 = (DerOctetString)obj;
					derOut.WriteObject(obj2);
				}
				derOut.WriteByte(0);
				derOut.WriteByte(0);
				return;
			}
			base.Encode(derOut);
		}

		// Token: 0x040026C6 RID: 9926
		private const int MaxLength = 1000;

		// Token: 0x040026C7 RID: 9927
		private readonly IEnumerable octs;
	}
}
