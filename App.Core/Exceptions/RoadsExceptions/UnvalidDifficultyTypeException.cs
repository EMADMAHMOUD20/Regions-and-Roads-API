using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Exceptions.RoadsExceptions
{
    public class UnvalidDifficultyTypeException:Exception
    {
        public UnvalidDifficultyTypeException(string msg):base(msg)
        {
            
        }
    }
}
