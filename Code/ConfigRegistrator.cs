using System;
using UnityEngine;

namespace Code.New_Core_Architecture
{
    [DefaultExecutionOrder(-5000)]
    public abstract class ConfigRegistrator : DependencyRegistrator
    {
        protected void RegisterConfig<T>(object config) where T : IConfig
        {
            DependencyTransmitter.Instance.RegisterConfig(typeof(T), config);
            RegistredTypes.Add(typeof(T));
        }

        protected sealed override void UnregisterDependency()
        {
            foreach (Type registredType in RegistredTypes)
                DependencyTransmitter.Instance.UnregisterConfig(registredType);
        }
    }
}
