using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.ExceptionHandlers
{
    public class BusinessException : Exception
    {
        public BusinessException(string message):base(message)
        {

        }
    }
}
