using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007CB RID: 1995
	public class ProtectedPart : Asn1Encodable
	{
		// Token: 0x060046FF RID: 18175 RVA: 0x00194B99 File Offset: 0x00192D99
		private ProtectedPart(Asn1Sequence seq)
		{
			this.header = PkiHeader.GetInstance(seq[0]);
			this.body = PkiBody.GetInstance(seq[1]);
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x00194BC5 File Offset: 0x00192DC5
		public static ProtectedPart GetInstance(object obj)
		{
			if (obj is ProtectedPart)
			{
				return (ProtectedPart)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new ProtectedPart((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x00194C04 File Offset: 0x00192E04
		public ProtectedPart(PkiHeader header, PkiBody body)
		{
			this.header = header;
			this.body = body;
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06004702 RID: 18178 RVA: 0x00194C1A File Offset: 0x00192E1A
		public virtual PkiHeader Header
		{
			get
			{
				return this.header;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004703 RID: 18179 RVA: 0x00194C22 File Offset: 0x00192E22
		public virtual PkiBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x00194C2A File Offset: 0x00192E2A
		public override Asn1Object ToAsn1Object()
		{
			return new DerSequence(new Asn1Encodable[]
			{
				this.header,
				this.body
			});
		}

		// Token: 0x04002E56 RID: 11862
		private readonly PkiHeader header;

		// Token: 0x04002E57 RID: 11863
		private readonly PkiBody body;
	}
}
