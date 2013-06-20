using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using SimpleIoC.ConfigFile;

namespace SimpleIoC
{
    public class IocContainer : IIocContainer
    {
        protected readonly IList<IRegisteredObject> registeredObjects;

        public IocContainer()
            : this(new List<IRegisteredObject>())
        { }

        public IocContainer(IList<IRegisteredObject> registeredObjectsCollection)
        {
            this.registeredObjects = registeredObjectsCollection;
        }

        public virtual IIocContainer RegisterType<TTypeToResolve, TConcrete>()
            where TConcrete : class
        {
            return this.RegisterType<TTypeToResolve, TConcrete>(LifeCycle.Transient);
        }

        public virtual IIocContainer RegisterType<TTypeToResolve, TConcrete>(LifeCycle lifeCycle)
            where TConcrete : class
        {
            registeredObjects.Add(new RegisteredObject(typeof(TTypeToResolve), typeof(TConcrete), lifeCycle));
            return this;
        }

        public virtual TTypeToResolve ResolveType<TTypeToResolve>()
        {
            return (TTypeToResolve)this.ResolveObject(typeof(TTypeToResolve));
        }

        public virtual object ResolveType(Type typeToResolve)
        {
            return this.ResolveObject(typeToResolve);
        }

        public virtual TTypeToResolve TryResolveType<TTypeToResolve>()
        {
            if (!this.RegisteredObjectExists(typeof(TTypeToResolve)))
            {
                return default(TTypeToResolve);
            }
            return (TTypeToResolve)this.ResolveObject(typeof(TTypeToResolve));
        }

        public virtual object TryResolveType(Type typeToResolve)
        {
            if (!this.RegisteredObjectExists(typeToResolve)) 
            {
                return null;
            }
            return this.ResolveObject(typeToResolve);
        }

        public virtual IIocContainer RegisterConfigObjects<TRegisteredObject>()
            where TRegisteredObject : IRegisteredObject
        {
            IocContainerSettingsSection config = ConfigurationManager.GetSection("iocContainerSettings") as IocContainerSettingsSection;
            if (config != null)
            {
                Type typeToResolve, concreteType;
                LifeCycle lifeCycle;
                IRegisteredObject registeredObject;
                foreach (RegisteredObjectElement obj in config.RegisteredObjects)
                {
                    typeToResolve = Type.GetType(obj.TypeToResolve);
                    concreteType = Type.GetType(obj.ConcreteType);
                    if (typeToResolve == null || concreteType == null)
                    {
                        continue;
                    }

                    lifeCycle = (LifeCycle)Enum.Parse(typeof(LifeCycle), obj.LifeCycle);
                    registeredObject = (TRegisteredObject)Activator.CreateInstance(typeof(TRegisteredObject), new object[] { typeToResolve, concreteType, lifeCycle });
                    if (registeredObject != null)
                    {
                        registeredObjects.Add(registeredObject);
                    }
                }
            }
            return this;
        }

        public virtual IIocContainer RegisterConfigObjects()
        {
            return this.RegisterConfigObjects<RegisteredObject>();
        }

        protected bool RegisteredObjectExists(Type typeToResolve)
        {
            return this.registeredObjects.Any
                (
                    x => x.TypeToResolve == typeToResolve
                );
        }

        protected virtual object ResolveObject(Type typeToResolve)
        {
            var registeredObject = registeredObjects.FirstOrDefault
                (
                    obj => obj.TypeToResolve == typeToResolve
                );
            if (registeredObject == null)
            {
                throw new TypeNotRegisteredException(string.Format("The type {0} has not been registered", typeToResolve.Name));
            }
            return this.GetInstance(registeredObject);
        }

        protected virtual object GetInstance(IRegisteredObject registeredObject)
        {
            if (registeredObject.Instance == null || registeredObject.LifeCycle == LifeCycle.Transient)
            {
                var parameters = this.ResolveConstructorParameters(registeredObject);
                registeredObject.CreateInstance(parameters.ToArray());
            }
            return registeredObject.Instance;
        }

        protected virtual IEnumerable<object> ResolveConstructorParameters(IRegisteredObject registeredObject)
        {
            var constructorInfo = registeredObject.ConcreteType.GetConstructors().First();
            foreach (var parameter in constructorInfo.GetParameters())
            {
                yield return this.ResolveObject(parameter.ParameterType);
            }
        }
    }
}
