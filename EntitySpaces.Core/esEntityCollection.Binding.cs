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
using System.ComponentModel;

using System.Xml.Serialization;

#if (LINQ)
using System.Data.Linq;
using System.Linq;
#endif

#if (!WindowsCE)
using System.Runtime.Serialization;
#endif

using EntitySpaces.Interfaces;
using EntitySpaces.DynamicQuery;

namespace EntitySpaces.Core
{
    abstract public partial class esEntityCollection<T> : esEntityCollectionBase, IList<T>, ITypedList, IBindingList, ICancelAddNew, IRaiseItemChangedEvents
        where T : esEntity, new()
    {
        #region IList<T> Members

        int IList<T>.IndexOf(T item)
        {
#if (TRACE)
            Console.WriteLine("int IList<T>.IndexOf(T item)");
#endif
            IList<T> i = entities as IList<T>;
            return i.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
#if (TRACE)
            Console.WriteLine("void IList<T>.Insert(int index, T item)");
#endif
            IList<T> i = entities as IList<T>;
            i.Insert(index, item);
        }

        void IList<T>.RemoveAt(int index)
        {
#if (TRACE)
            Console.WriteLine("void IList<T>.RemoveAt(int index)");
#endif
            esEntity entity = ((IList)entities)[index] as esEntity;

            entities.Remove((T)entity);

            entity.MarkAsDeleted();

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
            }

            if (updateViewNotification != null)
            {
                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, entity);
            }
        }

        T IList<T>.this[int index]
        {
            get
            {
#if (TRACE)
                Console.WriteLine("T IList<T>.this[int index] - GET");
#endif
                IList<T> i = entities as IList<T>;
                return i[index];
            }

            set
            {
#if (TRACE)
                Console.WriteLine("T IList<T>.this[int index] - SET");
#endif
                IList<T> i = entities as IList<T>;
                i[index] = value;

                if (updateViewNotification != null)
                {
                    OnUpdateViewNotification(this, ListChangedType.ItemChanged, (esEntity)value);
                }
            }
        }

        #endregion

        #region ICollection<T> Members

