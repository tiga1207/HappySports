using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class CustomSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        [SerializeField] private bool isDontDestoryOnLoad = false;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
                if (isDontDestoryOnLoad)
                    DontDestroyOnLoad(_instance);
            }
        }
    }
}