using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.New_Core_Architecture
{
    public abstract class DependencyRegistrator : MonoBehaviour
    {
        [SerializeField] private ScopeType _scopeType;
        
        protected readonly List<Type> RegistredTypes = new(); 
            
        private void Awake()
        {
            if (_scopeType == ScopeType.Global)
                DontDestroyOnLoad(this);
            
            RegisterDependency();
        }

        private void Start()
        {
            InitializeServices();
        }

        private void OnDestroy()
        {
            UnregisterDependency();
        }
        
        protected virtual void RegisterDependency() {}
        
        protected virtual void  UnregisterDependency() {}
        
        protected virtual void InitializeServices() {}
        
        private enum ScopeType
        {
            Scene,
            Global
        }
    }
}
