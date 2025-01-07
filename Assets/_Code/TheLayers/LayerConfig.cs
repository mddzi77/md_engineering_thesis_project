using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLayers
{
    [CreateAssetMenu(fileName = "Layer", menuName = "TheLayers/Layer")]
    public class LayerConfig : ScriptableObject
    {
        [SerializeField] private string layerName;
        [SerializeField] private int order;
        [SerializeField] private Sprite sprite;
        [SerializeField] private Color color;

        public string LayerName => layerName;
        public int Order => order;
        public Sprite Sprite => sprite;
        public Color Color => color;
    }
}
