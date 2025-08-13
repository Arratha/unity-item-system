#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utils.Singleton
{
    [DefaultExecutionOrder(-100)]
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T instance
        {
            get
            {
                if (!_isQuitting && _instance == null)
                {
                    var instanceContainer = new GameObject($"{typeof(T).Name}_Instance");
                    instanceContainer.AddComponent<T>();
                }

                return _instance;
            }
        }

        private static T _instance;

        private static bool _isQuitting;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError($"Instance of type {typeof(T)} already exists", gameObject);
                Destroy(this);
                return;
            }
            
            _instance = (T)this;
            DontDestroyOnLoad(this);

            HandleAwake();
        }

        private void OnDestroy()
        {
            if (!_isQuitting && Equals(_instance))
            {
                _isQuitting = true;
                Debug.LogError($"Singleton of type {typeof(T)} was destroyed during runtime. This may trigger unexpected behavior.");
            }

            HandleDestroy();
        }
        
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        protected virtual void HandleAwake()
        {
            
        }

        protected virtual void HandleDestroy()
        {
            
        }
    }
}