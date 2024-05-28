using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLayers
{
    [CreateAssetMenu(fileName = "Layer", menuName = "TheLayers/Layer")]
    public class Layer : ScriptableObject
    {
        [SerializeField] private string layerName;
        [SerializeField] private int order;
        [SerializeField] private Sprite sprite;
        [SerializeField] private LayerOffset[] layerOffsets;

        public string LayerName => layerName;
        public int Order => order;
        public Sprite Sprite => sprite;
        public LayerOffset[] LayerOffsets => layerOffsets;
    }
    
    [Serializable]
    public struct LayerOffset
    {
        [SerializeField] public Layer layer;
        [SerializeField] public int offset;
    }
    
    public enum LayerCategory
    {
        Metal,
        Polycrystal,
    }
}
