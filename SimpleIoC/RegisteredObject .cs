using System;

namespace SimpleIoC
{
    internal class RegisteredObject : IRegisteredObject
    {
        public RegisteredObject(Type typeToResolve, Type concreteType, LifeCycle lifeCycle)
        {
            TypeToResolve = typeToResolve;
            ConcreteType = concreteType;
            LifeCycle = lifeCycle;
        }

        public Type TypeToResolve { get; protected set; }
        public Type ConcreteType { get; protected set; }
        public object Instance { get; protected set; }
        public LifeCycle LifeCycle { get; protected set; }

        public void CreateInstance(params object[] args)
        {
            this.Instance = Activator.CreateInstance(this.ConcreteType, args);
        }
    }
}
