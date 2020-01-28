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
using System.Diagnostics;

namespace EntitySpaces.DynamicQuery
{

    public class esRowNumberOver : esBaseOverClause
    {
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"ROW_NUMBER() OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"ROW_NUMBER() OVER(PARTITION BY {partionby}) AS {aliasOpen}{alias}{aliasClose}";
            else if (orderBy != null && alias != null)
                return $"ROW_NUMBER() OVER(ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else
                return "ROW_NUMBER() WAS INVALID";
        }
    }

    public class esPercentRankOver : esBaseOverClause
    {
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"PERCENT_RANK() OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"PERCENT_RANK() OVER(PARTITION BY {partionby}) AS {aliasOpen}{alias}{aliasClose}";
            else if (orderBy != null && alias != null)
                return $"PERCENT_RANK() OVER(ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else
                return "PERCENT_RANK() WAS INVALID";
        }
    }

    public class esRankOver : esBaseOverClause
    {
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"RANK() OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"RANK() OVER(PARTITION BY {partionby}) AS {aliasOpen}{alias}{aliasClose}";
            else if (orderBy != null && alias != null)
                return $"RANK() OVER(ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else
                return "RANK() WAS INVALID";
        }
    }

    public class esDenseRankOver : esBaseOverClause
    {
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"DENSE_RANK() OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"DENSE_RANK() OVER(PARTITION BY {partionby}) AS {aliasOpen}{alias}{aliasClose}";
            else if (orderBy != null && alias != null)
                return $"DENSE_RANK() OVER(ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else
                return "DENSE_RANK() WAS INVALID";
        }
    }

    public class esNtileOver : esBaseOverClause
    {
        private long nTile;

        private esNtileOver() { }

        public esNtileOver(int nTile)
        {
            this.nTile = nTile;
        }

        public esNtileOver(long nTile)
        {
            this.nTile = nTile;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            if (partionby != null && orderBy != null && alias != null)
                return $"NTILE({nTile}) OVER(PARTITION BY {partionby} ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else if (partionby != null && orderBy == null && alias != null)
                return $"NTILE({nTile}) OVER(PARTITION BY {partionby}) AS {aliasOpen}{alias}{aliasClose}";
            else if (orderBy != null && alias != null)
                return $"NTILE({nTile}) OVER(ORDER BY {orderBy}) AS {aliasOpen}{alias}{aliasClose}";
            else
                return "NTILE() WAS INVALID";
        }
    }
}