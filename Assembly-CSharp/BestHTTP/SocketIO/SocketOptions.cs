using System;
using System.Collections.Generic;
using System.Text;
using BestHTTP.SocketIO.Transports;
using PlatformSupport.Collections.ObjectModel;
using PlatformSupport.Collections.Specialized;

namespace BestHTTP.SocketIO
{
	// Token: 0x020001CC RID: 460
	public sealed class SocketOptions
	{
		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600113E RID: 4414 RVA: 0x000A0ECC File Offset: 0x0009F0CC
		// (set) Token: 0x0600113F RID: 4415 RVA: 0x000A0ED4 File Offset: 0x0009F0D4
		public TransportTypes ConnectWith { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x000A0EDD File Offset: 0x0009F0DD
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x000A0EE5 File Offset: 0x0009F0E5
		public bool Reconnection { get; set; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06001142 RID: 4418 RVA: 0x000A0EEE File Offset: 0x0009F0EE
		// (set) Token: 0x06001143 RID: 4419 RVA: 0x000A0EF6 File Offset: 0x0009F0F6
		public int ReconnectionAttempts { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06001144 RID: 4420 RVA: 0x000A0EFF File Offset: 0x0009F0FF
		// (set) Token: 0x06001145 RID: 4421 RVA: 0x000A0F07 File Offset: 0x0009F107
		public TimeSpan ReconnectionDelay { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x000A0F10 File Offset: 0x0009F110
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x000A0F18 File Offset: 0x0009F118
		public TimeSpan ReconnectionDelayMax { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x000A0F21 File Offset: 0x0009F121
		// (set) Token: 0x06001149 RID: 4425 RVA: 0x000A0F29 File Offset: 0x0009F129
		public float RandomizationFactor
		{
			get
			{
				return this.randomizationFactor;
			}
			set
			{
				this.randomizationFactor = Math.Min(1f, Math.Max(0f, value));
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600114A RID: 4426 RVA: 0x000A0F46 File Offset: 0x0009F146
		// (set) Token: 0x0600114B RID: 4427 RVA: 0x000A0F4E File Offset: 0x0009F14E
		public TimeSpan Timeout { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600114C RID: 4428 RVA: 0x000A0F57 File Offset: 0x0009F157
		// (set) Token: 0x0600114D RID: 4429 RVA: 0x000A0F5F File Offset: 0x0009F15F
		public bool AutoConnect { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x000A0F68 File Offset: 0x0009F168
		// (set) Token: 0x0600114F RID: 4431 RVA: 0x000A0F70 File Offset: 0x0009F170
		public ObservableDictionary<string, string> AdditionalQueryParams
		{
			get
			{
				return this.additionalQueryParams;
			}
			set
			{
				if (this.additionalQueryParams != null)
				{
					this.additionalQueryParams.CollectionChanged -= this.AdditionalQueryParams_CollectionChanged;
				}
				this.additionalQueryParams = value;
				this.BuiltQueryParams = null;
				if (value != null)
				{
					value.CollectionChanged += this.AdditionalQueryParams_CollectionChanged;
				}
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x000A0FBF File Offset: 0x0009F1BF
		// (set) Token: 0x06001151 RID: 4433 RVA: 0x000A0FC7 File Offset: 0x0009F1C7
		public bool QueryParamsOnlyForHandshake { get; set; }

		// Token: 0x06001152 RID: 4434 RVA: 0x000A0FD0 File Offset: 0x0009F1D0
		public SocketOptions()
		{
			this.ConnectWith = TransportTypes.Polling;
			this.Reconnection = true;
			this.ReconnectionAttempts = int.MaxValue;
			this.ReconnectionDelay = TimeSpan.FromMilliseconds(1000.0);
			this.ReconnectionDelayMax = TimeSpan.FromMilliseconds(5000.0);
			this.RandomizationFactor = 0.5f;
			this.Timeout = TimeSpan.FromMilliseconds(20000.0);
			this.AutoConnect = true;
			this.QueryParamsOnlyForHandshake = true;
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000A1054 File Offset: 0x0009F254
		internal string BuildQueryParams()
		{
			if (this.AdditionalQueryParams == null || this.AdditionalQueryParams.Count == 0)
			{
				return string.Empty;
			}
			if (!string.IsNullOrEmpty(this.BuiltQueryParams))
			{
				return this.BuiltQueryParams;
			}
			StringBuilder stringBuilder = new StringBuilder(this.AdditionalQueryParams.Count * 4);
			foreach (KeyValuePair<string, string> keyValuePair in this.AdditionalQueryParams)
			{
				stringBuilder.Append("&");
				stringBuilder.Append(keyValuePair.Key);
				if (!string.IsNullOrEmpty(keyValuePair.Value))
				{
					stringBuilder.Append("=");
					stringBuilder.Append(keyValuePair.Value);
				}
			}
			return this.BuiltQueryParams = stringBuilder.ToString();
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000A1130 File Offset: 0x0009F330
		private void AdditionalQueryParams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.BuiltQueryParams = null;
		}

		// Token: 0x040014B6 RID: 5302
		private float randomizationFactor;

		// Token: 0x040014B9 RID: 5305
		private ObservableDictionary<string, string> additionalQueryParams;

		// Token: 0x040014BB RID: 5307
		private string BuiltQueryParams;
	}
}
