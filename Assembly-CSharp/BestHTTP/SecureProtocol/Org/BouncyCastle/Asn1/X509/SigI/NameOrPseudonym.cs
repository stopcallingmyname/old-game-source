using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509.SigI
{
	// Token: 0x020006CA RID: 1738
	public class NameOrPseudonym : Asn1Encodable, IAsn1Choice
	{
		// Token: 0x0600401B RID: 16411 RVA: 0x0017D694 File Offset: 0x0017B894
		public static NameOrPseudonym GetInstance(object obj)
		{
			if (obj == null || obj is NameOrPseudonym)
			{
				return (NameOrPseudonym)obj;
			}
			if (obj is IAsn1String)
			{
				return new NameOrPseudonym(DirectoryString.GetInstance(obj));
			}
			if (obj is Asn1Sequence)
			{
				return new NameOrPseudonym((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600401C RID: 16412 RVA: 0x0017D6F5 File Offset: 0x0017B8F5
		public NameOrPseudonym(DirectoryString pseudonym)
		{
			this.pseudonym = pseudonym;
		}

		// Token: 0x0600401D RID: 16413 RVA: 0x0017D704 File Offset: 0x0017B904
		private NameOrPseudonym(Asn1Sequence seq)
		{
			if (seq.Count != 2)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			if (!(seq[0] is IAsn1String))
			{
				throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(seq[0]));
			}
			this.surname = DirectoryString.GetInstance(seq[0]);
			this.givenName = Asn1Sequence.GetInstance(seq[1]);
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x0017D789 File Offset: 0x0017B989
		public NameOrPseudonym(string pseudonym) : this(new DirectoryString(pseudonym))
		{
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x0017D797 File Offset: 0x0017B997
		public NameOrPseudonym(DirectoryString surname, Asn1Sequence givenName)
		{
			this.surname = surname;
			this.givenName = givenName;
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x0017D7AD File Offset: 0x0017B9AD
		public DirectoryString Pseudonym
		{
			get
			{
				return this.pseudonym;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06004021 RID: 16417 RVA: 0x0017D7B5 File Offset: 0x0017B9B5
		public DirectoryString Surname
		{
			get
			{
				return this.surname;
			}
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x0017D7C0 File Offset: 0x0017B9C0
		public DirectoryString[] GetGivenName()
		{
			DirectoryString[] array = new DirectoryString[this.givenName.Count];
			int num = 0;
			foreach (object obj in this.givenName)
			{
				array[num++] = DirectoryString.GetInstance(obj);
			}
			return array;
		}

		// Token: 0x06004023 RID: 16419 RVA: 0x0017D834 File Offset: 0x0017BA34
		public override Asn1Object ToAsn1Object()
		{
			if (this.pseudonym != null)
			{
				return this.pseudonym.ToAsn1Object();
			}
			return new DerSequence(new Asn1Encodable[]
			{
				this.surname,
				this.givenName
			});
		}

		// Token: 0x040028CE RID: 10446
		private readonly DirectoryString pseudonym;

		// Token: 0x040028CF RID: 10447
		private readonly DirectoryString surname;

		// Token: 0x040028D0 RID: 10448
		private readonly Asn1Sequence givenName;
	}
}
