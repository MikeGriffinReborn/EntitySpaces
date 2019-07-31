using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace EntitySpaces.ProfilerApplication
{
    public class SortableBindingList<T> : BindingList<T>
    {
        List<T> originalList;
        ListSortDirection sortDirection;
        PropertyDescriptor sortProperty = null;
        Action<SortableBindingList<T>, List<T>> populateBaseList = (a, b) => a.ResetItems(b);

        class PropertyCompare : IComparer<T>
        {
            PropertyDescriptor _property;
            ListSortDirection _direction;

            public PropertyCompare(PropertyDescriptor property, ListSortDirection direction)
            {
                _property = property; _direction = direction;
            }

            public int Compare(T comp1, T comp2)
            {
                var value1 = _property.GetValue(comp1) as IComparable;
                var value2 = _property.GetValue(comp2) as IComparable;

                if (value1 == null) value1 = 0;
                if (value2 == null) value2 = 0;

                if (_direction == ListSortDirection.Ascending)
                {
                    return value1.CompareTo(value2);
                }
                else
                {
                    return value2.CompareTo(value1);
                }
            }
        }

        public SortableBindingList()
        {
            originalList = new List<T>();
        }

        public SortableBindingList(IEnumerable<T> enumerable)
        {
            originalList = new List<T>(enumerable);
            populateBaseList(this, originalList);
        }

        public SortableBindingList(List<T> list)
        {
            originalList = list;
            populateBaseList(this, originalList);
        }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            PropertyCompare comp = new PropertyCompare(prop, sortDirection);
            List<T> sortedList = new List<T>(this);
            sortedList.Sort(comp);
            ResetItems(sortedList); ResetBindings();
            sortDirection = sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
        }

        protected override void RemoveSortCore()
        {
            ResetItems(originalList);
        }

        private void ResetItems(List<T> items)
        {
            base.ClearItems();

            for (int i = 0; i < items.Count; i++)
            {
                base.InsertItem(i, items[i]);
            }
        }
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            try
            {
                base.OnListChanged(e);
                originalList = new List<T>(base.Items);
            }
            catch { }
        }
    }
}