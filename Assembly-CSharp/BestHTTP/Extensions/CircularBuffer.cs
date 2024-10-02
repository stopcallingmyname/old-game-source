using System;
using System.Text;

namespace BestHTTP.Extensions
{
	// Token: 0x020007EB RID: 2027
	public sealed class CircularBuffer<T>
	{
		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06004819 RID: 18457 RVA: 0x0019802A File Offset: 0x0019622A
		// (set) Token: 0x0600481A RID: 18458 RVA: 0x00198032 File Offset: 0x00196232
		public int Capacity { get; private set; }

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x0600481B RID: 18459 RVA: 0x0019803B File Offset: 0x0019623B
		// (set) Token: 0x0600481C RID: 18460 RVA: 0x00198043 File Offset: 0x00196243
		public int Count { get; private set; }

		// Token: 0x17000AA8 RID: 2728
		public T this[int idx]
		{
			get
			{
				int num = (this.startIdx + idx) % this.Capacity;
				return this.buffer[num];
			}
			set
			{
				int num = (this.startIdx + idx) % this.Capacity;
				this.buffer[num] = value;
			}
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x001980A2 File Offset: 0x001962A2
		public CircularBuffer(int capacity)
		{
			this.Capacity = capacity;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x001980B4 File Offset: 0x001962B4
		public void Add(T element)
		{
			if (this.buffer == null)
			{
				this.buffer = new T[this.Capacity];
			}
			this.buffer[this.endIdx] = element;
			this.endIdx = (this.endIdx + 1) % this.Capacity;
			if (this.endIdx == this.startIdx)
			{
				this.startIdx = (this.startIdx + 1) % this.Capacity;
			}
			this.Count = Math.Min(this.Count + 1, this.Capacity);
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x00198140 File Offset: 0x00196340
		public void Clear()
		{
			this.startIdx = (this.endIdx = 0);
		}

		// Token: 0x06004822 RID: 18466 RVA: 0x00198160 File Offset: 0x00196360
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder("[");
			int num = this.startIdx;
			while (num != this.endIdx)
			{
				stringBuilder.Append(this.buffer[num].ToString());
				num = (num + 1) % this.Capacity;
				if (num != this.endIdx)
				{
					stringBuilder.Append("; ");
				}
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x04002EFB RID: 12027
		private T[] buffer;

		// Token: 0x04002EFC RID: 12028
		private int startIdx;

		// Token: 0x04002EFD RID: 12029
		private int endIdx;
	}
}
