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
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EntitySpaces.Interfaces
{
    public class esTracePacket
    {
        private string traceChannel;
        private string applicationName;
        private long? duration;
        private string objectType;
        private int? transactionId;
        private int? threadId;
        private string stackTrace;
        private string sql;
        private esParameters sqlParameters = new esParameters();

        public string TraceChannel
        {
            get { return traceChannel; }
            set { traceChannel = value; }
        }

        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public long? Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public string ObjectType 
        {
            get { return objectType; }
            set { objectType = value; } 
        }

        public int? TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }

        public int? ThreadId
        {
            get { return threadId; }
            set { threadId = value; }
        }

        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }

        public string Sql
        {
            get { return sql; }
            set { sql = value; }
        }

        public esParameters SqlParameters
        {
            get { return sqlParameters; }
            set { sqlParameters = value; }
        }
    }
}
