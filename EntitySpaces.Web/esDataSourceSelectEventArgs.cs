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
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

using EntitySpaces.Core;
using EntitySpaces.Interfaces;

namespace EntitySpaces.Web
{
    /// <summary>
    /// Passed to the esPreSelect, esSelect, and esPostSelect events.
    /// </summary>
    /// <seealso cref="esDataSource"/>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class esDataSourceSelectEventArgs : EventArgs
    {
        /// <summary>
        /// This property is used in the pre events to apply business logic criteria and cancel the actual event if those criteria are met.
        /// </summary>
        public bool Cancel;

        /// <summary>
        /// The collection to be used in binding. This is assigned during the esSelect Event.
        /// </summary>
        public esEntityCollectionBase Collection;

        /// <summary>
        /// The query to be used to load the collection with. This is assigned during the esSelect Event.
        /// </summary>
        public esDynamicQuery Query;

        /// <summary>
        /// This is the low level .NET class passed on for completeness
        /// </summary>
        public DataSourceSelectArguments Arguments;

        /// <summary>
        /// The number of rows to display. This is filled in for you and taken from
        /// DataSourceSelectArguments.MaximumRows.
        /// </summary>
        /// <seealso cref="DataSourceSelectArguments"/>
        public int PageSize;

        /// <summary>
        /// The 1 based page number to display. This is filled in by the esDataSource. 
        /// </summary>
        /// <remarks>
        /// This page number is determined using this algorithm.
        /// <example>
        /// <code>
        /// private void CalculatePageSizeAndNumber(esDataSourceSelectEventArgs e)
        /// {
        ///     // Calc PageSize/PageNumber
        ///     if (e.Arguments.MaximumRows > 0)
        ///     {
        ///         e.PageSize = e.Arguments.MaximumRows;
        ///         e.PageNumber = (int)((e.Arguments.StartRowIndex / e.Arguments.MaximumRows) + 1);
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        /// <seealso cref="DataSourceSelectArguments"/>
        public int PageNumber;

        /// <summary>
        /// This contains the DataSourceSelectArguments.SortExpression broken down into a list of 
        /// <see cref="esDataSourceSortItem"/> so that they can be passed to the <see cref="esDynamicQuery"/> 
        /// OrderBy method.
        /// </summary>
        public List<esDataSourceSortItem> SortItems; 
    }
}
