namespace EntitySpaces.OracleManagedClientProvider
{
    class Delimiters
    {
        private const string tableOpen = "\"";
        private const string tableClose = "\"";
        private const string columnOpen = "\"";
        private const string columnClose = "\"";
        private const string stringOpen = "\"";
        private const string stringClose = "\"";
        private const string aliasOpen = "'";
        private const string aliasClose = "'";
        private const string storedProcNameOpen = "\"";
        private const string storedProcNameClose = "\"";
        private const string param = ":";

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

        public static string AliasOpen
        {
            get { return aliasOpen; }
        }

        public static string AliasClose
        {
            get { return aliasClose; }
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
