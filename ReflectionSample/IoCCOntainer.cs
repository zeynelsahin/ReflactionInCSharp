using System.Reflection;
using Microsoft.VisualBasic;

namespace ReflectionSample;

public class IoCCOntainer
{
    private Dictionary<Type, Type> _map = new Dictionary<Type, Type>();
    private MethodInfo _resolvedMethod;
    
    public void Register<TContract,TImplementation>()
    {
        if (!_map.ContainsKey(typeof(TContract)))
        {
            _map.Add(typeof(TContract),typeof(TImplementation));
        }
    }
    public void Register(Type contract, Type implementation)
    {
        if (!_map.ContainsKey(contract))
        {
            _map.Add(contract,implementation);
        }
    }
    public TContract Resolve<TContract>()
    {
        if (typeof(TContract).IsGenericType&&_map.ContainsKey(typeof(TContract).GetGenericTypeDefinition()))
        {
            var openImplementation = _map[typeof(TContract).GetGenericTypeDefinition()];
            var closedImplementation = openImplementation.MakeGenericType(typeof(TContract).GenericTypeArguments);
            return Create<TContract>(closedImplementation);
        }
        if (!_map.ContainsKey(typeof(TContract)))
        {
            throw new ArgumentException($"No registration found for {typeof(TContract)}");
        }

        return Create<TContract>(_map[typeof(TContract)]);
    }

    private TContract Create<TContract>(Type implementationType)
    {
        if (_resolvedMethod==null)
        {
            _resolvedMethod = typeof(IoCCOntainer).GetMethod("Resolve");
        }
        var constructorParameters = implementationType.GetConstructors().OrderByDescending(c=>c.GetParameters().Length).First().GetParameters().Select(p =>
        {
            var genericResolveMethod = _resolvedMethod.MakeGenericMethod(p.ParameterType);
            return genericResolveMethod.Invoke(this, null);
        }).ToArray();
        
        return (TContract)Activator.CreateInstance(implementationType,constructorParameters);
    }
}