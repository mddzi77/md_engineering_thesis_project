using System;
using TheLayers;
using TMPro;
using UI.LayersPalette;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Right
{
    public class LayerView : MonoBehaviour
    {
        [SerializeField] private LayerSelectButton layerSelectButton;
        [SerializeField] private TextMeshProUGUI layerName;
        [SerializeField] private VisibilityButtonView visibilityButton;
        
        public event Action<LayerConfig, bool> OnVisibility;

        private LayerConfig _layerConfig;
        private bool _isVisible = true;
        
        private void Start()
        {
            visibilityButton.AddListener(OnButtonClick);
        }
        
        public void SetLayer(LayerConfig layerConfig)
        {
            layerSelectButton.Init(layerConfig);
            _layerConfig = layerConfig;
            layerName.text = layerConfig.LayerName;
        }
        
        private void OnButtonClick()
        {
            _isVisible = !_isVisible;
            OnVisibility?.Invoke(_layerConfig, _isVisible);
            visibilityButton.ToggleIcon(_isVisible);
        }
    }
}