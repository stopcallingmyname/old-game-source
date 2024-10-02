using System;
using System.Collections;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Engines;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Parameters;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Digests
{
	// Token: 0x020005BF RID: 1471
	public class SkeinEngine : IMemoable
	{
		// Token: 0x06003871 RID: 14449 RVA: 0x00161C04 File Offset: 0x0015FE04
		static SkeinEngine()
		{
			SkeinEngine.InitialState(256, 128, new ulong[]
			{
				16217771249220022880UL,
				9817190399063458076UL,
				1155188648486244218UL,
				14769517481627992514UL
			});
			SkeinEngine.InitialState(256, 160, new ulong[]
			{
				1450197650740764312UL,
				3081844928540042640UL,
				15310647011875280446UL,
				3301952811952417661UL
			});
			SkeinEngine.InitialState(256, 224, new ulong[]
			{
				14270089230798940683UL,
				9758551101254474012UL,
				11082101768697755780UL,
				4056579644589979102UL
			});
			SkeinEngine.InitialState(256, 256, new ulong[]
			{
				18202890402666165321UL,
				3443677322885453875UL,
				12915131351309911055UL,
				7662005193972177513UL
			});
			SkeinEngine.InitialState(512, 128, new ulong[]
			{
				12158729379475595090UL,
				2204638249859346602UL,
				3502419045458743507UL,
				13617680570268287068UL,
				983504137758028059UL,
				1880512238245786339UL,
				11730851291495443074UL,
				7602827311880509485UL
			});
			SkeinEngine.InitialState(512, 160, new ulong[]
			{
				2934123928682216849UL,
				14047033351726823311UL,
				1684584802963255058UL,
				5744138295201861711UL,
				2444857010922934358UL,
				15638910433986703544UL,
				13325156239043941114UL,
				118355523173251694UL
			});
			SkeinEngine.InitialState(512, 224, new ulong[]
			{
				14758403053642543652UL,
				14674518637417806319UL,
				10145881904771976036UL,
				4146387520469897396UL,
				1106145742801415120UL,
				7455425944880474941UL,
				11095680972475339753UL,
				11397762726744039159UL
			});
			SkeinEngine.InitialState(512, 384, new ulong[]
			{
				11814849197074935647UL,
				12753905853581818532UL,
				11346781217370868990UL,
				15535391162178797018UL,
				2000907093792408677UL,
				9140007292425499655UL,
				6093301768906360022UL,
				2769176472213098488UL
			});
			SkeinEngine.InitialState(512, 512, new ulong[]
			{
				5261240102383538638UL,
				978932832955457283UL,
				10363226125605772238UL,
				11107378794354519217UL,
				6752626034097301424UL,
				16915020251879818228UL,
				11029617608758768931UL,
				12544957130904423475UL
			});
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x00161D3B File Offset: 0x0015FF3B
		private static void InitialState(int blockSize, int outputSize, ulong[] state)
		{
			SkeinEngine.INITIAL_STATES.Add(SkeinEngine.VariantIdentifier(blockSize / 8, outputSize / 8), state);
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x00161D58 File Offset: 0x0015FF58
		private static int VariantIdentifier(int blockSizeBytes, int outputSizeBytes)
		{
			return outputSizeBytes << 16 | blockSizeBytes;
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x00161D60 File Offset: 0x0015FF60
		public SkeinEngine(int blockSizeBits, int outputSizeBits)
		{
			if (outputSizeBits % 8 != 0)
			{
				throw new ArgumentException("Output size must be a multiple of 8 bits. :" + outputSizeBits);
			}
			this.outputSizeBytes = outputSizeBits / 8;
			this.threefish = new ThreefishEngine(blockSizeBits);
			this.ubi = new SkeinEngine.UBI(this, this.threefish.GetBlockSize());
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x00161DC6 File Offset: 0x0015FFC6
		public SkeinEngine(SkeinEngine engine) : this(engine.BlockSize * 8, engine.OutputSize * 8)
		{
			this.CopyIn(engine);
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x00161DE8 File Offset: 0x0015FFE8
		private void CopyIn(SkeinEngine engine)
		{
			this.ubi.Reset(engine.ubi);
			this.chain = Arrays.Clone(engine.chain, this.chain);
			this.initialState = Arrays.Clone(engine.initialState, this.initialState);
			this.key = Arrays.Clone(engine.key, this.key);
			this.preMessageParameters = SkeinEngine.Clone(engine.preMessageParameters, this.preMessageParameters);
			this.postMessageParameters = SkeinEngine.Clone(engine.postMessageParameters, this.postMessageParameters);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x00161E79 File Offset: 0x00160079
		private static SkeinEngine.Parameter[] Clone(SkeinEngine.Parameter[] data, SkeinEngine.Parameter[] existing)
		{
			if (data == null)
			{
				return null;
			}
			if (existing == null || existing.Length != data.Length)
			{
				existing = new SkeinEngine.Parameter[data.Length];
			}
			Array.Copy(data, 0, existing, 0, existing.Length);
			return existing;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00161EA2 File Offset: 0x001600A2
		public IMemoable Copy()
		{
			return new SkeinEngine(this);
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x00161EAC File Offset: 0x001600AC
		public void Reset(IMemoable other)
		{
			SkeinEngine skeinEngine = (SkeinEngine)other;
			if (this.BlockSize != skeinEngine.BlockSize || this.outputSizeBytes != skeinEngine.outputSizeBytes)
			{
				throw new MemoableResetException("Incompatible parameters in provided SkeinEngine.");
			}
			this.CopyIn(skeinEngine);
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x0600387A RID: 14458 RVA: 0x00161EEE File Offset: 0x001600EE
		public int OutputSize
		{
			get
			{
				return this.outputSizeBytes;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600387B RID: 14459 RVA: 0x00161EF6 File Offset: 0x001600F6
		public int BlockSize
		{
			get
			{
				return this.threefish.GetBlockSize();
			}
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x00161F04 File Offset: 0x00160104
		public void Init(SkeinParameters parameters)
		{
			this.chain = null;
			this.key = null;
			this.preMessageParameters = null;
			this.postMessageParameters = null;
			if (parameters != null)
			{
				if (parameters.GetKey().Length < 16)
				{
					throw new ArgumentException("Skein key must be at least 128 bits.");
				}
				this.InitParams(parameters.GetParameters());
			}
			this.CreateInitialState();
			this.UbiInit(48);
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x00161F64 File Offset: 0x00160164
		private void InitParams(IDictionary parameters)
		{
			IEnumerator enumerator = parameters.Keys.GetEnumerator();
			IList list = Platform.CreateArrayList();
			IList list2 = Platform.CreateArrayList();
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				int num = (int)obj;
				byte[] value = (byte[])parameters[num];
				if (num == 0)
				{
					this.key = value;
				}
				else if (num < 48)
				{
					list.Add(new SkeinEngine.Parameter(num, value));
				}
				else
				{
					list2.Add(new SkeinEngine.Parameter(num, value));
				}
			}
			this.preMessageParameters = new SkeinEngine.Parameter[list.Count];
			list.CopyTo(this.preMessageParameters, 0);
			Array.Sort<SkeinEngine.Parameter>(this.preMessageParameters);
			this.postMessageParameters = new SkeinEngine.Parameter[list2.Count];
			list2.CopyTo(this.postMessageParameters, 0);
			Array.Sort<SkeinEngine.Parameter>(this.postMessageParameters);
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x00162038 File Offset: 0x00160238
		private void CreateInitialState()
		{
			ulong[] array = (ulong[])SkeinEngine.INITIAL_STATES[SkeinEngine.VariantIdentifier(this.BlockSize, this.OutputSize)];
			if (this.key == null && array != null)
			{
				this.chain = Arrays.Clone(array);
			}
			else
			{
				this.chain = new ulong[this.BlockSize / 8];
				if (this.key != null)
				{
					this.UbiComplete(0, this.key);
				}
				this.UbiComplete(4, new SkeinEngine.Configuration((long)(this.outputSizeBytes * 8)).Bytes);
			}
			if (this.preMessageParameters != null)
			{
				for (int i = 0; i < this.preMessageParameters.Length; i++)
				{
					SkeinEngine.Parameter parameter = this.preMessageParameters[i];
					this.UbiComplete(parameter.Type, parameter.Value);
				}
			}
			this.initialState = Arrays.Clone(this.chain);
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0016210D File Offset: 0x0016030D
		public void Reset()
		{
			Array.Copy(this.initialState, 0, this.chain, 0, this.chain.Length);
			this.UbiInit(48);
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x00162132 File Offset: 0x00160332
		private void UbiComplete(int type, byte[] value)
		{
			this.UbiInit(type);
			this.ubi.Update(value, 0, value.Length, this.chain);
			this.UbiFinal();
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x00162157 File Offset: 0x00160357
		private void UbiInit(int type)
		{
			this.ubi.Reset(type);
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x00162165 File Offset: 0x00160365
		private void UbiFinal()
		{
			this.ubi.DoFinal(this.chain);
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x00162178 File Offset: 0x00160378
		private void CheckInitialised()
		{
			if (this.ubi == null)
			{
				throw new ArgumentException("Skein engine is not initialised.");
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0016218D File Offset: 0x0016038D
		public void Update(byte inByte)
		{
			this.singleByte[0] = inByte;
			this.Update(this.singleByte, 0, 1);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x001621A6 File Offset: 0x001603A6
		public void Update(byte[] inBytes, int inOff, int len)
		{
			this.CheckInitialised();
			this.ubi.Update(inBytes, inOff, len, this.chain);
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x001621C4 File Offset: 0x001603C4
		public int DoFinal(byte[] outBytes, int outOff)
		{
			this.CheckInitialised();
			if (outBytes.Length < outOff + this.outputSizeBytes)
			{
				throw new DataLengthException("Output buffer is too short to hold output");
			}
			this.UbiFinal();
			if (this.postMessageParameters != null)
			{
				for (int i = 0; i < this.postMessageParameters.Length; i++)
				{
					SkeinEngine.Parameter parameter = this.postMessageParameters[i];
					this.UbiComplete(parameter.Type, parameter.Value);
				}
			}
			int blockSize = this.BlockSize;
			int num = (this.outputSizeBytes + blockSize - 1) / blockSize;
			for (int j = 0; j < num; j++)
			{
				int outputBytes = Math.Min(blockSize, this.outputSizeBytes - j * blockSize);
				this.Output((ulong)((long)j), outBytes, outOff + j * blockSize, outputBytes);
			}
			this.Reset();
			return this.outputSizeBytes;
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x00162280 File Offset: 0x00160480
		private void Output(ulong outputSequence, byte[] outBytes, int outOff, int outputBytes)
		{
			byte[] array = new byte[8];
			ThreefishEngine.WordToBytes(outputSequence, array, 0);
			ulong[] array2 = new ulong[this.chain.Length];
			this.UbiInit(63);
			this.ubi.Update(array, 0, array.Length, array2);
			this.ubi.DoFinal(array2);
			int num = (outputBytes + 8 - 1) / 8;
			for (int i = 0; i < num; i++)
			{
				int num2 = Math.Min(8, outputBytes - i * 8);
				if (num2 == 8)
				{
					ThreefishEngine.WordToBytes(array2[i], outBytes, outOff + i * 8);
				}
				else
				{
					ThreefishEngine.WordToBytes(array2[i], array, 0);
					Array.Copy(array, 0, outBytes, outOff + i * 8, num2);
				}
			}
		}

		// Token: 0x040024BD RID: 9405
		public const int SKEIN_256 = 256;

		// Token: 0x040024BE RID: 9406
		public const int SKEIN_512 = 512;

		// Token: 0x040024BF RID: 9407
		public const int SKEIN_1024 = 1024;

		// Token: 0x040024C0 RID: 9408
		private const int PARAM_TYPE_KEY = 0;

		// Token: 0x040024C1 RID: 9409
		private const int PARAM_TYPE_CONFIG = 4;

		// Token: 0x040024C2 RID: 9410
		private const int PARAM_TYPE_MESSAGE = 48;

		// Token: 0x040024C3 RID: 9411
		private const int PARAM_TYPE_OUTPUT = 63;

		// Token: 0x040024C4 RID: 9412
		private static readonly IDictionary INITIAL_STATES = Platform.CreateHashtable();

		// Token: 0x040024C5 RID: 9413
		private readonly ThreefishEngine threefish;

		// Token: 0x040024C6 RID: 9414
		private readonly int outputSizeBytes;

		// Token: 0x040024C7 RID: 9415
		private ulong[] chain;

		// Token: 0x040024C8 RID: 9416
		private ulong[] initialState;

		// Token: 0x040024C9 RID: 9417
		private byte[] key;

		// Token: 0x040024CA RID: 9418
		private SkeinEngine.Parameter[] preMessageParameters;

		// Token: 0x040024CB RID: 9419
		private SkeinEngine.Parameter[] postMessageParameters;

		// Token: 0x040024CC RID: 9420
		private readonly SkeinEngine.UBI ubi;

		// Token: 0x040024CD RID: 9421
		private readonly byte[] singleByte = new byte[1];

		// Token: 0x0200097A RID: 2426
		private class Configuration
		{
			// Token: 0x06004F88 RID: 20360 RVA: 0x001B6838 File Offset: 0x001B4A38
			public Configuration(long outputSizeBits)
			{
				this.bytes[0] = 83;
				this.bytes[1] = 72;
				this.bytes[2] = 65;
				this.bytes[3] = 51;
				this.bytes[4] = 1;
				this.bytes[5] = 0;
				ThreefishEngine.WordToBytes((ulong)outputSizeBits, this.bytes, 8);
			}

			// Token: 0x17000C5C RID: 3164
			// (get) Token: 0x06004F89 RID: 20361 RVA: 0x001B689F File Offset: 0x001B4A9F
			public byte[] Bytes
			{
				get
				{
					return this.bytes;
				}
			}

			// Token: 0x040036CD RID: 14029
			private byte[] bytes = new byte[32];
		}

		// Token: 0x0200097B RID: 2427
		public class Parameter
		{
			// Token: 0x06004F8A RID: 20362 RVA: 0x001B68A7 File Offset: 0x001B4AA7
			public Parameter(int type, byte[] value)
			{
				this.type = type;
				this.value = value;
			}

			// Token: 0x17000C5D RID: 3165
			// (get) Token: 0x06004F8B RID: 20363 RVA: 0x001B68BD File Offset: 0x001B4ABD
			public int Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x17000C5E RID: 3166
			// (get) Token: 0x06004F8C RID: 20364 RVA: 0x001B68C5 File Offset: 0x001B4AC5
			public byte[] Value
			{
				get
				{
					return this.value;
				}
			}

			// Token: 0x040036CE RID: 14030
			private int type;

			// Token: 0x040036CF RID: 14031
			private byte[] value;
		}

		// Token: 0x0200097C RID: 2428
		private class UbiTweak
		{
			// Token: 0x06004F8D RID: 20365 RVA: 0x001B68CD File Offset: 0x001B4ACD
			public UbiTweak()
			{
				this.Reset();
			}

			// Token: 0x06004F8E RID: 20366 RVA: 0x001B68E7 File Offset: 0x001B4AE7
			public void Reset(SkeinEngine.UbiTweak tweak)
			{
				this.tweak = Arrays.Clone(tweak.tweak, this.tweak);
				this.extendedPosition = tweak.extendedPosition;
			}

			// Token: 0x06004F8F RID: 20367 RVA: 0x001B690C File Offset: 0x001B4B0C
			public void Reset()
			{
				this.tweak[0] = 0UL;
				this.tweak[1] = 0UL;
				this.extendedPosition = false;
				this.First = true;
			}

			// Token: 0x17000C5F RID: 3167
			// (get) Token: 0x06004F90 RID: 20368 RVA: 0x001B6930 File Offset: 0x001B4B30
			// (set) Token: 0x06004F91 RID: 20369 RVA: 0x001B6942 File Offset: 0x001B4B42
			public uint Type
			{
				get
				{
					return (uint)(this.tweak[1] >> 56 & 63UL);
				}
				set
				{
					this.tweak[1] = ((this.tweak[1] & 18446743798831644672UL) | ((ulong)value & 63UL) << 56);
				}
			}

			// Token: 0x17000C60 RID: 3168
			// (get) Token: 0x06004F92 RID: 20370 RVA: 0x001B6968 File Offset: 0x001B4B68
			// (set) Token: 0x06004F93 RID: 20371 RVA: 0x001B6980 File Offset: 0x001B4B80
			public bool First
			{
				get
				{
					return (this.tweak[1] & 4611686018427387904UL) > 0UL;
				}
				set
				{
					if (value)
					{
						this.tweak[1] |= 4611686018427387904UL;
						return;
					}
					this.tweak[1] &= 13835058055282163711UL;
				}
			}

			// Token: 0x17000C61 RID: 3169
			// (get) Token: 0x06004F94 RID: 20372 RVA: 0x001B69B8 File Offset: 0x001B4BB8
			// (set) Token: 0x06004F95 RID: 20373 RVA: 0x001B69D0 File Offset: 0x001B4BD0
			public bool Final
			{
				get
				{
					return (this.tweak[1] & 9223372036854775808UL) > 0UL;
				}
				set
				{
					if (value)
					{
						this.tweak[1] |= 9223372036854775808UL;
						return;
					}
					this.tweak[1] &= 9223372036854775807UL;
				}
			}

			// Token: 0x06004F96 RID: 20374 RVA: 0x001B6A08 File Offset: 0x001B4C08
			public void AdvancePosition(int advance)
			{
				if (this.extendedPosition)
				{
					ulong[] array = new ulong[]
					{
						this.tweak[0] & (ulong)-1,
						this.tweak[0] >> 32 & (ulong)-1,
						this.tweak[1] & (ulong)-1
					};
					ulong num = (ulong)((long)advance);
					for (int i = 0; i < array.Length; i++)
					{
						num += array[i];
						array[i] = num;
						num >>= 32;
					}
					this.tweak[0] = ((array[1] & (ulong)-1) << 32 | (array[0] & (ulong)-1));
					this.tweak[1] = ((this.tweak[1] & 18446744069414584320UL) | (array[2] & (ulong)-1));
					return;
				}
				ulong num2 = this.tweak[0];
				num2 += (ulong)advance;
				this.tweak[0] = num2;
				if (num2 > 18446744069414584320UL)
				{
					this.extendedPosition = true;
				}
			}

			// Token: 0x06004F97 RID: 20375 RVA: 0x001B6AD8 File Offset: 0x001B4CD8
			public ulong[] GetWords()
			{
				return this.tweak;
			}

			// Token: 0x06004F98 RID: 20376 RVA: 0x001B6AE0 File Offset: 0x001B4CE0
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					this.Type,
					" first: ",
					this.First.ToString(),
					", final: ",
					this.Final.ToString()
				});
			}

			// Token: 0x040036D0 RID: 14032
			private const ulong LOW_RANGE = 18446744069414584320UL;

			// Token: 0x040036D1 RID: 14033
			private const ulong T1_FINAL = 9223372036854775808UL;

			// Token: 0x040036D2 RID: 14034
			private const ulong T1_FIRST = 4611686018427387904UL;

			// Token: 0x040036D3 RID: 14035
			private ulong[] tweak = new ulong[2];

			// Token: 0x040036D4 RID: 14036
			private bool extendedPosition;
		}

		// Token: 0x0200097D RID: 2429
		private class UBI
		{
			// Token: 0x06004F99 RID: 20377 RVA: 0x001B6B38 File Offset: 0x001B4D38
			public UBI(SkeinEngine engine, int blockSize)
			{
				this.engine = engine;
				this.currentBlock = new byte[blockSize];
				this.message = new ulong[this.currentBlock.Length / 8];
			}

			// Token: 0x06004F9A RID: 20378 RVA: 0x001B6B74 File Offset: 0x001B4D74
			public void Reset(SkeinEngine.UBI ubi)
			{
				this.currentBlock = Arrays.Clone(ubi.currentBlock, this.currentBlock);
				this.currentOffset = ubi.currentOffset;
				this.message = Arrays.Clone(ubi.message, this.message);
				this.tweak.Reset(ubi.tweak);
			}

			// Token: 0x06004F9B RID: 20379 RVA: 0x001B6BCC File Offset: 0x001B4DCC
			public void Reset(int type)
			{
				this.tweak.Reset();
				this.tweak.Type = (uint)type;
				this.currentOffset = 0;
			}

			// Token: 0x06004F9C RID: 20380 RVA: 0x001B6BEC File Offset: 0x001B4DEC
			public void Update(byte[] value, int offset, int len, ulong[] output)
			{
				int num = 0;
				while (len > num)
				{
					if (this.currentOffset == this.currentBlock.Length)
					{
						this.ProcessBlock(output);
						this.tweak.First = false;
						this.currentOffset = 0;
					}
					int num2 = Math.Min(len - num, this.currentBlock.Length - this.currentOffset);
					Array.Copy(value, offset + num, this.currentBlock, this.currentOffset, num2);
					num += num2;
					this.currentOffset += num2;
					this.tweak.AdvancePosition(num2);
				}
			}

			// Token: 0x06004F9D RID: 20381 RVA: 0x001B6C78 File Offset: 0x001B4E78
			private void ProcessBlock(ulong[] output)
			{
				this.engine.threefish.Init(true, this.engine.chain, this.tweak.GetWords());
				for (int i = 0; i < this.message.Length; i++)
				{
					this.message[i] = ThreefishEngine.BytesToWord(this.currentBlock, i * 8);
				}
				this.engine.threefish.ProcessBlock(this.message, output);
				for (int j = 0; j < output.Length; j++)
				{
					output[j] ^= this.message[j];
				}
			}

			// Token: 0x06004F9E RID: 20382 RVA: 0x001B6D10 File Offset: 0x001B4F10
			public void DoFinal(ulong[] output)
			{
				for (int i = this.currentOffset; i < this.currentBlock.Length; i++)
				{
					this.currentBlock[i] = 0;
				}
				this.tweak.Final = true;
				this.ProcessBlock(output);
			}

			// Token: 0x040036D5 RID: 14037
			private readonly SkeinEngine.UbiTweak tweak = new SkeinEngine.UbiTweak();

			// Token: 0x040036D6 RID: 14038
			private readonly SkeinEngine engine;

			// Token: 0x040036D7 RID: 14039
			private byte[] currentBlock;

			// Token: 0x040036D8 RID: 14040
			private int currentOffset;

			// Token: 0x040036D9 RID: 14041
			private ulong[] message;
		}
	}
}
