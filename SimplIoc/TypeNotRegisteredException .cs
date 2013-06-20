using System;

namespace SimplIoc
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message)
            : base(message)
        { }
    }
}
