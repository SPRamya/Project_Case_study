using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case_study_PayXpert.exception
{
    public class PayrollGenerationException : Exception
    {
        public PayrollGenerationException(string message)
            : base(message)
        {
        }
    }
}