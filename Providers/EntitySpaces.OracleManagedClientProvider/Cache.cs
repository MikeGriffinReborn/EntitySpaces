using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;


using EntitySpaces.Interfaces;

namespace EntitySpaces.OracleManagedClientProvider
{
    class Cache
    {
        static public Dictionary<string, OracleParameter> GetParameters(esDataRequest request)
        {
            return GetParameters(request.DataID, request.ProviderMetadata, request.Columns);
        }

        static public Dictionary<string, OracleParameter> GetParameters(Guid dataID,
            esProviderSpecificMetadata providerMetadata, esColumnMetadataCollection columns)
        {
            lock (parameterCache)
            {
                if (!parameterCache.ContainsKey(dataID))
                {
                    // The Parameters for this Table haven't been cached yet, this is a one time operation
                    Dictionary<string, OracleParameter> types = new Dictionary<string, OracleParameter>();

                    OracleParameter param1;
                    foreach (esColumnMetadata col in columns)
                    {
                        esTypeMap typeMap = providerMetadata.GetTypeMap(col.PropertyName);
                        if (typeMap != null)
                        {
                            string nativeType = typeMap.NativeType;
                            OracleDbType dbType = Cache.NativeTypeToDbType(nativeType);


                            param1 = new OracleParameter(Delimiters.Param + col.PropertyName, dbType, 0, col.Name);
                            param1.SourceColumn = col.Name;

                            switch (dbType)
                            {
                                case OracleDbType.Decimal:

                                    //param1.Size = (int)col.CharacterMaxLength;
                                    //param1.Precision = (byte)col.NumericPrecision;
                                    //param1.Scale = (byte)col.NumericScale;
                                    break;

                                case OracleDbType.NClob:
                                case OracleDbType.Char:
                                case OracleDbType.NChar:
                                case OracleDbType.Varchar2:
                                case OracleDbType.NVarchar2:

                                    param1.Size = (int)col.CharacterMaxLength;
                                    break;

                            }

                            //    case SqlDbType.DateTime:

                            //        param1.Precision = 23;
                            //        param1.Scale = 3;
                            //        break;

                            //    case SqlDbType.SmallDateTime:

                            //        param1.Precision = 16;
                            //        break;

                            //}
                            types[col.Name] = param1;
                        }
                    }

                    parameterCache[dataID] = types;
                }
            }

            return parameterCache[dataID];
        }

        static private OracleDbType NativeTypeToDbType(string nativeType)
        {
            switch (nativeType)
            {
                case "BFILE": return OracleDbType.BFile;
                case "BINARY FLOAT": return OracleDbType.BinaryFloat;
                case "BINARY DOUBLE": return OracleDbType.BinaryDouble;
                case "BLOB": return OracleDbType.Blob;
                case "CHAR": return OracleDbType.Char;
                case "CLOB": return OracleDbType.Clob;
                case "CURSOR": return OracleDbType.RefCursor;
                case "DATE": return OracleDbType.Date;
                case "FLOAT": return OracleDbType.Decimal;
                case "INTERVAL DAY TO SECOND": return OracleDbType.IntervalDS;
                case "INTERVAL YEAR TO MONTH": return OracleDbType.IntervalYM;
                case "LONGRAW": return OracleDbType.LongRaw;
                case "LONG": return OracleDbType.Long;
                case "NCHAR": return OracleDbType.NChar;
                case "NCLOB": return OracleDbType.NClob;
                case "NUMBER": return OracleDbType.Decimal;
                case "NVARCHAR2": return OracleDbType.NVarchar2;
                case "RAW": return OracleDbType.Raw;
                //case "ROWID": return OracleDbType.Ref;
                case "TIMESTAMP": return OracleDbType.TimeStamp;
                case "TIMESTAMP WITH TIME ZONE": return OracleDbType.TimeStampTZ;
                case "TIMESTAMP WITH LOCAL TIME ZONE": return OracleDbType.TimeStampLTZ;
                case "VARCHAR2": return OracleDbType.Varchar2;

                default: return OracleDbType.Varchar2;
            }
        }

        static public OracleParameter CloneParameter(OracleParameter p)
        {
            ICloneable param = p as ICloneable;
            return param.Clone() as OracleParameter;
        }

        static private Dictionary<Guid, Dictionary<string, OracleParameter>> parameterCache
            = new Dictionary<Guid, Dictionary<string, OracleParameter>>();
    }
}
