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

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// The query type
    /// </summary>
    public enum esQueryType
    {
        /// <summary>
        /// Unassigned
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// TableDirect
        /// </summary>
        TableDirect,
        /// <summary>
        /// StoredProcedure
        /// </summary>
        StoredProcedure,
        /// <summary>
        /// Database Function
        /// </summary>
        Function,
        /// <summary>
        /// DynamicQuery
        /// </summary>
        DynamicQuery,
        /// <summary>
        /// DynamicQuery returns only the SQL, doesn't execute the query
        /// </summary>
        DynamicQueryParseOnly,
        /// <summary>
        /// Raw Text
        /// </summary>
        Text,
        /// <summary>
        /// Use a LINQ Query (IQueryable) as the source
        /// </summary>
        IQueryable,
        /// <summary>
        /// ManyToMany - Used internally by the EntitySpaces Hierarchical code.
        /// </summary>
        ManyToMany
    }

    /// <summary>
    /// The type of SubOperator such as ToLower
    /// </summary>
    /// Aggregate and GROUP BY Feature Support by DBMS:
    /// <code>
    ///                 MS    MS    My    SQ    Vista Fire  Post
    /// Feature         SQL   Acces SQL   Lite  DB    bird  gre   Oracle  Ads
    /// --------------- ----- ----- ----- ----- ----- ----- ----- ------ -----
    /// Avg              Y     Y     Y     Y     Y     Y     Y     Y       Y
    /// Count            Y     Y     Y     Y     Y     Y     Y     Y       Y
    /// Max              Y     Y     Y     Y     Y     Y     Y     Y       Y
    /// Min              Y     Y     Y     Y     Y     Y     Y     Y       Y
    /// Sum              Y     Y     Y     Y     Y     Y     Y     Y       Y
    /// StdDev           Y     Y     Y     -     Y     -     Y    (4)      -
    /// Var              Y     Y     Y     -     -     -     Y     Y       -
    /// Aggregate in
    ///   ORDER BY       Y     Y    (1)    Y    (3)    Y     Y     Y       Y
    ///   GROUP BY       -     -     -     Y    (3)    Y     Y     Y       -
    /// WITH ROLLUP      Y     -    (2)    -     Y     -     -     Y       -
    /// COUNT(DISTINCT)  Y     -     Y     -     Y     Y     Y     Y       Y
    /// 
    /// Notes:
    ///   (1) - MySQL - accepts an aggregate's alias in an
    ///         ORDER BY clause.
    ///   (2) - MySQL - WITH ROLLUP and ORDER BY are mutually
    ///         exclusive
    ///   (3) - VistaDB - will not ORDER BY or GROUP BY 'COUNT(*)' 
    ///         the rest works fine.   
    ///   (4) - Uses TRUNC(STDDEV(column),10) to avoid overflow errors
    ///   
    /// </code>
#if !SILVERLIGHT    
    [Serializable]
