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
    public interface IDynamicQuerySerializableInternal
    {
        /// <summary>
        /// Guid DataID.
        /// </summary>
        /// <returns>Guid DataID.</returns>
        Guid DataID { get; set; }

        /// <summary>
        /// Taken from the Connection just before the request is made
        /// </summary>
        string Catalog { get; set; }

        /// <summary>
        /// Taken from the Connection just before the request is made
        /// </summary>
        string Schema { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool IsInSubQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool HasSetOperation { get; }

        /// <summary>
        /// 
        /// </summary>
        string SubQueryAlias { get; }

        /// <summary>
        /// 
        /// </summary>
        string JoinAlias { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string LastQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string QuerySource { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool SelectAll { get; }

        /// <summary>
        /// 
        /// </summary>
        List<esQueryItem> SelectAllExcept { get; }

        /// <summary>
        /// 
        /// </summary>
        esSubquerySearchCondition SubquerySearchCondition { get; }

        /// <summary>
        /// List&lt;esJoinItem&gt; InternalSelectColumns.
        /// </summary>
        /// <returns>List&lt;esExpression&gt; InternalSelectColumns.</returns>
        List<esExpression> InternalSelectColumns { get; set; }

        /// <summary>
        /// The From SubQuery
        /// </summary>
        esDynamicQuerySerializable InternalFromQuery { get; }

        /// <summary>
        /// List&lt;esJoinItem&gt; InternalJoinItems.
        /// </summary>
        /// <returns>List&lt;esJoinItem&gt; InternalJoinItems.</returns>
        List<esJoinItem> InternalJoinItems { get; }

        /// <summary>
        /// List&lt;esComparison&gt; InternalWhereItems.
        /// </summary>
        /// <returns>List&lt;esComparison&gt; InternalWhereItems.</returns>
        List<esComparison> InternalWhereItems { get; }

        /// <summary>
        /// List&lt;esJoinItem&gt; InternalOrderByItems.
        /// </summary>
        /// <returns>List&lt;esOrderByItem&gt; InternalOrderByItems.</returns>
        List<esOrderByItem> InternalOrderByItems { get; set; }

        /// <summary>
        /// List&lt;esComparison&gt; InternalHavingItems.
        /// </summary>
        /// <returns>List&lt;esComparison&gt; InternalHavingItems.</returns>
        List<esComparison> InternalHavingItems { get; }

        /// <summary>
        /// List&lt;esComparison&gt; InternalGroupByItems.
        /// </summary>
        /// <returns>List&lt;esGroupByItem&gt; InternalGroupByItems.</returns>
        List<esGroupByItem> InternalGroupByItems { get; set; }

        /// <summary>
        /// The Set Operation, if any, Union, UnionAll, Intersect, Except
        /// </summary>
        /// <returns>List&lt;esGroupByItem&gt; InternalGroupByItems.</returns>
        List<esSetOperation> InternalSetOperations { get; set; }

        /// <summary>
        /// This is of type object because this library cannot link with other 
        /// assemblies and this is in the EntitySpaces.Interfaces assembly. This
        /// is filled in just before being sent to the EntitySpaces DataProvider
        /// and is used only when building the SQL i the provider itself
        /// </summary>
        object ProviderMetadata { get; set; }

        /// <summary>
        /// This is of type object because this library cannot link with other 
        /// assemblies and this is in the EntitySpaces.Interfaces assembly. This
        /// is filled in just before being sent to the EntitySpaces DataProvider
        /// and is used only when building the SQL i the provider itself
        /// </summary>
        object Columns { get; set; }

        /// <summary>
        /// This is used because esDynamicQuerySerializable.AssignProviderMetadata is
        /// not public and esJoinItem needs to be able to execute it
        /// </summary>
        void HookupProviderMetadata(esDynamicQuerySerializable query);

        /// <summary>
        /// This is used internally
        /// </summary>
        Dictionary<string, esDynamicQuerySerializable> queries { get; }

        /// <summary>
        /// The number of rows to skip in the result set (starting from the beginning)
        /// </summary>
        int? Skip { get; }

        /// <summary>
        /// The number of rows to take from the result set (starting from the Skip)
        /// </summary>
        int? Take { get; }
    }
}
