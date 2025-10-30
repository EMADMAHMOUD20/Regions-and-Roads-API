using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions
{
    public class UnableToAddToDbException:Exception
    {
        public UnableToAddToDbException(string msg):base(msg) { }
    }
}
