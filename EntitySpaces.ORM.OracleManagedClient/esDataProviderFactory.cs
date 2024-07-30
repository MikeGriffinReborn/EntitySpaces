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
using System.Reflection;

using EntitySpaces.Interfaces;

namespace EntitySpaces.Loader
{
    /// <summary>
    /// Used to load the EntitySpaces data providers in a loosely coupled fashion.
    /// </summary>
    public class esDataProviderFactory : IDataProviderFactory
    {
        static private IDataProvider _provider = new EntitySpaces.OracleManagedClientProvider.DataProvider();
        /// <summary>
        /// Loads and Caches the EntitySpaces DataProvider.
        /// </summary>
        /// <remarks>
        /// The providerClass parameter determines whether or not distributed transactions are used or
        /// if ADO.NET connection based transactions are used. When "DataProvider" is used the <see cref="esTransactionScope"/>
        /// class is used to enforce transactions. When "DataProviderEnterprise" is used then <see cref="TransactionScope"/>
        /// is used
        /// </remarks>
        /// <param name="providerName">The name of the EntitySpaces DataProvider, for example, "EntitySpaces.SqlClientProvider"</param>
        /// <param name="providerClass">The class to use, either "DataProvider" or "DataProviderEnterprise"</param>
        /// <returns>The approprate data provider such as "EntitySpaces.SqlClientProvider"</returns>
        public IDataProvider GetDataProvider(string providerName, string providerClass)
        {
            return _provider;
        }
    }
}
