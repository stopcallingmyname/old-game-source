using System;
using System.Collections.Generic;
using System.Threading;

namespace BestHTTP.Futures
{
	// Token: 0x020007E9 RID: 2025
	public class Future<T> : IFuture<T>
	{
		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x060047E6 RID: 18406 RVA: 0x00197412 File Offset: 0x00195612
		public FutureState state
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x060047E7 RID: 18407 RVA: 0x0019741C File Offset: 0x0019561C
		public T value
		{
			get
			{
				if (this._state != FutureState.Success && this._state != FutureState.Processing)
				{
					throw new InvalidOperationException("value is not available unless state is Success or Processing.");
				}
				return this._value;
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x060047E8 RID: 18408 RVA: 0x00197445 File Offset: 0x00195645
		public Exception error
		{
			get
			{
				if (this._state != FutureState.Error)
				{
					throw new InvalidOperationException("error is not available unless state is Error.");
				}
				return this._error;
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x00197463 File Offset: 0x00195663
		public Future()
		{
			this._state = FutureState.Pending;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x001974A0 File Offset: 0x001956A0
		public IFuture<T> OnItem(FutureValueCallback<T> callback)
		{
			if (this._state < FutureState.Success && !this._itemCallbacks.Contains(callback))
			{
				this._itemCallbacks.Add(callback);
			}
			return this;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x001974C8 File Offset: 0x001956C8
		public IFuture<T> OnSuccess(FutureValueCallback<T> callback)
		{
			if (this._state == FutureState.Success)
			{
				callback(this.value);
			}
			else if (this._state != FutureState.Error && !this._successCallbacks.Contains(callback))
			{
				this._successCallbacks.Add(callback);
			}
			return this;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x00197514 File Offset: 0x00195714
		public IFuture<T> OnError(FutureErrorCallback callback)
		{
			if (this._state == FutureState.Error)
			{
				callback(this.error);
			}
			else if (this._state != FutureState.Success && !this._errorCallbacks.Contains(callback))
			{
				this._errorCallbacks.Add(callback);
			}
			return this;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x00197560 File Offset: 0x00195760
		public IFuture<T> OnComplete(FutureCallback<T> callback)
		{
			if (this._state == FutureState.Success || this._state == FutureState.Error)
			{
				callback(this);
			}
			else if (!this._complationCallbacks.Contains(callback))
			{
				this._complationCallbacks.Add(callback);
			}
			return this;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0019759C File Offset: 0x0019579C
		public IFuture<T> Process(Func<T> func)
		{
			if (this._state != FutureState.Pending)
			{
				throw new InvalidOperationException("Cannot process a future that isn't in the Pending state.");
			}
			this.BeginProcess(default(T));
			this._processFunc = func;
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
			return this;
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x001975E8 File Offset: 0x001957E8
		private void ThreadFunc(object param)
		{
			try
			{
				this.AssignImpl(this._processFunc());
			}
			catch (Exception error)
			{
				this.FailImpl(error);
			}
			finally
			{
				this._processFunc = null;
			}
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00197638 File Offset: 0x00195838
		public void Assign(T value)
		{
			if (this._state != FutureState.Pending && this._state != FutureState.Processing)
			{
				throw new InvalidOperationException("Cannot assign a value to a future that isn't in the Pending or Processing state.");
			}
			this.AssignImpl(value);
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x00197661 File Offset: 0x00195861
		public void BeginProcess(T initialItem = default(T))
		{
			this._state = FutureState.Processing;
			this._value = initialItem;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x00197674 File Offset: 0x00195874
		public void AssignItem(T value)
		{
			this._value = value;
			this._error = null;
			foreach (FutureValueCallback<T> futureValueCallback in this._itemCallbacks)
			{
				futureValueCallback(this.value);
			}
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x001976D8 File Offset: 0x001958D8
		public void Fail(Exception error)
		{
			if (this._state != FutureState.Pending && this._state != FutureState.Processing)
			{
				throw new InvalidOperationException("Cannot fail future that isn't in the Pending or Processing state.");
			}
			this.FailImpl(error);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x00197701 File Offset: 0x00195901
		private void AssignImpl(T value)
		{
			this._value = value;
			this._error = null;
			this._state = FutureState.Success;
			this.FlushSuccessCallbacks();
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x00197720 File Offset: 0x00195920
		private void FailImpl(Exception error)
		{
			this._value = default(T);
			this._error = error;
			this._state = FutureState.Error;
			this.FlushErrorCallbacks();
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x00197744 File Offset: 0x00195944
		private void FlushSuccessCallbacks()
		{
			foreach (FutureValueCallback<T> futureValueCallback in this._successCallbacks)
			{
				futureValueCallback(this.value);
			}
			this.FlushComplationCallbacks();
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x001977A0 File Offset: 0x001959A0
		private void FlushErrorCallbacks()
		{
			foreach (FutureErrorCallback futureErrorCallback in this._errorCallbacks)
			{
				futureErrorCallback(this.error);
			}
			this.FlushComplationCallbacks();
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x001977FC File Offset: 0x001959FC
		private void FlushComplationCallbacks()
		{
			foreach (FutureCallback<T> futureCallback in this._complationCallbacks)
			{
				futureCallback(this);
			}
			this.ClearCallbacks();
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x00197854 File Offset: 0x00195A54
		private void ClearCallbacks()
		{
			this._itemCallbacks.Clear();
			this._successCallbacks.Clear();
			this._errorCallbacks.Clear();
			this._complationCallbacks.Clear();
		}

		// Token: 0x04002EE7 RID: 12007
		private volatile FutureState _state;

		// Token: 0x04002EE8 RID: 12008
		private T _value;

		// Token: 0x04002EE9 RID: 12009
		private Exception _error;

		// Token: 0x04002EEA RID: 12010
		private Func<T> _processFunc;

		// Token: 0x04002EEB RID: 12011
		private readonly List<FutureValueCallback<T>> _itemCallbacks = new List<FutureValueCallback<T>>();

		// Token: 0x04002EEC RID: 12012
		private readonly List<FutureValueCallback<T>> _successCallbacks = new List<FutureValueCallback<T>>();

		// Token: 0x04002EED RID: 12013
		private readonly List<FutureErrorCallback> _errorCallbacks = new List<FutureErrorCallback>();

		// Token: 0x04002EEE RID: 12014
		private readonly List<FutureCallback<T>> _complationCallbacks = new List<FutureCallback<T>>();
	}
}
