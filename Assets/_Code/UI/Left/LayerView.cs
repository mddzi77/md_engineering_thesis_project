using System;
using TheLayers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Left
{
    public class LayerView : MonoBehaviour
    {
        [SerializeField] private Image layerSprite;
        [SerializeField] private TextMeshProUGUI layerName;
        [SerializeField] private Button visibilityButton;
        [SerializeField] private TextMeshProUGUI visibilityText;
        
        public event Action<LayerConfig, bool> OnVisibility;

        private LayerConfig _layerConfig;
        private bool _isVisible = true;
        
        private void Start()
        {
            visibilityButton.onClick.AddListener(OnButtonClick);
        }
        
        public void SetLayer(LayerConfig layerConfig)
        {
            _layerConfig = layerConfig;
            layerSprite.sprite = layerConfig.Sprite;
            layerSprite.color = layerConfig.Color;
            layerName.text = layerConfig.LayerName;
        }
        
        private void OnButtonClick()
        {
            _isVisible = !_isVisible;
            OnVisibility?.Invoke(_layerConfig, _isVisible);
            SetVisibilityButton();
        }

        private void SetVisibilityButton()
        {
            visibilityText.text = _isVisible ? "On" : "Off";
        }
    }
}