using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorBlast
{
    /// <summary>
    /// This class is used as Service Locator. 
    /// </summary>
    public class ServiceManager
    {
        // Make ServiceManager as Singleton
        private static readonly Lazy<ServiceManager> lazy = new Lazy<ServiceManager>(() => new ServiceManager());

        public static ServiceManager Instance { get { return lazy.Value; } }

        private readonly Dictionary<Type, IService> mServices;

        private ServiceManager()
        {
            mServices = new Dictionary<Type, IService>();
        }

        public void Init() 
        {
            foreach(var service in mServices.Values) 
            {
                service.Init();
            }
        }
        
        public void Release()
        {
            foreach (var service in mServices.Values)
            {
                service.Release();
            }
        }

        public void Add<T>(IService service)
        {
            if(!mServices.ContainsKey(typeof(T))) 
            {
                mServices.Add(typeof(T), service);
            }
            else 
            {
                Debug.LogError("This service is already added: " + typeof(T));
            }
            
        }

        public T Get<T>() 
        {
            if(mServices.TryGetValue(typeof(T), out var service)) 
            {
                return (T)service;
            }
            else 
            {
                Debug.LogError("Can't find Service: " + typeof(T));
                return default(T);
            }
        }
    }
}
