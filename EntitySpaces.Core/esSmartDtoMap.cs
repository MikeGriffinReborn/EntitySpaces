using System;
using System.Collections.Generic;

namespace EntitySpaces.Core
{
    public class esSmartDtoMap
    {
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
        internal Dictionary<string, string> getters = new Dictionary<string, string>();

        public esSmartDtoMap() 
        {
       
        }

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