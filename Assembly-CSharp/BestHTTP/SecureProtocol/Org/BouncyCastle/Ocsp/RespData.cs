using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.X509;
using BestHTTP.SecureProtocol.Org.BouncyCastle.X509;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp
{
	// Token: 0x02000300 RID: 768
	public class RespData : X509ExtensionBase
	{
		// Token: 0x06001BDC RID: 7132 RVA: 0x000D1883 File Offset: 0x000CFA83
		public RespData(ResponseData data)
		{
			this.data = data;
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001BDD RID: 7133 RVA: 0x000D1892 File Offset: 0x000CFA92
		public int Version
		{
			get
			{
				return this.data.Version.Value.IntValue + 1;
			}
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x000D18AB File Offset: 0x000CFAAB
		public RespID GetResponderId()
		{
			return new RespID(this.data.ResponderID);
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x000D18BD File Offset: 0x000CFABD
		public DateTime ProducedAt
		{
			get
			{
				return this.data.ProducedAt.ToDateTime();
			}
		}

		// Token: 0x06001BE0 RID: 7136 RVA: 0x000D18D0 File Offset: 0x000CFAD0
		public SingleResp[] GetResponses()
		{
			Asn1Sequence responses = this.data.Responses;
			SingleResp[] array = new SingleResp[responses.Count];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = new SingleResp(SingleResponse.GetInstance(responses[num]));
			}
			return array;
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06001BE1 RID: 7137 RVA: 0x000D1918 File Offset: 0x000CFB18
		public X509Extensions ResponseExtensions
		{
			get
			{
				return this.data.ResponseExtensions;
			}
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x000D1925 File Offset: 0x000CFB25
		protected override X509Extensions GetX509Extensions()
		{
			return this.ResponseExtensions;
		}

		// Token: 0x04001912 RID: 6418
		internal readonly ResponseData data;
	}
}
