using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace EntitySpaces.ProfilerApplication
{
    public class esTracePacket
    {
        private long packetOrder;
        private string syntax;
        private string traceChannel;
        private string applicationName;
        private long? duration;
        private long? ticks;
        private string objectType;
        private int? transactionId;
        private int? threadId;
        private string action;
        private string callStack;
        private string exception;
        private string sql;
        private esParameters sqlParameters = new esParameters();

        public long PacketOrder
        {
            get { return packetOrder; }
            set { packetOrder = value; }
        }

        public string Syntax
        {
            get { return syntax; }
            set { syntax = value; }
        }

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

        public long? Ticks
        {
            get { return ticks; }
            set { ticks = value; }
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

        public string Action
        {
            get { return action; }
            set { action = value; }
        }

        public string CallStack
        {
            get { return callStack; }
            set { callStack = value; }
        }

        public string Exception
        {
            get { return exception; }
            set { exception = value; }
        }

        public bool HasException
        {
            get { return !String.IsNullOrEmpty(exception); }
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
