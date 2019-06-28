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

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// Used Internally by EntitySpaces and its data providers to get at the internal data of 
    /// the generated Metdata classes.
    /// </summary>
    public interface IMetadata
    {
        /// <summary>
        /// The DataID - Currently this is not really used.
        /// </summary>
        Guid DataID { get; }
        /// <summary>
        /// True if this class is meant to operator with multiple EntitySpaces data providers in a 
        /// database independent fashion.
        /// </summary>
        bool MultiProviderMode { get; }
        /// <summary>
        /// The columns collection and their data mappings for the type of database currently
        /// being accessed.
        /// </summary>
        esColumnMetadataCollection Columns { get; }
        /// <summary>
        /// Database specific meta data, such as <see cref="esTypeMap"/> and other Metadata class properties.
        /// </summary>
        /// <param name="key">This is driven by the 'providerMetadataKey' in the app.config or web.config file on the EntitySpaces connection section.</param>
        /// <returns>The provider specific metadata</returns>
        esProviderSpecificMetadata GetProviderMetadata(string key);
    }
}
