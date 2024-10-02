using System;
using System.Collections;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x02000283 RID: 643
	public class PemObject : PemObjectGenerator
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x000B832F File Offset: 0x000B652F
		public PemObject(string type, byte[] content) : this(type, Platform.CreateArrayList(), content)
		{
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000B833E File Offset: 0x000B653E
		public PemObject(string type, IList headers, byte[] content)
		{
			this.type = type;
			this.headers = Platform.CreateArrayList(headers);
			this.content = content;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060017A8 RID: 6056 RVA: 0x000B8360 File Offset: 0x000B6560
		public string Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x000B8368 File Offset: 0x000B6568
		public IList Headers
		{
			get
			{
				return this.headers;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x060017AA RID: 6058 RVA: 0x000B8370 File Offset: 0x000B6570
		public byte[] Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000947CE File Offset: 0x000929CE
		public PemObject Generate()
		{
			return this;
		}

		// Token: 0x04001810 RID: 6160
		private string type;

		// Token: 0x04001811 RID: 6161
		private IList headers;

		// Token: 0x04001812 RID: 6162
		private byte[] content;
	}
}
