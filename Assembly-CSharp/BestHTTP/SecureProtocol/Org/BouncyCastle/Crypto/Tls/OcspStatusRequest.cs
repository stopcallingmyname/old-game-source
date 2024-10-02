using System;
using System.Collections;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200043C RID: 1084
	public class OcspStatusRequest
	{
		// Token: 0x06002AC0 RID: 10944 RVA: 0x0011344C File Offset: 0x0011164C
		public OcspStatusRequest(IList responderIDList, X509Extensions requestExtensions)
		{
			this.mResponderIDList = responderIDList;
			this.mRequestExtensions = requestExtensions;
		}

		// Token: 0x170005A1 RID: 1441
		// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x00113462 File Offset: 0x00111662
		public virtual IList ResponderIDList
		{
			get
			{
				return this.mResponderIDList;
			}
		}

		// Token: 0x170005A2 RID: 1442
		// (get) Token: 0x06002AC2 RID: 10946 RVA: 0x0011346A File Offset: 0x0011166A
		public virtual X509Extensions RequestExtensions
		{
			get
			{
				return this.mRequestExtensions;
			}
		}

		// Token: 0x06002AC3 RID: 10947 RVA: 0x00113474 File Offset: 0x00111674
		public virtual void Encode(Stream output)
		{
			if (this.mResponderIDList == null || this.mResponderIDList.Count < 1)
			{
				TlsUtilities.WriteUint16(0, output);
			}
			else
			{
				MemoryStream memoryStream = new MemoryStream();
				for (int i = 0; i < this.mResponderIDList.Count; i++)
				{
					TlsUtilities.WriteOpaque16(((ResponderID)this.mResponderIDList[i]).GetEncoded("DER"), memoryStream);
				}
				TlsUtilities.CheckUint16(memoryStream.Length);
				TlsUtilities.WriteUint16((int)memoryStream.Length, output);
				Streams.WriteBufTo(memoryStream, output);
			}
			if (this.mRequestExtensions == null)
			{
				TlsUtilities.WriteUint16(0, output);
				return;
			}
			byte[] encoded = this.mRequestExtensions.GetEncoded("DER");
			TlsUtilities.CheckUint16(encoded.Length);
			TlsUtilities.WriteUint16(encoded.Length, output);
			output.Write(encoded, 0, encoded.Length);
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x0011353C File Offset: 0x0011173C
		public static OcspStatusRequest Parse(Stream input)
		{
			IList list = Platform.CreateArrayList();
			int num = TlsUtilities.ReadUint16(input);
			if (num > 0)
			{
				MemoryStream memoryStream = new MemoryStream(TlsUtilities.ReadFully(num, input), false);
				do
				{
					ResponderID instance = ResponderID.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadOpaque16(memoryStream)));
					list.Add(instance);
				}
				while (memoryStream.Position < memoryStream.Length);
			}
			X509Extensions requestExtensions = null;
			int num2 = TlsUtilities.ReadUint16(input);
			if (num2 > 0)
			{
				requestExtensions = X509Extensions.GetInstance(TlsUtilities.ReadDerObject(TlsUtilities.ReadFully(num2, input)));
			}
			return new OcspStatusRequest(list, requestExtensions);
		}

		// Token: 0x04001D9E RID: 7582
		protected readonly IList mResponderIDList;

		// Token: 0x04001D9F RID: 7583
		protected readonly X509Extensions mRequestExtensions;
	}
}
