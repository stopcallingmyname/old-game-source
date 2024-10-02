using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Cmp
{
	// Token: 0x020007C9 RID: 1993
	public class PopoDecKeyChallContent : Asn1Encodable
	{
		// Token: 0x060046F7 RID: 18167 RVA: 0x00194A67 File Offset: 0x00192C67
		private PopoDecKeyChallContent(Asn1Sequence seq)
		{
			this.content = seq;
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x00194A76 File Offset: 0x00192C76
		public static PopoDecKeyChallContent GetInstance(object obj)
		{
			if (obj is PopoDecKeyChallContent)
			{
				return (PopoDecKeyChallContent)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PopoDecKeyChallContent((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x00194AB8 File Offset: 0x00192CB8
		public virtual Challenge[] ToChallengeArray()
		{
			Challenge[] array = new Challenge[this.content.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = Challenge.GetInstance(this.content[num]);
			}
			return array;
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x00194AF9 File Offset: 0x00192CF9
		public override Asn1Object ToAsn1Object()
		{
			return this.content;
		}

		// Token: 0x04002E54 RID: 11860
		private readonly Asn1Sequence content;
	}
}
