using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.JwtTokenExceptions
{
    public class InvalidRefreshTokenException: Exception
    {
        public InvalidRefreshTokenException(string msg):base(msg)
        {
            
        }
    }
}
