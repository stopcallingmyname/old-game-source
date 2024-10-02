using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Crmf
{
	// Token: 0x02000776 RID: 1910
	public class PopoSigningKeyInput : Asn1Encodable
	{
		// Token: 0x06004489 RID: 17545 RVA: 0x0018DF30 File Offset: 0x0018C130
		private PopoSigningKeyInput(Asn1Sequence seq)
		{
			Asn1Encodable asn1Encodable = seq[0];
			if (asn1Encodable is Asn1TaggedObject)
			{
				Asn1TaggedObject asn1TaggedObject = (Asn1TaggedObject)asn1Encodable;
				if (asn1TaggedObject.TagNo != 0)
				{
					throw new ArgumentException("Unknown authInfo tag: " + asn1TaggedObject.TagNo, "seq");
				}
				this.sender = GeneralName.GetInstance(asn1TaggedObject.GetObject());
			}
			else
			{
				this.publicKeyMac = PKMacValue.GetInstance(asn1Encodable);
			}
			this.publicKey = SubjectPublicKeyInfo.GetInstance(seq[1]);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0018DFB3 File Offset: 0x0018C1B3
		public static PopoSigningKeyInput GetInstance(object obj)
		{
			if (obj is PopoSigningKeyInput)
			{
				return (PopoSigningKeyInput)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new PopoSigningKeyInput((Asn1Sequence)obj);
			}
			throw new ArgumentException("Invalid object: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x0018DFF2 File Offset: 0x0018C1F2
		public PopoSigningKeyInput(GeneralName sender, SubjectPublicKeyInfo spki)
		{
			this.sender = sender;
			this.publicKey = spki;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x0018E008 File Offset: 0x0018C208
		public PopoSigningKeyInput(PKMacValue pkmac, SubjectPublicKeyInfo spki)
		{
			this.publicKeyMac = pkmac;
			this.publicKey = spki;
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x0600448D RID: 17549 RVA: 0x0018E01E File Offset: 0x0018C21E
		public virtual GeneralName Sender
		{
			get
			{
				return this.sender;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x0018E026 File Offset: 0x0018C226
		public virtual PKMacValue PublicKeyMac
		{
			get
			{
				return this.publicKeyMac;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x0018E02E File Offset: 0x0018C22E
		public virtual SubjectPublicKeyInfo PublicKey
		{
			get
			{
				return this.publicKey;
			}
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x0018E038 File Offset: 0x0018C238
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.sender != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerTaggedObject(false, 0, this.sender)
				});
			}
			else
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.publicKeyMac
				});
			}
			asn1EncodableVector.Add(new Asn1Encodable[]
			{
				this.publicKey
			});
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002CEE RID: 11502
		private readonly GeneralName sender;

		// Token: 0x04002CEF RID: 11503
		private readonly PKMacValue publicKeyMac;

		// Token: 0x04002CF0 RID: 11504
		private readonly SubjectPublicKeyInfo publicKey;
	}
}
