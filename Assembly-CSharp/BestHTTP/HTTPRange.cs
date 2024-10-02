using System;

namespace BestHTTP
{
	// Token: 0x0200017F RID: 383
	public sealed class HTTPRange
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000D8B RID: 3467 RVA: 0x000931BC File Offset: 0x000913BC
		// (set) Token: 0x06000D8C RID: 3468 RVA: 0x000931C4 File Offset: 0x000913C4
		public int FirstBytePos { get; private set; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000931CD File Offset: 0x000913CD
		// (set) Token: 0x06000D8E RID: 3470 RVA: 0x000931D5 File Offset: 0x000913D5
		public int LastBytePos { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x000931DE File Offset: 0x000913DE
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x000931E6 File Offset: 0x000913E6
		public int ContentLength { get; private set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000D91 RID: 3473 RVA: 0x000931EF File Offset: 0x000913EF
		// (set) Token: 0x06000D92 RID: 3474 RVA: 0x000931F7 File Offset: 0x000913F7
		public bool IsValid { get; private set; }

		// Token: 0x06000D93 RID: 3475 RVA: 0x00093200 File Offset: 0x00091400
		internal HTTPRange()
		{
			this.ContentLength = -1;
			this.IsValid = false;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00093216 File Offset: 0x00091416
		internal HTTPRange(int contentLength)
		{
			this.ContentLength = contentLength;
			this.IsValid = false;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0009322C File Offset: 0x0009142C
		internal HTTPRange(int firstBytePosition, int lastBytePosition, int contentLength)
		{
			this.FirstBytePos = firstBytePosition;
			this.LastBytePos = lastBytePosition;
			this.ContentLength = contentLength;
			this.IsValid = (this.FirstBytePos <= this.LastBytePos && this.ContentLength > this.LastBytePos);
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x0009327C File Offset: 0x0009147C
		public override string ToString()
		{
			return string.Format("{0}-{1}/{2} (valid: {3})", new object[]
			{
				this.FirstBytePos,
				this.LastBytePos,
				this.ContentLength,
				this.IsValid
			});
		}
	}
}
