using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.RoadsExceptions
{
    public class DuplicatedRoadFoundException :Exception
    {
        public DuplicatedRoadFoundException(string msg):base(msg)
        {
            
        }
    }
}
