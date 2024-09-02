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

        private Button _button;
        private Image _image;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
            _image = GetComponent<Image>();
            _image.sprite = _layerConfig.Sprite;
        }

        private void OnClick()
        {
            LayersManager.Instance.SetCurrentLayer(_layerConfig);
            Debug.Log($"Layer changed to {_layerConfig.LayerName}");
        }
    }
}
