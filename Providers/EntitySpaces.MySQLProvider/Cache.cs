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

using MySql.Data.MySqlClient;

namespace EntitySpaces.MySQLProvider
{
    class Cache
    {
        static public Dictionary<string, MySqlParameter> GetParameters(esDataRequest request)
        {
            return GetParameters(request.DataID, request.ProviderMetadata, request.Columns);
        }

        static public Dictionary<string, MySqlParameter> GetParameters(Guid dataID,
            esProviderSpecificMetadata providerMetadata, esColumnMetadataCollection columns)
        {
            lock (parameterCache)
            {
                if (!parameterCache.ContainsKey(dataID))
                {
                    // The Parameters for this Table haven't been cached yet, this is a one time operation
                    Dictionary<string, MySqlParameter> types = new Dictionary<string, MySqlParameter>();

                    MySqlParameter param1;
                    foreach (esColumnMetadata col in columns)
                    {
                        esTypeMap typeMap = providerMetadata.GetTypeMap(col.PropertyName);
                        if (typeMap != null)
                        {
                            string nativeType = typeMap.NativeType;
                            MySqlDbType dbType = Cache.NativeTypeToDbType(nativeType);

                            param1 = new MySqlParameter(Delimiters.Param + col.PropertyName, dbType, 0, col.Name);
                            param1.SourceColumn = col.Name;

                            switch (dbType)
                            {
                                case MySqlDbType.Decimal:
                                case MySqlDbType.NewDecimal:
                                case MySqlDbType.Double:
                                case MySqlDbType.Float:
                                case MySqlDbType.Int16:
                                case MySqlDbType.Int24:
                                case MySqlDbType.Int32:
                                case MySqlDbType.Int64:
                                case MySqlDbType.UInt16:
                                case MySqlDbType.UInt24:
                                case MySqlDbType.UInt32:
                                case MySqlDbType.UInt64:

                                    param1.Size = (int)col.CharacterMaxLength;
                                    param1.Precision = (byte)col.NumericPrecision;
                                    param1.Scale = (byte)col.NumericScale;
                                    break;

                                case MySqlDbType.String:
                                case MySqlDbType.VarString:
                                case MySqlDbType.VarChar:
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

        static private MySqlDbType NativeTypeToDbType(string nativeType)
        {
            switch(nativeType)
            {
                case "BIT": return MySqlDbType.Bit;
                case "BIGINT": return MySqlDbType.Int64;
                case "INT": return MySqlDbType.Int32;
                case "MEDIUMINT": return MySqlDbType.Int24;
                case "SMALLINT": return MySqlDbType.Int16;
                case "TINYINT": return MySqlDbType.Byte;
                case "BIGINT UNSIGNED": return MySqlDbType.UInt64;
                case "INT UNSIGNED": return MySqlDbType.UInt32;
                case "MEDIUMINT UNSIGNED": return MySqlDbType.UInt24;
                case "SMALLINT UNSIGNED": return MySqlDbType.UInt16;
                case "TINYINT UNSIGNED": return MySqlDbType.UByte;
                case "FLOAT": return MySqlDbType.Float;
                case "FLOAT UNSIGNED": return MySqlDbType.Float;
                case "DECIMAL": return MySqlDbType.Decimal;
                case "DECIMAL UNSIGNED": return MySqlDbType.Decimal;
                case "NEWDECIMAL": return MySqlDbType.NewDecimal;
                case "NUMERIC": return MySqlDbType.Decimal;
                case "NUMERIC UNSIGNED": return MySqlDbType.Decimal;
                case "DOUBLE": return MySqlDbType.Double;
                case "DOUBLE UNSIGNED": return MySqlDbType.Double;
                case "REAL": return MySqlDbType.Double;
                case "REAL UNSIGNED": return MySqlDbType.Double;
                case "TIMESTAMP": return MySqlDbType.Timestamp;
                case "DATETIME": return MySqlDbType.DateTime;
                case "DATE": return MySqlDbType.Date;
                case "TIME": return MySqlDbType.Time;
                case "YEAR": return MySqlDbType.Year;
                case "BINARY": return MySqlDbType.Binary;
                case "VARBINARY": return MySqlDbType.VarBinary;
                case "BLOB": return MySqlDbType.Blob;
                case "LONGBLOB": return MySqlDbType.LongBlob;
                case "MEDIUMBLOB": return MySqlDbType.MediumBlob;
                case "TINYBLOB": return MySqlDbType.TinyBlob;
                case "VARCHAR": return MySqlDbType.VarChar;
                case "CHAR": return MySqlDbType.String;
                case "VARSTRING": return MySqlDbType.VarString;
                case "TEXT": return MySqlDbType.Text;
                case "LONGTEXT": return MySqlDbType.LongText;
                case "MEDIUMTEXT": return MySqlDbType.MediumText;
                case "TINYTEXT": return MySqlDbType.TinyText;
                case "SET": return MySqlDbType.Set;
                case "ENUM": return MySqlDbType.Enum;

                default: 
                    return MySqlDbType.VarChar;
            }
        }

        static public MySqlParameter CloneParameter(MySqlParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as MySqlParameter;
        }
        
        static private Dictionary<Guid, Dictionary<string, MySqlParameter>> parameterCache
            = new Dictionary<Guid, Dictionary<string, MySqlParameter>>();
    }
}
