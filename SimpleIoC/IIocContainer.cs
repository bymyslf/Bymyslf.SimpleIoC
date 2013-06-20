using System;

namespace SimpleIoC
{
    public interface IIocContainer
    {
        IIocContainer RegisterType<TTypeToResolve, TConcrete>() where TConcrete : class;
        IIocContainer RegisterType<TTypeToResolve, TConcrete>(LifeCycle lifeCycle) where TConcrete : class;
        TTypeToResolve ResolveType<TTypeToResolve>();
        object ResolveType(Type typeToResolve);
        TTypeToResolve TryResolveType<TTypeToResolve>();
        object TryResolveType(Type typeToResolve);
    }
}
