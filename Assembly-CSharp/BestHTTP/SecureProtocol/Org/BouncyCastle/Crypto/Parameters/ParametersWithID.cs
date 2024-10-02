using System;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004F3 RID: 1267
	public class ParametersWithID : ICipherParameters
	{
		// Token: 0x06003084 RID: 12420 RVA: 0x001279CC File Offset: 0x00125BCC
		public ParametersWithID(ICipherParameters parameters, byte[] id) : this(parameters, id, 0, id.Length)
		{
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x001279DA File Offset: 0x00125BDA
		public ParametersWithID(ICipherParameters parameters, byte[] id, int idOff, int idLen)
		{
			this.parameters = parameters;
			this.id = Arrays.CopyOfRange(id, idOff, idOff + idLen);
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x001279FA File Offset: 0x00125BFA
		public byte[] GetID()
		{
			return this.id;
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06003087 RID: 12423 RVA: 0x00127A02 File Offset: 0x00125C02
		public ICipherParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x04002017 RID: 8215
		private readonly ICipherParameters parameters;

		// Token: 0x04002018 RID: 8216
		private readonly byte[] id;
	}
}
