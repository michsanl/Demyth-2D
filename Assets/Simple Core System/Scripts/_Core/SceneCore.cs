using Core.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-2)]
    public class SceneCore : MonoBehaviour
    {
        public Dictionary<EnumId, ISceneService> Services => _cachedServices;

        [Header("Scene Services")]
        [SerializeField]
        private List<GameObject> servicesGameObjects = new List<GameObject>();

        private Dictionary<EnumId, ISceneService> _cachedServices = new Dictionary<EnumId, ISceneService>();
        
        private void Awake()
        {
            var servicesOnScene = GameObject.FindGameObjectsWithTag(SceneServiceProvider.SCENE_SERVICE_TAG);

            foreach (var serviceGO in servicesOnScene)
            {
                if (servicesGameObjects.Contains(serviceGO) == false)
                    servicesGameObjects.Add(serviceGO);
            }

            foreach (var serviceGO in servicesGameObjects)
            {
                var service = serviceGO.GetComponent<ISceneService>();
                if (service == null)
                    continue;
                if (service.ServiceId != null)
                    _cachedServices.TryAdd(service.ServiceId, service);

                service.Core = this;
            }
        }

        private IEnumerator Start()
        {
            foreach (var service in _cachedServices.Values)
            {
                yield return service.StartService();
            }
        }
    }

    public static class SceneServiceProvider
    {
        public const string SCENE_CORE_TAG = "SceneCore";        
        public const string SCENE_SERVICE_TAG = "SceneService";

        private static SceneCore _cachedCore;

        public static SceneCore GetSceneCore()
        {
            if (_cachedCore != null)
                return _cachedCore;

            var gameObject = GameObject.FindGameObjectWithTag(SCENE_CORE_TAG);
            _cachedCore = gameObject.GetComponent<SceneCore>();

            if (_cachedCore == null)
                Debug.LogWarning("No scene core in this scene");

            return _cachedCore;
        }

        public static T GetService<T>(EnumId serviceId) where T : ISceneService
        {
            var core = GetSceneCore();

            if (core.Services.TryGetValue(serviceId, out var service) == false)
            {
                Debug.LogWarning($"No services found in the scene");
            }

            return (T)service;
        }

        public static T GetService<T>() where T : ISceneService
        {
            var core = GetSceneCore();
            foreach (var service in core.Services.Values)
            {
                if (service.GetType() == typeof(T))
                    return (T)service;
            }

            Debug.LogWarning($"No services found in the scene");
            return default(T);
        }
    }    
}