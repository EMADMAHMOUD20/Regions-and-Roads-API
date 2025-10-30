using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions
{
    public class DublicateCountryException : Exception
    {
        public DublicateCountryException(string msg):base(msg) { }
    }
}
