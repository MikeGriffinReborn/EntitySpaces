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

namespace EntitySpaces.DynamicQuery
{
    public class esCountOver : esBaseOverClause
    {
        private esCountOver() { }

        public esCountOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"COUNT({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"COUNT({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"COUNT({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "COUNT() WAS INVALID";
        }
    }

    public class esCountBigOver : esBaseOverClause
    {
        private esCountBigOver() { }

        public esCountBigOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"COUNT_BIG({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"COUNT_BIG({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"COUNT_BIG({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "COUNT_BIG() WAS INVALID";
        }
    }

    public class esSumOver : esBaseOverClause
    {
        private esSumOver() { }

        public esSumOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"SUM({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"SUM({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"SUM({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "SUM() WAS INVALID";
        }
    }

    public class esAvgOver : esBaseOverClause
    {
        private esAvgOver() { }

        public esAvgOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"AVG({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"AVG({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"AVG({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "AVG() WAS INVALID";
        }
    }

    public class esMinOver : esBaseOverClause
    {
        private esMinOver() { }

        public esMinOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"MIN({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"MIN({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"MIN({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "MIN() WAS INVALID";
        }
    }

    public class esMaxOver : esBaseOverClause
    {
        private esMaxOver() { }

        public esMaxOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"MAX({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"MAX({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"MAX({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "MAX() WAS INVALID";
        }
    }

    public class esStdDevOver : esBaseOverClause
    {
        private esStdDevOver() { }

        public esStdDevOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"STDEV({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"STDEV({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"STDEV({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "STDEV() WAS INVALID";
        }
    }

    public class esStdDevpOver : esBaseOverClause
    {
        private esStdDevpOver() { }

        public esStdDevpOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"STDEVP({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"STDEVP({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"STDEVP({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "STDEVP() WAS INVALID";
        }
    }

    public class esVarOver : esBaseOverClause
    {
        private esVarOver() { }

        public esVarOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"VAR({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"VAR({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"VAR({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "VAR() WAS INVALID";
        }
    }

    public class esVarpOver : esBaseOverClause
    {
        private esVarpOver() { }

        public esVarpOver(esQueryItem columnExpression)
        {
            base._columnExpression = columnExpression;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"VARP({columnExpression}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {alias}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"VARP({columnExpression}) OVER(PARTITION BY {partionby}) AS {alias}";
            else if (orderBy != null && alias != null)
                return $"VARP({columnExpression}) OVER(ORDER BY {orderBy}) AS {alias}";
            else
                return "VARP() WAS INVALID";
        }
    }
}