/*  New BSD License
-------------------------------------------------------------------------------
Copyright (c) 2006-2012, EntitySpaces, LLC
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:
    * Redistributions of source code must retain the above copyright
      notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
      notice, this list of conditions and the following disclaimer in the
      documentation and/or other materials provided with the distribution.
    * Neither the name of the EntitySpaces, LLC nor the
      names of its contributors may be used to endorse or promote products
      derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL EntitySpaces, LLC BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-------------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using System.Security;
using System.Security.Permissions;


using EntitySpaces.Core;
using EntitySpaces.DynamicQuery;
using EntitySpaces.Interfaces;

//[assembly: SecurityTransparent]

namespace EntitySpaces.Web
{
    /// <summary>
    /// The EntitySpaces DataSourceView
    /// </summary>
    public class esDataSourceView : DataSourceView, IStateManager
    {
        private esDataSource owner;
        private bool tracking = true;
        private DataTable table; // used to get the automatic rowcount

        internal esDataSourceView(esDataSource owner, string name)
            : base(owner, name)
        {
            this.owner = owner;	
        }

        #region IStateManager Members

        bool IStateManager.IsTrackingViewState
        {
            get { return tracking; }
        }

        void IStateManager.LoadViewState(object oState)
        {
            if (oState != null)
            {
                Hashtable ht = (Hashtable)oState;

                this.TotalRowCount = (int)ht["TotalRowCount"];
                this.AutoPaging = (bool)ht["AutoPaging"];
                this.AutoSorting = (bool)ht["AutoSorting"];

                this.state = (Hashtable)ht["State"];
            }
        }

        object IStateManager.SaveViewState()
        {
            Hashtable ht = new Hashtable();
            ht["TotalRowCount"] = this.TotalRowCount;
            ht["AutoPaging"] = this.AutoPaging;
            ht["AutoSorting"] = this.AutoSorting;

            ht["State"] = this.State;

            return ht;
        }

        void IStateManager.TrackViewState()
        {
            tracking = true;
        }

        #endregion

        #region Properties Stored in the ViewState

        public bool AutoPaging
        {
            [System.Security.SecuritySafeCritical]
            get { return this.autoPaging; }

            [System.Security.SecuritySafeCritical]
            set
            {
                if (!value.Equals(this.autoPaging))
                {
                    this.autoPaging = value;
                }
            }
        }
        private bool autoPaging = false;


        public bool AutoSorting
        {
            get { return this.autoSorting; }
            set
            {
                if (!value.Equals(this.autoSorting))
                {
                    this.autoSorting = value;
                }
            }
        }
        private bool autoSorting = false;

        public int TotalRowCount
        {
            get { return this.totalRowCount; }
            set
            {
                if (!value.Equals(this.totalRowCount))
                {
                    this.totalRowCount = value;
                }
            }
        }
        private int totalRowCount = -1;

        public Hashtable State
        {
            get { return this.state; }
            set
            {

            }
        }
        private Hashtable state = new Hashtable();
        #endregion

        public override bool CanSort { get { return true; } }
        public override bool CanPage { get { return true; } }
        public override bool CanRetrieveTotalRowCount { get { return true; } }
        public override bool CanInsert { get { return true; } }
        public override bool CanUpdate { get { return true; } }
        public override bool CanDelete { get { return true; } }

        #region Create EntitySpaces objects Events

        private static readonly object evCreateEntity = new object();
        private static readonly object evCreateCollection = new object();

        public event esDataSource.esDataSourceCreateEntityEventHandler CreateEntityEvent
        {
            add { base.Events.AddHandler(evCreateEntity, value); }
            remove { base.Events.RemoveHandler(evCreateEntity, value); }
        }

        public event esDataSource.esDataSourceCreateCollectionEventHandler CreateCollectionEvent
        {
            add { base.Events.AddHandler(evCreateCollection, value); }
            remove { base.Events.RemoveHandler(evCreateCollection, value); }
        }

        protected virtual void OnCreateEntity(esDataSourceCreateEntityEventArgs e)
        {
            esDataSource.esDataSourceCreateEntityEventHandler handler = base.Events[evCreateEntity]
                as esDataSource.esDataSourceCreateEntityEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCreateCollection(esDataSourceCreateCollectionEventArgs e)
        {
            esDataSource.esDataSourceCreateCollectionEventHandler handler = base.Events[evCreateCollection]
                as esDataSource.esDataSourceCreateCollectionEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        #endregion

        #region Select Logic

        private static readonly object evPreSelect = new object();
        private static readonly object evSelect = new object();
        private static readonly object evPostSelect = new object();

        public event esDataSource.esDataSourceSelectEventHandler PreSelectEvent
        {
            add { base.Events.AddHandler(evPreSelect, value); }
            remove { base.Events.RemoveHandler(evPreSelect, value); }
        }

        public event esDataSource.esDataSourceSelectEventHandler SelectEvent
        {
            add { base.Events.AddHandler(evSelect, value); }
            remove { base.Events.RemoveHandler(evSelect, value); }
        }

        public event esDataSource.esDataSourceSelectEventHandler PostSelectEvent
        {
            add { base.Events.AddHandler(evPostSelect, value); }
            remove { base.Events.RemoveHandler(evPostSelect, value); }
        }

        protected virtual void OnPreSelect(esDataSourceSelectEventArgs e)
        {
            esDataSource.esDataSourceSelectEventHandler handler = base.Events[evPreSelect]
                as esDataSource.esDataSourceSelectEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSelect(esDataSourceSelectEventArgs e)
        {
            esDataSource.esDataSourceSelectEventHandler handler = base.Events[evSelect]
                as esDataSource.esDataSourceSelectEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPostSelect(esDataSourceSelectEventArgs e)
        {
            esDataSource.esDataSourceSelectEventHandler handler = base.Events[evPostSelect]
                as esDataSource.esDataSourceSelectEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [System.Security.SecuritySafeCritical]
        protected override System.Collections.IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            esDataSourceSelectEventArgs e = null;

            try
            {
                e = new esDataSourceSelectEventArgs();

                e.Arguments = arguments;
                e.Collection = this.owner.Collection;

                this.OnPreSelect(e);

                if (e.Cancel)
                    return null;

                this.CalculatePageSizeAndNumber(e);
                this.SetTotalRowCount(e);
                this.PopulateSortItems(e);

                //this.OnPreSelect(e);
                this.OnSelect(e);

                this.PerformAutoLogic(e);

                this.FetchTotalRowCount(e);

                this.OnPostSelect(e);

                return e.Collection;
            }
            catch (Exception ex)
            {
                esDataSourceExceptionEventArgs exArgs = new esDataSourceExceptionEventArgs(ex);
                exArgs.EventType = esDataSourceEventType.Select;
                exArgs.SelectArgs = e;

                try
                {
                    this.OnException(exArgs);
                }
                catch { }

                if (!exArgs.ExceptionWasHandled)
                {
                    throw;
                }

                return null;
            }
        }
        
        [System.Security.SecuritySafeCritical]
        public override void Select(DataSourceSelectArguments arguments, DataSourceViewSelectCallback callback)
        {
            esDataSourceSelectEventArgs e = null;

            try
            {
                e = new esDataSourceSelectEventArgs();

                e.Arguments = arguments;
                e.Collection = this.owner.Collection;

                this.OnPreSelect(e);

                if (e.Cancel)
                    return;

                this.CalculatePageSizeAndNumber(e);
                this.SetTotalRowCount(e);
                this.PopulateSortItems(e);

                //this.OnPreSelect(e);
                
                this.OnSelect(e);

                this.PerformAutoLogic(e);

                this.FetchTotalRowCount(e);

                this.OnPostSelect(e);

                if (e.Collection != null)
                {
                    this.owner.Collection = e.Collection;
                }
            }
            catch (Exception ex)
            {
                esDataSourceExceptionEventArgs exArgs = new esDataSourceExceptionEventArgs(ex);
                exArgs.EventType = esDataSourceEventType.Select;
                exArgs.SelectArgs = e;

                try
                {
                    this.OnException(exArgs);
                }
                catch { }

                if (!exArgs.ExceptionWasHandled)
                {
                    throw;
                }
            }
            finally
            {
                callback(e.Collection);
            }
        }

        private void CalculatePageSizeAndNumber(esDataSourceSelectEventArgs e)
        {
            // Calc PageSize/PageNumber
            if (e.Arguments.MaximumRows > 0)
            {
                e.PageSize = e.Arguments.MaximumRows;
                e.PageNumber = (int)((e.Arguments.StartRowIndex / e.Arguments.MaximumRows) + 1);
            }
        }

        private void FetchTotalRowCount(esDataSourceSelectEventArgs e)
        {
            esDynamicQuery query = e.Query != null ? e.Query : e.Collection.es.Query;

            var iQuery = query as IDynamicQueryInternal;

            if (e.Arguments.RetrieveTotalRowCount)
            {
                if (this.totalRowCount == -1)
                {
                    #region Backup 
                    //
                    // Back up everything cause we're going to restore it
                    //
                    List<esExpression> select = iQuery.InternalSelectColumns;
                    List<esOrderByItem> orderBy = iQuery.InternalOrderByItems;

                    int? pageNumber = query.InternalPageNumber;
                    int? pageSize = query.InternalPageSize;
                    string lastQuery = query.es.LastQuery;

                    EntitySpaces.Interfaces.esDynamicQuery.QueryLoadedDelegate origDelegate = query.OnLoadDelegate;
                    #endregion

                    //
                    // Clear some stuff so we can make our query
                    //
                    iQuery.InternalSelectColumns = null;
                    iQuery.InternalOrderByItems = null;

                    query.InternalPageNumber = null;
                    query.InternalPageSize = null;

                    query.Select(query.Count().As("Count"));

                    object o = query.OnLoadDelegate;

                    try
                    {
                        query.OnLoadDelegate = this.OnQueryLoaded;

                        if (query.Load())
                        {
                          this.totalRowCount = Convert.ToInt32(table.Rows[0]["Count"]);
                        }
                    }
                    finally
                    {
                        #region Restore 
                        iQuery.InternalSelectColumns = select;
                        iQuery.InternalOrderByItems = orderBy;

                        query.InternalPageNumber = pageNumber;
                        query.InternalPageSize = pageSize;

                        query.OnLoadDelegate = origDelegate;
                        iQuery.LastQuery = lastQuery;

                        #endregion Restore
                    }
                }

                e.Arguments.TotalRowCount = this.totalRowCount;
            }
        }

        protected bool OnQueryLoaded(esDynamicQuery Query, DataTable table)
        {
            this.table = table;
            return true;
        }

        private void SetTotalRowCount(esDataSourceSelectEventArgs e)
        {
            // Set TotalRowCount if we can
            if (e.Arguments.RetrieveTotalRowCount && this.totalRowCount > 0)
            {
                e.Arguments.TotalRowCount = this.totalRowCount;
            }
        }

        private void PopulateSortItems(esDataSourceSelectEventArgs e)
        {
            //-----------------------------------------
            // Populate the esDataSourceSortItem's
            //-----------------------------------------
            if (e.Arguments.SortExpression != null && e.Arguments.SortExpression.Length > 0)
            {
                e.SortItems = new List<esDataSourceSortItem>();

                string[] entries = e.Arguments.SortExpression.Split(',');

                for (int i = 0; i < entries.Length; i++)
                {
                    esDataSourceSortItem sortItem = new esDataSourceSortItem();

                    string sortEntry = entries[i].TrimEnd().TrimStart();

                    //------------------------------------
                    // Determine the Sort Direction
                    //------------------------------------
                    int index = sortEntry.IndexOf(' ');

                    if (index == -1)
                    {
                        sortItem.Direction = esOrderByDirection.Ascending;
                    }
                    else if (sortEntry.Contains(" DESC") || sortEntry.Contains(" desc") || sortEntry.Contains(" Desc"))
                    {
                        sortItem.Direction = esOrderByDirection.Descending;
                    }
                    else
                    {
                        sortItem.Direction = esOrderByDirection.Ascending;
                    }

                    //------------------------------------
                    // Determine the Property Name
                    //------------------------------------
                    if (index == -1)
                    {
                        sortItem.Property = sortEntry;
                    }
                    else
                    {
                        sortItem.Property = sortEntry.Substring(0, index);
                    }

                    e.SortItems.Add(sortItem);
                }
            }
        }

        private void PerformAutoLogic(esDataSourceSelectEventArgs e)
        {
            esDynamicQuery query = e.Query  != null ? e.Query : e.Collection.es.Query;

            var iQuery = query as IDynamicQueryInternal;

            if (this.autoPaging)
            {
                query.PageNumber(e.PageNumber);
                query.PageSize(e.PageSize);
            }

            if (this.autoSorting)
            {
                if (e.SortItems != null)
                {
                    foreach (esDataSourceSortItem sortItem in e.SortItems)
                    {
                        esColumnMetadata col = e.Collection.es.Meta.Columns.FindByPropertyName(sortItem.Property);
                        if(col != null)
                        {
                            query.OrderBy(col.Name, sortItem.Direction);
                        }
                        else if (sortItem.Property[0] == '<')
                        {
                            query.OrderBy(sortItem.Property, sortItem.Direction);
                        }
                    }
                }
                else
                {
                    if (this.AutoPaging)
                    {
                        List<esColumnMetadata> pks = e.Collection.es.Meta.Columns.PrimaryKeys;
                        if (pks != null)
                        {
                            foreach (esColumnMetadata pk in pks)
                            {
                                query.OrderBy(pk.Name, esOrderByDirection.Ascending);
                            }
                        }
                    }
                }
            }

            if (this.autoSorting || this.AutoPaging)
            {
                if (e.Query != null)
                {
                    IEntityCollection iColl = e.Collection as IEntityCollection;
                    iColl.HookupQuery(query);
                }

                 query.Load();
            }
        }

        #endregion

        #region Insert Logic

        private static readonly object evPreInsert = new object();
        private static readonly object evInsert = new object();
        private static readonly object evPostInsert = new object();

        public event esDataSource.esDataSourceInsertEventHandler PreInsertEvent
        {
            add { base.Events.AddHandler(evPreInsert, value); }
            remove { base.Events.RemoveHandler(evPreInsert, value); }
        }

        public event esDataSource.esDataSourceInsertEventHandler InsertEvent
        {
            add { base.Events.AddHandler(evInsert, value); }
            remove { base.Events.RemoveHandler(evInsert, value); }
        }

        public event esDataSource.esDataSourceInsertEventHandler PostInsertEvent
        {
            add { base.Events.AddHandler(evPostInsert, value); }
            remove { base.Events.RemoveHandler(evPostInsert, value); }
        }

        protected virtual void OnPreInsert(esDataSourceInsertEventArgs e)
        {
            esDataSource.esDataSourceInsertEventHandler handler = base.Events[evPreInsert]
                as esDataSource.esDataSourceInsertEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnInsert(esDataSourceInsertEventArgs e)
        {
            esDataSource.esDataSourceInsertEventHandler handler = base.Events[evInsert]
                as esDataSource.esDataSourceInsertEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPostInsert(esDataSourceInsertEventArgs e)
        {
            esDataSource.esDataSourceInsertEventHandler handler = base.Events[evPostInsert]
                as esDataSource.esDataSourceInsertEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [System.Security.SecuritySafeCritical]
        public override void Insert(System.Collections.IDictionary values, DataSourceViewOperationCallback callback)
        {
            esDataSourceInsertEventArgs e = null;

            try
            {
                e = new esDataSourceInsertEventArgs();
                e.Values = values;

                this.OnPreInsert(e);
                if (e.Cancel)
                    return;

                esDataSourceCreateEntityEventArgs ce = new esDataSourceCreateEntityEventArgs();
                this.OnCreateEntity(ce);

                esEntity entity = ce.Entity;
                e.Entity = entity;

                if (entity != null)
                {
                    entity.SetProperties(values);
                }

                //this.OnPreInsert(e);

                e.EventWasHandled = false;
                this.OnInsert(e);

                if (!e.EventWasHandled)
                {
                    entity.Save();
                    this.OnDataSourceViewChanged(EventArgs.Empty);
                }

                e.EventWasHandled = false;
                this.OnPostInsert(e);
            }
            catch (Exception ex)
            {
                esDataSourceExceptionEventArgs exArgs = new esDataSourceExceptionEventArgs(ex);
                exArgs.EventType = esDataSourceEventType.Insert;
                exArgs.InsertArgs = e;

                try
                {
                    this.OnException(exArgs);
                }
                catch { }

                if (!exArgs.ExceptionWasHandled)
                {
                    throw;
                }
            }
            finally
            {
                callback(1, null);
            }
        }

        #endregion

        #region Update Logic

        private static readonly object evPreUpdate = new object();
        private static readonly object evUpdate = new object();
        private static readonly object evPostUpdate = new object();

        public event esDataSource.esDataSourceUpdateEventHandler PreUpdateEvent
        {
            add { base.Events.AddHandler(evPreUpdate, value); }
            remove { base.Events.RemoveHandler(evPreUpdate, value); }
        }

        public event esDataSource.esDataSourceUpdateEventHandler UpdateEvent
        {
            add { base.Events.AddHandler(evUpdate, value); }
            remove { base.Events.RemoveHandler(evUpdate, value); }
        }

        public event esDataSource.esDataSourceUpdateEventHandler PostUpdateEvent
        {
            add { base.Events.AddHandler(evPostUpdate, value); }
            remove { base.Events.RemoveHandler(evPostUpdate, value); }
        }

        protected virtual void OnPreUpdate(esDataSourceUpdateEventArgs e)
        {
            esDataSource.esDataSourceUpdateEventHandler handler = base.Events[evPreUpdate]
                as esDataSource.esDataSourceUpdateEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnUpdate(esDataSourceUpdateEventArgs e)
        {
            esDataSource.esDataSourceUpdateEventHandler handler = base.Events[evUpdate]
                as esDataSource.esDataSourceUpdateEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPostUpdate(esDataSourceUpdateEventArgs e)
        {
            esDataSource.esDataSourceUpdateEventHandler handler = base.Events[evPostUpdate]
                as esDataSource.esDataSourceUpdateEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [System.Security.SecuritySafeCritical]
        public override void Update
        (
            System.Collections.IDictionary keys,
            System.Collections.IDictionary values,
            System.Collections.IDictionary oldValues,
            DataSourceViewOperationCallback callback
        )
        {
            esDataSourceUpdateEventArgs e = null;

            try
            {
                if (keys != null && keys.Count > 0)
                {
                    e = new esDataSourceUpdateEventArgs();

                    e.Keys = keys;
                    e.Values = values;
                    e.OldValues = oldValues;

                    this.OnPreUpdate(e);
                    if (e.Cancel)
                        return;

                    // Find the proper esEntity and set it's values
                    object[] pks = new object[keys.Count];

                    int index = 0;
                    foreach (object value in keys.Values)
                    {
                        pks[index++] = value;
                    }

                    esDataSourceCreateEntityEventArgs ce = new esDataSourceCreateEntityEventArgs();
                    ce.PrimaryKeys = pks;
                    this.OnCreateEntity(ce);

                    esEntity entity = ce.Entity;
                    e.Entity = entity;

                    //this.OnPreUpdate(e);

                    if (entity != null)
                    {
                        entity.SetProperties(values);
                    }

                    e.EventWasHandled = false;
                    this.OnUpdate(e);

                    if (!e.EventWasHandled)
                    {
                        entity.Save();
                        this.OnDataSourceViewChanged(EventArgs.Empty);
                    }

                    this.OnPostUpdate(e);
                }
            }
            catch (Exception ex)
            {
                esDataSourceExceptionEventArgs exArgs = new esDataSourceExceptionEventArgs(ex);
                exArgs.EventType = esDataSourceEventType.Delete;
                exArgs.UpdateArgs = e;

                try
                {
                    this.OnException(exArgs);
                }
                catch { }

                if (!exArgs.ExceptionWasHandled)
                {
                    throw;
                }
            }
            finally
            {
                callback(1, null);
            }
        }

        #endregion

        #region Delete Logic

        private static readonly object evPreDelete = new object();
        private static readonly object evDelete = new object();
        private static readonly object evPostDelete = new object();

        public event esDataSource.esDataSourceDeleteEventHandler PreDeleteEvent
        {
            add { base.Events.AddHandler(evPreDelete, value); }
            remove { base.Events.RemoveHandler(evPreDelete, value); }
        }

        public event esDataSource.esDataSourceDeleteEventHandler DeleteEvent
        {
            add { base.Events.AddHandler(evDelete, value); }
            remove { base.Events.RemoveHandler(evDelete, value); }
        }

        public event esDataSource.esDataSourceDeleteEventHandler PostDeleteEvent
        {
            add { base.Events.AddHandler(evPostDelete, value); }
            remove { base.Events.RemoveHandler(evPostDelete, value); }
        }

        protected virtual void OnPreDelete(esDataSourceDeleteEventArgs e)
        {
            esDataSource.esDataSourceDeleteEventHandler handler = base.Events[evPreDelete]
                as esDataSource.esDataSourceDeleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDelete(esDataSourceDeleteEventArgs e)
        {
            esDataSource.esDataSourceDeleteEventHandler handler = base.Events[evDelete]
                as esDataSource.esDataSourceDeleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPostDelete(esDataSourceDeleteEventArgs e)
        {
            esDataSource.esDataSourceDeleteEventHandler handler = base.Events[evPostDelete]
                as esDataSource.esDataSourceDeleteEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [System.Security.SecuritySafeCritical]
        public override void Delete(System.Collections.IDictionary keys, System.Collections.IDictionary oldValues, DataSourceViewOperationCallback callback)
        {
            esDataSourceDeleteEventArgs e = null;

            try
            {
                if (keys != null && keys.Count > 0)
                {
                    e = new esDataSourceDeleteEventArgs();
                    e.Keys = keys;
                    e.OldValues = oldValues;

                    this.OnPreDelete(e);
                    if (e.Cancel)
                        return;

                    // Find the proper esEntity and set it's values
                    object[] pks = new object[keys.Count];

                    int index = 0;
                    foreach (object value in keys.Values)
                    {
                        pks[index++] = value;
                    }

                    esDataSourceCreateEntityEventArgs ce = new esDataSourceCreateEntityEventArgs();
                    ce.PrimaryKeys = pks;
                    this.OnCreateEntity(ce);

                    esEntity entity = ce.Entity;
                    e.Entity = entity;

                    //this.OnPreDelete(e);

                    e.EventWasHandled = false;
                    this.OnDelete(e);

                    if (!e.EventWasHandled)
                    {
                        entity.MarkAsDeleted();
                        entity.Save();

                        this.OnDataSourceViewChanged(EventArgs.Empty);
                    }

                    this.OnPostDelete(e);
                }
            }
            catch (Exception ex)
            {
                esDataSourceExceptionEventArgs exArgs = new esDataSourceExceptionEventArgs(ex);
                exArgs.EventType = esDataSourceEventType.Delete;
                exArgs.DeleteArgs = e;

                try
                {
                    this.OnException(exArgs);
                }
                catch { }

                if (!exArgs.ExceptionWasHandled)
                {
                    throw;
                }
            }
            finally
            {
                callback(1, null);
            }
        }

        #endregion

        #region Exception Logic

        private static readonly object evException = new object();

        public event esDataSource.esDataSourceExceptionEventHandler ExceptionEvent
        {
            add { base.Events.AddHandler(evException, value); }
            remove { base.Events.RemoveHandler(evException, value); }
        }

        protected virtual void OnException(esDataSourceExceptionEventArgs e)
        {
            esDataSource.esDataSourceExceptionEventHandler handler = base.Events[evException]
                as esDataSource.esDataSourceExceptionEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        public void ResetTotalRowCount()
        {
            this.TotalRowCount = -1;
        }
    }
}
