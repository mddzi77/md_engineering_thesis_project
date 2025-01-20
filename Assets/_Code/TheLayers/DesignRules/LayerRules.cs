using System;
using UnityEngine;

namespace TheLayers.DesignRules
{
    [Serializable]
    public class LayerRules
    {
        [SerializeField] private int minimumWidth;
        [SerializeField] private int minimumSpacing;
        
        public int MinimumWidth => minimumWidth;
        public int MinimumSpacing => minimumSpacing;
    }
}