using System;

namespace SimpleIoC
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message)
            : base(message)
        { }
    }
}
