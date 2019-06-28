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

using EntitySpaces.Interfaces;

namespace EntitySpaces.Core
{
    /// <summary>
    /// Serves as a base class to the generated meta data classes.
    /// </summary>
    public class esMetadata
    {
        protected esColumnMetadataCollection m_columns = null;
        protected Guid m_dataID = Guid.NewGuid();

        protected Dictionary<string, esProviderSpecificMetadata> m_providerMetadataMaps =
            new Dictionary<string, esProviderSpecificMetadata>();

        /// <summary>
        /// Used to eliminate the need for Reflection so EntitySpaces can run under
        /// medium trust
        /// </summary>
        /// <param name="mapName">The name of the 'providerMetadataKey' from the config file</param>
        /// <returns>A delegate that can be called to get the esProviderSpecificMetadata matching the 'providerMetadataKey'</returns>
        protected delegate esProviderSpecificMetadata MapToMeta(string mapName);
    }
}