#endif
    public enum esQuerySubOperatorType
    {
        /// <summary>
        /// Unassigned
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// Convert to upper case
        /// </summary>
        ToUpper,
        /// <summary>
        /// Convert to lower case
        /// </summary>
        ToLower,
        /// <summary>
        /// Left trim any leading spaces
        /// </summary>
        LTrim,
        /// <summary>
        /// Right trin any trailing spaces
        /// </summary>
        RTrim,
        /// <summary>
        /// Trim both leading and trailing spaces
        /// </summary>
        Trim,
        /// <summary>
        /// Return a sub-string 
        /// </summary>
        SubString,
        /// <summary>
        ///  Return the first non null evaluating expresssion
        /// </summary>
        Coalesce,
        /// <summary>
        ///  Returns only the date of a datetime type
        /// </summary>
        Date,
        /// <summary>
        /// If the string contains multi-byte characters, and the proper collation is being used, 
        /// LENGTH returns the number of characters, not the number of bytes. If string is of 
        /// BINARY data type, the LENGTH function behaves as BYTE_LENGTH.
        /// </summary>
        Length,
        /// <summary>
        /// Rounds the numeric-expression to the specified integer-expression amount of places after the decimal point.
        /// </summary>
        Round,
        /// <summary>
        /// Returns the value of part of a datetime value.
        /// </summary>
        DatePart,
        /// <summary>
        /// Average
        /// </summary>
        Avg,
        /// <summary>
        /// Count
        /// </summary>
        Count,
        /// <summary>
        /// Maximum
        /// </summary>
        Max,
        /// <summary>
        /// Minimum
        /// </summary>
        Min,
        /// <summary>
        /// Standard Deviation
        /// </summary>
        StdDev,
        /// <summary>
        /// Variance
        /// </summary>
        Var,
        /// <summary>
        /// Sum
        /// </summary>
        Sum,
        /// <summary>
        /// 
        /// </summary>
        Cast
    }

    /// <summary>
    /// The direction used by DynamicQuery.AddOrderBy
    /// </summary>
    public enum esOrderByDirection
    {
        /// <summary>
        /// Unassigned
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// Ascending
        /// </summary>
        Ascending,
        /// <summary>
        /// Descending
        /// </summary>
        Descending
    };

    /// <summary>
    /// The type of comparison in a Where clause
    /// </summary>
#if !SILVERLIGHT    
    [Serializable]
