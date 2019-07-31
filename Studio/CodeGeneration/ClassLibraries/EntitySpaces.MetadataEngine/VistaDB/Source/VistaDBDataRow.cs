using System;
using System.ComponentModel;

namespace Provider.VistaDB
{
	/// <summary>
	/// Summary description for VistaDBDataRow.
	/// </summary>
	public class VistaDBDataRow: IEditableObject
	{
		private VistaDBDataSet parent;
		private int rowIndex;
		private long rowID;
		private object[] values;
		private bool editing;

		internal VistaDBDataRow(VistaDBDataSet parent, long rowID, int rowIndex)
		{
			this.editing  = false;//rowID == -1;
			this.parent   = parent;
			this.rowID    = rowID;
			this.rowIndex = rowIndex;
			this.values   = new object[this.parent.table.ColumnCount()];

			if(this.rowID > 0)
			{
				for(int i = 0; i < this.parent.table.ColumnCount(); i++)
					this.values[i] = this.parent.table.GetObject(i);
			}
			else
			{
				for(int i = 0; i < this.parent.table.ColumnCount(); i++)
					this.values[i] = DBNull.Value;
			}
		}

		internal VistaDBDataRow(VistaDBDataSet parent)
		{
			this.editing  = false;
			this.parent   = parent;
			this.rowID    = -1;
			this.rowIndex = -1;
			this.values   = new object[this.parent.table.ColumnCount()];
			for(int i = 0; i < this.parent.table.ColumnCount(); i++)
				this.values[i] = DBNull.Value;
		}

		/// <summary>
		/// Begins an edit on an object.
		/// </summary>
		public void BeginEdit()
		{
		}

		/// <summary>
		/// Discards changes since the last BeginEdit call.
		/// </summary>
		public void CancelEdit()
		{
			if(this.rowID < 0)
				this.parent.CancelInsert();
			else
				this.editing = false;
		}

		/// <summary>
		/// Pushes changes since the last BeginEdit or IBindingList.AddNew call into the underlying object.
		/// </summary>
		public void EndEdit()
		{
			if(this.editing)
			{
				if(this.rowID > 0)
				{
					this.parent.table.MoveTo(this.rowID);
				}
				else
				{
					this.parent.table.Insert();
					this.parent.Inserting = false;
				}

				for(int i = 0; i < this.values.Length; i++)
					this.parent.table.PutObject(i, this.values[i]);

				this.parent.table.Post();

				if(this.rowID < 0)
				{
					this.rowID = this.parent.table.CurrentRowID();
				}

				this.editing = false;
			}
		}

		/// <summary>
		/// Return column value
		/// </summary>
		/// <param name="columnNo">Column number (1-based)</param>
		/// <returns>Column value</returns>
		public object GetValue(int columnNo)
		{
			return this.values[columnNo - 1];
		}

		/// <summary>
		/// Set column value
		/// </summary>
		/// <param name="columnNo">Column number (1-based)</param>
		/// <param name="value"></param>
		public void SetValue(int columnNo, object value)
		{
			this.values[columnNo - 1] = value;
			this.editing = true;
		}

		////////////////////////////////////////////////////////////////////
		///////////////// P R O P E R T I E S //////////////////////////////
		////////////////////////////////////////////////////////////////////
		
		/// <summary>
		/// Physical row index
		/// </summary>
		public long RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}
	}
}