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

using Npgsql;
using NpgsqlTypes;

namespace EntitySpaces.Npgsql2Provider
{
    class Cache
    {
        static public Dictionary<string, NpgsqlParameter> GetParameters(esDataRequest request)
        {
            return GetParameters(request.DataID, request.ProviderMetadata, request.Columns);
        }

        static public Dictionary<string, NpgsqlParameter> GetParameters(Guid dataID,
            esProviderSpecificMetadata providerMetadata, esColumnMetadataCollection columns)
        {
            lock (parameterCache)
            {
                if (!parameterCache.ContainsKey(dataID))
                {
                    // The Parameters for this Table haven't been cached yet, this is a one time operation
                    Dictionary<string, NpgsqlParameter> types = new Dictionary<string, NpgsqlParameter>();

                    NpgsqlParameter param1;
                    foreach (esColumnMetadata col in columns)
                    {
                        esTypeMap typeMap = providerMetadata.GetTypeMap(col.PropertyName);
                        if (typeMap != null)
                        {
                            string nativeType = typeMap.NativeType;
                            NpgsqlDbType dbType = Cache.NativeTypeToDbType(nativeType);

                            param1 = new NpgsqlParameter(Delimiters.Param + col.PropertyName, dbType, 0, col.Name);
                            param1.SourceColumn = col.Name;

                            switch (dbType)
                            {
                                case NpgsqlDbType.Numeric:

                                    if (col.NumericPrecision > 0)
                                    {
                                        param1.Precision = (byte)col.NumericPrecision;
                                        param1.Scale = (byte)col.NumericScale;
                                    }

                                    break;

                                case NpgsqlDbType.Char:

                                    if (col.CharacterMaxLength > 0)
                                    {
                                        param1.Size = (int)col.CharacterMaxLength;
                                    }

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

        static private NpgsqlDbType NativeTypeToDbType(string nativeType)
        {
            switch (nativeType.ToLower())
            {
                case "int8": return NpgsqlDbType.Bigint;
                case "bool": return NpgsqlDbType.Boolean;
                case "box": return NpgsqlDbType.Box;
                case "circle": return NpgsqlDbType.Circle;
                case "line": return NpgsqlDbType.Line;
                case "lseg": return NpgsqlDbType.LSeg;
                case "path": return NpgsqlDbType.Path;
                case "point": return NpgsqlDbType.Point;
                case "polygon": return NpgsqlDbType.Polygon;
                case "bytea": return NpgsqlDbType.Bytea;
                case "date": return NpgsqlDbType.Date;
                case "float8": return NpgsqlDbType.Double;
                case "int4": return NpgsqlDbType.Integer;
                case "money": return NpgsqlDbType.Money;
                case "float4": return NpgsqlDbType.Real;
                case "int2": return NpgsqlDbType.Smallint;
                case "text": return NpgsqlDbType.Text;
                case "time": return NpgsqlDbType.Time;
                case "timetz": return NpgsqlDbType.Time;
                case "timestamp": return NpgsqlDbType.Timestamp;
                case "timestamptz": return NpgsqlDbType.TimestampTZ;
                case "varchar": return NpgsqlDbType.Varchar;
                case "inet": return NpgsqlDbType.Inet;
                case "bit": return NpgsqlDbType.Bit;
                case "numeric": return NpgsqlDbType.Numeric;
                case "bpchar": return NpgsqlDbType.Char;
                case "uuid": return NpgsqlDbType.Uuid;

                default:
                    return NpgsqlDbType.Integer;
            }
        }

        static public NpgsqlParameter CloneParameter(NpgsqlParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as NpgsqlParameter;
        }

        static private Dictionary<Guid, Dictionary<string, NpgsqlParameter>> parameterCache
            = new Dictionary<Guid, Dictionary<string, NpgsqlParameter>>();
    }
}
