using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200044C RID: 1100
	public class SrpTlsClient : AbstractTlsClient
	{
		// Token: 0x06002B3C RID: 11068 RVA: 0x001146D3 File Offset: 0x001128D3
		public SrpTlsClient(byte[] identity, byte[] password) : this(new DefaultTlsCipherFactory(), new DefaultTlsSrpGroupVerifier(), identity, password)
		{
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x001146E7 File Offset: 0x001128E7
		public SrpTlsClient(TlsCipherFactory cipherFactory, byte[] identity, byte[] password) : this(cipherFactory, new DefaultTlsSrpGroupVerifier(), identity, password)
		{
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x001146F7 File Offset: 0x001128F7
		public SrpTlsClient(TlsCipherFactory cipherFactory, TlsSrpGroupVerifier groupVerifier, byte[] identity, byte[] password) : base(cipherFactory)
		{
			this.mGroupVerifier = groupVerifier;
			this.mIdentity = Arrays.Clone(identity);
			this.mPassword = Arrays.Clone(password);
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06002B3F RID: 11071 RVA: 0x0007D96F File Offset: 0x0007BB6F
		protected virtual bool RequireSrpServerExtension
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x00114720 File Offset: 0x00112920
		public override int[] GetCipherSuites()
		{
			return new int[]
			{
				49182
			};
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x00114730 File Offset: 0x00112930
		public override IDictionary GetClientExtensions()
		{
			IDictionary dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(base.GetClientExtensions());
			TlsSrpUtilities.AddSrpExtension(dictionary, this.mIdentity);
			return dictionary;
		}

		// Token: 0x06002B42 RID: 11074 RVA: 0x00114749 File Offset: 0x00112949
		public override void ProcessServerExtensions(IDictionary serverExtensions)
		{
			if (!TlsUtilities.HasExpectedEmptyExtensionData(serverExtensions, 12, 47) && this.RequireSrpServerExtension)
			{
				throw new TlsFatalAlert(47);
			}
			base.ProcessServerExtensions(serverExtensions);
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x00114770 File Offset: 0x00112970
		public override TlsKeyExchange GetKeyExchange()
		{
			int keyExchangeAlgorithm = TlsUtilities.GetKeyExchangeAlgorithm(this.mSelectedCipherSuite);
			if (keyExchangeAlgorithm - 21 <= 2)
			{
				return this.CreateSrpKeyExchange(keyExchangeAlgorithm);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x0010D2C7 File Offset: 0x0010B4C7
		public override TlsAuthentication GetAuthentication()
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x0011479F File Offset: 0x0011299F
		protected virtual TlsKeyExchange CreateSrpKeyExchange(int keyExchange)
		{
			return new TlsSrpKeyExchange(keyExchange, this.mSupportedSignatureAlgorithms, this.mGroupVerifier, this.mIdentity, this.mPassword);
		}

		// Token: 0x04001DF0 RID: 7664
		protected TlsSrpGroupVerifier mGroupVerifier;

		// Token: 0x04001DF1 RID: 7665
		protected byte[] mIdentity;

		// Token: 0x04001DF2 RID: 7666
		protected byte[] mPassword;
	}
}
