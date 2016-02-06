using System;
using System.Collections.Generic;
using System.Text;

namespace Levolution.Core
{
    public class Class1
    {
#if Net35
        public string Message => ".Net Framework 3.5";
#else
        public string Message => ".Net Framework Pcl";
#endif
    }
}
