using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EntitySpaces.DynamicQuery
{
    /*
    oQuery.es.PartitionBy(oQuery.EmployeeID, oQuery.FirstName)
        .OrderBy(odq.Quantity)
        .DistinctOn(oq.OrderID)
        .Count(4);
        */

    public class esPartionOrderBy
    {
        public esPartionOrderBy(esDynamicQuerySerializable query)
        {
            this.query = query;
        }

        public esPartionDistinctBy OrderBy(params esOrderByItem[] partitionByorderByItems)
        {
            query.es.PartitionByOrderBy(partitionByorderByItems);
            return new esPartionDistinctBy(query);
        }

        private esDynamicQuerySerializable query;
    }

    public class esPartionDistinctBy
    {
        public esPartionDistinctBy(esDynamicQuerySerializable query)
        {
            this.query = query;
        }

        public esPartionTop DistinctBy(params esQueryItem[] partitionDistinctColumns)
        {
            query.es.PartitionDistinctColumns(partitionDistinctColumns);
            return new esPartionTop(query);
        }

        private esDynamicQuerySerializable query;
    }

    public class esPartionTop
    {
        public esPartionTop(esDynamicQuerySerializable query)
        {
            this.query = query;
        }

        public int Top
        {
            set
            {
                this.query.es.PartitionByTop = value;
            }
        }

        private esDynamicQuerySerializable query;
    }
}