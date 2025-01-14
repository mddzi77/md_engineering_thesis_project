using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Level", menuName = "Game/Level")]
    public class Level : ScriptableObject
    {
        [SerializeField] private List<LevelData> levelDatas;
        
        public List<LevelData> LevelDatas => levelDatas;
    }
    
    [Serializable]
    public class LevelData
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