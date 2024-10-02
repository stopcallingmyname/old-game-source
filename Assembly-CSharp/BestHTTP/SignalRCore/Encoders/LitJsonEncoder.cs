using System;
using LitJson;

namespace BestHTTP.SignalRCore.Encoders
{
	// Token: 0x020001EE RID: 494
	public sealed class LitJsonEncoder : IEncoder
	{
		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06001252 RID: 4690 RVA: 0x000A470E File Offset: 0x000A290E
		public string Name
		{
			get
			{
				return "json";
			}
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x000A4718 File Offset: 0x000A2918
		public LitJsonEncoder()
		{
			JsonMapper.RegisterImporter<int, long>((int input) => (long)input);
			JsonMapper.RegisterImporter<long, int>((long input) => (int)input);
			JsonMapper.RegisterImporter<double, int>((double input) => (int)(input + 0.5));
			JsonMapper.RegisterImporter<string, DateTime>((string input) => Convert.ToDateTime(input).ToUniversalTime());
			JsonMapper.RegisterImporter<double, float>((double input) => (float)input);
			JsonMapper.RegisterImporter<string, byte[]>((string input) => Convert.FromBase64String(input));
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000A4803 File Offset: 0x000A2A03
		public T DecodeAs<T>(string text)
		{
			return JsonMapper.ToObject<T>(text);
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000947C7 File Offset: 0x000929C7
		public T DecodeAs<T>(byte[] data)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000947C7 File Offset: 0x000929C7
		public byte[] EncodeAsBinary<T>(T value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x000A480B File Offset: 0x000A2A0B
		public string EncodeAsText<T>(T value)
		{
			return JsonMapper.ToJson(value);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x000A4818 File Offset: 0x000A2A18
		public object ConvertTo(Type toType, object obj)
		{
			string json = JsonMapper.ToJson(obj);
			return JsonMapper.ToObject(toType, json);
		}
	}
}