//      void ICollection<T>.Add(T item)
        public void Add(T item)
        {
#if (TRACE)
            Console.WriteLine("void ICollection<T>.Add(T item)");
#endif
            item.Collection = this;

            if (item.RowState == esDataRowState.Deleted)
            {
                if (deletedEntities == null)
                {
                    deletedEntities = new BindingList<T>();
                }

                this.deletedEntities.Add(item);
            }
            else
            {
                ICollection<T> i = entities as ICollection<T>;
                i.Add(item);

                if (updateViewNotification != null)
                {
                    OnUpdateViewNotification(this, ListChangedType.ItemAdded, item);
                }
            }
        }

        void ICollection<T>.Clear()
        {
#if (TRACE)
            Console.WriteLine("void ICollection<T>.Clear()");
#endif
            ICollection<T> i = entities as ICollection<T>;
            i.Clear();
        }

        bool ICollection<T>.Contains(T item)
        {
#if (TRACE)
            Console.WriteLine("bool ICollection<T>.Contains(T item)");
#endif
            ICollection<T> i = entities as ICollection<T>;
            return i.Contains(item);
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
#if (TRACE)
            Console.WriteLine("void ICollection<T>.CopyTo(T[] array, int arrayIndex)");
#endif
            ICollection<T> i = entities as ICollection<T>;
            i.CopyTo(array, arrayIndex);
        }

        int ICollection<T>.Count
        {
            get
            {
#if (TRACE)
                Console.WriteLine("int ICollection<T>.Count");
#endif
                ICollection<T> i = entities as ICollection<T>;
                return i.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
#if (TRACE)
                Console.WriteLine("bool ICollection<T>.IsReadOnly");
#endif
                ICollection<T> i = entities as ICollection<T>;
                return i.IsReadOnly;
            }
        }

        bool ICollection<T>.Remove(T item)
        {
#if (TRACE)
            Console.WriteLine("bool ICollection<T>.Remove(T item)");
#endif
            esEntity entity = item as esEntity;

            bool removed = entities.Remove((T)entity);

            entity.MarkAsDeleted();

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
            }

            if (updateViewNotification != null)
            {
                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, entity);
            }

            return removed;
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return entities.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            IEnumerator enumerator = ((ICollection)entities).GetEnumerator();
            return enumerator;
        }

        #endregion

        #region ITypedList Members

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            // We are binding so we turn this on ...
            entities.RaiseListChangedEvents = true;

            PropertyDescriptorCollection props = null;
            Type type = null;

            if (listAccessors == null || listAccessors.Length == 0)
            {
                esEntity e = null;

                // It wants "this" object and so we can use this technique
                if (this.Count > 0)
                {
                    e = entities[0] as esEntity;
                }
                else
                {
                    e = new T();
                }

                type = e.GetType();

                //------------------------------------
                // Check to see if it's in the cached
                //------------------------------------
                if (this.BindingPropertyCache.ContainsKey(type))
                {
                    return this.BindingPropertyCache[type];
                }

                props = this.GetProperties(e, this);

                this.BindingPropertyCache[type] = props;
            }
            else
            {
                if (this.EnableHierarchicalBinding == false) return null;

                // We should not enter this else statement if hierarchical binding is false
                PropertyDescriptor prop = listAccessors[listAccessors.Length - 1];

                //------------------------------------
                // Check to see if it's in the cached
                //------------------------------------
                if (this.BindingPropertyCache.ContainsKey(prop.PropertyType))
                {
                    return this.BindingPropertyCache[prop.PropertyType];
                }

                esPropertyDescriptor esProp = prop as esPropertyDescriptor;

                if (esProp != null)
                {
                    // Nope, not in the cache, let's get the info
                    props = this.GetProperties(esProp.ContainedEntity, null);
                }
                else
                {
                    // I give up, go get the raw properties
                    props = TypeDescriptor.GetProperties(prop.PropertyType);
                }

                this.BindingPropertyCache[prop.PropertyType] = props;
            }

            return props;
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            return GetCollectionName();
        }

        internal override PropertyDescriptorCollection GetProperties(esEntity entity, esEntityCollectionBase baseCollection)
        {
            bool weHaveData = false;
            int lastOrdinal = 0;

            esColumnMetadataCollection esMetaCols = entity.es.Meta.Columns;

            esEntityCollectionBase theBaseCollection = baseCollection != null ? baseCollection : entity.Collection;

            bool enableHierarchcialBinding = theBaseCollection != null ? theBaseCollection.EnableHierarchicalBinding : true;

            if (theBaseCollection != null)
            {
                if (theBaseCollection.GetList() != null)
                {
                    // Do we have any entities?
                    weHaveData = theBaseCollection.GetList().Count > 0;

                    if (weHaveData == false)
                    {
                        // If selectedColumns has data then they attempted a load and we know the columns based on thier select statement
                        weHaveData = theBaseCollection.selectedColumns != null && theBaseCollection.selectedColumns.Keys.Count > 0;
                    }
                }
            }

            //------------------------------------------------------------
            // First we deal with Properties from the DataTable.Columns
            // or from the esColumnMetadataCollection.
            //------------------------------------------------------------
            ArrayList collNested = new ArrayList();
            SortedList<int, PropertyDescriptor> coll = new SortedList<int, PropertyDescriptor>();

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(entity, true);

            // Note, we check for selectedColumns because we might be a deserialized collection in which
            // case there will not be any selectedColumns
            if (weHaveData && theBaseCollection.selectedColumns != null)
            {
                SortedList list = GetSortedListOfProperties(entity, baseCollection);

                for (int i = 0; i < list.Count; i++)
                {
                    string column = (string)list.GetByIndex(i);

                    if (column == "ESRN") continue;

                    esColumnMetadata esCol = entity.es.Meta.Columns.FindByColumnName(column);

                    if (esCol != null)
                    {
                        PropertyDescriptor prop = props[esCol.PropertyName];

                        if (prop != null)
                        {
                            coll.Add(lastOrdinal++, prop);
                        }
                    }
                    else
                    {
                        esCol = theBaseCollection.extraColumnMetadata[column];

                        if (esCol != null)
                        {
                            // Extra or Extended Properties
                            esPropertyDescriptor dpd = new esPropertyDescriptor
                            (
                                typeof(T),
                                column,
                                esCol != null ? esCol.Type : typeof(string),
                                delegate(object p)
                                {
                                    return ((esEntity)p).currentValues[column];
                                },
                                delegate(object p, object data)
                                {
                                    ((esEntity)p).currentValues[column] = data;
                                    ((esEntity)p).OnPropertyChanged(column);
                                }
                            );

                            coll.Add(lastOrdinal++, dpd);
                        }
                    }
                }
            }
            else
            {
                foreach (esColumnMetadata esCol in esMetaCols)
                {
                    coll.Add(lastOrdinal++, props[esCol.PropertyName]);
                }
            }

            //------------------------------------------------------------
            // Now we deal with extended properties that are using the
            // esExtendedPropertyAttribute technique
            //------------------------------------------------------------
#if (!WindowsCE)
            foreach (PropertyDescriptor prop in props)
            {
                if (prop.Attributes.Contains(esEntityCollection<T>.extendedPropertyAttribute))
                {
                    coll.Add(lastOrdinal++, prop);
                }
            }
#endif

            //------------------------------------------------------------
            // Now we deal with any local properties. Local properties are
            // properties that users may want to bind with that are
            // NOT backed by data in the DataTable
            //------------------------------------------------------------
            List<esPropertyDescriptor> localProps = entity.GetLocalBindingProperties();
            if (localProps != null)
            {
                foreach (esPropertyDescriptor esProp in localProps)
                {
                    // We check this incase they add a local based property for a DataColumn
                    // based property, they would do this so it appears in design time, and 
                    // we don't want to add a duplicate
                    bool exists = coll.ContainsValue(props[esProp.Name]);

                    if (!exists)
                    {
                        if (props[esProp.Name] != null)
                        {
                            coll.Add(lastOrdinal++, props[esProp.Name]);
                        }
                        else
                        {
                            coll.Add(lastOrdinal++, esProp);
                        }
                    }
                }
            }

            ArrayList tempColl = new ArrayList();

            if (enableHierarchcialBinding)
            {
                List<esPropertyDescriptor> hierProps = entity.GetHierarchicalProperties();
                if (hierProps != null)
                {
                    foreach (esPropertyDescriptor esProp in hierProps)
                    {
                        esProp.TrueDescriptor = props[esProp.Name];
                    //  coll.Add(lastOrdinal++, esProp);

                        tempColl.Add(esProp);
                    }
                }
            }

            // Create the collection
            foreach (PropertyDescriptor p in coll.Values)
            {
                tempColl.Add(p);
            }
            tempColl.AddRange(collNested);

            PropertyDescriptorCollection theProps =
                new PropertyDescriptorCollection((PropertyDescriptor[])tempColl.ToArray(typeof(PropertyDescriptor)));

            return theProps;
        }

        private SortedList GetSortedListOfProperties(esEntity entity, esEntityCollectionBase baseCollection)
        {
            SortedList list = new SortedList();

            esEntityCollectionBase theBaseCollection = baseCollection != null ? baseCollection : entity.Collection;

            if (theBaseCollection != null)
            {
                if (theBaseCollection.selectedColumns != null)
                {
                    foreach (KeyValuePair<string, int> selectedColumn in theBaseCollection.selectedColumns)
                    {
                        list.Add(selectedColumn.Value, selectedColumn.Key);
                    }
                }

                if (theBaseCollection.extraColumnMetadata != null)
                {
                    foreach (KeyValuePair<string, esColumnMetadata> extraColumn in theBaseCollection.extraColumnMetadata)
                    {
                        list.Add(extraColumn.Value.Ordinal, extraColumn.Key);
                    }
                }
            }

            return list;
        }

        #endregion

        #region IBindingList Members

        void IBindingList.AddIndex(PropertyDescriptor property)
        {
#if(TRACE)
            Console.WriteLine("object IBindingList.AddNew()");
#endif
            IBindingList i = entities as IBindingList;
            i.AddIndex(property);
        }

        object IBindingList.AddNew()
        {
#if(TRACE)
            //Console.WriteLine("object IBindingList.AddNew()");
#endif
            T o = (T)objectCreator.CreateInstance();
            o.Collection = this;
            lastNewIndex = entities.Count;
            entities.Add(o);

            if (entitiesFilterBackup != null)
            {
                IBindingList i = entitiesFilterBackup as IBindingList;
                i.Add(o);
            }

            if (updateViewNotification != null)
            {
                OnUpdateViewNotification(this, ListChangedType.ItemAdded, o);
            }

            return o;
        }

        /// <summary>
        /// True if list items can be edited; otherwise, false. The default is true. If you set this value you are also setting IBindingList.AllowEdit.
        /// </summary>
        public bool AllowEdit
        {
            get
            {
                if (this.allowEdit.HasValue)
                {
                    return this.allowEdit.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowEdit;
                }
            }

            set
            {
                this.allowEdit = value;
            }
        }

        bool IBindingList.AllowEdit
        {
            get
            {
                if (this.allowEdit.HasValue)
                {
                    return this.allowEdit.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowEdit;
                }
            }
        }

        /// <summary>
        /// True if you can add items to the list with the AddNew method; otherwise, false. The default depends on the underlying type contained in the list. If you set this value you are also setting IBindingList.AllowNew.
        /// </summary>
        public bool AllowNew
        {
            get
            {
                if (this.allowNew.HasValue)
                {
                    return this.allowNew.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowNew;
                }
            }

            set
            {
                this.allowNew = value;
            }
        }

        bool IBindingList.AllowNew
        {
            get
            {
                if (this.allowNew.HasValue)
                {
                    return this.allowNew.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowNew;
                }
            }
        }

        /// <summary>
        /// True if you can add items to the list with the AddNew method; otherwise, false. The default depends on the underlying type contained in the list. 
        /// If you set this value you are also setting IBindingList.AllowRemove.
        /// </summary>
        public bool AllowDelete
        {
            get
            {
                if (this.allowDelete.HasValue)
                {
                    return this.allowDelete.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowRemove;
                }
            }

            set
            {
                this.allowDelete = value;
            }
        }

        bool IBindingList.AllowRemove
        {
            get
            {
                if (this.allowDelete.HasValue)
                {
                    return this.allowDelete.Value;
                }
                else
                {
                    IBindingList i = entities as IBindingList;
                    return i.AllowRemove;
                }
            }
        }

        void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
