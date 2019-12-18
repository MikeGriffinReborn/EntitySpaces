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

namespace EntitySpaces.SQLiteProvider
{
    class Delimiters
    {
        private const string tableOpen = "[";
        private const string tableClose = "]";
        private const string columnOpen = "[";
        private const string columnClose = "]";
        private const string stringOpen = "'";
        private const string stringClose = "'";
        private const string storedProcNameOpen = "[";
        private const string storedProcNameClose = "]";
        private const string param = "@";

        public static string TableOpen
        {
            get { return tableOpen; }
        }

        public static string TableClose
        {
            get { return tableClose; }
        }

        public static string ColumnOpen
        {
            get { return columnOpen; }
        }

        public static string ColumnClose
        {
            get { return columnClose; }
        }

        public static string StringOpen
        {
            get { return stringOpen; }
        }

        public static string StringClose
        {
            get { return stringClose; }
        }

        public static string StoredProcNameOpen
        {
            get { return storedProcNameOpen; }
        }

        public static string StoredProcNameClose
        {
            get { return storedProcNameClose; }
        }

        public static string Param
        {
            get { return param; }
        }

    }
}
