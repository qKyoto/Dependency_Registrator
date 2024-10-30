using System;
using UnityEngine;

namespace Code.New_Core_Architecture
{
    [DefaultExecutionOrder(-4000)]
    public abstract class ComponentRegistrator : DependencyRegistrator
    {
        protected void RegisterMonoServiceFromInstance<T>(object instance) where T : IDependency
        {
            DependencyTransmitter.Instance.RegisterService(instance, typeof(T));
            UpdateRegistredTypes(new []{typeof(T)});
        }
        
        protected void RegisterMonoServiceFromInstance(object instance, params Type[] contractTypes)
        {
            DependencyTransmitter.Instance.RegisterService(instance, contractTypes);
            UpdateRegistredTypes(contractTypes);
        }

        protected void RegisterMonoServiceFromPrefab<T>(Component prefab) where T : IDependency
        {
            Component component = Instantiate(prefab, transform);
            DependencyTransmitter.Instance.RegisterService(component, typeof(T));
            UpdateRegistredTypes(new []{typeof(T)});
        }
        
        protected void RegisterMonoServiceFromPrefab(Component prefab, params Type[] contractTypes)
        {
            Component component = Instantiate(prefab, transform);
            DependencyTransmitter.Instance.RegisterService(component, contractTypes);
            UpdateRegistredTypes(contractTypes);
        }

        protected void RegisterNonMonoService<T>(object instance) where T : IDependency
        {
            DependencyTransmitter.Instance.RegisterService(instance, typeof(T));
            UpdateRegistredTypes(new []{typeof(T)});
        }
        
        protected void RegisterNonMonoService(object instance, params Type[] contractTypes)
        {
            DependencyTransmitter.Instance.RegisterService(instance, contractTypes);
            UpdateRegistredTypes(contractTypes);
        }

        protected sealed override void InitializeServices()
        {
            DependencyTransmitter.Instance.InitializeService(RegistredTypes.ToArray());
        }
        
        protected sealed override void UnregisterDependency()
        {
            DependencyTransmitter.Instance.UnregisterService(RegistredTypes.ToArray());
        }

        private void UpdateRegistredTypes(Type[] types)
        {
            foreach (Type type in types)
                RegistredTypes.Add(type);
        }
    }
}
