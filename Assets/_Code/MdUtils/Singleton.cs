using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MdUtils
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance => _instance;
        
        private static T _instance;
        
        protected void Awake()
        {

            if (_instance != null && _instance != this as T)
                throw new Exception($"Singleton already exists! : {_instance.name}");
            _instance = this as T;
        }
    }
}
