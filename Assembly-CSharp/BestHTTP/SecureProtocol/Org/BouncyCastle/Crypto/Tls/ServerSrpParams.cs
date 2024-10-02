using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Math;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000446 RID: 1094
	public class ServerSrpParams
	{
		// Token: 0x06002B1C RID: 11036 RVA: 0x001142DD File Offset: 0x001124DD
		public ServerSrpParams(BigInteger N, BigInteger g, byte[] s, BigInteger B)
		{
			this.m_N = N;
			this.m_g = g;
			this.m_s = Arrays.Clone(s);
			this.m_B = B;
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06002B1D RID: 11037 RVA: 0x00114307 File Offset: 0x00112507
		public virtual BigInteger B
		{
			get
			{
				return this.m_B;
			}
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06002B1E RID: 11038 RVA: 0x0011430F File Offset: 0x0011250F
		public virtual BigInteger G
		{
			get
			{
				return this.m_g;
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x06002B1F RID: 11039 RVA: 0x00114317 File Offset: 0x00112517
		public virtual BigInteger N
		{
			get
			{
				return this.m_N;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x06002B20 RID: 11040 RVA: 0x0011431F File Offset: 0x0011251F
		public virtual byte[] S
		{
			get
			{
				return this.m_s;
			}
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x00114327 File Offset: 0x00112527
		public virtual void Encode(Stream output)
		{
			TlsSrpUtilities.WriteSrpParameter(this.m_N, output);
			TlsSrpUtilities.WriteSrpParameter(this.m_g, output);
			TlsUtilities.WriteOpaque8(this.m_s, output);
			TlsSrpUtilities.WriteSrpParameter(this.m_B, output);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x0011435C File Offset: 0x0011255C
		public static ServerSrpParams Parse(Stream input)
		{
			BigInteger n = TlsSrpUtilities.ReadSrpParameter(input);
			BigInteger g = TlsSrpUtilities.ReadSrpParameter(input);
			byte[] s = TlsUtilities.ReadOpaque8(input);
			BigInteger b = TlsSrpUtilities.ReadSrpParameter(input);
			return new ServerSrpParams(n, g, s, b);
		}

		// Token: 0x04001DD9 RID: 7641
		protected BigInteger m_N;

		// Token: 0x04001DDA RID: 7642
		protected BigInteger m_g;

		// Token: 0x04001DDB RID: 7643
		protected BigInteger m_B;

		// Token: 0x04001DDC RID: 7644
		protected byte[] m_s;
	}
}
