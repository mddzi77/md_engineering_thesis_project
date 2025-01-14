using UnityEngine;

namespace UI
{
    public class DynamicCanvasElement : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;
        
        private Transform _worldSpaceElement;
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var position = _worldSpaceElement.position;
            position.x += 0.8f;
            position.y += 0.8f;
            var screenPos = _camera.WorldToScreenPoint(position);
            rect.position = screenPos;
        }
        
        public void SetWorldSpaceElement(Transform worldSpaceElement)
        {
            _worldSpaceElement = worldSpaceElement;
        }
    }
}