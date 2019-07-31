using System;
using Provider.VistaDB;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design;

namespace Provider.VistaDB
{
	/// <summary>
	/// Summary description for VistaDBDataSet.
	/// </summary>
	public class VistaDBDataSet: Component, IBindingList, ITypedList, ISupportInitialize, IVistaDBDataSet
	{
		private int CacheSize = 100;
			
		internal VistaDBDataSetTable table;
		private PropertyDescriptor sortProperty;
		private ListSortDirection sortDirection;
		private int cacheMinRowIndex, cacheMaxRowIndex, cacheIndex;
		private VistaDBDataRow[] dataRowCache;
		private int rowIndex;
		private bool firstRun, secondRun;
		private int count;
		private VistaDBDataRow dataRow;
		private bool inserting;
		private bool initStarted = false;
		private bool needOpen    = false;
		private string indexName = null;

		/// <summary>
		/// Constructor
		/// </summary>
		public VistaDBDataSet()
		{
			this.table            = null;
			this.sortProperty     = null;
			this.sortDirection    = ListSortDirection.Ascending;
			this.cacheMinRowIndex = -1;
			this.cacheMaxRowIndex = -1;
			this.rowIndex         = -1;

			this.dataRowCache     = new VistaDBDataRow[CacheSize];
			this.cacheIndex       = -1;

			this.firstRun         = true;
			this.secondRun        = false;
			this.count            = 0;

			this.table            = new VistaDBDataSetTable(this);

			this.dataRow          = null;

			this.inserting        = false;
		}

		/// <summary>
		/// Destructor
		/// </summary>
		~VistaDBDataSet()
		{
			this.table.InternalClose();
			this.sortProperty = null;
			this.dataRowCache = null;
			this.dataRow      = null;
		}

		/// <summary>
		/// Not used. Always return -1.
		/// </summary>
		public int Add(object value)
		{
			return -1;
		}

		/// <summary>
		/// Adds the PropertyDescriptor to the indexes used for searching.
		/// </summary>
		/// <param name="property">The PropertyDescriptor to add to the indexes used for searching.</param>
		public void AddIndex(PropertyDescriptor property)
		{
		}

		/// <summary>
		/// Adds a new item to the list.
		/// </summary>
		/// <returns>The item added to the list.</returns>
		public object AddNew()
		{
			VistaDBDataRow dataRow;

			if (!this.AllowNew)
			{
				throw new InvalidOperationException("Adding a new row is not allowed");
			}

			dataRow = this.Insert();

