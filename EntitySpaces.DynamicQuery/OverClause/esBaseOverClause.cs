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
using System.Collections.Generic;

namespace EntitySpaces.DynamicQuery
{
    public interface IOverClause
    {
        esQueryItem ColumnExpression { get; }
        esQueryItem[] PartionByColumns { get; }
        List<esOrderByItem> OrderByColumns { get; }
        string Alias { get; }

        string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias);
    }

    public abstract class esBaseOverClause : IOverClause
    {
        protected internal esQueryItem _columnExpression;
        protected internal esQueryItem[] _partitionByColumns;
        protected internal List<esOrderByItem> _orderByItems;
        protected internal string _alias;

        #region Fluent Variables
        internal esBaseOverClausePartitionBy partitionBy;
        internal esBaseOverClauseOrderBy orderBy;
        #endregion

        public virtual esBaseOverClausePartitionBy PartitionBy(params esQueryItem[] partitionByColumns)
        {
            _partitionByColumns = partitionByColumns;
            return partitionBy = new esBaseOverClausePartitionBy(this);
        }
        public virtual esBaseOverClauseOrderBy OrderBy(params esOrderByItem[] orderByItems)
        {
            SetOrderByItems(orderByItems);
            return orderBy = new esBaseOverClauseOrderBy(this);
        }

        protected internal void SetOrderByItems(params esOrderByItem[] orderByItems)
        {
            _orderByItems = new List<esOrderByItem>();

            foreach (esOrderByItem orderByItem in orderByItems)
            {
                _orderByItems.Add(orderByItem);
            }
        }

        protected abstract string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias);

        #region IOverClause

        esQueryItem IOverClause.ColumnExpression => _columnExpression;

        esQueryItem[] IOverClause.PartionByColumns => _partitionByColumns;

        List<esOrderByItem> IOverClause.OrderByColumns => _orderByItems;

        string IOverClause.Alias => _alias;

        string IOverClause.CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            return CreateOverStatement(columnExpression, partionby, orderBy, alias);
        }

        #endregion
    }

    public class esBaseOverClausePartitionBy
    {
        private esBaseOverClause _parent;

        public esBaseOverClausePartitionBy(esBaseOverClause parent) => _parent = parent;

        public esBaseOverClauseOrderBy OrderBy(params esOrderByItem[] orderByItems)
        {
            _parent.SetOrderByItems(orderByItems);
            return _parent.orderBy = new esBaseOverClauseOrderBy(_parent);
        }
    }

    public class esBaseOverClauseOrderBy
    {
        private esBaseOverClause _parent;

        public esBaseOverClauseOrderBy(esBaseOverClause parent) => _parent = parent;

        public esBaseOverClause As(string alias)
        {
            _parent._alias = alias;
            return _parent;
        }
    }
}