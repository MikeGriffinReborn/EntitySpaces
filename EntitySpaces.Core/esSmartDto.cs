using EntitySpaces.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static EntitySpaces.Core.esSmartDtoMap;

namespace EntitySpaces.Core
{
    abstract public class esSmartDto
    {
        public esSmartDto()
        {
            rowState = esDataRowState.Added;
            currentValues.FirstAccess += new esSmartDictionary.esSmartDictionaryFirstAccessEventHandler(CurrentValues_OnFirstAccess);
        }

        protected void HrydateFromEntity(esEntity entity)
        {
            if (entity != null)
            {
                this.currentValues = entity.currentValues;
                this.originalValues = entity.originalValues;
                this.rowState = entity.rowState;
                this.m_modifiedColumns = entity.m_modifiedColumns;
            }
                
        }

        public List<string> GetModifiedColumns()
        {
            var list = new List<string>();

            if (m_modifiedColumns != null && m_modifiedColumns.Count > 0)
            {
                list.AddRange(this.m_modifiedColumns);
            }

            return list;
        }

        public object GetColumn(string columnName)
        {
            if (currentValues == null || currentValues.Count == 0 || !currentValues.ContainsKey(columnName) || rowState == esDataRowState.Deleted)
            {
                return null;
            }
            else
            {
                object o = currentValues[columnName];
                return (o == DBNull.Value) ? null : o;
            }
        }

        private Dictionary<string, string> Getters
        {
            get
            {
                if (getters == null)
                {
                    getters = GetMap().getters;
                }

                return getters;
            }
        }

        protected T GetValue<T>([CallerMemberName] string propertyName = null)
        {
            if (!FieldsExists(this)) return default(T);

            currentValues.TryGetValue(Getters[propertyName], out var o);

            if (o == null || o == DBNull.Value)
                return default(T);
            else
            {
                Type tType = typeof(T);
                return (o is T) ? (T)o : (T)Convert.ChangeType(o, typeof(T));
            }
        }

        /// <summary>
        /// Called by all of the property setters
        /// </summary>
        /// <param name="columnName">The name of the column to set</param>
        /// <param name="data">the value to set it to</param>
        /// <returns>True if the value has truly changed</returns>
        protected bool SetValue(object data, [CallerMemberName] string propertyName = null)
        {
            bool changed = true;

            if (rowState == esDataRowState.Deleted) throw new Exception("Cannot modifiy deleted records");

            try
            {
                if (currentValues == null)
                {
                    currentValues = new esSmartDictionary();
                    CurrentValues_OnFirstAccess(currentValues);
                }

                if (currentValues.Count == 0)
                {
                    if (rowState == esDataRowState.Added && !applyDefaultsCalled)
                    {
                        applyDefaultsCalled = true;
                        ApplyDefaults();
                    }
                }

                string columnName = Getters[propertyName];
                if (!currentValues.ContainsKey(columnName))
                {
                    currentValues[columnName] = data;
                    changed = true;

                    if (rowState == esDataRowState.Unchanged)
                    {
                        rowState = esDataRowState.Modified;
                    }
                }
                else
                {
                    object o = currentValues[columnName];
                    bool isNull = (o == DBNull.Value || o == null);

                    // Note that we grab this before we make the change
                    esDataRowState state = rowState;

                    if (data == null && isNull)
                    {
                        // Nothing to do here
                        changed = false;
                    }
                    else
                    {
                        if (isNull && data != null)
                            currentValues[columnName] = data;
                        else if (data == null && !isNull)
                            currentValues[columnName] = DBNull.Value;
                        else if (!o.Equals(data))
                        {
                            this.currentValues[columnName] = data;

                            // Special logic to see if we have changed it back to it's original value, if 
                            // so we mark this column as no longer dirty, which if the only one could return
                            // the rowstate back to "Unchanged"
                            if (originalValues != null && originalValues.ContainsKey(columnName))
                            {
                                if (data == originalValues[columnName])
                                {
                                    MarkFieldAsUnchanged(columnName);
                                    return true; // it still changed but we don't want to mark it as dirty
                                }
                            }
                        }
                        else
                            changed = false;
                    }
                }

                if (changed)
                {
                    MarkFieldAsModified(columnName);
                }
            }
            finally
            {

            }

            return changed;
        }

