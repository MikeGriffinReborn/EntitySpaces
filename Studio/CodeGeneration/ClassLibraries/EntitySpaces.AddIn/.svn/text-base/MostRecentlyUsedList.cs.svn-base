using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;

namespace EntitySpaces.AddIn
{
    internal class MostRecentlyUsedList : IEnumerable<string>
    {
        private List<string> list = new List<string>(10);

        public MostRecentlyUsedList()
        {
            for (int i = 0; i < 10; i++)
            {
                list.Add(null);
            }
        }

        public void Push(string keyValue)
        {
            if (keyValue != null)
            {
                // Make sure it doesn't already exist
                for (int i = 0; i < 10; i++)
                {
                    if (list[i] != null)
                    {
                        if (list[i].ToLower() == keyValue.ToLower())
                        {
                            Remove(keyValue);
                            break;
                        }
                    }
                }
            }

            // Shift everybody down one
            for (int i = 9; i > 0; i--)
            {
                list[i] = list[i - 1];
            }

            // Add our new guy
            list[0] = keyValue == string.Empty ? null : keyValue;
        }

        public void Remove(string keyValue)
        {
            int index = -1;

            for (int i = 0; i < 10; i++)
            {
                if (list[i] != null)
                {
                    if (list[i].ToLower() == keyValue.ToLower())
                    {
                        index = i;
                        break;
                    }
                }
            }

            if (index != -1)
            {
                for (int i = index; i < 9; i++)
                {
                    list[i] = list[i + 1];
                }

                list[9] = null;
            }
        }

        public void Load(RegistryKey key, string subKey)
        {
            for (int i = 10; i > 0; i--)
            {
                Push((string)key.GetValue(subKey + i.ToString()));
            }
        }

        public void Save(RegistryKey key, string subKey)
        {
            string temp = string.Empty;

            for (int i = 0; i < 10; i++)
            {
                temp = subKey + (i + 1).ToString();
                key.CreateSubKey(temp, RegistryKeyPermissionCheck.ReadWriteSubTree);
                key.SetValue(temp, list[i] == null ? "" : list[i]);
            }
        }

        #region IEnumerable<string> Members

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion
    }
}
