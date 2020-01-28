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

/*
 * 
    { Rows.UnBoundedPreceding | Rows(x).Preceding | Rows.CurrentRow }.As()
      |
    { Rows.
      { Between.UnboundedPreceding | { Between(x).Preceding | Between(x).Following } | Between.CurrentRow     } 
      { Between.UnboundedFollowing | { And(x).Preceding     | And(x).Following     } | And.Between.CurrentRow }
      .As()
    }

*/
namespace EntitySpaces.DynamicQuery
{

    public class esWfRows
    {
        internal esBaseOverClause overClause;

        private esWfRows() { }

        internal esWfRows(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame = "ROWS";
        }

        internal esWfRows(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame = $"ROWS {count}";
        }

        public esWfUnBoundedPrecedingAs UnBoundedPreceding => new esWfUnBoundedPrecedingAs(this.overClause);

        public esWfCurrentRowAs CurrentRow => new esWfCurrentRowAs(this.overClause);

        public esWfPrecedingAs Preceding => new esWfPrecedingAs(this.overClause);

        public esWfBetween Between => new esWfBetween(this.overClause);

        internal esWfBetweenX BetweenX(esBaseOverClause overClause, int count) => new esWfBetweenX(this.overClause, count);
    }

    public class esWfRange
    {
        internal esBaseOverClause overClause;

        private esWfRange() { }

        internal esWfRange(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame = "RANGE";
        }

        internal esWfRange(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame = $"RANGE {count}";
        }

        public esWfUnBoundedPrecedingAs UnBoundedPreceding => new esWfUnBoundedPrecedingAs(this.overClause);

        public esWfCurrentRowAs CurrentRow => new esWfCurrentRowAs(this.overClause);

        public esWfPrecedingAs Preceding => new esWfPrecedingAs(this.overClause);

        public esWfBetween Between => new esWfBetween(this.overClause);

        internal esWfBetweenX BetweenX(esBaseOverClause overClause, int count) => new esWfBetweenX(this.overClause, count);
    }

    public static class esWfExtensions
    {
        public static esWfBetweenX Between(this esWfRows obj, int count)
        {
            return obj.BetweenX(obj.overClause, count);
        }

        public static esWfBetweenX Between(this esWfRange obj, int count)
        {
            return obj.BetweenX(obj.overClause, count);
        }

        public static esWfAndX And(this esWfUnBoundedPrecedingAnd obj, int count)
        {
            return obj.AndX(count);
        }

        public static esWfAndX And(this esWfPrecedingAnd obj, int count)
        {
            return obj.AndX(count);
        }

        public static esWfAndX And(this esWfFollowingAnd obj, int count)
        {
            return obj.AndX(count);
        }

        public static esWfAndX And(this esWfCurrentRowAnd obj, int count)
        {
            return obj.AndX(count);
        }
    }

    public class esWfBetween
    {
        internal esBaseOverClause overClause;

        private esWfBetween() { }

        internal esWfBetween(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " BETWEEN";
        }

        internal esWfBetween(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame += $" BETWEEN {count}";
        }

        public esWfUnBoundedPrecedingAnd UnBoundedPreceding => new esWfUnBoundedPrecedingAnd(this.overClause);

        public esWfCurrentRowAnd CurrentRow => new esWfCurrentRowAnd(this.overClause);
    }

    public class esWfBetweenX
    {
        internal esBaseOverClause overClause;

        private esWfBetweenX() { }

        internal esWfBetweenX(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " BETWEEN";
        }

        internal esWfBetweenX(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame += $" BETWEEN {count}";
        }

        public esWfPrecedingAnd Preceding => new esWfPrecedingAnd(this.overClause);

        public esWfFollowingAnd Following => new esWfFollowingAnd(this.overClause);
    }

    public class esWfAnd
    {
        internal esBaseOverClause overClause;

        private esWfAnd() { }

        internal esWfAnd(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " AND";
        }

        internal esWfAnd(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame += $" AND {count}";
        }

        public esWfUnBoundedPrecedingAs UnBoundedPreceding => new esWfUnBoundedPrecedingAs(this.overClause);

