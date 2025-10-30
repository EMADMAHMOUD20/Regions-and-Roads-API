using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.RoadsExceptions
{
    public class CannotDeleteRoadException:Exception
    {
        public CannotDeleteRoadException(string s):base(s)
        {
            
        }
    }
}
