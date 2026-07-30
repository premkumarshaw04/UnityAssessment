using System;
using System.Collections.Generic;
using UnityEngine;
using EndlessRunner.Interfaces;

namespace EndlessRunner.Architecture
{
    /// <summary>
    /// Centralized service locator providing decoupled access to system services.
    /// Eliminates direct singleton references across domain boundaries.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, IGameService> Services = new Dictionary<Type, IGameService>();

        /// <summary>
        /// Registers a new game service instance.
        /// </summary>
        public static void RegisterService<T>(T service) where T : class, IGameService
        {
            Type type = typeof(T);
            if (Services.ContainsKey(type))
            {
                Debug.LogWarning($"[ServiceLocator] Service of type {type.Name} is already registered. Overwriting.");
                Services[type].Deinitialize();
                Services[type] = service;
            }
            else
            {
                Services.Add(type, service);
            }
        }

        /// <summary>
        /// Retrieves a registered service instance.
        /// </summary>
        public static T GetService<T>() where T : class, IGameService
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out IGameService service))
            {
                return service as T;
            }

            Debug.LogError($"[ServiceLocator] Service of type {type.Name} was not found.");
            return null;
        }

        /// <summary>
        /// Unregisters and cleans up a service instance.
        /// </summary>
        public static void UnregisterService<T>() where T : class, IGameService
        {
            Type type = typeof(T);
            if (Services.TryGetValue(type, out IGameService service))
            {
                service.Deinitialize();
                Services.Remove(type);
            }
        }

        /// <summary>
        /// Clears all registered services. Used during application shutdown.
        /// </summary>
        public static void ClearAll()
        {
            foreach (var service in Services.Values)
            {
                service.Deinitialize();
            }
            Services.Clear();
        }
    }
}