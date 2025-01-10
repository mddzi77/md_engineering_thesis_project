using System.Collections;
using System.Collections.Generic;
using TheLayers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LayersPalette
{
    public class LayerSelectButton : MonoBehaviour
    {
        [SerializeField] private LayerConfig _layerConfig;
        [SerializeField] private Image selectedImage;

        private Button _button;
        private Image _image;

        public void Init(LayerConfig layerConfig)
        {
            _layerConfig = layerConfig;
            LayersManager.LayerChanged += OnLayerChanged;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            _image = GetComponent<Image>();
            _image.sprite = _layerConfig.Sprite;
            _image.color = _layerConfig.Color;
        }

        private void OnClick()
        {
            LayersManager.Instance.SetCurrentLayer(_layerConfig);
            Debug.Log($"Layer changed to {_layerConfig.LayerName}");
        }
        
        private void OnLayerChanged(LayerConfig layerConfig)
        {
            if (layerConfig == _layerConfig)
            {
                selectedImage.enabled = true;
            }
            else
            {
                selectedImage.enabled = false;
            }
        }
    }
}
