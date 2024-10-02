using System;

namespace LitJson
{
	// Token: 0x02000153 RID: 339
	public class JsonException : Exception
	{
		// Token: 0x06000BC1 RID: 3009 RVA: 0x0008BF1D File Offset: 0x0008A11D
		public JsonException()
		{
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0008BF25 File Offset: 0x0008A125
		internal JsonException(ParserToken token) : base(string.Format("Invalid token '{0}' in input string", token))
		{
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0008BF3D File Offset: 0x0008A13D
		internal JsonException(ParserToken token, Exception inner_exception) : base(string.Format("Invalid token '{0}' in input string", token), inner_exception)
		{
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0008BF56 File Offset: 0x0008A156
		internal JsonException(int c) : base(string.Format("Invalid character '{0}' in input string", (char)c))
		{
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0008BF6F File Offset: 0x0008A16F
		internal JsonException(int c, Exception inner_exception) : base(string.Format("Invalid character '{0}' in input string", (char)c), inner_exception)
		{
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0008BF89 File Offset: 0x0008A189
		public JsonException(string message) : base(message)
		{
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0008BF92 File Offset: 0x0008A192
		public JsonException(string message, Exception inner_exception) : base(message, inner_exception)
		{
		}
	}
}
