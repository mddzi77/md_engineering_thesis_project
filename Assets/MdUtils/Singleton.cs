using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MdUtils
{
    public class Singleton : MonoBehaviour
    {
        public static Singleton Instance => _instance;
        
        private static Singleton _instance;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }
    }
}
