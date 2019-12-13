using EntitySpaces.Interfaces;
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
        public esPartionOrderBy(esDynamicQuery query)
        {
            this.query = query;
        }

        public esPartionDistinctBy OrderBy(params esOrderByItem[] partitionByorderByItems)
        {
            query.es.PartitionByOrderBy(partitionByorderByItems);
            return new esPartionDistinctBy(query);
        }

        private esDynamicQuery query;
    }

    public class esPartionDistinctBy
    {
        public esPartionDistinctBy(esDynamicQuery query)
        {
            this.query = query;
        }

        public esPartionTop DistinctBy(params esQueryItem[] partitionDistinctColumns)
        {
            query.es.PartitionDistinctColumns(partitionDistinctColumns);
            return new esPartionTop(query);
        }

        private esDynamicQuery query;
    }

    public class esPartionTop
    {
        public esPartionTop(esDynamicQuery query)
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

        private esDynamicQuery query;
    }
}