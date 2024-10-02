using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Tsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tsp
{
	// Token: 0x020002A4 RID: 676
	public class TimeStampRequest : X509ExtensionBase
	{
		// Token: 0x0600189C RID: 6300 RVA: 0x000BA08A File Offset: 0x000B828A
		public TimeStampRequest(TimeStampReq req)
		{
			this.req = req;
			this.extensions = req.Extensions;
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000BA0A5 File Offset: 0x000B82A5
		public TimeStampRequest(byte[] req) : this(new Asn1InputStream(req))
		{
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000BA0B3 File Offset: 0x000B82B3
		public TimeStampRequest(Stream input) : this(new Asn1InputStream(input))
		{
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000BA0C4 File Offset: 0x000B82C4
		private TimeStampRequest(Asn1InputStream str)
		{
			try
			{
				this.req = TimeStampReq.GetInstance(str.ReadObject());
			}
			catch (InvalidCastException arg)
			{
				throw new IOException("malformed request: " + arg);
			}
			catch (ArgumentException arg2)
			{
				throw new IOException("malformed request: " + arg2);
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060018A0 RID: 6304 RVA: 0x000BA12C File Offset: 0x000B832C
		public int Version
		{
			get
			{
				return this.req.Version.Value.IntValue;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060018A1 RID: 6305 RVA: 0x000BA143 File Offset: 0x000B8343
		public string MessageImprintAlgOid
		{
			get
			{
				return this.req.MessageImprint.HashAlgorithm.Algorithm.Id;
			}
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000BA15F File Offset: 0x000B835F
		public byte[] GetMessageImprintDigest()
		{
			return this.req.MessageImprint.GetHashedMessage();
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060018A3 RID: 6307 RVA: 0x000BA171 File Offset: 0x000B8371
		public string ReqPolicy
		{
			get
			{
				if (this.req.ReqPolicy != null)
				{
					return this.req.ReqPolicy.Id;
				}
				return null;
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x060018A4 RID: 6308 RVA: 0x000BA192 File Offset: 0x000B8392
		public BigInteger Nonce
		{
			get
			{
				if (this.req.Nonce != null)
				{
					return this.req.Nonce.Value;
				}
				return null;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x060018A5 RID: 6309 RVA: 0x000BA1B3 File Offset: 0x000B83B3
		public bool CertReq
		{
			get
			{
				return this.req.CertReq != null && this.req.CertReq.IsTrue;
			}
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x000BA1D4 File Offset: 0x000B83D4
		public void Validate(IList algorithms, IList policies, IList extensions)
		{
			if (!algorithms.Contains(this.MessageImprintAlgOid))
			{
				throw new TspValidationException("request contains unknown algorithm", 128);
			}
			if (policies != null && this.ReqPolicy != null && !policies.Contains(this.ReqPolicy))
			{
				throw new TspValidationException("request contains unknown policy", 256);
			}
			if (this.Extensions != null && extensions != null)
			{
				foreach (object obj in this.Extensions.ExtensionOids)
				{
					DerObjectIdentifier derObjectIdentifier = (DerObjectIdentifier)obj;
					if (!extensions.Contains(derObjectIdentifier.Id))
					{
						throw new TspValidationException("request contains unknown extension", 8388608);
					}
				}
			}
			if (TspUtil.GetDigestLength(this.MessageImprintAlgOid) != this.GetMessageImprintDigest().Length)
			{
				throw new TspValidationException("imprint digest the wrong length", 4);
			}
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x000BA2BC File Offset: 0x000B84BC
		public byte[] GetEncoded()
		{
			return this.req.GetEncoded();
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x060018A8 RID: 6312 RVA: 0x000BA2C9 File Offset: 0x000B84C9
		internal X509Extensions Extensions
		{
			get
			{
				return this.req.Extensions;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x060018A9 RID: 6313 RVA: 0x000BA2D6 File Offset: 0x000B84D6
		public virtual bool HasExtensions
		{
			get
			{
				return this.extensions != null;
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000BA2E1 File Offset: 0x000B84E1
		public virtual X509Extension GetExtension(DerObjectIdentifier oid)
		{
			if (this.extensions != null)
			{
				return this.extensions.GetExtension(oid);
			}
			return null;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x000BA2F9 File Offset: 0x000B84F9
		public virtual IList GetExtensionOids()
		{
			return TspUtil.GetExtensionOids(this.extensions);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x000BA306 File Offset: 0x000B8506
		protected override X509Extensions GetX509Extensions()
		{
			return this.Extensions;
		}

		// Token: 0x04001836 RID: 6198
		private TimeStampReq req;

		// Token: 0x04001837 RID: 6199
		private X509Extensions extensions;
	}
}
