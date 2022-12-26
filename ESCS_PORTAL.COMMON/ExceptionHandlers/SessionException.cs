using System;
using System.Collections.Generic;
using System.Text;

namespace ESCS_PORTAL.COMMON.ExceptionHandlers
{
    public class SessionException : Exception
    {
        public SessionException(string message):base(message)
        {

        }
    }
}
