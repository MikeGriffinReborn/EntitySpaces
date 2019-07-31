using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EntitySpaces.QuerySandbox
{
    public class ExecutionResult
    {
        public IBindingList Collection;
        public string LastQuery;
        public string Exception;
    }
}
