using System;
using System.IO;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Security;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.IO.Pem;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.OpenSsl
{
	// Token: 0x020002D5 RID: 725
	public class PemWriter : PemWriter
	{
		// Token: 0x06001A95 RID: 6805 RVA: 0x000C7B22 File Offset: 0x000C5D22
		public PemWriter(TextWriter writer) : base(writer)
		{
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x000C7B2C File Offset: 0x000C5D2C
		public void WriteObject(object obj)
		{
			try
			{
				base.WriteObject(new MiscPemGenerator(obj));
			}
			catch (PemGenerationException ex)
			{
				if (ex.InnerException is IOException)
				{
					throw (IOException)ex.InnerException;
				}
				throw ex;
			}
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x000C7B74 File Offset: 0x000C5D74
		public void WriteObject(object obj, string algorithm, char[] password, SecureRandom random)
		{
			base.WriteObject(new MiscPemGenerator(obj, algorithm, password, random));
		}
	}
}
