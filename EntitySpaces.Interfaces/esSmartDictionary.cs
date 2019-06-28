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
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace EntitySpaces.Interfaces
{
    [Serializable]
    public class esSmartDictionary
    {
        #region Constructors

        public esSmartDictionary()
        {

        }

        public esSmartDictionary(int capacity)
        {
            ordinals = new Dictionary<string, int>(capacity);
            currentValues = new object[capacity];

            for (int i = 0; i < currentValues.Length; i++)
            {
                currentValues[i] = DBNull.Value;
            }
        }

        public esSmartDictionary(esSmartDictionary smartDictionary)
        {
            if (smartDictionary.ordinals != null)
            {
                ordinals = new Dictionary<string, int>(smartDictionary.ordinals);
            }

            if (smartDictionary.currentValues != null)
            {
                int length = smartDictionary.currentValues.Length;

                currentValues = new object[length];

                for (int i = 0; i < length; i++)
                {
                    currentValues[i] = smartDictionary.currentValues[i];
                }
            }

            onFirstAccess = smartDictionary.onFirstAccess;
        }

        public esSmartDictionary(DataColumnCollection columns, object[] values)
        {
            if (ordinals == null)
            {
                ordinals = new Dictionary<string, int>(columns.Count);
            }

            foreach (DataColumn col in columns)
            {
                ordinals[col.ColumnName] = col.Ordinal;
            }

            currentValues = values;
        }

        public esSmartDictionary(Dictionary<string, int> ordinals, object[] values)
        {
            this.ordinals = ordinals;
            currentValues = values;
        }

        public esSmartDictionary(Dictionary<string, int> ordinals, object[] values, bool cloneValues)
        {
            this.ordinals = ordinals;

            if (!cloneValues)
            {
                currentValues = values;
            }
            else
            {
                currentValues = new object[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    currentValues[i] = values[i];
                }
            }
        }

        #endregion

        #region Capacity Event Handler

        public delegate void esSmartDictionaryFirstAccessEventHandler(esSmartDictionary smartDictionary);

        public virtual event esSmartDictionaryFirstAccessEventHandler FirstAccess
        {
            add
            {
                onFirstAccess += value;
            }
            remove
            {
                onFirstAccess -= value;
            }
        }


        [NonSerialized]
        private esSmartDictionaryFirstAccessEventHandler onFirstAccess;

        #endregion

        public void Allocate(int capacity)
        {
            ordinals = new Dictionary<string, int>(capacity);
            currentValues = new object[capacity];

            for(int i = 0; i < currentValues.Length; i++)
            {
                currentValues[i] = DBNull.Value;
            }
        }

        public bool TryGetValue(string columnName, out object value)
        {
            value = null;

            if (currentValues == null || ordinals == null) return false;

            if (ordinals.ContainsKey(columnName))
            {
                int ordinal = ordinals[columnName];

                if (ordinal < currentValues.Length)
                {
                    value = currentValues[ordinals[columnName]];
                    return true;
                }
            }

            return false;
        }

        public int Count
        {
            get
            {
                return (ordinals != null) ? ordinals.Count : 0;
            }
        }

        public void SetOrdinal(string columnName, int ordinal)
        {
            ordinals[columnName] = ordinal;
        }

        public object this[string columnName]
        {
            get
            {
                object o = null;
                if (TryGetValue(columnName, out o))
                {
                    return o;
                }
                else return null;
            }

            set
            {
                if (currentValues == null && ordinals == null)
                {
                    if (onFirstAccess != null)
                    {
                        onFirstAccess(this);
                    }
                }

                if (ordinals.ContainsKey(columnName))
                {
                    int ordinal = ordinals[columnName];

                    // Maybe we're in a collection and this slot isn't in our
                    // object[] currentValues
                    if (ordinal >= currentValues.Length)
                    {
                        Reallocate();
                    }

                    // Set the value
                    currentValues[ordinals[columnName]] = value;
                }
                else
                {
                    int ordinal = ordinals.Count;

                    // Okay, all we know is that this value hasn't been set yet.
                    ordinals[columnName] = ordinal;

                    // Maybe we're in a collection and this slot isn't in our
                    // object[] currentValues
                    if (ordinal >= currentValues.Length)
                    {
                        Reallocate();
                    }

                    currentValues[ordinals[columnName]] = value;
                }
            }
        }

        public string IndexToColumnName(int index)
        {
            foreach (KeyValuePair<string, int> ordinal in ordinals)
            {
                if (ordinal.Value == index)
                {
                    return ordinal.Key;
                }
            }

            return null;
        }

        public bool ContainsKey(string key)
        {
            return ordinals != null ? ordinals.ContainsKey(key) : false;
        }

        public Dictionary<string, int>.KeyCollection Keys
        {
            get
            {
                return ordinals.Keys;
            }
        }

        internal void Reallocate()
        {
            int count = ordinals.Count;

            object[] newValues = new object[count];

            for (int i = 0; i < newValues.Length; i++)
            {
                newValues[i] = DBNull.Value;
            }

            for (int i = 0; i < currentValues.Length; i++)
            {
                newValues[i] = currentValues[i];
            }

            currentValues = newValues;
        }

        internal object[] currentValues;
        internal Dictionary<string, int> ordinals;
    }
}