        public esWfCurrentRowAs CurrentRow => new esWfCurrentRowAs(this.overClause);
    }

    public class esWfAndX
    {
        internal esBaseOverClause overClause;

        private esWfAndX() { }

        internal esWfAndX(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " AND";
        }

        internal esWfAndX(esBaseOverClause parent, int count)
        {
            this.overClause = parent;
            this.overClause.windowFrame += $" AND {count}";
        }

        public esWfPrecedingAs Preceding => new esWfPrecedingAs(this.overClause);

        public esWfFollowingAs Following => new esWfFollowingAs(this.overClause);
    }

    public class esWfUnBoundedPrecedingAs
    {
        internal esBaseOverClause overClause;

        private esWfUnBoundedPrecedingAs() { }

        internal esWfUnBoundedPrecedingAs(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " UNBOUNDED PRECEDING";
        }

        public IOverClause As(string alias, out esQueryItem aliasedItem)
        {
            aliasedItem = this.overClause.CreateAliasOutVar(alias);
            return this.overClause;
        }
    }

    public class esWfUnBoundedPrecedingAnd
    {
        internal esBaseOverClause overClause;

        private esWfUnBoundedPrecedingAnd() { }

        internal esWfUnBoundedPrecedingAnd(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " UNBOUNDED PRECEDING";
        }

        public esWfAnd And => new esWfAnd(this.overClause);

        internal esWfAndX AndX(int count) => new esWfAndX(this.overClause, count);
    }

    public class esWfPrecedingAs
    {
        internal esBaseOverClause overClause;

        private esWfPrecedingAs() { }

        internal esWfPrecedingAs(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " PRECEDING";
        }

        public IOverClause As(string alias, out esQueryItem aliasedItem)
        {
            aliasedItem = this.overClause.CreateAliasOutVar(alias);
            return this.overClause;
        }
    }

    public class esWfPrecedingAnd
    {
        internal esBaseOverClause overClause;

        private esWfPrecedingAnd() { }

        internal esWfPrecedingAnd(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " PRECEDING";
        }

        public esWfAnd And => new esWfAnd(this.overClause);

        internal esWfAndX AndX(int count) => new esWfAndX(this.overClause, count);
    }

    public class esWfFollowingAs
    {
        internal esBaseOverClause overClause;

        private esWfFollowingAs() { }

        internal esWfFollowingAs(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " FOLLOWING";
        }

        public IOverClause As(string alias)
        {
            this.overClause._alias = alias;
            return this.overClause;
        }

        public IOverClause As(string alias, out esQueryItem aliasedItem)
        {
            aliasedItem = this.overClause.CreateAliasOutVar(alias);
            return this.overClause;
        }
    }

    public class esWfFollowingAnd
    {
        internal esBaseOverClause overClause;

        private esWfFollowingAnd() { }

        internal esWfFollowingAnd(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " FOLLOWING";
        }

        public esWfAnd And => new esWfAnd(this.overClause);

        internal esWfAndX AndX(int count) => new esWfAndX(this.overClause, count);
    }

    public class esWfCurrentRowAs
    {
        internal esBaseOverClause overClause;

        private esWfCurrentRowAs() { }

        internal esWfCurrentRowAs(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " CURRENT ROW";
        }

        public IOverClause As(string alias)
        {
            this.overClause._alias = alias;
            return this.overClause;
        }

        public IOverClause As(string alias, out esQueryItem aliasedItem)
        {
            aliasedItem = this.overClause.CreateAliasOutVar(alias);
            return this.overClause;
        }
    }

    public class esWfCurrentRowAnd
    {
        internal esBaseOverClause overClause;

        private esWfCurrentRowAnd() { }

        internal esWfCurrentRowAnd(esBaseOverClause parent)
        {
            this.overClause = parent;
            this.overClause.windowFrame += " CURRENT ROW";
        }

        public esWfAnd And => new esWfAnd(this.overClause);

        internal esWfAndX AndX(int count) => new esWfAndX(this.overClause, count);
    }

}