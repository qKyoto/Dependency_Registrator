using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Penquin_Helper.Code.New_Core_Architecture
{
    public static class Container
    {
        private static readonly Dictionary<Type, object> SERVICES = new();
        private static readonly Dictionary<Type, object> CONFIGS = new();

        private static DependencyTransmitter _dependencyTransmitter;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _dependencyTransmitter = new DependencyTransmitter(
                RegisterService,
                UnregisterServices,
                InitializeServices,
                RegisterConfig,
                UnregisterConfig);
        }

        public static T Resolve<T>() where T : IDependency
        {
            Type type = typeof(T);
            
            if (SERVICES.TryGetValue(type, out object service))
                return (T)service;
            
            throw new NullReferenceException("Service not registered -> " + type);
        }

        public static T GetConfig<T>() where T : IConfig
        {
            Type type = typeof(T);

            if (CONFIGS.TryGetValue(type, out object config))
                return (T)config;
            
            throw new NullReferenceException("Config not registered -> " + type);
        }

        private static void RegisterService(Type serviceType, object serviceInstance)
        {
            SERVICES.TryAdd(serviceType, serviceInstance);
        }

        private static void InitializeServices(Type[] types)
        {
            List<object> intances = new();
            
            foreach (Type type in types)
            {
                if (SERVICES.TryGetValue(type, out object service)) 
                    intances.Add(service);
            }

            foreach (object instance in intances.Distinct())
            {
                if (instance is IInitializable initializeble)
                    initializeble.Initialize();
            }

            intances.Clear();
        }

        private static void UnregisterServices(Type[] types)
        {
            List<object> intances = new();
            
            foreach (Type type in types)
            {
                if (!SERVICES.TryGetValue(type, out object service)) 
                    continue;
                
                intances.Add(service);
                SERVICES.Remove(type);
            }

            foreach (object instance in intances.Distinct())
            {
                if (instance is IDisposable disposable)
                    disposable.Dispose();
            }

            intances.Clear();
        }

        private static void RegisterConfig(Type configType, object configInstance)
        {
            CONFIGS.TryAdd(configType, configInstance);
        }

        private static void UnregisterConfig(Type configType)
        {
            if (CONFIGS.ContainsKey(configType))
                CONFIGS.Remove(configType);
        }
    }
}
