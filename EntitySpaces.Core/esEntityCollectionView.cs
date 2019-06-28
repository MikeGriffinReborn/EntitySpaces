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
using System.Diagnostics;
using System.ComponentModel;
using System.Linq;

namespace EntitySpaces.Core
{
    public class esEntityCollectionView<T> : IList<T>, ITypedList, IBindingList, ICancelAddNew, IRaiseItemChangedEvents
        where T : esEntity, new()
    {
        private esEntityCollectionView() { }

        /// <summary>
        /// The only Constructor for the esEntityCollectionView class
        /// </summary>
        /// <param name="collection">The Collection the View is based upon</param>
        public esEntityCollectionView(esEntityCollection<T> collection)
        {
            this.collection = collection;

            baseEntityList = collection.entities;

            // We want to initially populate from the original list if there is an ongoing filer ..
            BindingList<T> list = (collection.entitiesFilterBackup == null) ? baseEntityList : collection.entitiesFilterBackup;

            foreach (T obj in list)
            {
                entities.Add(obj);
            }

            this.entities.ListChanged += new ListChangedEventHandler(this.OnListChanged);
       //   this.collection.entities.ListChanged += new ListChangedEventHandler(this.OnListChanged);
            this.collection.UpdateViewNotification += new esUpdateViewEventHandler(this.OnUpdateViewEventHandler);
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public T this[int index]
        {
            get
            {
                return entities[index];
            }
            set
            {
                entities[index] = value;
            }
        }

        #region Filter
        public IQueryable<T> Filter
        {
            set
            {
                if (value == null)
                {
                    RemoveFilter();
                }
                else
                {
                    RemoveFilter();
                    ApplyFilter(value);
                }

                OnListChanged(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        private void RemoveFilter()
        {
            if (entitiesFilterBackup == null) return;

            RaiseListChangeEvents_Disable();
            entities.Clear();

            foreach (T obj in entitiesFilterBackup)
            {
                entities.Add(obj);
            }
            entitiesFilterBackup = null;

            RaiseListChangeEvents_Restore();
        }

        private void ApplyFilter(IQueryable<T> filter)
        {
            BindingList<T> temp = new BindingList<T>();

            foreach (T obj in entities)
            {
                temp.Add(obj);
            }

            RaiseListChangeEvents_Disable();

            List<T> list = filter.ToList<T>();
            entities.Clear();
            foreach (T obj in list)
            {
                entities.Add(obj);
            }

            entitiesFilterBackup = temp;

            RaiseListChangeEvents_Restore();
        }
        #endregion

        #region RaiseListChangedEvents Processing

        [NonSerialized]
        private bool raiseListChangedEvents = false;

        public void RaiseListChangeEvents_Disable()
        {
            raiseListChangedEvents = entities.RaiseListChangedEvents;
            entities.RaiseListChangedEvents = false;
        }

        public void RaiseListChangeEvents_Restore()
        {
            entities.RaiseListChangedEvents = raiseListChangedEvents;
        }

        #endregion

        #region ListChanged Event Logic
        /// <summary>
        /// Called whenever the esEntityCollection data is changed.
        /// </summary>
        public virtual event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChangedEvent += value;
            }
            remove
            {
                onListChangedEvent -= value;
            }
        }

        private void OnListChanged(object sender, ListChangedEventArgs e)
        {
            ListChangedEventHandler handler = onListChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region OnUpdateViewEventHandler

        public void OnUpdateViewEventHandler(object sender, ListChangedType changeType, esEntity entity)
        {
            bool orginal = this.entities.RaiseListChangedEvents;
            this.entities.RaiseListChangedEvents = collection.RaiseListChangeEventsEnabled;

            switch (changeType)
            {
                case ListChangedType.ItemDeleted:
                    {
                        IList coll = this as IList;
                        coll.Remove(entity);

                        if (entitiesFilterBackup != null)
                        {
                            IBindingList i = entitiesFilterBackup as IBindingList;
                            i.Remove(entity);
                        }
                    }
                    break;

                case ListChangedType.ItemAdded:
                    {
                        IList coll = this as IList;
                        coll.Add(entity);

                        if (entitiesFilterBackup != null)
                        {
                            IBindingList i = entitiesFilterBackup as IBindingList;
                            i.Add(entity);
                        }
                    }
                    break;

                case ListChangedType.Reset:
                    {
                        entities.Clear();

                        // We want to initially populate from the original list if there is an ongoing filer ..
                        BindingList<T> list = (collection.entitiesFilterBackup == null) ? baseEntityList : collection.entitiesFilterBackup;

                        foreach (T obj in list)
                        {
                            entities.Add(obj);
                        }

                        if (entitiesFilterBackup != null)
                        {
                            entitiesFilterBackup = null;
                        }
                    }
                    break;
            }

            this.entities.RaiseListChangedEvents = orginal;
        }

        #endregion

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
            }
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
#if (TRACE)
            Console.WriteLine("void ICollection<T>.Add(T item)");
#endif
            ICollection<T> i = entities as ICollection<T>;
            i.Add(item);
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

            IList list = this as IList;
            bool removed = list.Contains(item);

            entities.Remove((T)entity);

            entity.MarkAsDeleted();

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
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
            ITypedList list = this.collection as ITypedList;
            return list.GetItemProperties(listAccessors);
        }

        string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
        {
            ITypedList list = this.collection as ITypedList;
            return list.GetListName(listAccessors);
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
            Console.WriteLine("object IBindingList.AddNew()");
#endif
            T o = collection.AddNew();

            //IBindingList i = entities as IBindingList;
            //T o = (T)i.AddNew();
            //o.rowState = esDataRowState.Added;
            //o.Collection = collection;

            if (entitiesFilterBackup != null)
            {
                IBindingList i = entities as IBindingList;
                i = entitiesFilterBackup as IBindingList;
                i.Add(o);
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
            IBindingList i = entities as IBindingList;
            return i.Find(property, key);
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
                onListChangedEvent += value;
            }
            remove
            {
#if(TRACE)
                Console.WriteLine("event ListChangedEventHandler IBindingList.ListChanged - REMOVE");
#endif
                onListChangedEvent -= value;
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
            return ((IList)entities).Add(value);
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

            entity.MarkAsDeleted();

            if (entitiesFilterBackup != null)
            {
                IList i = entitiesFilterBackup as IList;
                i.Remove(entity);
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
            T obj = entities[itemIndex];

            bool removed = collection.CollectionViewCancelNew(obj);

            if (removed && entitiesFilterBackup != null)
            {
                IList list = entitiesFilterBackup as IList;
                int index = list.IndexOf(obj);
                list.Remove(obj);
            }
        }

        void ICancelAddNew.EndNew(int itemIndex)
        {
            ICancelAddNew i = collection as ICancelAddNew;
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

        private bool? allowNew;
        private bool? allowEdit;
        private bool? allowDelete;

        internal bool isSorted;
        internal PropertyDescriptor sortProperty;
        internal ListSortDirection sortDirection = ListSortDirection.Ascending;
        private ListChangedEventHandler onListChangedEvent;

        private esEntityCollection<T> collection;
        private BindingList<T> baseEntityList;
        private BindingList<T> entitiesFilterBackup;

        private BindingList<T> entities = new BindingList<T>();
    }
}