#endif
    public enum esComparisonOperand
    {
        /// <summary>
        /// Unassigned
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// Equal Comparison
        /// </summary>
        Equal = 1,
        /// <summary>
        /// Not Equal Comparison
        /// </summary>
        NotEqual,
        /// <summary>
        /// Greater Than Comparison
        /// </summary>
        GreaterThan,
        /// <summary>
        /// Greater Than or Equal Comparison
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// Less Than Comparison
        /// </summary>
        LessThan,
        /// <summary>
        /// Less Than or Equal Comparison
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// Like Comparison, "%s%" does it have an 's' in it? "s%" does it begin with 's'?
        /// </summary>
        Like,
        /// <summary>
        /// Is the value null in the database
        /// </summary>
        IsNull,
        /// <summary>
        /// Is the value non-null in the database
        /// </summary>
        IsNotNull,
        /// <summary>
        /// Is the value between two parameters? 
        /// Note that Between can be for other data types than just dates.
        /// </summary>
        Between,
        /// <summary>
        /// Is the value in a list, ie, "4,5,6,7,8"
        /// </summary>
        In,
        /// <summary>
        /// NOT in a list, ie not in, "4,5,6,7,8"
        /// </summary>
        NotIn,
        /// <summary>
        /// Not Like Comparison, "%s%", anything that does not it have an 's' in it.
        /// </summary>
        NotLike,
        /// <summary>
        /// SQL Server only -
        /// Contains is used to search columns containing
        /// character-based data types for precise or
        /// fuzzy (less precise) matches to single words and phrases.
        /// The column must have a Full Text index.
        /// </summary>
        Contains,
        /// <summary>
        /// A subquery is being used in an Exists command
        /// </summary>
        Exists,
        /// <summary>
        /// A subquery is being used in an Not Exists command
        /// </summary>
        NotExists
    };

    /// <summary>
    /// The conjunction used between WhereParameters
    /// </summary>
    public enum esConjunction
    {
        /// <summary>
        /// WhereParameters are used via the default passed into DynamicQuery.Load.
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// WhereParameters are joined via "And"
        /// </summary>
        And,
        /// <summary>
        /// WhereParameters are joined via "Or"
        /// </summary>
        Or,
        /// <summary>
        /// WhereParameters are joined via "And Not"
        /// </summary>
        AndNot,
        /// <summary>
        /// WhereParameters are joined via "Or Not"
        /// </summary>
        OrNot
    };

    /// <summary>
    /// The Parenthesis used in queries, '(' and ')'
    /// </summary>
    public enum esParenthesis
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// WhereParameters need an open paren '('
        /// </summary>
        Open,
        /// <summary>
        /// WhereParameters need a closing paren ')'
        /// </summary>
        Close
    };

    /// <summary>
    /// ANY, ALL, and SOME are SubQuery qualifiers.
    /// <remarks>
    /// SubQuery qualifiers precede the SubQuery they apply to. For most databases, ANY and SOME are synonymous. 
    /// Usually, if you use an operator (= | &lt;> | != | > | >= | !> | &lt; | &lt;= | !&lt;) in a Where clause against a SubQuery, 
    /// then the SubQuery must return a single value. By applying a qualifier to the SubQuery, you can use operators against SubQueries
    /// that return multiple results.
    /// </remarks>
    /// </summary>
    /// <example>
    /// Notice, below, that the ALL qualifier is set to true for the SubQuery with "cq.es.All = true". 
    /// This query searches for the DateAdded for Customers whose Manager = 3.
    /// <code>
    /// // DateAdded for Customers whose Manager = 3
    /// CustomerQuery cq = new CustomerQuery("c");
    /// cq.es.All = true;  // &lt;== This will set it to esSubquerySearchCondition.ALL
    /// cq.Select(cq.DateAdded);
    /// cq.Where(cq.Manager == 3);
    /// 
    /// // OrderID and CustID where the OrderDate is 
    /// // less than all of the dates in the CustomerQuery above.
    /// OrderQuery oq = new OrderQuery("o");
    /// oq.Select(
    ///     oq.OrderID,
    ///    oq.CustID
    /// );
    /// oq.Where(oq.OrderDate &lt; cq);
    ///
    /// OrderCollection collection = new OrderCollection();
    /// collection.Load(oq);
    /// </code>
    /// </example>
    public enum esSubquerySearchCondition
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// scalar_expression { = | &lt;> | != | > | >= | !> | &lt; | &lt;= | !&lt; } ALL ( subquery )
        /// </summary>
        All,
        /// <summary>
        /// scalar_expression { = | &lt;> | != | > | >= | !> | &lt; | &lt;= | !&lt; }  ANY ( subquery ) 
        /// </summary>
        Any,
        /// <summary>
        /// scalar_expression { = | &lt;> | != | > | >= | !> | &lt; | &lt;= | !&lt; }  Some ( subquery )
        /// </summary>
        Some
    }

    /// <summary>
    /// The Type of Join
    /// </summary>
    public enum esJoinType
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// INNER JOIN
        /// </summary>
        InnerJoin,
        /// <summary>
        /// LEFT OUTER JOIN
        /// </summary>
        LeftJoin,
        /// <summary>
        /// RIGHT OUTER JOIN
        /// </summary>
        RightJoin,
        /// <summary>
        /// FULL OUTER JOIN
        /// </summary>
        FullJoin,
        /// <summary>
        /// CROSS JOIN
        /// </summary>
        CrossJoin
    };

    /// <summary>
    /// The type of Combination
    /// </summary>
    public enum esSetOperationType
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,

        /// <summary>
        /// UNION
        /// </summary>
        Union,

        /// <summary>
        /// UNION ALL
        /// </summary>
        UnionAll,

        /// <summary>
        /// INTERSECT
        /// </summary>
        Intersect,

        /// <summary>
        /// EXCEPT
        /// </summary>
        Except
    }

    /// <summary>
    /// Used to Track arithmetic expression in the DynamicQuery API
    /// </summary>
    public enum esArithmeticOperator
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,
        /// <summary>
        /// Addition
        /// </summary>
        Add,
        /// <summary>
        /// Subtraction
        /// </summary>
        Subtract,
        /// <summary>
        /// Multiplication
        /// </summary>
        Multiply,
        /// <summary>
        /// Division
        /// </summary>
        Divide,
        /// <summary>
        /// Returns the remainder of one number divided by another. 
        /// </summary>
        Modulo
    };
 
    /// <summary>
    /// Used to track the type of Cast requested, set automatically by the implicit cast 
    /// operators such as (esString)query.Age. See also <see cref="esQueryItem.Cast"/>
    /// </summary>
    public enum esCastType
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,

        /// <summary>
        /// Self Explanatory
        /// </summary>
        Boolean,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Byte,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Char,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        DateTime,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Double,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Decimal,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Guid,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Int16,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Int32,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Int64,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        Single,
        /// <summary>
        /// Self Explanatory
        /// </summary>
        String
    }

    /// <summary>
    /// Used to track data types in various places within the EntitySpaces Architecture
    /// </summary>
    public enum esSystemType
    {
        /// <summary>
        /// Unassigned.
        /// </summary>
        Unassigned = 0,

        /// <summary>
        /// System.Boolean
        /// </summary>
        Boolean,
        /// <summary>
        /// System.Byte
        /// </summary>
        Byte,
        /// <summary>
        /// System.Char
        /// </summary>
        Char,
        /// <summary>
        /// System.DateTime
        /// </summary>
        DateTime,
        /// <summary>
        /// System.DateTimeOffset
        /// </summary>
        DateTimeOffset,
        /// <summary>
        /// System.Double
        /// </summary>
        Double,
        /// <summary>
        /// System.Decimal
        /// </summary>
        Decimal,
        /// <summary>
        /// System.Guid
        /// </summary>
        Guid,
        /// <summary>
        /// System.Int16
        /// </summary>
        Int16,
        /// <summary>
        /// System.Int32
        /// </summary>
        Int32,
        /// <summary>
        /// System.Int64
        /// </summary>
        Int64,
        /// <summary>
        /// System.Object
        /// </summary>
        Object,
        /// <summary>
        /// System.SByte
        /// </summary>
        SByte,
        /// <summary>
        /// System.Single
        /// </summary>
        Single,
        /// <summary>
        /// System.String
        /// </summary>
        String,
        /// <summary>
        /// System.TimeSpan
        /// </summary>
        TimeSpan,
        /// <summary>
        /// System.UInt16
        /// </summary>
        UInt16,
        /// <summary>
        /// System.UInt32
        /// </summary>
        UInt32,
        /// <summary>
        /// System.UInt64
        /// </summary>
        UInt64,

        // Arrays

        /// <summary>
        /// System.Boolean[]
        /// </summary>
        BooleanArray,
        /// <summary>
        /// System.Byte[]
        /// </summary>
        ByteArray,
        /// <summary>
        /// System.Char[]
        /// </summary>
        CharArray,
        /// <summary>
        /// System.DateTime[]
        /// </summary>
        DateTimeArray,
        /// <summary>
        /// System.Double[]
        /// </summary>
        DoubleArray,
        /// <summary>
        /// System.Decimal[]
        /// </summary>
        DecimalArray,
        /// <summary>
        /// System.Guid[]
        /// </summary>
        GuidArray,
        /// <summary>
        /// System.Int16[]
        /// </summary>
        Int16Array,
        /// <summary>
        /// System.Int32[]
        /// </summary>
        Int32Array,
        /// <summary>
        /// System.Int32[]
        /// </summary>
        Int64Array,
        /// <summary>
        /// System.Object[]
        /// </summary>
        ObjectArray,
        /// <summary>
        /// System.SByte[]
        /// </summary>
        SByteArray,
        /// <summary>
        /// System.Single[]
        /// </summary>
        SingleArray,
        /// <summary>
        /// System.String[]
        /// </summary>
        StringArray,
        /// <summary>
        /// System.TimeSpan[]
        /// </summary>
        TimeSpanArray,
        /// <summary>
        /// System.UInt16[]
        /// </summary>
        UInt16Array,
        /// <summary>
        /// System.UInt32[]
        /// </summary>
        UInt32Array,
        /// <summary>
        /// System.UInt64[]
        /// </summary>
        UInt64Array
    };
}
