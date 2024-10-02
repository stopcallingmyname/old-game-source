using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1
{
	// Token: 0x0200065A RID: 1626
	public class DerOutputStream : FilterStream
	{
		// Token: 0x06003CCE RID: 15566 RVA: 0x00172794 File Offset: 0x00170994
		public DerOutputStream(Stream os) : base(os)
		{
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x001727A0 File Offset: 0x001709A0
		private void WriteLength(int length)
		{
			if (length > 127)
			{
				int num = 1;
				uint num2 = (uint)length;
				while ((num2 >>= 8) != 0U)
				{
					num++;
				}
				this.WriteByte((byte)(num | 128));
				for (int i = (num - 1) * 8; i >= 0; i -= 8)
				{
					this.WriteByte((byte)(length >> i));
				}
				return;
			}
			this.WriteByte((byte)length);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x001727F7 File Offset: 0x001709F7
		internal void WriteEncoded(int tag, byte[] bytes)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(bytes.Length);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00172815 File Offset: 0x00170A15
		internal void WriteEncoded(int tag, byte first, byte[] bytes)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(bytes.Length + 1);
			this.WriteByte(first);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x0017283C File Offset: 0x00170A3C
		internal void WriteEncoded(int tag, byte[] bytes, int offset, int length)
		{
			this.WriteByte((byte)tag);
			this.WriteLength(length);
			this.Write(bytes, offset, length);
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x00172858 File Offset: 0x00170A58
		internal void WriteTag(int flags, int tagNo)
		{
			if (tagNo < 31)
			{
				this.WriteByte((byte)(flags | tagNo));
				return;
			}
			this.WriteByte((byte)(flags | 31));
			if (tagNo < 128)
			{
				this.WriteByte((byte)tagNo);
				return;
			}
			byte[] array = new byte[5];
			int num = array.Length;
			array[--num] = (byte)(tagNo & 127);
			do
			{
				tagNo >>= 7;
				array[--num] = (byte)((tagNo & 127) | 128);
			}
			while (tagNo > 127);
			this.Write(array, num, array.Length - num);
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x001728D1 File Offset: 0x00170AD1
		internal void WriteEncoded(int flags, int tagNo, byte[] bytes)
		{
			this.WriteTag(flags, tagNo);
			this.WriteLength(bytes.Length);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x001728EF File Offset: 0x00170AEF
		protected void WriteNull()
		{
			this.WriteByte(5);
			this.WriteByte(0);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00172900 File Offset: 0x00170B00
		[Obsolete("Use version taking an Asn1Encodable arg instead")]
		public virtual void WriteObject(object obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			if (obj is Asn1Object)
			{
				((Asn1Object)obj).Encode(this);
				return;
			}
			if (obj is Asn1Encodable)
			{
				((Asn1Encodable)obj).ToAsn1Object().Encode(this);
				return;
			}
			throw new IOException("object not Asn1Object");
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00172950 File Offset: 0x00170B50
		public virtual void WriteObject(Asn1Encodable obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			obj.ToAsn1Object().Encode(this);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x00172968 File Offset: 0x00170B68
		public virtual void WriteObject(Asn1Object obj)
		{
			if (obj == null)
			{
				this.WriteNull();
				return;
			}
			obj.Encode(this);
		}
	}
}
