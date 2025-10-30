using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.RoadsExceptions
{
    public class NoRoadsFoundException : Exception
    {
        public NoRoadsFoundException(string msg):base(msg) { }
    }
}
