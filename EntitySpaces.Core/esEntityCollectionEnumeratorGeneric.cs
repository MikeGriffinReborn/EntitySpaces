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

using EntitySpaces.Interfaces;

namespace EntitySpaces.Core
{
    /// <summary>
    /// Added to the Generated Collections to support <B>LINQ</B> Queries into the collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class esEntityCollectionEnumeratorGeneric<T> : IEnumerator<T> where T:esEntity
    {
        /// <summary>
        /// Added to the Generated Collections to support <B>LINQ</B> Queries into the collections.
        /// </summary>
        public esEntityCollectionEnumeratorGeneric(List<T> coll)
        {
            this.entities = coll;
            enumerator = coll.GetEnumerator();
        }

        #region IEnumerator<T> Members

        T IEnumerator<T>.Current
        {
            get
            {
                return (T)enumerator.Current;
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return enumerator.Current;
            }
        }

        bool IEnumerator.MoveNext()
        {
            bool moved = false;

            while (true)
            {
                if (enumerator.MoveNext())
                {
                    IEnumerator e = this as IEnumerator;
                    esEntity obj = e.Current as esEntity;

                    if (obj.rowState != esDataRowState.Deleted)
                    {
                        moved = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return moved;
        }

        void IEnumerator.Reset()
        {
            enumerator.Reset();
        }

        #endregion

        internal IEnumerator enumerator;
        internal List<T> entities;
    }
}
