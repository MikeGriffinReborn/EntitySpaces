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
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

using EntitySpaces.Core;




namespace EntitySpaces.Web
{
    /// <summary>
    /// The EntitySpaces DataSourceControl Implementation for ASP.NET. Supports both runtime and design time
    /// databinding.
    /// </summary>
    [DefaultProperty("State"), 
    PersistChildren(false), 
    DefaultEvent("Select"), 
    ParseChildren(true),
    Description("EntitySpaces DataSourceControl"),
    Designer("EntitySpaces.Web.Design.esDataSourceDesigner, EntitySpaces.Web.Design"),
    AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.None), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.None)]
    public class esDataSource : DataSourceControl
    {
        /// <summary>
        /// Delegate used to invoke the <see cref="esSelect"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceSelectEventHandler(object sender, esDataSourceSelectEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esInsert"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceInsertEventHandler(object sender, esDataSourceInsertEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esUpdate"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceUpdateEventHandler(object sender, esDataSourceUpdateEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esDelete"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceDeleteEventHandler(object sender, esDataSourceDeleteEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esCreateEntity"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceCreateEntityEventHandler(object sender, esDataSourceCreateEntityEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esException"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceExceptionEventHandler(object sender, esDataSourceExceptionEventArgs e);

        /// <summary>
        /// Delegate used to invoke the <see cref="esCreateCollection"/> event.
        /// </summary>
        /// <param name="sender">The esDataSourceView</param>
        /// <param name="e">The event arguments</param>
        public delegate void esDataSourceCreateCollectionEventHandler(object sender, esDataSourceCreateCollectionEventArgs e);

        #region Event Handlers

        /// <summary>
        /// Called before the esSelect event. Typically this is not used.
        /// </summary>
        /// <seealso cref="esSelect"/>
        /// <seealso cref="esPostSelect"/>
        /// <seealso cref="esDataSourceSelectEventArgs"/>
        [esWebDescription("esDataSource_PreSelect"), Category("Select")]
        public event esDataSourceSelectEventHandler esPreSelect
        {
            add
            {
                this.GetView().PreSelectEvent += value;
            }
            remove
            {
                this.GetView().PreSelectEvent -= value;
            }
        }

        /// <summary>
        /// The esSelect event is called when the esDataSource needs you to retreive data, typically
        /// this is done as follows
        /// </summary>
        /// <example>
        /// <code>
        /// protected void EsDataSource1_esSelect(object sender, EntitySpaces.Web.esDataSourceSelectEventArgs e)
        /// {
        ///     // We using AutoPaging and AutoSorting, nothing to do here since we also
        ///     // do not need a Where clause
        ///     OrderDetailsCollection coll = new OrderDetailsCollection();
        ///     e.Collection = coll;
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="esPreSelect"/>
        /// <seealso cref="esPostSelect"/>
        /// <seealso cref="esDataSourceSelectEventArgs"/>
        [esWebDescription("esDataSource_Select"), Category("Select")]
        public event esDataSourceSelectEventHandler esSelect
        {
            add
            {
                this.GetView().SelectEvent += value;
            }
            remove
            {
                this.GetView().SelectEvent -= value;
            }
        }

        /// <summary>
        /// This event can be useful when using either AutoPaging or AutoSorting since the esEntityCollection
        /// is loaded by the esDataSource. If you need to look at the LastQuery results or otherwise need to 
        /// take action after the grid is bound to the collection.
        /// </summary>
        /// <seealso cref="esPreSelect"/>
        /// <seealso cref="esSelect"/>
        /// <seealso cref="esDataSourceSelectEventArgs"/>
        [esWebDescription("esDataSource_PostSelect"), Category("Select")]
        public event esDataSourceSelectEventHandler esPostSelect
        {
            add
            {
                this.GetView().PostSelectEvent += value;
            }
            remove
            {
                this.GetView().PostSelectEvent -= value;
            }
        }

        /// <summary>
        /// Called just before the esInsert event is called.
        /// </summary>
        /// <seealso cref="esInsert"/>
        /// <seealso cref="esPostInsert"/>
        /// <seealso cref="esDataSourceInsertEventArgs"/>
        [esWebDescription("esDataSource_PreInsert"), Category("Insert")]
        public event esDataSourceInsertEventHandler esPreInsert
        {
            add
            {
                this.GetView().PreInsertEvent += value;
            }
            remove
            {
                this.GetView().PreInsertEvent -= value;
            }
        }

        /// <summary>
        /// By default the esDataSource knows how to do the insert, however, if you need to provide a custom 
        /// implementation you can trap this event.
        /// </summary>
        /// <remarks>
        /// If you do trap this event and don't want the default behavior to occur then set the 
        /// esDataSourceInsertEventArgs.EventWasHandled flag to true.
        /// </remarks>
        /// <seealso cref="esPreInsert"/>
        /// <seealso cref="esPostInsert"/>
        /// <seealso cref="esDataSourceInsertEventArgs"/>
        [esWebDescription("esDataSource_Insert"), Category("Insert")]
        public event esDataSourceInsertEventHandler esInsert
        {
            add
            {
                this.GetView().InsertEvent += value;
            }
            remove
            {
                this.GetView().InsertEvent -= value;
            }
        }

        /// <summary>
        /// Called after <see cref="esInsert"/>
        /// </summary>
        /// <seealso cref="esPreInsert"/>
        /// <seealso cref="esInsert"/>
        /// <seealso cref="esDataSourceInsertEventArgs"/>
        [esWebDescription("esDataSource_PostInsert"), Category("Insert")]
        public event esDataSourceInsertEventHandler esPostInsert
        {
            add
            {
                this.GetView().PostInsertEvent += value;
            }
            remove
            {
                this.GetView().PostInsertEvent -= value;
            }
        }

        /// <summary>
        /// By default the esDataSource knows how to do the update, however, if you need to provide a custom 
        /// implementation you can trap this event.
        /// </summary>
        /// <remarks>
        /// If you do trap this event and don't want the default behavior to occur then set the 
        /// esDataSourceUpdateEventArgs.EventWasHandled flag to true.
        /// </remarks>
        /// <seealso cref="esUpdate"/>
        /// <seealso cref="esPostUpdate"/>
        /// <seealso cref="esDataSourceUpdateEventArgs"/>
        [esWebDescription("esDataSource_PreUpdate"), Category("Update")]
        public event esDataSourceUpdateEventHandler esPreUpdate
        {
            add
            {
                this.GetView().PreUpdateEvent += value;
            }
            remove
            {
                this.GetView().PreUpdateEvent -= value;
            }
        }

        /// <summary>
        /// Called just before esUpdate
        /// </summary>
        /// <seealso cref="esPreUpdate"/>
        /// <seealso cref="esPostUpdate"/>
        /// <seealso cref="esDataSourceUpdateEventArgs"/>
        [esWebDescription("esDataSource_Update"), Category("Update")]
        public event esDataSourceUpdateEventHandler esUpdate
        {
            add
            {
                this.GetView().UpdateEvent += value;
            }
            remove
            {
                this.GetView().UpdateEvent -= value;
            }
        }

        /// <summary>
        /// By default the esDataSource knows how to do the delete, however, if you need to provide a custom 
        /// implementation you can trap this event.
        /// </summary>
        /// <remarks>
        /// If you do trap this event and don't want the default behavior to occur then set the 
        /// esDataSourceUpdateEventArgs.EventWasHandled flag to true.
        /// </remarks>
        /// <seealso cref="esPreUpdate"/>
        /// <seealso cref="esUpdate"/>
        /// <seealso cref="esDataSourceUpdateEventArgs"/>
        [esWebDescription("esDataSource_PostUpdate"), Category("Update")]
        public event esDataSourceUpdateEventHandler esPostUpdate
        {
            add
            {
                this.GetView().PostUpdateEvent += value;
            }
            remove
            {
                this.GetView().PostUpdateEvent -= value;
            }
        }

        /// <summary>
        /// Called just before esDelete
        /// </summary>
        /// <seealso cref="esDelete"/>
        /// <seealso cref="esPostDelete"/>
        /// <seealso cref="esDataSourceDeleteEventArgs"/>
        [esWebDescription("esDataSource_PreDelete"), Category("Delete")]
        public event esDataSourceDeleteEventHandler esPreDelete
        {
            add
            {
                this.GetView().PreDeleteEvent += value;
            }
            remove
            {
                this.GetView().PreDeleteEvent -= value;
            }
        }

        /// <summary>
        /// Called just before esDelete
        /// </summary>
        /// <seealso cref="esPreDelete"/>
        /// <seealso cref="esPostDelete"/>
        /// <seealso cref="esDataSourceDeleteEventArgs"/>
        [esWebDescription("esDataSource_Delete"), Category("Delete")]
        public event esDataSourceDeleteEventHandler esDelete
        {
            add
            {
                this.GetView().DeleteEvent += value;
            }
            remove
            {
                this.GetView().DeleteEvent -= value;
            }
        }

        /// <summary>
        /// Called just before esDelete
        /// </summary>
        /// <seealso cref="esPreDelete"/>
        /// <seealso cref="esDelete"/>
        /// <seealso cref="esDataSourceDeleteEventArgs"/>
        [esWebDescription("esDataSource_PostDelete"), Category("Delete")]
        public event esDataSourceDeleteEventHandler esPostDelete
        {
            add
            {
                this.GetView().PostDeleteEvent += value;
            }
            remove
            {
                this.GetView().PostDeleteEvent -= value;
            }
        }

        /// <summary>
        /// The esSelect event is called when the esDataSource needs to work eith a single entity, typically
        /// this is done as follows
        /// </summary>
        /// <example>
        /// <code>
        /// protected void EsDataSource1_esCreateEntity(object sender, EntitySpaces.Web.esDataSourceCreateEntityEventArgs e)
        /// {
        ///     OrderDetails od = new OrderDetails();
        /// 
        ///     if (e.PrimaryKeys == null)
        ///         od.AddNew();
        ///     else
        ///         od.LoadByPrimaryKey((int)e.PrimaryKeys[0], (int)e.PrimaryKeys[1]);
        /// 
        ///     e.Entity = od;
        /// }
        /// </code>
        /// </example>
        [esWebDescription("esDataSource_CreateEntity"), Category("Create")]
        public event esDataSourceCreateEntityEventHandler esCreateEntity
        {
            add
            {
                this.GetView().CreateEntityEvent += value;
            }
            remove
            {
                this.GetView().CreateEntityEvent -= value;
            }
        }

        [esWebDescription("esDataSource_CreateCollection"), Category("Create")]
        public event esDataSourceCreateCollectionEventHandler esCreateCollection
        {
            add
            {
                this.GetView().CreateCollectionEvent += value;
            }
            remove
            {
                this.GetView().CreateCollectionEvent -= value;
            }
        }

        [esWebDescription("esDataSource_Exception"), Category("Exception")]
        public event esDataSourceExceptionEventHandler esException
        {
            add
            {
                this.GetView().ExceptionEvent += value;
            }
            remove
            {
                this.GetView().ExceptionEvent -= value;
            }
        }

        #endregion

        /// <summary>
        /// If True then the esDataSource will handle the paging logic entirely. This will work only
        /// for those databases that have built in paging support such as SQL 2005, Oracle, and MySQL.
        /// </summary>
        /// <remarks>
        /// Both the esDynamicQuery PageSize and PageNumber properties will be populated with
        /// values from the esDataSourceSelectEventArgs class. Also, very important, you create and assign
        /// the esDataSourceSelectEventArgs.Collection property but you must not Load it when AutoPaging is
        /// set to True. The esDataSource will load the collection after assigning the PageSize and PageNumber
        /// properties.
        /// </remarks>
        /// <seealso cref="AutoSorting"/>
        [Category("Data"),
        DefaultValue(false),
        Description("Automatically Handle Paging via esEntityCollection PageSize/PageNumber")]
        public bool AutoPaging
        {
            get { return this.GetView().AutoPaging; }
            set
            {
                if (!value.Equals(this.GetView().AutoPaging))
                {
                    this.GetView().AutoPaging = value;
                    this.RaiseDataSourceChangedEvent(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// If true, then the esDataSource will handle the sorting logic.
        /// </summary>
        /// <remarks>
        /// The <see cref="esDataSourceSelectEventArgs"/> SortItems will be fed to the collections 
        /// esDynamicQuery.OrderBy() method. Also, very important, you create and assign
        /// the esDataSourceSelectEventArgs.Collection property but you must not Load it when AutoSorting
        /// is set to True.
        /// </remarks>
        /// <seealso cref="AutoPaging"/>
        [Category("Data"),
        DefaultValue(false),
        Description("Automatically Handle Sorting")]
        public bool AutoSorting
        {
            get { return this.GetView().AutoSorting; }
            set
            {
                if (!value.Equals(this.GetView().AutoSorting))
                {
                    this.GetView().AutoSorting = value;
                    this.RaiseDataSourceChangedEvent(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// The TotalRowCount is required when <see cref="AutoPaging"/> is set to True. If TotalRowCount is set 
        /// to -1 (which is the default) then it will be set for you. Otherwise, if you plan to set it manually 
        /// the TotalRowCount only needs to be set once per the lifetime of the esDataSource unless the count
        /// is changed via a deleted or inserted record. 
        /// </summary>
        /// <remarks>
        /// This is a sample Page_Load method that loads retrieves the record count one time only
        /// and assigns the esDataSrc.TotalRowCount. It also sets the default sort for the grid at
        /// startup.
        /// <code>
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///    if (!this.Page.IsPostBack)
        ///    {
        ///        Employees emp = new Employees();
        ///        emp.Query.es.CountAll = true;
        ///        emp.Query.es.CountAllAlias = "Count";
        ///        if (emp.Query.Load())
        ///        {
        ///            esDataSrc.TotalRowCount = (int)emp.GetColumn("Count");
        ///        }
        ///
        ///        gridView.Sort(EmployeesMetadata.PropertyNames.LastName, SortDirection.Ascending);
        ///    }
        /// }
        /// </code>
        /// </remarks>
        [Category("Data"),
        DefaultValue(1),
        Description("The Total Number or Rows in the Collection"),
        Browsable(false)]
        public int TotalRowCount
        {
            get { return this.GetView().TotalRowCount; }
            set
            {
                if (!value.Equals(this.GetView().TotalRowCount))
                {
                    this.GetView().TotalRowCount = value;
                    this.RaiseDataSourceChangedEvent(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// This is a Hashtable that can be used to store any serializable data. The State lives
        /// for the lifetime of the esDataSource including beyond postbacks.
        /// </summary>
        /// <remarks>
        /// This example stores the EmployeeID on a esDataSource attached to a detail view so upon the
        /// next page request the record can be loaded.
        /// <code>
        /// this.myDataSource.State["EmployeeID"] = 42;
        /// </code>
        /// </remarks>
        [Category("Data"), 
        Description("A HashTable whose lifespan lives through cross-page requests"),
        Browsable(false)]
        public Hashtable State
        {
            get { return this.GetView().State; }
        }

        #region Views
        static private readonly string[] viewNames = { "DefaultView" };

        public esDataSource()
        {
            this.dataView = new esDataSourceView(this, viewNames[0]);
        }

        protected override DataSourceView GetView(string viewName)
        {
            if (base.IsTrackingViewState)
            {
                ((IStateManager)dataView).TrackViewState();
            }
            return this.dataView;
        }

        [System.Security.SecuritySafeCritical]
        protected esDataSourceView GetView()
        {
            return this.GetView(viewNames[0]) as esDataSourceView;
        }

        protected override ICollection GetViewNames() 
        {
            return viewNames;
        }

        private esDataSourceView dataView;
        internal esEntityCollectionBase Collection;
        #endregion

        #region ViewState Management

        protected override void TrackViewState()
        {
            base.TrackViewState();

            ((IStateManager)this.GetView()).TrackViewState();
        }

        protected override void LoadViewState(object savedState)
        {
            ((IStateManager)this.GetView()).LoadViewState(savedState);
        }

        protected override object SaveViewState()
        {
            return ((IStateManager)this.GetView()).SaveViewState();
        }

        #endregion
    }
}
