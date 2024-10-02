using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x020004FE RID: 1278
	public class SkeinParameters : ICipherParameters
	{
		// Token: 0x060030B9 RID: 12473 RVA: 0x0012806F File Offset: 0x0012626F
		public SkeinParameters() : this(Platform.CreateHashtable())
		{
		}

		// Token: 0x060030BA RID: 12474 RVA: 0x0012807C File Offset: 0x0012627C
		private SkeinParameters(IDictionary parameters)
		{
			this.parameters = parameters;
		}

		// Token: 0x060030BB RID: 12475 RVA: 0x0012808B File Offset: 0x0012628B
		public IDictionary GetParameters()
		{
			return this.parameters;
		}

		// Token: 0x060030BC RID: 12476 RVA: 0x00128093 File Offset: 0x00126293
		public byte[] GetKey()
		{
			return (byte[])this.parameters[0];
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x001280AB File Offset: 0x001262AB
		public byte[] GetPersonalisation()
		{
			return (byte[])this.parameters[8];
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x001280C3 File Offset: 0x001262C3
		public byte[] GetPublicKey()
		{
			return (byte[])this.parameters[12];
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x001280DC File Offset: 0x001262DC
		public byte[] GetKeyIdentifier()
		{
			return (byte[])this.parameters[16];
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x001280F5 File Offset: 0x001262F5
		public byte[] GetNonce()
		{
			return (byte[])this.parameters[20];
		}

		// Token: 0x04002030 RID: 8240
		public const int PARAM_TYPE_KEY = 0;

		// Token: 0x04002031 RID: 8241
		public const int PARAM_TYPE_CONFIG = 4;

		// Token: 0x04002032 RID: 8242
		public const int PARAM_TYPE_PERSONALISATION = 8;

		// Token: 0x04002033 RID: 8243
		public const int PARAM_TYPE_PUBLIC_KEY = 12;

		// Token: 0x04002034 RID: 8244
		public const int PARAM_TYPE_KEY_IDENTIFIER = 16;

		// Token: 0x04002035 RID: 8245
		public const int PARAM_TYPE_NONCE = 20;

		// Token: 0x04002036 RID: 8246
		public const int PARAM_TYPE_MESSAGE = 48;

		// Token: 0x04002037 RID: 8247
		public const int PARAM_TYPE_OUTPUT = 63;

		// Token: 0x04002038 RID: 8248
		private IDictionary parameters;

		// Token: 0x02000954 RID: 2388
		public class Builder
		{
			// Token: 0x06004F11 RID: 20241 RVA: 0x001B3C58 File Offset: 0x001B1E58
			public Builder()
			{
			}

			// Token: 0x06004F12 RID: 20242 RVA: 0x001B3C6C File Offset: 0x001B1E6C
			public Builder(IDictionary paramsMap)
			{
				foreach (object obj in paramsMap.Keys)
				{
					int num = (int)obj;
					this.parameters.Add(num, paramsMap[num]);
				}
			}

			// Token: 0x06004F13 RID: 20243 RVA: 0x001B3CCC File Offset: 0x001B1ECC
			public Builder(SkeinParameters parameters)
			{
				foreach (object obj in parameters.parameters.Keys)
				{
					int num = (int)obj;
					this.parameters.Add(num, parameters.parameters[num]);
				}
			}

			// Token: 0x06004F14 RID: 20244 RVA: 0x001B3D34 File Offset: 0x001B1F34
			public SkeinParameters.Builder Set(int type, byte[] value)
			{
				if (value == null)
				{
					throw new ArgumentException("Parameter value must not be null.");
				}
				if (type != 0 && (type <= 4 || type >= 63 || type == 48))
				{
					throw new ArgumentException("Parameter types must be in the range 0,5..47,49..62.");
				}
				if (type == 4)
				{
					throw new ArgumentException("Parameter type " + 4 + " is reserved for internal use.");
				}
				this.parameters.Add(type, value);
				return this;
			}

			// Token: 0x06004F15 RID: 20245 RVA: 0x001B3D9D File Offset: 0x001B1F9D
			public SkeinParameters.Builder SetKey(byte[] key)
			{
				return this.Set(0, key);
			}

			// Token: 0x06004F16 RID: 20246 RVA: 0x001B3DA7 File Offset: 0x001B1FA7
			public SkeinParameters.Builder SetPersonalisation(byte[] personalisation)
			{
				return this.Set(8, personalisation);
			}

			// Token: 0x06004F17 RID: 20247 RVA: 0x001B3DB4 File Offset: 0x001B1FB4
			public SkeinParameters.Builder SetPersonalisation(DateTime date, string emailAddress, string distinguisher)
			{
				SkeinParameters.Builder result;
				try
				{
					MemoryStream memoryStream = new MemoryStream();
					StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
					streamWriter.Write(date.ToString("YYYYMMDD", CultureInfo.InvariantCulture));
					streamWriter.Write(" ");
					streamWriter.Write(emailAddress);
					streamWriter.Write(" ");
					streamWriter.Write(distinguisher);
					Platform.Dispose(streamWriter);
					result = this.Set(8, memoryStream.ToArray());
				}
				catch (IOException innerException)
				{
					throw new InvalidOperationException("Byte I/O failed.", innerException);
				}
				return result;
			}

			// Token: 0x06004F18 RID: 20248 RVA: 0x001B3E40 File Offset: 0x001B2040
			public SkeinParameters.Builder SetPublicKey(byte[] publicKey)
			{
				return this.Set(12, publicKey);
			}

			// Token: 0x06004F19 RID: 20249 RVA: 0x001B3E4B File Offset: 0x001B204B
			public SkeinParameters.Builder SetKeyIdentifier(byte[] keyIdentifier)
			{
				return this.Set(16, keyIdentifier);
			}

			// Token: 0x06004F1A RID: 20250 RVA: 0x001B3E56 File Offset: 0x001B2056
			public SkeinParameters.Builder SetNonce(byte[] nonce)
			{
				return this.Set(20, nonce);
			}

			// Token: 0x06004F1B RID: 20251 RVA: 0x001B3E61 File Offset: 0x001B2061
			public SkeinParameters Build()
			{
				return new SkeinParameters(this.parameters);
			}

			// Token: 0x04003636 RID: 13878
			private IDictionary parameters = Platform.CreateHashtable();
		}
	}
}
