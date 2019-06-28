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

namespace EntitySpaces.DynamicQuery
{
    /// <summary>
    /// The base class for all of the explicit casting operators such as 
    /// <see cref="esByte"/>, <see cref="esChar"/>, <see cref="esDateTime"/>.
    /// </summary>
    public class esCast
    {
        internal esCast() { }

        internal esQueryItem item;
    }

    #region esBoolean
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esBoolean)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esBoolean : esCast
    {
        private esBoolean() { }

        internal esBoolean(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Boolean);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esBoolean cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esByte
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esByte)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esByte : esCast
    {
        private esByte() { }

        internal esByte(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Byte);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esByte cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esChar
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esChar)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esChar : esCast
    {
        private esChar() { }

        internal esChar(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Char);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esChar cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esDateTime
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esDateTime)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esDateTime : esCast
    {
        private esDateTime() { }

        internal esDateTime(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.DateTime);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esDateTime cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esDecimal
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esDecimal)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esDecimal : esCast
    {
        private esDecimal() { }

        internal esDecimal(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Decimal);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esDecimal cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esDouble
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esDouble)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esDouble : esCast
    {
        private esDouble() { }

        internal esDouble(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Double);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esDouble cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esGuid
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esGuid)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esGuid : esCast
    {
        private esGuid() { }

        internal esGuid(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Guid);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esGuid cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esInt16
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esInt16)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esInt16 : esCast
    {
        private esInt16() { }

        internal esInt16(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Int16);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esInt16 cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esInt32
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esInt32)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esInt32 : esCast
    {
        private esInt32() {}

        internal esInt32(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Int32);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esInt32 cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esInt64
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esInt64)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esInt64 : esCast
    {
        private esInt64() {}

        internal esInt64(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Int64);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esInt64 cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esSingle
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esSingle)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esSingle : esCast
    {
        private esSingle() {}

        internal esSingle(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.Single);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esSingle cast)
        {
            return cast.item;
        }
    }
    #endregion

    #region esString
    /// <summary>
    /// Used in the DynamicQuery syntax to cast like this: (esString)query.SomeProperty
    /// </summary>
    /// <remarks>
    /// VB.NET users will need to use esQueryItem.Cast.
    /// </remarks>
    /// <example>
    /// Here we are building a special column like this "Smith, John [24]" where 24 is
    /// his age. We use the (esString) cast operator so that the database system will
    /// convert it to a string.
    /// <code>
    /// EmployeeCollection coll = new EmployeeCollection(); 
    /// EmployeeQuery q = coll.Query; 
    /// 
    /// q.Select
    /// (
    ///     (
    ///         (q.LastName + ", " + q.FirstName).Trim() + " [" + (esString)q.Age + "]"
    ///     )
    ///     .ToUpper().As("FullName")
    /// ); 
    /// 
    /// if (coll.Query.Load())
    /// {
    ///     foreach (Employee emp in coll)
    ///     {
    ///         string fn = emp.GetColumn("FullName");
    ///     }
    /// }
    /// </code>
    /// </example>
    public class esString : esCast
    {
        internal esString(esQueryItem item)
        {
            base.item = item;
            item.Cast(esCastType.String);
        }

        /// <summary>
        /// This is called automatically for you when necessary.
        /// </summary>
        public static implicit operator esQueryItem(esString cast)
        {
            return cast.item;
        }
    }
    #endregion
}
