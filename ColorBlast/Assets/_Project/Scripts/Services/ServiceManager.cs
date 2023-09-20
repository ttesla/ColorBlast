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
            AddServices();

            foreach(var service in mServices.Values) 
            {
                service.Init();
            }
        }

        private void AddServices() 
        {
            // Add all services here. We may move these to a seperate method and add in Main also. 
            mServices.Add(typeof(ISaveService), new DummySaveService());
            mServices.Add(typeof(IAudioService), new AudioService());
        }

        public void Release()
        {
            foreach (var service in mServices.Values)
            {
                service.Release();
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

        #region HandyShortcuts

        public IAudioService GetAudioService() 
        {
            return Get<IAudioService>();
        }

        public ISaveService GetSaveService() 
        {
            return Get<ISaveService>();
        }

        #endregion
    }
}
