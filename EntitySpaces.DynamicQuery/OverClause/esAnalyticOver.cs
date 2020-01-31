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
    public class esLagOver : esBaseOverClause
    {
        private long offset;

        private esLagOver() { }

        public esLagOver(esQueryItem expression, int offset = 1, esQueryItem theDefault = null)
        {
            base._columnExpression = expression;
            this.offset = offset;
        }

        // MEG - DEFAULT
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"LAG({columnExpression},{offset},0) OVER(";

            if (partionby != null) over += $" PARTITION BY {partionby}";
            if (orderBy != null) over += $" ORDER BY {orderBy}";
            if (this.WindowFrame.Length > 0) over += $" {this.WindowFrame}";

            if (alias != null)
                over += $" ) AS {aliasOpen}{alias}{aliasClose}";
            else
                over += " )";

            return over;
        }
    }

    public class esLeadOver : esBaseOverClause
    {
        private long offset;

        private esLeadOver() { }

        public esLeadOver(esQueryItem expression, int offset = 1, esQueryItem theDefault = null)
        {
            base._columnExpression = expression;
            this.offset = offset;
        }

        // MEG - DEFAULT
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"LEAD({columnExpression},{offset},0) OVER(";

            if (partionby != null) over += $" PARTITION BY {partionby}";
            if (orderBy != null) over += $" ORDER BY {orderBy}";
            if (this.WindowFrame.Length > 0) over += $" {this.WindowFrame}";

            if (alias != null)
                over += $" ) AS {aliasOpen}{alias}{aliasClose}";
            else
                over += " )";

            return over;
        }
    }

    public class esCumeDistOver : esBaseOverClause
    {
        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"CUME_DIST() OVER(";

            if (partionby != null) over += $" PARTITION BY {partionby}";
            if (orderBy != null) over += $" ORDER BY {orderBy}";
            if (this.WindowFrame.Length > 0) over += $" {this.WindowFrame}";

            if (alias != null)
                over += $" ) AS {aliasOpen}{alias}{aliasClose}";
            else
                over += " )";

            return over;
        }
    }

    public class esFirstValueOver : esBaseOverClause
    {
        private esFirstValueOver() { }

        public esFirstValueOver(esQueryItem expression)
        {
            base._columnExpression = expression;
            ((IOverClause)this).IsWindowFrameSupported = true;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"FIRST_VALUE({columnExpression}) OVER(";

            if (partionby != null) over += $" PARTITION BY {partionby}";
            if (orderBy != null) over += $" ORDER BY {orderBy}";
            if (this.WindowFrame.Length > 0) over += $" {this.WindowFrame}";

            if (alias != null)
                over += $" ) AS {aliasOpen}{alias}{aliasClose}";
            else
                over += " )";

            return over;
        }
    }

    public class esLastValueOver : esBaseOverClause
    {
        private esLastValueOver() { }

        public esLastValueOver(esQueryItem expression)
        {
            base._columnExpression = expression;
            ((IOverClause)this).IsWindowFrameSupported = true;
        }

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"LAST_VALUE({columnExpression}) OVER(";

            if (partionby != null) over += $" PARTITION BY {partionby}";
            if (orderBy != null) over += $" ORDER BY {orderBy}";
            if (this.WindowFrame.Length > 0) over += $" {this.WindowFrame}";

            if (alias != null)
                over += $" ) AS {aliasOpen}{alias}{aliasClose}";
            else
                over += " )";

            return over;
        }
    }

    public class esPercentileContOver : esBaseOverClause
    {
        private string literal;

        private esPercentileContOver() { }

        #region Constructors

        public esPercentileContOver(string literal)
        {
            this.literal = literal;
        }

        public esPercentileContOver(long literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileContOver(int literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileContOver(short literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileContOver(float literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileContOver(decimal literal)
        {
            this.literal = $"{literal}";
        }

        #endregion

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"PERCENTILE_CONT({literal}) WITHIN GROUP(ORDER BY {orderBy})";

            if (partionby != null) over += $" OVER(PARTITION BY {partionby})";

            if (alias != null)
                over += $" AS {aliasOpen}{alias}{aliasClose}";

            return over;
        }
    }

    public class esPercentileDiscOver : esBaseOverClause
    {
        private string literal;

        private esPercentileDiscOver() { }

        #region Constructors

        public esPercentileDiscOver(string literal)
        {
            this.literal = literal;
        }

        public esPercentileDiscOver(long literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileDiscOver(int literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileDiscOver(short literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileDiscOver(float literal)
        {
            this.literal = $"{literal}";
        }

        public esPercentileDiscOver(decimal literal)
        {
            this.literal = $"{literal}";
        }

        #endregion

        protected override string CreateOverStatement(string columnExpression, string partionby, string orderBy, string alias, string aliasOpen, string aliasClose)
        {
            string over = $"PERCENTILE_DISC({literal}) WITHIN GROUP(ORDER BY {orderBy})";

            if (partionby != null) over += $" OVER(PARTITION BY {partionby})";

            if (alias != null)
                over += $" AS {aliasOpen}{alias}{aliasClose}";

            return over;
        }
    }
}