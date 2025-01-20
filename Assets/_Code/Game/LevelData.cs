using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private List<ComponentData> components;
        
        public List<ComponentData> Components => components;
    }
    
    [Serializable]
    public class ComponentData
    {
        public ComponentType type;
        public string pin1;
        public string pin2;
        public int W;
        public int L;
    }
    
    public enum ComponentType
    {
        NTransistor,
        PTransistor
    }
}