			if (this.ListChanged != null)
			{
				this.ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, (this.Count - 1)));
			}
			return dataRow;
		}

		/// <summary>
		/// Sorts the list based on a PropertyDescriptor and a ListSortDirection.
		/// </summary>
		/// <param name="property">The PropertyDescriptor to sort by.</param>
		/// <param name="direction">One of the ListSortDirection values.</param>
		public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			string columnName = property.Name.ToUpper();
			bool active, unique, primary, desc, fts;
			string keyExpr;
			int orderIndex;
			string indexName;
			string[] list;

			this.CheckOpened(true);

			this.table.EnumIndexes(out list);

			for(int i = 0; i < list.Length; i++)
			{
				indexName = list[i];

				this.table.GetIndex(indexName, out active, out orderIndex, out unique, out primary, out desc, out keyExpr, out fts);

				if(!fts && keyExpr.Trim().ToUpper() == columnName)
				{					
					this.table.ActiveIndex = "";
					this.table.ActiveIndex = indexName;

					if(direction == ListSortDirection.Ascending)
					{
						this.table.IndexAscending();
					}
					else if(direction == ListSortDirection.Descending)
					{
						this.table.IndexDescending();
					}

					this.sortProperty  = property;
					this.sortDirection = direction;

					this.RefreshDataSet(false);

					return;
				}
			}
		}

		void ISupportInitialize.BeginInit()
		{
			this.initStarted = true;
			this.needOpen = false;
			this.indexName = null;
		}

		internal void CancelInsert()
		{
			this.cacheMaxRowIndex--;
			if(this.cacheMaxRowIndex < 0)
				this.cacheMinRowIndex = -1;
			this.SetCount(this.count - 1);
			this.Inserting = false;
		}

		private bool CheckOpened(bool raiseException)
		{
			if(!this.IsOpened && raiseException)
				throw new InvalidOperationException("Table is not opened");

			return this.IsOpened;
		}

		/// <summary>
		/// Not used.
		/// </summary>
		public void Clear()
		{
		}

		private void ClearCache()
		{
			this.rowIndex         = -1;
			this.cacheMinRowIndex = -1;
			this.cacheMaxRowIndex = -1;

			for(int i = 0; i < CacheSize; i++)
				this.SetDataRow(i, null);

			this.cacheIndex       = -1;

			this.RefreshCount();
		}

		/// <summary>
		/// Clear data set filter
		/// </summary>
		public void ClearFilter()
		{
			this.SetFilter("");
		}

		/// <summary>
		/// Clear data set scope
		/// </summary>
		public void ClearScope()
		{
			this.SetScope("");
		}

		/// <summary>
		/// Close data set
		/// </summary>
		public void Close()
		{
			this.IsOpened = false;
		}

		/// <summary>
		/// Determines whether the VistaDBDataSet object contains a specific VistaDBDataRow object.
		/// </summary>
		/// <param name="value">The VistaDBDataRow object to locate.</param>
		/// <returns>true if the VistaDBDataRow object is found; otherwise, false.</returns>
		public bool Contains(object value)
		{
			return !(this.IndexOf(value) < 0); 
		}

		/// <summary>
		/// Copies the VistaDBDataRow objects of to an Array, starting at a particular Array index
		/// </summary>
		/// <param name="array">The one-dimensional Array that is the destination of the elements copied from VistaDBDataSet. The Array must have zero-based indexing.</param>
		/// <param name="index">A 32-bit integer that represents the position of the Array element to set.</param>
		public void CopyTo(Array array, int index)
		{
			for (int i = 0; i < this.Count; i++)
				array.SetValue(this[i], index + i);
		}

		void ISupportInitialize.EndInit()
		{
			this.initStarted = false;
			((IVistaDBDataSet)this).OpenAfterInit();
		}

		/// <summary>
		/// Returns the index of the row that has the given PropertyDescriptor.
		/// </summary>
		/// <param name="property">The PropertyDescriptor to search on.</param>
		/// <param name="key">The value of the property parameter to search for.</param>
		/// <returns>The index of the row that has the given PropertyDescriptor.</returns>
		public int Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException("Method is not supported");
		}

		private VistaDBDataRow GetDataRow(int index)
		{
			return this.dataRowCache[index];
		}

		private VistaDBDataRow GetDataRow()
		{
			if(this.cacheIndex >= 0)
				return this.GetDataRow(this.cacheIndex);
			else
				return null;
		}

		/// <summary>
		/// Returns an enumerator that can iterate through a rows.
		/// </summary>
		/// <returns>An IEnumerator that can be used to iterate through the rows.</returns>
		public IEnumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		/// <summary>
		/// Returns the PropertyDescriptorCollection that represents the properties on each item used to bind data.
		/// </summary>
		/// <param name="listAccessors">An array of PropertyDescriptor objects to find in the collection as bindable. This can be a null reference (Nothing in Visual Basic).</param>
		/// <returns>The PropertyDescriptorCollection that represents the properties on each item used to bind data.</returns>
		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection collection;
			collection = new PropertyDescriptorCollection(null);

			if (this.CheckOpened(false))
			{
				for (int i = 0; i < this.table.ColumnCount(); i++)
				{
					collection.Add(new VistaDBPropertyDescriptor(this.table.Columns[i].Name, this.table.Columns[i].Type, i + 1, this.table.Columns[i].VistaDBType, this.table.Columns[i].ReadOnly));
				}
 
			}

			return collection;
		}

		/// <summary>
		/// Returns the name of the list.
		/// </summary>
		/// <param name="listAccessors">An array of PropertyDescriptor objects, the list name for which is returned. This can be a null reference (Nothing in Visual Basic).</param>
		/// <returns>The name of the list.</returns>
		public string GetListName(PropertyDescriptor[] listAccessors)
		{
			return "List";
		}

		/// <summary>
		/// Determines the index of a specific VistaDBDataRow object.
		/// </summary>
		/// <param name="value">The VistaDBDataRow object to locate.</param>
		/// <returns>The index of value if found in the list; otherwise, -1.</returns>
		public int IndexOf(object value)
		{
			for(int i = 0; i < this.Count; i++)
			{
				if(this[i] == value)
					return i;
			}

			return -1;
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		public void Insert(int index, object value)
		{
		}

		private VistaDBDataRow Insert()
		{
			//Synchronize record set
			this.SyncRecordSet(this.Count - 1, true);

			//Prepare space for new row
			this.SetCount(this.count + 1);
			this.SynchronizeCache(this.Count - 1);
			this.cacheIndex++;
			this.rowIndex = this.Count - 1;

			this.SetDataRow(new VistaDBDataRow(this, -1, this.Count - 1));
			this.firstRun = true;
			this.Inserting = true;

			return this.GetDataRow();
		}

		/// <summary>
		/// Opens data set
		/// </summary>
		public void Open()
		{
			this.IsOpened = true;
		}

		void IVistaDBDataSet.OpenAfterInit()
		{
			if(this.needOpen)
			{
				this.Open();
				this.ActiveIndex = this.indexName;
				this.needOpen    = false;
			}
		}

		/// <summary>
		/// Refresh data set
		/// </summary>
		public void RefreshDataSet()
		{
			this.RefreshDataSet(false);
		}

		private void RefreshDataSet(bool requiredSecondRun)
		{
			if(this.IsOpened)
			{
				this.ClearCache();				
				this.firstRun  = true;
				this.secondRun = requiredSecondRun;
				this.ListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));
			}
		}

		private void RefreshCount()
		{
			if(this.IsOpened)
				this.SetCount((int)this.table.RowCount());
			else
				this.SetCount(0);

			this.Inserting = false;
		}

		/// <summary>
		/// Not supported.
		/// </summary>
		public void Remove(object value)
		{
		}

		/// <summary>
		/// Removes the row at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			if (!this.AllowRemove)
			{
				throw new InvalidOperationException("Removing a row is not allowed"); 
			}

			if(this.SyncRecordSet(index, true))
			{
				this.table.DeleteCurrentRow();
				if(this.cacheMaxRowIndex - index >= 1)
				{
					Array.Copy(this.dataRowCache, this.cacheIndex + 1, this.dataRowCache, this.cacheIndex, this.cacheMaxRowIndex - this.cacheMinRowIndex);
				}
				else
				{
					this.cacheIndex--;
					this.rowIndex--;
				}

				this.cacheMaxRowIndex--;
				if(this.cacheMaxRowIndex < 0)
					this.cacheMinRowIndex = -1;

				this.SetCount(this.count - 1);
			}
		}

		/// <summary>
		/// Removes the PropertyDescriptor from the indexes used for searching.
		/// </summary>
		/// <param name="property">The PropertyDescriptor to remove from the indexes used for searching.</param>
		public void RemoveIndex(PropertyDescriptor property)
		{
		}

		/// <summary>
		/// Removes any sort applied using ApplySort.
		/// </summary>
		public void RemoveSort()
		{
			this.CheckOpened(true);
			this.table.ActiveIndex = "";
		}

		private void SetCount(int value)
		{
			this.count = value;
		}

		private void SetDataRow(int index, VistaDBDataRow dataRow)
		{
			this.dataRowCache[index] = dataRow;
		}

		private void SetDataRow(VistaDBDataRow dataRow)
		{
			if(this.cacheIndex >= 0)
				this.dataRowCache[this.cacheIndex] = dataRow;
		}

		/// <summary>
		/// Set filter on data set
		/// </summary>
		/// <param name="expression">Filter expression</param>
		/// <returns>True if success</returns>
		public bool SetFilter(string expression)
		{
			return this.SetFilter(expression, true);
		}

		/// <summary>
		/// Set filter on data set
		/// </summary>
		/// <param name="expression">Filter expression</param>
		/// <param name="useOptimization">True for using optimization</param>
		/// <returns>True if success</returns>
		public bool SetFilter(string expression, bool useOptimization)
		{
			if(this.IsOpened)
			{
				this.table.SetFilter(expression, useOptimization);
				this.RefreshDataSet(false);
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Set scope
		/// </summary>
		/// <param name="val">Scope expression</param>
		/// <returns>True if success</returns>
		public bool SetScope(string val)
		{
			return this.SetScope(val, val);
		}

		/// <summary>
		/// Set scope
		/// </summary>
		/// <param name="lowVal">Scope low value</param>
		/// <param name="highVal">Scoppe high value</param>
		/// <returns>Tru if success</returns>
		public bool SetScope(string lowVal, string highVal)
		{
			if(this.IsOpened)
			{
				this.table.SetScope(lowVal, highVal);
				this.RefreshDataSet(false);
				return true;
			}
			else
				return false;
		}

		private void SynchronizeCache(int index)
		{
			//Synchronize cache
			if(this.cacheMinRowIndex >= 0)
			{
				if(this.cacheMinRowIndex - index + 1 > CacheSize || index - this.cacheMaxRowIndex + 1 > CacheSize)
				{
					this.ClearCache();
				}
				else
				{
					if(index < this.cacheMinRowIndex)
					{
						int addLen  = this.cacheMinRowIndex - index;
						int copyLen = this.cacheMaxRowIndex - this.cacheMinRowIndex + 1;

						if(copyLen + addLen > CacheSize)
						{
							copyLen = CacheSize - addLen;
							this.cacheMaxRowIndex -= CacheSize - copyLen;
						}

						Array.Copy(this.dataRowCache, 0, this.dataRowCache, addLen, copyLen);
						this.cacheMinRowIndex = index;
						this.cacheIndex = addLen;
					}
					else
					{
						int addLen      = index - this.cacheMaxRowIndex;
						int copyLen     = this.cacheMaxRowIndex - this.cacheMinRowIndex + 1;

						this.cacheIndex = copyLen - 1;

						if(copyLen + addLen > CacheSize)
						{
							copyLen = CacheSize - addLen;
							Array.Copy(this.dataRowCache, this.cacheIndex - copyLen + 1, this.dataRowCache, 0, copyLen);
							this.cacheMinRowIndex += this.cacheIndex - copyLen + 1;
							this.cacheIndex = copyLen - 1;
						}

						this.cacheMaxRowIndex += addLen;
					}
				}
			}
			else
			{
				this.cacheMinRowIndex = 0;
				this.cacheMaxRowIndex = 0;
				this.cacheIndex       = -1;
			}
		}

		private bool SyncRecordSet(int index, bool hard)
		{
			if(!this.SyncRecordSet(index))
			{
				return false;
			}

			if(hard && this.rowIndex != index)
			{
				if(this.rowIndex < index)
				{
					for(int i = this.rowIndex; i < index; i++)
					{
						this.rowIndex++;
						this.table.Next();
					}
				}
				else
				{
					for(int i = this.rowIndex; i > index; i--)
					{
						this.rowIndex--;
						this.table.Prior();
					}
				}
			}

			return true;
		}
		
		private bool SyncRecordSet(int index)
		{
			int cacheRowIndex;

			if(index < 0 || index >= this.Count)
				return false;

			//Check if data present in the cache, then use this data, else refresh
			if(this.cacheMinRowIndex > index || this.cacheMaxRowIndex < index)
			{
				this.CheckOpened(true);

				if(this.rowIndex < 0)
				{
					this.ClearCache();

					if(index < this.Count - index)
					{
						this.table.First();

						if(this.table.EndOfSet())
							return false;

						this.rowIndex = 0;
					}
					else
					{
						this.table.Last();

						if(this.table.BeginOfSet())
							return false;

						this.rowIndex = this.Count - 1;
					}
				}
				else
				{
					//If this required go to last or first row
					if(!this.Inserting)
					{
						if(index == 0)
						{
							this.table.First();
							this.ClearCache();
							this.rowIndex = 0;
						}
						else if(index == this.Count - 1)
						{
							this.table.Last();
							this.ClearCache();
							this.rowIndex = this.Count - 1;
						}
					}
				}

				//Synchronize cache
				if(this.rowIndex < index)
					cacheRowIndex = this.cacheMaxRowIndex;
				else
					cacheRowIndex = this.cacheMinRowIndex;
				this.SynchronizeCache(index);

				//Find requested row
				if(this.rowIndex < index)
				{
					for(int i = this.rowIndex; i < index; i++)
					{
						this.rowIndex++;
						this.table.Next();

						if(this.table.EndOfSet())
							break;

						if(this.cacheMinRowIndex >= 0 && cacheRowIndex >= 0 && this.rowIndex > cacheRowIndex)
						{
							this.cacheIndex++;
							this.dataRowCache[this.cacheIndex] = new VistaDBDataRow(this, this.table.CurrentRowID(), this.rowIndex);
						}
					}
				}
				else
				{
					for(int i = this.rowIndex; i > index; i--)
					{
						this.rowIndex--;
						this.table.Prior();

						if(this.table.BeginOfSet())
							break;

						if(this.cacheMinRowIndex >= 0 && cacheRowIndex >= 0 && this.rowIndex < cacheRowIndex)
						{
							this.cacheIndex--;
							this.dataRowCache[this.cacheIndex] = new VistaDBDataRow(this, this.table.CurrentRowID(), this.rowIndex);
						}
					}
				}

				//Synchronize
				if(this.table.EndOfSet())
				{
					this.RefreshCount();
					this.rowIndex   = this.Count - 1;
					this.cacheIndex = -1;
				}
				else if(this.table.BeginOfSet())
				{
					this.RefreshCount();
					this.rowIndex   = 0;
					this.cacheIndex = -1;
				}
			
				//Create new VistaDBDataRow object
				if(cacheRowIndex < 0 && this.Count > 0)
				{
					this.cacheIndex       = 0;
					this.cacheMinRowIndex = this.rowIndex;
					this.cacheMaxRowIndex = this.rowIndex;
					this.dataRowCache[this.cacheIndex] = new VistaDBDataRow(this, this.table.CurrentRowID(), this.rowIndex);
				}
			}
			else
			{
				//Synchronize cacheIndex
				this.cacheIndex = index - this.cacheMinRowIndex;
			}

			return true;
		}


		//////////////////////////////////////////////////////////////
		//////////////// P R O P E R T I E S /////////////////////////
		//////////////////////////////////////////////////////////////

		/// <summary>
		/// Active table index
		/// </summary>
		public string ActiveIndex
		{
			get
			{
				return this.table.ActiveIndex;
			}
			set
			{
				if(this.initStarted)
					this.indexName = value;
				else
				{
					if(value != this.table.ActiveIndex)
					{
						this.table.ActiveIndex = value;

						if(this.table.ActiveIndex == value)
						{
							this.RefreshDataSet(false);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets whether you can update items in the list.
		/// </summary>
		[Browsable(false)]
		public bool AllowEdit
		{
			get
			{
				return this.CheckOpened(false) && !this.table.Database.ReadOnly;
			}
		}

		/// <summary>
		/// Gets whether you can add items to the list using AddNew.
		/// </summary>
		[Browsable(false)]
		public bool AllowNew
		{
			get
			{
				return this.CheckOpened(false) && !this.table.Database.ReadOnly;
			}
		}

		/// <summary>
		/// Gets whether you can remove items from the list, using Remove or RemoveAt.
		/// </summary>
		[Browsable(false)]
		public bool AllowRemove
		{
			get
			{
				return this.CheckOpened(false) && !this.table.Database.ReadOnly;
			}
		}

		/// <summary>
		/// Data set columns
		/// </summary>
		public VistaDBTable.ColumnCollection Columns
		{
			get
			{
				return this.table.Columns;
			}
		}

		/// <summary>
		/// Gets the row count
		/// </summary>
		[Browsable(false)]
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		/// <summary>
		/// VistaDBDatabase object
		/// </summary>
		public VistaDBDatabase Database
		{
			get
			{
				return this.table.Database;
			}
			set
			{
				this.table.Database = value;

				if(this.initStarted && this.table.Database != null)
					this.table.Database.AddToInitList(this);
			}
		}

		/// <summary>
		/// Gets or sets the VistaDBDataRow object at the specified index.
		/// </summary>
		object IList.this[int index]
		{
			get
			{
				if(this.secondRun && !this.firstRun)
				{
					this.secondRun = false;
					this.firstRun  = true;
				}

				if(this.firstRun)
				{
					this.firstRun = index != this.Count - 1;
					return this.dataRow;
				}

				if(this.SyncRecordSet(index))
					return this.GetDataRow();
				else
					return null;
			}
			set
			{
			}
		}

		internal bool Inserting
		{
			get
			{
				return this.inserting;
			}
			set
			{
				this.inserting = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Show if data set opened
		/// </summary>
		public bool IsOpened
		{
			get
			{
				return this.table.Opened;
			}
			set
			{
				if(this.initStarted)
					this.needOpen = value;
				else
				{
					if(value != this.table.Opened)
					{
						if(value)
						{
							this.table.Open();
							this.dataRow = new VistaDBDataRow(this);						
						}
						else
						{
							this.table.InternalClose();
							this.dataRow = null;
						}

						this.RefreshCount();
						this.ClearCache();
						this.sortProperty = null;

						if (this.ListChanged != null)
						{
							if(value)
							{
								this.firstRun  = true;
								this.ListChanged(this, new ListChangedEventArgs(ListChangedType.PropertyDescriptorAdded, 0));
								this.firstRun  = true;
								this.secondRun = true;
								this.ListChanged(this, new ListChangedEventArgs(ListChangedType.ItemAdded, 0));
							}
							else
								this.ListChanged(this, new ListChangedEventArgs(ListChangedType.PropertyDescriptorDeleted, 0));
						}
						else
						{
							this.firstRun  = true;
							this.secondRun = false;
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[Browsable(false)]
		public bool IsReadOnly
		{
			get
			{
				return CheckOpened(false) && this.table.Database.ReadOnly;
			}
		}

		/// <summary>
		/// Gets whether the items in the list are sorted.
		/// </summary>
		[Browsable(false)]
		public bool IsSorted
		{
			get
			{
				return this.table.ActiveIndex != "" && this.SortProperty != null;
			}
		}

		/// <summary>
		/// Always return false
		/// </summary>
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets the direction of the sort.
		/// </summary>
		[Browsable(false)]
		public ListSortDirection SortDirection
		{
			get
			{
				return this.sortDirection;
			}
		}

		/// <summary>
		/// Gets the PropertyDescriptor that is being used for sorting.
		/// </summary>
		[Browsable(false)]
		public PropertyDescriptor SortProperty
		{
			get
			{
				return this.sortProperty;
			}
		}

		/// <summary>
		/// Gets whether a ListChanged event is raised when the list changes or an item in the list changes.
		/// </summary>
		[Browsable(false)]
		public bool SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets whether the list supports searching using the Find method.
		/// </summary>
		[Browsable(false)]
		public bool SupportsSearching 
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Gets whether the list supports sorting.
		/// </summary>
		[Browsable(false)]
		public bool SupportsSorting
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///	Gets an object that can be used to synchronize access to the VistaDBDataSet 
		/// </summary>
		[Browsable(false)]
		public object SyncRoot 
		{
			get
			{
				return this;
			}
		}

		/// <summary>
		/// Return VistaDBDataRow
		/// </summary>
		public VistaDBDataRow this[int index]
		{
			get
			{
				if(this.SyncRecordSet(index))
					return this.GetDataRow();
				else
					return null;
			}
		}

		/// <summary>
		/// Table name
		/// </summary>
		public string TableName
		{
			get
			{
				return this.table.TableName;
			}
			set
			{
				this.table.TableName = value;
			}
		}


		//////////////////////////////////////////////////////////////////
		///////////////// E V E N T S ////////////////////////////////////
		//////////////////////////////////////////////////////////////////

		/// <summary>
		/// Occurs when the list changes or an item in the list changes.
		/// </summary>
		public event ListChangedEventHandler ListChanged;


		//////////////////////////////////////////////////////////////////
		///////////////////// C L A S S E S //////////////////////////////
		//////////////////////////////////////////////////////////////////

		/// <summary>
		/// VistaDB property descriptor class
		/// </summary>
		public class VistaDBPropertyDescriptor: PropertyDescriptor
		{

			internal VistaDBPropertyDescriptor(string name, Type propertyType, int columnNo, VistaDBType dataType, bool readOnly): base(name, null)
			{
				this.propertyType = propertyType;
				this.columnNo     = columnNo;
				this.dataType     = dataType;
				this.readOnly     = readOnly;
			}

			/// <summary>
			/// 
			/// </summary>
			public override bool CanResetValue(object component)
			{
				return false;
			}

			/// <summary>
			/// Return column value
			/// </summary>
			/// <param name="component">VistaDBDataRow object</param>
			/// <returns></returns>
			public override object GetValue(object component)
			{
					return ((VistaDBDataRow)component).GetValue(this.columnNo);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="component"></param>
			public override void ResetValue(object component)
			{  
			}

			/// <summary>
			/// Set column value
			/// </summary>
			/// <param name="component"></param>
			/// <param name="value"></param>
			public override void SetValue(object component, object value)
			{
				((VistaDBDataRow)component).SetValue((this.columnNo), value);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="component"></param>
			/// <returns></returns>
			public override bool ShouldSerializeValue(object component)
			{
					return true; 
			}

			/// <summary>
			/// Type of VistaDBDataRow
			/// </summary>
			public override Type ComponentType
			{
				get
				{
					return typeof(VistaDBDataRow);
				}
			}

			/// <summary>
			/// Return if data set is read only (true) or no (fase)
			/// </summary>
			public override bool IsReadOnly
			{
				get
				{
					return this.readOnly;
				}
			}

			/// <summary>
			/// 
			/// </summary>
			public override Type PropertyType
			{
				get
				{
					return this.propertyType;
				}
			}

			private Type propertyType;
			private int columnNo;
			private VistaDBType dataType;
			private bool readOnly;
		}


		class Enumerator: IEnumerator
		{
			private VistaDBDataSet parent;
			private int position;

			public Enumerator(VistaDBDataSet parent)
			{
				parent.CheckOpened(true);
				this.parent = parent;
				this.Reset();
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection</returns>
			public bool MoveNext()
			{
				if(this.position < this.parent.Count - 2)
				{
					this.position++;
					return true;
				}
				return false;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				this.position = -1;
				this.parent.table.First();
			}

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public object Current
			{
				get
				{
					return this.parent[position];
				}
			}
		}


		internal class VistaDBDataSetTable: VistaDBTable
		{
			private VistaDBDataSet parent;

			internal VistaDBDataSetTable(VistaDBDataSet parent)
			{
				this.parent = parent;
			}

			public override void Close()
			{
				this.parent.IsOpened = false;
			}

			internal void InternalClose()
			{
				base.Close();
			}
		}

	}
}