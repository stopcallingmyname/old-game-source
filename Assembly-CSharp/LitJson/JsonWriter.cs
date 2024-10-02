using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace LitJson
{
	// Token: 0x02000162 RID: 354
	public sealed class JsonWriter
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x0008DDF4 File Offset: 0x0008BFF4
		// (set) Token: 0x06000C54 RID: 3156 RVA: 0x0008DDFC File Offset: 0x0008BFFC
		public int IndentValue
		{
			get
			{
				return this.indent_value;
			}
			set
			{
				this.indentation = this.indentation / this.indent_value * value;
				this.indent_value = value;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x0008DE1A File Offset: 0x0008C01A
		// (set) Token: 0x06000C56 RID: 3158 RVA: 0x0008DE22 File Offset: 0x0008C022
		public bool PrettyPrint
		{
			get
			{
				return this.pretty_print;
			}
			set
			{
				this.pretty_print = value;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x0008DE2B File Offset: 0x0008C02B
		public TextWriter TextWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0008DE33 File Offset: 0x0008C033
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0008DE3B File Offset: 0x0008C03B
		public bool Validate
		{
			get
			{
				return this.validate;
			}
			set
			{
				this.validate = value;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0008DE50 File Offset: 0x0008C050
		public JsonWriter()
		{
			this.inst_string_builder = new StringBuilder();
			this.writer = new StringWriter(this.inst_string_builder);
			this.Init();
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x0008DE7A File Offset: 0x0008C07A
		public JsonWriter(StringBuilder sb) : this(new StringWriter(sb))
		{
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x0008DE88 File Offset: 0x0008C088
		public JsonWriter(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			this.writer = writer;
			this.Init();
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x0008DEAC File Offset: 0x0008C0AC
		private void DoValidation(Condition cond)
		{
			if (!this.context.ExpectingValue)
			{
				this.context.Count++;
			}
			if (!this.validate)
			{
				return;
			}
			if (this.has_reached_end)
			{
				throw new JsonException("A complete JSON symbol has already been written");
			}
			switch (cond)
			{
			case Condition.InArray:
				if (!this.context.InArray)
				{
					throw new JsonException("Can't close an array here");
				}
				break;
			case Condition.InObject:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't close an object here");
				}
				break;
			case Condition.NotAProperty:
				if (this.context.InObject && !this.context.ExpectingValue)
				{
					throw new JsonException("Expected a property");
				}
				break;
			case Condition.Property:
				if (!this.context.InObject || this.context.ExpectingValue)
				{
					throw new JsonException("Can't add a property here");
				}
				break;
			case Condition.Value:
				if (!this.context.InArray && (!this.context.InObject || !this.context.ExpectingValue))
				{
					throw new JsonException("Can't add a value here");
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0008DFD0 File Offset: 0x0008C1D0
		private void Init()
		{
			this.has_reached_end = false;
			this.hex_seq = new char[4];
			this.indentation = 0;
			this.indent_value = 4;
			this.pretty_print = false;
			this.validate = true;
			this.ctx_stack = new Stack<WriterContext>();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0008E034 File Offset: 0x0008C234
		private static void IntToHex(int n, char[] hex)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = n % 16;
				if (num < 10)
				{
					hex[3 - i] = (char)(48 + num);
				}
				else
				{
					hex[3 - i] = (char)(65 + (num - 10));
				}
				n >>= 4;
			}
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0008E075 File Offset: 0x0008C275
		private void Indent()
		{
			if (this.pretty_print)
			{
				this.indentation += this.indent_value;
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0008E094 File Offset: 0x0008C294
		private void Put(string str)
		{
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				for (int i = 0; i < this.indentation; i++)
				{
					this.writer.Write(' ');
				}
			}
			this.writer.Write(str);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0008E0E0 File Offset: 0x0008C2E0
		private void PutNewline()
		{
			this.PutNewline(true);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0008E0EC File Offset: 0x0008C2EC
		private void PutNewline(bool add_comma)
		{
			if (add_comma && !this.context.ExpectingValue && this.context.Count > 1)
			{
				this.writer.Write(',');
			}
			if (this.pretty_print && !this.context.ExpectingValue)
			{
				this.writer.Write('\n');
			}
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0008E148 File Offset: 0x0008C348
		private void PutString(string str)
		{
			this.Put(string.Empty);
			this.writer.Write('"');
			int length = str.Length;
			int i = 0;
			while (i < length)
			{
				char c = str[i];
				switch (c)
				{
				case '\b':
					this.writer.Write("\\b");
					break;
				case '\t':
					this.writer.Write("\\t");
					break;
				case '\n':
					this.writer.Write("\\n");
					break;
				case '\v':
					goto IL_E4;
				case '\f':
					this.writer.Write("\\f");
					break;
				case '\r':
					this.writer.Write("\\r");
					break;
				default:
					if (c != '"' && c != '\\')
					{
						goto IL_E4;
					}
					this.writer.Write('\\');
					this.writer.Write(str[i]);
					break;
				}
				IL_141:
				i++;
				continue;
				IL_E4:
				if (str[i] >= ' ' && str[i] <= '~')
				{
					this.writer.Write(str[i]);
					goto IL_141;
				}
				JsonWriter.IntToHex((int)str[i], this.hex_seq);
				this.writer.Write("\\u");
				this.writer.Write(this.hex_seq);
				goto IL_141;
			}
			this.writer.Write('"');
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0008E2AE File Offset: 0x0008C4AE
		private void Unindent()
		{
			if (this.pretty_print)
			{
				this.indentation -= this.indent_value;
			}
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0008E2CB File Offset: 0x0008C4CB
		public override string ToString()
		{
			if (this.inst_string_builder == null)
			{
				return string.Empty;
			}
			return this.inst_string_builder.ToString();
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0008E2E8 File Offset: 0x0008C4E8
		public void Reset()
		{
			this.has_reached_end = false;
			this.ctx_stack.Clear();
			this.context = new WriterContext();
			this.ctx_stack.Push(this.context);
			if (this.inst_string_builder != null)
			{
				this.inst_string_builder.Remove(0, this.inst_string_builder.Length);
			}
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0008E343 File Offset: 0x0008C543
		public void Write(bool boolean)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(boolean ? "true" : "false");
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0008E373 File Offset: 0x0008C573
		public void Write(decimal number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0008E3A0 File Offset: 0x0008C5A0
		public void Write(double number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			string text = Convert.ToString(number, JsonWriter.number_format);
			this.Put(text);
			if (text.IndexOf('.') == -1 && text.IndexOf('E') == -1)
			{
				this.writer.Write(".0");
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0008E3FF File Offset: 0x0008C5FF
		public void Write(int number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0008E42B File Offset: 0x0008C62B
		public void Write(long number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0008E457 File Offset: 0x0008C657
		public void Write(string str)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			if (str == null)
			{
				this.Put("null");
			}
			else
			{
				this.PutString(str);
			}
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0008E489 File Offset: 0x0008C689
		public void Write(ulong number)
		{
			this.DoValidation(Condition.Value);
			this.PutNewline();
			this.Put(Convert.ToString(number, JsonWriter.number_format));
			this.context.ExpectingValue = false;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0008E4B8 File Offset: 0x0008C6B8
		public void WriteArrayEnd()
		{
			this.DoValidation(Condition.InArray);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("]");
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0008E524 File Offset: 0x0008C724
		public void WriteArrayStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("[");
			this.context = new WriterContext();
			this.context.InArray = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0008E578 File Offset: 0x0008C778
		public void WriteObjectEnd()
		{
			this.DoValidation(Condition.InObject);
			this.PutNewline(false);
			this.ctx_stack.Pop();
			if (this.ctx_stack.Count == 1)
			{
				this.has_reached_end = true;
			}
			else
			{
				this.context = this.ctx_stack.Peek();
				this.context.ExpectingValue = false;
			}
			this.Unindent();
			this.Put("}");
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0008E5E4 File Offset: 0x0008C7E4
		public void WriteObjectStart()
		{
			this.DoValidation(Condition.NotAProperty);
			this.PutNewline();
			this.Put("{");
			this.context = new WriterContext();
			this.context.InObject = true;
			this.ctx_stack.Push(this.context);
			this.Indent();
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0008E638 File Offset: 0x0008C838
		public void WritePropertyName(string property_name)
		{
			this.DoValidation(Condition.Property);
			this.PutNewline();
			this.PutString(property_name);
			if (this.pretty_print)
			{
				if (property_name.Length > this.context.Padding)
				{
					this.context.Padding = property_name.Length;
				}
				for (int i = this.context.Padding - property_name.Length; i >= 0; i--)
				{
					this.writer.Write(' ');
				}
				this.writer.Write(": ");
			}
			else
			{
				this.writer.Write(':');
			}
			this.context.ExpectingValue = true;
		}

		// Token: 0x04001247 RID: 4679
		private static NumberFormatInfo number_format = NumberFormatInfo.InvariantInfo;

		// Token: 0x04001248 RID: 4680
		private WriterContext context;

		// Token: 0x04001249 RID: 4681
		private Stack<WriterContext> ctx_stack;

		// Token: 0x0400124A RID: 4682
		private bool has_reached_end;

		// Token: 0x0400124B RID: 4683
		private char[] hex_seq;

		// Token: 0x0400124C RID: 4684
		private int indentation;

		// Token: 0x0400124D RID: 4685
		private int indent_value;

		// Token: 0x0400124E RID: 4686
		private StringBuilder inst_string_builder;

		// Token: 0x0400124F RID: 4687
		private bool pretty_print;

		// Token: 0x04001250 RID: 4688
		private bool validate;

		// Token: 0x04001251 RID: 4689
		private TextWriter writer;
	}
}
