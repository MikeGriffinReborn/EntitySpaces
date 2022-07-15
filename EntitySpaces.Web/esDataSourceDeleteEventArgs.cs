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
using System.Security.Permissions;
using System.Web;

using EntitySpaces.Core;

namespace EntitySpaces.Web
{
    /// <summary>
    /// Passed to the esPreDelete, esDelete, and esPostDelete events.
    /// </summary>
    /// <seealso cref="esDataSource"/>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class esDataSourceDeleteEventArgs : EventArgs
    {
        /// <summary>
        /// This property is used in the pre events to apply business logic criteria and cancel the actual event if those criteria are met.
        /// </summary>
        public bool Cancel;

        /// <summary>
        /// The esEntityCollection if valid
        /// </summary>
        public esEntityCollectionBase Collection;

        /// <summary>
        /// The esEntity if valid
        /// </summary>
        public esEntity Entity;

        /// <summary>
        /// The property names that the control is attempting to set.
        /// </summary>
        public IDictionary Keys;

        /// <summary>
        /// The original property values
        /// </summary>
        public IDictionary OldValues;

        /// <summary>
        /// If not null/Nothing then you need to use these to call LoadByPrimaryKey
        /// </summary>
        public object[] PrimaryKeys;

        /// <summary>
        /// Set this to true to indicate that you have deleted the data. This will prevent
        /// the esDataSource from attempting to delete it.
        /// </summary>
        public bool EventWasHandled;
    }
}
