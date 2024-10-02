using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X500;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.IsisMtt.X509
{
	// Token: 0x0200072A RID: 1834
	public class NamingAuthority : Asn1Encodable
	{
		// Token: 0x0600428E RID: 17038 RVA: 0x00186AD0 File Offset: 0x00184CD0
		public static NamingAuthority GetInstance(object obj)
		{
			if (obj == null || obj is NamingAuthority)
			{
				return (NamingAuthority)obj;
			}
			if (obj is Asn1Sequence)
			{
				return new NamingAuthority((Asn1Sequence)obj);
			}
			throw new ArgumentException("unknown object in factory: " + Platform.GetTypeName(obj), "obj");
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x00186B1D File Offset: 0x00184D1D
		public static NamingAuthority GetInstance(Asn1TaggedObject obj, bool isExplicit)
		{
			return NamingAuthority.GetInstance(Asn1Sequence.GetInstance(obj, isExplicit));
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x00186B2C File Offset: 0x00184D2C
		private NamingAuthority(Asn1Sequence seq)
		{
			if (seq.Count > 3)
			{
				throw new ArgumentException("Bad sequence size: " + seq.Count);
			}
			IEnumerator enumerator = seq.GetEnumerator();
			if (enumerator.MoveNext())
			{
				Asn1Encodable asn1Encodable = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable is DerObjectIdentifier)
				{
					this.namingAuthorityID = (DerObjectIdentifier)asn1Encodable;
				}
				else if (asn1Encodable is DerIA5String)
				{
					this.namingAuthorityUrl = DerIA5String.GetInstance(asn1Encodable).GetString();
				}
				else
				{
					if (!(asn1Encodable is IAsn1String))
					{
						throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable));
					}
					this.namingAuthorityText = DirectoryString.GetInstance(asn1Encodable);
				}
			}
			if (enumerator.MoveNext())
			{
				Asn1Encodable asn1Encodable2 = (Asn1Encodable)enumerator.Current;
				if (asn1Encodable2 is DerIA5String)
				{
					this.namingAuthorityUrl = DerIA5String.GetInstance(asn1Encodable2).GetString();
				}
				else
				{
					if (!(asn1Encodable2 is IAsn1String))
					{
						throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable2));
					}
					this.namingAuthorityText = DirectoryString.GetInstance(asn1Encodable2);
				}
			}
			if (!enumerator.MoveNext())
			{
				return;
			}
			Asn1Encodable asn1Encodable3 = (Asn1Encodable)enumerator.Current;
			if (asn1Encodable3 is IAsn1String)
			{
				this.namingAuthorityText = DirectoryString.GetInstance(asn1Encodable3);
				return;
			}
			throw new ArgumentException("Bad object encountered: " + Platform.GetTypeName(asn1Encodable3));
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06004291 RID: 17041 RVA: 0x00186C75 File Offset: 0x00184E75
		public virtual DerObjectIdentifier NamingAuthorityID
		{
			get
			{
				return this.namingAuthorityID;
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06004292 RID: 17042 RVA: 0x00186C7D File Offset: 0x00184E7D
		public virtual DirectoryString NamingAuthorityText
		{
			get
			{
				return this.namingAuthorityText;
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06004293 RID: 17043 RVA: 0x00186C85 File Offset: 0x00184E85
		public virtual string NamingAuthorityUrl
		{
			get
			{
				return this.namingAuthorityUrl;
			}
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x00186C8D File Offset: 0x00184E8D
		public NamingAuthority(DerObjectIdentifier namingAuthorityID, string namingAuthorityUrl, DirectoryString namingAuthorityText)
		{
			this.namingAuthorityID = namingAuthorityID;
			this.namingAuthorityUrl = namingAuthorityUrl;
			this.namingAuthorityText = namingAuthorityText;
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x00186CAC File Offset: 0x00184EAC
		public override Asn1Object ToAsn1Object()
		{
			Asn1EncodableVector asn1EncodableVector = new Asn1EncodableVector(Array.Empty<Asn1Encodable>());
			if (this.namingAuthorityID != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.namingAuthorityID
				});
			}
			if (this.namingAuthorityUrl != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					new DerIA5String(this.namingAuthorityUrl, true)
				});
			}
			if (this.namingAuthorityText != null)
			{
				asn1EncodableVector.Add(new Asn1Encodable[]
				{
					this.namingAuthorityText
				});
			}
			return new DerSequence(asn1EncodableVector);
		}

		// Token: 0x04002B6D RID: 11117
		public static readonly DerObjectIdentifier IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern = new DerObjectIdentifier(IsisMttObjectIdentifiers.IdIsisMttATNamingAuthorities + ".1");

		// Token: 0x04002B6E RID: 11118
		private readonly DerObjectIdentifier namingAuthorityID;

		// Token: 0x04002B6F RID: 11119
		private readonly string namingAuthorityUrl;

		// Token: 0x04002B70 RID: 11120
		private readonly DirectoryString namingAuthorityText;
	}
}
