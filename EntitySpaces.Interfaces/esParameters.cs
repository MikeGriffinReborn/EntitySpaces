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
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace EntitySpaces.Interfaces
{
    /// <summary>
    /// This class is used to pass parameters to the providers. 
    /// </summary>
    /// <example>
    /// The example below calls a stored procedure that concatenates an Employees first name and last name
    /// and returns them as an output parameter. Notice that when the FullName parameter is added it is
    /// necessary to provide the direction, type and size. This is ONLY necessary when adding output parameters
    /// and you are encouraged to only provide the name/value for all other parameters. This will make your code
    /// much more provider independent.
    /// <code>
    /// public partial class Employees : esEmployees
    /// {
    ///		public string GetFullName(int employeeID)
    ///		{
    ///			esParameters parms = new esParameters();
    ///
    ///			parms.Add("EmployeeID", employeeID);
    ///			parms.Add("FullName", esParameterDirection.Output, DbType.String, 40);
    ///
    ///			this.ExecuteNonQuery(esQueryType.StoredProcedure, "proc_GetEmployeeFullName", parms);
    ///
    ///			return parms["FullName"].Value as string;
    ///		}
    ///}
    /// </code>
    /// <seealso cref="esParameter"/>, <seealso cref="esDataProvider"/>, <seealso cref="IDataProvider"/>
    /// </example> 
    [Serializable] 
    public class esParameters : IEnumerable 
    {
        public esParameters()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter">An already created esParameter. This is mostly used internally.</param>
        /// <returns>The same parameter passed in</returns>
        public esParameter Add(esParameter parameter)
        {
            this.parameters.Add(parameter);
            this.hash[parameter.Name] = parameter;
            return parameter;
        }

        /// <summary>
        /// This is the preferred overload for adding parameters.
        /// </summary>
        /// <remarks>
        /// Do not add prefixes such as @ or ? or : as the providers append the proper prefix. This allows you 
        /// to pass parameters in a provider independent fashion. All parameters added via this method are input
        /// parameters.
        /// </remarks>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        /// <returns>The newly created parameter</returns>
        public esParameter Add(string name, object value)
        {
            return this.Add(new esParameter(name, value));
        }

        /// <summary>
        /// If you need to add a parameter with an esParameterDirection other than Input use this method.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="direction">The parameter direction</param>
        /// <returns>The newly created parameter</returns>
        public esParameter Add(string name, esParameterDirection direction)
        {
            return this.Add(new esParameter(name, null, direction));
        }

        /// <summary>
        /// This should ONLY be used for parameters other than input parameters.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="direction">The parameter direction</param>
        /// <returns>The newly created parameter</returns>
        public esParameter Add(string name, object value, esParameterDirection direction)
        {
            return this.Add(new esParameter(name, value, direction));
        }

        /// <summary>
        /// This should ONLY be used for parameters other than input parameters.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="direction">The parameter direction</param>
        /// <param name="type">The System.Data.DbType of the parameter</param>
        /// <param name="size">The size. For strings the length, for most others 0 can be passed in</param>
        /// <returns>The newly created parameter</returns>
        public esParameter Add(string name, esParameterDirection direction, DbType type, int size)
        {
            return this.Add(new esParameter(name, direction, type, size));
        }

        /// <summary>
        /// This should ONLY be used for parameters other than input parameters.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="direction">The parameter direction</param>
        /// <param name="type">The System.Data.DbType of the parameter</param>
        /// <param name="size">The size. For strings the length, for most others 0 can be passed in</param>
        /// <returns>The newly created parameter</returns>
        public esParameter Add(string name, object value, esParameterDirection direction, DbType type, int size)
        {
            return this.Add(new esParameter(name, value, direction, type, size));
        }

        /// <summary>
        /// The number of parameters in the collection
        /// </summary>
        public int Count
        {
            get { return parameters.Count; }
        }

        /// <summary>
        /// Use this method to fetch a given esParameter by name. There should be no prefix such as @, ? or : in the name.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <returns>The desired parameter or null/Nothing if not found</returns>
        public esParameter this[string name]
        {
            get
            {
                esParameter param = null;

                if (this.hash.ContainsKey(name))
                {
                    param = this.hash[name];
                }

                return param;
            }
        }

        /// <summary>
        /// Used Internally to merge parameters returned by the provider back into the original collection.
        /// </summary>
        /// <param name="parms"></param>
        internal void Merge(esParameters parms)
        {
            foreach(esParameter esParam in parms)
            {
                esParameter esOriginalParam = this.hash[esParam.Name];

                esOriginalParam.Value = esParam.Value;
            }
        }

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return this.parameters.GetEnumerator();
        }

        #endregion

        private List<esParameter> parameters = new List<esParameter>();

        [NonSerialized]
        private Dictionary<System.String, esParameter> hash = new Dictionary<string, esParameter>();
    }

    /// <summary>
    /// Represents a parameter typically passed into a stored procedure.
    /// </summary>
    /// <remarks>
    /// Normally you don't directly create esParameter objects, instead you call the Add method on the esParameters Collection.
    /// It is important not to add prefixes to the parameter names such as @, ?, : as the providers will do this.
    /// </remarks>
    /// <seealso cref="esParameters"/>
    [Serializable] 
    public class esParameter
    {
        public esParameter()
        {

        }

        /// <summary>
        /// This is the provider independent way to use parameters, you can do so with the other methods as well but 
        /// this is the preferred constructor for all input parameters.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        public esParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// Use this constructor for non-input parameters only.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="direction">The direction of the parameter</param>
        public esParameter(string name, object value, esParameterDirection direction)
        {
            this.name = name;
            this.value = value;
            this.direction = direction;
        }

        /// <summary>
        /// Use this constructor for non-input parameters only.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="direction">The parameter direction</param>
        /// <param name="type">The System.Data.DbType of the parameter</param>
        /// <param name="size">The size. For strings the length, for most others 0 can be passed in</param>
        public esParameter(string name, esParameterDirection direction, DbType type, int size)
        {
            this.name = name;
            this.direction = direction;
            this.dbType = type;
            this.size = size;
        }

        /// <summary>
        /// Use this constructor for non-input parameters only.
        /// </summary>
        /// <param name="name">The name of the parameter without a prefix, for example, "EmployeeID" not "@EmployeeID".</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="direction">The parameter direction</param>
        /// <param name="type">The System.Data.DbType of the parameter</param>
        /// <param name="size">The size. For strings the length, for most others 0 can be passed in</param>
        /// <returns>The newly created parameter</returns>
        public esParameter(string name, object value, esParameterDirection direction, DbType type, int size)
        {
            this.name = name;
            this.value = value;
            this.direction = direction;
            this.dbType = type;
            this.size = size;
        }

        /// <summary>
        /// The Name of the parameter
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The Value of the parameter. After the call to the EntitySpaces DataProvider is made any output or return
        /// parameters will have their "Value" set accordingly.
        /// </summary>
        public object Value
        {
            get { return this.value;  }
            set { this.value = value; }
        }

        /// <summary>
        /// The parameter direction. The default is Input.
        /// </summary>
        public esParameterDirection Direction
        {
            get { return direction;  }
            set { direction = value; }
        }

        /// <summary>
        /// See System.Data.DbType, this should only be used for parameters other than input parameters.
        /// </summary>
        public DbType DbType
        {
            get { return dbType;  }
            set { dbType = value; }
        }

        /// <summary>
        /// Used for things such as SqlGeometry/SqlGeography. You use the SQL column type, ie, "geometry" or geography" 
        /// </summary>
        public string UdtTypeName
        {
            get { return udtTypeName; }
            set { udtTypeName = value; }
        }

        /// <summary>
        /// This should only be used for parameters other than input parameters. For strings this is the length, for most
        /// other types set to 0.
        /// </summary>
        public int Size
        {
            get { return size;}
            set { size = value; }
        }

        /// <summary>
        /// This should only be used for parameters other than input parameters.
        /// </summary>
        /// <remarks>
        /// "Obsolete" for Oracle.
        /// Not supported for Access.
        /// </remarks>
        public byte Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// This should only be used for parameters other than input parameters.
        /// </summary>
        /// <remarks>
        /// "Obsolete" for Oracle.
        /// Not supported for Access.
        /// </remarks>
        public byte Precision
        {
            get { return precision; }
            set { precision = value; }
        }

        private string name;
        private object value;
        private esParameterDirection direction;
        private DbType dbType;
        private int size;
        private byte scale;
        private byte precision;
        private string udtTypeName;
    }

    /// <summary>
    /// Used to determine the direction of the esParameter class. The default is Input
    /// </summary>
    [Serializable] 
    public enum esParameterDirection
    {
        /// <summary>
        /// The parameter is an input parameter. 
        /// </summary>
        Input = 0,
        /// <summary>
        /// The parameter is capable of both input and output. 
        /// </summary>
        InputOutput,
        /// <summary>
        /// The parameter is an output parameter. 
        /// </summary>
        Output,
        /// <summary>
        /// The parameter represents a return value from an operation such as a stored procedure, built-in function, or user-defined function. 
        /// </summary>
        ReturnValue
    }
}
