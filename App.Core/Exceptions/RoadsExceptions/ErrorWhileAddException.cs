using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.RoadsExceptions
{
    public class ErrorWhileAddException:Exception
    {
        public ErrorWhileAddException(string msg):base(msg)
        {
            
        }
    }
}
