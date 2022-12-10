using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilfingerStructure.Core.RebarOverridingInView
{
    public class IncorrectViewException : Exception
    {
        public IncorrectViewException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