        /// <summary>
        /// Called when the first property is accessed or set when an entity has the
        /// RowState of Added. Use this method to initialize any properties of a newly
        /// created entity.
        /// </summary>
        virtual internal protected void ApplyDefaults()
        {

        }

        private void CurrentValues_OnFirstAccess(esSmartDictionary smartDictionary)
        {
            smartDictionary.Allocate(1);
        }

        protected void MarkFieldAsUnchanged(string columnName)
        {
            if (this.m_modifiedColumns == null) return;

            if (m_modifiedColumns.Contains(columnName))
            {
                m_modifiedColumns.Remove(columnName);

                if (m_modifiedColumns.Count == 0)
                {
                    AcceptChanges();
                }
            }
        }

        /// <summary>
        /// Called to accept all proposed changes and mark the dirty as Unchanged. See <see cref="RejectChanges"/> as well.
        /// </summary>
        virtual public void AcceptChanges()
        {
            if (rowState == esDataRowState.Deleted)
            {
                currentValues = originalValues = null;
                m_modifiedColumns = null;
                rowState = esDataRowState.Invalid;
            }
            else
            {
                originalValues = new esSmartDictionary(currentValues);
                rowState = esDataRowState.Unchanged;
                m_modifiedColumns = null;
            }
        }

        /// <summary>
        /// RejectChanges does just the opposite of <see cref="AcceptChanges"/>. That is, RejectChanges moves the
        /// original values back into the current values, it's as if nothing was ever changed.
        /// </summary>
        /// <seealso cref="AcceptChanges"/>
        virtual public void RejectChanges()
        {
            if (rowState == esDataRowState.Added)
            {
                currentValues = new esSmartDictionary(currentValues.Count);
                rowState = esDataRowState.Unchanged;
                m_modifiedColumns = null;
            }
            else
            {
                currentValues = new esSmartDictionary(originalValues);
                rowState = esDataRowState.Unchanged;
                m_modifiedColumns = null;
            }
        }

        /// <summary>
        /// Marks a column as dirty
        /// </summary>
        /// <param name="columnName">The column name to mark dirty</param>
        protected void MarkFieldAsModified(string columnName)
        {
            if (this.m_modifiedColumns == null)
            {
                m_modifiedColumns = new List<string>();
            }

            if (!m_modifiedColumns.Contains(columnName))
            {
                m_modifiedColumns.Add(columnName);

                if (rowState != esDataRowState.Added)
                {
                    rowState = esDataRowState.Modified;
                }
            }
        }

        static private bool FieldsExists(esSmartDto entity)
        {
            if (entity.currentValues == null || entity.currentValues.Count == 0)
            {
                if (entity.rowState == esDataRowState.Added && !entity.applyDefaultsCalled)
                {
                    entity.applyDefaultsCalled = true;
                    entity.ApplyDefaults();

                    if (entity.currentValues != null)
                        return true;
                }

                return false;
            }

            return true;
        }

        internal abstract protected esSmartDtoMap GetMap();

        /// <summary>
        /// A list of column names that have been modified (or that are dirty) since this entity was
        /// retrieved from the database or since it was instantied if created new.
        /// </summary>
        internal List<string> m_modifiedColumns;

        internal esSmartDictionary originalValues;

        internal esSmartDictionary currentValues = new esSmartDictionary();

        internal esDataRowState rowState = esDataRowState.Unchanged;

        internal Dictionary<string, string> getters;

        private bool applyDefaultsCalled;
    }
}
