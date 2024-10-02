using System;
using System.Collections;

namespace PlatformSupport.Collections.Specialized
{
	// Token: 0x0200016E RID: 366
	public class NotifyCollectionChangedEventArgs : EventArgs
	{
		// Token: 0x06000CB1 RID: 3249 RVA: 0x0008F9F2 File Offset: 0x0008DBF2
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
		{
			if (action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0008FA20 File Offset: 0x0008DC20
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[]
				{
					changedItem
				}, -1);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0008FA88 File Offset: 0x0008DC88
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action != NotifyCollectionChangedAction.Reset)
			{
				this.InitializeAddOrRemove(action, new object[]
				{
					changedItem
				}, index);
				return;
			}
			if (changedItem != null)
			{
				throw new ArgumentException("action");
			}
			if (index != -1)
			{
				throw new ArgumentException("action");
			}
			this.InitializeAdd(action, null, -1);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0008FAFC File Offset: 0x0008DCFC
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException("action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				this.InitializeAddOrRemove(action, changedItems, -1);
				return;
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0008FB68 File Offset: 0x0008DD68
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Add && action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
			{
				throw new ArgumentException("action");
			}
			if (action == NotifyCollectionChangedAction.Reset)
			{
				if (changedItems != null)
				{
					throw new ArgumentException("action");
				}
				if (startingIndex != -1)
				{
					throw new ArgumentException("action");
				}
				this.InitializeAdd(action, null, -1);
				return;
			}
			else
			{
				if (changedItems == null)
				{
					throw new ArgumentNullException("changedItems");
				}
				if (startingIndex < -1)
				{
					throw new ArgumentException("startingIndex");
				}
				this.InitializeAddOrRemove(action, changedItems, startingIndex);
				return;
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0008FBF0 File Offset: 0x0008DDF0
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			this.InitializeMoveOrReplace(action, new object[]
			{
				newItem
			}, new object[]
			{
				oldItem
			}, -1, -1);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0008FC40 File Offset: 0x0008DE40
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			this.InitializeMoveOrReplace(action, new object[]
			{
				newItem
			}, new object[]
			{
				oldItem
			}, index, index);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0008FC90 File Offset: 0x0008DE90
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, -1, -1);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0008FCE8 File Offset: 0x0008DEE8
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
		{
			if (action != NotifyCollectionChangedAction.Replace)
			{
				throw new ArgumentException("action");
			}
			if (newItems == null)
			{
				throw new ArgumentNullException("newItems");
			}
			if (oldItems == null)
			{
				throw new ArgumentNullException("oldItems");
			}
			this.InitializeMoveOrReplace(action, newItems, oldItems, startingIndex, startingIndex);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0008FD44 File Offset: 0x0008DF44
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException("action");
			}
			if (index < 0)
			{
				throw new ArgumentException("index");
			}
			object[] array = new object[]
			{
				changedItem
			};
			this.InitializeMoveOrReplace(action, array, array, index, oldIndex);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0008FD9A File Offset: 0x0008DF9A
		public NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
		{
			if (action != NotifyCollectionChangedAction.Move)
			{
				throw new ArgumentException("action");
			}
			if (index < 0)
			{
				throw new ArgumentException("index");
			}
			this.InitializeMoveOrReplace(action, changedItems, changedItems, index, oldIndex);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0008FDDC File Offset: 0x0008DFDC
		internal NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int newIndex, int oldIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : new ReadOnlyList(newItems));
			this._oldItems = ((oldItems == null) ? null : new ReadOnlyList(oldItems));
			this._newStartingIndex = newIndex;
			this._oldStartingIndex = oldIndex;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0008FE38 File Offset: 0x0008E038
		private void InitializeAddOrRemove(NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			if (action == NotifyCollectionChangedAction.Add)
			{
				this.InitializeAdd(action, changedItems, startingIndex);
				return;
			}
			if (action == NotifyCollectionChangedAction.Remove)
			{
				this.InitializeRemove(action, changedItems, startingIndex);
			}
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0008FE54 File Offset: 0x0008E054
		private void InitializeAdd(NotifyCollectionChangedAction action, IList newItems, int newStartingIndex)
		{
			this._action = action;
			this._newItems = ((newItems == null) ? null : new ReadOnlyList(newItems));
			this._newStartingIndex = newStartingIndex;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0008FE76 File Offset: 0x0008E076
		private void InitializeRemove(NotifyCollectionChangedAction action, IList oldItems, int oldStartingIndex)
		{
			this._action = action;
			this._oldItems = ((oldItems == null) ? null : new ReadOnlyList(oldItems));
			this._oldStartingIndex = oldStartingIndex;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0008FE98 File Offset: 0x0008E098
		private void InitializeMoveOrReplace(NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, int oldStartingIndex)
		{
			this.InitializeAdd(action, newItems, startingIndex);
			this.InitializeRemove(action, oldItems, oldStartingIndex);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0008FEAE File Offset: 0x0008E0AE
		public NotifyCollectionChangedAction Action
		{
			get
			{
				return this._action;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x0008FEB6 File Offset: 0x0008E0B6
		public IList NewItems
		{
			get
			{
				return this._newItems;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x0008FEBE File Offset: 0x0008E0BE
		public IList OldItems
		{
			get
			{
				return this._oldItems;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0008FEC6 File Offset: 0x0008E0C6
		public int NewStartingIndex
		{
			get
			{
				return this._newStartingIndex;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0008FECE File Offset: 0x0008E0CE
		public int OldStartingIndex
		{
			get
			{
				return this._oldStartingIndex;
			}
		}

		// Token: 0x04001284 RID: 4740
		private NotifyCollectionChangedAction _action;

		// Token: 0x04001285 RID: 4741
		private IList _newItems;

		// Token: 0x04001286 RID: 4742
		private IList _oldItems;

		// Token: 0x04001287 RID: 4743
		private int _newStartingIndex = -1;

		// Token: 0x04001288 RID: 4744
		private int _oldStartingIndex = -1;
	}
}
