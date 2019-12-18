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

using System.Data.SQLite;

namespace EntitySpaces.SQLiteProvider
{
    class Cache
    {
        static public Dictionary<string, SQLiteParameter> GetParameters(esDataRequest request)
        {
            return GetParameters(request.DataID, request.ProviderMetadata, request.Columns);
        }

        static public Dictionary<string, SQLiteParameter> GetParameters(Guid dataID,
            esProviderSpecificMetadata providerMetadata, esColumnMetadataCollection columns)
        {
            lock (parameterCache)
            {
                if (!parameterCache.ContainsKey(dataID))
                {
                    // The Parameters for this Table haven't been cached yet, this is a one time operation
                    Dictionary<string, SQLiteParameter> types = new Dictionary<string, SQLiteParameter>();

                    SQLiteParameter param1;
                    foreach (esColumnMetadata col in columns)
                    {
                        esTypeMap typeMap = providerMetadata.GetTypeMap(col.PropertyName);
                        if (typeMap != null)
                        {
                            string nativeType = typeMap.NativeType;
                            System.Data.DbType dbType = Cache.NativeTypeToDbType(nativeType);

                            param1 = new SQLiteParameter(Delimiters.Param + col.PropertyName, dbType, 0, col.Name);
                            param1.SourceColumn = col.Name;

                            switch (dbType)
                            {
                                case System.Data.DbType.Int64:
                                //case VistaDBType.Int:
                                //case VistaDBType.SmallInt:
                                //case VistaDBType.Decimal:
                                //case VistaDBType.Float:
                                //case VistaDBType.Money:
                                //case VistaDBType.SmallMoney:

                                    param1.Size = (int)col.CharacterMaxLength;
                                    break;

                            }
                            types[col.Name] = param1;
                        }
                    }

                    parameterCache[dataID] = types;
                }
            }

            return parameterCache[dataID];
        }

        static private System.Data.DbType NativeTypeToDbType(string nativeType)
        {
            switch(nativeType)
            {
                case "blob": return System.Data.DbType.Binary;
                case "boolean": return System.Data.DbType.Boolean;
                case "bit": return System.Data.DbType.Boolean;
                case "datetime": return System.Data.DbType.DateTime;
                case "numeric": return System.Data.DbType.Decimal;
                case "float": return System.Data.DbType.Double;
                case "real": return System.Data.DbType.Double;
                case "integer": return System.Data.DbType.Int32;
                case "time": return System.Data.DbType.Time;
                case "date": return System.Data.DbType.DateTime;
                case "timestamp": return System.Data.DbType.Binary;
                case "varchar": return System.Data.DbType.String;

                default: 
                    return System.Data.DbType.String;
            }
        }

        static public SQLiteParameter CloneParameter(SQLiteParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as SQLiteParameter;
        }

        static private Dictionary<Guid, Dictionary<string, SQLiteParameter>> parameterCache
            = new Dictionary<Guid, Dictionary<string, SQLiteParameter>>();
    }
}
