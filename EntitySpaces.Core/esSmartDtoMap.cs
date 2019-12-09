using System;
using System.Collections;
using System.Collections.Generic;

namespace EntitySpaces.Core
{
    public class esSmartDtoMap
    {
        public esSmartDtoMap() 
        {
       
        }

        static public void Map(esEntity src, esSmartDto dst)
        {
            dst.HrydateFromEntity(src);
        }

        static public void Map<T>(esEntityCollectionBase src, List<T> dst) 
        {
            if (src != null && src.Count > 0 && dst != null && dst.Count > 0)
            {
                if (src.Count == dst.Count)
                {
                    IEnumerable iEnum = src as IEnumerable;
                    var x = iEnum.GetEnumerator();

                    int i = 0;
                    foreach (esEntity entity in iEnum)
                    {
                        esSmartDto dto = dst[i++] as esSmartDto;
                        dto.HrydateFromEntity(entity);
                    }
                }
            }
        }
        static public void Map(esSmartDto src, esEntity dst) 
        {
            dst.HrydateFromDto(src);
        }

        public string PropertyToSqlColumn(string propertyName)
        {
            string sqlColumn = null;

            if(getters.ContainsKey(propertyName))
            {
                sqlColumn = getters[propertyName];
            }

            return sqlColumn;
        }

        public void Create(Type entityType,
            params (string dtoPropertyName, string dtoColumnName, (Type entWriteType, string entWriteColumn)[] writes)[] entries)
        {
            Dictionary<string, string> defaultMap = entityType == null ? null : GetWriteMap(entityType);

            foreach ((string dtoPropertyName, string dtoColumnName, (Type entWriteType, string entWriteColumn)[] writes) in entries)
            {
                getters[dtoPropertyName] = dtoColumnName;

                if (writes != null)
                {
                    foreach ((Type entWriteType, string entWriteColumn) in writes)
                    {
                        Dictionary<string, string> map = GetWriteMap(entWriteType);
                        map[dtoColumnName] = entWriteColumn;
                    }
                }
                else
                {
                    if (defaultMap != null)
                    {
                        defaultMap[dtoColumnName] = dtoColumnName;
                    }
                }
            }
        }

        //----------------------------------
        // This acts as our write list
        //
        // [
        //   Entity.GetType().FullName
        //   [
        //       dtoColumnName, entityColumnName,
        //       dtoColumnName, entityColumnName
        //   ]
        //   Entity.GetType().FullName
        //   [
        //       dtoColumnName, entityColumnName,
        //       dtoColumnName, entityColumnName
        //   ]
        // ]
        private Dictionary<string, Dictionary<string, string>> writeMap = new Dictionary<string, Dictionary<string, string>>();

        // Used during GetValue()/SetValue() only
        internal Dictionary<string, string> getters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

        internal IReadOnlyDictionary<string, string> GetMap(Type type)
        {
            if (writeMap.ContainsKey(type.FullName))
            {
                return writeMap[type.FullName];
            }
            else
            {
                return null;
            }
        }

        private Dictionary<string, string> GetWriteMap(Type entityType)
        {
            Dictionary<string, string> map = null;

            if (writeMap.ContainsKey(entityType.FullName))
            {
                map = writeMap[entityType.FullName];
            }
            else
            {
                writeMap[entityType.FullName] = new Dictionary<string, string>();
                map = writeMap[entityType.FullName];
            }

            return map;
        }
    }
}