using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw8
{
    /// <summary>
    /// Is thrown when no such role was found during new user registration
    /// </summary>
    public class UndefinedUserRoleException : Exception
    {
        public UndefinedUserRoleException()
        {
        }

        public UndefinedUserRoleException(string message)
            : base(message)
        {
        }

        public UndefinedUserRoleException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