#if(TRACE)
            Console.WriteLine("void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)");
#endif
            isSorted = true;

            IBindingList i = entities as IBindingList;

            this.sortProperty = property;

            esEntityComparer<T> comparer = new esEntityComparer<T>(this.sortProperty, this.sortDirection);

            List<T> sortList = new List<T>(entities);
            sortList.Sort(comparer);

            if (sortDirection == ListSortDirection.Descending)
            {
                sortList.Reverse();
            }

            RaiseListChangeEvents_Disable();
            entities.Clear();

            foreach (T obj in sortList)
            {
                entities.Add(obj);
            }
            RaiseListChangeEvents_Restore();

            OnListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, 0));

            sortDirection = (sortDirection == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        int IBindingList.Find(PropertyDescriptor property, object key)
        {
#if(TRACE)
            Console.WriteLine("int IBindingList.Find(PropertyDescriptor property, object key)");
#endif
            int index = -1;

            try
            {
                int tempIndex = -1;

                object value = null;

                foreach (esEntity entity in this)
                {
                    tempIndex++;

                    value = entity.GetColumn(property.Name);

                    if (value != null && value.Equals(key))
                    {
                        index = tempIndex;
                        break;
                    }
                    else if (value == null && key == null)
                    {
                        index = tempIndex;
                        break;
                    }
                }
            }
            catch { }

            return index;
        }

        bool IBindingList.IsSorted
        {
            get
            {
                return isSorted;
            }
        }

        event ListChangedEventHandler IBindingList.ListChanged
        {
            add
            {
#if(TRACE)
                Console.WriteLine("event ListChangedEventHandler IBindingList.ListChanged - ADD");
#endif
                listChangedEventHandlerCount++;
                onListChangedEvent += value;

                if (entities.RaiseListChangedEvents == false)
                {
                    entities.RaiseListChangedEvents = true;
                }
            }
            remove
            {
#if(TRACE)
                Console.WriteLine("event ListChangedEventHandler IBindingList.ListChanged - REMOVE");
#endif
                listChangedEventHandlerCount = Math.Max(0, --listChangedEventHandlerCount);
                onListChangedEvent -= value;

                if (listChangedEventHandlerCount == 0)
                {
                    entities.RaiseListChangedEvents = false;
                }
            }
        }

        void IBindingList.RemoveIndex(PropertyDescriptor property)
        {
#if(TRACE)
            Console.WriteLine("void IBindingList.RemoveIndex(PropertyDescriptor property)");
#endif
            IBindingList i = entities as IBindingList;
            i.RemoveIndex(property);
        }

        void IBindingList.RemoveSort()
        {
#if(TRACE)
            Console.WriteLine("void IBindingList.RemoveSort()");
#endif
            IBindingList i = entities as IBindingList;
            i.RemoveSort();
        }

        ListSortDirection IBindingList.SortDirection
        {
            get
            {
                return sortDirection;
            }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get
            {
                return sortProperty;
            }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get
            {
                IBindingList i = entities as IBindingList;
                return i.SupportsChangeNotification;
            }
        }

        bool IBindingList.SupportsSearching
        {
            get
            {
                IBindingList i = entities as IBindingList;
                return i.SupportsSearching;
            }
        }

        bool IBindingList.SupportsSorting
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
#if(TRACE)
            Console.WriteLine("int IList.Add(object value)");
#endif
            T entity = (T)value;

            entity.Collection = this;

            if (entity.RowState == esDataRowState.Deleted)
            {
                if (deletedEntities == null)
                {
                    deletedEntities = new BindingList<T>();
                }
                this.deletedEntities.Add(entity);
                return -1;
            }
            else
            {

                int index = ((IList)entities).Add(value);

                if (updateViewNotification != null)
                {
                    OnUpdateViewNotification(this, ListChangedType.ItemAdded, (esEntity)value);
                }

                return index;
            }
        }

        void IList.Clear()
        {
#if(TRACE)
            Console.WriteLine("void IList.Clear()");
#endif
            ((IList)entities).Clear();
        }

        bool IList.Contains(object value)
        {
#if(TRACE)
            Console.WriteLine("bool IList.Contains(object value)");
#endif
            return ((IList)entities).Contains(value);
        }

        int IList.IndexOf(object value)
        {
#if(TRACE)
            Console.WriteLine("int IList.IndexOf(object value)");
#endif
            return ((IList)entities).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
#if(TRACE)
            Console.WriteLine("void IList.Insert(int index, object value)");
#endif
            ((IList)entities).Insert(index, value);
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)entities).IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList)entities).IsReadOnly; }
        }

        void IList.Remove(object value)
        {
#if(TRACE)
            Console.WriteLine("void IList.Remove(object value)");
#endif
            esEntity entity = value as esEntity;

            entities.Remove((T)entity);

            if (entity.RowState != esDataRowState.Deleted && entity.RowState != esDataRowState.Added)
            {
                entity.MarkAsDeleted();
            }

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
            }

            if (updateViewNotification != null)
            {
                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, entity);
            }
        }

        void IList.RemoveAt(int index)
        {
#if(TRACE)
            Console.WriteLine("void IList.RemoveAt(int index)");
#endif
            esEntity entity = ((IList)entities)[index] as esEntity;

            entities.Remove((T)entity);

            entity.MarkAsDeleted();

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
            }

            if (updateViewNotification != null)
            {
                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, entity);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return ((IList)entities)[index];
            }

            set
            {
                ((IList)entities)[index] = value;

                if (updateViewNotification != null)
                {
                    OnUpdateViewNotification(this, ListChangedType.ItemChanged, (esEntity)value);
                }
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)entities).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return ((ICollection)entities).Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)entities).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((ICollection)entities).SyncRoot; }
        }

        #endregion

        #region ICancelAddNew Members

        void ICancelAddNew.CancelNew(int itemIndex)
        {
#if (TRACE)
            Console.WriteLine("void ICancelAddNew.CancelNew(int itemIndex)");
#endif
            if (itemIndex == lastNewIndex)
            {
                T obj = entities[itemIndex];

                ICancelAddNew i = entities as ICancelAddNew;
                i.CancelNew(itemIndex);

                if (!obj.es.IsAdded) return;

                if (entitiesFilterBackup != null)
                {
                    IList list = entitiesFilterBackup as IList;
                    int index = list.IndexOf(obj);
                    list.Remove(obj);
                }

                // Clicking around bug fix
                entities.Remove(obj);

                lastNewIndex = -1;

                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, obj);
            }
        }

        internal bool CollectionViewCancelNew(T obj)
        {
            IList iList = this as IList;
            int itemIndex = iList.IndexOf(obj);

            if (itemIndex == lastNewIndex)
            {
                ICancelAddNew i = entities as ICancelAddNew;
                i.CancelNew(itemIndex);

                if (!obj.es.IsAdded) return false;

                if (entitiesFilterBackup != null)
                {
                    IList list = entitiesFilterBackup as IList;
                    int index = list.IndexOf(obj);
                    list.Remove(obj);
                }

                // Clicking around bug fix
                entities.Remove(obj);

                lastNewIndex = -1;

                OnUpdateViewNotification(this, ListChangedType.ItemDeleted, obj);

                return true;
            }

            return false;
        }

        void ICancelAddNew.EndNew(int itemIndex)
        {
            ICancelAddNew i = entities as ICancelAddNew;
            i.EndNew(itemIndex);

            if (entitiesFilterBackup != null)
            {
                i = entitiesFilterBackup as ICancelAddNew;
                i.EndNew(itemIndex);
            }
        }

        #endregion

        #region IRaiseItemChangedEvents Members

        bool IRaiseItemChangedEvents.RaisesItemChangedEvents
        {
            get
            {
                IRaiseItemChangedEvents i = entities as IRaiseItemChangedEvents;
                return i.RaisesItemChangedEvents;
            }
        }

        #endregion

        private int lastNewIndex = -1;

        [NonSerialized]
        private bool? allowNew;
        [NonSerialized]
        private bool? allowEdit;
        [NonSerialized]
        private bool? allowDelete;
    }
}
